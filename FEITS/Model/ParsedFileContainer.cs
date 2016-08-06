using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FEITS.Model
{
    /// <summary>
    /// Container for the contents of a parsed script file.
    /// </summary>
    public class ParsedFileContainer
    {
        public string FilePath;
        public string[] Header;
        public List<MessageBlock> MessageList;

        public ParsedFileContainer()
        {
            EmptyFileData();
        }

        /// <summary>
        /// Removes any previous message data
        /// and returns the file to an empty state.
        /// </summary>
        public void EmptyFileData()
        {
            FilePath = string.Empty;
            Header = null;
            MessageList = new List<MessageBlock>();
        }

        #region Loading
        /// <summary>
        /// Reads lines from specified file
        /// and parses the information
        /// </summary>
        /// <param name="filePath">Specified file name</param>
        public bool LoadFromFile(string filePath)
        {
            if(filePath != string.Empty)
            {
                EmptyFileData();

                //Store the file path
                FilePath = filePath;

                //Store text into an array split by line breaks
                string[] fileLines = File.ReadAllLines(filePath);
                //int messageProgress;

                //Parse for header and message blocks
                if (fileLines[0].StartsWith("MESS_"))
                {
                    //Find where the header ends
                    //Should be after "Message Name: Message"
                    int headerEndIndex = 0;
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains(":"))
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

                    //If we found the header, add it to Header
                    if (headerEndIndex != 0)
                    {
                        Header = new string[headerEndIndex];
                        for (int i = 0; i < headerEndIndex; i++)
                        {
                            Header[i] = fileLines[i];
                        }

                        //Create messages from the rest of the lines
                        for (int i = headerEndIndex; i < fileLines.Length; i++)
                        {
                            //Separate the prefix from the message itself
                            if (fileLines[i].Contains(":"))
                            {
                                MessageBlock newMessage = new MessageBlock();
                                int prefixIndex = fileLines[i].IndexOf(":");
                                newMessage.Prefix = fileLines[i].Substring(0, prefixIndex);

                                //Get the message by itself
                                string message = fileLines[i].Substring(prefixIndex + 2);

                                //Make sure we didn't leave any prefix stuff behind
                                if (message.StartsWith(":"))
                                {
                                    message = message.Remove(0, 2);

                                    return false;
                                }
                                else if (char.IsWhiteSpace(message[0]))
                                {
                                    message = message.Remove(0, 1);
                                }

                                //Have the message parse the rest
                                newMessage.ParseMessage(message);

                                //Add it to our list
                                MessageList.Add(newMessage);
                            }
                            else
                            {
                                MessageBox.Show("Message lines don't appear to be formatted correctly. Please make sure each message is preceeded with a Message Name.", "Error");
                                return false;
                            }
                            //messageProgress = (int)((float)i / fileLines.Length * 100);
                        }
                    }
                    else
                    {
                        MessageBox.Show("File header doesn't appear to be formatted correctly. Please make sure the formatting is correct.", "Error");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("File contents don't appear to be formatted correctly. Please make sure the formatting is correct and the file is compatible with FEITS.", "Error");
                    return false;
                }
            }
            else
            {
                EmptyFileData();
                Console.WriteLine("Opened file was empty; treating as a new object and keeping the file path.");
                return true;
            }

            return true;
        }

        public bool LoadFromString(string messageString)
        {
            EmptyFileData();

            try
            {
                MessageBlock newMessage = new MessageBlock();
                newMessage.Prefix = "Imported Message";
                newMessage.ParseMessage(messageString);
                MessageList.Add(newMessage);
            }
            catch
            {
                return false;
            }

            foreach(MessageBlock mb in MessageList)
            {
                foreach(MessageLine ml in mb.MessageLines)
                {
                    //Console.WriteLine(ml.RawLine);
                }
            }

            return true;
        }
        #endregion

        #region Saving
        /// <summary>
        /// Compiles the message list and
        /// writes contents with header to file.
        /// </summary>
        /// <param name="filePath">File to save as</param>
        public bool SaveToFile(string filePath)
        {
            if(filePath != string.Empty)
            {
                //Update file path in case different
                FilePath = filePath;

                //Start compiling a string to make up the new file
                string newFileText = string.Empty;
                foreach(string str in Header)
                {
                    newFileText += str + Environment.NewLine;
                }

                foreach (MessageBlock msg in MessageList)
                {
                    string compiledMsg = msg.CompileMessage();
                    newFileText += (compiledMsg + Environment.NewLine);
                }

                File.WriteAllText(filePath, newFileText);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
