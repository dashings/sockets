using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication2
{
    class Program
    {
        private static byte[] result = new byte[1024];
        static void Main(string[] args)
        {
            Socket receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9998);

            receiveSocket.Bind(hostIpEndPoint);
            //监听
            receiveSocket.Listen(2);
            ////接受客户端连接
            Socket hostSocket = receiveSocket.Accept();

            for (int i = 0; i < 10; i++)
            {
                byte[] data = new byte[4];
                int rect = hostSocket.Receive(data, 0, 4, 0); //用来接收图片字节流长度
                int size = BitConverter.ToInt32(data, 0);  //16进制转成int型


                byte[] buffer = new byte[size];
                hostSocket.Receive(buffer, buffer.Length, SocketFlags.None);
                Console.WriteLine("Receive success");

                FileStream fs1 = File.Create(i.ToString()+".jpg");
                fs1.Write(buffer, 0, buffer.Length);
                fs1.Close();
            }


            //关闭接收数据的Socket
            hostSocket.Shutdown(SocketShutdown.Receive);
            hostSocket.Close();
            //关闭发送连接
            receiveSocket.Close();
            
        }
    }
}
