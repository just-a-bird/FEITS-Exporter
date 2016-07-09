using System;
using System.Collections.Generic;
using FEITS.Model;
using System.Drawing;

namespace FEITS.Controller
{
    public interface IMainView
    {
        //Message controls
        int MsgListIndex { get; set; }
        string CurrentLine { get; set; }
        bool PrevLine { get; set; }
        bool NextLine { get; set; }
        Image PreviewImage { get; set; }

        //Options
        string ProtagonistName { get; set; }
        int CurrentTextbox { get; set; }
        bool EnableBackgrounds { get; set; }

        void SetController(MainController controller);
        void SetMessageList(List<MessageBlock> messages);

    }
}
