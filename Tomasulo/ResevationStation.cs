using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Tomasulo
{
    class ResevationStation
    {
        #region Members
        private int timer;
        private string instructionName;
        private bool busy;
        private string operation;
        private string vj;
        private string vk;
        private string qj;
        private string qk;
        private string destination;
        static DataTable resevationStationsDT; 
        #endregion

        #region Ctor
        public ResevationStation()
        {
            LoadDefaults();

        }

        
        #endregion

        #region Properties
        

        public int TimerProp
        {
            get
            {
                return timer;
            }
            set
            {
                timer = value;
            }
        }

        public string InstructionNameProp
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

        public string operationProp
        {
            get
            {
                return operation;
            }
            set
            {
                operation = value;
            }
        }

        public string VjProp
        {
            get
            {
                return vj;
            }
            set
            {
                vj = value;
            }
        }

        public string VkProp
        {
            get
            {
                return vk;
            }
            set
            {
                vk = value;
            }
        }

        public string QjProp
        {
            get
            {
                return qj;
            }
            set
            {
                qj = value;
            }
        }

        public string QkProp
        {
            get
            {
                return qk;
            }
            set
            {
                qk = value;
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

        public static DataTable ResevationStationsDT()
        {
            return resevationStationsDT;
        } 
        #endregion

        #region Methods
        public static bool UpdateResevationStations(int InstIndex, int timer, bool busy, string operation, string vj, string vk, string qj, string qk, string destination,int instructionNum)
        {
            resevationStationsDT.Rows[InstIndex]["Timer"] = timer;
            resevationStationsDT.Rows[InstIndex]["Busy"] = busy;
            resevationStationsDT.Rows[InstIndex]["Operation"] = operation;
            resevationStationsDT.Rows[InstIndex]["Vj"] = vj;
            resevationStationsDT.Rows[InstIndex]["Vk"] = vk;
            resevationStationsDT.Rows[InstIndex]["Qj"] = qj;
            resevationStationsDT.Rows[InstIndex]["Qk"] = qk;
            resevationStationsDT.Rows[InstIndex]["Destination"] = destination;
            resevationStationsDT.Rows[InstIndex]["InstructionNum"] = instructionNum;
            return true;
        }

        public static bool DeleteResevationStationsRow(int InstIndex)
        {
            resevationStationsDT.Rows[InstIndex]["Timer"] = 0;
            resevationStationsDT.Rows[InstIndex]["Busy"] = false;
            resevationStationsDT.Rows[InstIndex]["Operation"] = string.Empty;
            resevationStationsDT.Rows[InstIndex]["Vj"] = string.Empty;
            resevationStationsDT.Rows[InstIndex]["Vk"] = string.Empty;
            resevationStationsDT.Rows[InstIndex]["Qj"] = string.Empty;
            resevationStationsDT.Rows[InstIndex]["Qk"] = string.Empty;
            resevationStationsDT.Rows[InstIndex]["Destination"] = string.Empty;
            resevationStationsDT.Rows[InstIndex]["InstructionNum"] = DBNull.Value;
            return true;
        }

        public static bool Delete()
        {
            for (int i = 0; i < 7; i++)
            {
                resevationStationsDT.Rows[i]["Timer"] = 0;
                resevationStationsDT.Rows[i]["Busy"] = false;
                resevationStationsDT.Rows[i]["Operation"] = string.Empty;
                resevationStationsDT.Rows[i]["Vj"] = string.Empty;
                resevationStationsDT.Rows[i]["Vk"] = string.Empty;
                resevationStationsDT.Rows[i]["Qj"] = string.Empty;
                resevationStationsDT.Rows[i]["Qk"] = string.Empty;
                resevationStationsDT.Rows[i]["Destination"] = string.Empty;
                resevationStationsDT.Rows[i]["InstructionNum"] = DBNull.Value;
            }
            return true;
        }

        public static bool IsResevationStationBusy()
        {
            int numOfBusyRows = 0;

            for (int i = 0; i < resevationStationsDT.Rows.Count; i++)
            {
                if (resevationStationsDT.Rows[i]["Busy"].ToString() == "True")
                {
                    numOfBusyRows++;
                }
            }

            if (numOfBusyRows < 5)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int GetFirstFreeIndex(string typeOfIns)
        {
            for (int i = 0; i < 7; i++)
            {
                if (typeOfIns == "FP Add")
                {
                    if (resevationStationsDT.Rows[i]["Busy"].ToString() == "False" && ((string.Compare("ADD1", resevationStationsDT.Rows[i]["Name"].ToString()) == 0) || (string.Compare("ADD2", resevationStationsDT.Rows[i]["Name"].ToString()) == 0) || (string.Compare("ADD3", resevationStationsDT.Rows[i]["Name"].ToString()) == 0)))
                    {
                        return i;
                    }
                }
                else if (typeOfIns == "FP Multiply")
                {
                    if (resevationStationsDT.Rows[i]["Busy"].ToString() == "False" && ((string.Compare("MULT1", resevationStationsDT.Rows[i]["Name"].ToString()) == 0) || string.Compare("MULT2", resevationStationsDT.Rows[i]["Name"].ToString()) == 0))
                    {

                        return i;
                    }
                }
                else if (typeOfIns == "INTADD")
                {
                    if (resevationStationsDT.Rows[i]["Busy"].ToString() == "False" && ((string.Compare("INTADD1", resevationStationsDT.Rows[i]["Name"].ToString()) == 0) || string.Compare("INTADD2", resevationStationsDT.Rows[i]["Name"].ToString()) == 0))
                    {
                        return i;
                    }
                }
            }
            return 0;
        }
        public static int GetIndexOfIns(int InstructionNumber)
        {
            for (int i = 0; i < 7; i++)
            {
                if (resevationStationsDT.Rows[i]["InstructionNum"] != DBNull.Value
                    &&( int)resevationStationsDT.Rows[i]["InstructionNum"] == InstructionNumber)
                {
                    return i;
                    
                }
            }
            return 0;
        }
        public static int GetIndexOfIns(int InstructionNumber,string Instruction)
        {
            for (int i = 0; i < 7; i++)
            {
                if (resevationStationsDT.Rows[i]["InstructionNum"] != DBNull.Value
                    && (int)resevationStationsDT.Rows[i]["InstructionNum"] == InstructionNumber)
                {
                    return i;
                    
                }
                else if (resevationStationsDT.Rows[i]["Operation"].ToString() == Instruction)
                {

                }
            }
            return 0;
        }

        public static int GetTimerOfIns(int InstructionNumber)
        {
            for (int i = 0; i < 7; i++)
            {
                if (resevationStationsDT.Rows[i]["InstructionNum"] != DBNull.Value
                    && (int)resevationStationsDT.Rows[i]["InstructionNum"] == InstructionNumber)
                {
                    return int.Parse(resevationStationsDT.Rows[i]["Timer"].ToString());
                }
            }
            return 0;
        }
        public static int GetTimerOfIns(int InstructionNumber, string Instruction)
        {
            for (int i = 0; i < 7; i++)
            {
                if (resevationStationsDT.Rows[i]["InstructionNum"] != DBNull.Value
                    && (int)resevationStationsDT.Rows[i]["InstructionNum"] == InstructionNumber)
                {
                    return int.Parse(resevationStationsDT.Rows[i]["Timer"].ToString());
                }
                else if (resevationStationsDT.Rows[i]["Operation"].ToString() == Instruction)
                {
                    return int.Parse(resevationStationsDT.Rows[i]["Timer"].ToString());
                }
            }
            return 0;
        }
        private void LoadDefaults()
        {
            timer = 0;
            instructionName = string.Empty;
            busy = false;
            operation = string.Empty;
            vj = string.Empty;
            vk = string.Empty;
            qj = string.Empty;
            qk = string.Empty;
            destination = string.Empty;
            resevationStationsDT = new DataTable("ResevationStations");
            resevationStationsDT.Columns.Add("Timer", typeof(int));
            resevationStationsDT.Columns.Add("Name", typeof(string));
            resevationStationsDT.Columns.Add("Busy", typeof(bool));
            resevationStationsDT.Columns.Add("Operation", typeof(string));
            resevationStationsDT.Columns.Add("Vj", typeof(string));
            resevationStationsDT.Columns.Add("Vk", typeof(string));
            resevationStationsDT.Columns.Add("Qj", typeof(string));
            resevationStationsDT.Columns.Add("Qk", typeof(string));
            resevationStationsDT.Columns.Add("Destination", typeof(string));
            resevationStationsDT.Columns.Add("InstructionNum", typeof(int));
            resevationStationsDT.Rows.Add(0, "ADD1", false);
            resevationStationsDT.Rows.Add(0, "ADD2", false);
            resevationStationsDT.Rows.Add(0, "ADD3", false);
            resevationStationsDT.Rows.Add(0, "MULT1", false);
            resevationStationsDT.Rows.Add(0, "MULT2", false);
            resevationStationsDT.Rows.Add(0, "INTADD1", false);
            resevationStationsDT.Rows.Add(0, "INTADD2", false);
        } 
        public static bool UpdateTimer(int index, int timer)
        {
            resevationStationsDT.Rows[index]["Timer"] = timer;
            return true;
        }
        #endregion

    }
}
