using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Tomasulo
{
    //==========================================================================================================
    // - updates that still needs to be done as of : 14/11/2017
    // - Correct the cycles of execution. All of them. This needs to be input as per the reuirements of the user.
    // - Check for incorrect config bindings. For FP MUL
    // - Check for WPF execution order. -- URGENT        
    //==========================================================================================================
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles(); // has been done. ButtonClicks need to be updated
            
            Application.SetCompatibleTextRenderingDefault(false); // need to do this - done preliminary**
            Application.Run(new Main());
        }
    }
}