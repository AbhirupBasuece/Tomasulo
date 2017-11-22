namespace Tomasulo
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.dgvInstructionQueue = new System.Windows.Forms.DataGridView();
            this.dgvInstructionStatus = new System.Windows.Forms.DataGridView();
            this.dgvRob = new System.Windows.Forms.DataGridView();
            this.dgvResevationsS = new System.Windows.Forms.DataGridView();
            this.openTraceFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openCnfFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.InstructionCollectionButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.dgvLoadBuffer = new System.Windows.Forms.DataGridView();
            this.lblLoadBuffer = new System.Windows.Forms.Label();
            this.lblInstructionsFromTraceFile = new System.Windows.Forms.Label();
            this.lblReorderBuffer = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnLoadProgram = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnStep = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.ClockLbl = new System.Windows.Forms.Label();
            this.brnShowResources = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblFPMult = new System.Windows.Forms.Label();
            this.lblFPAdd = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCDB = new System.Windows.Forms.Label();
            this.lblCommit = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRunAll = new System.Windows.Forms.Button();
            this.trbSpeed = new System.Windows.Forms.TrackBar();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblCPI = new System.Windows.Forms.Label();
            this.lblCPIValue = new System.Windows.Forms.Label();
            this.lblInsPerCycle = new System.Windows.Forms.Label();
            this.dgvRegisterResultStatus = new System.Windows.Forms.DataGridView();
            this.lblRegisterResultStatus = new System.Windows.Forms.Label();
            this.btnLoadRegisters = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstructionQueue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstructionStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResevationsS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadBuffer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegisterResultStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvInstructionQueue
            // 
            this.dgvInstructionQueue.AllowUserToAddRows = false;
            this.dgvInstructionQueue.AllowUserToDeleteRows = false;
            this.dgvInstructionQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstructionQueue.Location = new System.Drawing.Point(208, 27);
            this.dgvInstructionQueue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvInstructionQueue.Name = "dgvInstructionQueue";
            this.dgvInstructionQueue.ReadOnly = true;
            this.dgvInstructionQueue.Size = new System.Drawing.Size(560, 212);
            this.dgvInstructionQueue.TabIndex = 4;
            // 
            // dgvInstructionStatus
            // 
            this.dgvInstructionStatus.AllowUserToAddRows = false;
            this.dgvInstructionStatus.AllowUserToDeleteRows = false;
            this.dgvInstructionStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInstructionStatus.Location = new System.Drawing.Point(207, 455);
            this.dgvInstructionStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvInstructionStatus.Name = "dgvInstructionStatus";
            this.dgvInstructionStatus.ReadOnly = true;
            this.dgvInstructionStatus.Size = new System.Drawing.Size(1108, 246);
            this.dgvInstructionStatus.TabIndex = 0;
            // 
            // dgvRob
            // 
            this.dgvRob.AllowUserToAddRows = false;
            this.dgvRob.AllowUserToDeleteRows = false;
            this.dgvRob.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRob.Location = new System.Drawing.Point(208, 267);
            this.dgvRob.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvRob.Name = "dgvRob";
            this.dgvRob.ReadOnly = true;
            this.dgvRob.Size = new System.Drawing.Size(1108, 162);
            this.dgvRob.TabIndex = 0;
            // 
            // dgvResevationsS
            // 
            this.dgvResevationsS.AllowUserToAddRows = false;
            this.dgvResevationsS.AllowUserToDeleteRows = false;
            this.dgvResevationsS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResevationsS.Location = new System.Drawing.Point(208, 725);
            this.dgvResevationsS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvResevationsS.Name = "dgvResevationsS";
            this.dgvResevationsS.ReadOnly = true;
            this.dgvResevationsS.Size = new System.Drawing.Size(1108, 165);
            this.dgvResevationsS.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1015, 25);
            this.toolStrip1.TabIndex = 25;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // InstructionCollectionButton
            // 
            this.InstructionCollectionButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.InstructionCollectionButton.Image = ((System.Drawing.Image)(resources.GetObject("InstructionCollectionButton.Image")));
            this.InstructionCollectionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InstructionCollectionButton.Name = "InstructionCollectionButton";
            this.InstructionCollectionButton.Size = new System.Drawing.Size(23, 22);
            this.InstructionCollectionButton.Text = "Instruction Collection";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // dgvLoadBuffer
            // 
            this.dgvLoadBuffer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLoadBuffer.Location = new System.Drawing.Point(796, 27);
            this.dgvLoadBuffer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvLoadBuffer.Name = "dgvLoadBuffer";
            this.dgvLoadBuffer.Size = new System.Drawing.Size(519, 210);
            this.dgvLoadBuffer.TabIndex = 0;
            //this.dgvLoadBuffer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLoadBuffer_CellContentClick);
            // 
            // lblLoadBuffer
            // 
            this.lblLoadBuffer.AutoSize = true;
            this.lblLoadBuffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblLoadBuffer.Location = new System.Drawing.Point(1011, 7);
            this.lblLoadBuffer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoadBuffer.Name = "lblLoadBuffer";
            this.lblLoadBuffer.Size = new System.Drawing.Size(93, 17);
            this.lblLoadBuffer.TabIndex = 31;
            this.lblLoadBuffer.Text = "Load Buffer";
            // 
            // lblInstructionsFromTraceFile
            // 
            this.lblInstructionsFromTraceFile.AutoSize = true;
            this.lblInstructionsFromTraceFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblInstructionsFromTraceFile.Location = new System.Drawing.Point(433, 7);
            this.lblInstructionsFromTraceFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInstructionsFromTraceFile.Name = "lblInstructionsFromTraceFile";
            this.lblInstructionsFromTraceFile.Size = new System.Drawing.Size(110, 17);
            this.lblInstructionsFromTraceFile.TabIndex = 32;
            this.lblInstructionsFromTraceFile.Text = "Input Program";
            // 
            // lblReorderBuffer
            // 
            this.lblReorderBuffer.AutoSize = true;
            this.lblReorderBuffer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblReorderBuffer.Location = new System.Drawing.Point(687, 247);
            this.lblReorderBuffer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReorderBuffer.Name = "lblReorderBuffer";
            this.lblReorderBuffer.Size = new System.Drawing.Size(116, 17);
            this.lblReorderBuffer.TabIndex = 33;
            this.lblReorderBuffer.Text = "Reorder Buffer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label8.Location = new System.Drawing.Point(687, 433);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 17);
            this.label8.TabIndex = 34;
            this.label8.Text = "Instruction Status";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label10.Location = new System.Drawing.Point(687, 705);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(153, 17);
            this.label10.TabIndex = 35;
            this.label10.Text = "Resevation Stations";
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Location = new System.Drawing.Point(12, 23);
            this.btnLoadConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(171, 49);
            this.btnLoadConfig.TabIndex = 3;
            this.btnLoadConfig.Text = "Load Config";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // btnLoadProgram
            // 
            this.btnLoadProgram.Enabled = false;
            this.btnLoadProgram.Location = new System.Drawing.Point(12, 80);
            this.btnLoadProgram.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadProgram.Name = "btnLoadProgram";
            this.btnLoadProgram.Size = new System.Drawing.Size(171, 49);
            this.btnLoadProgram.TabIndex = 8;
            this.btnLoadProgram.Text = "Load Program";
            this.btnLoadProgram.UseVisualStyleBackColor = true;
            this.btnLoadProgram.Click += new System.EventHandler(this.btnLoadProgram_Click);
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(96, 267);
            this.btnReset.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(73, 32);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // btnGo
            // 
            this.btnGo.Enabled = false;
            this.btnGo.Location = new System.Drawing.Point(12, 267);
            this.btnGo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(73, 32);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnStep
            // 
            this.btnStep.Enabled = false;
            this.btnStep.Location = new System.Drawing.Point(12, 306);
            this.btnStep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(73, 32);
            this.btnStep.TabIndex = 23;
            this.btnStep.Text = "Step";
            this.btnStep.UseVisualStyleBackColor = true;
            this.btnStep.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 352);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "Cycle:";
            // 
            // ClockLbl
            // 
            this.ClockLbl.AutoSize = true;
            this.ClockLbl.Location = new System.Drawing.Point(69, 352);
            this.ClockLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ClockLbl.Name = "ClockLbl";
            this.ClockLbl.Size = new System.Drawing.Size(16, 17);
            this.ClockLbl.TabIndex = 1;
            this.ClockLbl.Text = "0";
            // 
            // brnShowResources
            // 
            this.brnShowResources.Location = new System.Drawing.Point(12, 194);
            this.brnShowResources.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.brnShowResources.Name = "brnShowResources";
            this.brnShowResources.Size = new System.Drawing.Size(171, 49);
            this.brnShowResources.TabIndex = 29;
            this.brnShowResources.Text = "Show FU Usage";
            this.brnShowResources.UseVisualStyleBackColor = true;
            this.brnShowResources.Click += new System.EventHandler(this.brnShowResources_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(160, 747);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 679);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "FP MUL Length (I):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 715);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "FP Add Length (J):";
            // 
            // lblFPMult
            // 
            this.lblFPMult.AutoSize = true;
            this.lblFPMult.ForeColor = System.Drawing.Color.Black;
            this.lblFPMult.Location = new System.Drawing.Point(160, 679);
            this.lblFPMult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFPMult.Name = "lblFPMult";
            this.lblFPMult.Size = new System.Drawing.Size(0, 17);
            this.lblFPMult.TabIndex = 16;
            // 
            // lblFPAdd
            // 
            this.lblFPAdd.AutoSize = true;
            this.lblFPAdd.ForeColor = System.Drawing.Color.Black;
            this.lblFPAdd.Location = new System.Drawing.Point(160, 715);
            this.lblFPAdd.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFPAdd.Name = "lblFPAdd";
            this.lblFPAdd.Size = new System.Drawing.Size(0, 17);
            this.lblFPAdd.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 747);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 17);
            this.label7.TabIndex = 21;
            this.label7.Text = "INT,LD/SD,Branch:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 578);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Issue/Cycle (K):";
            // 
            // lblCDB
            // 
            this.lblCDB.AutoSize = true;
            this.lblCDB.ForeColor = System.Drawing.Color.Black;
            this.lblCDB.Location = new System.Drawing.Point(160, 645);
            this.lblCDB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCDB.Name = "lblCDB";
            this.lblCDB.Size = new System.Drawing.Size(0, 17);
            this.lblCDB.TabIndex = 20;
            // 
            // lblCommit
            // 
            this.lblCommit.AutoSize = true;
            this.lblCommit.ForeColor = System.Drawing.Color.Black;
            this.lblCommit.Location = new System.Drawing.Point(160, 609);
            this.lblCommit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCommit.Name = "lblCommit";
            this.lblCommit.Size = new System.Drawing.Size(0, 17);
            this.lblCommit.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 645);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "CDB/Cycle (N):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 609);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Commit/Cycle (M):";
            // 
            // btnRunAll
            // 
            this.btnRunAll.Enabled = false;
            this.btnRunAll.Location = new System.Drawing.Point(12, 385);
            this.btnRunAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(73, 32);
            this.btnRunAll.TabIndex = 36;
            this.btnRunAll.Text = "RunAll";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
            // 
            // trbSpeed
            // 
            this.trbSpeed.Location = new System.Drawing.Point(8, 450);
            this.trbSpeed.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trbSpeed.Maximum = 7;
            this.trbSpeed.Name = "trbSpeed";
            this.trbSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trbSpeed.Size = new System.Drawing.Size(139, 56);
            this.trbSpeed.TabIndex = 37;
            this.trbSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trbSpeed.Scroll += new System.EventHandler(this.trbSpeed_Scroll);
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(12, 431);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(49, 17);
            this.lblSpeed.TabIndex = 38;
            this.lblSpeed.Text = "Speed";
            // 
            // lblCPI
            // 
            this.lblCPI.AutoSize = true;
            this.lblCPI.Location = new System.Drawing.Point(16, 522);
            this.lblCPI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCPI.Name = "lblCPI";
            this.lblCPI.Size = new System.Drawing.Size(33, 17);
            this.lblCPI.TabIndex = 39;
            this.lblCPI.Text = "CPI:";
            // 
            // lblCPIValue
            // 
            this.lblCPIValue.AutoSize = true;
            this.lblCPIValue.Location = new System.Drawing.Point(53, 522);
            this.lblCPIValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCPIValue.Name = "lblCPIValue";
            this.lblCPIValue.Size = new System.Drawing.Size(0, 17);
            this.lblCPIValue.TabIndex = 40;
            // 
            // lblInsPerCycle
            // 
            this.lblInsPerCycle.AutoSize = true;
            this.lblInsPerCycle.ForeColor = System.Drawing.Color.Black;
            this.lblInsPerCycle.Location = new System.Drawing.Point(160, 578);
            this.lblInsPerCycle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInsPerCycle.Name = "lblInsPerCycle";
            this.lblInsPerCycle.Size = new System.Drawing.Size(0, 17);
            this.lblInsPerCycle.TabIndex = 22;
            // 
            // dgvRegisterResultStatus
            // 
            this.dgvRegisterResultStatus.AllowUserToAddRows = false;
            this.dgvRegisterResultStatus.AllowUserToDeleteRows = false;
            this.dgvRegisterResultStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRegisterResultStatus.Location = new System.Drawing.Point(24, 902);
            this.dgvRegisterResultStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvRegisterResultStatus.Name = "dgvRegisterResultStatus";
            this.dgvRegisterResultStatus.ReadOnly = true;
            this.dgvRegisterResultStatus.Size = new System.Drawing.Size(1600, 137);
            this.dgvRegisterResultStatus.TabIndex = 41;
            // 
            // lblRegisterResultStatus
            // 
            this.lblRegisterResultStatus.AutoSize = true;
            this.lblRegisterResultStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblRegisterResultStatus.Location = new System.Drawing.Point(16, 874);
            this.lblRegisterResultStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRegisterResultStatus.Name = "lblRegisterResultStatus";
            this.lblRegisterResultStatus.Size = new System.Drawing.Size(171, 17);
            this.lblRegisterResultStatus.TabIndex = 42;
            this.lblRegisterResultStatus.Text = "Register Result Status";
            // 
            // btnLoadRegisters
            // 
            this.btnLoadRegisters.Location = new System.Drawing.Point(12, 138);
            this.btnLoadRegisters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadRegisters.Name = "btnLoadRegisters";
            this.btnLoadRegisters.Size = new System.Drawing.Size(171, 49);
            this.btnLoadRegisters.TabIndex = 171;
            this.btnLoadRegisters.Text = "Load Src Regs";
            this.btnLoadRegisters.UseVisualStyleBackColor = true;
            this.btnLoadRegisters.Click += new System.EventHandler(this.btnLoadRegisters_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(1636, 1050);
            this.Controls.Add(this.btnLoadRegisters);
            this.Controls.Add(this.lblRegisterResultStatus);
            this.Controls.Add(this.dgvRegisterResultStatus);
            this.Controls.Add(this.lblInsPerCycle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ClockLbl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCDB);
            this.Controls.Add(this.lblFPMult);
            this.Controls.Add(this.lblFPAdd);
            this.Controls.Add(this.lblCommit);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnStep);
            this.Controls.Add(this.lblCPIValue);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lblCPI);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.trbSpeed);
            this.Controls.Add(this.btnRunAll);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dgvResevationsS);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dgvInstructionStatus);
            this.Controls.Add(this.lblReorderBuffer);
            this.Controls.Add(this.dgvRob);
            this.Controls.Add(this.lblInstructionsFromTraceFile);
            this.Controls.Add(this.dgvInstructionQueue);
            this.Controls.Add(this.lblLoadBuffer);
            this.Controls.Add(this.dgvLoadBuffer);
            this.Controls.Add(this.brnShowResources);
            this.Controls.Add(this.btnLoadProgram);
            this.Controls.Add(this.btnLoadConfig);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximumSize = new System.Drawing.Size(2661, 2451);
            this.Name = "Main";
            this.Text = "Tomasulo Simulator!!";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstructionQueue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInstructionStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResevationsS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoadBuffer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRegisterResultStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvInstructionStatus;
        private System.Windows.Forms.DataGridView dgvRob;
        private System.Windows.Forms.DataGridView dgvResevationsS;
        private System.Windows.Forms.DataGridView dgvInstructionQueue;
        private System.Windows.Forms.OpenFileDialog openTraceFileDialog;
        private System.Windows.Forms.OpenFileDialog openCnfFileDialog;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton InstructionCollectionButton;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.DataGridView dgvLoadBuffer;
        private System.Windows.Forms.Label lblLoadBuffer;
        private System.Windows.Forms.Label lblInstructionsFromTraceFile;
        private System.Windows.Forms.Label lblReorderBuffer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.Button btnLoadProgram;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label ClockLbl;
        private System.Windows.Forms.Button brnShowResources;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFPMult;
        private System.Windows.Forms.Label lblFPAdd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCDB;
        private System.Windows.Forms.Label lblCommit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRunAll;
        private System.Windows.Forms.TrackBar trbSpeed;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblCPI;
        private System.Windows.Forms.Label lblCPIValue;
        private System.Windows.Forms.Label lblInsPerCycle;
        private System.Windows.Forms.DataGridView dgvRegisterResultStatus;
        private System.Windows.Forms.Label lblRegisterResultStatus;
        private System.Windows.Forms.Button btnLoadRegisters;
    }
}

