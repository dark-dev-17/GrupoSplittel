using DbManagerDark;
using DbManagerDark.Managers;
using EcommerceApiLogic.Models;
using EcommerceApiLogic.Validators;
using Microsoft.Extensions.Configuration;
using System;

namespace EcommerceApiLogic
{
    public class DarkDev : DbManagerDark.DarkManager
    {
        public DarkManagerMySQL<Pedido> Pedido { get; set; }
        public DarkManagerMySQL<Usuario> Usuario { get; set; }
        public TokenValidationAction tokenValidationAction;
        public DarkDev(IConfiguration configuration, DarkMode darkMode) : base(configuration, darkMode)
        {
            tokenValidationAction = new TokenValidationAction(configuration);
        }

        public void LoadObject(MysqlObject mysqlObject)
        {
            if (mysqlObject == MysqlObject.Pedido)
            {
                Pedido = new DarkManagerMySQL<Pedido>(this.ConnectionMySQL);
            }
            else if (mysqlObject == MysqlObject.Usuario)
            {
                Usuario = new DarkManagerMySQL<Usuario>(this.ConnectionMySQL);
            }
        }
    }
    public enum MysqlObject
    {
        Pedido = 1,
        Usuario = 2,
    }

}
