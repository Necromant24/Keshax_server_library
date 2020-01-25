﻿using System;
using System.Collections.Generic;
 using System.IO;
 using System.Text.RegularExpressions;

namespace Keshax
{
    public delegate string action(KClient client);
    public class Controller
    {
        public struct MyStruct
        {
            public Delegate delAct;
            public string name;
            public MyStruct(string varname, Delegate del)
            {
                this.name = varname;
                delAct = del;
            }
        }

        public static Dictionary<string, MyStruct> roadMap = new Dictionary<string, MyStruct>();
        
        
        public static void  Post(string road, Delegate del)
        {
            string varname ="";
            if (road.Contains("{"))
            {
                varname = road.Split('{')[1].Split('}')[0];
                road = road.Split('{')[0];
            }
            
            roadMap.Add("POST "+road,new MyStruct(varname,del));
        }
        
        
        
        
        public static void Get(string road, Delegate del)
        {
            string varname ="";
            if (road.Contains("{"))
            {
                varname = road.Split('{')[1].Split('}')[0];
                road = road.Split('{')[0];
            }
            
            roadMap.Add("GET "+road,new MyStruct(varname,del));
        }


        public static void Static(string path)
        {
            Controller.Get("/static/{road}",(action) delegate(KClient client)
            {
                var file = "";
                try
                {
                    file = File.ReadAllText(path + client.GetRoadParam());
                }
                catch (Exception e)
                {
                    file = "404 not found!";
                }
                    
                    
                return client.PlainText(file);
            });
            
        }
        
        
        

        public static string GetRoad(string url)
        {
            string road = "GET /notfound";
            foreach (var kroad in roadMap.Keys)
            {
                var regex = new Regex(@"^" + @url + @"\w*");
                if (regex.IsMatch(kroad))
                {
                    road = kroad;
                    break;
                }
                
            }

            return road;
        } 
        

    }
}