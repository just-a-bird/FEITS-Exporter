using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEITS.Model;

namespace FEITS.Controller
{
    public class ImportExportController
    {
        private IExportImportView view;
        private string messageScript;
        public string MessageScript { get { return messageScript; } }

        bool[] validChars = TextGeneration.ValidCharacters;

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
        }

        public void OnImportMsgChanged()
        {
            if(messageScript != view.MessageText)
                messageScript = view.MessageText;

            CheckForValidChars();
        }

        private void CheckForValidChars()
        {
            bool containsInvalids = false;
            List<char> inv = new List<char>();
            foreach(char c in messageScript.Where(c => !validChars[TextGeneration.GetValue(c)]))
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
    }
}
