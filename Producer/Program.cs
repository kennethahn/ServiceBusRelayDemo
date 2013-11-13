using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        private static string queueName = "greetings";

        static void Main(string[] args)
        {
            Console.Out.WriteLine("Who do you want to send a message to?");
            string name = Console.ReadLine();
            Console.Out.WriteLine("What is your message to " + name + "?");
            string message = Console.ReadLine();
        }

        private void CreateQueue()
        {
            NamespaceManager nsMgr = NamespaceManager.Create();
            Console.Out.Write("Creating queue {0}... ", queueName);
            if (nsMgr.QueueExists(queueName))
            {
                nsMgr.DeleteQueue(queueName);
            }
            nsMgr.CreateQueue(queueName);
            Console.Out.WriteLine("Done.");
        }





    }
}
