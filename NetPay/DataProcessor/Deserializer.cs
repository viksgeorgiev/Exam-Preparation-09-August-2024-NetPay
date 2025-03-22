using NetPay.Data;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ImportDtos;
using NetPay.Utilities;
using Newtonsoft.Json;

namespace NetPay.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

        public static string ImportHouseholds(NetPayContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            ImportHouseholdsDto[]? householdsDtos =
                XmlHelper.Deserialize<ImportHouseholdsDto[]>(xmlString, "Households");

            if (householdsDtos != null && householdsDtos.Length > 0)
            {
                ICollection<Household> householdsToAdd = new List<Household>();

                foreach (ImportHouseholdsDto householdsDto in householdsDtos)
                {
                    if (!IsValid(householdsDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var validHouseholds = context.Households.ToList();


                    if (IsExisting(householdsDto, householdsToAdd)
                        || IsExisting(householdsDto, validHouseholds))
                    {
                        sb.AppendLine(DuplicationDataMessage);
                        continue;
                    }

                    Household household = new Household()
                    {
                        ContactPerson = householdsDto.ContactPerson,
                        Email = householdsDto.Email,
                        PhoneNumber = householdsDto.PhoneNumber
                    };

                    householdsToAdd.Add(household);
                    sb.AppendLine(string.Format(SuccessfullyImportedHousehold, householdsDto.ContactPerson));
                }
                context.AddRange(householdsToAdd);
                context.SaveChanges();
            }
            return sb.ToString().TrimEnd();
        }

        public static bool IsExisting(ImportHouseholdsDto houseHold, ICollection<Household> collection)
        {
            return collection.Any(i =>
                       i.ContactPerson == houseHold.ContactPerson
                    || i.Email == houseHold.Email
                    || i.PhoneNumber == houseHold.PhoneNumber);
        }


        public static string ImportExpenses(NetPayContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ExpensesImportJsonDto[]? importExpenseDto =
                JsonConvert.DeserializeObject<ExpensesImportJsonDto[]>(jsonString);

            if (importExpenseDto != null && importExpenseDto.Length > 0)
            {
                ICollection<Expense> expensesToAdd = new List<Expense>();

                foreach (ExpensesImportJsonDto expensesDto in importExpenseDto)
                {
                    if (!IsValid(expensesDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var validHouseholdIds = context.Households.Select(h => h.Id).ToHashSet();
                    var validServiceIds = context.Services.Select(s => s.Id).ToHashSet();

                    if ((!validHouseholdIds.Contains(expensesDto.HouseholdId))
                        || (!validServiceIds.Contains(expensesDto.ServiceId)))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isValidDateTime = DateTime.TryParseExact(expensesDto.DueDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDateParsed);

                    bool isValidEnum = Enum.TryParse<PaymentStatus>(expensesDto.PaymentStatus, out PaymentStatus parsedPaymentStatus);


                    if ((!isValidEnum) || (!isValidDateTime))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    Expense expense = new Expense()
                    {
                        ExpenseName = expensesDto.ExpenseName,
                        Amount = expensesDto.Amount,
                        DueDate = dueDateParsed,
                        PaymentStatus = parsedPaymentStatus,
                        HouseholdId = expensesDto.HouseholdId,
                        ServiceId = expensesDto.ServiceId
                    };

                    string amount = $"{expensesDto.Amount:F2}";
                    expensesToAdd.Add(expense);
                    sb.AppendLine(string.Format(SuccessfullyImportedExpense, expensesDto.ExpenseName, amount));
                }
                context.AddRange(expensesToAdd);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach (var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage;
            }

            return isValid;
        }
    }
}
