using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEITS
{
    public class Message
    {
        public string rawLine = string.Empty;
        public string spokenText = string.Empty;
        public int speechIndex = -1;
    }

    public class Command
    {
        public string cmd;
        public int numParams;
        public string[] Params;
    }
}
