using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_BlogComentario
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Required]
        public int Idblog { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public int IdComentario { get; set; }
        [Required]
        public string Comentario { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public bool Activo { get; set; }
        //atributos complementarios
        public string NombreCliente { get; private set; }

        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_BlogComentario()
        {

        }
        public Ecom_BlogComentario()
        {

        }
        public Ecom_BlogComentario(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Ecom_DBConnection_.StartProcedure("Admin_BlogCometario");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Idblog, "Idblog", "INT");
                Ecom_DBConnection_.AddParameter(IdCliente, "IdCliente", "INT");
                Ecom_DBConnection_.AddParameter(IdComentario, "IdComentario", "INT");
                Ecom_DBConnection_.AddParameter(Comentario, "Comentario_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Fecha, "Fecha_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Activo ? "1" : "0", "Activo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Ecom_BlogComentarioActions.Agregar, "ModeProcedure", "INT");
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
        public bool Update(Ecom_BlogComentarioActions Action)
        {
            try
            {

                Ecom_DBConnection_.StartProcedure("Admin_BlogCometario");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Idblog, "Idblog", "INT");
                Ecom_DBConnection_.AddParameter(IdCliente, "IdCliente", "INT");
                Ecom_DBConnection_.AddParameter(IdComentario, "IdComentario", "INT");
                Ecom_DBConnection_.AddParameter(Comentario, "Comentario_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Tipo, "Tipo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Fecha, "Fecha_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Activo ? "1" : "0", "Activo_", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Action, "ModeProcedure", "INT");
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
        public bool Get(int IdComentario)
        {
            List<Ecom_BlogComentario> List = ReadDatReader(string.Format("SELECT * FROM menu_blog_comentarios where id = '{0}'", IdComentario));
            if (List.Count > 0)
            {
                List.ForEach(item =>
                {
                    Id = item.Id;
                    Idblog = item.Idblog;
                    IdCliente = item.IdCliente;
                    IdComentario = item.IdComentario;
                    Comentario = item.Comentario;
                    Tipo = item.Tipo;
                    Fecha = item.Fecha;
                    Activo = item.Activo;
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public int LastId()
        {
            return Ecom_DBConnection_.ExecuteScalarInt("SELECT max(id) FROM menu_blog_comentarios;");
        }
        public List<Ecom_BlogComentario> Get()
        {
            return ReadDatReader(string.Format("SELECT * FROM menu_blog_comentarios;"));
        }
        public List<Ecom_BlogComentario> GetByBlog(int IdBlogg)
        {
            List<Ecom_BlogComentario> Lista = ReadDatReader(string.Format("SELECT * FROM menu_blog_comentarios where id_blog = '{0}'", IdBlogg));
            Lista.ForEach(comentario =>
            {
                if(comentario.IdCliente == 0)
                {
                    comentario.NombreCliente = "Usuario";
                }
                else if (comentario.IdCliente == -1)
                {
                    comentario.NombreCliente = "Soporte Fibremex";
                }
                else
                {
                    Ecom_Cliente ecom_Cliente = new Ecom_Cliente(Ecom_DBConnection_);
                    if (ecom_Cliente.Get(comentario.IdCliente))
                    {
                        comentario.NombreCliente = ecom_Cliente.Nombre + " " + ecom_Cliente.Apellidos;
                    }
                }
            });
            return Lista;
        }
        private List<Ecom_BlogComentario> ReadDatReader(string Statement)
        {
            List<Ecom_BlogComentario> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_BlogComentario>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        List.Add(new Ecom_BlogComentario
                        {
                            Id = Data.IsDBNull(0) ? 0 : (int)Data.GetUInt32(0),
                            Idblog = Data.IsDBNull(1) ? -1 : Data.GetInt32(1),
                            IdCliente = Data.IsDBNull(2) ? -1 : Data.GetInt32(2),
                            IdComentario = Data.IsDBNull(3) ? -1 : Data.GetInt32(3),
                            Comentario = Data.IsDBNull(4) ? "" : Data.GetString(4),
                            Tipo = Data.IsDBNull(5) ? "" : Data.GetString(5),
                            Fecha = Data.IsDBNull(6) ? DateTime.Today : Data.GetDateTime(6),
                            Activo = Data.IsDBNull(7) ? false : (Data.GetString(7) == "1" ? true : false)
                        }); ;

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
    public enum Ecom_BlogComentarioActions
    {
        Agregar = 1,
        EditarAll = 2,
        EditarEstatus = 3,
        Eliminar = 4,
    }
}
