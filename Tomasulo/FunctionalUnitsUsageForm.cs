using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tomasulo
{
    public partial class FunctionalUnitsUsageForm : Form
    {
        public FunctionalUnitsUsageForm()
        {
            InitializeComponent();
        }

        private void ResourcesForm_Load(object sender, EventArgs e)
        {
            ResourcesGV.DataSource = FuUsage.FUUsageDT();
        }

        public static void updateResourceTable(int iteration, int clockCycle, int instructioNum)
        {
            FuUsage.Update(iteration, clockCycle, instructioNum);
        }


    }
}
