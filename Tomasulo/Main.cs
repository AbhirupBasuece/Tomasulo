using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;

namespace Tomasulo
{
    public partial class Main : Form
    {
        #region Members
        public int speed { get; set; }
        int numOfInsPerCycle;   
        int numOfInsToCommit;   
        int numOfInsToCDB;    
        int numOfIterations;
        int fpAddLength = 0;
        int fpMullLength = 0;
        Run run;
        Execution execution; 
        #endregion

        #region Methods
        
        public Main()
        {
            
            InitializeComponent();
            LoadDefaults();
        }

        // this contains default defined values for now. Will be updated as per Reservation station requirements
        private void LoadDefaults()   
        {
            this.trbSpeed.Value = 2;
            this.speed=this.trbSpeed.Value*100;
            numOfInsPerCycle = 1;   
            numOfInsToCommit = 1;   
            numOfInsToCDB = 1;      
            numOfIterations = 1;
        }

        private void ResetObjects() // on reset - internal
        {
            FuUsage.RefreshFUnits();
            InstructionSet.RefreshSet();
            LoadBuffer.Refresh();
            ReorderBuffer.Refresh();
            ResevationStation.Delete();
            InstructionStatusManager.Refresh();
            InstructionFromInput.Refresh();
            dgvInstructionQueue.DataSource = null;
            RegisterStatus.Refresh();
            InstructionStatusManager.ResetIsComment();
            run.ResetTimerFlag();
            Execution.SetInsIssued = false;
            Execution.SetLastInsNum = -1;
            Execution.SetNumOfCommitedIns = 0;
            Execution.ResetClock();
        }

        private void ResetControls()  // on reset - visi controls#1
        {
            btnLoadConfig.Enabled = true;
            btnLoadProgram.Enabled = false;
            btnGo.Enabled = false;
            btnReset.Enabled = false;
            btnStep.Enabled = false;
            btnRunAll.Enabled = false;
            ClockLbl.Text = "0";
            this.dgvRob.FirstDisplayedScrollingRowIndex =0;
        }

        private void ResetInputs() // oon reset - visi controls#2
        {
            lblCDB.Text = string.Empty;
            lblInsPerCycle.Text = string.Empty;
            lblCommit.Text = string.Empty;
            lblFPAdd.Text = string.Empty;
            lblFPMult.Text = string.Empty;
        }

        private static void BuildResources()
        {
            FuUsage[] Resrcs = new FuUsage[50];
            for (int i = 0; i < Resrcs.Length; i++)
            {
                Resrcs[i] = new FuUsage();
            }
        }


        //============================================================
        // Function name   : BuildRegisterResultStatus
         
        // Description     : constructing Register result status table
        // Return type     : static void 
        //============================================================
        private void BuildRegisterResultStatus()
        {
            
            RegisterStatus registerResultStatus = new RegisterStatus();
            Type t = registerResultStatus.GetType();
            PropertyInfo[] pi = t.GetProperties();
            foreach (PropertyInfo prop in pi)
            {
                prop.SetValue(registerResultStatus,"No",null);
            }
            for (int i = 0; i < 64; i++)
            {

              RegisterStatus.Insert(i+1,pi[i].GetValue(registerResultStatus,null).ToString());
            }
            this.dgvRegisterResultStatus.DataSource = RegisterStatus.RegisterResultStatusDT();
        }


        //============================================================
        // Function name   : BuildExecutionManager
         
        // Description     : constructing Instruction Status table
        // Return type     : void 
        //============================================================
        private void BuildExecutionManager()
        {
            InstructionStatusManager executionManager = new InstructionStatusManager();
            dgvInstructionStatus.DataSource = InstructionStatusManager.ExecutionManagerDT();
            dgvInstructionStatus.Columns[0].Visible = false;
            dgvInstructionStatus.Columns[1].Width = 110;
            dgvInstructionStatus.Columns[2].Width = 40;
            dgvInstructionStatus.Columns[3].Width = 60;
            dgvInstructionStatus.Columns[4].Width = 50;
            dgvInstructionStatus.Columns[5].Width = 45;
            dgvInstructionStatus.Columns[6].Width = 45;
            dgvInstructionStatus.Columns[7].Width = 420;
            dgvInstructionStatus.Columns[7].DefaultCellStyle.SelectionForeColor = Color.Black ;
        }


        //============================================================
        // Function name   : BuildResevationsStations
         
