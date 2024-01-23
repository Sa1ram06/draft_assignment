using System.Collections.Generic;
using System;

namespace draft_assignment
{
    public class Order
    {
        // Declaring attributes
        private int _id;
        private DateTime _timeReceived;
        private DateTime? _timeFulfilled;
        private List<IceCream> _iceCreamList = new List<IceCream>();

        // Accessing attributes through getters and setters
        public int Id { get { return _id; } set { _id = value; } }
        public DateTime TimeReceived { get { return _timeReceived; } set { _timeReceived = value; } }
        public DateTime? TimeFulfilled { get { return _timeFulfilled; } set { _timeFulfilled = value; } }
        public List<IceCream> IceCreamList { get { return _iceCreamList; } set { _iceCreamList = value; } }

        // Declaring default constructor List
        public Order()
        {
            int id = 5;
            id += 1;
            Id = id;
            TimeReceived = DateTime.Now;
        }
        // Declaring parameterized constructor
        public Order(int id, DateTime timerecived)
        {
            Id = id;
            TimeReceived = timerecived;
        }
        // Methods
        public void ModifyIceCream(int index)
        {
            // Not sure
        }

        public void AddIceCream(IceCream iceCream)
        {
            IceCreamList.Add(iceCream);

        }
        public void DeleteIceCream(int index)
        {
            if (IceCreamList.Count > 1)
            {
                IceCreamList.RemoveAt(index);
            }
            else
            {
                Console.WriteLine("Cannot have zero ice creams in an order.");
            }
        }
        public double CalculateTotal()
        {
            double total = 0.0;
            foreach (IceCream iceCream in IceCreamList)
            {
                total += iceCream.CalculatePrice();
            }
            return total;
        }
        public override string ToString()
        {
            string icecreamDetails = "";
            foreach (IceCream icecream in IceCreamList)
            {
                icecreamDetails += icecream.ToString();
            }

            string timeFulfilled = "------";
            if (TimeFulfilled != null)
            {
                timeFulfilled = TimeFulfilled.ToString();
            }

            return $" Id: {Id,-10}\n Time Received: {TimeReceived.ToString(),-10}\n Time Fulfilled: {TimeFulfilled,-10}\n Ice Cream Details: {icecreamDetails}\n Total price: {CalculateTotal():F2}";
        }
    }
}
