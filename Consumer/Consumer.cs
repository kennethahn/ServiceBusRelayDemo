using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    class Consumer
    {
        private static string queueName = "greetings";
        private static QueueClient queueClient;

        static void Main(string[] args)
        {
            CreateQueue();
            queueClient = QueueClient.Create(queueName);
            try
            {
                while (true)
                {
                    Console.Out.WriteLine("Ready for messages...");
                    var msg = queueClient.Receive(new TimeSpan(1, 0, 0));
                    Console.Out.WriteLine("Received message with id {0}", msg.MessageId);
                    Console.Out.WriteLine("Message is: {0}", msg.GetBody<string>());
                }
            }
            finally
            {
                queueClient.Close();
            }
        }

        private static void CreateQueue()
        {
            NamespaceManager nsMgr = NamespaceManager.Create();
            if (!nsMgr.QueueExists(queueName))
            {
                Console.Out.Write("Creating queue {0}... ", queueName);
                nsMgr.CreateQueue(queueName);
                Console.Out.WriteLine("Done.");
            }
        }
    }
}
