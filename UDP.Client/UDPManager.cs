using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UDP.Client.Prop;
using ExpressEncription;

namespace UDP.Client
{
    public class UDPManager
    {
        UdpClient _udp;
        IPEndPoint _ipEndpoint;
        IPAddress _ipAdress;
        private int lifes = 2;

        
        public UDPManager()
        {
            _udp = new UdpClient();
            _udp.Client.Bind(new IPEndPoint(IPAddress.Any, Config.ClientPort));
            _ipAdress = IPAddress.Parse(Config.IPAdress);
            _ipEndpoint = new IPEndPoint(_ipAdress, Config.ClientPort);
        }

        public void Start()
        {
            Console.WriteLine("New Connection");
            Console.WriteLine();
            Console.WriteLine("Enter message: ");
            string? message = Console.ReadLine();
            while (message != string.Empty)
            {
                SendMessage(message);

                ReceiveMessage();
                message = Console.ReadLine();

               
            }
            Console.ReadLine();
        }

        private void SendMessage(string? message)
        {
            var EncryptedMessage = ExpressEncription.RSAEncription.EncryptString(message, @"D:\programare\Max volosenko\Task\UDP.Client\public.key");
            byte[] bufferToSend = Encoding.ASCII.GetBytes(EncryptedMessage);

            _udp.Send(bufferToSend, bufferToSend.Length, Config.IPAdress, Config.ServerPort);
        }

        private void ReceiveMessage()
        {
            byte[] bytes = _udp.Receive(ref _ipEndpoint);

            string messageFromServer = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

            var decyptedMessage = ExpressEncription.RSAEncription.DecryptString(messageFromServer, @"D:\programare\Max volosenko\Task\UDP.Client\private.key");

            if (string.IsNullOrWhiteSpace(decyptedMessage) || decyptedMessage == "Error")
            {
                Console.WriteLine("Message invalid, try again");
                
                lifes--;
                if (lifes < 0 )
                {
                    Console.WriteLine();
                    Console.WriteLine("Retrying connection");
                    lifes = 2;
                    Start();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter message");
                    
                   
                }
                

            }
            else
            {
                Console.WriteLine("Success");
                Console.WriteLine();
                Console.WriteLine("Enter a message");
                lifes = 2;
            }

        }

    }
}
