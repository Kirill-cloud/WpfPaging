using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPaging.Messages
{
    public class DependedObjectMessage : IMessage
    {
        public DependedObjectMessage(int sourceItemId, int dependItemId)
        {
            SourceItemId = sourceItemId;
            DependItemId = dependItemId;
        }

        public int SourceItemId { get; set; }
        public int DependItemId { get; set; }
    }
}
