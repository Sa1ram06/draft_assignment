using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Tracing;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
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

            // Creating Two Queue (First-In-First-Out)
            Queue<Customer> regularqueue = new Queue<Customer>();
            Queue<Customer> Goldqueue = new Queue<Customer>();

            // Creating Dictionary to store different flavours, toppings and its costs
            Dictionary<string, int> AvailableFlavourDic = new Dictionary<string, int>();
            Dictionary<string, double> AvailableToppingsDic = new Dictionary<string, double>();
            ReadFlavourrCSV(AvailableFlavourDic);
            ReadToppingsCSV(AvailableToppingsDic);

            // Creating a dictionary, to store customer information
            Dictionary<int, Customer> customerDic = new Dictionary<int, Customer>();
            ReadCustomerCSV(customerDic);
            ReadOrderCSV(customerDic, AvailableFlavourDic);


            while (true)
            {
                // Call the display the method in while loop
                DisplayMenu();
                Console.Write(" Enter your option: ");
                // Exceptional handling (check for invalid options)
                try
                {
                    // Collecting the user input option
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
                            NewCustomerOrder(customerDic, regularqueue, Goldqueue, AvailableFlavourDic, AvailableToppingsDic);
                        }
                        else if (opt == 5)
                        {
                            DisplayOrderDetails(customerDic);
                        }
                        else if (opt == 6)
                        {
                            ModifyOrderDetails(customerDic, AvailableFlavourDic, AvailableToppingsDic);
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
                catch (FormatException) // Exception Handling 
                {
                    Console.WriteLine(" Invalid input! Please enter a valid integer between 0 and 8");

                }
                catch (Exception ex) // Exception Handling 
                {
                    Console.WriteLine($"{ex.Message}");
                }

            }

        }
        // Display Menu 
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

                try
                {
                    string s = sr.ReadLine(); // skipping the header of the csv file
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(','); // splitting by comma
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
                catch (FileNotFoundException ex) // Handle a exception if file not found
                {
                    Console.WriteLine($" File not found: {ex.Message}");
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine("Error parsing data. Check the format in the CSV file.");
                }
                catch (Exception ex) // General exception handling
                {
                    Console.WriteLine($"Error reading file: {ex.Message}");
                }
            }
        }

        // Read and store data from order.csv file. (Dictionary)
        public static void ReadOrderCSV(Dictionary<int, Customer> customerDic, Dictionary<string, int> availableflavoursdic)
        {
            try
            {
                using (StreamReader sr = new StreamReader("draft_orders.csv"))
                {
                    string s = sr.ReadLine(); // skipping the header line
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 15)
                        {
                            List<Flavour> orderedFlavourlist = new List<Flavour>(); // initialising empty list for ordered flavours
                            List<Topping> orderedtoppinglist = new List<Topping>(); // initialising empty list for ordered toppings
                            int orderId = int.Parse(parts[0]);
                            int memberId = int.Parse(parts[1]);
                            DateTime timerecieved = DateTime.Parse(parts[2]);
                            DateTime timefullfiled = DateTime.Parse(parts[3]);
                            string option = parts[4];
                            int scoops = int.Parse(parts[5]);
                            bool dipped;
                            string waffleFlavour = null;
                            List<string> flavours = new List<string> { parts[8], parts[9], parts[10] }; // initialising all the three flavours into a list despite being 
                            List<string> toppings = new List<string> { parts[11], parts[12], parts[13], parts[14] }; // initialising all the three flavours into a list despite being
                            List<string> regularflavours = new List<string>(); //initialising a list to hold strings of regular flavours
                            List<string> premiumflavours = new List<string>(); //initialising a list to hold strings of premium flavours
                            foreach (var kvp in availableflavoursdic) // For loop the dictionary, to identify which flavours are premium. 
                            {
                                string flavourName = kvp.Key;
                                int flavourCost = kvp.Value;

                                if (flavourCost == 2)
                                {
                                    premiumflavours.Add(flavourName); //Assuming premium flavours have $2.00, then add to premium list
                                }
                                else
                                {
                                    regularflavours.Add(flavourName); // Add to regular list if, its not premium
                                }
                            }
                            bool ispremium = false; // Initialise premium to false

                            foreach (string flavour in flavours)
                            {
                                if (!string.IsNullOrEmpty(flavour)) // Check if the value is valid (not nill/empty),
                                {
                                    if (premiumflavours.Contains(flavour.ToUpper()))
                                    {
                                        ispremium = true; //  if the flavour is found in the premium list, premium set to true
                                    }
                                    else
                                    {
                                        ispremium = false; //  if the flavour is not found in the premium list, premium set to false
                                    }
                                    orderedFlavourlist.Add(new Flavour(flavour, ispremium, 1)); // Create a new flavour
                                }
                            }

                            foreach (string topping in toppings)
                            {
                                if (!string.IsNullOrEmpty(topping))  // Check if the valid is (not nill/empty) 
                                {
                                    orderedtoppinglist.Add(new Topping(topping));  // Create a new topping
                                }

                            }

                            Order order = new Order(orderId, timerecieved)  // Creating new Order with appropriate parameters
                            {
                                TimeFulfilled = timefullfiled
                            };

                            if (option == "Cup") // Creating Cup IceCream with valid information.
                            {
                                IceCream icecream = new Cup(option, scoops, orderedFlavourlist, orderedtoppinglist);
                                order.AddIceCream(icecream); // Appending the created ice cream to the order
                            }
                            else if (option == "Cone") // Creating Cone IceCream with valid information.
                            {
                                dipped = bool.Parse(parts[6]);
                                IceCream icecream = new Cone(option, scoops, orderedFlavourlist, orderedtoppinglist, dipped);
                                order.AddIceCream(icecream); // Appending the created ice cream to the order
                            }
                            else if (option == "Waffle") // Creating Waffle IceCream with valid information.
                            {
                                waffleFlavour = parts[7];
                                IceCream icecream = new Waffle(option, scoops, orderedFlavourlist, orderedtoppinglist, waffleFlavour);
                                order.AddIceCream(icecream); // Appending the created ice cream to the order
                            }

                            if (customerDic.ContainsKey(memberId)) // Check for appropriate customer to add the order
                            {
                                Customer customer = customerDic[memberId];
                                customer.OrderHistory.Add(order); // Add the order to the specfic customer

                            }

                        }
                    }
                }
            }
            catch (FileNotFoundException) // Handle a exception if file not found
            {
                Console.WriteLine("File not found.");
            }
            catch (FormatException) // Handle format exception
            {
                Console.WriteLine("Error parsing data. Check the format in the CSV file.");
            }
            catch (Exception)// General exception handling
            {
                Console.WriteLine("Error reading file.");
            }
        }


        // Option 1 of listing the customers
        public static void ListAllCustomers(Dictionary<int, Customer> customerDic)
        {
            Console.WriteLine($" {"Name",-10} {"ID",-10} {"DOB",-15} {"Membership",-20} {"Points",-10} {"PunchCard",-10}");
            foreach (Customer customer in customerDic.Values) // Loop through the values of the dictionary (!  NOT THE KEY )
            {
                Console.WriteLine(customer.ToString() + $" {customer.Rewards.Tier,-20} {customer.Rewards.Points,-10} {customer.Rewards.PunchCard,-10}");
            }
        }


        // Option 2 of listing the customers' order
        public static void ListAllCurrentOrders(Dictionary<int, Customer> customerDic, Queue<Customer> regularQueue, Queue<Customer> goldQueue) // Call in the necessary parameters 
        {
            //Created 2 method with two parameters each to display the orders in that specific queue
            DisplayOrdersFromQueue("Regular Member Queue", regularQueue);
            DisplayOrdersFromQueue("Gold Member Queue", goldQueue);
        }
        // Sub Method for Option 2
        public static void DisplayOrdersFromQueue(string queueType, Queue<Customer> queue)
        {

            if (queue.Count > 0) // Ensure there is orders to display
            {
                Console.WriteLine($"\n --------------------------------{queueType}----------------------------------");
                foreach (Customer customer in queue) // Loop through the specified queue, to obtain customer information 
                {
                    Console.WriteLine($" ------------------------Order #{customer.CurrentOrder.Id}------------------------------"); // Order Id 
                    Console.WriteLine($" Customer Name: {customer.Name}"); // Name
                    Console.WriteLine($" Member ID: {customer.Memberid}"); // Member Id  
                    Console.WriteLine($" DOB: {customer.Dob.ToString("dd/MM/yyyy")}"); // Dob

                    int iceCreamNumber = 1; //initialise the icenumber, To keep record of how many icecreams in each order,
                    foreach (IceCream iceCream in customer.CurrentOrder.IceCreamList)
                    {
                        Console.WriteLine($" -----#{iceCreamNumber} IceCream-------");
                        Console.WriteLine(iceCream.ToString());
                        iceCreamNumber++;
                        Console.WriteLine(" -----------------------");
                    }

                    Console.WriteLine($" -------------------------------------------------------------------------");

                }
            }
            else // No orders in that specified queue, 
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
                {   // Getting user credentials 
                    string name;
                    while (true)
                    {
                        Console.Write(" Please enter your name: ");
                        name = Console.ReadLine().Trim();
                        if (name.Any(char.IsDigit))
                        {
                            Console.WriteLine(" Please provide a valid name\n");
                            continue;
                        }
                        break;
                    }
                    int memberid;
                    DateTime dob;
                    while (true)
                    {
                        Console.Write(" Please enter member id number: ");
                        memberid = int.Parse(Console.ReadLine().Trim());
                        if (memberid < 0) //Validating the memberid ( cannot be less than zero )
                        {
                            Console.WriteLine(" Please a enter a valid MemeberID that is greater than 0\n");
                            continue;
                        }
                        else if (customerDic.ContainsKey(memberid))  //Validating the memberid ( no duplicates )
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
                        if (dob > DateTime.Now)  //Validating the dob ( cannot be greater than current time )
                        {
                            Console.WriteLine(" Please enter a valid date of birth.\n");
                            continue;
                        }
                        break;
                    }
                    // Creating a new customer
                    Customer newCustomer = new Customer(name, memberid, dob);
                    PointCard newPointCard = new PointCard(); // Creating the default pointcard for the new customer
                    newCustomer.Rewards = newPointCard; // Assigning the pointcard to the new customer, through rewards attribute
                    customerDic.Add(memberid, newCustomer); // Adding the new customer to dictionary

                    // Intialisng path for customers.csv
                    string filePath = "draft_customers.csv";
                    // Writing the new customer info to the csv file
                    using (StreamWriter sw = new StreamWriter(filePath, true)) // true, (append)
                    {
                        // Format the customer information as a CSV line
                        string customerLine = $"{newCustomer.Name},{newCustomer.Memberid},{newCustomer.Dob:dd/MM/yyyy},{newCustomer.Rewards.Tier},{newCustomer.Rewards.Points},{newCustomer.Rewards.PunchCard}";
                        sw.WriteLine(customerLine);
                    }
                    Console.WriteLine($" {name} with Order Id {memberid} was created. ");
                    break;
                }
                catch (FileNotFoundException) // Handle a exception if file not found
                {
                    Console.WriteLine("File not found.");
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine(" Enter a valid input");
                }
                catch (Exception)// General exception handling
                {
                    Console.WriteLine("Error reading file.");
                }
            }
        }


        // Option 4 Create a customer’s order
        public static void NewCustomerOrder(Dictionary<int, Customer> customerDic, Queue<Customer> regularQueue, Queue<Customer> goldQueue, Dictionary<string, int> availableflavoursdic, Dictionary<string, double> availbletoppingsdic)
        {
            //Lisy all customers from the customer dictionary that stores the customer information
            ListAllCustomers(customerDic);
            while (true)
            {
                try
                {
                    Console.Write("\n Please enter the customer ID that you wish to select: ");
                    int id = int.Parse(Console.ReadLine());

                    if (customerDic.ContainsKey(id)) // Validating if input (customerid) exist in customer dictionary
                    {
                        Customer customer = customerDic[id]; // Retrieve the appropriate customer, by their id 
                        if (customer.CurrentOrder == null)
                        {
                            Order newOrder = customer.MakeOrder(); // Create a new  order.
                            customer.CurrentOrder = newOrder; // Assigning the new order to the current order
                            ProcessIceCreamOrder(newOrder, availableflavoursdic, availbletoppingsdic,customer); // Call in the function,
                        }
                        else
                        {
                            Console.WriteLine($" {customer.Name} has current order. ");
                        }
                        if (customer.Rewards.Tier == "Gold") // Check for the Customer Tier to enqueue the order to that specific queue
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
                        Console.WriteLine(" Invalid customer ID. Please enter a valid ID."); // Validating if no customer id is found, by the input.
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine(" Invalid input! please try again. "); // Handle format exception
                }
                catch (Exception ex) // General exception handling
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }
            }
        }

        // Sub Methods for Method: 01
        public static IceCream CreateIceCream(Dictionary<string, int> AvailableFlavoursDic, Dictionary<string, double> AvailableToppingssDic)
        {
            IceCream iceCream = null; // Initialising icrecream tk be null
            try
            {
                string option = GetValidIceCreamOption(); // Call the method to get a valid ice cream option
                int scoops = GetScoops(); // Obtain int scoops, from the called function

                List<Flavour> flavourlist = GetFlavours(AvailableFlavoursDic, scoops);  // Obtain List<Flavour>, from the called function
                List<Topping> toppingslist = GetToppings(AvailableToppingssDic);  // Obtain int List<Toppings>, from the called function

                //Creating options based the user option
                if (option == "CUP")
                {
                    iceCream = new Cup(option, scoops, flavourlist, toppingslist); // Creating a Cup IceCream
                }
                else if (option == "CONE")
                {
                    while (true)
                    {
                        bool dipped = GetDippedConePreference();
                        iceCream = new Cone(option, scoops, flavourlist, toppingslist, dipped); // Creating a Cone IceCream
                        break;
                    }
                }
                else if (option == "WAFFLE")
                {
                    List<string> waffleFlavoursAvailable = new List<string> { "RED VELVET", "CHARCOAL", "PANDAN" }; //Initialising the different waffle flavours
                    Console.WriteLine("\n ---------Waffle Flavour Available-------------");
                    Console.WriteLine(" Name");
                    foreach (string flavour in waffleFlavoursAvailable) //Displaying teh different waffle flavours
                    {
                        Console.WriteLine($" {flavour.ToLower()}");
                    }
                    while (true)
                    {
                        Console.WriteLine("\n ---------Waffle Flavour-------------");
                        Console.Write(" Enter the waffle flavour you would like: ");
                        string waffleFlavour = Console.ReadLine().ToUpper().Trim();

                        if (waffleFlavoursAvailable.Contains(waffleFlavour)) // Validating if waffle flavour (user input) exist.
                        {
                            iceCream = new Waffle(option, scoops, flavourlist, toppingslist, waffleFlavour);  // Creating a Waffle IceCream
                            break;
                        }
                        else
                        {
                            Console.WriteLine(" Enter a valid waffle flavour.\n"); // Error message for flavours that don't exist
                        }
                    }
                }
            }
            catch (FormatException) // Handle format exception
            {
                Console.WriteLine(" Invalid input! Please try again.");
            }
            catch (Exception ex)  // General exception handling
            {
                Console.WriteLine($" Error occurred: {ex.Message}\n");
            }
            return iceCream;
        }
        // Sub Methods for Method: 02
        private static string GetValidIceCreamOption()
        {
            string option;
            while (true)
            {
                Console.WriteLine("\n ---------Option-------------");
                Console.Write(" Enter the Ice cream option (Cup/Cone/Waffle): ");
                option = Console.ReadLine()?.ToUpper().Trim(); // Convert to Upper, user able to type in both CAPs and no Caps

                if (option == "CUP" || option == "CONE" || option == "WAFFLE") // Validating for an appropriate option
                {
                    break;
                }
                else
                {
                    Console.WriteLine(" Please enter a valid ice cream option (Cup/Cone/Waffle)."); // Error message
                }
            }
            return option;
        }
        // Sub Methods for Method: 03
        public static int GetScoops()
        {
            int scoops; //Initialising the data type for scoops
            while (true)
            {
                try
                {
                    Console.WriteLine("\n ---------Scoop-------------");
                    Console.Write(" Enter the number of scoops (not more than 3): ");
                    scoops = int.Parse(Console.ReadLine().Trim());
                    if (scoops >= 1 && scoops <= 3) // Validating the number of scoops
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine($" We don't serve {scoops} ice cream here, Please enter a valid number of scoops.");

                    }
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine(" Invalid input! Please try again.\n");
                }
                catch (Exception ex)  // General exception handling
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }

            }
            return scoops; // Return the int scoops
        }

        // Sub Methods for Method: 04
        public static List<Flavour> GetFlavours(Dictionary<string, int> availableflavoursdic, int scoops)
        {
            List<Flavour> flavourlist = new List<Flavour>(); //Initialising the List<flavour>, one of the parameters for creating the icecream, holds flavour objects
            DisplayFlavours(availableflavoursdic); //Display the available flavours
            List<string> regularflavours = new List<string>(); //initialising a list to hold strings of regular flavours
            List<string> premiumflavours = new List<string>(); //initialising a list to hold strings of premium flavours
            foreach (var kvp in availableflavoursdic) // For loop the dictionary, to identify which flavours are premium. 
            {
                string flavourName = kvp.Key;
                int flavourCost = kvp.Value;

                if (flavourCost == 2)
                {
                    premiumflavours.Add(flavourName.ToUpper()); //Assuming premium flavours have $2.00, then add to premium list
                }
                else
                {
                    regularflavours.Add(flavourName.ToUpper()); // Add to regular list if, its not premium
                }
            }
            while (true)
            {
                Console.WriteLine("\n ---------Flavours-------------");
                for (int i = 1; i <= scoops; i++)
                {
                    Console.Write($" Enter the flavour {i} you would like to add: ");
                    string flavour = Console.ReadLine().ToUpper().Trim(); //Chnage the input to upper case, compare with teh premiumflavour, regularflavour list
                    bool premium = premiumflavours.Contains(flavour);

                    if (!(regularflavours.Contains(flavour) || premiumflavours.Contains(flavour))) //Check if the input(flavour) exist.
                    {
                        Console.WriteLine(" Please provide a valid flavour.\n");
                        i--; // Decrement i to re-enter the current iteration
                        continue;
                    }
                    flavourlist.Add(new Flavour(flavour, premium, 1)); // Create a new flavour and append ot the flavour list
                }
                break; // exit the loop
            }
            return flavourlist; // return the List<Flavour> list

        }

        // Sub Methods for Method: 05
        public static List<Topping> GetToppings(Dictionary<string, double> availabletoppingsdic)
        {
            DisplayToppings(availabletoppingsdic); // Display the available toppings
            List<Topping> toppingslist = new List<Topping>();  //Initialising the List<Toppings>, one of the parameters for creating the icecream, holds toppings objects
            List<string> ToppingsAvailable = new List<string>(); // Create a list to contain the available toppings
            foreach (var kvp in availabletoppingsdic) // Loop through the dictionary, to add the toppings to the list
            {
                string toppingName = kvp.Key;
                ToppingsAvailable.Add(toppingName.ToUpper());
            }
            while (true)
            {
                try
                {
                    {
                        Console.WriteLine("\n ---------Toppings-------------");
                        Console.Write(" Enter the number of toppings you would like to add: ");
                        int numberOfToppings = int.Parse(Console.ReadLine().Trim());

                        if (numberOfToppings <= 3 && numberOfToppings >= 0) //Validating for appropriate number of toppings
                        {
                            for (int i = 1; i <= numberOfToppings; i++)
                            {
                                Console.Write($" Enter the topping {i} you would like to add: ");
                                string topping = Console.ReadLine().Trim();

                                if (ToppingsAvailable.Contains(topping.ToUpper())) // Check if the topping exist in thw dictionary
                                {
                                    toppingslist.Add(new Topping(topping));  // Create a new topping and append to the topping list
                                }
                                else
                                {
                                    Console.WriteLine(" Enter a valid topping.\n");  //Error message for wrong input
                                    i--;
                                }
                            }
                            break;
                        }
                        else if (numberOfToppings == 4)
                        {
                            foreach(string topping  in ToppingsAvailable)
                            {
                                toppingslist.Add(new Topping(topping.ToUpper()));
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine(" You are only allowed to add up to 4 toppings.\n"); //Error message for wrong input
                        }
                    }
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine(" Invalid input! Please try again.\n");
                }
                catch (Exception ex)  // General exception handling
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }

            }
            return toppingslist;
        }


        // Sub Methods for Method: 06
        private static bool GetDippedConePreference()
        {
            while (true)
            {
                Console.WriteLine("\n ---------Dipped-------------");
                Console.Write(" Would you like to have chocolate dipped cone [Y/N]: ");
                string ans = Console.ReadLine().Trim();

                if (ans.ToUpper() == "Y")
                {
                    return true; // User wants a chocolate dipped cone
                }
                else if (ans.ToUpper() == "N")
                {
                    return false; // User doesn't want a chocolate dipped cone
                }
                else
                {
                    Console.WriteLine(" Enter a valid input.\n"); // Validating for wrong inputs.
                }
            }
        }
        // Sub Methods for Method: 07
        public static void ReadFlavourrCSV(Dictionary<string, int> AvailableFlavourDic)
        {
            using (StreamReader sr = new StreamReader("draft_flavours.csv"))
            {

                try
                {
                    string s = sr.ReadLine(); //Skipping header line
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(','); //Splitting by comma
                        if (parts.Length == 2)
                        {

                            string name = parts[0];
                            int cost = int.Parse(parts[1]);
                            AvailableFlavourDic.Add(name, cost); //Adding the name (key) cost(value) key value pair
                        }
                    }
                }
                catch (FileNotFoundException) // Handle a exception if file not found
                {
                    Console.WriteLine("File not found.");
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine("Error parsing data. Check the format in the CSV file.");
                }
                catch (Exception)// General exception handling
                {
                    Console.WriteLine("Error reading file.");
                }
            }

        }


        // Sub Methods for Method: 08

        public static void ReadToppingsCSV(Dictionary<string, double> AvailableToppingsDic)
        {
            using (StreamReader sr = new StreamReader("draft_toppings.csv"))
            {

                try
                {
                    string s = sr.ReadLine(); //Skipping the header line
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(','); //Split by comma
                        if (parts.Length == 2)
                        {

                            string name = parts[0];
                            double cost = double.Parse(parts[1]);
                            AvailableToppingsDic.Add(name, cost); //Adding the name (key) cost(value) key value pair
                        }
                    }
                }
                catch (FileNotFoundException) // Handle a exception if file not found
                {
                    Console.WriteLine("File not found.");
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine("Error parsing data. Check the format in the CSV file.");
                }
                catch (Exception)// General exception handling
                {
                    Console.WriteLine("Error reading file.");
                }

            }
        }
        // Sub Methods for Method: 09
        public static void DisplayFlavours(Dictionary<string, int> AvailableflavourDic)
        {
            Console.WriteLine(" \n-----------Flavours Available-------------"); //Display flavours
            Console.WriteLine($" {"Name",-10} {"Cost",-10}");
            foreach (KeyValuePair<string, int> Kvp in AvailableflavourDic)
            {
                Console.WriteLine($" {Kvp.Key,-10} ${Kvp.Value,-2:F2}");
            }
            Console.WriteLine(" -----------------------------------------");

        }

        // Sub Methods for Method: 10
        public static void DisplayToppings(Dictionary<string, double> AvailableToppingsDic)
        {
            Console.WriteLine(" \n-----------Toppings Available-------------"); //Display Toppings
            Console.WriteLine($" {"Name",-10} {"Cost",-10}");
            foreach (KeyValuePair<string, double> Kvp in AvailableToppingsDic)
            {
                Console.WriteLine($" {Kvp.Key,-10} ${Kvp.Value,-2:F2}");
            }
            Console.WriteLine(" ------------------------------------------");

        }
        // Sub Methods for Method: 11
        private static void ProcessIceCreamOrder(Order newOrder, Dictionary<string, int> availableflavoursdic, Dictionary<string, double> availbletoppingsdic,Customer customer)
        {
            bool orderCompleted = false; // Initialising orderCompleted to false, for CreateIceCream function to recursively happen.

            while (!orderCompleted) // while(true)
            {
                try
                {
                    
                    IceCream iceCream = CreateIceCream(availableflavoursdic, availbletoppingsdic); // Calling the method to create the icecream
                    newOrder.AddIceCream(iceCream); // Appending the icecream to current order icecream list.
                    bool invalidInput = true;
                    while (invalidInput)
                    {
                        Console.Write(" Do you want to add another ice cream to your order? (Y/N): ");
                        string addAnotherIceCream = Console.ReadLine().ToUpper();

                        if (addAnotherIceCream == "Y")
                        {
                            invalidInput = false; // Valid input, exit the loop
                        }
                        else if (addAnotherIceCream == "N")
                        {
                            Console.WriteLine("\n ---------Result-------------");
                            Console.WriteLine($" Order Successfully created for {customer.Name}.");
                            Console.WriteLine(" Confirmed Order");
                            DisplayCustomerOrderDetails(customer);
                            orderCompleted = true;
                            invalidInput = false; // Valid input, exit the loop
                        }
                        else
                        {
                            Console.WriteLine(" Invalid input. Please enter 'Y' to add another ice cream or 'N' to finish the order.");
                        }
                    }
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine(" Invalid input! Please try again.\n");
                }
                catch (Exception ex)  // General exception handling
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }
            }
        }



        // Option 5 Display order details of customer
        public static void DisplayOrderDetails(Dictionary<int, Customer> customerDic)
        {
            ListAllCustomers(customerDic); //List all the customers
            while (true)
            {
                try
                {
                    Console.Write("\n Please enter the customer ID that you wish to select: ");
                    int id = int.Parse(Console.ReadLine());
                    if (customerDic.ContainsKey(id)) //Validate if there input(customerid) exist in dictionary
                    {
                        Customer customer = customerDic[id]; //Retrieve the appropriate customer(keu) using the id (value) provided
                        if (customer.OrderHistory.Count() > 0) // Check if customer even got ORder history to display
                        {
                            Console.WriteLine($" Order details for {customer.Name} (Customer ID: {customer.Memberid})");
                            
                            foreach (Order order in customer.OrderHistory) // Loopp through the order history
                            {
                                Console.WriteLine($" \nOrder ID: {order.Id}");
                                foreach (IceCream iceCream in order.IceCreamList)
                                {
                                    Console.WriteLine(iceCream.ToString());
                                }
                                Console.WriteLine($" Time Received: {order.TimeReceived}");

                                if (order.TimeFulfilled != null)//CUrrent orders which are added to order history, timefulfilled will be null.
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
                            Console.WriteLine($" {customer.Name} has no Order History "); // Message to display there is no orders for the selected customer
                        }
                        break;

                    }
                    else
                    {
                        Console.WriteLine(" Invalid customer ID. Please enter a valid ID."); // Error message for invalid customer id
                        continue;
                    }
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine(" Invalid input! Please try again.");
                }
                catch (Exception ex)  // General exception handling
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }

            }
           
        }

        // Option 6 Display order details of customer
        public static void ModifyOrderDetails(Dictionary<int, Customer> customerDic,Dictionary<string, int> availableflavoursdic, Dictionary<string, double> availbletoppingsdic)
        {
            ListAllCustomers(customerDic); //List all the customers
            while (true)
            {
                try
                {
                    Console.Write("\n Please enter the customer ID that you wish to select: ");
                    int id = int.Parse(Console.ReadLine());
                    if (customerDic.ContainsKey(id)) //Validate if there input(customerid) exist in dictionary
                    {
                        Customer customer = customerDic[id];  //Retrieve the appropriate customer(keu) using the id (value) provided
                        Order oldorder = customer.CurrentOrder; // Assign a variable with Order type to hold the current current order
                        if (oldorder != null)//Check even if the customer has any current orders
                        {
                            DisplayCustomerOrderDetails(customer);
                            while (true)
                            {
                                DisplayMenuOpt6(); // Display the menu options
                                Console.Write(" Enter the option: ");
                                int opt = int.Parse(Console.ReadLine());
                                Console.WriteLine(" ----------------------------------------------------");

                                if (opt == 4)
                                {
                                    break;
                                }
                                IceCream iceCreamToModify;
                                while (true)
                                {
                                    Console.Write(" Enter the ice cream number you want to modify: ");
                                    int iceCreamNumber = int.Parse(Console.ReadLine());

                                    if (iceCreamNumber >= 1 && iceCreamNumber <= oldorder.IceCreamList.Count)
                                    {
                                        iceCreamToModify = oldorder.IceCreamList[iceCreamNumber - 1];
                                        Console.WriteLine($" Modifying details for Ice Cream #{iceCreamNumber}");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine($" Enter a valid icecream number");
                                        continue;
                                    }
                                }
                                if (opt >= 1 && opt <= 3)
                                {
                                    if (opt == 1)
                                    {
                                        ModifyIceCreamDetails(oldorder, availableflavoursdic, availbletoppingsdic, customer, iceCreamToModify);
                                    }
                                    else if (opt == 2)
                                    {
                                        Order newOrder = customer.MakeOrder();
                                        IceCream icecream = CreateIceCream(availableflavoursdic, availbletoppingsdic); // Call in the function, to add icecreams in neworder
                                        newOrder.AddIceCream(icecream);
                                        customer.UpdateCurrentOrder(newOrder);

                                    }
                                    else if (opt == 3)
                                    {

                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($" {customer.Name} has no current orders."); // Display message, if there isn't current order to modify
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine(" Invalid customer ID. Please enter a valid ID."); // Error massage when specified id not food
                        continue;
                    }
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine(" Invalid input! Please try again.");
                }
                catch (Exception ex)  // General exception handling
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }
            }
            
        }
        // Display Menu for opt 6
        public static void DisplayMenuOpt6()
        {
            Console.WriteLine("\n ---------------- Modify Order Details --------------");
            Console.WriteLine(" [1] Modify an IceCream");
            Console.WriteLine(" [2] Add new an IceCream");
            Console.WriteLine(" [3] Delete an IceCream");
            Console.WriteLine(" [4] Exit");
            Console.WriteLine(" ----------------------------------------------------");
        }

        private static void ModifyIceCreamDetails(Order order, Dictionary<string, int> availableflavoursdic, Dictionary<string, double> availbletoppingsdic, Customer customer, IceCream iceCreamToModify)
        {
            bool exitModifyIceCreamDetails = false;
            while (!exitModifyIceCreamDetails)
            {
                try
                {
                    // Display options to modify ice cream
                    Console.WriteLine("\n ---------------- Modify an IceCream ---------------");
                    Console.WriteLine(" [1] Modify Option");
                    Console.WriteLine(" [2] Modify Scoops");
                    Console.WriteLine(" [3] Modify Flavours");
                    Console.WriteLine(" [4] Modify Toppings");
                    Console.WriteLine(" [5] Modify Dipped Cone");
                    Console.WriteLine(" [6] Modify Waffle Flavour");
                    Console.WriteLine(" [0] Exit");
                    Console.WriteLine(" ----------------------------------------------------");

                    Console.Write(" Enter the option: ");
                    int modifyOption = int.Parse(Console.ReadLine());
                    Console.WriteLine(" ----------------------------------------------------");

                    if (modifyOption == 0)
                    {
                        exitModifyIceCreamDetails = true;
                        break;
                    }
                    else if (modifyOption >= 1 && modifyOption <= 6)
                    {
                        if (modifyOption == 1)
                        {
                            string newOption = GetValidIceCreamOption();
                            if (newOption != iceCreamToModify.Option)
                            {
                                IceCream newIceCream;

                                if (newOption == "CUP")
                                {
                                    newIceCream = new Cup(newOption, iceCreamToModify.Scoop, iceCreamToModify.Flavours, iceCreamToModify.Toppings);
                                }
                                else if (newOption == "CONE")
                                {
                                    bool dipped = GetDippedConePreference();
                                    newIceCream = new Cone(newOption, iceCreamToModify.Scoop, iceCreamToModify.Flavours, iceCreamToModify.Toppings, dipped);
                                }
                                else if (newOption == "WAFFLE")
                                {
                                    List<string> waffleFlavoursAvailable = new List<string> { "RED VELVET", "CHARCOAL", "PANDAN" };
                                    Console.WriteLine("\n ---------Waffle Flavour Available-------------");
                                    Console.WriteLine(" Name");

                                    foreach (string flavour in waffleFlavoursAvailable)
                                    {
                                        Console.WriteLine($" {flavour.ToLower()}");
                                    }
                                   
                                    while (true)
                                    {
                                        Console.WriteLine("\n ---------Waffle Flavour-------------");
                                        Console.Write(" Enter the waffle flavour you would like: ");
                                        string waffleFlavour = Console.ReadLine().ToUpper();

                                        if (waffleFlavoursAvailable.Contains(waffleFlavour))
                                        {
                                            newIceCream = new Waffle(newOption, iceCreamToModify.Scoop, iceCreamToModify.Flavours, iceCreamToModify.Toppings, waffleFlavour);
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine(" Enter a valid waffle flavour.\n");
                                            continue;
                                        }
                                    }
                                  
                                }
                                else
                                {
                                    Console.WriteLine($" Unknown ice cream option: {newOption}");
                                    continue;
                                }

                                // Replace the existing ice cream with the new one in the IceCreamList
                                int index = order.IceCreamList.IndexOf(iceCreamToModify);
                                order.IceCreamList[index] = newIceCream;

                                // Update iceCreamToModify reference to the new ice cream
                                iceCreamToModify = newIceCream;
                            }
                            else
                            {
                                Console.WriteLine($" Ice cream option is already {newOption}.");
                                continue;
                            }
                        }
                        else if (modifyOption == 2)
                        {
                            while (true)
                            {
                                int scoops = GetScoops();
                                if (iceCreamToModify.Scoop != scoops)
                                {
                                    iceCreamToModify.Scoop = scoops;
                                    iceCreamToModify.Flavours = GetFlavours(availableflavoursdic, scoops);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine($" Ice cream has already: {scoops} scoops.");
                                    continue;
                                }
                            }
                            
                        }
                        else if (modifyOption == 3)
                        {
                            iceCreamToModify.Flavours = GetFlavours(availableflavoursdic, iceCreamToModify.Scoop);
                        }
                        else if (modifyOption == 4)
                        {
                            iceCreamToModify.Toppings = GetToppings(availbletoppingsdic);
                        }
                        else if (modifyOption == 5)
                        {
                            if (iceCreamToModify is Cone)
                            {
                                
                                Cone coneIceCream = (Cone)iceCreamToModify;
                                bool dipped = GetDippedConePreference();
                                coneIceCream.Dipped = dipped;
                            }
                            else
                            {
                                Console.WriteLine($" {iceCreamToModify.Option} does not have the dipped property to be changed.");
                                continue;
                            }
                        }
                        else if (modifyOption == 6)
                        {
                            if (iceCreamToModify is Waffle)
                            {
                                Waffle waffleIceCream = (Waffle)iceCreamToModify;
                                List<string> waffleFlavoursAvailable = new List<string> { "RED VELVET", "CHARCOAL", "PANDAN" };
                                Console.WriteLine("\n ---------Waffle Flavour Available-------------");
                                Console.WriteLine(" Name");

                                foreach (string flavour in waffleFlavoursAvailable)
                                {
                                    Console.WriteLine($" {flavour.ToLower()}");
                                }
                                while (true)
                                {
                                    Console.WriteLine("\n ---------Waffle Flavour-------------");
                                    Console.Write(" Enter the waffle flavour you would like: ");
                                    string waffleFlavour = Console.ReadLine().ToUpper();

                                    if (waffleFlavoursAvailable.Contains(waffleFlavour))
                                    {
                                        waffleIceCream.WaffleFlavour = waffleFlavour;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine(" Enter a valid waffle flavour.\n");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($" {iceCreamToModify.Option} does not have the waffle flavour property to be changed.");
                                continue;
                            }
                        }

                        order.ModifyIceCream(index); // Add this line
                    }
                    else
                    {
                        Console.WriteLine(" Invalid option! Enter an option from the display shown,");
                    }
                }
                catch (FormatException) // Handle format exception
                {
                    Console.WriteLine(" Invalid input! Please try again.");
                }
                catch (Exception ex)  // General exception handling
                {
                    Console.WriteLine($" Error occurred: {ex.Message}\n");
                }
            }
        }


        // Extra methods to display IceCream details 
        private static void DisplayCustomerOrderDetails(Customer customer)
        {
            int iceCreamNo = 1;
            Console.WriteLine($" ----------- Order #{customer.CurrentOrder.Id} -----------");

            foreach (IceCream iceCream in customer.CurrentOrder.IceCreamList)
            {
                Console.WriteLine($" -----#{iceCreamNo} IceCream-------");
                Console.WriteLine(iceCream.ToString());
                iceCreamNo++;
                Console.WriteLine(" -----------------------");
            }

            Console.WriteLine($" ---------------------------------");
        }

    }
}














