using System.Collections.Generic;

namespace draft_assignment
{
    public class Cup : IceCream
    {
        // Declaring default constructor
        public Cup() : base() { }
        // Declaring parameterized constructor
        public Cup(string Option, int scoop, List<Flavour> flavours, List<Topping> toppings) : base(Option, scoop, flavours, toppings) { }
        // Implementing the abstract class
        public override double CalculatePrice()
        {
            
            double basePrice = 0.00;
            {
                if (Scoop == 1)
                {
                    basePrice = 4.00;
                }
                else if (Scoop == 2)
                {
                    basePrice = 5.50;
                }
                else if (Scoop == 3)
                {
                    basePrice = 6.50;
                }
            }

            // Assign appropriate cost for premium flavours
            double premimumflavourcost = 0.0;
            foreach(Flavour flavour in Flavours)
            {
                if(flavour.Premium == true)
                {
                    premimumflavourcost += 2.00;
                }
            }
            double toppingsCost = Toppings.Count * 1.0;

            // Calculate the total price
            double totalPrice = basePrice + toppingsCost + premimumflavourcost;

            return totalPrice;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
