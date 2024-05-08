using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Collections.Generic;
using MessageMedia.Messages;
using MessageMedia.Messages.Controllers;
using MessageMedia.Messages.Exceptions;
using MessageMedia.Messages.Models;
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
using Message = Microsoft.Azure.Devices.Client.Message;

namespace CulmativeWinAppCS12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static DeviceClient deviceClient;
        string usersEmail;
        string sendConnectionString = "HostName=WCHS.azure-devices.net;DeviceId=wchs5;SharedAccessKey=oqnP/yu8GtfqXRs4ffkAy/r2skjaIvK9ZAIoTDW+sBk=";
        private readonly static string connectionString = "Endpoint=sb://iothub-ns-wchs-58009161-d596108607.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=glM1A7WyVpMhjjMrhLcdQjkIr91upa8tSAIoTFiDvUI=;EntityPath=wchs";
        private readonly static string EventHubName = "wchs";
        private Data data = new Data(0, 0, 0, 0);
        static int msgCount = 1;
        Plant[] plants = new Plant[2035];
        bool moistureBelow = false;
        bool moistureAbove = false;

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
                        switch (datum.Substring(1, 3)) // Seperates the IoT data into the corrosponding properties
                            {
                            case "Hum":
                                if (float.TryParse(datum.Substring(11, 4), out f)) {
                                    data.Humidity = float.Parse(datum.Substring(11, 4));
                                    humBox.Text = data.Humidity.ToString();
                                }
                                break;
                            case "Tem":
                                if (float.TryParse(datum.Substring(14, 4), out f))
                                {

                                    data.Temperature = float.Parse(datum.Substring(14, 4));
                                    tempBox.Text = data.Temperature.ToString();
                                }
                                break;
                            case "Lig":
                                if (float.TryParse(datum.Substring(9, 4), out f))
                                {
                                    data.Light = float.Parse(datum.Substring(9, 4));
                                    lightBox.Text = data.Light.ToString();
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
                SendMsg("MMin" + plants[plantSelector.SelectedIndex].MoistureMin.ToString(), sendConnectionString);
                SendMsg("MMax" + plants[plantSelector.SelectedIndex].MoistureMax.ToString(), sendConnectionString);
            }
        }
        public static void SendEmail(string subject, string body, string email)
        {
            var smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("neolastless@outlook.com", "NeoOne1New"),
                EnableSsl = true,
            };
            smtpClient.Send("neolastless@outlook.com", email, subject, body);
        }
        public static async void SendMsg(string msg, string connectionString)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(500);
                deviceClient = DeviceClient.CreateFromConnectionString(connectionString, Microsoft.Azure.Devices.Client.TransportType.Mqtt);
                string jsonData = JsonConvert.SerializeObject(msg);
                Message message = new Message(Encoding.ASCII.GetBytes(jsonData));
                await deviceClient.SendEventAsync(message);
                await Task.Delay(300);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMsg("off", sendConnectionString);
            SendEmail("Your plant has been watered!", "Thank you for using our amazing serive! Your plant was just watered", usersEmail);
        }

        private void userEmail_TextChanged(object sender, EventArgs e)
        {
            usersEmail = userEmail.Text;
        }
    }
}
