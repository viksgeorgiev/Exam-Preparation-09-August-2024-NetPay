using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ExportDtos;
using NetPay.Utilities;
using Newtonsoft.Json;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext context)
        {
            var households = context
                .Households
                .Include(h =>h.Expenses)
                .ThenInclude(e => e.Service)
                .Where(h => h.Expenses.Any(e => e.PaymentStatus != PaymentStatus.Paid))
                .OrderBy(h => h.ContactPerson)
                .ToArray()
                .Select(h => new HouseholdDto()
                {
                    ContactPerson = h.ContactPerson,
                    Email = h.Email,
                    PhoneNumber = h.PhoneNumber,
                    Expenses = h.Expenses
                        .Where(h => h.PaymentStatus != PaymentStatus.Paid)
                        .Select(e => new ExpenseDto()
                        {
                            ExpenseName = e.ExpenseName,
                            Amount = e.Amount.ToString("F2"),
                            PaymentDate = e.DueDate.ToString("yyyy-MM-dd"),
                            ServiceName = e.Service.ServiceName
                        })
                        .ToArray()
                        .OrderBy(e => e.PaymentDate)
                        .ThenBy(e => e.Amount)
                        .ToArray()
                })
                .ToArray();

            string result = XmlHelper.Serialize(households, "Households");
            return result;
        }

        public static string ExportAllServicesWithSuppliers(NetPayContext context)
        {
           var services = context
               .Services
               .OrderBy(s => s.ServiceName)
               .Select(e => new 
               {
                   e.ServiceName,
                   Suppliers = e.SuppliersServices
                       .Select(ss => new
                       {
                           SupplierName = ss.Supplier.SupplierName
                       })
                       .OrderBy(ss => ss.SupplierName)
                       .ToArray()
               })
               .ToArray();

           string result = JsonConvert.SerializeObject(services, Formatting.Indented);

           return result;
        }
    }
}
