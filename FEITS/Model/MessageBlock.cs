using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FEITS.Model
{
    /// <summary>
    /// Stores a line of dialogue with
    /// its prefix separated from the messages.
    /// </summary>
    public class MessageBlock
    {
        public string Prefix { get; set; } = string.Empty;
        public List<MessageLine> MessageLines { get; } = new List<MessageLine>();

        #region Parsing

        /// <summary>
        /// Returns a list of parsed message lines.
        /// </summary>
        /// <param name="message">The message to parse</param>
        public void ParseMessage(string message)
        {
            message = CommandTokens.NormalizeLineEndings(message);

            //Split the lines based on the line-end markers
            var splits = CommandTokens.SplitLines(message);

            foreach (var s in splits)
            {
                var newLine = new MessageLine {RawLine = s};
                MessageLines.Add(newLine);
            }
        }

        public static Tuple<string, Command> ParseCommand(string line, int offset)
        {
            var trunc = line.Substring(offset);
            var newCmd = new Command();

            foreach (var delim in CommandTokens.GetNoParamTokens())
            {
                if (!trunc.StartsWith(delim)) continue;

                newCmd.CommandWithPrefix = delim;
                newCmd.numParams = 0;
                newCmd.Params = new string[newCmd.numParams];
                line = line.Substring(0, offset) + line.Substring(offset + delim.Length);
                break;
            }

            foreach (var delim in CommandTokens.GetSingleParamTokens())
            {
                if (!trunc.StartsWith(delim)) continue;

                newCmd.CommandWithPrefix = delim;
                newCmd.numParams = 1;
                newCmd.Params = new string[newCmd.numParams];
                var index = line.IndexOf("|", offset, StringComparison.Ordinal);
                newCmd.Params[0] = line.Substring(offset + delim.Length, index - (offset + delim.Length));
                line = line.Substring(0, offset) + line.Substring(index + 1);
            }

            foreach (var delim in CommandTokens.GetDoubleParamTokens())
            {
                if (!trunc.StartsWith(delim)) continue;

                newCmd.CommandWithPrefix = delim;
                newCmd.numParams = 2;
                newCmd.Params = new string[newCmd.numParams];
                var index = line.IndexOf("|", offset, StringComparison.Ordinal);
                var index2 = line.IndexOf("|", index + 1, StringComparison.Ordinal);

                switch (delim)
                {
                    case CommandTokens.Srp:
                        Console.WriteLine(@"$Srp processed!");
                        break;
                    case CommandTokens.Wm:
                        newCmd.Params[0] = line.Substring(offset + delim.Length, index - (offset + delim.Length));
                        newCmd.Params[1] = line.Substring(index + 1, 1);
                        line = line.Substring(0, offset) + line.Substring(index + 2);
                        break;
                    default:
                        newCmd.Params[0] = line.Substring(offset + delim.Length, index - (offset + delim.Length));
                        newCmd.Params[1] = line.Substring(index + 1, index2 - (index + 1));
                        line = line.Substring(0, offset) + line.Substring(index2 + 1);
                        break;
                }
            }

            return new Tuple<string, Command>(line, newCmd);
        }

        #endregion

        public string CompileMessage(bool includePrefix = true)
        {
            //Recompile the message with the prefix and all lines
            var combinedLines = string.Empty;

            if (includePrefix)
                combinedLines = (Prefix != string.Empty ? Prefix + ": " : string.Empty);

            foreach (var line in MessageLines)
            {
                line.UpdateRawWithNewDialogue();
                combinedLines += line.RawLine;
            }

            combinedLines = combinedLines.Replace("\n", "\\n");
            return combinedLines;
        }
    }
}