using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Keshax;

namespace Keshax_server
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            var server = new KXServer();
            
            Controller.Get("/test/{param}",(action)delegate(KClient client) 
            {
                return client.HTML("<h1>works! "+client.GetRoadParam()+"</h1>");
            });
            
            Controller.Post("/post",(action) delegate(KClient client)
            {
                    Console.WriteLine(client.RawBody +" is rawbody");
                    return client.Json(new Dictionary<string, string>() {{"message", "ok"}});
            });
            
            // full path to your file serve directory
            // Controller.Static("C:/Users/асеr/Desktop/static/");
            
            
            server.serve();
            
        }
        
    }
}