using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExecutionPlanner
{
    public class ChangeSets
    {
        public int count { get; set; }
        public CValue[] value { get; set; }
    }

    public class CValue
    {
        public string id { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        
        public DateTime timestamp { get; set; }
        public string location { get; set; }
        public bool messageTruncated { get; set; }
    }

   
}
