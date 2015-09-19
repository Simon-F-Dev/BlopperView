using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace test
{
	class ServerClass
	{
		public static void main (string[] args)
		{

			{ 
				TcpListener server=null;   
				try
				{
					Int32 port = 12345;
					IPAddress localIp = IPAddress.Parse("127.0.0.1");

					server = new TcpListener(localIp, port);

					server.Start();

					Byte[] bytes = new Byte[256];
					String data = null;

					while(true) 
					{
						Console.Write("Waiting for a connection... ");

						TcpClient client = server.AcceptTcpClient();            
						Console.WriteLine("Connected!");
						
						data = null;

						NetworkStream stream = client.GetStream();
						
						int i;

						while((i = stream.Read(bytes, 0, bytes.Length))!=0) 
						{   
							data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
							Console.WriteLine(data);

							byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);          
						}


					}
				}
				catch(SocketException e)
				{
					Console.WriteLine("SocketException: {0}", e);
				}
				finally
				{
					server.Stop();
				}
				Console.WriteLine("\nHit enter to continue...");
				Console.Read();
			}
		}
	}
}
