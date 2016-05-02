using System;
using System.Collections.Generic;
using FEITS.Model;

namespace FEITS.Controller
{
    interface IMainView
    {
        //Block controls
        List<string> BlockList { get; set; }

        //Message controls
        string CurrentLine { get; set; }
        string ActiveCharacter { get; set; }
        string CharacterPortrait { get; set; }
        string Emotion { get; set; }

        //Options
        string ProtagonistName { get; set; }
        List<string> CurrentTextbox { get; set; }
        bool EnableBackgrounds { get; set; }

        void SetController(MainController controller);
    }
}
