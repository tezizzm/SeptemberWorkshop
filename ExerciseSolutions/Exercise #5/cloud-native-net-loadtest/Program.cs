﻿using System;
using System.Net.Http;

namespace loadtest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing load ...");

            HttpClient c = new HttpClient();

            for(int i = 0; i < 2500; i++)
            {
                var result = c.GetAsync("https://core-cf-microservice-martez.apps.pcfone.io//api/products").Result; 
                Console.WriteLine("calling API - " + DateTime.Now.ToString());
                
                System.Threading.Thread.Sleep(100);
            }   

            Console.WriteLine("done calling API");
        }
    }
}
