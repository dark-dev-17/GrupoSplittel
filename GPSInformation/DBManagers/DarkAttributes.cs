using GPSInformation.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GPSInformation.DBManagers
{
    public class DarkAttributes<T> where T : new()
    {
        private DBConnection dBConnection { get; set; }
        private string Nametable { get; set; }

        public T Element { get; set; }

        public DarkAttributes()
        {
            Nametable = GetRealNameClass();
        }

        public DarkAttributes(DBConnection dBConnection)
        {
            this.dBConnection = dBConnection;
            Nametable = GetRealNameClass();
        }
        private string GetRealNameClass()
        {
            string Nombre = "";
            TableDB tableDefinifiton = GetClassAttribute();
            if (tableDefinifiton.IsMappedByLabels)
            {
                Nombre = tableDefinifiton.Name;
            }
            else
            {
                Nombre = Nametable;
            }
            return Nombre;
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
            return dBConnection.GetIntegerValue(string.Format("select max(Id{0}) from {0}", Nametable));
        }

        public T Get(int? id)
        {
            List<T> Lista = DataReader(string.Format("select * from {0} where Id{0} = '{1}'", Nametable, id));
            if (Lista.Count == 0)
            {
                return default(T);
            }
            return Lista.ElementAt(0);
        }

        public T GetByColumn(string id, string nameCol)
        {
            List<T> Lista = DataReader(string.Format("select * from {0} where {1} = '{2}'", Nametable, nameCol, id));
            if (Lista.Count == 0)
            {
                return default(T);
            }
            return Lista.ElementAt(0);
        }

        public List<T> Get(Predicate<T> match)
        {
            return DataReader(string.Format("select * from {0}", Nametable)).FindAll(match);
        }

        public List<T> Get(string id, string nameCol)
        {
            return DataReader(string.Format("select * from {0} where {1} = '{2}'", Nametable, nameCol, id));
        }

        public List<T> Get()
        {
            return DataReader(string.Format("select * from {0}", Nametable));
        }

        private List<T> DataReader(string SqlStatements)
        {
            TableDB tableDefinifiton = GetClassAttribute();

            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<T> Response = new List<T>();

            //mapeo de tabla con los nombres de los campos ya existentes
            if (tableDefinifiton.IsMappedByLabels)
            {
                while (Data.Read())
                {
                    object exFormAsObj = Activator.CreateInstance(typeof(T));
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                        ColumnDB hiddenAttribute = (ColumnDB)propertyInfo.GetCustomAttribute(typeof(ColumnDB));

                        if (hiddenAttribute == null)
                        {
                            throw new Exceptions.GpExceptions(string.Format("The attribute was not found in the attribute '{0}', if you don´t want to use mapTable, please set IsMappedByLabels = false", prop.Name));
                        }

                        var value = Data.GetValue(Data.GetOrdinal(hiddenAttribute.Name.Trim()));

                        if (prop.PropertyType.Equals(typeof(DateTime)))
                        {
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.DateTime), null);
                        }
                        if (prop.PropertyType.Equals(typeof(TimeSpan)))
                        {
                            propertyInfo.SetValue(exFormAsObj, value, null);
                        }
                        if (prop.PropertyType.Equals(typeof(double)))
                        {
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.Double), null);
                        }
                        if (prop.PropertyType.Equals(typeof(string)))
                        {
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.String), null);
                        }
                        if (prop.PropertyType.Equals(typeof(int)))
                        {
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                    }
                    Response.Add((T)exFormAsObj);
                }
                Data.Close();
            }
            else
            {
                while (Data.Read())
                {
                    object exFormAsObj = Activator.CreateInstance(typeof(T));
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        var value = Data.GetValue(Data.GetOrdinal(prop.Name));
                        PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                        if (prop.PropertyType.Equals(typeof(DateTime)))
                        {
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.DateTime), null);
                        }
                        if (prop.PropertyType.Equals(typeof(TimeSpan)))
                        {
                            propertyInfo.SetValue(exFormAsObj, value, null);
                        }
                        if (prop.PropertyType.Equals(typeof(double)))
                        {
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.Double), null);
                        }
                        if (prop.PropertyType.Equals(typeof(string)))
                        {
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.String), null);
                        }
                        if (prop.PropertyType.Equals(typeof(int)))
                        {
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                    }
                    Response.Add((T)exFormAsObj);
                }
                Data.Close();
            }
            return Response;
        }
        private bool ActionsObject(DbManagerTypes dbManagerTypes)
        {
            List<ProcedureModel> procedureModels = new List<ProcedureModel>();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (prop.PropertyType.Equals(typeof(int)) || prop.PropertyType.Equals(typeof(string)) || prop.PropertyType.Equals(typeof(double)) || prop.PropertyType.Equals(typeof(TimeSpan)) || prop.PropertyType.Equals(typeof(DateTime)))
                {
                    PropertyInfo propertyInfo = Element.GetType().GetProperty(prop.Name);
                    procedureModels.Add(new ProcedureModel { Namefield = prop.Name, value = propertyInfo.GetValue(Element) });
                }
            }
            procedureModels.Add(new ProcedureModel { Namefield = "ModeProcedure", value = dbManagerTypes });
            dBConnection.StartProcedure(string.Format("Gps_{0}", Nametable), procedureModels);
            if (dBConnection.ErrorCode == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private TableDB GetClassAttribute()
        {
            TableDB tableDefinifiton = (TableDB)Attribute.GetCustomAttribute(typeof(T), typeof(TableDB));

            if (tableDefinifiton == null)
            {
                throw new Exceptions.GpExceptions(string.Format("The attribute was not found in the class '{0}'.", Nametable));
            }
            return tableDefinifiton;
        }

    }

    public enum DbManagerTypes
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }
}