using System;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace QueueReceiveConsole.Core
{
    class Program
    {
        const string _queueName = "appqueue";
        private static IQueueClient _client;
        
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(@"D:\Projects\Azure Labs\Queue.Console\QueueReceive\appSettings.json").Build();
            
            Console.WriteLine($"{configuration["connectionString"]}");

            _client = new QueueClient(configuration["connectionString"], _queueName);

            MessageHandlerOptions options = new MessageHandlerOptions(ExceptionReceived)
            {
                AutoComplete = false,
                MaxConcurrentCalls = 10
            };

            _client.RegisterMessageHandler(ProcessMessage, options);
            Console.WriteLine("Reading Messages...");
            Console.Read();           

        }


        static async Task ProcessMessage(Message message, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync(Encoding.UTF8.GetString(message.Body));

            await _client.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceived(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine(args.Exception);
            return Task.CompletedTask;
        }


    }
}
