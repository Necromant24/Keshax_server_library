﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Keshax
{
    public class KXServer
    {
        
        
        
        
        
        
        

        public void serve()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("0.0.0.0"),5000);
            listener.Start();
            
            Console.WriteLine("started on: http://"+"localhost:5000");
            
            ThreadPool.SetMinThreads(2, 2);
            ThreadPool.SetMaxThreads(3, 3);
            
            var worker = new KWorker();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                ThreadPool.QueueUserWorkItem(new KWorker().Response, client);

//                NetworkStream stream = client.GetStream();
//                
//                byte[] data = new byte[100];
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    int bytesRead;
//                    while ((bytesRead = stream.Read(data,0,data.Length))>0)
//                    {
//                        ms.Write(data,0,bytesRead);
//                        Console.WriteLine(stream.DataAvailable);
//                        if (!stream.DataAvailable)
//                        {
//                            break;
//                        }
//                    }
//                    Console.WriteLine("exitet while");
//                    
//                    
//                    var str = Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);
//                    Console.WriteLine(str);
//                    var resp = "HTTP/1.1 200 OK\r\n\r\nAll ok \r\n\r\n";
//                    stream.Write( Encoding.ASCII.GetBytes(resp ),0,resp.Length);
//                    stream.Flush();
//                    stream.Close();
//                    
//                }


            }
            
            
        }
        
    }
}