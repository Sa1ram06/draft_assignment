using System;
using System.Collections.Generic;

namespace draft_assignment
{
    public abstract class IceCream: IComparable<IceCream>
    {
        // Declaring attributes
        private string _option;
        private int _scoop;
        private List<Flavour> _flavours = new List<Flavour>();
        private List<Topping> _toppings = new List<Topping>();

        // Accessing it using getters and setters
        public string Option { get { return _option; } set { _option = value; } }
        public int Scoop { get { return _scoop; } set { _scoop = value; } }
        public List<Flavour> Flavours { get { return _flavours; } set { _flavours = value; } }
        public List<Topping> Toppings { get { return _toppings; } set { _toppings = value; } }

        // Declaring default constructor
        public IceCream() { }
        // Declaring parameterized constructor
        public IceCream(string option, int scoop, List<Flavour> flavours, List<Topping> toppings)
        {
            Option = option;
            Scoop = scoop;
            Flavours = flavours;
            Toppings = toppings;
        }
        // Method
        // Abstract method 
        public abstract double CalculatePrice();

        public int CompareTo(IceCream other)
        {
            // Compare based on price (you might need to adjust this based on your actual implementation)
            return CalculatePrice().CompareTo(other.CalculatePrice());
        }
        public override string ToString()
        {
            string flavourDetails = "";
            foreach (Flavour flavour in Flavours)
            {
                flavourDetails += $"{flavour.Type.ToLower()},";
            }
            string toppingDetails = "";
            foreach (Topping topping in Toppings)
            {
                toppingDetails += $"{topping.Type.ToLower()},";
            }
            return $" Ice Cream Details:\n  - Option: {Option.ToLower(),-10}\n  - Scoop: {Scoop,-10}\n  - Flavours: {flavourDetails}\n  - Toppings: {toppingDetails}";

        }
    }
}
