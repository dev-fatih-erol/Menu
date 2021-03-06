﻿using Menu.Core.Enums;

namespace Menu.Cash.Extensions
{
    public static class EnumExtension
    {
        public static OrderCashStatus ToOrderCashStatusEnum(this string source)
        {
            return source switch
            {
                "Ödeme Yapılmadı" => OrderCashStatus.NoPayment,
                "Ödendi" => OrderCashStatus.PaymentCompleted,
                "Vip - Misafir" => OrderCashStatus.Treat,
                _ => OrderCashStatus.NoPayment,
            };
        }

        public static string ToOrderCashStatus(this OrderCashStatus source)
        {
            return source switch
            {
                OrderCashStatus.NoPayment => "Ödeme Yapılmadı",
                OrderCashStatus.PaymentCompleted => "Ödendi",
                OrderCashStatus.Treat => "Vip - Misafir",

                _ => null,
            };
        }

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