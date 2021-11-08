using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Debug = System.Diagnostics.Debug;

namespace WNet.Core
{
    internal interface IWorkerThreadScheduler
    {
        void AddTask(IWorkerTask task);
        void RemoveTask(IWorkerTask task);
    }

    internal interface IWorkerTask
    {
        enum ScheduleState
        {
            Added,
            Removed
        }

        Task Execute(IWorkerThreadScheduler scheduler);
        void OnChangedSchedule(ScheduleState state);
    }

    internal class WorkerThread : IWorkerThreadScheduler
    {
        private const long kMsTick = 10000;  // timespan 1 tick = 100ns  10000tick = 1ms
        private const long kMs = kMsTick * 1000;
        
        private readonly int _syncTickRate;  // Hz
        private readonly TimeSpan _targetSleepTime;

        private bool _quitWorker;
        private Thread _rawWorkerThread;
        private List<IWorkerTask> _workerTasks;
        private Task[] _asyncTasks;
        private bool _taskExecuting;
        private ConcurrentBag<IWorkerTask> _addWorkerBag;
        private ConcurrentBag<IWorkerTask> _removeWorkerBag;
        private bool _changedWorkerList;
        private Action _stopedAction;

        public WorkerThread(int syncTickRate = 60, Action[] stopedActions = null)
        {
            Debug.Assert(syncTickRate > 0);

            _syncTickRate = syncTickRate;
            _targetSleepTime = new TimeSpan(kMs / _syncTickRate);
            
            _rawWorkerThread = new Thread(WorkerLoop);
            _workerTasks = new List<IWorkerTask>();
            _asyncTasks = new Task[0];
            _addWorkerBag = new ConcurrentBag<IWorkerTask>();
            _removeWorkerBag = new ConcurrentBag<IWorkerTask>();

            if (stopedActions == null)
                return;

            foreach (var stopedAction in stopedActions)
            {
                _stopedAction += stopedAction;
            }
        }

        public void Start() => _rawWorkerThread.Start();
        public void Stop() => _quitWorker = true;
        public ThreadState State => _rawWorkerThread.ThreadState;

        private void WorkerLoop()
        {
            // 최대 내부에서의 객체복제, branch가 발생하지 않도록 할 것
            Logger.Core.InfoFormat("Started WorkerThread. Update sync hz : {0}, Target sleep time {1, 6}",
                _syncTickRate, _targetSleepTime.ToString());

            DateTime taskStartTime, taskEndTime;
            taskStartTime = taskEndTime = DateTime.Now;
            while (!_quitWorker)
            {
                do
                {
                    AdaptiveSleep(taskStartTime, taskEndTime);

                    taskStartTime = DateTime.Now;
                    _taskExecuting = true;

                    // 반드시 모든 Task 종료 후 이 스레드 context로 복귀해야 함
                    for (int i = 0; i < _workerTasks.Count; ++i)
                    {
                        _asyncTasks[i] = _workerTasks[i].Execute(this);
                    }

                    Task.WaitAll(_asyncTasks);

                    _taskExecuting = false;
                    taskEndTime = DateTime.Now;
                } while (!_quitWorker && !_changedWorkerList);

                if (!_changedWorkerList)
                    continue;

                UpdateTaskList();
                taskEndTime = DateTime.Now; // 새로운 작업 종료시간으로 갱신
            }

            _stopedAction?.Invoke();

            Logger.Core.InfoFormat("Stopped WorkerThread");
        }

        private void AdaptiveSleep(DateTime startTime, DateTime endTime)
        {
            var diffSpan = _targetSleepTime - (endTime - startTime);

            if(diffSpan.Ticks > 0)
                Thread.Sleep(diffSpan);
        }

        private void UpdateTaskList()
        {
            // 이 함수가 호출된 시점에서는 반드시 WorkerTasks를 실행 중이면 안된다.
            Debug.Assert(!_taskExecuting);

            IWorkerTask task;
            while (_addWorkerBag.TryTake(out task))
            {
                _workerTasks.Add(task);
                task.OnChangedSchedule(IWorkerTask.ScheduleState.Added);
            }

            while (_removeWorkerBag.TryTake(out task))
            {
                _workerTasks.Remove(task);
                task.OnChangedSchedule(IWorkerTask.ScheduleState.Removed);
            }

            if(_asyncTasks.Length != _workerTasks.Count)
                _asyncTasks = new Task[_workerTasks.Count];

            _changedWorkerList = false;
            Logger.Core.InfoFormat("Updated WorkerThread TaskList");
        }

        /// <summary> worker thread schduler에 task를 추가합니다. 단 즉시 추가된다는 보장은 없습니다. </summary>
        public void AddTask(IWorkerTask task)
        {
            Debug.Assert(task != null);

            _addWorkerBag.Add(task);
            _changedWorkerList = true;
        }

        /// <summary> worker thread schduler에서 task를 제거합니다. 단 즉시 제거된다는 보장은 없습니다. </summary>
        public void RemoveTask(IWorkerTask task)
        {
            Debug.Assert(task != null);

            _removeWorkerBag.Add(task);
            _changedWorkerList = true;
        }
    }
}