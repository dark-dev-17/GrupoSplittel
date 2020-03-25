using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public class Ecom_ProductoConfigurable
    {
        #region Propiedades
        public Ecom_ProductoCategoria Ecom_ProductoCategoria_ { get; private set; }
        public Ecom_ProductoSubCategoria Ecom_ProductoSubCategoria_ { get; private set; }
        public string CodigoProducto { get;  set; }
        public string Descripcion { get;  set; }
        public string ForderName { get;  set; }
        public string ClaveCodigoProg { get;  set; }
        public bool IsActiveEcommerce { get;  set; }
        public bool IsProximanente { get;  set; }
        //public List<Ecom_Producto> Productos { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_ProductoConfigurable()
        {
            
        }
        public Ecom_ProductoConfigurable()
        {

        }
        public Ecom_ProductoConfigurable(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                string Statement = string.Format("Admin_Configurable|CategoriaID@VARCHAR={0}" +
                    "&SubcategoriaID@VARCHAR={1}" +
                    "&CodigoProducto@VARCHAR={2}" +
                    "&Descripcion@TEXT={3}" +
                    "&ForderName@TEXT={4}" +
                    "&ClaveCodigoProg@VARCHAR={5}" +
                    "&IsActiveEcommerce@VARCHAR={6}" +
                    "&IsProximanente@INT={7}" +
                    "&ModeProcedure@INT={8}", Ecom_ProductoCategoria_.Id_categoria, Ecom_ProductoSubCategoria_.Id_subcategoria, CodigoProducto, Descripcion, ForderName, ClaveCodigoProg, (IsActiveEcommerce ? "si" : "no"), (IsProximanente ? 0 : 1), 1);
                int result = Ecom_DBConnection_.ExecuteStoreProcedure(Statement);
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    throw new Ecom_Exception(Ecom_DBConnection_.Message);
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Update()
        {
            try
            {
                string Statement = string.Format("Admin_Configurable|CategoriaID@VARCHAR={0}" +
                    "&SubcategoriaID@VARCHAR={1}" +
                    "&CodigoProducto@VARCHAR={2}" +
                    "&Descripcion@TEXT={3}" +
                    "&ForderName@TEXT={4}" +
                    "&ClaveCodigoProg@VARCHAR={5}" +
                    "&IsActiveEcommerce@VARCHAR={6}" +
                    "&IsProximanente@INT={7}" +
                    "&ModeProcedure@INT={8}", Ecom_ProductoCategoria_.Id_categoria, Ecom_ProductoSubCategoria_.Id_subcategoria, CodigoProducto, Descripcion, ForderName, ClaveCodigoProg,( IsActiveEcommerce ? "si": "no"),( IsProximanente ? 0 : 1), 2);
                int result = Ecom_DBConnection_.ExecuteStoreProcedure(Statement);
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    throw new Ecom_Exception(Ecom_DBConnection_.Message);
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete()
        {
            try
            {
                string Statement = string.Format("Admin_Configurable|CategoriaID@VARCHAR={0}" +
                    "&SubcategoriaID@VARCHAR={1}" +
                    "&CodigoProducto@VARCHAR={2}" +
                    "&Descripcion@TEXT={3}" +
                    "&ForderName@TEXT={4}" +
                    "&ClaveCodigoProg@VARCHAR={5}" +
                    "&IsActiveEcommerce@VARCHAR={6}" +
                    "&IsProximanente@INT={7}" +
                    "&ModeProcedure@INT={8}", Ecom_ProductoCategoria_.Id_categoria, Ecom_ProductoSubCategoria_.Id_subcategoria, CodigoProducto, Descripcion, ForderName, ClaveCodigoProg, (IsActiveEcommerce ? "si" : "no"), (IsProximanente ? 0 : 1), 3);
                int result = Ecom_DBConnection_.ExecuteStoreProcedure(Statement);
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    throw new Ecom_Exception(Ecom_DBConnection_.Message);
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_ProductoConfigurable> Get(string Regla, FiltroProducto filtroProducto)
        {
            try
            {
                if(filtroProducto == FiltroProducto.Categoria)
                {
                    return ReadDatReader(string.Format("select * from admin_productosConfigurables where id_categoria = '{0}'", Regla));
                }
                else if (filtroProducto == FiltroProducto.Subcategoria)
                {
                    return ReadDatReader(string.Format("select * from admin_productosConfigurables where id_subcategoria = '{0}'", Regla));
                }
                else
                {
                    throw new Ecom_Exception(string.Format("El tipo de consulta '{0}' no es valido", filtroProducto.ToString()));
                }
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public bool Get(string Codigo)
        {
            try
            {
                List<Ecom_ProductoConfigurable> Data = ReadDatReader(string.Format("select * from admin_productosConfigurables where codigo = '{0}'", Codigo));
                if(Data.Count == 1)
                {
                    Data.ForEach(Item => {
                        Ecom_ProductoCategoria_ = Item.Ecom_ProductoCategoria_;
                        Ecom_ProductoSubCategoria_ = Item.Ecom_ProductoSubCategoria_;
                        CodigoProducto = Item.CodigoProducto;
                        Descripcion = Item.Descripcion;
                        ForderName = Item.ForderName;
                        ClaveCodigoProg = Item.ClaveCodigoProg;
                        IsActiveEcommerce = Item.IsActiveEcommerce;
                        IsProximanente = Item.IsProximanente;
                    });
                    return true;
                }
                else
                {
                    Ecom_DBConnection_.Message = string.Format("No se encontro el producto: '{0}'",Codigo);
                    return false;
                }
                
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public List<Ecom_ProductoConfigurable> Get()
        {
            try
            {
                return ReadDatReader("select * from admin_productosConfigurables");
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        private List<Ecom_ProductoConfigurable> ReadDatReader(string Statement)
        {
            List<Ecom_ProductoConfigurable> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_ProductoConfigurable>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_ProductoConfigurable
                        {
                            CodigoProducto = Data.IsDBNull(3) ? "" : Data.GetString(3),
                            Descripcion = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            ForderName = Data.IsDBNull(5) ? "" : Data.GetString(5),
                            ClaveCodigoProg = Data.IsDBNull(6) ? "" : Data.GetString(6),
                            IsActiveEcommerce = Data.IsDBNull(8) ? false : (Data.GetString(8) == "si" ? true : false),
                            IsProximanente = Data.IsDBNull(9) ? false : (Data.GetInt32(9) == 0 ? true : false),
                            Ecom_ProductoCategoria_ = new Ecom_ProductoCategoria { Id_categoria = Data.IsDBNull(1) ? "" : Data.GetString(1), Description = Data.IsDBNull(11) ? "" : Data.GetString(11) },
                            Ecom_ProductoSubCategoria_ = new Ecom_ProductoSubCategoria { Id_subcategoria = Data.IsDBNull(2) ? "" : Data.GetString(2), Description = Data.IsDBNull(10) ? "" : Data.GetString(10) }
                        }); ;

                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "No existen datos";
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
        #endregion
    }
    }
