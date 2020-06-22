using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcomDataProccess
{
    public class Ecom_Producto
    {
        #region Propiedades
        public int IdProducto { get;  set; }
        public string ItemCode { get;  set; }
        public string Description { get;  set; }
        public string LargeDescription { get;  set; }
        public Ecom_ProductoCategoria Category { get;  set; }
        public Ecom_ProductoSubCategoria SubCategory { get;  set; }
        public Ecom_ProductoFichaTecnica FichaTecnica { get;  set; }
        public double UnitPrice { get;  set; }
        public double Discount { get;  set; }
        public double Stock { get;  set; }
        public bool IsActiveEcomerce { get;  set; }
        public string IdDescripcionLarga { get;  set; }
        public string IdImagen { get;  set; }
        public string Categoria { get;  set; }
        public string SubCategoria { get;  set; }
        public string IdMarca { get;  set; }
        public string ImgPrincipal { get;  set; }
        public string CodigoConfigurable { get;  set; }
        public string ProductoRelacionados { get;  set; }
        public string InfoAdicional { get;  set; }
        public string PesosDimencionales { get;  set; }
        public string HojaTecnica { get;  set; }
        public string Novedades { get;  set; }
        public string InfoTecnica { get;  set; }
        public string Caracteristicas { get;  set; }
        public object Descuento { get;  set; }
        public string Valoraciones { get;  set; }

        public Ecom_ProductoDescripcion Ecom_ProductoDescripcion_;
        private Ecom_DBConnection Ecom_DBConnection_;
        private bool RequireShiptCost;
        #endregion

        #region Constructores
        ~Ecom_Producto()
        {
            
        }
        public Ecom_Producto()
        {

        }
        public Ecom_Producto(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Update(int ModeProcedure)
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_productos");
                Ecom_DBConnection_.AddParameter(ItemCode, "ItemCode", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Description, "Descripcion", "VARCHAR");
                Ecom_DBConnection_.AddParameter(IdDescripcionLarga, "IdDescripcionLarga", "VARCHAR");
                Ecom_DBConnection_.AddParameter(IdImagen, "IdImagen", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Categoria, "Categoria", "VARCHAR");
                Ecom_DBConnection_.AddParameter(SubCategoria, "SubCategoria", "VARCHAR");
                Ecom_DBConnection_.AddParameter(IdMarca, "IdMarca", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Valoraciones, "Valoraciones", "VARCHAR");
                Ecom_DBConnection_.AddParameter(UnitPrice, "Precio", "DOUBLE");
                Ecom_DBConnection_.AddParameter(Descuento, "Descuento", "INT");
                Ecom_DBConnection_.AddParameter(Caracteristicas, "Caracteristicas", "VARCHAR");
                Ecom_DBConnection_.AddParameter(InfoTecnica, "InfoTecnica", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Novedades, "Novedades", "VARCHAR");
                Ecom_DBConnection_.AddParameter(HojaTecnica, "HojaTecnica", "VARCHAR");
                Ecom_DBConnection_.AddParameter(PesosDimencionales, "PesosDimencionales", "VARCHAR");
                Ecom_DBConnection_.AddParameter(InfoAdicional, "InfoAdicional", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ProductoRelacionados, "ProductoRelacionados", "VARCHAR");
                Ecom_DBConnection_.AddParameter(CodigoConfigurable, "CodigoConfigurable", "VARCHAR");
                Ecom_DBConnection_.AddParameter((IsActiveEcomerce ? "si" : "no"), "IsActiveEcomerce", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgPrincipal, "ImgPrincipal", "TEXT");
                Ecom_DBConnection_.AddParameter((RequireShiptCost ? "si" : "no"), "RequireShiptCost", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ModeProcedure, "ModeProcedure", "INT");
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
        public bool UpdImagenPrincipal(string ItemCode_, string ImagenPrincipal)
        {
            try
            {
                string Statement = string.Format("Admin_Producto_UpdImagenPrincial|ItemCode@VARCHAR={0}&ImagenNombre@VARCHAR={1}", ItemCode_, ImagenPrincipal);
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
        public bool UpdLargeDescription(string ItemCode_, string LargeDesc)
        {
            try
            {
                string Statement = string.Format("Admin_Producto_UpdDescripcionLarga|ItemCode@VARCHAR={0}&LargeDescription@VARCHAR={1}", ItemCode_, LargeDesc);
                int result = Ecom_DBConnection_.ExecuteStoreProcedure(Statement);
                if(result == 0)
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
        public bool UpdActive(string ItemCode_, bool IsActive)
        {
            try
            {
                string Statement = string.Format("Admin_Producto_ActDesc|ItemCode@VARCHAR={0}&IsActiveEcomerce@VARCHAR={1}", ItemCode_, (IsActive ? "si" : "no"));
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

        public async Task<bool> Get(string codigo_)
        {
            bool result = false;
            try
            {
                string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria where codigo = '{0}'", codigo_);
                List<Ecom_Producto> List = await ReadDatReader(Statement);
                if(List.Count == 1)
                {
                    List.ForEach(item =>
                    {
                        IdProducto = item.IdProducto;
                        ItemCode = item.ItemCode;
                        Description = item.Description;
                        UnitPrice = item.UnitPrice;
                        Discount = item.Discount;
                        Stock = item.Stock;
                        IsActiveEcomerce = item.IsActiveEcomerce;
                        Category = item.Category;
                        SubCategory = item.SubCategory;
                        LargeDescription = item.LargeDescription;
                        FichaTecnica = item.FichaTecnica;
                        IdDescripcionLarga = item.IdDescripcionLarga;
                        FichaTecnica = new Ecom_ProductoFichaTecnica(Ecom_DBConnection_).GetID(FichaTecnica.Id);
                        Ecom_ProductoDescripcion_ = new Ecom_ProductoDescripcion(Ecom_DBConnection_);
                        Ecom_ProductoDescripcion_.Get(IdDescripcionLarga);

                    });
                    result = true;
                }
                return result;
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Ecom_Producto>> Get()
        {
            try
            {
                string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria where codigo_configurable  = '';");
                return await ReadDatReader(Statement);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Ecom_Producto>> GetConf()
        {
            try
            {
                string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria where codigo_configurable  != '';");
                return await ReadDatReader(Statement);
            }
            catch (Ecom_Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Ecom_Producto>> Get(string Regla, FiltroProducto filtroProducto)
        {
            try
            {
                if(filtroProducto == FiltroProducto.Categoria)
                {
                    string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria where id_codigo  = '{0}';", Regla);
                    return await ReadDatReader(Statement);
                }
                else if (filtroProducto == FiltroProducto.Subcategoria)
                {
                    string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria where id_subcategoria = '{0}';", Regla);
                    return await ReadDatReader(Statement);
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
        private async Task<List<Ecom_Producto>> ReadDatReader(string Statement)
        {
            List<Ecom_Producto> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_Producto>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_Producto
                        {
                            IdProducto = (int)Data.GetUInt32(0),
                            ItemCode = Data.IsDBNull(1) ? "" : Data.GetString(1),
                            Description = Data.IsDBNull(2) ? "" : Data.GetString(2),
                            UnitPrice = Data.IsDBNull(3) ? 0 : Data.GetFloat(3),
                            Discount = Data.IsDBNull(15) ? 0 : Data.GetFloat(15),//
                            Stock = Data.IsDBNull(4) ? 0 : Data.GetFloat(4),
                            IsActiveEcomerce = Data.IsDBNull(16) ? false : Data.GetString(16) == "si" ? true : false,//
                            Category = new Ecom_ProductoCategoria { Description = Data.IsDBNull(6) ? "" : Data.GetString(6), Id = Data.IsDBNull(5) ? 0 : Data.GetInt32(5) },
                            SubCategory = new Ecom_ProductoSubCategoria { Description = Data.IsDBNull(10) ? "" : Data.GetString(10), Id = Data.IsDBNull(8) ? 0 : Data.GetInt32(8) },
                            IdDescripcionLarga = Data.IsDBNull(13) ? "" : Data.GetString(13),
                            LargeDescription = Data.IsDBNull(14) ? "Sin descripción" : Data.GetString(14),
                            FichaTecnica = new Ecom_ProductoFichaTecnica { Id = Data.IsDBNull(11) ? 0 : Data.GetInt32(11), Ruta = Data.IsDBNull(12) ? "" : Data.GetString(12) },
                        });

                    }
                    Data.Close();
                }
                else
                {
                    Ecom_DBConnection_.Message = "No existen registros";
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
