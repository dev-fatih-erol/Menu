using Menu.Core.Enums;

namespace Menu.Cash.Extensions
{
    public static class EnumExtension
    {
        public static string ToTableStatus(this TableStatus source)
        {
            return source switch
            {
                TableStatus.Closed => "Kapalı",
                TableStatus.Open => "Açık",
                _ => null,
            };
        }
    }
}