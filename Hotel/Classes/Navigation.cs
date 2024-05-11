using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hotel.Classes
{
    public class Navigation
    {
        public void OpenAsNewPage(Window ShowWindow, Window CloseWindow)
        {
            ShowWindow.Show();
            CloseWindow.Close();
        }

        public void OpenAsDialog(Window ShowDialogWindow)
        {
            ShowDialogWindow.ShowDialog();
        }
    }
}
