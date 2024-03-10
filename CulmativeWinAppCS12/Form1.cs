using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CulmativeWinAppCS12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly static string connectionString = "Endpoint=sb://iothub-ns-wchs-58009161-d596108607.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=glM1A7WyVpMhjjMrhLcdQjkIr91upa8tSAIoTFiDvUI=;EntityPath=wchs";
        private readonly static string EventHubName = "wchs";
        private Data data = new Data(0, 0, 0, 0);
        static int msgCount = 1;

        // start reading any messages
        private async Task ReceiveMessagesFromDeviceAsync()
        {
                await using EventHubConsumerClient consumer = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, connectionString, EventHubName);
                await foreach (PartitionEvent partitionEvent in consumer.ReadEventsAsync())
                {
                    partitionEvent.Data.SystemProperties.TryGetValue("iothub-connection-device-id", out object deviceID);  // read event message from Event Hub partition 
                    if (deviceID.ToString() == "wchs6")
                    {
                        string datum = Encoding.UTF8.GetString(partitionEvent.Data.Body.ToArray());
                        listMsgs.Items.Add(datum);
                        switch (datum.Substring(1, 3))
                        {
                            case "Hum":
                                data.Humidity = float.Parse(datum.Substring(11, 4));
                                humBox.Text = data.Humidity.ToString();
                                break;
                            case "Tem":
                                data.Temperature = float.Parse(datum.Substring(14, 4));
                                tempBox.Text = data.Temperature.ToString();
                                break;
                            case "Lig":
                                data.Light = float.Parse(datum.Substring(9, 4));
                                lightBox.Text = data.Light.ToString();
                                break;
                            case "Moi":
                                data.Moisture = float.Parse(datum .Substring(12, 4));
                                moisBox.Text = data.Moisture.ToString();
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
            ReceiveMessagesFromDeviceAsync();
        }
    }
}
