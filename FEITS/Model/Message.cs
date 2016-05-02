using System.Collections.Generic;

namespace FEITS.Model
{
    /// <summary>
    /// Stores a line of dialogue with
    /// its prefix separated from the messages.
    /// </summary>
    public class Message
    {
        public string prefix = string.Empty;
        public List<MessageLine> messages = new List<MessageLine>();

        public void ParseMessage(string message)
        {

        }
    }
}
