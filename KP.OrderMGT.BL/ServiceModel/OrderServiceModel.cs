﻿using KP.OrderMGT.BL.DBModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace KP.OrderMGT.BL.ServiceModel
{

    [DataContract]
    public class SaleAmountByPassport
    {
        [DataMember]
        public decimal? SaleAmt { get; set; }

        [DataMember]
        public decimal? Alcohol { get; set; }

        [DataMember]
        public decimal? Tobacco { get; set; }

        public SaleAmountByPassport() { }

        public SaleAmountByPassport(get_sale_passport_vol2Result data)
        {
            this.SaleAmt = data.net;
            this.Alcohol = data.lq;
            this.Tobacco = data.tb;
        }
    }

    [DataContract]
    public class SaleOnlineByPassport
    {
        [DataMember]
        public decimal SumSKU { get; set; }

        [DataMember]
        public decimal SumTotal { get; set; }

        [DataMember]
        public List<SaleOnlineByPassportDetail> Details { get; set; }
    }

    [DataContract]
    public class SaleOnlineByPassportDetail
    {
        [DataMember]
        public string SKU { get; set; }

        [DataMember]
        public string QTY { get; set; }

        [DataMember]
        public string Total { get; set; }
    }

    [DataContract]
    public class OrderHeader
    {
        [DataMember]
        public Order NewOrder { get; set; }

        [DataMember]
        public Flight Flight { get; set; }

        [DataMember]
        public Billing Billing { get; set; }

        [DataMember]
        public List<ItemSKU> Items { get; set; }

        [DataMember]
        public List<Payment> Payments { get; set; }
    }

    [DataContract]
    public class OrderSession
    {
        [DataMember]
        public Guid SessionGuid { get; set; }

        [DataMember]
        public long SessionId { get; set; }

        [DataMember]
        public string SaleOrderNo { get; set; }

        [DataMember]
        public string POSInvoiceNo { get; set; }

        [DataMember]
        public string POSOrderNo { get; set; }

        [DataMember]
        public string POSStatus { get; set; }
    }

    [DataContract]
    public class Order
    {
        [DataMember]
        public string OrderNo { get; set; }

        [DataMember]
        public string DeliveryCost { get; set; }

        [DataMember]
        public string PassportNo { get; set; }

        [DataMember]
        public string InvoiceNo { get; set; }

        [DataMember]
        public string MemberID { get; set; }

        [DataMember]
        public string UserToken { get; set; }

        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public string PaymentNo { get; set; }

        [DataMember]
        public bool IsAcceptMedia { get; set; }

        [DataMember]
        public string WeChatID { get; set; }

        [DataMember]
        public string DutyTax { get; set; }

        [DataMember]
        public string AgentCode { get; set; }
    }

    [DataContract]
    public class Billing
    {
        [DataMember]
        public string BillAddress1 { get; set; }

        [DataMember]
        public string BillAddress2 { get; set; }

        [DataMember]
        public string BillAddress3 { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string District { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string Birthday { get; set; }

        [DataMember]
        public string Sex { get; set; }

        [DataMember]
        public string Telephone { get; set; }

        [DataMember]
        public string MobilePhone { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Fax { get; set; }

        [DataMember]
        public string PersonalTitle { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string PhoneCountryCode { get; set; }

    }

    [DataContract]
    public class Ship
    {

        [DataMember]
        public string ShipAddress1 { get; set; }

        [DataMember]
        public string ShipAddress2 { get; set; }

        [DataMember]
        public string ShipAddress3 { get; set; }

        [DataMember]
        public string ShipStreet { get; set; }

        [DataMember]
        public string ShipDistrict { get; set; }

        [DataMember]
        public string ShipCity { get; set; }

        [DataMember]
        public string ShipProvince { get; set; }

        [DataMember]
        public string ShipPostalCode { get; set; }

        [DataMember]
        public string ShipCountryCode { get; set; }

        [DataMember]
        public string ShipCountryName { get; set; }

        [DataMember]
        public string ShipTelephone { get; set; }

        [DataMember]
        public string ShipMobile { get; set; }

        [DataMember]
        public string ShipEmail { get; set; }

        [DataMember]
        public string ShipFax { get; set; }

        [DataMember]
        public string LogisticID { get; set; }

        [DataMember]
        public string ShipPersonalTitle { get; set; }

        [DataMember]
        public string ShipFirstName { get; set; }

        [DataMember]
        public string ShipLastName { get; set; }

        [DataMember]
        public string ShipPhoneCountryCode { get; set; }

        [DataMember]
        public string ShipLogisticName { get; set; }

        [DataMember]
        public string NationalID { get; set; }
    }

    [DataContract]
    public class ItemSKU
    {
        [DataMember]
        public string MaterialCode { get; set; }

        [DataMember]
        public decimal Quantity { get; set; }

        [DataMember]
        public decimal SellingPrice { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal Discount { get; set; }

        [DataMember]
        public decimal DiscountRate { get; set; }

        [DataMember]
        public string PromoCode { get; set; }

        [DataMember]
        public decimal Net { get; set; }

        [DataMember]
        public decimal SPDiscount { get; set; }

        [DataMember]
        public decimal SPDiscountRate { get; set; }

        [DataMember]
        public string SPPromoCode { get; set; }

        [DataMember]
        public decimal TotalDiscount { get; set; } // Discount + SPDiscount = TotalDiscount

        [DataMember]
        public decimal TotalNet { get; set; } // Net + SPDiscount = TotalNet

        [DataMember]
        public string SellerAdjustFee { get; set; }

        [DataMember]
        public string ord_no { get; set; }

        [DataMember]
        public string SubOrderType { get; set; }

        [DataMember]
        public string RecNo { get; set; }
    }

    [DataContract]
    public class Payment
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public decimal Amount { get; set; }
    }

    [DataContract]
    public class SaleQueae
    {
        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string SKU { get; set; }

        [DataMember]
        public decimal Quantity { get; set; }
    }

}
