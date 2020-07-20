using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace GPDataInformation
{
    public class DbManager<T> where T : new()
    {
        
        private DBConnection dBConnection { get; set; }

        public T Element { get; set; }

        public DbManager()
        {

        }

        public DbManager(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
        }
        public bool Add()
        {
            return ActionsObject(DbManagerTypes.Add);
        }

        public bool Update()
        {
            return ActionsObject(DbManagerTypes.Update);
        }

        public bool Delete()
        {
            return ActionsObject(DbManagerTypes.Delete);
        }
        public int GetLastId()
        {
            return dBConnection.GetIntegerValue(string.Format("select max(Id{0}) from {0}", typeof(T).Name));
        }

        public T Get(int? id)
        {
            List<T> Lista = DataReader(string.Format("select * from {0} where Id{0} = '{1}'", typeof(T).Name, id));
            if (Lista.Count == 0)
            {
                return default(T);
            }
            return Lista.ElementAt(0);
        }

        public T GetByColumn(string id, string nameCol)
        {
            List<T> Lista = DataReader(string.Format("select * from {0} where {1} = '{2}'", typeof(T).Name, nameCol, id));
            if (Lista.Count == 0)
            {
                return default(T);
            }
            return Lista.ElementAt(0);
        }

        public List<T> Get(Predicate<T> match)
        {
            return DataReader(string.Format("select * from {0}", typeof(T).Name)).FindAll(match);
        }

        public List<T> Get(string id, string nameCol)
        {
            return DataReader(string.Format("select * from {0} where {1} = '{2}'", typeof(T).Name, nameCol, id));
        }

        public List<T> Get()
        {
            return DataReader(string.Format("select * from {0}", typeof(T).Name));
        }

        private List<T> DataReader(string SqlStatements)
        {
            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<T> Response = new List<T>();
            while (Data.Read())
            {
                object exFormAsObj = Activator.CreateInstance(typeof(T));
                foreach (var prop in typeof(T).GetProperties())
                {
                    if (prop.PropertyType.Equals(typeof(DateTime)))
                    {
                        var value = Data.GetValue(Data.GetOrdinal(prop.Name));
                        PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.DateTime), null);
                    }
                    if (prop.PropertyType.Equals(typeof(TimeSpan)))
                    {
                        var value = Data.GetValue(Data.GetOrdinal(prop.Name));
                        PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(exFormAsObj, value, null);
                    }
                    if (prop.PropertyType.Equals(typeof(double)))
                    {
                        var value = Data.GetValue(Data.GetOrdinal(prop.Name));
                        PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.Double), null);
                    }
                    if (prop.PropertyType.Equals(typeof(string)))
                    {
                        var value = Data.GetValue(Data.GetOrdinal(prop.Name));
                        PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.String), null);
                    }
                    if (prop.PropertyType.Equals(typeof(int)))
                    {
                        var value = Data.GetValue(Data.GetOrdinal(prop.Name));
                        PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                    }
                }
                Response.Add((T)exFormAsObj);
            }
            Data.Close();
            return Response;
        }
       
        private bool ActionsObject(DbManagerTypes dbManagerTypes)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if(prop.PropertyType.Equals(typeof(int)) || prop.PropertyType.Equals(typeof(string)) || prop.PropertyType.Equals(typeof(double)) || prop.PropertyType.Equals(typeof(TimeSpan)) || prop.PropertyType.Equals(typeof(DateTime)))
                {
                    PropertyInfo propertyInfo = Element.GetType().GetProperty(prop.Name);
                    procedureModels.Add(new ProcedureModel { Namefield = prop.Name, value = propertyInfo.GetValue(Element) });
                }
            }
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = dbManagerTypes });
            dBConnection.StartProcedure(string.Format("Gps_{0}", typeof(T).Name), procedureModels);
            if (dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IList GetList()
        {
            throw new NotImplementedException();
        }
    }

    public enum DbManagerTypes
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }
    
}
