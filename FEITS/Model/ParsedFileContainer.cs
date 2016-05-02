using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FEITS.Model
{
    /// <summary>
    /// Container for the contents of a parsed script file.
    /// </summary>
    public class ParsedFileContainer
    {
        public string header = string.Empty;
        public List<Message> messageList = new List<Message>();

        public void LoadFromFile(string fileName)
        {
            if(fileName != string.Empty)
            {
                string[] fileLines = File.ReadAllLines(fileName);

                //Parse for header and message blocks
                if(fileLines[0].StartsWith("MESS_"))
                {
                    //Find where the header ends
                    //Should be after "Message Name: Message"
                    int headerEndIndex = 0;
                    for(int i = 0; i < fileLines.Length; i++)
                    {
                        if(fileLines[i].Contains(":"))
                        {
                            if (headerEndIndex != 0)
                            {
                                headerEndIndex = i;
                                break;
                            }
                            else
                            {
                                headerEndIndex = i;
                                continue;
                            }
                        }
                    }

                    //If we found the header, add it to header
                    if(headerEndIndex != 0)
                    {
                        string newHeader = string.Empty;
                        for(int i = 0; i < headerEndIndex; i++)
                        {
                            newHeader += fileLines[i];
                        }

                        header = newHeader;

                        //Create messages from the rest of the lines
                        for(int i = headerEndIndex; i < fileLines.Length; i++)
                        {
                            //Separate the prefix from the message itself
                            if(fileLines[i].Contains(":"))
                            {
                                Message newMessage = new Message();
                                int prefixIndex = fileLines[i].IndexOf(":");
                                newMessage.prefix = fileLines[i].Substring(0, prefixIndex + 1);

                                //Get the message by itself
                                string message = fileLines[i].Substring(prefixIndex + 1);

                                //Make sure we haven't messed up
                                if(message.StartsWith(":"))
                                {
                                    MessageBox.Show("Error: There is a bug in the parsing code. Please inform the programmer of this issue.");

                                    return;
                                }
                                else if(char.IsWhiteSpace(message[0]))
                                {
                                    message = message.Remove(0, 1);
                                }

                                //Have the message parse the rest
                                newMessage.ParseMessage(message);

                                //Add it to our list
                                messageList.Add(newMessage);
                            }
                            else
                            {
                                MessageBox.Show("Error: Message lines don't appear to be formatted correctly. Please make sure each message is preceeded with a Message Name.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: File header doesn't appear to be formatted correctly. Please make sure the formatting is correct.");
                    }
                }
                else
                {
                    MessageBox.Show("Error: File contents don't appear to be formatted correctly. Please make sure the formatting is correct.");
                }
            }
        }

        public void SaveToFile(string fileName)
        {

        }
    }
}
