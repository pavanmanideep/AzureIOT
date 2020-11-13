using System;
using Microsoft.Azure.Devices.Client;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SimulationDeviceToCloud
{
    class Program
    {
        private static DeviceClient s_deviceClient;
		//Provide your hub connection string
        private readonly static string s_connectionString01 = "";
        static void Main(string[] args)
        {
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString01, TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync(s_deviceClient);
            Console.ReadLine();

        }

        private static async void SendDeviceToCloudMessagesAsync(DeviceClient s_deviceClient)
        {
            try
            {
                double minTemperature = 20;
                double minHumidity = 60;
                Random rand = new Random();

                while (true)
                {
                    double currentTemperature = minTemperature + rand.NextDouble() * 15;
                    double currentHumidity = minHumidity + rand.NextDouble() * 20;

                   // Create JSON message

                    var telemetryDataPoint = new
                    {
                       
                        temperature = currentTemperature,
                        humidity = currentHumidity
                    };

                    string messageString = "";



                    messageString = JsonConvert.SerializeObject(telemetryDataPoint);

                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    // Add a custom application property to the message.
                    // An IoT hub can filter on these properties without access to the message body.
                    //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

                    // Send the telemetry message
                    await s_deviceClient.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
                    await Task.Delay(1000 * 10);
                
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
