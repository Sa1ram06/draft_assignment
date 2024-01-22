using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
            Queue<Customer> Goldqueue = new Queue<Customer>();

            // Creating a dictionary, to store customer information
            Dictionary<int, Customer> customerDic = new Dictionary<int, Customer>();
            ReadCustomerCSV(customerDic);
            ReadOrderCSV(customerDic);

            while (true)
            {
                // Call the display the method in while loop
                DisplayMenu();
                Console.Write(" Enter your option: ");
                // Exceptional handling (check for invalid options)
                try
                {
                    int opt = int.Parse(Console.ReadLine());
                    Console.WriteLine(" -----------------------------------------");
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
                            ListAllCurrentOrders(customerDic, regularqueue, Goldqueue);

                        }
                        else if (opt == 3)
                        {
                            AddCustomerToCSV(customerDic);
                        }
                        else if (opt == 4)
                        {
                            NewCustomerOrder(customerDic, regularqueue, Goldqueue);
                        }
                        else if (opt == 5)
                        {
                            DisplayOrderDetails(customerDic);
                        }
                        else if (opt == 6)
                        {
                            ModifyOrderDetails(customerDic, regularqueue, Goldqueue);
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
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

            }

        }
        // Display MenU 
        public static void DisplayMenu()
        {
            Console.WriteLine("\n ----------------M E N U------------------");
            Console.WriteLine(" [1] List all customers");
            Console.WriteLine(" [2] List all current orders");
            Console.WriteLine(" [3] Register a new customer");
            Console.WriteLine(" [4] Create a customer's order");
            Console.WriteLine(" [5] Display order details of a customer");
            Console.WriteLine(" [6] Modify order details");
            Console.WriteLine(" [7] Process an order and checkout");
            Console.WriteLine(" [8] Display monthly charged amounts breakdown & total charged amount for the year");
            Console.WriteLine(" [0] Exit");
            Console.WriteLine(" -----------------------------------------");
        }


        // Read and store data from customer.csv file. (Dictionary)
        public static void ReadCustomerCSV(Dictionary<int, Customer> customerDic)
        {
            using (StreamReader sr = new StreamReader("draft_customers.csv"))
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
                            int id = Convert.ToInt32(parts[1]);
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
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($" File not found: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file: {ex.Message}");
                }
            }
        }

        // Read and store data from order.csv file. (Dictionary)
        public static void ReadOrderCSV(Dictionary<int, Customer> customerDic)
        {
            try
            {
                using (StreamReader sr = new StreamReader("draft_orders.csv"))
                {
                    string s = sr.ReadLine(); // Read and discard header line
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 15)
                        {
                            List<Flavour> orderedFlavourlist = new List<Flavour>();
                            List<Topping> orderedtoppinglist = new List<Topping>();
                            int orderId = int.Parse(parts[0]);
                            int memberId = int.Parse(parts[1]);
                            DateTime timerecieved = DateTime.Parse(parts[2]);
                            DateTime timefullfiled = DateTime.Parse(parts[3]);
                            string option = parts[4];
                            int scoops = int.Parse(parts[5]);
                            bool dipped;
                            string waffleFlavour = null;
                            List<string> flavours = new List<string> { parts[8], parts[9], parts[10] };
                            List<string> toppings = new List<string> { parts[11], parts[12], parts[13], parts[14] };
                            List<string> regularflavours = new List<string> { "VANILLA", "CHOCOLATE", "STRAWBERRY" };
                            List<string> premiumflavours = new List<string> { "DURIAN", "UBE", "SEA SALT" };
                            bool ispremium = false;
                            // Checking if the flavours are premium
                            foreach (string flavour in flavours)
                            {
                                if (!string.IsNullOrEmpty(flavour))
                                {
                                    if (premiumflavours.Contains(flavour.ToUpper()))
                                    {
                                        ispremium = true;
                                    }
                                    else
                                    {
                                        ispremium = false;
                                    }
                                    orderedFlavourlist.Add(new Flavour(flavour, ispremium, 1));
                                }
                            }

                            foreach (string topping in toppings)
                            {
                                if (!string.IsNullOrEmpty(topping))
                                {
                                    orderedtoppinglist.Add(new Topping(topping));
                                }
                                   
                            }
                            // Creating new IceCream
                            Order order = new Order(orderId, timerecieved)
                            {
                                TimeFulfilled = timefullfiled
                            };

                            if (option == "Cup")
                            {
                                IceCream icecream = new Cup(option, scoops, orderedFlavourlist, orderedtoppinglist);
                                order.AddIceCream(icecream);
                            }
                            else if (option == "Cone")
                            {
                                dipped = bool.Parse(parts[6]);
                                IceCream icecream = new Cone(option, scoops, orderedFlavourlist, orderedtoppinglist, dipped);
                                order.AddIceCream(icecream);
                            }
                            else if (option == "Waffle")
                            {
                                waffleFlavour = parts[7];
                                IceCream icecream = new Waffle(option, scoops, orderedFlavourlist, orderedtoppinglist, waffleFlavour);
                                order.AddIceCream(icecream);
                            }
                            // Check for appropriate customer to add the order
                            if (customerDic.ContainsKey(memberId))
                            {
                                Customer customer = customerDic[memberId];
                                customer.OrderHistory.Add(order);

                            }

                        }

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
            catch (Exception)
            {
                Console.WriteLine("Error reading file.");
            }
        }


        // Option 1 of listing the customers
        public static void ListAllCustomers(Dictionary<int, Customer> customerDic)
        {
            Console.WriteLine($" {"Name",-10} {"ID",-10} {"DOB",-15} {"Membership",-20} {"Points",-10} {"PunchCard",-10}");
            foreach (Customer customer in customerDic.Values)
            {
                Console.WriteLine(customer.ToString() + $" {customer.Rewards.Tier,-20} {customer.Rewards.Points,-10} {customer.Rewards.PunchCard,-10}");
            }
        }
        // Option 2 of listing the customers' order
        public static void ListAllCurrentOrders(Dictionary<int, Customer> customerDic, Queue<Customer> regularQueue, Queue<Customer> goldQueue)
        {

            DisplayOrdersFromQueue("Regular Member Queue", regularQueue);
            DisplayOrdersFromQueue("Gold Member Queue", goldQueue);
        }
        public static void DisplayOrdersFromQueue(string queueType, Queue<Customer> queue)
        {
            
            if (queue.Count > 0)
            {
                Console.WriteLine($"\n --------------------------------{queueType}----------------------------------");
                foreach (Customer customer in queue)
                {
                    Console.WriteLine($" ------------------------Order #{customer.CurrentOrder.Id}------------------------------");
                    Console.WriteLine($" Customer Name: {customer.Name}");
                    Console.WriteLine($" Member ID: {customer.Memberid}");
                    Console.WriteLine($" DOB: {customer.Dob.ToString("dd/MM/yyyy")}");

                    int iceCreamNumber = 1;
                    foreach (IceCream iceCream in customer.CurrentOrder.IceCreamList)
                    {
                        Console.WriteLine($"-----#{iceCreamNumber} IceCream-------");
                        Console.WriteLine(iceCream.ToString());
                        iceCreamNumber++;
                        Console.WriteLine("-----------------------");
                    }

                    Console.WriteLine($" -------------------------------------------------------------------------");
                    
                }
            }
            else
            {
                Console.WriteLine($" Currently no orders in the {queueType}");
            }
        }

        // Option 3  Register a new customer
        public static void AddCustomerToCSV(Dictionary<int, Customer> customerDic)
        {
            while (true)
            {
                try
                {
                    // Getting user credentials 
                    Console.Write(" Please enter your name: ");
                    string name = Console.ReadLine();
                    int memberid;
                    DateTime dob;
                    while (true)
                    {
                        Console.Write(" Please enter member id number: ");
                        memberid = int.Parse(Console.ReadLine());
                        if (memberid < 0)
                        {
                            Console.WriteLine(" Please a enter a valid MemeberID that is greater than 0\n");
                            continue;
                        }
                        else if (customerDic.ContainsKey(memberid))
                        {
                            Console.WriteLine(" Member ID have been already used. \n");
                            continue;
                        }
                        break;
                    }
                    while (true)
                    {
                        Console.Write(" Please enter your date of birth (MM/dd//yyyy): ");
                        dob = DateTime.Parse(Console.ReadLine());
                        if (dob > DateTime.Now)
                        {
                            Console.WriteLine(" Please enter a valid date of birth.\n");
                            continue;
                        }
                        break;
                    }
                    // Creating a new customer
                    Customer newCustomer = new Customer(name, memberid, dob);
                    PointCard newPointCard = new PointCard(0, 0)
                    {
                        Tier = "Ordinary"
                    };
                    newCustomer.Rewards = newPointCard;
                    customerDic.Add(memberid, newCustomer);

                    // Intialisng path for customers.csv
                    string filePath = "draft_customers.csv";
                    using (StreamWriter sw = new StreamWriter(filePath, true))
                    {
                        // Format the customer information as a CSV line
                        string customerLine = $"{newCustomer.Name},{newCustomer.Memberid},{newCustomer.Dob:dd/MM/yyyy},{newCustomer.Rewards.Tier},{newCustomer.Rewards.Points},{newCustomer.Rewards.PunchCard}";
                        sw.WriteLine(customerLine);
                    }
                    Console.WriteLine(" Successfully created");
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine(" Please enter a valid input.\n");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine(" Customer CSV file not found. Please make sure the file exists.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }
            }
        }
        // Option 4 Create a customer’s order
        public static void NewCustomerOrder(Dictionary<int, Customer> customerDic, Queue<Customer> regularQueue, Queue<Customer> goldQueue)
        {
            while (true)
            {
                try
                {
                    // Step 1: List the customers from the customers.csv
                    ListAllCustomers(customerDic);

                    Console.Write("\n Please enter the customer ID that you wish to select: ");
                    int id = int.Parse(Console.ReadLine());

                    if (customerDic.ContainsKey(id))
                    {
                        Customer customer = customerDic[id];
                        Order newOrder = customer.MakeOrder();
                        customer.CurrentOrder = newOrder;
                        if (customer.Rewards.Tier == "Gold")
                        {
                            goldQueue.Enqueue(customer);
                        }
                        else
                        {
                            regularQueue.Enqueue(customer);
                        }

                        // Break out of the loop if the order is successfully processed
                        break;
                    }
                    else
                    {
                        Console.WriteLine(" Invalid customer ID. Please enter a valid ID.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine(" Invalid input! please try again.\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }
            }
        }
        // Option 5 Display order details of customer
        public static void DisplayOrderDetails(Dictionary<int, Customer> customerDic)
        {
            ListAllCustomers(customerDic);

            Console.Write("\n Please enter the customer ID that you wish to select: ");
            int id = int.Parse(Console.ReadLine());
            if (customerDic.ContainsKey(id))
            {
                Customer customer = customerDic[id];
                Console.WriteLine($" Order details for {customer.Name} (Customer ID: {customer.Memberid})");

                foreach (Order order in customer.OrderHistory)
                {
                    Console.WriteLine($" \nOrder ID: {order.Id}");
                    foreach (IceCream iceCream in order.IceCreamList)
                    {
                        Console.WriteLine(iceCream.ToString());
                    }
                    Console.WriteLine($" Time Received: {order.TimeReceived}");

                    if (order.TimeFulfilled != null)
                    {
                        Console.WriteLine($" Time Fulfilled: {order.TimeFulfilled}");
                    }
                    else
                    {
                        Console.WriteLine(" Order not fulfilled yet.");
                    }
                }
            }
            else
            {
                Console.WriteLine(" Invalid customer ID. Please enter a valid ID.");
            }
        }
        // Option 5 Display order details of customer
        public static void ModifyOrderDetails(Dictionary<int, Customer> customerDic, Queue<Customer> regularQueue, Queue<Customer> goldQueue)
        {
            ListAllCustomers(customerDic);
            Console.Write("\n Please enter the customer ID that you wish to select: ");
            int id = int.Parse(Console.ReadLine());
            if (customerDic.ContainsKey(id))
            {
                Customer customer = customerDic[id];
                Order orders = customer.CurrentOrder;
                if (orders != null)
                {
                    int iceCreamNumber = 1;
                    foreach (IceCream iceCream in customer.CurrentOrder.IceCreamList)
                    {
                        Console.WriteLine($"\n-----#{iceCreamNumber} IceCream-------");
                        Console.WriteLine(iceCream.ToString());
                        iceCreamNumber++;
                        Console.WriteLine("-----------------------\n");
                        DisplayMenuOpt6();
                        Console.Write(" Enter the option: ");
                        int opt = int.Parse(Console.ReadLine());
                        if (opt == 0)
                        {
                            break;
                        }
                        else if (opt == 1)
                        {
                           
                        }
                        else if (opt == 2)
                        {
                            Order newOrder = customer.MakeOrder();
                            customer.CurrentOrder = newOrder;
                            if (customer.Rewards.Tier == "Gold")
                            {
                                goldQueue.Enqueue(customer);
                            }
                            else
                            {
                                regularQueue.Enqueue(customer);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($" {customer.Name} has no current orders.");
                }
            }
            else
            {
                Console.WriteLine(" Invalid customer ID. Please enter a valid ID.");
            }
        }
        public static void DisplayMenuOpt6()
        {
            Console.WriteLine(" [1] Modify an IceCream");
            Console.WriteLine(" [2] Add new an IceCream");
            Console.WriteLine(" [3] Delete an IceCream");
            Console.WriteLine(" [4] Exit");

        }
    }
}
