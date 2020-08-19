using iosha.WorkLogger.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace iosha.WorkLogger
{
    public class WorkTimer
    {
        private const int AFK_MAX_TIME_MINUTE = 1;

        private Hooker _hooker;

        private double _baseWorkTimeMilliseconds = 0;
        public double TotalWorkTimeMilliseconds => _baseWorkTimeMilliseconds + _workTimer.ElapsedMilliseconds;

        private Timer _afkTimer;

        private Stopwatch _workTimer;

        private bool _isInWork;

        private IWorkLogManager _workLogManager;

        public WorkTimer(
            Hooker hooker,
            IWorkLogManager workLogManager)
        {
            _hooker = hooker;
            _workLogManager = workLogManager;
            Initialize();

        }

        private void Initialize()
        {
            _workTimer = new Stopwatch();

            _afkTimer = new Timer();
            _afkTimer.Interval = AFK_MAX_TIME_MINUTE * 10000;
            _afkTimer.Elapsed += AfkDetected;

            _hooker.KeyboardWasPressedEvent += Act;

            _baseWorkTimeMilliseconds = _workLogManager.GetLogs(new GetLogsRequest()).WorkTimeMillisecond;

            StartWork();
        }

        public void StartWork()
        {
            _afkTimer.Start();
            _workTimer.Start();
        }

        private void AfkDetected(object sender, EventArgs args)
        {
            StopWork();
        }

        private void Act(object sender, EventArgs args)
        {
            if (!_isInWork)
            {
                StartWork();
                return;
            }
            SaveLog();

            _afkTimer.Stop();
            _afkTimer.Start();
        }

        private void SaveLog()
        {
            _workLogManager.SaveLog(new WorkLog()
            {
                Id = 1,
                Day = DateTime.Now,
                PCRunTimeMillisecond = 0,
                WorkTimeMillisecond = TotalWorkTimeMilliseconds
            });
        }

        private void StopWork()
        {
            _isInWork = false;
            _afkTimer.Stop();
            _baseWorkTimeMilliseconds += _workTimer.ElapsedMilliseconds;
            SaveLog();
            _workTimer.Reset();
            _workTimer.Stop();
        }

        public WorkLog WorkLog => _workLogManager.GetLogs(new GetLogsRequest()); 

        public void Stop()
        {
            StopWork();
          
        }

    }
}
