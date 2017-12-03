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
        static DataTable reservationStationsDT; 
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
            return reservationStationsDT;
        }
        #endregion


        #region Methods

        /*---------------------------------------------------------------------------
         Function name   : UpdateRS       
         Description     : This method handles the update of the Reservation Stations 
         Return type     : static void 
        ---------------------------------------------------------------------------*/

        public static bool UpdateRSUpdate(int instIndex, int timer, bool busy, string operation, string vj, string vk, string qj, string qk, string destination,int instructionNum)
        {
            reservationStationsDT.Rows[instIndex]["Timer"] = timer;
            reservationStationsDT.Rows[instIndex]["Busy"] = busy;
            reservationStationsDT.Rows[instIndex]["Operation"] = operation;
            reservationStationsDT.Rows[instIndex]["Vj"] = vj;
            reservationStationsDT.Rows[instIndex]["Vk"] = vk;
            reservationStationsDT.Rows[instIndex]["Qj"] = qj;
            reservationStationsDT.Rows[instIndex]["Qk"] = qk;
            reservationStationsDT.Rows[instIndex]["Destination"] = destination;
            reservationStationsDT.Rows[instIndex]["InstructionNum"] = instructionNum;
            return true;
        }

        /*---------------------------------------------------------------------------
         Function name   : ClearRS       
         Description     : This method handles clears the Reservation Stations 
         Return type     : static void 
        ---------------------------------------------------------------------------*/


        public static bool DeleteResevationStationsRow(int instIndex)
        {
            reservationStationsDT.Rows[instIndex]["Timer"] = 0;
            reservationStationsDT.Rows[instIndex]["Busy"] = false;
            reservationStationsDT.Rows[instIndex]["Operation"] = string.Empty;
            reservationStationsDT.Rows[instIndex]["Vj"] = string.Empty;
            reservationStationsDT.Rows[instIndex]["Vk"] = string.Empty;
            reservationStationsDT.Rows[instIndex]["Qj"] = string.Empty;
            reservationStationsDT.Rows[instIndex]["Qk"] = string.Empty;
            reservationStationsDT.Rows[instIndex]["Destination"] = string.Empty;
            reservationStationsDT.Rows[instIndex]["InstructionNum"] = DBNull.Value;
            return true;
        }

        /*-----------------------------------------------------------------------------------------------------------
          Function name   : ResetRS       
          Description     : This method handles resets the Reservation Stations after the completion of the execution 
          Return type     : static void 
        ------------------------------------------------------------------------------------------------------------*/


        public static bool ResetRS()
        {
            for (int i = 0; i < 7; i++)
            {
                reservationStationsDT.Rows[i]["Timer"] = 0;
                reservationStationsDT.Rows[i]["Busy"] = false;
                reservationStationsDT.Rows[i]["Operation"] = string.Empty;
                reservationStationsDT.Rows[i]["Vj"] = string.Empty;
                reservationStationsDT.Rows[i]["Vk"] = string.Empty;
                reservationStationsDT.Rows[i]["Qj"] = string.Empty;
                reservationStationsDT.Rows[i]["Qk"] = string.Empty;
                reservationStationsDT.Rows[i]["Destination"] = string.Empty;
                reservationStationsDT.Rows[i]["InstructionNum"] = DBNull.Value;
            }
            return true;
        }

        /*--------------------------------------------------------------------------------------------
          Function name   : CheckRSStatus       
          Description     : This method checks the status of the RS during the exeution of the program
          Return type     : static void 
        --------------------------------------------------------------------------------------------*/


        public static bool CheckRSStatus()
        {
            int numOfBusyRows = 0;

            for (int i = 0; i < reservationStationsDT.Rows.Count; i++)
            {
                if (reservationStationsDT.Rows[i]["Busy"].ToString() == "True")
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
                    if (reservationStationsDT.Rows[i]["Busy"].ToString() == "False" && ((string.Compare("ADD1", reservationStationsDT.Rows[i]["Name"].ToString()) == 0) || (string.Compare("ADD2", reservationStationsDT.Rows[i]["Name"].ToString()) == 0) || (string.Compare("ADD3", reservationStationsDT.Rows[i]["Name"].ToString()) == 0)))
                    {
                        return i;
                    }
                }
                else if (typeOfIns == "FP Multiply")
                {
                    if (reservationStationsDT.Rows[i]["Busy"].ToString() == "False" && ((string.Compare("MULT1", reservationStationsDT.Rows[i]["Name"].ToString()) == 0) || string.Compare("MULT2", reservationStationsDT.Rows[i]["Name"].ToString()) == 0))
                    {

                        return i;
                    }
                }
                else if (typeOfIns == "INTADD")
                {
                    if (reservationStationsDT.Rows[i]["Busy"].ToString() == "False" && ((string.Compare("INTADD1", reservationStationsDT.Rows[i]["Name"].ToString()) == 0) || string.Compare("INTADD2", reservationStationsDT.Rows[i]["Name"].ToString()) == 0))
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
                if (reservationStationsDT.Rows[i]["InstructionNum"] != DBNull.Value
                    &&( int)reservationStationsDT.Rows[i]["InstructionNum"] == InstructionNumber)
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
                if (reservationStationsDT.Rows[i]["InstructionNum"] != DBNull.Value
                    && (int)reservationStationsDT.Rows[i]["InstructionNum"] == InstructionNumber)
                {
                    return i;
                    
                }
                else if (reservationStationsDT.Rows[i]["Operation"].ToString() == Instruction)
                {

                }
            }
            return 0;
        }

        public static int GetTimerOfIns(int InstructionNumber)
        {
            for (int i = 0; i < 7; i++)
            {
                if (reservationStationsDT.Rows[i]["InstructionNum"] != DBNull.Value
                    && (int)reservationStationsDT.Rows[i]["InstructionNum"] == InstructionNumber)
                {
                    return int.Parse(reservationStationsDT.Rows[i]["Timer"].ToString());
                }
            }
            return 0;
        }
        public static int GetTimerOfIns(int InstructionNumber, string Instruction)
        {
            for (int i = 0; i < 7; i++)
            {
                if (reservationStationsDT.Rows[i]["InstructionNum"] != DBNull.Value
                    && (int)reservationStationsDT.Rows[i]["InstructionNum"] == InstructionNumber)
                {
                    return int.Parse(reservationStationsDT.Rows[i]["Timer"].ToString());
                }
                else if (reservationStationsDT.Rows[i]["Operation"].ToString() == Instruction)
                {
                    return int.Parse(reservationStationsDT.Rows[i]["Timer"].ToString());
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
            reservationStationsDT = new DataTable("ResevationStations");
            reservationStationsDT.Columns.Add("Timer", typeof(int));
            reservationStationsDT.Columns.Add("Name", typeof(string));
            reservationStationsDT.Columns.Add("Busy", typeof(bool));
            reservationStationsDT.Columns.Add("Operation", typeof(string));
            reservationStationsDT.Columns.Add("Vj", typeof(string));
            reservationStationsDT.Columns.Add("Vk", typeof(string));
            reservationStationsDT.Columns.Add("Qj", typeof(string));
            reservationStationsDT.Columns.Add("Qk", typeof(string));
            reservationStationsDT.Columns.Add("Destination", typeof(string));
            reservationStationsDT.Columns.Add("InstructionNum", typeof(int));
            reservationStationsDT.Rows.Add(0, "ADD1", false);
            reservationStationsDT.Rows.Add(0, "ADD2", false);
            reservationStationsDT.Rows.Add(0, "ADD3", false);
            reservationStationsDT.Rows.Add(0, "MULT1", false);
            reservationStationsDT.Rows.Add(0, "MULT2", false);
            reservationStationsDT.Rows.Add(0, "INTADD1", false);
            reservationStationsDT.Rows.Add(0, "INTADD2", false);
        } 
        public static bool UpdateTimer(int index, int timer)
        {
            reservationStationsDT.Rows[index]["Timer"] = timer;
            return true;
        }
        #endregion

    }
}
