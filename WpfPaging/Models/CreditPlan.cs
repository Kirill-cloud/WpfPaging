using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.Models.Enums;

namespace WpfPaging.Models
{
    public class CreditPlan: IdEntity
    {
        public int MinimalScore { get; set; }
        public decimal Amount { get; set; }
        public CreditTarget Target { get; set; }
        public Bank Bank { get; set; }
        public int BankId { get; set; }
    }
}
