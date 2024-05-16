
namespace Api.Calculator
{
    public static class DateTimeHelper
    {
        public static bool OlderThan(this DateTime birthDate, int maxAge)
        {
            int age = DateTime.Today.Year - birthDate.Year; // maybe adjust "Today" to the specific timezone (UTC)
            if (birthDate > DateTime.Today.AddYears(-age)) age--;
            return age >= maxAge;
        }
    }
}
