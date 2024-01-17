﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace draft_assignment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Initialising the diff generic collection used

            // Creating Queue (First-In-First-Out)
            Queue<Customer> regularqueue = new Queue<Customer>();
            Queue<Customer> Goldquue = new Queue<Customer>();

            // Creating a dictionary, to store customer information
            Dictionary<int, Customer> customerDic = new Dictionary<int, Customer>();
            ReadCustomerCSV(customerDic);

            // Creating a dictionary, to store Order information
            Dictionary<int, Order> OrderDic = new Dictionary<int, Order>();
            

            while (true)
            {
                // Call the display the method in while loop
                DisplayMenu();
                Console.Write(" Enter your option: ");
                // Exceptional handling (check for invalid options)
                try
                {
                    int opt = int.Parse(Console.ReadLine());
                    if (opt >= 0 && opt <= 8)
                    {
                        if (opt == 0)
                        {
                            Console.WriteLine(" Bye! Have a great day");
                            break;
                        }
                        else if (opt == 1)
                        {

                            ListAllCustomers(customerDic);
                        }
                        else if (opt == 2)
                        {
                            ListAllCurrentOrders(customerDic);

                        }
                        else if (opt == 3)
                        {
                            AddCustomerToCSV();
                        }
                        else if (opt == 4)
                        {
                            // Call method for option 4
                        }
                        else if (opt == 5)
                        {
                            // Call method for option 5
                        }
                        else if (opt == 6)
                        {
                            // Call method for option 6
                        }
                        else if (opt == 7)
                        {
                            // Call method for option 7
                        }
                        else if (opt == 8)
                        {
                            // Call method for option 8
                        }

                    }
                    else
                    {
                        Console.WriteLine(" Invalid option! Enter a option from the display shown,");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine(" Invalid input! Please enter a valid integer between 0 and 8");

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input! Please enter a valid integer.");
                }

            }

        }
        // Display MenU 
        public static void DisplayMenu()
        {
            Console.WriteLine("----------------M E N U------------------");
            Console.WriteLine(" [1] List all customers");
            Console.WriteLine(" [2] List all current orders");
            Console.WriteLine(" [3] Register a new customer");
            Console.WriteLine(" [4] Create a customer's order");
            Console.WriteLine(" [5] Display order details of a customer");
            Console.WriteLine(" [6] Modify order details");
            Console.WriteLine(" [7] Process an order and checkout");
            Console.WriteLine(" [8] Display monthly charged amounts breakdown & total charged amount for the year");
            Console.WriteLine(" [0] Exit");
            Console.WriteLine("-----------------------------------------");
        }


        // Read and store data from customer.csv file. (Dictionary)
        public static void ReadCustomerCSV(Dictionary<int, Customer> customerDic)
        {
            using (StreamReader sr = new StreamReader("customers.csv"))
            {
                // Handle a exception if file not found
                try
                {
                    string s = sr.ReadLine();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 6)
                        {

                            string name = parts[0];
                            int id = int.Parse(parts[1]);
                            DateTime dob = DateTime.Parse(parts[2]);
                            string tier = parts[3];
                            int points = int.Parse(parts[4]);
                            int punchCard = int.Parse(parts[5]);
                            Customer customer = new Customer(name, id, dob);
                            customer.Rewards = new PointCard(points, punchCard);
                            customer.Rewards.Tier = tier;
                            customerDic.Add(id, customer);
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Error reading file.");
                }
            }
        }

        // Read and store data from order.csv file. (Dictionary)
        public static void ReadOrderCSV(Dictionary<int, Order> OrderDic)
        {
            try
            {
                using (StreamReader sr = new StreamReader("orders.csv"))
                {
                    string s = sr.ReadLine(); // Read and discard header line

                    Console.WriteLine($"{"Id",-10} {"Option",-10} {"Flavour1",-15} {"Flavour2",-15} {"Flavour3",-15} {"Topping1",-15} {"Topping2",-15} {"Topping3",-15} {"Topping4",-15}");

                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');

                        int Id = int.Parse(parts[0]);
                        string option = parts[4];
                        string flavour1 = parts[8];
                        string flavour2 = parts[9];
                        string flavour3 = parts[10];
                        string topping1 = parts[11];
                        string topping2 = parts[12];
                        string topping3 = parts[13];
                        string topping4 = parts[14];

                        Console.WriteLine($"{Id,-10} {Id,-10} {option,-10} {flavour1,-15} {flavour2,-15} {flavour3,-15} {topping1,-15} {topping2,-15} {topping3,-15} {topping4,-15}");

                        // Create instances of IceCream and Order as per your existing logic

                        // ... (unchanged code)
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error parsing data. Check the format in the CSV file.");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Error reading data. Make sure each line in the CSV file has the correct number of columns.");
            }
            catch (Exception)
            {
                Console.WriteLine("Error reading file.");
            }
        }


        // Option 1 of listing the customers
        public static void ListAllCustomers(Dictionary<int, Customer> customerDic)
        {
            Console.WriteLine($"{"Name",-10} {"ID",-10} {"DOB",-15} {"Membership",-20} {"Points",-10} {"PunchCard",-10}");
            foreach (Customer customer in customerDic.Values)
            {
                Console.WriteLine(customer.ToString() + $" {customer.Rewards.Tier,-20} {customer.Rewards.Points,-10} {customer.Rewards.PunchCard,-10}");
            }
        }
        // Option 2 of listing the customers' order
        public static void ListAllCurrentOrders(Dictionary<int, Customer> customerDic)
        {
            foreach (Customer customer in customerDic.Values)
            {
                if (customer.CurrentOrder != null)
                {
                    string membershipStatus = customer.Rewards.Tier;

                    Console.WriteLine($"Customer Name: {customer.Name}");
                    Console.WriteLine($"Member ID: {customer.Memberid}");
                    Console.WriteLine($"DOB: {customer.Dob.ToString("dd/MM/yyyy")}");

                    if (membershipStatus == "Gold")
                    {
                        Console.WriteLine("Gold Member Queue:");
                    }
                    else
                    {
                        Console.WriteLine("Regular Member Queue:");
                    }

                    foreach (IceCream iceCream in customer.CurrentOrder.IceCreamList)
                    {
                        Console.WriteLine(iceCream.ToString());
                        Console.WriteLine(); // Move to the next line for the next order
                    }
                }
            }
        }
        // Option 3  Register a new customer
        public static void AddCustomerToCSV()
        {
            while (true)
            {
                try
                {
                    // Getting user credentials 
                    Console.Write(" Please enter your name: ");
                    string name = Console.ReadLine();
                    Console.Write(" Please enter id number: ");
                    int memberid = int.Parse(Console.ReadLine());
                    if (memberid < 0)
                    {
                        Console.WriteLine(" Please a enter a valid MemeberID that is greater than 0\n");
                        continue;
                    }
                    Console.Write(" Please enter your date of birth (MM/dd//yyyy): ");
                    DateTime dob = DateTime.Parse(Console.ReadLine());
                    if (dob > DateTime.Now)
                    {
                        Console.WriteLine(" Please enter a valid date of birth.\n");
                        continue;
                    }
                    // Creating a new customer
                    Customer newCustomer = new Customer(name, memberid, dob);
                    PointCard newPointCard = new PointCard(0, 0);
                    newCustomer.Rewards = newPointCard;
                    // Intialisng path for customers.csv
                    string filePath = "customers.csv";
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        // Format the customer information as a CSV line
                        string customerLine = $"{newCustomer.Name},{newCustomer.Memberid},{newCustomer.Dob:MM/dd/yyyy},{newCustomer.Rewards.Tier},{newCustomer.Rewards.Points},{newCustomer.Rewards.PunchCard}";
                        sw.WriteLine(customerLine);
                    }
                    Console.WriteLine(" Successfully created");
                }
                catch (FormatException)
                {
                    Console.WriteLine(" Please enter a valid input.");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine(" Customer CSV file not found. Please make sure the file exists.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error occurred: {ex.Message}");
                }
            }
        }
    }
}
