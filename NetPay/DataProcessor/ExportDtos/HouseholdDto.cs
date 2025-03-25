using NetPay.Data.Models;
using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos
{
    [XmlType("Household")]
    public class HouseholdDto
    {
        [XmlElement(nameof(ContactPerson))]
        public string ContactPerson { get; set; } = null!;

        [XmlElement(nameof(Email))]
        public string? Email { get; set; }

        [XmlElement(nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = null!;

        [XmlArray(nameof(Expenses))]
        public ExpenseDto[] Expenses { get; set; } = null!;
    }
}
