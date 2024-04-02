using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using static TrafficLightsSystem.TrafficLights.TrafficLight;
using static TrafficLightsSystem.TrafficLights.TrafficLightSignal;

namespace TrafficLightsSystem.TrafficLights
{
    internal class TwoSectionTrafficLight : TrafficLight
    {
        private TrafficLightSignal red;
        private TrafficLightSignal green;

        private int redTimingSeconds;
        private int greenTimingSeconds;
        private int stage;
        private DateTime lastStageStartTime;

        // Событие обновления состояния светофора
        public override event UpdateAction UpdateEvent;
        public double LastStageElapsedSeconds
        {
            get
            {
                return (DateTime.Now - lastStageStartTime).TotalSeconds;
            }
        }

        // таймер работы светофора
        private DispatcherTimer timer;

        public TwoSectionTrafficLight(
            int id, string description, int redTimingSeconds, int greenTimingSeconds)
            : base(id, description)
        {
            red = new TrafficLightSignal("Red", SignalState.Inactive);
            green = new TrafficLightSignal("Green", SignalState.Inactive);

            // Проинициализировать тайминги
            this.redTimingSeconds = redTimingSeconds;
            this.greenTimingSeconds = greenTimingSeconds;
        }

        public override void Start()
        {
            if (timer != null && timer.IsEnabled)
            {
                // если обработка уже запущена
                throw new InvalidOperationException("Processing already started");
            }
            if (timer == null)
            {
                // создавать таймер только один раз
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += ChangeStateHandler;
            }
            green.On();
            red.Off();          
            stage = 1;
            lastStageStartTime = DateTime.Now;
            MakeUpdate();
            timer.Start();
        }

        public override void Stop()
        {
            timer.Stop();
        }

        // ChangeStateHandler - обработчик изменения состояния светофора 
        private void ChangeStateHandler(object sender, EventArgs e)
        {
            if (stage == 0 && LastStageElapsedSeconds > redTimingSeconds)
            {
                // Переключение на зеленый
                red.Off();
                green.On();
                stage++;
                lastStageStartTime = DateTime.Now;
                MakeUpdate();
            }
            else if (stage == 1 && LastStageElapsedSeconds > greenTimingSeconds)
            {
                // Переключение на красный
                green.Off();
                red.On();
                stage = 0;
                lastStageStartTime = DateTime.Now;
                MakeUpdate();
            }
        }

        private void MakeUpdate()
        {
            UpdateEvent?.Invoke(this, new List<TrafficLightSignal>() { red, green });
        }

    }
}
