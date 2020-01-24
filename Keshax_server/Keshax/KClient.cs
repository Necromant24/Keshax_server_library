﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Keshax
{
    public class KClient
    {
        
        private string response = "";
        private string rawText = "";
        private string rawHeaders = "";
        private string rawBody = "";
        private string firstLine = "";
        private string roadParam = "";
        
        
        // all client headers data structure
        Dictionary<string,string> clientHeaders = new Dictionary<string, string>();
        
        string httpHeader200 = "HTTP/1.1 200 ok" + "\r\n";
        string contentHTML = "Content-Type: text/html"+"\r\n";
        string contentJson = "Content-Type: text/json"+"\r\n";
        string contentText = "Content-Type: text/plain"+"\r\n";
        
        
        string contentLength = "Content-Length: ";
        string endHeader = "\r\n";
        string endAnswer = "\r\n\r\n";

        public action makeResponse;

        public string Road
        {
            get { return firstLine.Split()[1]; }  
        }

        public string RoadParam => roadParam;

        public string RawText
        {
            get => rawText;
            set => rawText = value;
        }

        public string RawBody
        {
            get => rawBody;
            set => rawBody = value;
        }

        public string FirstLine => firstLine;

        public KClient(string rawData)
        {
            parseRequest(rawData);
            Console.WriteLine(firstLine + " - is 1st line");
            Console.WriteLine(GetRoad(firstLine) + " - is getted road");
            SetResponser(Controller.roadMap[GetRoad(firstLine)].delAct);
            roadParam = Controller.roadMap[GetRoad(firstLine)].name;

        }

        public string AdditionalRoad(string pattern)
        {
            string s = roadParam;
            int i = roadParam.IndexOf("{");

            return s;
        }


        public string GetRoadParam()
        {
            int i = GetRoad(firstLine).Length;
            var s = firstLine[i..];
            return s.Split()[0];
        }

        
        
        
        
        
        public string GetHeader(string name)
        {
            return clientHeaders[name];
        }

        
        public string GetRoad(string url)
        {
                string road = "GET /notfound";
                try
                {
                    string[] roadParts = url.Split();
                    url = roadParts[0]+" "+roadParts[1];
                    Console.WriteLine(url);
                    Console.WriteLine("-----------");
                    foreach (var kroad in Controller.roadMap.Keys)
                    {
                        Console.WriteLine(kroad);
                        var reg = new Regex(@"^"+@kroad+@".*");
                        if (reg.IsMatch(url))
                        {
                            road = kroad;
                            Console.WriteLine("matched");
                            break;
                        }

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
//
//                if (road == "GET /notfound")
//                {
//                    Console.WriteLine("!!!+++++qwert");
//                }
//                else
//                {
//                    Console.WriteLine("----!!!!!----");
//                }

                return road;
        } 

        // TODO: make funcs for header usage

        public void SetResponser(Delegate del)
        {
            makeResponse =(action)del;
        }
        
        public string Json(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return httpHeader200 + contentJson + contentLength + json.Length + endAnswer + json + endAnswer;
        }
        
        public string Json<Tkey, Tval>(Dictionary<Tkey, Tval> dict)
        {
            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            return httpHeader200 + contentJson + contentLength + json.Length + endAnswer + json + endAnswer;
        }
        
        
        private void parseRequest(string rawText)
        {
            this.rawText = rawText;
            string[] data = rawText.Split("\r\n\r\n".ToCharArray());
            
            //rawBody = data[1];
            if (data.Length > 1)
            {
                rawBody = data[1];
            }
            
            string rawHeaders = data[0];
            string[] masHeaders = rawHeaders.Split("\r\n".ToCharArray());
            firstLine = masHeaders[0]; 
            for (int i = 1; i < masHeaders.Length; i++)
            {
                string[] name_Header = masHeaders[i].Split(':');
                clientHeaders[name_Header[0]] = name_Header[1].Trim();
            }
            
        }
        
        public string PlainText(string text)
        {
            return httpHeader200 + contentText + contentLength + text.Length + endAnswer + text + endAnswer;
        }
        

        public string HTML(string html)
        {
            return httpHeader200 + contentHTML + contentLength + html.Length + endAnswer + html + endAnswer;
        }

        public string Answer()
        {
            return makeResponse(this);
        }


    }
}