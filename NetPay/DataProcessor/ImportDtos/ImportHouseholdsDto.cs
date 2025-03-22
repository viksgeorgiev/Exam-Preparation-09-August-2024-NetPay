using System.Xml.Serialization;

namespace NetPay.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    [XmlType("Household")]
    public class ImportHouseholdsDto
    {
        [XmlElement(nameof(ContactPerson))]
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string ContactPerson { get; set; } = null!;

        [XmlElement(nameof(Email))]
        [MinLength(6)]
        [MaxLength(80)]
        public string? Email { get; set; }

        [XmlAttribute("phone")]
        [Required]
        [RegularExpression(@"^\+\d{3}/\d{3}-\d{6}$")]
        public string PhoneNumber { get; set; } = null!;
    }
}
