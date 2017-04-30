using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfRenderingBecnhmark
{
    public class AnimationFrameCounterLabel : Label
    {

        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);
        private DateTime? _secondStart;
        private int _framesCounted = 0;
        

        public AnimationFrameCounterLabel()
        {
            CompositionTarget.Rendering += OnTimerTick;
            //var dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Interval=TimeSpan.FromMilliseconds(0.00001);

            //dispatcherTimer.Tick += OnTimerTick;

            //dispatcherTimer.Start();
        }

        void OnTimerTick(object sender, EventArgs e)
        {

            var now = DateTime.Now;

            if (_secondStart == null)
            {
                _secondStart = now;
            }

            _framesCounted++;

            var timeDiff = now - _secondStart.Value;
            if (timeDiff >= OneSecond)
            {
                double fps = _framesCounted / timeDiff.TotalSeconds;
                this.Content = fps;
                _secondStart = now;
                _framesCounted = 0;
            }
        }

	 
    }
}