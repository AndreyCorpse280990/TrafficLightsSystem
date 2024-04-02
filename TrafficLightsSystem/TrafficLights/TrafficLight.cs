using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightsSystem.TrafficLights
{
    // TrafficLight - абстрактный класс светофора
    internal abstract class TrafficLight
    {
        // делегаты работающие со светофором
        // делегат запуска светофора
        public delegate void StartAction();
        // делегат остановки светофора
        public delegate void StopAction();
        // делегат обновления состояния светофора
        public delegate void UpdateAction(TrafficLight sender, List<TrafficLightSignal> signals); 


        private int id;
        private string description;

        // событие обновления состояние светофора
        public virtual event UpdateAction UpdateEvent;

        public TrafficLight(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        // абстрактные методы работы со светофором

        // Start - метод запуска работы светофора
        public abstract void Start();

        // Stop - метод остановки работы светофора
        public abstract void Stop();

        public override string ToString()
        {
            return $"{id}[{description}]";
        }
    }
}
