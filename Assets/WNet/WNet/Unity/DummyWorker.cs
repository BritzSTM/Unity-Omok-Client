using System;
using WNet.Core;
using UnityEngine;
using System.Threading.Tasks;

namespace WNet.Unity
{
    internal class DummyWorker : IWorkerTask
    {
        private DateTime _prevTime;

        public void OnChangedSchedule(IWorkerTask.ScheduleState state)
        {
            WNet.Core.Logger.WNet.InfoFormat("OnChangedSchedule");
        }

        public async Task Execute(IWorkerThreadScheduler scheduler)
        {
            await Task.Yield();
            var nowTime = DateTime.Now;
            //Debug.Log($"DummyWoker execute interval : {(nowTime - _prevTime).TotalSeconds}");
            _prevTime = nowTime;
        }
    }
}