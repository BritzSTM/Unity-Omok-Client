using System;
using WNet.Core;
using UnityEngine;

namespace WNet.Unity
{

    internal class DummyWorker : IWorkerTask
    {
        private DateTime _prevTime;
        public void Execute()
        {
            var nowTime = DateTime.Now;
            Debug.Log($"DummyWoker execute interval : {(nowTime - _prevTime).TotalSeconds}");
            _prevTime = nowTime;
        }

        public void Execute(IWorkerThreadScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public void OnChangedSchedule(IWorkerTask.ScheduleState state)
        {
            throw new NotImplementedException();
        }
    }
}