using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tomasulo
{
    class Execution
    {
        #region members

        private string currentInstType, /*prevInstType,*/ currentInstName, currentInstFullName;         // member variable for storing data on Instruction type being executed

        private int instNumberInQueue, issuePhaseCycles, exePhaseCyclesCount;          // member variables to control flow of execution on the operand

        private bool ROBbusy, ResevationStationBusy, LoadBufferBusy;                                // member variables to maintain status of ROB during execution

        private string destination;                                                                 

        private static Dictionary<string, Dictionary<int, int>> addMulTimers;                       // DataStructure to operate on multiply operations

        static int clock;
        private string result, calcResult, vj, vk, qj, qk;                                          // member variables to store the values from config files

        private static int FPAdd, FPMultiply, instructionIndex, InstructionLatest;                  // member variables to store the value of MUL process in ROB during execution
        
        private string[] comment;                                                                   // for updating comment in DataView
        private string fullComment;
        private int timer;

        private static int numOfUsedCDBCycles;                                                      // universal member variable to keep count on ROB cycles

        private string[] DependentIns;        private int[] numOfDependentIns;          private int[] CDBCycle;     //member variables for handling loops

        private string nameOfDependentInstruction;
        private int TempNumberOfDependecies;
        
        private int flag_2;                              
        private int CDBNewCycle;
        private bool IsCDBCycle;
                                                                                                                    
        private static int DifferentFUCounter;                                                       // member variables for keeping track of the Functional Units               
        private static int SameFUCounter;
        private static int IssuedInstCounter;
        private static bool IsInstructionIssued;
        //private static bool ThreeIssueInstHelperFlag;
        private static int NumOfCommitedIns;
        private bool Commit_2;

        #endregion                          

        #region Ctor
        public Execution()
        {    
            LoadDefaults();
        }


        #endregion

        #region props
        public Dictionary<string ,Dictionary<int,int>> AddMulTimers
        {
            get
            {
                return addMulTimers;
            }
            set
            {
                addMulTimers = value;
            }
        }
        public static int NumOfIssuedInst
        {
            get
            {
                return IssuedInstCounter;
            }
            set
            {
                IssuedInstCounter = value;
            }
        }

        public static int ResetDifferentFU
        {
            get
            {
                return DifferentFUCounter;
            }
            set
            {
                DifferentFUCounter = value;
            }
        }

        public static int ResetSameFU
        {
            get
            {
                return SameFUCounter;
            }
            set
            {
                SameFUCounter = value;
            }
        }

        public static bool SetInsIssued
        {
            get
            {
                return IsInstructionIssued;
            }
            set
            {
                IsInstructionIssued = value;
            }
        }

        public static int SetLastInsNum
        {
            get
            {
                return InstructionLatest;
            }
            set
            {
                InstructionLatest = value;
            }
        }

        public int ClockProp
        {
            get
            {
                return clock;
            }
            set
            {
                clock = value;
            }
        }

        public int FPMultProp
        {
            get
            {
                return FPMultiply;
            }
            set
            {
                FPMultiply = value;
            }
        }

        public int FPADdProp
        {
            get
            {
                return FPAdd;
            }
            set
            {
                FPAdd = value;
            }
        }

        public int instructionIndexProp
        {
            get
            {
                return instructionIndex;
            }
            set
            {
                instructionIndex = value;
            }
        }

        public static int SetNumOfCommitedIns
        {
            get
            {
                return NumOfCommitedIns;
            }

            set
            {
                NumOfCommitedIns = value;
            }
        }

        public static int SetNumOfCDBIns
        {
            get
            {
                return numOfUsedCDBCycles;
            }

            set
            {
                numOfUsedCDBCycles = value;
            }
        }

        public static int ClockTick()
        {
            clock++;
            return clock;
        }

        #endregion

        #region Methods

        /*--------------------------------------
         Function name   : ResetClock         
         Description     : reset clock to zero
         Return type     : static void 
        --------------------------------------*/
        public static void ResetClock()
        {
            clock = 0;
        }


        /*-------------------------------------------------------------------
         Function name   : GetArrayMaxValue
         Description     : finds the maximum index length for any given array
         Return type     : static integer 
         Argument        : int[] array
        -------------------------------------------------------------------*/
        public static int GetArrayMaxValue(int[] array)
        {
            int maxVal = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] > maxVal)
                {
                    maxVal = array[i];
                }
            }

            return maxVal;
        }


        /*-------------------------------------------------------------------------------------------------------------------------------------
         Function name   : Issue        
         Description     : This method handles the ISSUE command in the program. Basically it executes the 
                           number of instructions to be executed. For our case we have at max one instruction per step.
                           The instruction number is taken from InstructionFromInput 
         Return type     : boolean 
         Argument        : int instructionIndex, int numOfInsPerCycle, int IterationNum, int clock, int? numOfInsToCDB, int? numofInsToCommit
        --------------------------------------------------------------------------------------------------------------------------------------*/
        public bool Issue(int instructionIndex, int numOfInsPerCycle, int iterationNumber, int clock, int? numOfInsToCDB, int? numofInsToCommit)
        {
            instNumberInQueue = InstructionFromInput.InstructionsFromInputDT().Rows.Count;
            currentInstName = ConstructInstructionFullName(instructionIndex);
            if (instNumberInQueue > 0)
            {
                currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["Instruction Name"].ToString());
                currentInstName = InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["Instruction Name"].ToString();

                if (currentInstType.Equals("LD/SD"))
                {
                    LoadBufferBusy = LoadBuffer.IsBusy();
                }
                else
                {
                    ResevationStationBusy = ResevationStation.IsResevationStationBusy();
                }

                ROBbusy = ReorderBuffer.IsROBBusy();
            }

            if (!(ROBbusy && (LoadBufferBusy || (ResevationStationBusy
                && (currentInstType.Equals("LD/SD"))))))
            {

                switch (numOfInsPerCycle)
                {
                    case 1:
                        HandleOneInsPerCycle(instructionIndex, iterationNumber, clock);
                        break;
                   /* case 2:
                        HandleIssueTwoInsPerCycle(instructionIndex, numOfInsPerCycle, iterationNumber, clock);
                        break;
                    case 3:
                        HandleIssueThreeInsPerCycle(instructionIndex, numOfInsPerCycle, iterationNumber, clock);
                        break; */
                    default:
                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        /*------------------------------------------------------------------------------------------------------------------------
        // Function name   : ConstructInstructionFullName        
        // Description     : this method constructs the full syntax of instruction from the Instruction set defined in the program
        // Return type     : string 
        // Argument        : int InstructionIndex
        -------------------------------------------------------------------------------------------------------------------------*/
        private string ConstructInstructionFullName(int InstructionIndex)
        {
            return InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString()
                + " " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString()
                + ", " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()
                + ", " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString(); //Execution order differentiaL
        }

/*
        // remove from here
        //============================================================
        // Function name   : HandleThreeInsPerCycle         
        // Description     : for given instruction ,  for each FU , handles the state of 3 issues in one cycle
        // Return type     : void 
        // Argument        : int InstructionIndex
        // Argument        : int numOfInsPerCycle
        // Argument        : int IterationNum
        // Argument        : int clock
        //============================================================
        private void HandleIssueThreeInsPerCycle(int InstructionIndex, int numOfInsPerCycle, int IterationNum, int clock) 
        {
            IsInstructionIssued = false;
            currentInstFullName = ConstructInstructionFullName(InstructionIndex);

            if (InstructionIndex == 0)
            {
                issuePhaseCycles = 1;

                if (issuePhaseCycles == clock)
                {
                    IsInstructionIssued = true;
                    InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, ""); // check if InstructionStatusManager recieves the value
                    InstructionLatest = InstructionIndex;
                    IssuedInstCounter++;
                }
            }
            else
            {
                if (InstructionIndex == 0)   // might be redundant-but not working otherwise
                {
                    issuePhaseCycles = 1;

                    if (issuePhaseCycles == clock)
                    {
                        IsInstructionIssued = true;
                        InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");
                        IssuedInstCounter++;
                    }
                }
                else
                {
                    if (InstructionIndex > (InstructionLatest) && IssuedInstCounter != numOfInsPerCycle && SameFUCounter == 0)
                    {
                        if (InstructionLatest == -1)
                        {
                            InstructionLatest = 0;
                        }

                        if (InstructionLatest == 0)
                        {
                            if (IssuedInstCounter != 0)
                            {
                                if ((InstructionIndex - InstructionLatest) == 1)
                                {
                                    prevInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex - 1]["Instruction Name"].ToString());
                                    currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString());
                                }
                                else
                                {
                                    prevInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex - 2]["Instruction Name"].ToString());
                                    currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString());
                                    ThreeIssueInstHelperFlag = true;
                                }
                            }
                            else
                            {
                                prevInstType = "NoInstructionIssued";
                                currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString());
                            }
                        }
                        else
                        {
                            if (IssuedInstCounter != 0)
                            {

                                if ((InstructionIndex - InstructionLatest) == 2)
                                {
                                    prevInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex - 1]["Instruction Name"].ToString());
                                    currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString());
                                }
                                else
                                {
                                    prevInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex - 2]["Instruction Name"].ToString());
                                    currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString());
                                    ThreeIssueInstHelperFlag = true;
                                }
                            }
                            else
                            {
                                prevInstType = "NoInstructionIssued";
                                currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString());
                            }
                        }

                        currentInstName = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString();

                        
                        if (prevInstType.Equals(currentInstType) || currentInstType.Equals("Branch"))
                        {
                            SameFUCounter++;
                            InstructionLatest = InstructionIndex - 1;
                            IsInstructionIssued = false;
                            ThreeIssueInstHelperFlag = false;
                        }
                        else
                        {
                            if (ThreeIssueInstHelperFlag) 
                            {
                                prevInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex - 1]["Instruction Name"].ToString());
                                currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString());
                                currentInstName = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString();

                                if (prevInstType.Equals(currentInstType) || currentInstType.Equals("Branch"))
                                {
                                    SameFUCounter++;
                                    InstructionLatest = InstructionIndex - 1;
                                    IsInstructionIssued = false;
                                    ThreeIssueInstHelperFlag = false;
                                }
                                else
                                {
                                    DifferentFUCounter++;
                                    issuePhaseCycles = clock;
                                    IssuedInstCounter++;
                                    IsInstructionIssued = true;
                                    InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");

                                    if (InstructionIndex == InstructionFromInput.InstructionsFromInputDT().Rows.Count - 1)
                                    {
                                        IssuedInstCounter = numOfInsPerCycle;
                                    }
                                }
                            }
                            else
                            {
                                DifferentFUCounter++;
                                issuePhaseCycles = clock;
                                IssuedInstCounter++;
                                IsInstructionIssued = true;
                                InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");

                                if (InstructionIndex == InstructionFromInput.InstructionsFromInputDT().Rows.Count - 1)
                                {
                                    IssuedInstCounter = numOfInsPerCycle;
                                }
                            }
 
                        }


                    }

                    if (IssuedInstCounter == 0 && InstructionIndex > InstructionLatest)
                    {
                        issuePhaseCycles = clock;
                        IssuedInstCounter++;
                        IsInstructionIssued = true;
                        InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");

                        if (InstructionIndex == InstructionFromInput.InstructionsFromInputDT().Rows.Count - 1)
                        {
                            InstructionLatest = InstructionIndex;
                            IssuedInstCounter = numOfInsPerCycle;

                        }

                    }
                }
            }

            if (IssuedInstCounter <= numOfInsPerCycle && IsInstructionIssued)
            {
                if (IssuedInstCounter == numOfInsPerCycle)
                {
                    IsInstructionIssued = false;
                    InstructionLatest = InstructionIndex;
                }
                switch (currentInstType)
                {
                    case "LD/SD":
                        if (currentInstName.Equals("LD") || currentInstName.Equals("L.D"))
                        {
                            result = "Mem[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + " + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]]";
                         //   result = "Mem[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + " + "RegisterResultStatus.RegisterResultStatusDT().Rows[2][];
                            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                            {
                                destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();
                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                                }
                            }
                            else
                            {
                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                }
                            }
                            if (issuePhaseCycles == clock)
                            {
                                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result,"");
                                LoadBuffer.Update(LoadBuffer.GetBusyFalseItem(), true, result);
                            }
                        }
                        else 
                        {
                            result = "Mem[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + " + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]]";
                            if (issuePhaseCycles == clock)
                            {
                                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", result, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(),"");
                            }
                        }
                        break;
                    case "FP Add":
                        if (currentInstName.Equals("ADD.D"))
                        {
                            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";

                            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " + " + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() +" = "
                                + (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) +  Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
                            //result = result + " -> " +CalcResult;
                            if (InstructionIndex == InstructionFromInput.InstructionsFromInputDT().Rows.Count -1)
                            {
                                if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                                {
                                    destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                        ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result, calcResult);
                                    }
                                }
                                else
                                {
                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                    }
                                }
                                
                            }
                            else
                            {
                                if (issuePhaseCycles == clock)
                                {
                                    ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result, calcResult);
                                }

                                if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                                {
                                    destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                    }
                                }
                                else
                                {
                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                    }
                                }

                            }

                        }
                        else
                        {
                            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] - Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";

                            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " - " + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
                                + (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) - Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
                            
                            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                            {
                                destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                }
                            }
                            else
                            {
                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                }
                            }
                            if (issuePhaseCycles == clock)
                            {
                                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result, calcResult);
                            }



                        }

                        if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()) != "")
                        {
                            qj = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString());
                            vj = string.Empty;
                        }
                        else
                        {
                            qj = string.Empty;
                            vj = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "]";
                        }

                        if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()) != "")
                        {
                            qk = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString());
                            vk = string.Empty;
                        }
                        else
                        {
                            qk = string.Empty;
                            vk = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";
                        }

                        destination = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString());

                        if (issuePhaseCycles == clock)
                        {
                            ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("FP Add"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString(), vj, vk, qj, qk, destination, InstructionIndex);
                        }
                        break;
                    case "FP Multiply":
                        if (currentInstName.Equals("MUL.D"))
                        {
                            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] * Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";

                            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " * " + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
                              + (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) * Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
                            

                            if (issuePhaseCycles == clock)
                            {
                                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result, calcResult);
                            }

                            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                            {
                                destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                }
                            }
                            else
                            {
                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                }
                            }
                        }
                        else
                        {
                            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] / Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";

                            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " / " + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
                      + (Convert.ToDouble(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) / Convert.ToDouble(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
                    

                            if (issuePhaseCycles == clock)
                            {
                                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result, calcResult);
                            }

                            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                            {
                                destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                }
                            }
                            else
                            {
                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                }
                            }
                        }

                        if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()) != "")
                        {
                            qj = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString());
                            vj = string.Empty;
                        }
                        else
                        {
                            qj = string.Empty;
                            vj = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "]";
                        }

                        if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()) != "")
                        {
                            qk = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString());
                            vk = string.Empty;
                        }
                        else
                        {
                            qk = string.Empty;
                            vk = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";
                        }

                        destination = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString());
                        if (issuePhaseCycles == clock)
                        {
                            ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("FP Multiply"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString(), vj, vk, qj, qk, destination,InstructionIndex);
                        }

                        break;
                    case "Branch":
                        if (InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString().Equals("BEQ") 
                        || InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString().Equals("BNE"))
                        {
                            result = "Address[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";
                        }
                        else if (InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString().Equals("BEQZ")
                            || InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString().Equals("BNEZ"))
                        {
                            result = "Address[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "]";
                        }

                        if (issuePhaseCycles == clock)
                        {
                            ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", "", result,"");
                        }
                        break;
                    case "Integer":
                        result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] + " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString();

                        calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " + " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + " = "
  + (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) + Convert.ToInt32(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString())).ToString();

                        ResevationsStationUpdater(InstructionIndex);

                        if (issuePhaseCycles == clock)
                        {
                            ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("INTADD"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString(), vj, vk, qj, qk, destination, InstructionIndex);
                        }
                        if (issuePhaseCycles == clock)
                        {
                            ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result, calcResult);
                        }

                        if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                        {
                            destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                            if (issuePhaseCycles == clock)
                            {
                                RegisterStatus.Update((int.Parse(destination.Substring(1))) + 33, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                            }
                        }
                        else
                        {
                            if (issuePhaseCycles == clock)
                            {
                                RegisterStatus.Update((int.Parse(destination.Substring(1))) + 33, string.Empty, "No");
                            }
                        }

                        if (ReorderBuffer.GetRobNum(destination) != "None")
                        {
                            destination = ReorderBuffer.GetRobNum(destination);
                        }
                        break;
                    
                    default:
                        break;

                }
            }
        }


        /*-------------------------------------------------------------------------------------------------
         Function name   : HandleTwoInsPerCycle        
         Description     : for given instruction ,  for each FU , handles the state of 2 issues in on cycle
         Return type     : void 
         Argument        : int InstructionIndex
         Argument        : int numOfInsPerCycle
         Argument        : int IterationNum
         Argument        : int clock
        ---------------------------------------------------------------------------------------------------
        private void HandleIssueTwoInsPerCycle(int InstructionIndex, int numOfInsPerCycle, int IterationNum, int clock)
        {
            IsInstructionIssued = false;
            currentInstFullName = ConstructInstructionFullName(InstructionIndex);

            if (InstructionIndex == 0)
            {
                issuePhaseCycles = 1;

                if (issuePhaseCycles == clock)
                {
                    IssuedInstCounter++;
                    IsInstructionIssued = true;
                    InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");
                }
            }
            else
            {
                if (InstructionIndex == 0)
                {
                    issuePhaseCycles = 1;

                    if (issuePhaseCycles == clock)
                    {
                        IssuedInstCounter++;
                        IsInstructionIssued = true;
                        InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");
                    }
                }
                else
                {

                    if (InstructionIndex > (InstructionLatest + 1) && IssuedInstCounter != numOfInsPerCycle && SameFUCounter == 0)
                    {
                        if (InstructionLatest == -1)
                        {
                            InstructionLatest = 0;


                            for (int i = InstructionIndex; i > InstructionLatest; i--)
                            {
                                prevInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["Instruction Name"].ToString());
                                currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[i]["Instruction Name"].ToString());
                                currentInstName = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString();

                                if (prevInstType.Equals(currentInstType) ||currentInstName.Equals("Branch") || prevInstType.Equals("Branch"))
                                {
                                    issuePhaseCycles = clock;
                                    SameFUCounter++;

                                    if (IssuedInstCounter == 0)
                                    {
                                        IssuedInstCounter++;
                                        IsInstructionIssued = true;
                                        InstructionStatusManager.Insert(IterationNum, currentInstFullName, clock, 0, 0, 0, 0, "");

                                    }
                                    else
                                    {
                                        IsInstructionIssued = false;
                                    }
                                }
                                else if (SameFUCounter == 0)
                                {

                                    DifferentFUCounter++;

                                    issuePhaseCycles = clock;
                                    IssuedInstCounter++;
                                    IsInstructionIssued = true;
                                    InstructionStatusManager.Insert(IterationNum, currentInstFullName, clock, 0, 0, 0, 0, "");

                                }
                                else
                                {
                                    IssuedInstCounter++;
                                    IsInstructionIssued = true;
                                    InstructionStatusManager.Insert(IterationNum, currentInstFullName, clock, 0, 0, 0, 0, "");
                                }

                            }
                        }
                        else
                        {
                            for (int i = InstructionIndex; i > (InstructionLatest + 1); i--)
                            {
                                prevInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["Instruction Name"].ToString());
                                currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[i]["Instruction Name"].ToString());
                                currentInstName = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString();

                                if (string.Compare(prevInstType, currentInstType) == 0 || string.Compare(currentInstType, "Branch") == 0 || string.Compare(prevInstType, "Branch") == 0)
                                {
                                    SameFUCounter++;
                                    InstructionLatest = InstructionIndex - 1;
                                    IsInstructionIssued = false;
                                }

                                if (SameFUCounter == 0)
                                {
                                    DifferentFUCounter++;
                                    issuePhaseCycles = clock;
                                    IssuedInstCounter++;
                                    IsInstructionIssued = true;
                                    InstructionStatusManager.Insert(IterationNum, currentInstFullName, clock, 0, 0, 0, 0, "");
                                }

                            }
                        }
                    }
                }
                    if (IssuedInstCounter == 0 && InstructionIndex > InstructionLatest)
                    {
                        issuePhaseCycles = clock;
                        IssuedInstCounter++;
                        IsInstructionIssued = true;
                        InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");

                        if (InstructionIndex == InstructionFromInput.InstructionsFromInputDT().Rows.Count - 1)
                        {
                            InstructionLatest = InstructionIndex;
                            IssuedInstCounter = numOfInsPerCycle;

                        }

                    }


                }
                if (IssuedInstCounter <= numOfInsPerCycle && IsInstructionIssued)
                {
                    if (IssuedInstCounter == numOfInsPerCycle)
                    {
                        IsInstructionIssued = false;
                        InstructionLatest = InstructionIndex;
                    }
                    switch (currentInstType)
                    {
                        case "LD/SD":
                            if (currentInstName.Equals("LD") || currentInstName.Equals("L.D"))
                            {
                                result = "Mem[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + " + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]]";

                                if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                                {
                                    destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();
                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                                    }
                                }
                                else
                                {
                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                    }
                                }
                                if (issuePhaseCycles == clock)
                                {
                                    ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result,"");
                                    LoadBuffer.Update(LoadBuffer.GetBusyFalseItem(), true, result);
                                }
                            }
                            else
                            {
                                result = "Mem[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + " + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]]";
                                if (issuePhaseCycles == clock)
                                {
                                    ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", result, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(),"");
                                }
                            }
                            break;
                        case "FP Add":
                            if (currentInstName.Equals("ADD.D"))
                            {
                                result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";

                                calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " + " + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
+ (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) + Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
       

                                if (InstructionIndex == InstructionFromInput.InstructionsFromInputDT().Rows.Count - 1)
                                {
                                    if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                                    {
                                        destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                        if (issuePhaseCycles == clock)
                                        {
                                            RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                            ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result,calcResult);
                                        }
                                    }
                                    else
                                    {
                                        if (issuePhaseCycles == clock)
                                        {
                                            RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                        }
                                    }

                                }
                                else
                                {
                                    if (issuePhaseCycles == clock)
                                    {
                                        ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result,calcResult);
                                    }

                                    if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                                    {
                                        destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                        if (issuePhaseCycles == clock)
                                        {
                                            RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                        }
                                    }
                                    else
                                    {
                                        if (issuePhaseCycles == clock)
                                        {
                                            RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                        }
                                    }

                                }

                            }
                            else
                            {
                                result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] - Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";
                                calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " - " + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
+ (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) - Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
       

                                if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                                {
                                    destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                    }
                                }
                                else
                                {
                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                    }
                                }
                                if (issuePhaseCycles == clock)
                                {
                                    ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result,calcResult);
                                }



                            }

                            if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()) != "")
                            {
                                qj = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString());
                                vj = string.Empty;
                            }
                            else
                            {
                                qj = string.Empty;
                                vj = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "]";
                            }

                            if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()) != "")
                            {
                                qk = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString());
                                vk = string.Empty;
                            }
                            else
                            {
                                qk = string.Empty;
                                vk = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";
                            }

                            destination = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString());

                            if (issuePhaseCycles == clock)
                            {
                                ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("FP Add"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString(), vj, vk, qj, qk, destination,InstructionIndex);
                            }
                            break;
                        case "FP Multiply":
                            if (currentInstName.Equals("MUL.D"))
                            {
                                result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] * Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";

                                calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " * " + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
+ (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) * Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
       

                                if (issuePhaseCycles == clock)
                                {
                                    ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result,calcResult);
                                }

                                if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                                {
                                    destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                    }
                                }
                                else
                                {
                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                    }
                                }
                            }
                            else
                            {
                                result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] / Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";

                                calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " / " + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
+ (Convert.ToDouble(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) / Convert.ToDouble(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
       

                                if (issuePhaseCycles == clock)
                                {
                                    ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result,calcResult);
                                }


                                if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                                {
                                    destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();

                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][0].ToString(), "Yes");
                                    }
                                }
                                else
                                {
                                    if (issuePhaseCycles == clock)
                                    {
                                        RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                                    }
                                }
                            }

                            if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()) != "")
                            {
                                qj = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString());
                                vj = string.Empty;
                            }
                            else
                            {
                                qj = string.Empty;
                                vj = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "]";
                            }

                            if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()) != "")
                            {
                                qk = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString());
                                vk = string.Empty;
                            }
                            else
                            {
                                qk = string.Empty;
                                vk = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";
                            }

                            destination = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString());
                            if (issuePhaseCycles == clock)
                            {
                                ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("FP Multiply"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString(), vj, vk, qj, qk, destination, InstructionIndex);
                            }

                            break;
                        case "Branch":
                            if (InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString().Equals("BEQ")
                            || InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString().Equals("BNE"))
                            {
                                result = "Address[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]";
                            }
                            else if (InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString().Equals("BEQZ")
                                || InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString().Equals("BNEZ"))
                            {
                                result = "Address[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "]";
                            }

                            if (issuePhaseCycles == clock)
                            {
                                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", "", result,"");
                            }
                            break;
                        case "Integer":
                            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + "] + " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString();

                            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " + " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + " = "
      + (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) + Convert.ToInt32(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString())).ToString();
          
                            destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();
                            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
                            {

                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 33, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                                }
                            }
                            else
                            {
                                if (issuePhaseCycles == clock)
                                {
                                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 33, string.Empty, "No");
                                }
                            }

                            if (ReorderBuffer.GetRobNum(destination) != "None")
                            {
                                destination = ReorderBuffer.GetRobNum(destination);
                            }
                            ResevationsStationUpdater(InstructionIndex);

                            if (issuePhaseCycles == clock)
                            {
                                ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("INTADD"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString(), vj, vk, qj, qk, destination, InstructionIndex);
                            }
                            if (issuePhaseCycles == clock)
                            {
                                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString(), result,calcResult);
                            }
                            break;

                        default:
                            break;

                    }
                }
            
        }

       // till here

*/

        /*-----------------------------------------------------------------------------------------------------------------------------------------
         Function name   : HandleOneInsPerCycle        
         Description     : this method handles the FU control for every given instruction by verifying the condition for one issue in every cycle.
         Return type     : void 
         Argument        : int InstructionIndex
         Argument        : int IterationNum
         Argument        : int clock
        ------------------------------------------------------------------------------------------------------------------------------------------*/

            
        private void HandleOneInsPerCycle(int instructionId, int IterationNum, int clock)
        {
            currentInstFullName = ConstructInstructionFullName(instructionId);
            if (instructionId == 0)
            {
                issuePhaseCycles = 1;
                if (issuePhaseCycles == clock)
                {
                    InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");
                }
                
            }
            else
            {
                issuePhaseCycles++;
                if (issuePhaseCycles == clock)
                {
                    InstructionStatusManager.Insert(IterationNum, currentInstFullName, issuePhaseCycles, 0, 0, 0, 0, "");
                }
                
            }
            switch (currentInstType)
            {
                case "LD/SD":
                    HandleLoadStoreIssue(instructionId, clock);
                    break;
                case "FP Add":
                    HandleIssueAddSubstract(instructionId, clock);
                    break;
                case "FP Multiply":
                    HandleSingleIssueMult(instructionId, clock);
                    break;
                case "Branch":
                    HandleOneIssueBranch(instructionId, clock);
                    break;
                case "Integer":
                    HandleOneIssueINT(instructionId, clock);
                    break;
                default:
                    break;
            }
        }


        /*-------------------------------------------------------- 
         Function name   : HandleOneIssueINT         
         Description     : this method hanldes Integer operations 
         Return type     : void 
         Argument        : int InstructionIndex
         Argument        : int clock
        --------------------------------------------------------*/
        private void HandleOneIssueINT(int InstructionIndex, int clock)
        {
            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][2].ToString() + "] + " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][3].ToString();

            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + " + " + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + " = "
+ (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) + Convert.ToInt32(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString())).ToString();
          

            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
            {
                destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 33, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                }
            }
            else
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 33, string.Empty, "No");
                }
            }
            ResevationsStationUpdater(InstructionIndex);

            if (issuePhaseCycles == clock)
            {
                ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("INTADD"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString(), vj, vk, qj, qk, destination, InstructionIndex);
            }
            if (issuePhaseCycles == clock)
            {
                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][1].ToString(), result,calcResult);
               // ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("INTADD"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString(), vj, vk, qj, qk, Destination, InstructionIndex);
            }
            if (ReorderBuffer.GetRobNum(destination) != "None")             // taking care of an outlier
            {
                destination = ReorderBuffer.GetRobNum(destination);
            }
        }


        /*----------------------------------------------------------------------------
         Function name   : HandleOneIssueBranch         
         Description     : this method handles instructions that have branches in them
         Return type     : void 
         Argument        : int InstructionIndex
         Argument        : int clock
        -----------------------------------------------------------------------------*/
        private void HandleOneIssueBranch(int InstructionIndex, int clock)
        {
            if (InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString().Equals("BEQ") || 
                InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString().Equals("BNE"))
            {
                result = "Address[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][3].ToString() + "]";
            }
            else if (InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString().Equals("BEQZ") || 
                InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString().Equals("BNEZ"))
            {
                result = "Address[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][2].ToString() + "]";
            }

            if (issuePhaseCycles == clock)
            {
                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", "", result,"");
            }
        }


        /*-------------------------------------------------------------------------------------
         Function name   : HandleSingleIssueMult         
         Description     : this method handles all the Multiply operations in the instruction
         Return type     : void 
         Argument        : int InstructionIndex
         Argument        : int clock
        --------------------------------------------------------------------------------------*/
        private void HandleSingleIssueMult(int InstructionIndex, int clock)
        {
            ResevationsStationUpdater(InstructionIndex);
            if (string.Compare(currentInstName, "MUL.D") == 0)
            {
                HandleSingleIssueMulDiv(InstructionIndex, clock, '*');
            }
            else
            {
                HandleSingleIssueMulDiv(InstructionIndex, clock, '/');
            }
        }


        /*-----------------------------------------------------------------------------------
         Function name   : HandleSingleIssueMulDiv         
         Description     : this method handles all the divison functions in the instruction
         Return type     : void 
         Argument        : int InstructionIndex
         Argument        : int clock
         Argument        : char oper
        ------------------------------------------------------------------------------------*/
        private void HandleSingleIssueMulDiv(int InstructionIndex, int clock, char oper)
        {
            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][2].ToString() + "] " + oper + " Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][3].ToString() + "]";

                switch (oper)
	            {
                    case '*':
                        
            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + oper + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
                         + (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) * Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
            break;
                    case '/':

            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString() + oper + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString() + " = "
                         + (Convert.ToDouble(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString()].ToString()) / Convert.ToDouble(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString()].ToString())).ToString();
            break;
                    default:
                     break;
	            }

               
       

            destination = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString();
            
            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                }
            }
            else
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                }
            }
            if (issuePhaseCycles == clock)
            {
                destination = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString());
                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][1].ToString(), result,calcResult);
                ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("FP Multiply"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString(), vj, vk, qj, qk, destination, InstructionIndex);
            }
        }


        /*----------------------------------------------------------------------
         Function name   : HandleIssueAddSubstract         
         Description     : this method handles floating point ADD instructions
         Return type     : void 
         Argument        : int InstructionIndex
         Argument        : int clock
        -----------------------------------------------------------------------*/
        private void HandleIssueAddSubstract(int InstructionIndex, int clock)
        {
            if (currentInstName.Equals("ADD.D"))
            {
                HandleOneIssueAdd(InstructionIndex, clock);
            }
            else 
            {
                HandleOneIssueSubstract(InstructionIndex, clock);
            }

            ResevationsStationUpdater(InstructionIndex);

            if (issuePhaseCycles == clock)
            {
                ResevationStation.UpdateResevationStations(ResevationStation.GetFirstFreeIndex("FP Add"), 0, true, InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][0].ToString(), vj, vk, qj, qk, destination, InstructionIndex);
            }
        }


        /*----------------------------------------------------------------------------
        // Function name   : ResevationsStationUpdater         
        // Description     : this method updates the status of the Reservation Station
        // Return type     : void 
        // Argument        : int InstructionIndex
        -----------------------------------------------------------------------------*/
        private void ResevationsStationUpdater(int InstructionIndex)
        {
            if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][2].ToString()) != "")
            {
                qj = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][2].ToString());
                vj = string.Empty;
            }
            else
            {
                qj = string.Empty;
                vj = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][2].ToString() + "]";
            }

            if (RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][3].ToString()) != "")
            {
                qk = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][3].ToString());
                vk = string.Empty;
            }
            else
            {
                qk = string.Empty;
                vk = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex][3].ToString() + "]";
            }
            destination = RegisterStatus.GetROBID(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["DestReg"].ToString());
        }


        /*--------------------------------------------------------------------------------
         Function name   : HandleOneIssueSubstract         
         Description     : this method does the subtraction operation in the instruction
         Return type     : void 
         Argument        : int instructionIndex
         Argument        : int clock
        ---------------------------------------------------------------------------------*/
        private void HandleOneIssueSubstract(int instructionIndex, int clock)
        {
            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][2].ToString() + "] - Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][3].ToString() + "]";

                  
            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["SourceJ"].ToString()].ToString() + '-' + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["SourceK"].ToString()].ToString() + " = "
                         + (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["SourceJ"].ToString()].ToString()) - Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["SourceK"].ToString()].ToString())).ToString();

            destination = InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["DestReg"].ToString();
            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                }
            }
            else
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                }
            }
            if (issuePhaseCycles == clock)
            {
                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][1].ToString(), result,calcResult);
            }
        }


        /*--------------------------------------------------------------------------------
         Function name   : HandleOneIssueAdd         
         Description     : this method handles the addition operation in the instruction
         Return type     : void 
         Argument        : int instructionIndex
         Argument        : int clock
        --------------------------------------------------------------------------------*/
        private void HandleOneIssueAdd(int instructionIndex, int clock)
        {
            result = "Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][2].ToString() + "] + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][3].ToString() + "]";

            calcResult = RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["SourceJ"].ToString()].ToString() + '+' + RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["SourceK"].ToString()].ToString() + " = "
             + (Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["SourceJ"].ToString()].ToString()) + Convert.ToInt32(RegisterStatus.RegisterResultStatusDT().Rows[2][InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["SourceK"].ToString()].ToString())).ToString();


            destination = InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["DestReg"].ToString();
            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                }
            }
            else
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                }
            }
            if (issuePhaseCycles == clock)
            {
                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][1].ToString(), result,calcResult);
            }
        }


        /*----------------------------------------------------------
         Function name   : HandleLoadStoreIssue       
         Description     : this method handles LoadStore operations.
         Return type     : void 
         Argument        : int instructionIndex
         Argument        : int clock
        ----------------------------------------------------------*/
        private void HandleLoadStoreIssue(int instructionIndex, int clock)
        {
            if (currentInstName.Equals("LD") || currentInstName.Equals("L.D"))
            {
                HandleOneIssueLoad(instructionIndex, clock);
            }
            else
            {
                HandleStore(instructionIndex, clock);
            }
        }


        /*--------------------------------------------------------------
         Function name   : HandleStore         
         Description     : this method handles store operation in memory
         Return type     : void 
         Argument        : int instructionIndex
         Argument        : int clock
        --------------------------------------------------------------*/
        private void HandleStore(int instructionIndex, int clock)
        {
            result = "Mem[" + InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][2].ToString() + " + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][3].ToString() + "]]";
            if (issuePhaseCycles == clock)
            {
                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", result, InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][1].ToString(),"");
            }
        }


        /*--------------------------------------------------------------------
        // Function name   : HandleOneIssueLoad        
        // Description     : this method handles load operations in the memory
        // Return type     : void 
        // Argument        : int instructionIndex
        // Argument        : int clock
        --------------------------------------------------------------------*/
        private void HandleOneIssueLoad(int instructionIndex, int clock)
        {
            result = "Mem[" + InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][2].ToString() + " + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex][3].ToString() + "]]";
            destination = InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["DestReg"].ToString();
            if (!ReorderBuffer.ReorderBufferDT().Rows[ReorderBuffer.GetBusyFalseItem()][3].ToString().Equals("Commit"))
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, "ROB" + (ReorderBuffer.GetBusyFalseItem()).ToString(), "Yes");
                }
            }
            else
            {
                if (issuePhaseCycles == clock)
                {
                    RegisterStatus.Update((int.Parse(destination.Substring(1))) + 1, string.Empty, "No");
                }
            }
            if (issuePhaseCycles == clock)
            {
                ReorderBuffer.Update(ReorderBuffer.GetBusyFalseItem(), true, currentInstFullName, "Issue", InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["DestReg"].ToString(), result,"");
                LoadBuffer.Update(LoadBuffer.GetBusyFalseItem(), true, result);
            }
        }


        /*-----------------------------------------------------------------------------------------------
         Function name   : Execute         
         Description     : handling the execution phase in the algorithm
                           here  ExecutionManager , ROB and resevation stations will be updated
                           also , here dependencies will be resolved.
         Return type     : void 
         Argument        : int instNo
         Argument        : int numOfInsPerCycle
         Argument        : int IterationNum
         Argument        : int clock
         Argument        : int? numOfInsToCDB
         Argument        : int? numofInsToCommit
        --------------------------------------------------------------------------------------------------*/


        public void Execute(int instNo, int numOfInsPerCycle, int IterationNum, int clk, int? numOfInsToCDB, int? numofInsToCommit)
        {
            string prevDdest;
            int ROBIndex = 0;
            int dependenciesCounter = 0;
            timer = 0;
            currentInstName = ConstructInstructionFullName(instNo);
            string tempInst = string.Empty;
            string tempInst2 = string.Empty;

            if (instNo == 0)
            {
                if (clk == 2)       // check this part before final demo
                {
                    InstructionStatusManager.Update(instNo, IterationNum, currentInstName, 
                        int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString())),
                        2, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][4].ToString()), 
                        int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][5].ToString()), 
                        int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][6].ToString()), "");
                    
                    ReorderBuffer.Update(0, true, currentInstName, "Execute", ReorderBuffer.ReorderBufferDT().Rows[instNo][4].ToString( ),
                        ReorderBuffer.ReorderBufferDT().Rows[instNo][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][6].ToString());
                }
                return;
            }
            else if (InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString().Equals("S.D")
                || InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString().Equals("SD"))
            {
                if (numOfInsPerCycle != 1)
                {
                    if (clk >= instNo)
                    {
                        ROBIndex = UpdateExecutionAndRob(instNo, IterationNum, clk);
                    }
                }
                else
                {
                    if (clk > instNo)
                    {
                        ROBIndex = UpdateExecutionAndRob(instNo, IterationNum, clk);
                    }
                }
            }
            else
            {
                if (numOfInsPerCycle == 1)
                {
                    if (InstructionStatusManager.ExecutionManagerDT().Rows.Count > instNo)
                    {
                        if (clk > 2 && clk > (instNo - 1)
                            && InstructionStatusManager.ExecutionCycle(instNo) == 0)
                        {
                            prevDdest = InitAlgoTempVars(instNo, ref tempInst, ref tempInst2, ref dependenciesCounter);
                            if (dependenciesCounter != 0)
                            {
                                HandleDependencies(instNo, IterationNum, clk, ref ROBIndex, ref dependenciesCounter);
                            }
                            else
                            {
                                
                                if (clk > (instNo))
                                {
                                    if (clk - int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString()) == 1)
                                    {
                                        exePhaseCyclesCount = clk;
                                        InstructionStatusManager.Update(instNo, IterationNum, currentInstName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString())), exePhaseCyclesCount, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][4].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][5].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][6].ToString()), string.Empty);
                                        for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                                        {
                                            if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstName)
                                            {
                                                ROBIndex = j;
                                                ReorderBuffer.Update(ROBIndex, true, currentInstName, "Execute", ReorderBuffer.ReorderBufferDT().Rows[instNo][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][6].ToString());
                                                j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                                            }
                                        }
                                        SetNoDependenciesAlgoVars(instNo);

                                        if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Add")
                                            || InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Multiply")
                                            || InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
                                        {
                                            ResevationStation.UpdateResevationStations(ResevationStation.GetIndexOfIns(instNo), timer, true, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString(), vj, vk, string.Empty, string.Empty, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["DestReg"].ToString(), instNo);
                                        }
                                        return;
                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (InstructionStatusManager.ExecutionManagerDT().Rows.Count > instNo)
                    {
                        if (clk > 2 && clk >= (instNo - 1)
                            && InstructionStatusManager.ExecutionCycle(instNo) == 0)
                        {
                            prevDdest = InitAlgoTempVars(instNo, ref tempInst, ref tempInst2, ref dependenciesCounter);
                            if (dependenciesCounter != 0)
                            {
                                string tempComment;
                                tempComment = fullComment;
                                if (tempComment != string.Empty)
                                {
                                    tempComment = tempComment.Remove(0, 9).Substring(0, 3);
                                }
                                if (dependenciesCounter == 1 && (tempComment == "SD " || tempComment == "S.D"))
                                {
                                    dependenciesCounter = 0;
                                    InstructionStatusManager.ExecutionManagerDT().Rows[instNo][7] = string.Empty;

                                    if (clk - int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString()) == 1)
                                    {
                                        exePhaseCyclesCount = clk;
                                        InstructionStatusManager.Update(instNo, IterationNum, currentInstName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString())), exePhaseCyclesCount, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][4].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][5].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][6].ToString()), string.Empty);
                                        for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                                        {
                                            if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstName)
                                            {
                                                ROBIndex = j;
                                                ReorderBuffer.Update(ROBIndex, true, currentInstName, "Execute", ReorderBuffer.ReorderBufferDT().Rows[instNo][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][6].ToString());
                                                j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                                            }
                                        }
                                        SetNoDependenciesAlgoVars(instNo);

                                        if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Add")
                                            || InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Multiply")
                                             || InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
                                        {
                                            ResevationStation.UpdateResevationStations(ResevationStation.GetIndexOfIns(instNo), timer, true, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString(), vj, vk, string.Empty, string.Empty, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["DestReg"].ToString(), instNo);
                                        }
                                        return;
                                    }

                                }
                                else
                                {
                                    int DependentInstructionIndex;
                                    for (int i = 0; i < dependenciesCounter; i++)
                                    {
                                        numOfDependentIns[i] = ReorderBuffer.GetIndex(DependentIns[i]);
                                    }

                                    TempNumberOfDependecies = dependenciesCounter;

                                    for (int i = 0; i < TempNumberOfDependecies; i++)
                                    {
                                        if (string.Compare(ReorderBuffer.ReorderBufferDT().Rows[numOfDependentIns[i]][3].ToString(), "Write CDB") == 0 || string.Compare(ReorderBuffer.ReorderBufferDT().Rows[numOfDependentIns[i]][3].ToString(), "Commit") == 0)
                                        {
                                            dependenciesCounter--;
                                            nameOfDependentInstruction = DependentIns[i];
                                            DependentInstructionIndex = 0;

                                            for (int j = 0; j < InstructionStatusManager.ExecutionManagerDT().Rows.Count; j++)
                                            {
                                                if (string.Compare(nameOfDependentInstruction, InstructionStatusManager.ExecutionManagerDT().Rows[j][1].ToString()) == 0)
                                                {
                                                    DependentInstructionIndex = j;
                                                    CDBCycle[flag_2] = InstructionStatusManager.GetCDBCycle(DependentInstructionIndex);
                                                    flag_2++;
                                                }
                                            }

                                            DependentIns[i] = string.Empty;
                                            if (dependenciesCounter == 0)
                                            {
                                                if (InstructionStatusManager.ExecutionManagerDT().Rows[instNo][7].ToString() != string.Empty)
                                                {
                                                    for (int t = 0; t < InstructionStatusManager.CommentForInst(instNo).Length; t++)
                                                    {
                                                        if (InstructionStatusManager.CommentForInst(instNo)[t] == "SD" ||
                                                            InstructionStatusManager.CommentForInst(instNo)[t] == "S.D" ||
                                                            InstructionStatusManager.CommentForInst(instNo)[t] == "BEQ" ||
                                                            InstructionStatusManager.CommentForInst(instNo)[t] == "BNE" ||
                                                            InstructionStatusManager.CommentForInst(instNo)[t] == "BEQZ" ||
                                                            InstructionStatusManager.CommentForInst(instNo)[t] == "BNEZ")
                                                        {
                                                            Commit_2 = true;
                                                        }
                                                    }
                                                }
                                                if (!Commit_2)
                                                {
                                                    if (clk > GetArrayMaxValue(CDBCycle))
                                                    {
                                                        for (int e = 0; e < TempNumberOfDependecies; e++)
                                                        {
                                                            CDBCycle[e] = 0;
                                                        }
                                                        
                                                        flag_2 = 0;
                                                        exePhaseCyclesCount = clk;
                                                        InstructionStatusManager.Update(instNo, IterationNum, currentInstName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString())), exePhaseCyclesCount, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][4].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][5].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][6].ToString()), "");
                                                        for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                                                        {
                                                            if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstName)
                                                            {
                                                                ROBIndex = j;
                                                                ReorderBuffer.Update(ROBIndex, true, currentInstName, "Execute", ReorderBuffer.ReorderBufferDT().Rows[instNo][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][6].ToString());
                                                                j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                                                            }
                                                        }


                                                        if (!InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("LD/SD")
                                                            &&!InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Branch")
                                                            &&!InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
                                                        {
                                                            SetReslovedDependenciesAlgoVars(instNo);
                                                            ResevationStation.UpdateResevationStations(ResevationStation.GetIndexOfIns(instNo),
                                                                timer, true, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()
                                                                , vj, vk, string.Empty, string.Empty,
                                                                ReorderBuffer.ReorderBufferDT().Rows[ROBIndex][0].ToString(), instNo);
                                                            return;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    int[] CommitCycles = new int[10];
                                                    int c = 0;
                                                    int Max = 0;
                                                    Commit_2 = false;
                                                    for (int q = 0; q < InstructionStatusManager.InstructionWithDepen(instNo).Length; q++)
                                                    {
                                                        for (int j = 0; j < InstructionStatusManager.ExecutionManagerDT().Rows.Count; j++)
                                                        {
                                                            if (InstructionStatusManager.InstructionWithDepen(instNo)[q] == InstructionStatusManager.ExecutionManagerDT().Rows[j][1].ToString())
                                                            {
                                                                CommitCycles[c] = int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[j][6].ToString());
                                                                c++;
                                                            }
                                                        }
                                                    }


                                                    for (int r = 0; r < CommitCycles.Length; r++)
                                                    {
                                                        if (CommitCycles[r] > Max)
                                                        {
                                                            Max = CommitCycles[r]; //Gets the commit cycle - check value during fp mul 
                                                        }
                                                    }

                                                    if (clk > Max)
                                                    {
                                                        if (!Commit_2)
                                                        {
                                                            ROBIndex = ExecuteMaxClock(instNo, IterationNum, clk, ROBIndex);
                                                        }
                                                        else
                                                        {
                                                            Commit_2 = false;
                                                            for (int q = 0; q < InstructionStatusManager.InstructionWithDepen(instNo).Length; q++)
                                                            {
                                                                for (int j = 0; j < InstructionStatusManager.ExecutionManagerDT().Rows.Count; j++)
                                                                {
                                                                    if (InstructionStatusManager.InstructionWithDepen(instNo)[q] == InstructionStatusManager.ExecutionManagerDT().Rows[j][1].ToString())
                                                                    {
                                                                        CommitCycles[c] = int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[j][6].ToString());
                                                                        c++;
                                                                    }
                                                                }
                                                            }

                                                            for (int r = 0; r < CommitCycles.Length; r++)
                                                            {
                                                                if (CommitCycles[r] > Max)
                                                                {
                                                                    Max = CommitCycles[r];
                                                                }
                                                            }

                                                            if (clk > Max)
                                                            {
                                                                ROBIndex = ExecuteMaxClock(instNo, IterationNum, clk, ROBIndex);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                for (int i = 0; i < 10; i++)
                                {
                                    DependentIns[i] = string.Empty;
                                    numOfDependentIns[i] = 0;
                                }

                                InstructionStatusManager.UpdateComment(currentInstName, fullComment);
                                return;
                            }
                            else
                            {
                                if (clk >= (instNo - 1))
                                {
                                    if (clk - int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString()) == 1)
                                    {
                                        exePhaseCyclesCount = clk;
                                        InstructionStatusManager.Update(instNo, IterationNum, currentInstName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString())), exePhaseCyclesCount, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][4].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][5].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][6].ToString()), string.Empty);
                                        for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                                        {
                                            if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstName)
                                            {
                                                ROBIndex = j;
                                                ReorderBuffer.Update(ROBIndex, true, currentInstName, "Execute", ReorderBuffer.ReorderBufferDT().Rows[instNo][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][6].ToString());
                                                j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                                            }
                                        }
                                        SetNoDependenciesAlgoVars(instNo);

                                        if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Add") || InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Multiply")
                                            || InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
                                        {
                                            ResevationStation.UpdateResevationStations(ResevationStation.GetIndexOfIns(instNo), 
                                                timer, true, 
                                                InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString(), 
                                                vj, vk, string.Empty, string.Empty,
                                                InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["DestReg"].ToString(), instNo);
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return;
        }


        //============================================================
        // Function name   : ExecuteMaxClock
        // Description     : This method is required to execute till 
        //                   the end of the clock cycle so that that 
        //                   final state of registers can be obtained.
        // Return type     : int 
        // Argument        : int instNo
        // Argument        : int IterationNum
        // Argument        : int clock
        // Argument        : int ROBIndex
        //============================================================
        private int ExecuteMaxClock(int instNo, int IterationNum, int clock, int ROBIndex)
        {
            flag_2 = 0;
            exePhaseCyclesCount = clock;
            InstructionStatusManager.Update(instNo, IterationNum, currentInstName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString())), exePhaseCyclesCount, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][4].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][5].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][6].ToString()), "");
            for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
            {
                if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstName)
                {
                    ROBIndex = j;
                    ReorderBuffer.Update(ROBIndex, true, currentInstName, "Execute", ReorderBuffer.ReorderBufferDT().Rows[instNo][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][6].ToString());
                    j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                }
            }


            if (!InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("LD/SD")
                && !InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Branch")
                && !InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
            {
                SetReslovedDependenciesAlgoVars(instNo);
                ResevationStation.UpdateResevationStations(ResevationStation.GetIndexOfIns(instNo), timer, true, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString(), vj, vk, string.Empty, string.Empty, RegisterStatus.RegisterResultStatusDT().Rows[0][InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["DestReg"].ToString()].ToString(), instNo);
            }
            return ROBIndex;
        }


        //===============================================================
        // Function name   : InitAlgoTempVars
        // Description     : initialization of execution method variables
        // Return type     : string 
        // Argument        : int instNo
        // Argument        : ref string tempInst
        // Argument        : ref string tempInst2
        // Argument        : ref int dependenciesCounter
        //===============================================================
        private string InitAlgoTempVars(int instNo, ref string tempInst, ref string tempInst2, ref int dependenciesCounter)
        {
            string PrevDdest = String.Empty;
            InitHazardDetectionTemps(instNo, ref tempInst, ref tempInst2);

            for (int i = instNo; i > 0; i--)
            {
                string currentTempInst = InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["Instruction Name"].ToString();
                if (currentTempInst.Equals("S.D")
                    || currentTempInst.Equals("SD")
                    || currentTempInst.Equals("BEQZ")
                    || currentTempInst.Equals("BNEZ"))
                {
                    if (currentTempInst.Equals("S.D")
                        || currentTempInst.Equals("SD"))
                    {
                        PrevDdest = InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["SourceK"].ToString();
                    }
                    else
                    {
                        PrevDdest = InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["DestReg"].ToString();
                    }
                    if (tempInst.Equals(PrevDdest))
                    {
                        dependenciesCounter = SaveDependencies(dependenciesCounter, i);
                    }

                }
                else
                {
                    PrevDdest = InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["DestReg"].ToString();
                    if (tempInst.Equals(PrevDdest) || tempInst2.Equals(PrevDdest))
                    {
                        dependenciesCounter = SaveDependencies(dependenciesCounter, i);
                    }
                }
            }

            fullComment = string.Empty;
            for (int i = 0; i < dependenciesCounter; i++)
            {
                if (dependenciesCounter - i != 1)
                {
                    fullComment += comment[i] + "; ";
                    comment[i] = string.Empty;
                }
                else
                {
                    fullComment += comment[i];
                    comment[i] = string.Empty;
                }
            }
            return PrevDdest;
        }


        //=====================================================
        // Function name   : HandleDependencies
        // Description     : in charge of depenencies resolving
        // Return type     : void 
        // Argument        : int instNo
        // Argument        : int IterationNum
        // Argument        : int clock
        // Argument        : ref int ROBIndex
        // Argument        : ref int dependenciesCounter
        //======================================================
        private void HandleDependencies(int instNo, int IterationNum, int clock, ref int ROBIndex, ref int dependenciesCounter)
        {
            string tempComment;
            tempComment = fullComment;

            if (tempComment != string.Empty)
            {
                tempComment = tempComment.Remove(0, 9).Substring(0, 3);
            }

            if (dependenciesCounter == 1 && (tempComment == "SD " || tempComment == "S.D"))     //waw,war
            {
                dependenciesCounter = 0;
                InstructionStatusManager.ExecutionManagerDT().Rows[instNo][7] = string.Empty;

                if (clock - int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString()) == 1)
                {
                    HandleNoDependencies(instNo, IterationNum, clock, ref ROBIndex);
                    return;
                }

            }
            else
            {
                int DependentInstructionIndex;
                for (int i = 0; i < dependenciesCounter; i++)
                {
                    numOfDependentIns[i] = ReorderBuffer.GetIndex(DependentIns[i]);
                }

                TempNumberOfDependecies = dependenciesCounter;

                for (int i = 0; i < TempNumberOfDependecies; i++)
                {
                    if (ReorderBuffer.ReorderBufferDT().Rows[numOfDependentIns[i]][3].ToString().Equals("Write CDB")
                        || ReorderBuffer.ReorderBufferDT().Rows[numOfDependentIns[i]][3].ToString().Equals("Commit"))
                    {
                        dependenciesCounter--;
                        nameOfDependentInstruction = DependentIns[i];
                        DependentInstructionIndex = 0;
                        
                        for (int j = 0; j < InstructionStatusManager.ExecutionManagerDT().Rows.Count; j++)      //Resolving dependency
                        {
                            if (nameOfDependentInstruction.Equals(InstructionStatusManager.ExecutionManagerDT().Rows[j][1].ToString()))
                            {
                                DependentInstructionIndex = j;
                                CDBCycle[flag_2] = InstructionStatusManager.GetCDBCycle(DependentInstructionIndex);
                                flag_2++;
                            }
                        }
                        
                        DependentIns[i] = string.Empty;
                        if (dependenciesCounter == 0)
                        {
                            if (clock > GetArrayMaxValue(CDBCycle))
                            {
                                for (int e = 0; e < TempNumberOfDependecies; e++)
                                {
                                    CDBCycle[e] = 0;
                                }

                                if (InstructionStatusManager.ExecutionManagerDT().Rows[instNo][7].ToString() != string.Empty)
                                {
                                    for (int y = 0; y < InstructionStatusManager.CommentForInst(instNo).Length; y++)
                                    {
                                        if (InstructionStatusManager.CommentForInst(instNo)[y] == "SD" ||
                                            InstructionStatusManager.CommentForInst(instNo)[y] == "S.D" ||
                                            InstructionStatusManager.CommentForInst(instNo)[y] == "BEQ" ||
                                            InstructionStatusManager.CommentForInst(instNo)[y] == "BNE" ||
                                            InstructionStatusManager.CommentForInst(instNo)[y] == "BEQZ" ||
                                            InstructionStatusManager.CommentForInst(instNo)[y] == "BNEZ")
                                        {
                                            Commit_2 = true;
                                        }
                                    }
                                }
                                if (!Commit_2)
                                {
                                    flag_2 = 0;
                                    exePhaseCyclesCount = clock;
                                    InstructionStatusManager.Update(instNo, IterationNum, currentInstName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString())), exePhaseCyclesCount, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][4].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][5].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][6].ToString()), "");
                                    for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                                    {
                                        if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstName)
                                        {
                                            ROBIndex = j;
                                            ReorderBuffer.Update(ROBIndex, true, currentInstName, "Execute", ReorderBuffer.ReorderBufferDT().Rows[instNo][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instNo][6].ToString());
                                            j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                                        }
                                    }

                                    if (!InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("LD/SD")
                                        && !InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Branch")
                                        && !InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
                                    {
                                        SetReslovedDependenciesAlgoVars(instNo);
                                        ResevationStation.UpdateResevationStations(ResevationStation.GetIndexOfIns(instNo), timer, true, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString(), vj, vk, string.Empty, string.Empty, RegisterStatus.RegisterResultStatusDT().Rows[0][InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["DestReg"].ToString()].ToString(), instNo);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < 10; i++)
            {
                DependentIns[i] = string.Empty;
                numOfDependentIns[i] = 0;
            }

            InstructionStatusManager.UpdateComment(currentInstName, fullComment);
            return;
        }


        //============================================================
        // Function name   : SetReslovedDependenciesAlgoVars
        // Description     : when dependency resolved the resevation station variables will be updated
        // Return type     : void 
        // Argument        : int instNo
        //============================================================
        private void SetReslovedDependenciesAlgoVars(int instNo)
        {
            if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Multiply"))
            {
                timer = this.AddMulTimers["FPMullTimers"][instNo];
            }
            else if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Add"))
            {
                timer = this.AddMulTimers["FPAddTimers"][instNo];
            }
            else if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
            {
                timer = this.AddMulTimers["INTADD"][instNo];
            }

            vk = string.Empty;
            vj = string.Empty;
            for (int j = 0; j < ReorderBuffer.GetBusyFalseItem(); j++)
            {
                if (ReorderBuffer.ReorderBufferDT().Rows[j][4].ToString().Equals(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceJ"].ToString()))
                {
                    vj = ReorderBuffer.ReorderBufferDT().Rows[j][5].ToString();
                }

                if (ReorderBuffer.ReorderBufferDT().Rows[j][4].ToString().Equals(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceK"].ToString()))
                {
                    vk = ReorderBuffer.ReorderBufferDT().Rows[j][5].ToString();
                }
            }

            if (string.IsNullOrEmpty(vj))
            {
                vj = "Reg[" + InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceJ"].ToString() + "]";
            }

            if (string.IsNullOrEmpty(vk))
            {
                vk = "Reg[" + InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceK"].ToString() + "]";
            }
        }


        //============================================================
        // Function name   : HandleNoDependencies
        // Description     : when  there is no dependency attached to instruction , te algorithm variables will be updated
        // Return type     : void 
        // Argument        : int instNo
        // Argument        : int IterationNum
        // Argument        : int clock
        // Argument        : ref int ROBIndex
        //============================================================
        private void HandleNoDependencies(int instNo, int IterationNum, int clock, ref int ROBIndex)
        {
            exePhaseCyclesCount = clock;
            InstructionStatusManager.Update(instNo,
                IterationNum,
                currentInstName,
                int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instNo][2].ToString())),
                exePhaseCyclesCount, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][4].ToString()),
                int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][5].ToString()),
                int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instNo][6].ToString()),
                string.Empty);

            for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
            {
                if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstName)
                {
                    ROBIndex = j;
                    ReorderBuffer.Update(ROBIndex,
                        true,
                        currentInstName,
                        "Execute",
                        ReorderBuffer.ReorderBufferDT().Rows[instNo][4].ToString(),
                        ReorderBuffer.ReorderBufferDT().Rows[instNo][5].ToString(),
                        ReorderBuffer.ReorderBufferDT().Rows[instNo][6].ToString());

                    j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                }
            }
            SetNoDependenciesAlgoVars(instNo);

            if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Add")
                || InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Multiply")
                || InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
            {
                ResevationStation.UpdateResevationStations(ResevationStation.GetIndexOfIns(instNo), timer, true, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString(), vj, vk, string.Empty, string.Empty, InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["DestReg"].ToString(), instNo);
            }
        }


        //============================================================
        // Function name   : SetNoDependenciesAlgoVars
        // Description     : when there is no dependency attached to the instruction , the resevation station variabls will updated
        // Return type     : void 
        // Argument        : int instNo
        //============================================================
        private void SetNoDependenciesAlgoVars(int instNo)
        {
            if (!InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("LD/SD")
                || !InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Branch"))
            {

                if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Multiply"))
                {
                    timer = this.AddMulTimers["FPMullTimers"][instNo];
                }
                else if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("FP Add"))
                {
                    timer = this.AddMulTimers["FPAddTimers"][instNo];
                }
                else if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString()).Equals("Integer"))
                {
                    timer = this.AddMulTimers["INTADD"][instNo];
                }
                if (string.IsNullOrEmpty(vj))
                {
                    vj = "Reg[" + InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceJ"].ToString() + "]";
                }

                if (string.IsNullOrEmpty(vk))
                {
                    vk = "Reg[" + InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceK"].ToString() + "]";
                }
            }
        }


        //============================================================
        // Function name   : SaveDependencies
        // Description     : updating depenedncies data structures
        // Return type     : int 
        // Argument        : int dependenciesCounter
        // Argument        : int i
        //============================================================
        private int SaveDependencies(int dependenciesCounter, int i)
        {
            comment[dependenciesCounter] = "Wait for " + InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["Instruction Name"].ToString()
                + " " + InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["DestReg"].ToString()
                + ", " + InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["SourceJ"].ToString()
                + ", " + InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["SourceK"].ToString();
            DependentIns[dependenciesCounter] = InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["Instruction Name"].ToString()
                + " " + InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["DestReg"].ToString()
                + ", " + InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["SourceJ"].ToString()
                + ", " + InstructionFromInput.InstructionsFromInputDT().Rows[i - 1]["SourceK"].ToString();
            dependenciesCounter++;
            return dependenciesCounter;
        }

        private static void InitHazardDetectionTemps(int instNo, ref string tempInst, ref string tempInst2)
        {
             if (InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString().Equals("S.D")
                || InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString().Equals("SD")
                || InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString().Equals("BEQZ")
                || InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString().Equals("BNEZ"))
            {
                tempInst = InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["DestReg"].ToString();
            }
            else if (InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString().Equals("BEQ")
                || InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["Instruction Name"].ToString().Equals("BNE"))
            {
                tempInst = InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["DestReg"].ToString();
                tempInst2 = InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceJ"].ToString();
            }
            else
            {
                tempInst = InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceJ"].ToString();
                tempInst2 = InstructionFromInput.InstructionsFromInputDT().Rows[instNo]["SourceK"].ToString();
            }
        }


        //============================================================
        // Function name   : UpdateExecutionAndRob
        // Description     : updating Reorder buffer and ExecutionManager
        // Return type     : int 
        // Argument        : int InstructionIndex
        // Argument        : int IterationNum
        // Argument        : int clock
        //============================================================
        private int UpdateExecutionAndRob(int InstructionIndex, int IterationNum, int clock)
        {
            int ROBIndex = 0;

            if (InstructionIndex > InstructionStatusManager.ExecutionManagerDT().Rows.Count -1)
            {
                return ROBIndex;
            }
            if (int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][2].ToString())) != 0
                && InstructionStatusManager.ExecutionCycle(InstructionIndex) == 0
                && int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][2].ToString()) != clock)
            {
                InstructionStatusManager.Update(InstructionIndex,
                    IterationNum,
                    currentInstName,
                    int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][2].ToString())), clock,
                    int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][4].ToString()),
                    int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][5].ToString()),
                    int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][6].ToString()), "");

                for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                {
                    if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstName)
                    {
                        ROBIndex = j;
                        ReorderBuffer.Update((int)ROBIndex, true, currentInstName, "Execute", ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][6].ToString());
                        j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                    }
                }
            }

            return ROBIndex;
        }


        //============================================================
        // Function name   : HandleCommonDataBus
        // Description     : Handling CDB for given instruction number
        // Return type     : bool 
        // Argument        : int InstructionIndex
        // Argument        : int numOfInsPerCycle
        // Argument        : int IterationNum
        // Argument        : int clock
        // Argument        : int numOfInsToCDB
        // Argument        : int? numofInsToCommit
        //============================================================
        public bool HandleCommonDataBus(int InstructionIndex, int numOfInsPerCycle, int IterationNum, int clock, int numOfInsToCDB, int? numofInsToCommit)
        {
            int ROBIndex;
            string val;
            val = "Mem[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceJ"].ToString() + " + Regs[" + InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["SourceK"].ToString() + "]]";
            currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString());
            currentInstName = InstructionFromInput.InstructionsFromInputDT().Rows[InstructionIndex]["Instruction Name"].ToString();
            flag_2 = 0;

            if (numOfInsPerCycle != 1)
            {
                if (clock >= InstructionIndex)
                {
                    ROBIndex = CDBGeneralHandle(InstructionIndex, IterationNum, clock, numOfInsToCDB, val);
                }
            }
            else
            {
                if (clock > InstructionIndex)
                {

                    ROBIndex = CDBGeneralHandle(InstructionIndex, IterationNum, clock, numOfInsToCDB, val);
                }
            }
            return true;
        }


        //============================================================
        // Function name   : CDBGeneralHandle
        // Description     : handling CDB and updating reorder budder index by given operation type
        // Return type     : int 
        // Argument        : int InstructionIndex
        // Argument        : int IterationNum
        // Argument        : int clock
        // Argument        : int numOfInsToCDB
        // Argument        : string val
        //============================================================
        private int CDBGeneralHandle(int InstructionIndex, int IterationNum, int clock, int numOfInsToCDB, string val)
        {
            int ROBIndex =0;
            if (numOfUsedCDBCycles < numOfInsToCDB)
            {
                if (currentInstType.Equals("FP Add")
                    || currentInstType.Equals("FP Multiply"))
                {
                    ROBIndex = HandleCDBAddMULT(InstructionIndex, IterationNum, clock);
                }
                else if (currentInstType.Equals("LD/SD"))
                {
                    if (currentInstName.Equals("LD")
                        || currentInstName.Equals("L.D"))
                    {
                        ROBIndex = HandleCDBLoad(InstructionIndex, IterationNum, clock, val);
                    }
                }
                else if (currentInstType.Equals("Integer"))
                {
                    ROBIndex = HandleCDBINT(InstructionIndex, IterationNum, clock);
                }
            }
            return ROBIndex;
        }


        //============================================================
        // Function name   : HandleCDBINT
        // Description     : handling CDB phase for integer operation
        // Return type     : int 
        // Argument        : int InstructionIndex
        // Argument        : int IterationNum
        // Argument        : int clock
        //============================================================
        private int HandleCDBINT(int InstructionIndex, int IterationNum, int clock)
        {
            int ROBIndex=0;
            if (clock > int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][3].ToString()) && int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][3].ToString()) != 0 && int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][5].ToString()) == 0)
            {
                InstructionStatusManager.Update(InstructionIndex, IterationNum, currentInstFullName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][2].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][3].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][4].ToString())), clock, 0, string.Empty);

                for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                {
                    if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstFullName)
                    {
                        ROBIndex = j;
                        ReorderBuffer.Update(ROBIndex, true, currentInstFullName, "Write CDB", ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][6].ToString());
                        j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                    }
                }

                numOfUsedCDBCycles++;
            }
            return ROBIndex;
        }


        //============================================================
        // Function name   : HandleCDBLoad
        // Description     : handling CDB phase for load/store operation
        // Return type     : int 
        // Argument        : int InstructionIndex
        // Argument        : int IterationNum
        // Argument        : int clock
        // Argument        : string val
        //============================================================
        private int HandleCDBLoad(int InstructionIndex, int IterationNum, int clock, string val)
        {
            int ROBIndex=0;
            if (ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex]["State"].ToString().Equals("Memory Read"))
            {
                if (int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][4].ToString()) != 0
                    && clock == (int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][4].ToString()) + 1))
                {
                    CDBNewCycle = clock;
                    while ((InstructionStatusManager.ExecutionManagerDT().Rows.Count) != flag_2)
                    {
                        for (int i = 0; i < InstructionStatusManager.ExecutionManagerDT().Rows.Count; i++)
                        {
                            if (CDBNewCycle == int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[i][5].ToString()))
                            {
                                if (IsCDBCycle)
                                {
                                    flag_2 = 0;
                                    IsCDBCycle = false;
                                }
                                CDBNewCycle += 1;
                                break;
                            }
                            else
                            {
                                flag_2++;
                            }
                        }
                    }
                    flag_2 = 0;


                    if (CDBNewCycle == clock)
                    {
                        InstructionStatusManager.Update(InstructionIndex, IterationNum, currentInstFullName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][2].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][3].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][4].ToString())), clock, 0, string.Empty);

                        for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                        {
                            if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstFullName)
                            {
                                ROBIndex = j;
                                ReorderBuffer.Update(ROBIndex, true, currentInstFullName, "Write CDB", ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][6].ToString());
                                j = ReorderBuffer.ReorderBufferDT().Rows.Count;

                                for (int t = 0; t < LoadBuffer.LoadBufferDT().Rows.Count; t++)
                                {
                                    if (string.Compare(LoadBuffer.LoadBufferDT().Rows[t][2].ToString(), val) == 0)
                                    {
                                        LoadBuffer.Delete(t);
                                    }
                                }
                            }
                        }

                        numOfUsedCDBCycles++;
                    }
                }
            }
            return ROBIndex;
        }


        //============================================================
        // Function name   : HandleCDBAddMULT
        // Description     : handling CDB phase for Add/sub operation
        // Return type     : int 
        // Argument        : int InstructionIndex
        // Argument        : int IterationNum
        // Argument        : int clock
        //============================================================
        private int HandleCDBAddMULT(int InstructionIndex, int IterationNum, int clock)
        {
            int ROBIndex=0;
            if ((ResevationStation.GetTimerOfIns(InstructionIndex) - 1 == 0) || (ResevationStation.GetTimerOfIns(InstructionIndex) == 0)
            && (ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex]["State"].ToString().Equals("Execute")))
            {
                InstructionStatusManager.Update(InstructionIndex, IterationNum, currentInstFullName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][2].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][3].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[InstructionIndex][4].ToString())), clock, 0, string.Empty);

                for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                {
                    if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == currentInstFullName)
                    {
                        ROBIndex = j;
                        ReorderBuffer.Update(ROBIndex, true, currentInstFullName, "Write CDB", ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[InstructionIndex][6].ToString());
                        j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                    }
                }

                numOfUsedCDBCycles++;
            }
            return ROBIndex;
        }

        private void LoadDefaults()
        {
            ROBbusy = true;
            ResevationStationBusy = true;
            LoadBufferBusy = true;
            result = string.Empty;
            clock = 0;
            InstructionLatest = -1;
            comment = new string[10];
            fullComment = string.Empty;
            numOfUsedCDBCycles = 0;
            DependentIns = new string[10];
            numOfDependentIns = new int[10];
            CDBCycle = new int[10];
            IsCDBCycle = true;
            flag_2 = 0;
            DifferentFUCounter = 0;
            SameFUCounter = 0;
            IssuedInstCounter = 0;
            IsInstructionIssued = true;
            //ThreeIssueInstHelperFlag = false;
            NumOfCommitedIns = 0;
            Commit_2 = false;
            issuePhaseCycles = 0;
            NumOfCommitedIns = 0;
        }


        //============================================================
        // Function name   : Commit
        // Description     : Handling Commit phase for given instruction number
        // Return type     : bool 
        // Argument        : int InstructionIndex
        // Argument        : int numOfInsPerCycle
        // Argument        : int IterationNum
        // Argument        : int clock
        // Argument        : int numOfInsToCDB
        // Argument        : int numofInsToCommit
        //============================================================
        public bool CommitInst(int instructionIndex, int insPerCycle, int iterationNum, int clock, int numOfInsToCDB, int numofInsToCommit)
        {
            string Register;
            bool isCommit = false;

            if (instructionIndex == 0)
            {
                isCommit = true;
            }
            else
            {
                if (ReorderBuffer.IsCommitFinished(ReorderBuffer.GetIndex(currentInstFullName)))
                {
                    isCommit = true;
                }
                else
                {
                    isCommit = false;
                }
            }


            if (NumOfCommitedIns < numofInsToCommit)
            {
                if (isCommit)
                {
                    if (ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][3].ToString().Equals("Write CDB"))
                    {
                        if (clock > int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][5].ToString()))
                        {
                            ReorderBuffer.Update(ReorderBuffer.GetIndex(ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][2].ToString()), 
                                false, ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][2].ToString(), 
                                "Commit", ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][4].ToString(), 
                                ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][5].ToString(),
                                ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][6].ToString());

                            Register = ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][4].ToString();

                            for (int j = 0; j < RegisterStatus.RegisterResultStatusDT().Columns.Count; j++)
                            {
                                if (string.Compare(Register, RegisterStatus.RegisterResultStatusDT().Columns[j].ColumnName) == 0)
                                {
                                    RegisterStatus.Delete(j);
                                }
                            }

                            InstructionStatusManager.Update(instructionIndex, 
                                iterationNum, 
                                currentInstFullName, 
                                int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][2].ToString())), 
                                int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][3].ToString())), 
                                int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][4].ToString())), 
                                int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][5].ToString())), 
                                clock, 
                                string.Empty);

                            if (ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][6] != DBNull.Value)
                            {
                                RegisterStatus.UpdateRegValue(
                                    Register, ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][6].ToString().Split(new char[] { '=' })[1].Trim());
                                
                            }
           

                            NumOfCommitedIns++;
                        }
                    }
                    else
                    {
                        currentInstType = InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instructionIndex]["Instruction Name"].ToString());
                        if (ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][3].ToString().Equals("Execute"))
                        {
                            if (currentInstType.Equals("LD/SD") || currentInstType.Equals("Branch"))
                            {
                                if (clock > int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][3].ToString()) && InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][7].ToString() == string.Empty)
                                {
                                    ReorderBuffer.Update(ReorderBuffer.GetIndex(ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][2].ToString()), false, ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][2].ToString(), "Commit", ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][5].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][6].ToString());
                                    Register = ReorderBuffer.ReorderBufferDT().Rows[instructionIndex][4].ToString();

                                    for (int j = 0; j < RegisterStatus.RegisterResultStatusDT().Columns.Count; j++)
                                    {
                                        if (string.Compare(Register, RegisterStatus.RegisterResultStatusDT().Columns[j].ColumnName) == 0)
                                        {
                                            RegisterStatus.Delete(j);
                                        }
                                    }

                                    InstructionStatusManager.Update(instructionIndex, iterationNum, currentInstFullName, int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][2].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][3].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][4].ToString())), int.Parse((InstructionStatusManager.ExecutionManagerDT().Rows[instructionIndex][5].ToString())), clock, string.Empty);
                                    NumOfCommitedIns++;
                                }
                            }
                        }
                    }
                }
            }


            return true;
        } 
        #endregion

    }
}
