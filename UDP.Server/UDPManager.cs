using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UDP.Server.Prop;
using ExpressEncription;

namespace UDP.Server
{
    public class UDPManager
    {
        UdpClient _udp;
        IPEndPoint _ipEndpoint;
        IPAddress _ipAdress;
        //private int lifes = 2;



        public UDPManager()
        {
            _udp = new UdpClient();
            _udp.Client.Bind(new IPEndPoint(IPAddress.Any, Config.ServerPort));
            _ipAdress = IPAddress.Parse(Config.IPAdress);
            _ipEndpoint = new IPEndPoint(_ipAdress, Config.ServerPort);
        }
        public void Start()
        {
            Console.WriteLine("Waiting for broadcast");
            while (true)
            {
                string message = ReceiveMessage();
                SendMessage(message);
            }

        }
        private string ReceiveMessage()
        {
            byte[] bytes = _udp.Receive(ref _ipEndpoint);
            string messageFromClient = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
            var decyptedMessage = ExpressEncription.RSAEncription.DecryptString(messageFromClient, @"D:\programare\Max volosenko\Task\UDP.Client\private.key");
            Console.WriteLine($" {decyptedMessage}");

            return decyptedMessage;
        }

        private void SendMessage(string message)
        {
            

            if (!string.IsNullOrWhiteSpace(message) && !(message.Length < 5 ) )
            {
                string _messageToClient = "Message Received ";
                var EncryptedMessage = ExpressEncription.RSAEncription.EncryptString(_messageToClient, @"D:\programare\Max volosenko\Task\UDP.Client\public.key");
                byte[] bufferToSend = Encoding.ASCII.GetBytes(EncryptedMessage);


                _udp.Send(bufferToSend, bufferToSend.Length, Config.IPAdress, Config.ClientPort);
                //lifes = 2;
            }
            else
            {
               
                //lifes--;
                string _messageToClient = "Error";
                var EncryptedMessage = ExpressEncription.RSAEncription.EncryptString(_messageToClient, @"D:\programare\Max volosenko\Task\UDP.Client\public.key");
                byte[] bufferToSend = Encoding.ASCII.GetBytes(EncryptedMessage);

                _udp.Send(bufferToSend, bufferToSend.Length, Config.IPAdress, Config.ClientPort);

                //if (lifes < 0 )
                //{
                //    Console.WriteLine("Retrying connection");
                //    Console.WriteLine();
                //    Start();
                //}
                //else
                //{
                    Console.WriteLine("Trying to receive message again");

                //}
            }
        }
    }
}
