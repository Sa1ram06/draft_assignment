namespace draft_assignment
{
    public class Flavour
    {
        // Declaring attributes 
        private string _type;
        private bool _premium;
        private int _quantity;

        // Accessing it using getters and setters
        public string Type { get { return _type; } set { _type = value; } }
        public bool Premium { get { return _premium; } set { _premium = value; } }
        public int Quantity { get { return _quantity; } set { _quantity = value; } }

        // Declaring default constructor
        public Flavour() { }
        // Declaring parameterized constructor
        public Flavour(string type, bool premium, int qty)
        {
            Type = type;
            Premium = premium;
            Quantity = qty;
        }
        // Methods
        public override string ToString()
        {
            return $" Type: {Type,-10} Premium: {Premium,-10} Quantity: {Quantity,-10}";
        }
    }
}
