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
    class LoadBuffer
    {
        #region members
        private string name;
        static DataTable loadBufferDT;
        private bool busy;
        private string address;

        #endregion

        #region ctor
        public LoadBuffer()
        {
            busy = false;
            address = string.Empty;
            name = string.Empty;
            loadBufferDT = new DataTable("LoadBufferDT");
            loadBufferDT.Columns.Add("Busy", typeof(bool));
            loadBufferDT.Columns.Add("Name", typeof(string));
            loadBufferDT.Columns.Add("Address", typeof(string));
        } 
        #endregion

        #region props

        public static DataTable LoadBufferDT()
        {
            return loadBufferDT;
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

        public string AddressProp
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }

        public string NameProp
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        } 
        #endregion

        #region Methods
        public static bool Insert(bool busy, string name, string address)
        {
            loadBufferDT.Rows.Add(busy, name, address);
            return true;
        }

        public static bool Update(int index, bool busy, string address)
        {
            loadBufferDT.Rows[index]["Busy"] = busy;
            loadBufferDT.Rows[index]["Address"] = address;
            return true;
        }

        public static bool Delete(int index)
        {
            loadBufferDT.Rows[index]["Busy"] = false;
            loadBufferDT.Rows[index]["Address"] = string.Empty;
            return true;
        }

        public static bool Refresh()
        {
            for (int i = 0; i < loadBufferDT.Rows.Count; i++)
            {
                loadBufferDT.Rows[i]["Busy"] = false;
                loadBufferDT.Rows[i]["Address"] = string.Empty;
            }
            return true;
        }


        //============================================================
        // Function name   : GetBusyFalseItem
        // Description     : returning the first loadbuffer index which is not busy
        // Return type     : static int 
        //============================================================
        public static int GetBusyFalseItem()
        {
            for (int i = 0; i < loadBufferDT.Rows.Count; i++)
            {
                if (loadBufferDT.Rows[i]["Busy"].ToString() == "False")
                {
                    return i;
                }
            }
            return 0;
        }

        public static bool IsBusy()
        {
            int numOfBusyRows = 0;

            for (int i = 0; i < loadBufferDT.Rows.Count; i++)
            {
                if (loadBufferDT.Rows[i]["Busy"].ToString() == "False")
                {
                    numOfBusyRows++;
                }
            }

            if (numOfBusyRows < loadBufferDT.Rows.Count)
            {
                return false; 
            }
            else
            {
                return true; 
            }
        } 
        #endregion  
    }
}
