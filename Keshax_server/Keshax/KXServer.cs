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
            
            Controller.Get("/notfound",(action)delegate(KClient client) 
            {
                return client.HTML("<h1>404!</h1>");
            });
            
            TcpListener listener = new TcpListener(IPAddress.Parse("0.0.0.0"),5000);
            listener.Start();
            
            Console.WriteLine("started on: http://"+"localhost:5000");
            
            ThreadPool.SetMinThreads(2, 2);
            ThreadPool.SetMaxThreads(3, 3);
            
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(new KWorker().Response, client);
            }
            
        }
    }
}