        // Description     : constructing resevation station table
        // Return type     : void 
        //============================================================
        private void BuildResevationsStations()
        {
            ResevationStation rs = new ResevationStation();
            dgvResevationsS.DataSource = ResevationStation.ResevationStationsDT();
            dgvResevationsS.Columns[0].Width = 40;
            dgvResevationsS.Columns[2].Width = 40;
        }


        //============================================================
        // Function name   : BuildReorderBuffer
         
        // Description     : constructing reorder buffer table
        // Return type     : void 
        // Argument        : int size
        //============================================================
        private void BuildReorderBuffer(int size)
        {
            ReorderBuffer reorderBuffer = new ReorderBuffer(size);
            dgvRob.DataSource = ReorderBuffer.ReorderBufferDT();
            dgvRob.Columns[0].Width = 44;
            dgvRob.Columns[1].Width = 40;
            dgvRob.Columns[3].Width = 80;
            dgvRob.Columns[5].Width = 200;
            dgvRob.Columns[6].Width = 110;
        }

        private static void BuildInstructionSet()
        {
            InstructionSet instructionSet = new InstructionSet();
        }

        private static void BuildInstructions()
        {
            InstructionFromInput instructionFromInput = new InstructionFromInput();
        }


        //============================================================
        // Function name   : LoadBufferInit
         
        // Description     : Load buffer initialization
        // Return type     : void 
        //============================================================
        private void LoadBufferInit()
        {
            LoadBuffer[] loadBufferArray = new LoadBuffer[5];

            for (int i = 0; i < loadBufferArray.Length; i++)
            {
                loadBufferArray[i] = new LoadBuffer();
                loadBufferArray[i].BusyProp = false;
                loadBufferArray[i].NameProp = "Load" + i;
                loadBufferArray[i].AddressProp = string.Empty;
            }

            LoadBuffer.Insert(loadBufferArray[0].BusyProp, loadBufferArray[0].NameProp, loadBufferArray[0].AddressProp);
            LoadBuffer.Insert(loadBufferArray[1].BusyProp, loadBufferArray[1].NameProp, loadBufferArray[1].AddressProp);
            LoadBuffer.Insert(loadBufferArray[2].BusyProp, loadBufferArray[2].NameProp, loadBufferArray[2].AddressProp);
            LoadBuffer.Insert(loadBufferArray[3].BusyProp, loadBufferArray[3].NameProp, loadBufferArray[3].AddressProp);
            LoadBuffer.Insert(loadBufferArray[4].BusyProp, loadBufferArray[4].NameProp, loadBufferArray[4].AddressProp);
            dgvLoadBuffer.DataSource = LoadBuffer.LoadBufferDT();
            dgvLoadBuffer.Columns[2].Width = 127;
        }


