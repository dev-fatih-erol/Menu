using Menu.Core.Enums;

namespace Menu.Business.Extensions
{
    public static class EnumExtension
    {
        public static string ToOptionType(this OptionType source)
        {
            return source switch
            {
                OptionType.Select => "Tekli seçim",
                OptionType.CheckBox => "Çoklu seçim",
                _ => null,
            };
        }
    }
}