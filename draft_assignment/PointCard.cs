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
            Points = 0;
            PunchCard = 0;
            Tier = "Ordinary";
        }
        // Declaring parameterized constructor
        public PointCard(int points, int punchCard)
        {
            Points = points;
            PunchCard = punchCard;
        }
        // Methods
        public void AddPoint(int x)
        {
            Points += x; 
        }
        public void UpdateTier()
        {
            if (Points >= 100)
            {
                Tier = "Gold";
            }
            else if (Points >= 50 && Tier != "Gold")
            {
                Tier = "Silver";
            }
        }
        public void RedeemPoints(int pointsToRedeem)
        {
                Points -= pointsToRedeem;

        }

        public void Punch()
        {
            PunchCard++;

        }
        public override string ToString()
        {
            return $"{Tier,-20} {Points,-10} {PunchCard,-10}";
        }
    }
}
