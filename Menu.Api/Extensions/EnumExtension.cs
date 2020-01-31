using Menu.Core.Enums;

namespace Menu.Api.Extensions
{
    public static class EnumExtension
    {
        public static string ToOrderStatus(this OrderStatus source)
        {
            return source switch
            {
                OrderStatus.Pending => "Onay Bekliyor",
                OrderStatus.Approved => "Onaylandı",
                OrderStatus.Denied => "Rededildi",
                OrderStatus.Preparing => "Hazırlanıyor",
                OrderStatus.Cancel => "İptal",
                OrderStatus.Prepared => "Hazırlandı",
                OrderStatus.Closed => "Teslim Edildi",
                _ => null,
            };
        }
    }
}