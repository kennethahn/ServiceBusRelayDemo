using Messages;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    class Producer
    {
        private static string queueName = "greetings";
        private static QueueClient queueClient;

        static void Main(string[] args)
        {
            queueClient = QueueClient.Create(queueName);
            bool keepOn = true;
            while(keepOn)
            {
                Console.Out.WriteLine("Who do you want to send a message to?");
                string name = Console.ReadLine();
                Console.Out.WriteLine("What is your message to " + name + "?");
                string message = Console.ReadLine();
                SendMessage(name, message);
                Console.Out.WriteLine("Press [ENTER] to send another message or any other key to quit");
                ConsoleKeyInfo ki = Console.ReadKey();
                keepOn = (ki.Key == ConsoleKey.Enter);
            }
            queueClient.Close();
        }

        private static void SendMessage(string name, string message)
        {
            Greeting g = new Greeting() { Name = name, Message = message };
            while(true){
                try
                {
                    BrokeredMessage bmsg = new BrokeredMessage(message);
                    queueClient.Send(bmsg);
                    Console.Out.WriteLine("Sent message with id {0}", bmsg.MessageId);
                    break;
                }
                catch (MessagingException mex)
                {
                    if (!mex.IsTransient)
                    {
                        throw;
                    }
                    else
                    {
                        Console.Out.WriteLine("We experienced a temporary setback due to {0}", mex.Message);
                        Console.Out.WriteLine("Retrying in 2 seconds.");
                        Thread.Sleep(2000);
                    }
                }
            }
        }
    }
}
