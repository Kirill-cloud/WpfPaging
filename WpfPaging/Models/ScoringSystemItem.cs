using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
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

        public bool IsValid
        {
            get
            {
                if (ExactValue.HasValue)
                {
                    if (MinCondition.HasValue || MaxCondition.HasValue)
                    {
                        return false;
                    }

                    if (ExactValue.Value < 0 || Points < 0)
                    {
                        return false;
                    }
                }

                if (!ExactValue.HasValue)
                {
                    if (!MinCondition.HasValue || !MaxCondition.HasValue)
                    {
                        return false;
                    }

                    if (Points < 0 || MinCondition.Value < 0 || MaxCondition < 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
