using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEITS.Controller
{
    public interface IExportImportView
    {
        string MessageText { get; set; }
        string StatusText { get; set; }
        bool AllowImport { get; set; }

        void SetController(ImportExportController controller);
    }
}
