using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.Models.Enums;

namespace WpfPaging.Models
{
    public class User
    {
        public int? Age { get; set; }
        public int? KidsAmount { get; set; }
        public FamilyStates? FamilyState { get; set; }
        public JobTypes? JobType { get; set; }
        public QualificationTypes? Qualification { get; set; }
    }
}
