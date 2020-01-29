using System.ComponentModel;

namespace Menu.Core.Enums
{
    public enum OrderStatus
    {
        [Description("Onay Bekliyor")]
        Pending = 0,
        [Description("Onaylandı")]
        Approved = 1,
        [Description("Rededildi")]
        Denied = 2,
        [Description("Hazırlanıyor")]
        Preparing = 3,
        [Description("İptal")]
        Cancel = 4,
        [Description("Hazırlandı")]
        Prepared = 5,
        [Description("Teslim Edildi")]
        Closed = 6
    }
}