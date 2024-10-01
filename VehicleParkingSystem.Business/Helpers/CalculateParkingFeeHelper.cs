namespace VehicleParkingSystem.Business.Helpers
{
    public static class CalculateParkingFeeHelper
    {
        public static decimal CalculateParkingFee(DateTime entryTime, DateTime exitTime, decimal ratePerHour)
        {
            TimeSpan duration = exitTime - entryTime;
            double totalHours = duration.TotalHours;

            int roundedHours = (int)Math.Ceiling(totalHours);

            return roundedHours * ratePerHour;
        }
    }
}
