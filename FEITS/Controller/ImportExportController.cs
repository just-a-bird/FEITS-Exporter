using FEITS.Model;
using System.Collections.Generic;
using System.Linq;

namespace FEITS.Controller
{
    public class ImportExportController
    {
        private IExportImportView view;
        private string messageScript;
        public string MessageScript { get { return messageScript; } }
        private bool isMale;

        bool[] validChars = AssetGeneration.ValidCharacters;

        public ImportExportController(IExportImportView v, string currentMessage)
        {
            view = v;
            messageScript = currentMessage;
            view.SetController(this);
            SetupView();
        }

        private void SetupView()
        {
            view.MessageText = messageScript;
            CheckForGender();
        }

        public void OnImportMsgChanged()
        {
            if(messageScript != view.MessageText)
                messageScript = view.MessageText;

            CheckForValidChars();
        }

        private void CheckForValidChars()
        {
            if (messageScript.StartsWith("MESS_"))
            {
                view.AllowImport = true;
                view.StatusText = "";
                return;
            }

            bool containsInvalids = false;
            List<char> inv = new List<char>();
            foreach(char c in messageScript.Where(c => !validChars[AssetGeneration.GetValue(c)]))
            {
                if (!containsInvalids)
                    containsInvalids = true;
                if (!inv.Contains(c))
                    inv.Add(c);
            }

            if (containsInvalids)
            {
                view.AllowImport = false;
                view.StatusText = string.Format("Warning! Text contains one or more unsupported characters: {0}", string.Join(",", inv));
            }
            else
            {
                view.AllowImport = true;
                view.StatusText = "";
            }
        }

        private void CheckForGender()
        {
            if(messageScript.Contains("VOICE_PLAYER"))
            {
                view.ReversibleGenderCode = true;

                if(messageScript.Contains("VOICE_PLAYER_M#"))
                {
                    isMale = true;
                }
                else
                {
                    isMale = false;
                }
            }
            else
            {
                view.ReversibleGenderCode = false;
            }
        }

        public void ReverseGenderCode(bool reverseFromOriginal)
        {
            if(isMale)
            {
                if(reverseFromOriginal)
                    view.MessageText = messageScript.Replace("VOICE_PLAYER_M#", "VOICE_PLAYER_F#");
                else
                    view.MessageText = messageScript.Replace("VOICE_PLAYER_F#", "VOICE_PLAYER_M#");
            }
            else
            {
                if(reverseFromOriginal)
                    view.MessageText = messageScript.Replace("VOICE_PLAYER_F#", "VOICE_PLAYER_M#");
                else
                    view.MessageText = messageScript.Replace("VOICE_PLAYER_M#", "VOICE_PLAYER_F#");
            }

            view.StatusText = "NOTE: Gender has been swapped in code, but dialogue remains unchanged.";
        }
    }
}
