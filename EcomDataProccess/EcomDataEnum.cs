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
        ProductoSubcategoria = 3
    }
    public enum FiltroProducto
    {
        Categoria = 0,
        Subcategoria = 1,
    }
}
