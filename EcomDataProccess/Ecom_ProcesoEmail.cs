using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_ProcesoEmail
    {
        #region Propiedades
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<string> ListaTo { get; set; }
        public List<string> ListaCC { get; set; }
        public List<string> ListaBCC { get; set; }
        public bool IsActive { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProcesoEmail()
        {
            
        }
        public Ecom_ProcesoEmail()
        {

        }
        public Ecom_ProcesoEmail(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Update(int mode)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_ProcesoEmail");
                Ecom_DBConnection_.AddParameter(Id, "Id", "INT");
                Ecom_DBConnection_.AddParameter(Nombre, "Nombre", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Ecom_Tools.ConvevrtListString(ListaTo), "ListaTo", "TEXT");
                Ecom_DBConnection_.AddParameter(Ecom_Tools.ConvevrtListString(ListaCC), "ListaCC", "TEXT");
                Ecom_DBConnection_.AddParameter(Ecom_Tools.ConvevrtListString(ListaBCC), "ListaBCC", "TEXT");
                Ecom_DBConnection_.AddParameter((IsActive ? 1 : 0), "IsActive", "INT");
                Ecom_DBConnection_.AddParameter(mode, "ModeProcedure", "INT");
                int result = Ecom_DBConnection_.ExecProcedure();
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Get(int IdProcess)
        {
            List<Ecom_ProcesoEmail> Lista = ReadDatReader(string.Format("select * from t98_ProcesosEmail where t98_pk01 = '{0}'", IdProcess));
            if(Lista.Count > 0)
            {
                Lista.ForEach(item => {
                    Id = item.Id;
                    Nombre = item.Nombre;
                    ListaTo = item.ListaTo;
                    ListaCC = item.ListaCC;
                    ListaBCC = item.ListaBCC;
                    IsActive = item.IsActive;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_ProcesoEmail> Get()
        {
            List<Ecom_ProcesoEmail> Lista = ReadDatReader(string.Format("select * from t98_ProcesosEmail"));
            return Lista;
        }
        private List<Ecom_ProcesoEmail> ReadDatReader(string Statement)
        {
            List<Ecom_ProcesoEmail> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProcesoEmail>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProcesoEmail
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Nombre = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            ListaTo = Data.IsDBNull(2) ? Ecom_Tools.ProcessEmailList("") : Ecom_Tools.ProcessEmailList(Data.GetString(2)),
                            ListaCC = Data.IsDBNull(3) ? Ecom_Tools.ProcessEmailList("") : Ecom_Tools.ProcessEmailList(Data.GetString(3)),
                            ListaBCC = Data.IsDBNull(4) ? Ecom_Tools.ProcessEmailList("") : Ecom_Tools.ProcessEmailList(Data.GetString(4)),
                            IsActive = Data.IsDBNull(5) ? false : (Data.GetInt32(5) == 1 ? true: false),
                        });
                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "Registro no encontrado";
                }
                return List;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Data != null)
                {
                    Data.Close();
                }
            }
        }
        public void SetConnection(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion
    }
}
