﻿using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerDatabaseAPI.Models;

namespace BusinessLayerAPI.Controllers
{
    [RoutePrefix("api/data")]
    public class GenerateDataController : ApiController
    {
        private static string URL = "http://localhost:50915/";
        private static RestClient client = new RestClient(URL);
        Random rand = new Random();
        private string[] firstnames = { "Jack", "Chris", "Matthew", "Ryan", "Michael", "Steven" };
        private string[] lastnames = { "Love", "Brown", "Green", "McCarter", "O'Brian", "Griffith" };
        private uint[] pins = { 1111, 2222, 3333, 4444, 5555, 6666 };
        private uint[] acctNos = { 000001, 000002, 000003, 000004, 000005, 000006 };
        private int[] balances = { -100, 0, 2000, 3000, 50000, 100000 };
        private string[] profPicPaths = { "~/Content/profpic1.jpg", "~/Content/profpic2.jpg", "~/Content/profpic3.jpg", "~/Content/profpic4.jpg", "~/Content/profpic5.jpg", "~/Content/profpic6.jpg"};

        [Route("generate")]
        [HttpGet]
        public string GenerateData()
        {
            string test = "Fail";
            for (int i = 0; i < 100; i++)
            {
                RestRequest request = new RestRequest("api/customers", Method.Post);
                
                Customer customer = GetNextAccount(i);
                request.AddJsonBody(JsonConvert.SerializeObject(customer));
                RestResponse response = client.Execute(request);

                Customer returnCustomer = JsonConvert.DeserializeObject<Customer>(response.Content);
                if (returnCustomer != null)
                {
                    test = "Success";
                }
            }
            return test;
        }

        public Customer GetNextAccount(int i)
        {
            Customer customer = new Customer();
            customer.PinNumber = pins[GenerateRandNum()].ToString();
            customer.AccountNumber = i+1;
            customer.FirstName = firstnames[GenerateRandNum()];
            customer.LastName = lastnames[GenerateRandNum()];
            customer.Balance = balances[GenerateRandNum()];
            return customer;
        }

        private int GenerateRandNum()
        {
            return rand.Next(0, 5);
        }
    }
}
