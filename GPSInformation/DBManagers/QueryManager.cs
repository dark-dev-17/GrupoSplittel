using GPSInformation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GPSInformation.DBManagers
{
    public class QueryManager<T>
    {
        public void Where(JQ_Match<T> match)
        {

        }

        
    }

    public class manager
    {
        public QueryManager<Persona> queryManager { get; set; }

        public void Estart()
        {
            queryManager = new QueryManager<Persona>();
            queryManager.Where(a => a.Calle == "");
        }
    }


    public enum JqOperator
    {
        And = 1,
        Or = 2,
        Between = 3,
        Different = 4,
    }

    public delegate bool JQ_Match<in T>(T obj);
}
