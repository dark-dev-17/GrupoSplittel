using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionPersonal.Models
{
    public class GPSManager
    {
        private DB_Connection dB_Connection;

        public Nomina nomina { get; set; }

        public GPSManager()
        {
            dB_Connection = new DB_Connection("Server=192.168.2.29;Port=3306;Database=gestionPersonal;Uid=fibremex;Pwd=FBSrvAD*0316.;Allow Zero Datetime=True;Convert Zero Datetime=True;Persist Security Info=True");
        }

        public void OpenConnection()
        {
            dB_Connection.OpenConnection();
        }
        public void CloseConnection()
        {
            dB_Connection.CloseConnection();
        }

        public int GetErrorCode()
        {
            return dB_Connection.Code;
        }
        public string GetErrorMessage()
        {
            return dB_Connection.Message;
        }

        public DB_Connection getConnection()
        {
            return dB_Connection;
        }
    }
}
