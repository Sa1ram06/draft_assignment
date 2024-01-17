using System.Collections.Generic;

namespace draft_assignment
{
    public class Cone : IceCream
    {
        // Declaring attributes 
        private bool _dipped;
        // Accessing attributes through getters and setters 
        public bool Dipped { get { return _dipped; } set { _dipped = value; } }
        //Declaring default constructor
        public Cone() : base() { }
        // Declaring parameterized constructor
        public Cone(string Option, int scoop, List<Flavour> flavours, List<Topping> toppings, bool dipped) : base(Option, scoop, flavours, toppings)
        {
            Dipped = dipped;
        }
        // Implementing the abstract class
        public override double CalculatePrice()
        {
            // Initialise the base price for different flavours
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
            // Check if the order is chocolate dipped cone
            double dipPrice = 0.0;
            if (Dipped == true)
            {
                dipPrice = 2.0;
            }
            double toppingsCost = Toppings.Count * 1.0;

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
            double totalPrice = basePrice + toppingsCost + dipPrice + premimumflavourcost;

            return totalPrice;
        }
        public override string ToString()
        {
            return base.ToString() + $"Dipped: {Dipped,-10}";
        }
    }
}
