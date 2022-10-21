using BookStoreManagement.Core.Constants;
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
                case STATUS.Pending:
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == StatusData.PaymentStatusDelayedPayment);
                    break;
                case STATUS.Inprocess:
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StatusData.StatusInProcess);
                    break;
                case STATUS.Completed:
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StatusData.StatusShipped);
                    break;
                case STATUS.Approved:
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StatusData.StatusApproved);
                    break;
                default:
                    break;
            }

            return orderHeaders;
        }
    }
}
