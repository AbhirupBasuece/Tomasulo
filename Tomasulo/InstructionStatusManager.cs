using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Tomasulo
{
    class InstructionStatusManager
    {
        #region Members
        private int iteration, issuecycle, execution,memRead, writeToCDB, commit;
        private string instruction, comment;
        static DataTable executionManagerDT;
        private static bool[] IsComment; 
        #endregion

        #region Ctor
        public InstructionStatusManager()
        {
            LoadDefaults();

        } 
        #endregion

        #region Properties
        public int IterationProp
        {
            get
            {
                return iteration;
            }
            set
            {
                iteration = value;
            }
        }

        public string InstructionProp
        {
            get
            {
                return instruction;
            }
            set
            {
                instruction = value;
            }
        }

        public int IssuecycleProp
        {
            get
            {
                return issuecycle;
            }
            set
            {
                issuecycle = value;
            }
        }

        public int ExecutionProp
        {
            get
            {
                return execution;
            }
            set
            {
                execution = value;
            }
        }

        public int MemoryReadProp
        {
            get
            {
                return memRead;
            }
            set
            {
                memRead = value;
            }
        }

        public int WriteCDBProp
        {
            get
            {
                return writeToCDB;
            }
            set
            {
                writeToCDB = value;
            }
        }

        public int CommitProp
        {
            get
            {
                return commit;
            }
            set
            {
                commit = value;
            }
        }

        public static DataTable ExecutionManagerDT()
        {
            return executionManagerDT;
        }

        public string CommentProp
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
            }
        } 
        #endregion

        #region Methods
        public static void ResetIsComment()
        {
            for (int i = 0; i < 10; i++)
            {
                IsComment[i] = false;
            }
        }


        /*------------------------------------------------------------------------------------
         Function name   : Insert
         Description     : insert new entry into execution manager table -- need to update for reservation execution custom parameters **
         Return type     : static bool 
         Argument        : int iteration, issuecycle, execution, memRead, writeToCDB, commit
         Argument        : string instruction, comment
        -------------------------------------------------------------------------------------*/
        public static bool Insert(int iteration, string instruction, int issuecycle, int execution, int memRead, int writeToCDB, int commit, string comment)
        {
            executionManagerDT.Rows.Add(iteration, instruction, issuecycle, execution, memRead, writeToCDB, commit, comment);
            return true;
        }


        /*--------------------------------------------------------------------------------------------
         Function name   : Update
         Description     : update entry in execution manager table -- -- need to update for reservation execution custom parameters **
         Return type     : static bool 
         Arguments       : int index, iteration, issueCycle, execution, memRead, writeToCDB, commit
         Argument        : string instruction, comment
        ----------------------------------------------------------------------------------------------*/
        public static bool Update(int index, int iteration, string instruction, int issuecycle, int execution, int memRead, int writeToCDB, int commit, string comment)
        {
            executionManagerDT.Rows[index][0] = iteration;
            executionManagerDT.Rows[index][1] = instruction;
            executionManagerDT.Rows[index][2] = issuecycle;
            executionManagerDT.Rows[index][3] = execution;
            executionManagerDT.Rows[index][4] = memRead;
            executionManagerDT.Rows[index][5] = writeToCDB;
            executionManagerDT.Rows[index][6] = commit;
            executionManagerDT.Rows[index][7] = comment;
            return true;
        }

       /*--------------------------------------------------------------------------------------------
       Function name   : Delete
       Description     : delete any entry in execution manager table 
       Return type     : static bool 
       Arguments       : int index
       ---------------------------------------------------------------------------------------------*/

        public static bool Delete(int index)
        {
            executionManagerDT.Rows[index][0] = 0;
            executionManagerDT.Rows[index][1] = string.Empty;
            executionManagerDT.Rows[index][2] = 0;
            executionManagerDT.Rows[index][3] = 0;
            executionManagerDT.Rows[index][4] = 0;
            executionManagerDT.Rows[index][5] = 0;
            executionManagerDT.Rows[index][6] = 0;
            executionManagerDT.Rows[index][7] = string.Empty;
            return true;
        }
        /*--------------------------------------------------------------------------------------------
        Function name   : Refresh
        Description     : The table is cleared of all data
        Return type     : static bool 
        ---------------------------------------------------------------------------------------------*/
        public static bool Refresh()
        {
            executionManagerDT.Clear();
            return true;
        }

        public static bool UpdateComment(string instructionName, string commetToUpdate)
        {
            string[] comment = new string[10];
            comment = commetToUpdate.Split(';');
            string fullComment = string.Empty;

            for (int i = 0; i < executionManagerDT.Rows.Count; i++)
            {
                if (executionManagerDT.Rows[i]["Instruction"].ToString().Equals(instructionName))
                {
                    for (int j = 0; j < comment.Length; j++)
                    {
                        if (IsComment[i] == false)
                        {
                            if ((comment.Length - j) != 1)
                            {
                                executionManagerDT.Rows[i]["Comment"] += comment[j] + " ; ";
                            }
                            else
                            {
                                executionManagerDT.Rows[i]["Comment"] += comment[j];
                            }
                        }
                    }

                    IsComment[i] = true;
                }
            }

            return true;
        }

        public static int GetCDBCycle(int instructionNum)
        {
            return int.Parse(executionManagerDT.Rows[instructionNum]["Write CDB"].ToString());
        }

        public static int CommitCycle(int instructionNum)
        {
            return int.Parse(executionManagerDT.Rows[instructionNum]["Commit"].ToString());
        }

        public static int ExecutionCycle(int instructionNum)
        {
            return int.Parse(executionManagerDT.Rows[instructionNum][3].ToString());
        }

        private void LoadDefaults()
        {
            iteration = 0;
            instruction = string.Empty;
            issuecycle = 0;
            execution = 0;
            memRead = 0;
            writeToCDB = 0;
            commit = 0;
            comment = string.Empty;
            executionManagerDT = new DataTable("InstructionStatusDT");
            executionManagerDT.Columns.Add("Iteration", typeof(int));
            executionManagerDT.Columns.Add("Instruction", typeof(string));
            executionManagerDT.Columns.Add("Issue Cycle", typeof(int));
            executionManagerDT.Columns.Add("Execution", typeof(int));
            executionManagerDT.Columns.Add("Memory Read", typeof(int));
            executionManagerDT.Columns.Add("Write CDB", typeof(int));
            executionManagerDT.Columns.Add("Commit", typeof(int));
            executionManagerDT.Columns.Add("Comment", typeof(string));
            IsComment = new bool[10];

            for (int i = 0; i < 10; i++)
            {
                IsComment[i] = false;
            }
        }

        public static int MemoryReadCycle(int instructionNum)
        {
            return int.Parse(executionManagerDT.Rows[instructionNum]["Memory Read"].ToString());
        }

        public static string[] CommentForInst(int instructionNum)
        {
            string[] comment = new string[100];
            comment = executionManagerDT.Rows[instructionNum]["Comment"].ToString().Split(';');
            string[] instructionName = new string[10];

            for (int i = 0; i < comment.Length; i++)
            {
                instructionName[i] = comment[i].Split(',')[0];
                instructionName[i] = instructionName[i].Remove(0, 9).Split(' ')[0];
            }

            return instructionName;
        }


        //=================================================================
        // Function name   : InstructionWithDepen                // check with dry-runs**
        // Description     : return dependecies for given instruction number
        // Return type     : static string[] 
        // Argument        : int instructionNum
        //=================================================================
        public static string[] InstructionWithDepen(int instructionNum)
        {
            string[] WaitFor = new string[10];
            WaitFor = executionManagerDT.Rows[instructionNum][7].ToString().Split(';');
            string[] comment = new string[10];

            for (int i = 0; i < WaitFor.Length; i++)
            {
                comment[i] = WaitFor[i].Remove(0, 9);
            }

            return comment;
        } 
        #endregion

    }
}
