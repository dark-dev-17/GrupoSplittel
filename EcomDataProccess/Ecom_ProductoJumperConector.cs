using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoJumperConector
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Precio")]
        [RegularExpression(@"^\d+\.\d{0,3}$", ErrorMessage = "Solo se permiten 3 decimales")]
        [Required]
        public double Precio { get; set; }
        [Display(Name = "Conector")]
        public int Conector { get; set; }
        [Display(Name = "Tipo de Jumper")]
        public int TipoJumper { get; set; }
        [Display(Name = "Pulido")]
        public int Pulido { get; set; }
        [Display(Name = "Conector descripción")]
        public string ConectorDesc { get; set; }
        [Display(Name = "Descripción tipo jumper")]
        public string TipoJumperDesc { get; set; }
        [Display(Name = "Descripción pulido")]
        public string PulidoDesc { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoJumperConector()
        {
            
        }
        public Ecom_ProductoJumperConector()
        {

        }
        public Ecom_ProductoJumperConector(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_PrecioJumperConector");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Precio, "Precio_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Conector, "Conector", "INT");
                Ecom_DBConnection_.AddParameter(TipoJumper, "TipoJumper", "INT");
                Ecom_DBConnection_.AddParameter(Pulido, "Pulido", "INT");
                Ecom_DBConnection_.AddParameter(1, "ModeProcedure", "INT");
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
        public bool Update(int modeUpdate)
        {
            try
            {

                Ecom_DBConnection_.StartProcedure("Admin_PrecioJumperConector");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Precio, "Precio_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Conector, "Conector", "INT");
                Ecom_DBConnection_.AddParameter(TipoJumper, "TipoJumper", "INT");
                Ecom_DBConnection_.AddParameter(Pulido, "Pulido", "INT");
                Ecom_DBConnection_.AddParameter(modeUpdate, "ModeProcedure", "INT");
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
        public int GetlastId()
        {
            return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(keyy) FROM listar_jumper_precio_conector");
        }
        public bool Get(int IdElemento)
        {
            List<Ecom_ProductoJumperConector> List = ReadDatReader(string.Format("SELECT * FROM listar_jumper_precio_conector where keyy = '{0}'", IdElemento));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Precio = item.Precio;
                    Conector = item.Conector;
                    TipoJumper = item.TipoJumper;
                    Pulido = item.Pulido;
                    ConectorDesc = item.ConectorDesc;
                    TipoJumperDesc = item.TipoJumperDesc;
                    PulidoDesc = item.PulidoDesc;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_ProductoJumperConector> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM listar_jumper_precio_conector;"));
        }
        private List<Ecom_ProductoJumperConector> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoJumperConector> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoJumperConector>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoJumperConector
                        {
                            Id = Data.IsDBNull(0) ? -1 : (int)Data.GetUInt32(0),
                            Precio = Data.IsDBNull(1) ? -1 : Data.GetDouble(1),
                            Conector = Data.IsDBNull(2) ? -1 : Data.GetInt32(2),
                            TipoJumper = Data.IsDBNull(3) ? -1 : Data.GetInt32(3),
                            Pulido = Data.IsDBNull(4) ? -1 : Data.GetInt32(4),
                            ConectorDesc = string.Format("{0}", (Data.IsDBNull(5) ? "" : Data.GetString(5))),
                            TipoJumperDesc = string.Format("{0} - {1}", (Data.IsDBNull(6) ? "" : Data.GetString(6)), (Data.IsDBNull(7) ? "" : Data.GetString(7))),
                            PulidoDesc = string.Format("{0} - {1}", (Data.IsDBNull(8) ? "" : Data.GetString(8)), (Data.IsDBNull(9) ? "" : Data.GetString(9)))
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
