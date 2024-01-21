//==========================================================
// Student Number : S10259930
// Student Name : Sairam
// Partner Name : Pey Zhi Xun
//==========================================================
using System.Collections.Generic;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace draft_assignment
{
    public class Customer
    {
        // Declaring attributes
        private string _name;
        private int _memberid;
        private DateTime _dob;
        private Order _currentorder;
        private List<Order> _orderHistory = new List<Order>();
        private PointCard _rewards;

        // Accessing attributes through getters and setters
        public string Name { get { return _name; } set { _name = value; } }
        public int Memberid { get { return _memberid; } set { _memberid = value; } }
        public DateTime Dob { get { return _dob; } set { _dob = value; } }
        public Order CurrentOrder { get { return _currentorder; } set { _currentorder = value; } }
        public List<Order> OrderHistory { get { return _orderHistory; } set { _orderHistory = value; } }
        public PointCard Rewards { get { return _rewards; } set { _rewards = value; } }

        // Declaring default constructor
        public Customer()
        {
            DateTime dob = DateTime.Now;
        }
        // Declaring a parameterized constructor
        public Customer(string name, int id, DateTime dob)
        {
            Name = name;
            Memberid = id;
            Dob = dob;
        }
        // Methods
        public Order MakeOrder()
        {
            Order newOrder = new Order();
            bool orderCompleted = false;
            while (!orderCompleted)
            {
                try
                {
                    string option;
                    while (true)
                    {
                        Console.WriteLine("\n ---------Option-------------");
                        Console.Write(" Enter the Ice cream option (Cup/Cone/Waffle): ");
                        option = (Console.ReadLine().ToUpper());
                        if (option == "CUP" || option == "CONE" || option == "WAFFLE")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine(" Please enter a valid ice cream option (Cup/Cone/Waffle).");
                        }
                    }
                    int scoops;
                    while (true)
                    {
                        Console.WriteLine("\n ---------Scoop-------------");
                        Console.Write(" Enter the number of scoops (not more than 3): ");
                        scoops = int.Parse(Console.ReadLine());
                        if (scoops >=  1 && scoops <= 3)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine($" We don't serve {scoops} ice cream here, Please enter a valid number of scoops.");

                        }
                    }
                    List<Flavour> flavourlist = new List<Flavour>();
                    List<string> regularflavours = new List<string> { "VANILLA", "CHOCOLATE", "STRAWBERRY" };
                    List<string> premiumflavours = new List<string> { "DURIAN", "UBE", "SEA SALT" };
                    while (true)
                    {
                        Console.WriteLine("\n ---------Flavours-------------");
                        Console.Write(" Enter the number of flavours you would like to add: ");
                        int numberOfFlavours = int.Parse(Console.ReadLine());
                        if (numberOfFlavours > 0 && numberOfFlavours <= scoops)
                        {
                            break;
                        }
                        Console.WriteLine($" You can only add between 1 to {scoops} flavours.\n");
                    }

                    for (int i = 1; i <= scoops; i++)
                    {
                        Console.Write($" Enter the flavour {i} you would like to add: ");
                        string flavour = Console.ReadLine();
                        bool premium = premiumflavours.Contains(flavour);
                        if (!(regularflavours.Contains(flavour.ToUpper()) || premiumflavours.Contains(flavour.ToUpper())))
                        {
                            Console.WriteLine(" Please provide a valid flavour.\n");
                            i--;
                            continue;
                        }
                        flavourlist.Add(new Flavour(flavour, premium, 1));
                    }


                    List<Topping> toppingslist = new List<Topping>();
                    List<string> ToppingsAvailable = new List<string> { "SPRINKLES", "MOCHI", "SAGO", "OREOS" };
                    while (true)
                    {
                        Console.WriteLine("\n ---------Toppings-------------");
                        Console.Write(" Enter the number of toppings you would like to add: ");
                        int numberOfToppings = int.Parse(Console.ReadLine());

                        if (numberOfToppings >= 0 && numberOfToppings <= toppingslist.Count)
                        {
                            for (int i = 1; i <= numberOfToppings; i++)
                            {
                                Console.Write($" Enter the topping {i} you would like to add: ");
                                string topping = Console.ReadLine();

                                if (ToppingsAvailable.Contains(topping.ToUpper()))
                                {
                                    toppingslist.Add(new Topping(topping));
                                }
                                else
                                {
                                    Console.WriteLine(" Enter a valid topping.\n");
                                    i--;  
                                }
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine(" You are only allowed to add up to 4 toppings.\n");
                        }
                    }


                    if (option == "Cup")
                    {
                        IceCream IceCreamCup = new Cup(option, scoops, flavourlist, toppingslist);
                        newOrder.AddIceCream(IceCreamCup);
                    }
                    else if (option == "Cone")
                    {
                        Console.WriteLine("\n ---------Dipped-------------");
                        Console.Write(" Would you like to have chocolate dipped cone [Y/N]: ");
                        string ans = Console.ReadLine();
                        bool dipped = false;
                        if (ans.ToUpper() == "Y")
                        {
                            dipped = true;
                        }
                        else if (ans.ToUpper() == "N")
                        {
                            dipped = false;
                        }
                        else
                        {
                            Console.WriteLine(" Enter a valid input.\n");
                            continue;
                        }
                        IceCream IceCreamCone = new Cone(option, scoops, flavourlist, toppingslist, dipped);
                        newOrder.AddIceCream(IceCreamCone);
                    }
                    else if (option == "Waffle")
                    {
                        List<string> waffleFlavoursAvailable = new List<string> { "RED VELVET", "CHARCOAL", "PANDAM" };
                        while (true)
                        {
                            Console.WriteLine("\n ---------Waffle Flavour-------------");
                            Console.Write(" Enter the waffle flavour you would like: ");
                            string waffleFlavour = Console.ReadLine();

                            if (waffleFlavoursAvailable.Contains(waffleFlavour.ToUpper()))
                            {
                                IceCream iceCreamWaffle = new Waffle(option, scoops, flavourlist, toppingslist, waffleFlavour);
                                newOrder.AddIceCream(iceCreamWaffle);
                                break;
                            }
                            else
                            {
                                Console.WriteLine(" Enter a valid waffle flavour.\n");
                                continue;
                            }
                        }
                    }
                    while (true)
                    {
                        Console.Write(" Do you want to add another ice cream to your order? (Y/N): ");
                        string addAnotherIceCream = Console.ReadLine();

                        if (addAnotherIceCream.ToUpper() == "Y")
                        {
                            break;
                        }
                        else if (addAnotherIceCream.ToUpper() == "N")
                        {
                            Console.WriteLine("\n ---------Result-------------");
                            Console.WriteLine(" Order Successfully created.");
                            orderCompleted = true; // Exit the outer loop
                            break; // Exit the inner loop
                        }
                        else
                        {
                            Console.WriteLine(" Invalid input. Please enter 'Y' to add another ice cream or 'N' to finish the order.");
                            continue;
                        }
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
            CurrentOrder = newOrder;
            OrderHistory.Add(CurrentOrder);
            return CurrentOrder;
        }
        public bool IsBirthday()
        {
            DateTime datenow = DateTime.Now;
            if (datenow == Dob)
            {
                // Do some logic
                return true;
            }
            return false;
        }


        public override string ToString()
        {
            return $" {Name,-10} {Memberid,-10} {Dob.ToString("dd/MM/yyyy"),-15}";
        }
    }
}

