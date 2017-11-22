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

namespace Tomasulo
{
    class ReorderBuffer
    {
        #region Members
        private string entry;
        private bool busy;
        private string instruction;
        private string state;
        private string destination;
        private string val;
        static DataTable reorderBufferDT; 
        #endregion

        #region Ctor
        public ReorderBuffer(int size)
        {
            LoadDefualts();
            InitROBTable(size);

        } 
        #endregion

        #region Properties

        public static DataTable ReorderBufferDT()
        {
            return reorderBufferDT;
        }
        public string EnrtyProp
        {
            get
            {
                return entry;
            }
            set
            {
                entry = value;
            }
        }

        public bool BusyProp
        {
            get
            {
                return busy;
            }
            set
            {
                busy = value;
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

        public string StateProp
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public string DestinationProp
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }

        public string ValueProp
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
            }
        } 
        #endregion

        #region Methods
        public static bool Update(int index, bool busy, string instruction, string state, string destination, string value,string calclatedValue)
        {
            reorderBufferDT.Rows[index]["Busy"] = busy;
            reorderBufferDT.Rows[index]["Instruction"] = instruction;
            reorderBufferDT.Rows[index]["State"] = state;
            reorderBufferDT.Rows[index]["Destination"] = destination;
            reorderBufferDT.Rows[index]["Value"] = value;
            if (!string.IsNullOrEmpty(calclatedValue))
            {
                reorderBufferDT.Rows[index]["CalculatedValue"] = calclatedValue;
            }
           
            return true;
        }

        public static bool Delete(int index)
        {
            reorderBufferDT.Rows[index]["Busy"] = false; ;
            reorderBufferDT.Rows[index]["Instruction"] = string.Empty;
            reorderBufferDT.Rows[index]["State"] = string.Empty;
            reorderBufferDT.Rows[index]["Destination"] = string.Empty;
            reorderBufferDT.Rows[index]["Value"] = string.Empty;
            reorderBufferDT.Rows[index]["CalculatedValue"] = string.Empty;
            return true;
        }

        public static bool Refresh()
        {
            for (int i = 0; i < reorderBufferDT.Rows.Count; i++)
            {
                reorderBufferDT.Rows[i]["Busy"] = false; ;
                reorderBufferDT.Rows[i]["Instruction"] = string.Empty;
                reorderBufferDT.Rows[i]["State"] = string.Empty;
                reorderBufferDT.Rows[i]["Destination"] = string.Empty;
                reorderBufferDT.Rows[i]["Value"] = string.Empty;
                reorderBufferDT.Rows[i]["CalculatedValue"] = string.Empty;
            }
            return true;
        }

        public static int GetBusyFalseItem()
        {
            for (int i = 0; i < reorderBufferDT.Rows.Count; i++)
            {
                if (reorderBufferDT.Rows[i]["Busy"].ToString() == "False" && reorderBufferDT.Rows[i]["State"].ToString() != "Commit")
                {
                    return i;
                }
            }
            return 0;
        }

        public static bool IsROBBusy()
        {
            int numOfBusyRows = 0;

            for (int i = 0; i < reorderBufferDT.Rows.Count; i++)
            {
                if (reorderBufferDT.Rows[i]["Busy"].ToString() == "True")
                {
                    numOfBusyRows++;
                }
            }

            if (numOfBusyRows < 32)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string GetRobNum(string registerDst)
        {
            for (int i = 0; i < reorderBufferDT.Rows.Count; i++)
            {
                if (reorderBufferDT.Rows[i]["Destination"].ToString() == registerDst)
                {
                    return reorderBufferDT.Rows[i]["ID"].ToString();
                }
            }
            return "None";
        }

        public static int GetIndex(string instructionName)
        {
            for (int i = 0; i < reorderBufferDT.Rows.Count; i++)
            {
                if (reorderBufferDT.Rows[i]["Instruction"].ToString() == instructionName)
                {
                    return i;
                }
            }
            return 0;
        }

        public static bool IsCommitFinished(int index)
        {
            int counter = 0;

            for (int i = 0; i < index; i++)
            {
                if (reorderBufferDT.Rows[i]["State"].ToString() == "Commit")
                {
                    counter++;
                }
            }

            if (counter == index)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void InitROBTable(int size)
        {
            reorderBufferDT = new DataTable("ROB");
            reorderBufferDT.Columns.Add("ID", typeof(string));
            reorderBufferDT.Columns.Add("Busy", typeof(bool));
            reorderBufferDT.Columns.Add("Instruction", typeof(string));
            reorderBufferDT.Columns.Add("State", typeof(string));  
            reorderBufferDT.Columns.Add("Destination", typeof(string));
            reorderBufferDT.Columns.Add("Value", typeof(string));
            reorderBufferDT.Columns.Add("CalculatedValue", typeof(string));
            for (int i = 0; i < size; i++)
            {

                reorderBufferDT.Rows.Add("ROB"+i, false);
            }
        }

        private static void InitROBTable()
        {
            reorderBufferDT = new DataTable("ROB");
            reorderBufferDT.Columns.Add("Entry", typeof(string));
            reorderBufferDT.Columns.Add("Busy", typeof(bool));
            reorderBufferDT.Columns.Add("Instruction", typeof(string));
            reorderBufferDT.Columns.Add("State", typeof(string)); 
            reorderBufferDT.Columns.Add("Destination", typeof(string));
            reorderBufferDT.Columns.Add("Value", typeof(string));
            reorderBufferDT.Rows.Add("ROB1", false);
            reorderBufferDT.Rows.Add("ROB2", false);
            reorderBufferDT.Rows.Add("ROB3", false);
            reorderBufferDT.Rows.Add("ROB4", false);
            reorderBufferDT.Rows.Add("ROB5", false);
            reorderBufferDT.Rows.Add("ROB6", false);
            reorderBufferDT.Rows.Add("ROB7", false);
            reorderBufferDT.Rows.Add("ROB8", false);
            reorderBufferDT.Rows.Add("ROB9", false);
            reorderBufferDT.Rows.Add("ROB10", false);
            reorderBufferDT.Rows.Add("ROB11", false);
            reorderBufferDT.Rows.Add("ROB12", false);
            reorderBufferDT.Rows.Add("ROB13", false);
            reorderBufferDT.Rows.Add("ROB14", false);
            reorderBufferDT.Rows.Add("ROB15", false);
        }

        private void LoadDefualts()
        {
            entry = string.Empty;
            busy = false;
            instruction = string.Empty;
            state = string.Empty;
            destination = string.Empty;
            val = string.Empty;
        }

        public static string GetState(int instructionNum)
        {
            return reorderBufferDT.Rows[instructionNum]["State"].ToString();
        }

        public static string GetInstruction(int instructionNum)
        {
            return reorderBufferDT.Rows[instructionNum]["Instruction"].ToString();
        } 
        #endregion

    }
}
