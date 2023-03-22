using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExecutionPlanner
{
    public class Changes
    {
        public int count { get; set; }
        public ChValue[] value { get; set; }
    }

        public class ChValue
        {
            public Item item { get; set; }
            public string changeType { get; set; }
        }

        public class Item
        {
            public int version { get; set; }
            public int size { get; set; }
            public string hashValue { get; set; }
            public string path { get; set; }
            public string url { get; set; }
        }

    
}
