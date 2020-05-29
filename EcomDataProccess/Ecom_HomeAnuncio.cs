using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomDataProccess
{
    public class Ecom_HomeAnuncio
    {
        #region Propiedades
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Descripcion slide")]
        public string Descipcion { get; set; }
        public string ImgSmall { get;  set; }
        [Display(Name = "Imagen izquierda")]
        public IFormFile ImgSmall_ { get; set; }
        public string ImgLarge { get;  set; }
        [Display(Name = "Imagen derecha")]
        public IFormFile ImgLarge_ { get; set; }
        [Display(Name = "Link izquierdo")]
        public string ImgSmallLink { get; set; }
        [Display(Name = "Nueva pestaña")]
        public bool ImgSmallLinkNewTab { get; set; }
        [Display(Name = "Nueva pestaña")]
        public bool ImgLargeLinkNewTab { get; set; }
        [Display(Name = "Link derecho")]
        public string ImgLargeLink { get; set; }
        [Display(Name = "Link general")]
        public string Link { get; set; }
        [Display(Name = "Mostrar a")]
        public string ShowBy { get; set; }
        [Display(Name = "Clientes B2B y grupo(SAP)", Description = "Este campo aplicara solo para clientes B2B")]
        public string Group { get; set; }
        [Display(Name = "Referencia a producto",Description ="Este campo aplicara solo cuando el anuncio este ligado a un producto")]
        public string ItemCode { get; set; }
        [Display(Name = "Operador de regla")]
        public string RuleOperator { get; set; }
        [Display(Name = "Tipo regla")]
        public string RuleTopic { get; set; }
        [Display(Name = "Total($USD)")]
        public double Quantity { get; set; }
        [Display(Name = "Visible en Ecommerce")]
        public bool IsActive { get; set; }
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }
        public List<int> GroupoCode { get; set; }
        private string Rule
        {
            get { return string.Format("{0}@{1}@{2}", RuleTopic, RuleOperator, Quantity); }
        }
        public int Position { get; set; }
        private Ecom_DBConnection Ecom_DBConnection_;
        #endregion

        #region Constructores
        ~Ecom_HomeAnuncio()
        {
            
        }
        public Ecom_HomeAnuncio()
        {

        }
        public Ecom_HomeAnuncio(Ecom_DBConnection Ecom_DBConnection_)
        {
            this.Ecom_DBConnection_ = Ecom_DBConnection_;
        }
        #endregion

        #region Metodos
        public bool Add()
        {
            try
            {
                Group = GroupoCode == null ? "" : string.Join(",", GroupoCode);
                Ecom_DBConnection_.StartProcedure("Admin_HomeAnuncio");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Descipcion, "Descipcion", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgSmall, "ImgSmall", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgLarge, "ImgLarge", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgSmallLink, "ImgSmallLink", "TEXT");
                Ecom_DBConnection_.AddParameter(ImgLargeLink, "ImgLargeLink", "TEXT");
                Ecom_DBConnection_.AddParameter(Link, "Link", "TEXT");
                Ecom_DBConnection_.AddParameter(ShowBy, "ShowBy", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Group, "Groupp", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ItemCode, "ItemCode", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Rule, "Rule", "TEXT");
                Ecom_DBConnection_.AddParameter(Position, "Positionn", "INT");
                Ecom_DBConnection_.AddParameter(IsActive ? "si": "no", "IsActive", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgSmallLinkNewTab ? "si": "no", "ImgSmallLinkNewTab", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgLargeLinkNewTab ? "si": "no", "ImgLargeLinkNewTab", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Categoria, "Categoria", "VARCHAR");
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
        public bool Update(HomeAnuncioActionsDB ActionType)
        {
            try
            {
                Group = GroupoCode == null ? "" :string.Join(",", GroupoCode);
                Ecom_DBConnection_.StartProcedure("Admin_HomeAnuncio");
                Ecom_DBConnection_.AddParameter(Id, "Idd", "INT");
                Ecom_DBConnection_.AddParameter(Descipcion, "Descipcion", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgSmall, "ImgSmall", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgLarge, "ImgLarge", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgSmallLink, "ImgSmallLink", "TEXT");
                Ecom_DBConnection_.AddParameter(ImgLargeLink, "ImgLargeLink", "TEXT");
                Ecom_DBConnection_.AddParameter(Link, "Link", "TEXT");
                Ecom_DBConnection_.AddParameter(ShowBy, "ShowBy", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Group, "Groupp", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ItemCode, "ItemCode", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Rule, "Rule", "TEXT");
                Ecom_DBConnection_.AddParameter(Position, "Positionn", "INT");
                Ecom_DBConnection_.AddParameter(IsActive ? "si" : "no", "IsActive", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgSmallLinkNewTab ? "si" : "no", "ImgSmallLinkNewTab", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ImgLargeLinkNewTab ? "si" : "no", "ImgLargeLinkNewTab", "VARCHAR");
                Ecom_DBConnection_.AddParameter(Categoria, "Categoria", "VARCHAR");
                Ecom_DBConnection_.AddParameter(ActionType, "ModeProcedure", "INT");
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
        public bool Get(int IdAnuncio)
        {
            List<Ecom_HomeAnuncio> List = ReadDatReader(string.Format("select * from t35_HomeSlide where t35_pk01 = {0}", IdAnuncio));
            if (List.Count > 0)
            {
                List.ForEach(item => {
                    Id = item.Id;
                    Descipcion = item.Descipcion;
                    ImgSmall = item.ImgSmall;
                    ImgLarge = item.ImgLarge;
                    ImgSmallLink = item.ImgSmallLink;
                    ImgLargeLink = item.ImgLargeLink;
                    Link = item.Link;
                    ShowBy = item.ShowBy;
                    Group = item.Group;
                    ItemCode = item.ItemCode;
                    RuleOperator = item.RuleOperator;
                    RuleTopic = item.RuleTopic;
                    Quantity = item.Quantity;
                    Position = item.Position;
                    IsActive = item.IsActive;
                    Categoria = item.Categoria;
                    GroupoCode = item.GroupoCode;
                    ImgSmallLinkNewTab = item.ImgSmallLinkNewTab;
                    ImgLargeLinkNewTab = item.ImgLargeLinkNewTab;
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
            return Ecom_DBConnection_.ExecuteScalarInt("select max(t35_pk01) from t35_HomeSlide");
        }
        public List<Ecom_HomeAnuncio> Get()
        {
            return ReadDatReader(string.Format("select * from t35_HomeSlide"));
        }
        private List<Ecom_HomeAnuncio> ReadDatReader(string Statement)
        {
            List<Ecom_HomeAnuncio> List = null;
            MySqlDataReader Data = null;
            try
            {
                Ecom_Tools.ValidDBobject(Ecom_DBConnection_);
                Data = Ecom_DBConnection_.DoQuery(Statement);
                List = new List<Ecom_HomeAnuncio>();
                if (Data.HasRows)
                {
                    while (Data.Read())
                    {
                        string regla = Data.IsDBNull(10) ? "-@-@-" : Data.GetString(10).Replace('.','@');
                        Ecom_HomeAnuncio Ecom_HomeAnuncio_ = new Ecom_HomeAnuncio();
                        Ecom_HomeAnuncio_.Id = Data.IsDBNull(0) ? -1 : (int)Data.GetUInt32(0);
                        Ecom_HomeAnuncio_.Descipcion = Data.IsDBNull(1) ? "" : Data.GetString(1);
                        Ecom_HomeAnuncio_.ImgSmall = Data.IsDBNull(2) ? "" : Data.GetString(2);
                        Ecom_HomeAnuncio_.ImgLarge = Data.IsDBNull(3) ? "" : Data.GetString(3);
                        Ecom_HomeAnuncio_.ImgSmallLink = Data.IsDBNull(4) ? "" : Data.GetString(4);
                        Ecom_HomeAnuncio_.ImgLargeLink = Data.IsDBNull(5) ? "" : Data.GetString(5);
                        Ecom_HomeAnuncio_.Link = Data.IsDBNull(6) ? "" : Data.GetString(6);
                        Ecom_HomeAnuncio_.ShowBy = Data.IsDBNull(7) ? "" : Data.GetString(7);
                        Ecom_HomeAnuncio_.Group = Data.IsDBNull(8) ? "" : Data.GetString(8);
                        Ecom_HomeAnuncio_.ItemCode = Data.IsDBNull(9) ? "" : Data.GetString(9);
                        Ecom_HomeAnuncio_.RuleTopic = regla.Split('@')[0];
                        Ecom_HomeAnuncio_.RuleOperator = regla.Split('@')[1];
                        Ecom_HomeAnuncio_.Quantity = double.Parse(regla.Split('@')[2]);
                        Ecom_HomeAnuncio_.Position = Data.IsDBNull(11) ? -1 : Data.GetInt32(11);
                        Ecom_HomeAnuncio_.IsActive = Data.IsDBNull(12) ? false : Data.GetString(12) == "si" ? true : false;
                        Ecom_HomeAnuncio_.ImgSmallLinkNewTab = Data.IsDBNull(14) ? false : Data.GetString(14) == "si" ? true : false;
                        Ecom_HomeAnuncio_.ImgLargeLinkNewTab = Data.IsDBNull(15) ? false : Data.GetString(15) == "si" ? true : false;
                        Ecom_HomeAnuncio_.Categoria = Data.IsDBNull(13) ? "" : Data.GetString(13);

                        Ecom_HomeAnuncio_.GroupoCode = new List<int>();
                        foreach (string code in Ecom_HomeAnuncio_.Group.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            Ecom_HomeAnuncio_.GroupoCode.Add(Int32.Parse(code));
                        }
                        List.Add(Ecom_HomeAnuncio_);
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

    public enum HomeAnuncioActionsDB:int{
        AddNew = 1,
        UpdateData = 2,
        UpdateImgLeft = 3,
        UpdateImgRight = 4,
        UpdatePosition = 5,
        Delete = 6,
    }
}
