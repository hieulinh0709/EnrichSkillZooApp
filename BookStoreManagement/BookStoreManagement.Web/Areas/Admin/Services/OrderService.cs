using BookStoreManagement.Models;
using BookStoreManagement.Utility;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data;
using System.Security.Claims;

namespace BookStoreManagement.Web.Areas.Admin.Services
{
    public class OrderService
    {
        public IEnumerable<OrderHeader> FilterByStatus(IEnumerable<OrderHeader> orderHeaders, string status)
        {
            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == StatusData.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StatusData.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StatusData.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StatusData.StatusApproved);
                    break;
                default:
                    break;
            }

            return orderHeaders;
        }
    }
}
