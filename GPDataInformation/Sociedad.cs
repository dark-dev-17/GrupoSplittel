using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace GPDataInformation
{
    public class Sociedad : IDataModel<Sociedad>
    {
        public int Id { get; internal set; }
        public string Descripcion { get;  set; }
        public string Direccion { get;  set; }
        public bool Activo { get;  set; }
        private DBConnection dBConnection;
        

        public bool Add()
        {
            return Actions(SociedadActions.Add);
        }
        public bool Update()
        {
            return Actions(SociedadActions.Update);
        }
        public bool Delete()
        {
            return Actions(SociedadActions.delete);
        }

        public bool Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Sociedad> Get()
        {
            throw new NotImplementedException();
        }

        public int GetLastId()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Sociedad> ReadData(string Statement)
        {
            List<Sociedad> List = null;
            MySqlDataReader Data = null;
            try
            {

                Tools.ValidDBobject(dBConnection);
                Data = dBConnection.DoQuery(Statement);
                List = new List<Sociedad>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Sociedad
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Descripcion = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Direccion = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            Activo = Data.IsDBNull(3) ? false : (Data.GetString(3) == "1" ? true : false)
                        }); ;

                    }
                    Data.Close();
                }
                else
                {
                    dBConnection.Message = "Registro no encontrado";
                }
                return List;
            }
            catch (GpExceptions ex)
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
        private bool Actions(SociedadActions sociedadActions)
        {
            dBConnection.StartProcedure("Admin_BlogCometario");
            dBConnection.AddParameter(Id, "Idd", "INT");
            dBConnection.AddParameter(Descripcion, "Idblog", "INT");
            dBConnection.AddParameter(Direccion, "IdCliente", "INT");
            dBConnection.AddParameter(Activo ? "1" : "0", "Activo_", "VARCHAR");
            dBConnection.AddParameter(sociedadActions, "ModeProcedure", "INT");
            int result = dBConnection.ExecProcedure();
            if (result == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SetConnection(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }

    }
    public enum SociedadActions
    {
        Add = 1,
        Update = 2,
        delete = 3
    }
}
