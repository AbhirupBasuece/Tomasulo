using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Tomasulo
{
    class RegisterResultStatus
    {
        #region Members
        private string f0;
        private string f1;
        private string f2;
        private string f3;
        private string f4;
        private string f5;
        private string f6;
        private string f7;
        private string f8;
        private string f9;
        private string f10;
        private string f11;
        private string f12;
        private string f13;
        private string f14;
        private string f15;
        private string f16;
        private string f17;
        private string f18;
        private string f19;
        private string f20;
        private string f21;
        private string f22;
        private string f23;
        private string f24;
        private string f25;
        private string f26;
        private string f27;
        private string f28;
        private string f29;
        private string f30;
        private string f31;
        private string r0;
        private string r1;
        private string r2;
        private string r3;
        private string r4;
        private string r5;
        private string r6;
        private string r7;
        private string r8;
        private string r9;
        private string r10;
        private string r11;
        private string r12;
        private string r13;
        private string r14;
        private string r15;
        private string r16;
        private string r17;
        private string r18;
        private string r19;
        private string r20;
        private string r21;
        private string r22;
        private string r23;
        private string r24;
        private string r25;
        private string r26;
        private string r27;
        private string r28;
        private string r29;
        private string r30;
        private string r31;
        static DataTable registerResultStatusDT; 
        #endregion

        #region Ctor
        public RegisterResultStatus()
        {
            ConstructDataTable();
        } 
        #endregion

        #region Properties
        public string FoProp
        {
            get
            {
                return f0;
            }
            set
            {
                f0 = value;
            }
        }

        public string F1Prop
        {
            get
            {
                return f1;
            }
            set
            {
                f1 = value;
            }
        }

        public string F2Prop
        {
            get
            {
                return f2;
            }
            set
            {
                f2 = value;
            }
        }

        public string F3Prop
        {
            get
            {
                return f3;
            }
            set
            {
                f3 = value;
            }
        }

        public string F4Prop
        {
            get
            {
                return f4;
            }
            set
            {
                f4 = value;
            }
        }

        public string F5Prop
        {
            get
            {
                return f5;
            }
            set
            {
                f5 = value;
            }
        }

        public string F6Prop
        {
            get
            {
                return f6;
            }
            set
            {
                f6 = value;
            }
        }

        public string F7Prop
        {
            get
            {
                return f7;
            }
            set
            {
                f7 = value;
            }
        }

        public string F8Prop
        {
            get
            {
                return f8;
            }
            set
            {
                f8 = value;
            }
        }

        public string F9Prop
        {
            get
            {
                return f9;
            }
            set
            {
                f9 = value;
            }
        }

        public string F10Prop
        {
            get
            {
                return f10;
            }
            set
            {
                f10 = value;
            }
        }

        public string F11Prop
        {
            get
            {
                return f11;
            }
            set
            {
                f11 = value;
            }
        }

        public string F12Prop
        {
            get
            {
                return f12;
            }
            set
            {
                f12 = value;
            }
        }

        public string F13Prop
        {
            get
            {
                return f13;
            }
            set
            {
                f13 = value;
            }
        }

        public string F14Prop
        {
            get
            {
                return f14;
            }
            set
            {
                f14 = value;
            }
        }

        public string F15Prop
        {
            get
            {
                return f15;
            }
            set
            {
                f15 = value;
            }
        }

        public string F16Prop
        {
            get
            {
                return f16;
            }
            set
            {
                f16 = value;
            }
        }

        public string F17Prop
        {
            get
            {
                return f17;
            }
            set
            {
                f17 = value;
            }
        }

        public string F18Prop
        {
            get
            {
                return f18;
            }
            set
            {
                f18 = value;
            }
        }

        public string F19Prop
        {
            get
            {
                return f19;
            }
            set
            {
                f19 = value;
            }
        }

        public string F20Prop
        {
            get
            {
                return f20;
            }
            set
            {
                f20 = value;
            }
        }

        public string F21Prop
        {
            get
            {
                return f21;
            }
            set
            {
                f21 = value;
            }
        }

        public string F22Prop
        {
            get
            {
                return f22;
            }
            set
            {
                f22 = value;
            }
        }

        public string F23Prop
        {
            get
            {
                return f23;
            }
            set
            {
                f23 = value;
            }
        }

        public string F24Prop
        {
            get
            {
                return f24;
            }
            set
            {
                f24 = value;
            }
        }

        public string F25Prop
        {
            get
            {
                return f25;
            }
            set
            {
                f25 = value;
            }
        }

        public string F26Prop
        {
            get
            {
                return f26;
            }
            set
            {
                f26 = value;
            }
        }

        public string F27Prop
        {
            get
            {
                return f27;
            }
            set
            {
                f27 = value;
            }
        }

        public string F28Prop
        {
            get
            {
                return f28;
            }
            set
            {
                f28 = value;
            }
        }

        public string F29Prop
        {
            get
            {
                return f29;
            }
            set
            {
                f29 = value;
            }
        }

        public string F30Prop
        {
            get
            {
                return f30;
            }
            set
            {
                f30 = value;
            }
        }

        public string F31Prop
        {
            get
            {
                return f31;
            }
            set
            {
                f31 = value;
            }
        }

        public string RoProp
        {
            get
            {
                return r0;
            }
            set
            {
                r0 = value;
            }
        }

        public string R1Prop
        {
            get
            {
                return r1;
            }
            set
            {
                r1 = value;
            }
        }

        public string R2Prop
        {
            get
            {
                return r2;
            }
            set
            {
                r2 = value;
            }
        }

        public string R3Prop
        {
            get
            {
                return r3;
            }
            set
            {
                r3 = value;
            }
        }

        public string R4Prop
        {
            get
            {
                return r4;
            }
            set
            {
                r4 = value;
            }
        }

        public string R5Prop
        {
            get
            {
                return r5;
            }
            set
            {
                r5 = value;
            }
        }

        public string R6Prop
        {
            get
            {
                return r6;
            }
            set
            {
                r6 = value;
            }
        }

        public string R7Prop
        {
            get
            {
                return r7;
            }
            set
            {
                r7 = value;
            }
        }

        public string R8Prop
        {
            get
            {
                return r8;
            }
            set
            {
                r8 = value;
            }
        }

        public string R9Prop
        {
            get
            {
                return r9;
            }
            set
            {
                r9 = value;
            }
        }

        public string R10Prop
        {
            get
            {
                return r10;
            }
            set
            {
                r10 = value;
            }
        }

        public string R11Prop
        {
            get
            {
                return r11;
            }
            set
            {
                r11 = value;
            }
        }

        public string R12Prop
        {
            get
            {
                return r12;
            }
            set
            {
                r12 = value;
            }
        }

        public string R13Prop
        {
            get
            {
                return r13;
            }
            set
            {
                r13 = value;
            }
        }

        public string R14Prop
        {
            get
            {
                return r14;
            }
            set
            {
                r14 = value;
            }
        }

        public string R15Prop
        {
            get
            {
                return r15;
            }
            set
            {
                r15 = value;
            }
        }

        public string R16Prop
        {
            get
            {
                return r16;
            }
            set
            {
                r16 = value;
            }
        }

        public string R17Prop
        {
            get
            {
                return r17;
            }
            set
            {
                r17 = value;
            }
        }

        public string R18Prop
        {
            get
            {
                return r18;
            }
            set
            {
                r18 = value;
            }
        }

        public string R19Prop
        {
            get
            {
                return r19;
            }
            set
            {
                r19 = value;
            }
        }

        public string R20Prop
        {
            get
            {
                return r20;
            }
            set
            {
                r20 = value;
            }
        }

        public string R21Prop
        {
            get
            {
                return r21;
            }
            set
            {
                r21 = value;
            }
        }

        public string R22Prop
        {
            get
            {
                return r22;
            }
            set
            {
                r22 = value;
            }
        }

        public string R23Prop
        {
            get
            {
                return r23;
            }
            set
            {
                r23 = value;
            }
        }

        public string R24Prop
        {
            get
            {
                return r24;
            }
            set
            {
                r24 = value;
            }
        }

        public string R25Prop
        {
            get
            {
                return r25;
            }
            set
            {
                r25 = value;
            }
        }

        public string R26Prop
        {
            get
            {
                return r26;
            }
            set
            {
                r26 = value;
            }
        }

        public string R27Prop
        {
            get
            {
                return r27;
            }
            set
            {
                r27 = value;
            }
        }

        public string R28Prop
        {
            get
            {
                return r28;
            }
            set
            {
                r28 = value;
            }
        }

        public string R29Prop
        {
            get
            {
                return r29;
            }
            set
            {
                r29 = value;
            }
        }

        public string R30Prop
        {
            get
            {
                return r30;
            }
            set
            {
                r30 = value;
            }
        }

        public string R31Prop
        {
            get
            {
                return r31;
            }
            set
            {
                r31 = value;
            }
        }
        public static DataTable RegisterResultStatusDT()
        {
            return registerResultStatusDT;
        }
        #endregion

        #region Methods

        public static bool Insert(int idx, string busy)
        {
            registerResultStatusDT.Rows[1][idx] = busy;
            return true;
        }

        public static bool Update(int idx, string RobNum, string busy)
        {
            registerResultStatusDT.Rows[0][idx] = RobNum;
            registerResultStatusDT.Rows[1][idx] = busy;
            return true;
        }

        public static bool UpdateRegValue(string register, string value)
        {
            registerResultStatusDT.Rows[2][register] = value;
            return true;
        }
        public static bool Delete(int idx)
        {
            registerResultStatusDT.Rows[0][idx] = string.Empty;
            registerResultStatusDT.Rows[1][idx] = "No";
            return true;
        }

        public static bool Refresh()
        {
            for (int i = 0; i < registerResultStatusDT.Rows.Count; i++)
            {
                for (int j = 1; j < registerResultStatusDT.Rows[i].ItemArray.Count(); j++)
			    {
                    registerResultStatusDT.Rows[i][j] = string.Empty;
			    }
                
            }

            return true;
        }

        private static void ConstructDataTable()
        {
            registerResultStatusDT = new DataTable("RGisterResultStatusDT");
            registerResultStatusDT.Columns.Add("Field", typeof(string));
            for (int i = 0; i < 32; i++)
            {
                registerResultStatusDT.Columns.Add("F"+i, typeof(string)); 
            }

            for (int i = 0; i < 32; i++)
            {
                registerResultStatusDT.Columns.Add("R" + i, typeof(string));
            }
            registerResultStatusDT.Rows.Add("ROBID");
            registerResultStatusDT.Rows.Add("Busy");
            registerResultStatusDT.Rows.Add("Value");
            
        }

        public static bool IsBusy()
        {
            int numOfBusyCol = 0;

            for (int j = 0; j < registerResultStatusDT.Columns.Count; j++)
            {
                if (registerResultStatusDT.Rows[1][j].ToString() == "Yes")
                {
                    numOfBusyCol++;
                }
            }

            if (numOfBusyCol < 31)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string GetROBID(string destination)
        {
            for (int j = 1; j < registerResultStatusDT.Columns.Count; j++)
            {
                if (registerResultStatusDT.Columns[j].ToString() == destination)
                {
                    return registerResultStatusDT.Rows[0][j].ToString();
                }
            }
            return "";
        } 
        #endregion
    }
}
