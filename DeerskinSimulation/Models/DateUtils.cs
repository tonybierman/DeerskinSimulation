namespace DeerskinSimulation.Models
{
    public static class DateUtils
    {
        public static DateTime GameDate(int days)
        {
            return DateTime.Parse(Constants.SimStart).AddDays(days);
        }
    }
}
