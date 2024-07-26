namespace DeerskinSimulation.Models
{
    public static class MathUtils
    {
        public static double CalculateTransactionCost(int skins, double pricePerSkin)
        {
            return skins * pricePerSkin;
        }

        public static double CalculateSellingPrice(double totalCost, double markup)
        {
            return totalCost + (totalCost * markup);
        }

        public static double CalculateTotalCost(double principal, double extraCost, double duty)
        {
            return extraCost + (principal * duty);
        }
    }
}
