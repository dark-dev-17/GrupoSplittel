using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoJumperCable
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Precio")]
        //[RegularExpression(@"^\d+\.\d{0,3}$", ErrorMessage = "Solo se permiten 3 decimales")]
        [Required]
        public double Precio { get; set; }
        [Display(Name = "Tipo de Jumper")]
        public int TipoJumper { get; set; }
        [Display(Name = "Tipo Fibra")]
        public int TipoFibra { get; set; }
        [Display(Name = "Tipo Cubierta")]
        public int TipoCubierta { get;  set; }
        [Display(Name = "Descripción tipo jumper")]
        public string TipoJumperDesc { get; private set; }
        [Display(Name = "Descripción tipo fibra")]
        public string TipoFibraDesc { get; private set; }
        [Display(Name = "Descripción tipo Cubierta")]
        public string TipoCubiertaDesc { get; private set; }

        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoJumperCable()
        {
            
        }
        public Ecom_ProductoJumperCable()
        {

        }
        public Ecom_ProductoJumperCable(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_PrecioJumperCable");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Precio, "Precio_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(TipoJumper, "TipoJumper", "INT");
                Ecom_DBConnection_.AddParameter(TipoFibra, "TipoFibra", "INT");
                Ecom_DBConnection_.AddParameter(TipoCubierta, "TipoCubierta", "INT");
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
                Ecom_DBConnection_.AddParameter(TipoJumper, "TipoJumper", "INT");
                Ecom_DBConnection_.AddParameter(TipoFibra, "TipoFibra", "INT");
                Ecom_DBConnection_.AddParameter(TipoCubierta, "TipoCubierta", "INT");
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
            return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(keyy) FROM listar_jumper_precio_cable");
        }
        public bool Get(int IdElemento)
        {
            List<Ecom_ProductoJumperCable> List = ReadDatReader(string.Format("SELECT * FROM listar_jumper_precio_cable where keyy = '{0}'", IdElemento));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Precio = item.Precio;
                    TipoJumper = item.TipoJumper;
                    TipoFibra = item.TipoFibra;
                    TipoCubierta = item.TipoCubierta;
                    TipoJumperDesc = item.TipoJumperDesc;
                    TipoFibraDesc = item.TipoFibraDesc;
                    TipoCubiertaDesc = item.TipoCubiertaDesc;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Ecom_ProductoJumperCable> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM listar_jumper_precio_cable;"));
        }
        private List<Ecom_ProductoJumperCable> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoJumperCable> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoJumperCable>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoJumperCable
                        {
                            Id = Data.IsDBNull(0) ? -1 : (int)Data.GetUInt32(0),
                            Precio = Data.IsDBNull(1) ? -1 : Data.GetDouble(1),
                            TipoJumper = Data.IsDBNull(2) ? -1 : Data.GetInt32(2),
                            TipoFibra = Data.IsDBNull(3) ? -1 : Data.GetInt32(3),
                            TipoCubierta = Data.IsDBNull(4) ? -1 : Data.GetInt32(4),
                            TipoJumperDesc = string.Format("{0} - {1}", (Data.IsDBNull(5) ? "" : Data.GetString(5)), (Data.IsDBNull(6) ? "" : Data.GetString(6))),
                            TipoFibraDesc = string.Format("{0} - {1}", (Data.IsDBNull(7) ? "" : Data.GetString(7)), (Data.IsDBNull(8) ? "" : Data.GetString(8))),
                            TipoCubiertaDesc = string.Format("{0} - {1}", (Data.IsDBNull(9) ? "" : Data.GetString(9)), (Data.IsDBNull(10) ? "" : Data.GetString(10)))
                        }); ; ;
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
