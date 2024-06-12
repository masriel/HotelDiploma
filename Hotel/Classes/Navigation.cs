using System;
using System.Windows;

namespace Hotel.Classes
{
    public class Navigation
    {
        // Метод для открытия нового окна и закрытия текущего окна
        public void OpenAsNewPage(Window newWindow, Window currentWindow)
        {
            if (newWindow == null || currentWindow == null)
            {
                throw new ArgumentNullException("Окно не должно быть null.");
            }

            newWindow.Show();
            currentWindow.Close();
        }

        // Метод для открытия окна в виде диалога
        public void OpenAsDialog(Window dialogWindow)
        {
            if (dialogWindow == null)
            {
                throw new ArgumentNullException("Окно не должно быть null.");
            }

            dialogWindow.ShowDialog();
        }
    }
}
