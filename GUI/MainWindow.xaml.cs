﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomerDatabaseAPI.Models;
using Newtonsoft.Json;
using RestSharp;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string URL = "http://localhost:53590/";
        private static RestClient client = new RestClient(URL);
        public MainWindow()
        {
            InitializeComponent();
            
            
        }

        private void SearchByAcctNumBtn_Click(object sender, RoutedEventArgs e)
        {
            RestRequest request = new RestRequest("api/search/{id}");
            request.AddUrlSegment("id", SearchBox.Text);
            RestResponse response = client.Execute(request);

            Customer customer = JsonConvert.DeserializeObject<Customer>(response.Content);
            PopulateCustomerData(customer);
            // If statement, if customer is null
        }

        private void GenerateDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            RestRequest request = new RestRequest("api/data/generate");
            RestResponse response = client.Execute(request);
            MessageBox.Show("Data Successfully Generated");
        }

        private void PopulateCustomerData(Customer customer)
        {
            AcctNumBox.Text = customer.AccountNumber.ToString();
            FirstNameBox.Text = customer.FirstName;
            LastNameBox.Text = customer.LastName;
            BalanceBox.Text = customer.Balance.ToString();
            Pin_Number_Box.Text = customer.PinNumber;
        }

        private void AddCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            RestRequest request = new RestRequest("api/search", Method.Post);
            Customer customer = CreateCustomerByInput();
            request.AddJsonBody(JsonConvert.SerializeObject(customer));
            RestResponse response = client.Execute(request);

            Customer returnCustomer = JsonConvert.DeserializeObject<Customer>(response.Content);
            
        }

        private void DeleteByAcctNumBtn_Click(object sender, RoutedEventArgs e)
        {
            RestRequest request = new RestRequest("api/search/{id}", Method.Delete);
            request.AddUrlSegment("id", DeleteBox.Text);
            RestResponse response = client.Execute(request);

            Customer customer = JsonConvert.DeserializeObject<Customer>(response.Content);
        }

        private void UpdateCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            int id = Int32.Parse(AcctNumBox.Text);
            Customer customer = CreateCustomerByInput();

            RestRequest request = new RestRequest("api/search/{id}", Method.Put);
            request.AddUrlSegment("id", id);
            request.AddJsonBody(JsonConvert.SerializeObject(customer));
            RestResponse response = client.Execute(request);

            Customer returnCustomer = JsonConvert.DeserializeObject<Customer>(response.Content);
        }
        private Customer CreateCustomerByInput()
        {
            Customer customer = new Customer();
            customer.AccountNumber = Int32.Parse(AcctNumBox.Text);
            customer.FirstName = FirstNameBox.Text;
            customer.LastName = LastNameBox.Text;
            customer.PinNumber = Pin_Number_Box.Text;
            customer.Balance = Int32.Parse(BalanceBox.Text);
            return customer;
        }

        
    }
}