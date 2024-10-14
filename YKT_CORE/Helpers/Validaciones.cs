namespace YKT.CORE.Helpers
{
    public class Validaciones
    {
        public static string FormatUSPhoneNumber(string? phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                string cleanedNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

                if (cleanedNumber.Length >= 10)
                {
                    return string.Format("+1 ({0}) {1}-{2}", cleanedNumber.Substring(0, 3), cleanedNumber.Substring(3, 3), cleanedNumber.Substring(6, 4));
                }
                else
                {
                    return phoneNumber;
                }
            }
            return "";
        }
    }
}
