﻿using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Keshax
{
    public class KWorker
    {

        public void Response(object clientObj)
        {
            TcpClient client = (TcpClient) clientObj;
            NetworkStream stream = client.GetStream();
                
            byte[] data = new byte[1000];
            using (MemoryStream ms = new MemoryStream())
            {
                int bytesRead;
                while ((bytesRead = stream.Read(data,0,data.Length))>0)
                {
                    ms.Write(data,0,bytesRead);
                    
                    if (!stream.DataAvailable)
                    {
                        var str = Encoding.ASCII.GetString(ms.ToArray(), 0, (int)ms.Length);
                        var clnt = new KClient(str);
                
                        // TODO: refactor 
                        string resp = clnt.Answer();
                        stream.Write( Encoding.ASCII.GetBytes(resp),0,resp.Length);
                        stream.Flush();
                        stream.Close();
                        break;
                    }
                }

                    
            }
            
        }
        
    }
}