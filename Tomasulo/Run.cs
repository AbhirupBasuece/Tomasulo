using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tomasulo
{
    class Run
    {

        #region Members
        Execution execution;
        private bool flag;
        private int indexnum;
        private string instructionName;
        private bool[] TimerFlag; 
        #endregion

        #region Ctor
        public Run()
        {
            execution = new Execution();
            indexnum = 0;
            instructionName = string.Empty;
            flag = false;
            TimerFlag = new bool[7];

            for (int i = 0; i < 7; i++)
            {
                TimerFlag[i] = false;
            }
        } 
        #endregion

        #region Methods
        //============================================================
        // Function name   : ResetTimerFlag
        // Description     : Reset here
        // Return type     : void 
        //============================================================
        public void ResetTimerFlag()
        {
            for (int i = 0; i <7; i++)
            {
                TimerFlag[i] = false;
            }
        }

        //===================================================================================================================
        // Function name   : RunStep
        // Description     : handle one step in the algotithm for given instruction , this will stat the full algotithm flow 
        //                   which contains calls for the phases:  issue , execute , CDB , commit 
        // Return type     : void 
        // Argument        : int numOfInsPerCycle
        // Argument        : int numOfInsToCDB
        // Argument        : int iterationNum
        // Argument        : int instructionNum
        // Argument        : int numOfInsToCommit
        // Argument        : int clock
        //===================================================================================================================
        public void Step(int numOfInsPerCycle, int numOfInsToCDB, int iterationNum, int instructionNum, int numOfInsToCommit, int clock)
        {
            int ROBIndex;
            execution.Issue(instructionNum, numOfInsPerCycle, iterationNum, clock, null, null);
            execution.Execute(instructionNum, numOfInsPerCycle, iterationNum, clock, null, null);

            instructionName = InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString() + " " + InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][1].ToString() + ", " + InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][2].ToString() + ", " + InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][3].ToString();

            if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString()).Equals("LD/SD"))
            {
                if (!InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString().Equals("S.D")
                    && !InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString().Equals("SD"))
                {
                    ROBIndex = HandleLoadOper(iterationNum, instructionNum, clock);
                }

            }

            execution.HandleCommonDataBus(instructionNum, numOfInsPerCycle, iterationNum, clock, numOfInsToCDB, null);

            if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString()).Equals("FP Add"))
            {
                HandleAddOper(instructionNum);
            }

            if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString()).Equals("Integer"))
            {
                HandleINTOper(instructionNum);
            }

            if (InstructionSet.GetInstruction(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString()).Equals("FP Multiply"))
            {
                HandleMultOper(instructionNum);
            }


            if (instructionNum == InstructionFromInput.InstructionsFromInputDT().Rows.Count - 1)
            {
                flag = false;
            }

            execution.CommitInst(instructionNum, numOfInsPerCycle, iterationNum, clock, numOfInsToCDB, numOfInsToCommit);

            if (InstructionStatusManager.ExecutionManagerDT().Rows.Count > instructionNum)
            {
                FunctionalUnitsUsageForm.updateResourceTable(iterationNum, clock, instructionNum);
            }

        }


        //============================================================================================================
        // Function name   : HandleMultOper
        // Description     : handling FU Multiply operations . includes updating the relevant resevation station timer
        // Return type     : void 
        // Argument        : int instructionNum
        //============================================================================================================
        private void HandleMultOper(int instructionNum)
        {
            if (ResevationStation.GetTimerOfIns(instructionNum) != 0)
           //if (ResevationStation.GetTimerOfIns(instructionNum, InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum]["Instruction Name"].ToString()) != 0)
            {
                if (TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)])
               // if (TimerFlag[ResevationStation.GetIndexOfIns(instructionNum, InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum]["Instruction Name"].ToString())])
                {
                    execution.AddMulTimers["FPMullTimers"][instructionNum]--;
                    ResevationStation.UpdateTimer(ResevationStation.GetIndexOfIns(instructionNum), execution.AddMulTimers["FPMullTimers"][instructionNum]);
                    flag = true;
                    if (execution.AddMulTimers["FPMullTimers"][instructionNum] == 0)
                    {
                        ClearResevationStations(instructionNum);
                        ///
                        TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)] = false;
                        ///
                        execution.instructionIndexProp = ResevationStation.GetIndexOfIns(instructionNum);

                    }
                }
                else
                {
                    TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)] = true;
                }
            }
        }
        //private void HandleMultOper(int instructionNum)
        //{
        //    if (ResevationStation.GetTimerOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString()) != 0)
        //    {
        //        if (TimerFlag[ResevationStation.GetIndexOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString())])
        //        {
        //            execution.AddMulTimers["FPMullTimers"][instructionNum]--;
        //            ResevationStation.UpdateTimer(ResevationStation.GetIndexOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString()), execution.AddMulTimers["FPMullTimers"][instructionNum]);
        //            flag = true;
        //            if (execution.AddMulTimers["FPMullTimers"][instructionNum] == 0)
        //            {
        //                ClearResevationStations(instructionNum);
        //                execution.instructionIndexProp = ResevationStation.GetIndexOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString());

        //            }
        //        }
        //        else
        //        {
        //            TimerFlag[ResevationStation.GetIndexOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString())] = true;
        //        }
        //    }
        //}

        //============================================================
        // Function name   : HandleAddOper
        // Description     : handling FU Add operations . includes updating the relevant resevation station timer
        // Return type     : void 
        // Argument        : int instructionNum
        //============================================================
        private void HandleAddOper(int instructionNum)
        {
            if (ResevationStation.GetTimerOfIns(instructionNum) != 0)
            {
                if (TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)])
                {
                    execution.AddMulTimers["FPAddTimers"][instructionNum]--;
                    ResevationStation.UpdateTimer(ResevationStation.GetIndexOfIns(instructionNum), execution.AddMulTimers["FPAddTimers"][instructionNum]);
                    flag = true;
                    if (execution.AddMulTimers["FPAddTimers"][instructionNum] == 0)
                    {
                        ClearResevationStations(instructionNum);
                        ///
                        TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)] = false;
                        ///
                        execution.instructionIndexProp = ResevationStation.GetIndexOfIns(instructionNum);
                    }
                }
                else
                {
                    TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)] = true;
                }
            }
        }

        private void HandleINTOper(int instructionNum)
        {
            if (ResevationStation.GetTimerOfIns(instructionNum) != 0)
            {
                if (TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)])
                {
                    execution.AddMulTimers["INTADD"][instructionNum]--;
                    ResevationStation.UpdateTimer(ResevationStation.GetIndexOfIns(instructionNum), execution.AddMulTimers["INTADD"][instructionNum]);
                    flag = true;
                    if (execution.AddMulTimers["INTADD"][instructionNum] == 0)
                    {
                        ClearResevationStations(instructionNum);
                        ///
                        TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)] = false;
                        ///
                        execution.instructionIndexProp = ResevationStation.GetIndexOfIns(instructionNum);
                    }
                }
                else
                {
                    TimerFlag[ResevationStation.GetIndexOfIns(instructionNum)] = true;
                }
            }
        }

        //private void HandleAddOper(int instructionNum)
        //{
        //    if (ResevationStation.GetTimerOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString()) != 0)
        //    {
        //        if (TimerFlag[ResevationStation.GetIndexOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString())])
        //        {
        //            execution.AddMulTimers["FPAddTimers"][instructionNum]--;
        //            ResevationStation.UpdateTimer(ResevationStation.GetIndexOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString()), execution.AddMulTimers["FPAddTimers"][instructionNum]);
        //            flag = true;
        //            if (execution.AddMulTimers["FPAddTimers"][instructionNum] == 0)
        //            {
        //                ClearResevationStations(instructionNum);
        //                execution.instructionIndexProp = ResevationStation.GetIndexOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString());
        //            }
        //        }
        //        else
        //        {
        //            TimerFlag[ResevationStation.GetIndexOfIns(InstructionFromInput.InstructionsFromInputDT().Rows[instructionNum][0].ToString())] = true;
        //        }
        //    }
        //}

        private static void ClearResevationStations(int instructionNum)
        {
            ResevationStation.ResevationStationsDT().Rows[ResevationStation.GetIndexOfIns(instructionNum)][2] = false;
            ResevationStation.ResevationStationsDT().Rows[ResevationStation.GetIndexOfIns(instructionNum)][4] = string.Empty;
            ResevationStation.ResevationStationsDT().Rows[ResevationStation.GetIndexOfIns(instructionNum)][5] = string.Empty;
            ResevationStation.ResevationStationsDT().Rows[ResevationStation.GetIndexOfIns(instructionNum)][6] = string.Empty;
            ResevationStation.ResevationStationsDT().Rows[ResevationStation.GetIndexOfIns(instructionNum)][7] = string.Empty;
            ResevationStation.ResevationStationsDT().Rows[ResevationStation.GetIndexOfIns(instructionNum)][8] = string.Empty;
            ResevationStation.ResevationStationsDT().Rows[ResevationStation.GetIndexOfIns(instructionNum)][3] = string.Empty;
        }


        //============================================================
        // Function name   : HandleLoadOper
        // Description     : handling Load operations . includes updating the relevant resevation station timer
        // Return type     : int 
        // Argument        : int iterationNum
        // Argument        : int instructionNum
        // Argument        : int clock
        //============================================================
        private int HandleLoadOper(int iterationNum, int instructionNum, int clock)
        {
            int ROBIndex=0;
            for (int i = 0; i < InstructionStatusManager.ExecutionManagerDT().Rows.Count; i++)
            {
                if (InstructionStatusManager.ExecutionManagerDT().Rows[i][1].ToString().Equals(instructionName)
                    && int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[i][3].ToString()) != 0
                    && clock == int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[i][3].ToString()) + 1)
                {
                    indexnum = i;
                    InstructionStatusManager.Update(indexnum, iterationNum, instructionName, int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[indexnum][2].ToString()), int.Parse(InstructionStatusManager.ExecutionManagerDT().Rows[indexnum][3].ToString()), clock, 0, 0, "");
                    for (int j = 0; j < ReorderBuffer.ReorderBufferDT().Rows.Count; j++)
                    {
                        if (ReorderBuffer.ReorderBufferDT().Rows[j][2].ToString() == instructionName)
                        {
                            ROBIndex = j;
                            ReorderBuffer.Update(ROBIndex, true, instructionName, "Memory Read", ReorderBuffer.ReorderBufferDT().Rows[instructionNum][4].ToString(), ReorderBuffer.ReorderBufferDT().Rows[instructionNum][5].ToString(),"");
                            j = ReorderBuffer.ReorderBufferDT().Rows.Count;
                        }
                    }
                }
            }
            return ROBIndex;
        } 
        #endregion

    }
}
