using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrafficLightsSystem.TrafficLights;
using static TrafficLightsSystem.TrafficLights.TrafficLight;

namespace TrafficLightsSystem
{
    public partial class MainForm : Form
    {
        private StartAction start; 
        private StopAction stop; 

        public MainForm()
        {
            InitializeComponent();
            PrepareTrafficLights();
        }

        private void PrepareTrafficLights()
        {
            TrafficLight tl_1 = new ThreeSectionTrafficLight(1, "Автомобильный", 10, 3, 7);
            tl_1.UpdateEvent += trafficLight_Update;
            start += tl_1.Start;
            stop += tl_1.Stop;
            TrafficLight tl_2 = new TwoSectionTrafficLight(2, "Пешеходный", 13, 10);
            tl_2.UpdateEvent += trafficLight_Update;
            start += tl_2.Start;
            stop += tl_2.Stop;

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stop();
        }

        private void trafficLight_Update(TrafficLight trafficLight, List<TrafficLightSignal> signals)
        {
            logTexBox.Text += $"[{trafficLight}]:";
            foreach (var signal in signals)
            {
                logTexBox.Text += $" {signal}";
            }
            logTexBox.AppendText(Environment.NewLine);
        }
    }
}
