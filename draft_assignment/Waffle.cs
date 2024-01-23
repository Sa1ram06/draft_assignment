using System.Collections.Generic;

namespace draft_assignment
{
    public class Waffle : IceCream
    {
        // Declaring attributes 
        private string _waffleFlavour;
        // Accessing attributes through getters and setters 
        public string WaffleFlavour { get { return _waffleFlavour; } set { _waffleFlavour = value; } }
        //Declaring default constructor
        public Waffle() : base() { }
        // Declaring parameterized constructor
        public Waffle(string Option, int scoop, List<Flavour> flavours, List<Topping> toppings, string waffleFlavour) : base(Option, scoop, flavours, toppings)
        {
            WaffleFlavour = waffleFlavour;
        }
        // Implementing the abstract class
        public override double CalculatePrice()
        {
            double basePrice = 0.00;
            {
                if (Scoop == 1)
                {
                    basePrice = 7.00;
                }
                else if (Scoop == 2)
                {
                    basePrice = 8.50;
                }
                else if (Scoop == 3)
                {
                    basePrice = 9.50;
                }
            }
            double toppingsCost = Toppings.Count * 1.0;
            double wafflePrice = 3.00;


            // Assign appropriate cost for premium flavours
            double premimumflavourcost = 0.0;
            foreach (Flavour flavour in Flavours)
            {
                if (flavour.Premium == true)
                {
                    premimumflavourcost += 2.00;
                }

            }

            // Calculate the total price
            double totalPrice = basePrice + toppingsCost + wafflePrice + premimumflavourcost;

            return totalPrice;
        }
        public override string ToString()
        {
            return base.ToString() + $" \n  - Waffle flavour: {WaffleFlavour.ToLower(),-10}";
        }
    }
}
