﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_ProductoDistribuidorPrecar
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }
        [Display(Name = "Componente")]
        public string Componente { get; set; }
        [Display(Name = "Placas")]
        public string Placas { get; set; }
        [Display(Name = "Precio")]
        //[RegularExpression(@"^\d+\.\d{0,3}$", ErrorMessage = "Solo se permiten 3 decimales")]
        [Required]
        public double Precio { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoDistribuidorPrecar()
        {
            
        }
        public Ecom_ProductoDistribuidorPrecar()
        {

        }
        public Ecom_ProductoDistribuidorPrecar(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_PrecioDistribuidorPrecar");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Componente, "Componente_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Placas, "Placas_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Precio, "Precio_", "DOUBLE");
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

                Ecom_DBConnection_.StartProcedure("Admin_PrecioDistribuidorPrecar");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Componente, "Componente_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Placas, "Placas_", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Precio, "Precio_", "DOUBLE");
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
        public bool Get(int IdElemento)
        {
            List<Ecom_ProductoDistribuidorPrecar> List = ReadDatReader(string.Format("SELECT * FROM t26_precios_distribuidores_precargados where id = '{0}'", IdElemento));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Tipo = item.Tipo;
                    Componente = item.Componente;
                    Placas = item.Placas;
                    Precio = item.Precio;
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetlastId()
        {
            try
            {
                return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(id) FROM t26_precios_distribuidores_precargados;");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }

        public List<Ecom_ProductoDistribuidorPrecar> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM t26_precios_distribuidores_precargados;"));
        }
        private List<Ecom_ProductoDistribuidorPrecar> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoDistribuidorPrecar> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoDistribuidorPrecar>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoDistribuidorPrecar
                        {
                            Id = Data.IsDBNull(0) ? -1 : (int)Data.GetUInt32(0),
                            Tipo = Data.IsDBNull(1) ? "--" : Data.GetString(1),
                            Componente = Data.IsDBNull(2) ? "--" : Data.GetString(2),
                            Placas = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            Precio = Data.IsDBNull(5) ? -1 : Data.GetDouble(5),
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
