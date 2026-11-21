using System;
using System.Windows.Forms;

namespace CalendarApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new CalendarForm());
        }
    }
}

