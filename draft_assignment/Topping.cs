namespace draft_assignment
{
    public class Topping
    {
        // Declaring attributes 
        private string _type;

        // Accessing it using getters and setters
        public string Type { get { return _type; } set { _type = value; } }

        // Declaring default constructor
        public Topping() { }
        // Declaring parameterized constructor
        public Topping(string type) { Type = type; }
        // Method
        public override string ToString()
        {
            return $" Type: {Type,-10}";
        }
    }
}
