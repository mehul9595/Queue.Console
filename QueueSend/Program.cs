using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace QueueSendConsole.Core
{
    class Program
    {
        private const string _queueName = "appqueue";

        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(@"D:\Projects\Azure Labs\Queue.Console\QueueSend\appSettings.json").Build();
            
            Console.WriteLine($"{configuration["connectionString"]}");
            
            IQueueClient client = new QueueClient(configuration["connectionString"], _queueName);
            
            int i = 1;
            while (i <= 1000)
            {
                string message = $"Sending Message: {i} - {DateTime.Now.ToString("yyyyMMddHHmmss")}";
                Message msg = new Message(Encoding.UTF8.GetBytes(message));
                msg.TimeToLive = TimeSpan.FromSeconds(30);
                await client.SendAsync(msg);
                await Console.Out.WriteLineAsync($"sending {message}");
                i++;            
            }

            await Console.Out.WriteLineAsync("Hello World!");
            Console.Read();
        }
    }
}
