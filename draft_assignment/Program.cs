using System;
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
            ReadOrderCSV(OrderDic);

            while (true)
            {
                // Call the display the method in while loop
                DisplayMenu();
                Console.Write("Enter your option: ");
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
                            ListAllCustomersOrder(OrderDic);

                        }
                        else if (opt == 3)
                        {
                            // Call method for option 4
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
            using (StreamReader sr = new StreamReader("orders.csv"))
            {
                // Handle a exception if file not found
                try
                {
                    string s = sr.ReadLine();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 15)
                        {

                            int Id = int.Parse(parts[0]);
                            int memberId = int.Parse(parts[1]);
                            DateTime timereceived = DateTime.Parse(parts[2]);
                            DateTime timefulfilled = DateTime.Parse(parts[3]);
                            string option = parts[4];
                            int scoops = int.Parse(parts[5]);
                            bool dipped = bool.Parse(parts[6]);
                            string waffleflavour = parts[7];
                            List<Flavour> flavours = new List<Flavour>
                            {
                                new Flavour(parts[8],Ispremium(parts[8]),1),
                                new Flavour(parts[9],Ispremium(parts[8]),1),
                                new Flavour(parts[10],Ispremium(parts[8]),1),
                            };
                            List<Topping> toppings = new List<Topping>
                            {
                                new Topping(parts[11]),
                                new Topping(parts[12]),
                                new Topping(parts[13]),
                                new Topping(parts[14]),
                            };
                            IceCream iceCream;
                            if (option == "Cup")
                            {
                                iceCream = new Cup(option, scoops, flavours, toppings);
                            }
                            else if (option == "Cone")
                            {
                                iceCream = new Cone(option, scoops, flavours, toppings, dipped);
                            }
                            else if (option == "Waffle")
                            {
                                iceCream = new Waffle(option, scoops, flavours, toppings, waffleflavour);
                            }
                            else
                            {
                                Console.WriteLine("Invalid Option");
                                continue;
                            }

                            Order order = new Order(Id, timereceived)
                            {
                                TimeFulfilled = timefulfilled
                            };

                            order.AddIceCream(iceCream);
                            OrderDic.Add(Id,order);

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
        //  Check if the flavour is premium
        public static bool Ispremium(string flavour)
        {
            return flavour.Contains("Durian") || flavour.Contains("UBi") || flavour.Contains("Sea Salt");
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
        public static void ListAllCustomersOrder(Dictionary<int, Order> OrderDic)
        {
            foreach (Order order in OrderDic.Values)
            {
                Console.WriteLine(order.ToString());
            }
        }


    }
}
