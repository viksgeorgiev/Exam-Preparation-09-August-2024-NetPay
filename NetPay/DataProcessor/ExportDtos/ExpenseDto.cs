﻿using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos
{
    [XmlType("Expense")]
    public class ExpenseDto
    {
        [XmlElement(nameof(ExpenseName))]
        public string ExpenseName { get; set; } = null!;

        [XmlElement(nameof(Amount))]
        public string Amount { get; set; } = null!;

        [XmlElement(nameof(PaymentDate))]
        public string PaymentDate { get; set; } = null!;

        [XmlElement(nameof(ServiceName))]
        public string ServiceName { get; set; } = null!;
    }
}