        //===========================================================================
        // Function name   : LoadConfigFile         
        // Description     : load the configuration varaibels which consist i,j,k,n,m
        // Return type     : void 
        //===========================================================================
        private void LoadConfigFile()
        {
            DataTable configData = new DataTable();
            string TextFile;
            int flag = 0;
            string[] ConfigLines = new string[2];
            ConfigLines[0] = "Parameter";
            ConfigLines[1] = "value";
            DialogResult result;
            openCnfFileDialog.Filter = "Configuration files (*.default)|*.default|All files (*.*)|*.*";
            result = openCnfFileDialog.ShowDialog();

            if (result == DialogResult.OK)      // reading the config file. No changes to be made on this WPF generated code.
            {
                try
                {
                    openCnfFileDialog.OpenFile();
                    TextFile = openCnfFileDialog.FileName;
                   


                    configData = TextFileToDataTable.GetDataTable(TextFile, "=", ConfigLines);

                    if (InstructionSet.Update("FP Multiply", int.Parse(configData.Rows[0][1].ToString())))
                    {
                        lblFPMult.Text = configData.Rows[0][1].ToString();
                        fpMullLength = int.Parse(configData.Rows[0][1].ToString()); //FU Multiply length
                        flag++;
                    }

                    if (InstructionSet.Update("FP Add", int.Parse(configData.Rows[1][1].ToString())))
                    {
                        
                        lblFPAdd.Text = configData.Rows[1][1].ToString();
                        fpAddLength = int.Parse(configData.Rows[1][1].ToString());  //FU Add length
                        flag++;
                    }
                    //if (InstructionSet.Update("Integer", int.Parse(configData.Rows[1][1].ToString())))
                    //{

                    //    lblFPAdd.Text = configData.Rows[1][1].ToString();
                    //    fpAddLength = int.Parse(configData.Rows[1][1].ToString());//FU Add length
                    //    flag++;
                    //}

                    numOfInsPerCycle = int.Parse(configData.Rows[2][1].ToString());
                    lblInsPerCycle.Text = configData.Rows[2][1].ToString();
                    numOfInsToCommit = int.Parse(configData.Rows[3][1].ToString());
                    lblCommit.Text = configData.Rows[3][1].ToString();
                    numOfInsToCDB = int.Parse(configData.Rows[4][1].ToString());
                    lblCDB.Text = configData.Rows[4][1].ToString();
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (flag == 2)
            {
                btnLoadConfig.Enabled = false;
                btnLoadProgram.Enabled = true;
            }
        }
        

        //============================================================
        // Function name   : RunAll        
        // Description     : Running all flow in one single run. the grids will update simultaneously
        // Return type     : void 
        //============================================================
        private void RunAll()
        {
            while (!this.dgvRob.Rows[dgvInstructionQueue.Rows.Count - 1].Cells["State"].Value.ToString().Equals("Commit"))  // checking for the last instruction set in the queue to verify if it was a COMMIT 
            {
                OneStepExecution(true);
            }
            // last instruction made a commit - simulation end
            SimulationEnd();
        }


        //============================================================
        // Function name   : OneStepExecution        
        // Description     : Running one step in flow. the grids will update simultaneously
        // Return type     : void 
        // Argument        : bool runAll
        //============================================================
        private void OneStepExecution(bool runAll)
        {
            ClockLbl.Text = Execution.ClockTick().ToString();
            FuUsage.Insert(numOfIterations, int.Parse(ClockLbl.Text));
            for (int i = 0; i < InstructionFromInput.InstructionsFromInputDT().Rows.Count; i++)
            {
                run.Step(numOfInsPerCycle, numOfInsToCDB, numOfIterations, i, numOfInsToCommit, int.Parse(ClockLbl.Text));
                if (runAll)
                {
                    int beforePause = this.speed; // signifying instant execution of the process
                    Thread.Sleep(this.speed);
                    Application.DoEvents();       // finishing execution
                }

            }
            ResetExecutionVars();
            GridsColoring();
        }

        private static void ResetExecutionVars()        // method to handle events on Reset button control
        {
            Execution.NumOfIssuedInst = 0;
            Execution.ResetDifferentFU = 0;
            Execution.ResetSameFU = 0;
            Execution.SetNumOfCommitedIns = 0;
            Execution.SetNumOfCDBIns = 0;
        }


        //============================================================
        // Function name   : InstructionStatusScrollRePosition
        // Description     : reposition the instruction status grid after the last committed instruction
        // Return type     : void 
        //============================================================
        private void InstructionStatusScrollRePosition()
        {
            for (int i = 0; i < dgvInstructionQueue.Rows.Count - 1; i++)
            {
                if (this.dgvRob.Rows[i].Index != 0)
                {
                    if (this.dgvRob.Rows[i].Cells["State"].Value.ToString() != "Commit"
                        && this.dgvRob.Rows[i - 1].Cells["State"].Value.ToString() == "Commit")
                    {
                        this.dgvRob.FirstDisplayedScrollingRowIndex = this.dgvRob.Rows[i - 1].Index;

                    }
                }

            }
        }
        

        //============================================================
        // Function name   : GridsColoring
         
        // Description     : color each entry on grids which are in execution phase
        // Return type     : void 
        //============================================================
        private void GridsColoring()
        {
            for (int i = 0; i < dgvResevationsS.Rows.Count; i++)
            {
                if (dgvResevationsS.Rows[i].Cells["Timer"].Value.ToString() != "0")
                {
                    dgvResevationsS.Rows[i].DefaultCellStyle.BackColor = Color.Salmon;
                }
                else
                    dgvResevationsS.Rows[i].DefaultCellStyle.BackColor = Color.White;

            }
            for (int i = 0; i <= dgvInstructionQueue.Rows.Count - 1; i++)
            {
                if (this.dgvRob.Rows[i].Cells["State"].Value.ToString() == "Execute")
                {
                    this.dgvRob.Rows[i].DefaultCellStyle.BackColor = Color.Salmon;
                    this.dgvInstructionStatus.Rows[i].DefaultCellStyle.BackColor = Color.Salmon;


                }
                else
                {
                    this.dgvRob.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    if (this.dgvInstructionStatus.Rows.GetLastRow(DataGridViewElementStates.None) >= i)
                    {
                        this.dgvInstructionStatus.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }

                }
                if (this.dgvRob.Rows[i].Index > 3)
                {
                    if (this.dgvRob.Rows[i].Cells["State"].Value.ToString() != "Commit"
                        && this.dgvRob.Rows[i - 2].Cells["State"].Value.ToString() == "Commit")
                    {
                        this.dgvRob.FirstDisplayedScrollingRowIndex = this.dgvRob.Rows[i - 2].Index;

                    }
                }

            }
        }

        //============================================================
        // Function name   : BuildFUTimers         
        // Description     : based on the configuration data - constructing FU Multiply and Add Timers for the Resevation Station
        // Return type     : void 
        //============================================================
        private void BuildFUTimers()
        {
            this.execution.AddMulTimers = new Dictionary<string, Dictionary<int, int>>();
            this.execution.AddMulTimers.Add("FPAddTimers", new Dictionary<int, int>());
            this.execution.AddMulTimers.Add("FPMullTimers", new Dictionary<int, int>());
            this.execution.AddMulTimers.Add("INTADD", new Dictionary<int, int>());
            for (int i = 0; i < InstructionFromInput.InstructionsFromInputDT().Rows.Count; i++)  // checking from the input instruction gridView to figure out what kind of instruction it is.
            {
                if (InstructionFromInput.InstructionsFromInputDT().Rows[i]["Instruction Name"].ToString().Equals("MUL.D")
                    || InstructionFromInput.InstructionsFromInputDT().Rows[i]["Instruction Name"].ToString().Equals("DIV.D"))   //For multiplication
                {
                    this.execution.AddMulTimers["FPMullTimers"].Add(i, fpMullLength);
                }
                if (InstructionFromInput.InstructionsFromInputDT().Rows[i]["Instruction Name"].ToString().Equals("ADD.D")       //For addition
                    || InstructionFromInput.InstructionsFromInputDT().Rows[i]["Instruction Name"].ToString().Equals("SUB.D"))
                {
                    this.execution.AddMulTimers["FPAddTimers"].Add(i, fpAddLength);
                }
                if (InstructionFromInput.InstructionsFromInputDT().Rows[i]["Instruction Name"].ToString().Equals("DADDIU"))
                    //|| InstructionFromInput.InstructionsFromInputDT().Rows[i]["Instruction Name"].ToString().Equals("SUB.D"))
                {
                    this.execution.AddMulTimers["INTADD"].Add(i, 1);
                }


            }
        }
        private void SimulationEnd()
        {
            string CPI = (Convert.ToDouble(this.ClockLbl.Text) / dgvInstructionQueue.Rows.Count).ToString("#0.000");
            this.btnStep.Enabled = false;
            this.btnGo.Enabled = false;
            this.btnReset.Enabled = true;
            MessageBox.Show("Execution for " + dgvInstructionQueue.Rows.Count + " Instructions Ended after " + this.ClockLbl.Text + " Cycles, CPI = " + CPI);
            this.lblCPIValue.Text = CPI;
        }

        #endregion

        #region Events
        private void Main_Load(object sender, EventArgs e)
        {
            run = new Run();
            execution = new Execution();   // Final Linking for assimilation with WPF. Creating an object of the Execution class helps to load the progrm in the desired way

            BuildResevationsStations();     //invoking necessary objects creating methods ..
            BuildExecutionManager();
            BuildRegisterResultStatus();
            LoadBufferInit();
            BuildInstructions();
            BuildInstructionSet();
            BuildResources();
            btnLoadConfig.Enabled = true;       // before enabling input configuration from user
            btnLoadProgram.Enabled = false;
            btnGo.Enabled = false;
            btnReset.Enabled = false;
            btnStep.Enabled = false;
        }

        private void btnLoadProgram_Click(object sender, EventArgs e)
        {
            DialogResult result;
            openTraceFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            string FilePath;
            result = openTraceFileDialog.ShowDialog();
            string[] parameters = new string[4];
            string[] UnknownIns = new string[50];
            parameters[0] = "OP";
            parameters[1] = "Destination";
            parameters[2] = "SourceJ";
            parameters[3] = "SourceK";
            DataTable instructionsDataTable = new DataTable();
            UnknownIns[0] = string.Empty;

            if (result == DialogResult.OK)
            {
                try
                {
                    openTraceFileDialog.OpenFile();
                    FilePath = openTraceFileDialog.FileName;

                    instructionsDataTable = TextFileToDataTable.GetDataTable(FilePath, " ", parameters);

                    for (int i = 0; i < instructionsDataTable.Rows.Count; i++)
                    {
                        InstructionFromInput.Insert(instructionsDataTable.Rows[i][0].ToString(), instructionsDataTable.Rows[i][1].ToString(), instructionsDataTable.Rows[i][2].ToString(), instructionsDataTable.Rows[i][3].ToString());
                    }
                    BuildFUTimers();
                    BuildReorderBuffer(InstructionFromInput.InstructionsFromInputDT().Rows.Count);
                }
                catch (ArgumentNullException ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            dgvInstructionQueue.DataSource = instructionsDataTable;
            foreach (DataGridViewColumn col in dgvInstructionQueue.Columns)
            {
                if (col.Name == "SourceK" || col.Name == "OP")
                {
                    continue;
                }
                else
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                }

            }
            dgvInstructionQueue.Columns["OP"].Width = 70;
            dgvInstructionQueue.Columns["SourceK"].Width = 130;
            btnLoadProgram.Enabled = false;

        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            LoadConfigFile();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetInputs();
            ResetObjects();
            ResetControls();
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            if (this.dgvRob.Rows[dgvInstructionQueue.Rows.Count - 1].Cells["State"].Value.ToString().Equals("Commit"))
            {
                SimulationEnd();
            }
            else
            {
                OneStepExecution(false);
            }


        }

        private void ResourceButton_Click(object sender, EventArgs e)
        {
            FunctionalUnitsUsageForm frm = new FunctionalUnitsUsageForm();
            frm.Show();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            btnGo.Enabled = true;
            btnReset.Enabled = true;
            btnStep.Enabled = true;
            btnRunAll.Enabled = false;
        }

        private void brnShowResources_Click(object sender, EventArgs e)
        {
            FunctionalUnitsUsageForm frm = new FunctionalUnitsUsageForm();
            frm.Show();
        }
        private void btnRunAll_Click(object sender, EventArgs e)
        {
            RunAll();
        }

        private void trbSpeed_Scroll(object sender, EventArgs e)
        {
            this.speed = this.trbSpeed.Value * 100;
            Application.DoEvents();
        }
        #endregion

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLoadRegisters_Click(object sender, EventArgs e)
        {

            InitRegisters initregistersFrm = new InitRegisters(GetSrcRegiters());
            initregistersFrm.ShowDialog();
            btnGo.Enabled = true;
            btnRunAll.Enabled = true;
        }

        private List<string> GetSrcRegiters()
        {
            List<string> srcRegs = new List<string>();
            //string srcRegs = "";
            foreach (DataRow row in InstructionFromInput.InstructionsFromInputDT().Rows)
            {
                if (row["SourceJ"].ToString().ToUpper().Contains("R") || row["SourceK"].ToString().ToUpper().Contains("F")
                    || row["SourceJ"].ToString().ToUpper().Contains("F") || row["SourceK"].ToString().ToUpper().Contains("R"))
                {

                    if (row["SourceJ"].ToString().ToUpper() != row["SourceK"].ToString().ToUpper())
                    {
                        if (row["SourceJ"].ToString().ToUpper().Contains("R") ||row["SourceJ"].ToString().ToUpper().Contains("F")  )
                        {
                            if ((!row["SourceJ"].ToString().ToUpper().Contains("FOO") && !row["SourceJ"].ToString().ToUpper().Contains("EMPTY")))
                            {
                                if (!srcRegs.Contains(row["SourceJ"].ToString()))
                                {
                                     srcRegs.Add(row["SourceJ"].ToString().ToUpper()); 
                                }
                               
                            }
                           
                        }
                        if (row["SourceK"].ToString().ToUpper().Contains("R") || row["SourceK"].ToString().ToUpper().Contains("F"))
                        {
                            if ((!row["SourceK"].ToString().ToUpper().Contains("FOO") && !row["SourceK"].ToString().ToUpper().Contains("EMPTY")))
                            {
                                if (!srcRegs.Contains(row["SourceK"].ToString()))
                                {
                                    srcRegs.Add(row["SourceK"].ToString().ToUpper());
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        if (!srcRegs.Contains(row["SourceJ"].ToString()))
                        {
                            srcRegs.Add(row["SourceJ"].ToString().ToUpper());
                        }
                    }
                }

            }
            return srcRegs;
        }

       
    }
}
