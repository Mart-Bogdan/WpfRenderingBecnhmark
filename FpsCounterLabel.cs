using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfRenderingBecnhmark
{
    public class FpsCounterLabel : Label
    {
        public static DependencyProperty ThrottleProperty;
        private Random _random;

        private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1);
        private DateTime? _secondStart;
        private int _framesCounted = 0;
        private readonly DispatcherTimer _dispatcherTimer;
        private bool _extraTrhrottle;

        static FpsCounterLabel()
        {
            FrameworkPropertyMetadata fpm = new FrameworkPropertyMetadata(Guid.Empty,FrameworkPropertyMetadataOptions.AffectsRender);
            
            ThrottleProperty = DependencyProperty.Register("Throttle", typeof(Guid), typeof(FpsCounterLabel),fpm);
        }

        public FpsCounterLabel()
        {
            _random = new Random();
            CompositionTarget.Rendering += OnTimerTick;
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(0.001);

            _dispatcherTimer.Tick += OnTimerTick;

            //_dispatcherTimer.Start();
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            Throttle = Guid.NewGuid();
        }
	 
        public Guid Throttle 
        { 
            get { return (Guid) GetValue(ThrottleProperty); } 
            set { 
                SetValue(ThrottleProperty, value); 
            } 
        }

        public bool ExtraTrhrottle
        {
            get { return _extraTrhrottle; }
            set
            {
                _extraTrhrottle = value;
                if (value)
                {
                    _dispatcherTimer.Start();
                    CompositionTarget.Rendering -= OnTimerTick;
                }
                else
                {
                    _dispatcherTimer.Stop();
                    CompositionTarget.Rendering += OnTimerTick;
                }
            }
        }

        public double ExtraThrottleDelayMs
        {
            get { return _dispatcherTimer.Interval.TotalMilliseconds; }
            set
            {
                _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(value);
            }
        }

        protected override void OnRender(DrawingContext drawingContext) 
        { 
            base.OnRender(drawingContext);
            //do measure
            var now = DateTime.Now;

            if (_secondStart==null)
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