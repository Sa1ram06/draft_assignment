using System.Collections.Generic;
using System;

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
        public Order currentOrder { get { return _currentorder; } set { _currentorder = value; } }
        public List<Order> OrderHistory { get { return _orderHistory; } set { _orderHistory = value; } }
        public PointCard Rewards { get { return _rewards; } set { _rewards = value; } }

        // Declaring default constructor
        public Customer() { }
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
            currentOrder = new Order();
            return currentOrder;
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
            return $"{Name,-10} {Memberid,-10} {Dob.ToString("dd/MM/yyyy"),-15}";
        }
    }
}

