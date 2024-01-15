using System;

namespace draft_assignment
{
    public class PointCard
    {
        // Declaring attributes 
        private int _points;
        private int _punchCard;
        private string _tier;

        // Accessing it using getters and setters
        public int Points { get { return _points; } set { _points = value; } }
        public int PunchCard { get { return _punchCard; } set { _punchCard = value; } }
        public string Tier { get { return _tier; } set { _tier = value; } }

        // Declaring default constructor
        public PointCard()
        {
            Tier = "Ordinary";
        }
        // Declaring parameterized constructor
        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
        }
        // Methods
        public void AddPoint(double x)
        {
            // Check later, not sure wht x is int
            int earnedPoints = (int)Math.Floor(x * 0.72);
            Points += earnedPoints;
            if (Points >= 100 && Tier != "Gold")
            {
                Tier = "Gold";
            }
            else if (Points >= 50 && Tier != "Silver")
            {
                Tier = "Silver";
            }

        }
        public void RedeemPoints(int pointsToRedeem)
        {
            if ((pointsToRedeem <= Points) && (Tier == "Silver" || Tier == "Gold"))
            {
                Points -= pointsToRedeem;
                double redeemValue = pointsToRedeem * 0.02;
            }
        }

        public void Punch()
        {

        }
        public override string ToString()
        {
            return $"{Tier,-20} {Points,-10} {PunchCard,-10}";
        }
    }
}
