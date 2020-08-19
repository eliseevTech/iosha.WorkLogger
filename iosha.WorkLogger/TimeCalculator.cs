using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iosha.WorkLogger
{
    public class TimeCalculator
    {
        private const int AFK_MAX_TIME_MINUTE = 5;

        private Hooker _hooker;

        private DateTime _startTimeKeyPressed;
        private DateTime _lastTimeKeyPressed;

        private int _workTimeMinute = 0;
        public int WorkTimeMinute => _workTimeMinute;


        public TimeCalculator()
        {
            _startTimeKeyPressed = DateTime.Now;
            _lastTimeKeyPressed = DateTime.Now;
            StartWork();
        }

        public void StartWork()
        {
            _hooker = new Hooker();
            _hooker.KeyboardWasPressedEvent += WasPressed;
        }

        private void WasPressed(object sender, EventArgs args)
        {
              var pressedTime = DateTime.Now;

            if (_startTimeKeyPressed == null)
            {
                _startTimeKeyPressed = pressedTime;
                _lastTimeKeyPressed = pressedTime;
                return;
            }

            if (_lastTimeKeyPressed.AddMinutes(AFK_MAX_TIME_MINUTE) >= pressedTime)
            {
                _lastTimeKeyPressed = pressedTime;
                return;
            }

            if (_lastTimeKeyPressed.AddMinutes(AFK_MAX_TIME_MINUTE) <= pressedTime)
            {
                _workTimeMinute += _lastTimeKeyPressed.Subtract(_startTimeKeyPressed).Minutes;
                _startTimeKeyPressed = pressedTime;
                _lastTimeKeyPressed = pressedTime;
            }
        }

        public void RefreshTime()
        {
            var pressedTime = DateTime.Now;
            _workTimeMinute += _lastTimeKeyPressed.Subtract(_startTimeKeyPressed).Minutes;
            _startTimeKeyPressed = pressedTime;
            _lastTimeKeyPressed = pressedTime;
        }


        public void StopTimer()
        {
            _hooker.Unhook();
        }
    }
}
