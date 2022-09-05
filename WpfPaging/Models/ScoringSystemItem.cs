using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.Models.Enums;

namespace WpfPaging.Models
{
    public class ScoringSystemItem : IdEntity
    {
        public Bank Bank { get; set; }
        public int BankId { get; set; }

        public int? MinCondition { get; set; }
        public int? MaxCondition { get; set; }
        public int? ExactValue { get; set; }
        public int Points { get; set; }
        public ScoringItemType Type { get; set; }
    }
}
