using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Collections.Generic;

using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static Microsoft.Azure.Amqp.Serialization.SerializableType;
using System.Net.Mail;
using System.Net;
using System.IO.Ports;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Amqp.Framing;

namespace CulmativeWinAppCS12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static DeviceClient deviceClient;
        string usersEmail = "jsuurman@gmail.com";
        string sendConnectionString = "HostName=WCHS.azure-devices.net;DeviceId=wchs5;SharedAccessKey=oqnP/yu8GtfqXRs4ffkAy/r2skjaIvK9ZAIoTDW+sBk=";
        private readonly static string connectionString = "Endpoint=sb://iothub-ns-wchs-58009161-d596108607.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=glM1A7WyVpMhjjMrhLcdQjkIr91upa8tSAIoTFiDvUI=;EntityPath=wchs";
        private readonly static string EventHubName = "wchs";
        private Data data = new Data(0, 0, 0, 0);
        static int msgCount = 1;
        Plant[] plants = new Plant[2035];
        bool moistureBelow = false;
        bool moistureAbove = false;
        DateTime lightLastGood = DateTime.Now;
        DateTime tempLastGood = DateTime.Now;
        DateTime humidityLastGood = DateTime.Now;

        private async Task ReceiveMessagesFromDeviceAsync()
        {
            await using EventHubConsumerClient consumer = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, connectionString, EventHubName);
            await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync())
            {
                partitionEvent.Data.SystemProperties.TryGetValue("iothub-connection-device-id", out object deviceID);  // read event message from Event Hub partition 
                if (deviceID.ToString() == "wchs6")
                {
                    float f;
                    string datum = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                    this.listMsgs.Items.Insert(0, datum);
                    if (datum.Substring(1, 5) == "Humid" || datum.Substring(1, 5) == "Tempe" || datum.Substring(1, 5) == "Light" || datum.Substring(1, 5) == "Moist")
                    {
                        switch (datum.Substring(1, 3)) // Seperates the IoT data into the corrosponding properties
                        {
                            case "Hum":
                                if (float.TryParse(datum.Substring(11, 4), out f))
                                {
                                    data.Humidity = float.Parse(datum.Substring(11, 4));
                                    humBox.Text = data.Humidity.ToString();
                                    if (data.Humidity > 60 && data.Humidity < 80)
                                    {
                                        humidityTipBx.Text = "Humidity is good!";
                                        humidityLastGood = DateTime.Now;
                                    }
                                    if (Math.Abs(humidityLastGood.Day - DateTime.Now.Day) > 2)
                                    {
                                        if (data.Humidity < 60)
                                        {
                                            humidityTipBx.Text = "Not Humid enough. Move to a more humid place.";  
                                            SendEmail("Plant is Lacking Humidity", humidityTipBx.Text, usersEmail);
                                        }
                                        if (data.Humidity > 80)
                                        {
                                            humidityTipBx.Text = "Too Humid. Move to a less humid place.";
                                            SendEmail("Plant is too Humid", humidityTipBx.Text, usersEmail);
                                        }
                                    }
                                }
                                break;
                            case "Tem":
                                if (float.TryParse(datum.Substring(14, 4), out f))
                                {

                                    data.Temperature = float.Parse(datum.Substring(14, 4));
                                    tempBox.Text = data.Temperature.ToString();
                                    if (data.Temperature > 15 && data.Temperature < 25)
                                    {
                                        TempTipBx.Text = "Temperature is good!";
                                        tempLastGood = DateTime.Now;
                                    }
                                    if (Math.Abs(DateTime.Now.Day - tempLastGood.Day) > 2)
                                    {
                                        if (data.Temperature > 25)
                                        {
                                            TempTipBx.Text = "Temperature is too high. Please consider moving plant to a cooler place.";
                                            SendEmail("Plant Too Hot", "Temperature is too high. Please consider moving plant to a cooler place.", usersEmail);
                                        }
                                        if (data.Temperature < 15)
                                        {
                                            TempTipBx.Text = "Temperature is too cool. Please consider moving plant to a warmer place.";
                                            SendEmail("Plant Too Cold", "Temperature is too cold. Please consider moving plant to a warmer place.", usersEmail);
                                        }
                                    }
                                }
                                break;
                            case "Lig":
                                if (float.TryParse(datum.Substring(9, 4), out f) == true)
                                {
                                    data.Light = float.Parse(datum.Substring(9, 4));
                                    lightBox.Text = data.Light.ToString();
                                    if (data.Light > 60)
                                    {
                                        lightTipBx.Text = "Light Levels are good!";
                                        lightLastGood = DateTime.Now;
                                       
                                    }
                                    else if (Math.Abs(lightLastGood.Day - DateTime.Now.Day) > 2)
                                    {
                                        lightTipBx.Text = "Light levels are too low. Please move to a space with more light.";
                                        SendEmail("Low Light!", "Light levels are too low.Please move to a space with more light.", usersEmail);
                                    }
                                    else
                                    {
                                        lightTipBx.Text = "Light levels are currently too low";
                                    }
                                }
                                break;
                            case "Moi":
                                if (float.TryParse(datum.Substring(12, 4), out f))
                                {
                                    data.Moisture = float.Parse(datum.Substring(12, 4));
                                    moisBox.Text = data.Moisture.ToString();
                                    if (plantSelector.SelectedIndex > 0)
                                    {
                                        if (data.Moisture <= plants[plantSelector.SelectedIndex].MoistureMin && moistureBelow == false)
                                        {
                                            SendEmail("Low Water", "Your plants water moisture is low and will be watered shortly.", usersEmail);
                                            moistureBelow = true;
                                        }
                                        if (data.Moisture >= plants[plantSelector.SelectedIndex].MoistureMax & moistureAbove == false)
                                        {
                                            SendEmail("Too Much Water", "Your plants water moisture is way too high, please check to make sure the pump is working properly.", usersEmail);
                                            moistureAbove = true;
                                        }
                                        if (data.Moisture > plants[plantSelector.SelectedIndex].MoistureMin && data.Moisture < plants[plantSelector.SelectedIndex].MoistureMax)
                                        {
                                            moistureBelow = false;
                                            moistureAbove = false;
                                        }
                                    }
                                }
                                break;
                        }
                        }
                        UpdateChart(data);
                        msgCount++;
                    }
                }
        }
        private void UpdateChart(Data data)
        {
            this.chartData.Series["Temperature"].Points.AddXY(msgCount, data.Temperature);
            this.chartData.Series["Humidity"].Points.AddXY(msgCount++, data.Humidity);
            this.chartData.Series["Moisture"].Points.AddXY(msgCount++, data.Moisture);
            this.chartData.Series["Light Level"].Points.AddXY(msgCount++, data.Light);
            this.chartData.ChartAreas[0].AxisX.Minimum = 0;
                //Math.Round((this.chartData.ChartAreas[0].AxisX.Maximum - 500) / 50) * 50;
            this.chartData.ChartAreas[0].AxisY.Maximum = 100;
        }

        private void SetUpChart()
        {
            this.chartData.Palette = ChartColorPalette.SeaGreen;
            this.chartData.Titles.Add("Plant data");
            this.chartData.Series.Clear();
            this.chartData.Series.Add("Temperature");
            this.chartData.Series["Temperature"].ChartType = SeriesChartType.Line;
            this.chartData.Series.Add("Humidity");
            this.chartData.Series["Humidity"].ChartType = SeriesChartType.Line;
            this.chartData.Series.Add("Moisture");
            this.chartData.Series["Moisture"].ChartType = SeriesChartType.Line;
            this.chartData.Series.Add("Light Level");
            this.chartData.Series["Light Level"].ChartType = SeriesChartType.Line;
        }

        private void SetUpListBox()
        {
            this.listMsgs.Items.Add("Reading data from WCHS IoT Devices. Ctrl-C to exit.\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetUpChart();
            SetUpListBox();
            DownloadData();
            ReceiveMessagesFromDeviceAsync();
        }
        private void DownloadData()
        {
            StreamReader reader = new StreamReader("PlantInfo.csv");
            int ColumnsCount = reader.ReadLine().Split(',').Length;
            for (int j = 0; j < plants.Length; j ++)
            {
                if (reader.ReadLine() != null)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        var values = line.Split(',');
                        for (int i = 0; i < values[4].Length; i++) {
                            if (values[4].Substring(i, 1) == "?" || values[4].Substring(i, 1) == "(") {
                                values[4] = values[4].Substring(0, i);
                            }
                        }
                        Plant newPlant = new Plant(values[4], values[14]);
                        plants[j] = newPlant;
                    }
                }
            }        
            foreach (Plant plant in plants)
            {
                if (plant != null)
                {
                    plantSelector.Items.Add(plant.Name);
                }
            }      
        }

        private void plantSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (plantSelector.SelectedIndex > 0)
            {
                preferredMoisturebx.Text = plants[plantSelector.SelectedIndex].MoistureMin.ToString() + " - " + plants[plantSelector.SelectedIndex].MoistureMax.ToString();
                SendMsg("MMin" + (plants[plantSelector.SelectedIndex].MoistureMin*100).ToString(), sendConnectionString);
                SendMsg("MMax" + (plants[plantSelector.SelectedIndex].MoistureMax*100).ToString(), sendConnectionString);
            }
        }
        public static void SendEmail(string subject, string body, string email)
        {
            /*var smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("neolastless@outlook.com", "NeoOne1New"),
                EnableSsl = true,
            };
            smtpClient.Send("neolastless@outlook.com", email, subject, body);*/
        }
        public static async void SendMsg(string msg, string connectionString)
        {
                System.Threading.Thread.Sleep(500);
                deviceClient = DeviceClient.CreateFromConnectionString(connectionString, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
                string jsonData = JsonConvert.SerializeObject(msg);
                Microsoft.Azure.Devices.Client.Message message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(jsonData));
                await deviceClient.SendEventAsync(message);
                await Task.Delay(300);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMsg("ooff", sendConnectionString);
            SendEmail("Emergency Water Off", "You have activated the emergency protocol to turn off the water pump.", usersEmail);
        }

        private void userEmail_TextChanged(object sender, EventArgs e)
        {
            usersEmail = userEmail.Text;
        }

        private void onButton_Click(object sender, EventArgs e)
        {
            SendMsg("oon", sendConnectionString);
            SendEmail("Water On", "You have re-activated the water pump.", usersEmail);

        }
    }
}
