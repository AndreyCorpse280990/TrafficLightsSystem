using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using static TrafficLightsSystem.TrafficLights.TrafficLightSignal;

namespace TrafficLightsSystem.TrafficLights
{
    // ThreeSectionTrafficLight - обычный трехсекционный светофор
    internal class ThreeSectionTrafficLight : TrafficLight
    {
        // поля - 3 сигнала (красный, желтый, зеленый)
        private TrafficLightSignal red;
        private TrafficLightSignal yellow;
        private TrafficLightSignal green;

        // тайминги для каждого из цветов
        private int redTimingSeconds;
        private int yellowTimingSeconds;
        private int greenTimingSeconds;
        private int stage;                  // стадия работы светофора (0, 1, 2, 3)
        private DateTime lastStageStartTime; // время начала работы последней стадии

        // событие обновления состояние светофора
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

        public ThreeSectionTrafficLight(
            int id, string description, 
            int redTimingSeconds, int yellowTimingSeconds, int greenTimingSeconds
        ) : base(id, description)
        {
            // создать сигналы
            red = new TrafficLightSignal("Red", SignalState.Inactive);
            yellow = new TrafficLightSignal("Yellow", SignalState.Inactive);
            green = new TrafficLightSignal("Green", SignalState.Inactive);
            // проинициалировать тайминги
            this.redTimingSeconds = redTimingSeconds;
            this.yellowTimingSeconds = yellowTimingSeconds;
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
            red.On();
            yellow.Off();
            green.Off();
            stage = 0;
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
                // переключение на желтый
                red.Off();
                yellow.On();
                stage++;
                lastStageStartTime = DateTime.Now;
                MakeUpdate();
            } else if (stage == 1 && LastStageElapsedSeconds > yellowTimingSeconds)
            {
                // переключаем на зеленый
                yellow.Off();
                green.On();
                stage++;
                lastStageStartTime = DateTime.Now;
                MakeUpdate();
            } else if (stage == 2 && LastStageElapsedSeconds > greenTimingSeconds)
            {
                // переключаем обратно на желтый
                green.Off();
                yellow.On();
                stage++;
                lastStageStartTime = DateTime.Now;
                MakeUpdate();
            } else if (stage == 3 && LastStageElapsedSeconds > yellowTimingSeconds)
            {
                // переключаем обартно на красный
                yellow.Off();
                red.On();
                stage = 0;
                lastStageStartTime = DateTime.Now;
                MakeUpdate();
            }
        }

        private void MakeUpdate()
        {
            UpdateEvent?.Invoke(this, new List<TrafficLightSignal>() { red, yellow, green});
        }
    }
}
