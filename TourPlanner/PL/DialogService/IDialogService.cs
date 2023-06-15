using QuickMVVMSetup.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickMVVMSetup.BL.Services.MapQuest;

namespace QuickMVVMSetup.PL.DialogService
{
    internal interface IDialogService
    {
        (MapQuestRequestData start, MapQuestRequestData dest) ShowDialog(Action<string> callback);
    }
}
