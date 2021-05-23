using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xaml__
{
    public class Token
    {
        public int TableId { get; set; }
        public int RecordId { get; set; }
        public Token(int tableId, int recordId)
        {
            TableId = tableId;
            RecordId = recordId;
        }
        public override string ToString()
        {
            return "<" + TableId.ToString() + "," + RecordId.ToString() + ">";
        }
    }
}
