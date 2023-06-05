// See https://aka.ms/new-console-template for more information
using Microsoft.Azure.Devices.Client;
using IoTHubExample;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.Devices.Shared;

Console.WriteLine("Hello, World!");
Random Rand = new Random();
var deviceAuthentication = new DeviceAuthenticationWithRegistrySymmetricKey(Configuration.deviceId, Configuration.deviceKey);

DeviceClient deviceClient = DeviceClient.Create(Configuration.iotHubHostName, deviceAuthentication, TransportType.Mqtt);
var _messageId = 0;

deviceClient.SetDesiredPropertyUpdateCallback((TwinCollection desiredProperties, object userContext) =>
{

});

while (true)
{
  double currentTemperature = 20 + Rand.NextDouble() * 15;
  double currentHumidity = 60 + Rand.NextDouble() * 20;

  var telemetryDataPoint = new
  {
    messageId = _messageId++,
    deviceId = Configuration.deviceId,
    temperature = currentTemperature,
    humidity = currentHumidity
  };
  string messageString = JsonConvert.SerializeObject(telemetryDataPoint);
  Message message = new Message(Encoding.ASCII.GetBytes(messageString));
  message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

  await deviceClient.SendEventAsync(message);
  Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

  await Task.Delay(10000);
}