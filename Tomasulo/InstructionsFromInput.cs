using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Tomasulo
{
    class InstructionFromInput
    {
        #region Members
        private string instructionName;
        private string destReg;
        private string source1;
        private string source2;
        static DataTable instructionsFromInputDT; 
        #endregion

        #region Ctor
        public InstructionFromInput()
        {
            instructionName = string.Empty;
            destReg = string.Empty;
            source1 = string.Empty;
            source2 = string.Empty;
            instructionsFromInputDT = new DataTable("DataInstructionCollection");
            instructionsFromInputDT.Columns.Add("Instruction_Name", typeof(string));
            instructionsFromInputDT.Columns.Add("DestRegister", typeof(string));
            instructionsFromInputDT.Columns.Add("Source1", typeof(string));
            instructionsFromInputDT.Columns.Add("Source2", typeof(string));
        } 
        #endregion

        #region Properties
        public string InstNameProp
        {
            get
            {
                return instructionName;
            }
            set
            {
                instructionName = value;
            }
        }

        public string destRegProp
        {
            get
            {
                return destReg;
            }
            set
            {
                destReg = value;
            }
        }

        public string sourceJProp
        {
            get
            {
                return source1;
            }
            set
            {
                source1 = value;
            }
        }

        public static DataTable InstructionsFromInputDT()
        {
            return instructionsFromInputDT;
        }

        public string sourceKProp
        {
            get
            {
                return source2;
            }
            set
            {
                source2 = value;
            }
        } 
        #endregion

        #region Methods

        public static bool Insert(string insName, string destReg, string source_1, string source_2)       // Method to insert the values into the Datatable
        {
            instructionsFromInputDT.Rows.Add(insName, destReg, source_1, source_2);
            return true;
        }

        public static bool Update(int index, string insName, string destReg, string source_1, string source_2)  // method to update the DataTable
        {
            instructionsFromInputDT.Rows[index]["Instruction_Name"] = insName;
            instructionsFromInputDT.Rows[index]["DestRegister"] = destReg;
            instructionsFromInputDT.Rows[index]["Source1"] = source_1;
            instructionsFromInputDT.Rows[index]["Source2"] = source_2;
            return true;
        }

        public static bool Delete(int index)                        // method to delete instructions from the datatable if required after update
        {
            instructionsFromInputDT.Rows[index]["Instruction_Name"] = string.Empty;
            instructionsFromInputDT.Rows[index]["DestRegister"] = string.Empty;
            instructionsFromInputDT.Rows[index]["Source1"] = string.Empty;
            instructionsFromInputDT.Rows[index]["Source2"] = string.Empty;
            return true;
        }

        public static bool Refresh()            // DataTable being cleared of all values
        {
            instructionsFromInputDT.Rows.Clear();
            return true;
        } 
        #endregion
  
    }
}
