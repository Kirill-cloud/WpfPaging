using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.Models.Enums;

namespace WpfPaging.Messages
{
    public class ScoreSystemMessage : IMessage
    {
        public int ScoringItemId { get; set; }
        public ScoringItemType ScoringItemType { get; set; }
        public int BankId { get; set; }

        public ScoreSystemMessage(ScoringItemType scoringItemType, int bankId, int scoringItemId)
        {
            ScoringItemType = scoringItemType;
            BankId = bankId;
            ScoringItemId = scoringItemId;
        }
    }
}
