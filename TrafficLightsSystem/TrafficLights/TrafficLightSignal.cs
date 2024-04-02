using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficLightsSystem.TrafficLights
{
    // TrafficLightSignal - сигнал светофора
    internal class TrafficLightSignal
    {
        // SignalState - состояние сигнала светофора - активный/неактивный
        internal enum SignalState
        {
            Active,
            Inactive,
        }
        
        // описание сигнала
        public string Description { get; set; }

        // состояние сигнала
        public SignalState State { get; set; } 

        public TrafficLightSignal(string description, SignalState initialState)
        {
            Description = description;
            State = initialState;
        }

        // методы включения/выключения сигнала
        public void On()
        {
            State = SignalState.Active;
        }

        public void Off()
        {
            State = SignalState.Inactive;
        }

        public override string ToString()
        {
            return $"{Description}[{State}]";
        }
    }
}
