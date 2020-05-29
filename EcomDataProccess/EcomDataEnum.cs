using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace EcomDataProccess
{
    public enum ServerSource
    {
        Ecommerce = 0,
        Splitnet = 1
    }
    public enum ObjectSource
    {
        ProductoConfigurable = 0,
        ProductoFijo = 1,
        ProductoCategoria = 2,
        ProductoSubcategoria = 3,
        Blog = 4,
        PedidoLine = 5,
        Pedido = 6,
        Cliente = 7,
        Usuario = 8,
        UsuarioArea = 9,
        Notificacion = 10,
        DireccionEnvio = 11,
        DireccionFacturacion = 12,
        ProcesoEmail = 13,
        ProductoDescripcion = 14,
        ProductoFichaTecnica = 15,
        ProductoPatchCord = 16,
        ProductoMPO = 17,
        ProductoPigtail = 18,
        ProductoCabServicio = 19,
        ProductoDistribuidor = 20,
        ProductoJumperConector = 21,
        ProductoJumperCable = 22,
        ProductoDistribuidorPrecon = 23,
        ProductoDistribuidorPrecar = 24,
        HomeAnuncio = 25,
    }
    public enum FiltroProducto
    {
        Categoria = 0,
        Subcategoria = 1,
    }
    public enum EmailList
    {
        To = 0,
        CC = 1,
        BCC = 2,
    }
}
