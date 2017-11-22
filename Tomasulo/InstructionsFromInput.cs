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
        private string sourceJ;
        private string sourceK;
        static DataTable instructionsFromInputDT; 
        #endregion

        #region Ctor
        public InstructionFromInput()
        {
            instructionName = string.Empty;
            destReg = string.Empty;
            sourceJ = string.Empty;
            sourceK = string.Empty;
            instructionsFromInputDT = new DataTable("DataInstructionCollection");
            instructionsFromInputDT.Columns.Add("Instruction Name", typeof(string));
            instructionsFromInputDT.Columns.Add("DestReg", typeof(string));
            instructionsFromInputDT.Columns.Add("SourceJ", typeof(string));
            instructionsFromInputDT.Columns.Add("SourceK", typeof(string));
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
                return sourceJ;
            }
            set
            {
                sourceJ = value;
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
                return sourceK;
            }
            set
            {
                sourceK = value;
            }
        } 
        #endregion

        #region Methods

        public static bool Insert(string insName, string destReg, string sourceJ, string sourceK)
        {
            instructionsFromInputDT.Rows.Add(insName, destReg, sourceJ, sourceK);
            return true;
        }

        public static bool Update(int index, string insName, string destReg, string sourceJ, string sourceK)
        {
            instructionsFromInputDT.Rows[index]["Instruction Name"] = insName;
            instructionsFromInputDT.Rows[index]["DestReg"] = destReg;
            instructionsFromInputDT.Rows[index]["SourceJ"] = sourceJ;
            instructionsFromInputDT.Rows[index]["SourceK"] = sourceK;
            return true;
        }

        public static bool Delete(int index)
        {
            instructionsFromInputDT.Rows[index]["Instruction Name"] = string.Empty;
            instructionsFromInputDT.Rows[index]["DestReg"] = string.Empty;
            instructionsFromInputDT.Rows[index]["SourceJ"] = string.Empty;
            instructionsFromInputDT.Rows[index]["SourceK"] = string.Empty;
            return true;
        }

        public static bool Refresh()
        {
            instructionsFromInputDT.Rows.Clear();
            return true;
        } 
        #endregion
  
    }
}
