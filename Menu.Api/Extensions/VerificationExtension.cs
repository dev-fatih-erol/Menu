namespace Menu.Api.Extensions
{
    public static class VerificationExtension
    {
        public static bool ValidatePhoneNumber(this string source, string phoneNumber, string code)
        {
            return !string.Equals(phoneNumber, source.Split(',')[0]) || !string.Equals(code, source.Split(',')[1]);
        }
    }
}