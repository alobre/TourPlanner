using TourPlanner.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Services.MapQuest;

namespace TourPlanner.PL.DialogServices
{
    internal interface IDialogService
    {
        (MapQuestRequestData start, MapQuestRequestData dest) ShowDialog(Action<string> callback);
    }
}
