using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

namespace WNet.Core
{

    internal interface IWorkerTask
    {
        enum ScheduleState
        {
            Added,
            Removed
        }

        void Execute();
        void OnChangedSchedule(ScheduleState state);
    }

    internal class WorkerThread
    {
        private const int kMS = 1000;   //millisecond

        private int _syncTickRate; // Hz... sleep time -> 1000/SyncTickRate
        private int _targetSleepTime;

        private bool _quitWorker;
        private Thread _rawWorkerThread;
        private List<IWorkerTask> _workerTasks;
        private ConcurrentBag<IWorkerTask> _addWorkerBag;
        private ConcurrentBag<IWorkerTask> _removeWorkerBag;
        private bool _changedWorkerList;
        private Action _stopedAction;

        public WorkerThread(int syncTickRate = 60, Action stopedAction = null)
        {
            _rawWorkerThread = new Thread(WorkerLoop);

            _workerTasks = new List<IWorkerTask>();
            _addWorkerBag = new ConcurrentBag<IWorkerTask>();
            _removeWorkerBag = new ConcurrentBag<IWorkerTask>();

            _stopedAction = stopedAction;
        }

        public void Start() => _rawWorkerThread.Start();
        public void Stop() => _quitWorker = true;
        public ThreadState State => _rawWorkerThread.ThreadState;

        private void WorkerLoop()
        {
            while (!_quitWorker)
            {
                // 엄격하게 while문 내부에서의 객체복제, branch가 발생하지 않도록
                while (!_quitWorker)
                {
                    var taskStartTime = DateTime.Now;

                    for (int i = 0; i < _workerTasks.Count; ++i)
                    {
                        _workerTasks[i].Execute();
                    }

                    var taskEndTime = DateTime.Now;

                    // 1초에 동기화 하는 횟수를 맞추기 위해 적응형 시간계산
                    var diffMS = _targetSleepTime - (taskEndTime - taskStartTime).Milliseconds;
                    Thread.Sleep(diffMS);
                }

                if (!_changedWorkerList)
                    continue;

                // 새롭게 worker들이 있다면 추가 및 시간 계산 및 이벤트
                UpdateTaskList();
            }

            _stopedAction?.Invoke();
        }

        private void UpdateTaskList()
        {
            _changedWorkerList = false;
        }

        public void Add(IWorkerTask task)
        {
            _addWorkerBag.Add(task);
            _changedWorkerList = true;
        }

        public void Remove(IWorkerTask task)
        {
            _removeWorkerBag.Add(task);
            _changedWorkerList = true;
        }
    }
}