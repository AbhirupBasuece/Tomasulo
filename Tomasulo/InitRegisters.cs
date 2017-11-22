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
    public partial class InitRegisters : Form
    {
        public List<string> SrcRegs { get; set; }
        public Dictionary<string,string> RegValues{ get; set; }
        public InitRegisters(List<string> srcRegs)
        {
            this.SrcRegs = srcRegs;
            InitializeComponent();
        }

        private void InitRegisters_Load(object sender, EventArgs e)
        {
            foreach (string src in this.SrcRegs)
            {
                if (this.Controls.Find("txt"+src,false).Count()==1)
                {
                    this.Controls["txt" + src].Enabled = true;
                    ((Label)this.Controls["lbl" + src]).ForeColor =  Color.Green;
                }
                
            }
        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void btnSetRegisters_Click(object sender, EventArgs e)
        {
            bool isValid=true;
            decimal res;
            RegValues = new Dictionary<string, string>();
            foreach (string src in this.SrcRegs)
            {
               
                if (!string.IsNullOrEmpty(this.Controls["txt" + src].Text) && Decimal.TryParse(this.Controls["txt" + src].Text,out res))
                {
                    RegisterResultStatus.UpdateRegValue(src, this.Controls["txt" + src].Text);
                    RegValues.Add(src, this.Controls["txt" + src].Text); 
                }
                else
                {
                    isValid = false;
                    break;
                }
            }
            if (isValid)
            {
                this.Close(); 
            }
            else
            {
                MessageBox.Show("Please Fill all marked registers with decimal values");
            }
        }

        //private void ResourcesForm_Load(object sender, EventArgs e)
        //{
        //   // ResourcesGV.DataSource = FuncuinalUnitsUsage.FUUsageDT();
        //}

        //public static void updateResourceTable(int iteration, int clockCycle, int instructioNum)
        //{
        //    FuncuinalUnitsUsage.Update(iteration, clockCycle, instructioNum);
        //}


    }
}
