using TourPlanner.BL.Services;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL.Services.MapQuest;
using TourPlanner.UI.Views;

namespace TourPlanner.PL.DialogServices
{
    internal class DialogService : IDialogService
    {
        private MainVM viewModel;
        public DialogService(MainVM mainViewModel)
        {
            viewModel = mainViewModel; // Assign the MainVM instance to the field
        }
        public (MapQuestRequestData start, MapQuestRequestData dest) ShowDialog(Action<string> callback)
        {
            var dialog = new NewTourDialog(viewModel);
            dialog.Owner = Application.Current.MainWindow;

            EventHandler closeEventHandler = null;
            closeEventHandler = (s, e) =>
            {
                /*callback(dialog.DialogResult.ToString());
                dialog.Closed -= closeEventHandler;*/
                callback((bool)dialog.DialogResult ? "Success" : "Cancel"); // Invoke the callback with the appropriate result
                dialog.Closed -= closeEventHandler;
            };

            dialog.Closed += closeEventHandler;
            /*dialog.ShowDialog();*/

           /* if (dialog.ShowDialog() == true)
            {*/
                MapQuestRequestData start = new MapQuestRequestData(dialog.Start_AreaCode, dialog.Start_Address, dialog.Start_City, dialog.Start_Country);
                MapQuestRequestData dest = new MapQuestRequestData(dialog.Dest_AreaCode, dialog.Dest_Address, dialog.Dest_City, dialog.Dest_Country); ;
                return (start, dest);
        /*}
            else
            {
                // Return default values or handle cancellation accordingly
                return (null, null);
            }*/
        }
    }
}
