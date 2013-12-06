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
        private static NamespaceManager nsMgr;

        static void Main(string[] args)
        {
            CreateQueue();
            queueClient = QueueClient.Create(queueName, ReceiveMode.PeekLock);
            Console.Out.WriteLine("Queue client receive mode is " + queueClient.Mode.ToString());
            try
            {
                while (true)
                {
                    long msgCount = nsMgr.GetQueue(queueName).MessageCountDetails.ActiveMessageCount;
                    Console.Out.WriteLine("Ready for messages... ");
                    if (msgCount > 0) { Console.Out.WriteLine("The queue has " + msgCount + " messages waiting"); }
                    var msg = queueClient.Receive(new TimeSpan(1, 0, 0));
                    Console.Out.WriteLine("Received message with id {0}", msg.MessageId);
                    Messages.Greeting greeting = msg.GetBody<Messages.Greeting>();
                    Console.Out.WriteLine("Message for: {0}", greeting.Name );
                    Console.Out.WriteLine("{0}\n", greeting.Message);
                    if (queueClient.Mode == ReceiveMode.PeekLock)
                    {
                        try
                        {
                            Console.Out.WriteLine("Completing message with lock token {0}", msg.LockToken);
                            queueClient.Complete(msg.LockToken);
                        }
                        catch (MessageLockLostException)
                        {
                            Console.Out.WriteLine("Oh noes! We lost the lock on the message. ");
                        }
                    }
                    Console.Out.WriteLine();
                }
            }
            finally
            {
                queueClient.Close();
            }
        }

        private static void CreateQueue()
        {
            if (nsMgr == null)
            {
                nsMgr = NamespaceManager.Create();
            }
            if (!nsMgr.QueueExists(queueName))
            {
                Console.Out.Write("Creating queue {0}... ", queueName);
                nsMgr.CreateQueue(queueName);
                Console.Out.WriteLine("Done.");
            }
        }
    }
}
