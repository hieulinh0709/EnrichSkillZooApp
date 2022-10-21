using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.Core.Constants
{
    public struct ROLES
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
    }

    public struct STATUS
    {
        public const string Success = "success";
        public const string Pending = "pending";
        public const string Inprocess = "inprocess";
        public const string Completed = "completed";
        public const string Approved = "approved";
    }

    public struct MODE
    {
        public const string Payment = "payment";
    }

    public struct PaymentStatus
    {
        public const string Paid = "paid";
    }

    public struct ControllerConsts
    {
        public const string Order = "Order";
    }
    public struct ActionNameConsts
    {
        public const string Index = "Index";
        public const string Create = "Create";
        public const string Edit = "Edit";
        public const string Details = "Details";
        public const string Delete = "Delete";
        public const string Summary = "Summary";
    }

    public struct ViewNameConsts
    {
        public const string PaymentConfirmation = "PaymentConfirmation";
        public const string Index = "Index";
        public const string Create = "Create";
        public const string Edit = "Edit";
    }

    public struct StripeConsts
    {
        public const string SecretKey = "sk_test_51JcFLuLzMgCIgSRrqSw0rTwIt0RlyKd5EEY4p1ocZqgGq4C3LtIHtwiqODMmadnDwEcykJeWvzE8ec4hBdLOAMS400isCtFIR7";
    }

    public struct Currency
    {
        public const string USD = "usd";
    }

    public struct Entity
    {
        public const string Product = "Product";
        public const string ApplicationUser = "ApplicationUser";
    }

    public struct MSG
    {
        public const string MsgCode1 = "Order Details Updated Successfully.";
        public const string MsgCode2 = "Order Status Updated Successfully.";
        public const string MsgCode3 = "Order Shipped Successfully.";
        public const string MsgCode4 = "Order Cancelled Successfully.";
        public const string MsgCode5 = "Category deleted successfully";
        public const string MsgCode6 = "Category updated successfully";
        public const string MsgCode7 = "Category created successfully";
    }
}
