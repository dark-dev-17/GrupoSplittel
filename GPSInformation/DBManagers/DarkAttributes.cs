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
                Nombre = typeof(T).Name;
            }
            return Nombre;
        }

        public bool Add()
        {
            TableDB tableDefinifiton = GetClassAttribute();
            if (tableDefinifiton.IsStoreProcedure)
            {
                return ActionsObject(DbManagerTypes.Add);
            }
            else
            {
                return ActionsObjectCode(DbManagerTypes.Add, tableDefinifiton);
            }
            
        }

        public bool Update()
        {
            TableDB tableDefinifiton = GetClassAttribute();
            if (tableDefinifiton.IsStoreProcedure)
            {
                return ActionsObject(DbManagerTypes.Update);
            }
            else
            {
                return ActionsObjectCode(DbManagerTypes.Update, tableDefinifiton);
            }
        }

        public bool Delete()
        {
            TableDB tableDefinifiton = GetClassAttribute();
            if (tableDefinifiton.IsStoreProcedure)
            {
                return ActionsObject(DbManagerTypes.Delete);
            }
            else
            {
                return ActionsObjectCode(DbManagerTypes.Delete, tableDefinifiton);
            }
        }

        public int GetLastId()
        {
            return dBConnection.GetIntegerValue(string.Format("select max(Id{0}) from {0}", Nametable));
        }
        public int GetLastId(string nameCol, string Value)
        {
            return dBConnection.GetIntegerValue(string.Format("select max(Id{0}) from {0} where {1} = '{2}'", Nametable, nameCol, Value));
        }

        public T Get(int? id)
        {
            List<T> Lista = DataReader(string.Format("select * from {0} where {1} = '{2}'", Nametable, KeyCol(), id));
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
        public List<T> GetIn(int[] keys, string nameCol)
        {
            
            return DataReader(string.Format("select * from {0} where {1} in ({2})", Nametable, nameCol, string.Join(", ", keys)));
        }

        public List<T> GetIn(List<int> keys, string nameCol, List<int> keys2, string nameCol2)
        {

            return DataReader(string.Format("select * from {0} where {1} in ({2}) and {3} in ({4})", Nametable, nameCol, string.Join(", ", keys), nameCol2, string.Join(", ", keys2)));
        }
        public List<T> GetList(string Columna1, string Columna1Val, string Columna2, string Columna1Val2)
        {
            List<T> Lista = DataReader(string.Format("select * from {0} where {1} = '{2}' and  {3} = '{4}'", Nametable, Columna1, Columna1Val, Columna2, Columna1Val2));
            return Lista;
        }
        public T Get(string Columna1, string Columna1Val, string Columna2, string Columna1Val2)
        {
            List<T> Lista = DataReader(string.Format("select * from {0} where {1} = '{2}' and  {3} = '{4}'", Nametable, Columna1, Columna1Val, Columna2, Columna1Val2));
            if (Lista.Count == 0)
            {
                return default(T);
            }
            return Lista.ElementAt(0);
        }
        public List<T> Get()
        {
            return DataReader(string.Format("select * from {0}", Nametable));
        }
        public string ColumName(string Name)
        {
            string columna = "";
            if (GetClassAttribute().IsMappedByLabels)
            {
                object exFormAsObj = Activator.CreateInstance(typeof(T));
                foreach (var prop in typeof(T).GetProperties())
                {
                    PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                    ColumnDB hiddenAttribute = (ColumnDB)propertyInfo.GetCustomAttribute(typeof(ColumnDB));

                    if (prop.Name == Name)
                    {
                        columna = hiddenAttribute.Name;
                    }
                }
            }
            else
            {
                columna = Name;
            }

            return columna;
        }
        private string KeyCol()
        {
            string columna = "";
            if (GetClassAttribute().IsMappedByLabels)
            {
                object exFormAsObj = Activator.CreateInstance(typeof(T));
                foreach (var prop in typeof(T).GetProperties())
                {
                    PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                    ColumnDB hiddenAttribute = (ColumnDB)propertyInfo.GetCustomAttribute(typeof(ColumnDB));

                    if (hiddenAttribute.IsKey)
                    {
                        columna = hiddenAttribute.Name;
                    }
                }
            }
            else
            {
                object exFormAsObj = Activator.CreateInstance(typeof(T));
                foreach (var prop in typeof(T).GetProperties())
                {
                    PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                    ColumnDB hiddenAttribute = (ColumnDB)propertyInfo.GetCustomAttribute(typeof(ColumnDB));

                    if (hiddenAttribute.IsKey)
                    {
                        columna = prop.Name;
                    }
                }
            }

            return columna;
        }

        private List<T> DataReader(string SqlStatements)
        {
            TableDB tableDefinifiton = GetClassAttribute();

            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<T> Response = new List<T>();
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

                    if (hiddenAttribute.IsMapped)
                    {
                        string NombrePropiedad = "";
                        if (tableDefinifiton.IsMappedByLabels)
                        {
                            NombrePropiedad = hiddenAttribute.Name.Trim();
                        }
                        else
                        {
                            NombrePropiedad = prop.Name;
                        }
                        
                        if (prop.PropertyType.Equals(typeof(DateTime)))
                        {
                            var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? DateTime.Now : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.DateTime), null);
                        }
                        if (prop.PropertyType.Equals(typeof(DateTime?)))
                        {
                            var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? null : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                            propertyInfo.SetValue(exFormAsObj, value, null);
                        }
                        if (prop.PropertyType.Equals(typeof(TimeSpan)))
                        {
                            var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? null : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                            propertyInfo.SetValue(exFormAsObj, value, null);
                        }
                        if (prop.PropertyType.Equals(typeof(double)))
                        {
                            var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? 0 : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.Double), null);
                        }
                        if (prop.PropertyType.Equals(typeof(string)))
                        {
                            var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? "" : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.String), null);
                        }
                        if (prop.PropertyType.Equals(typeof(bool)))
                        {
                            var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? false : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.Boolean), null);
                        }
                        if (prop.PropertyType.Equals(typeof(int)))
                        {
                            var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? 0 : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                            propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                        
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
                if (prop.PropertyType.Equals(typeof(int)) || prop.PropertyType.Equals(typeof(string)) || prop.PropertyType.Equals(typeof(double)) || prop.PropertyType.Equals(typeof(TimeSpan)) || prop.PropertyType.Equals(typeof(DateTime)))
                {
                    PropertyInfo propertyInfo = Element.GetType().GetProperty(prop.Name);
                    procedureModels.Add(new ProcedureModel { Namefield = prop.Name, value = propertyInfo.GetValue(Element) });
                }
                if (prop.PropertyType.Equals(typeof(DateTime?)))
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

        private bool ActionsObjectCode(DbManagerTypes dbManagerTypes, TableDB tableDefinifiton)
        {
            //TableDB tableDefinifiton = GetClassAttribute();
            bool result = false;
            //mapeo de tabla con los nombres de los campos ya existentes
            string sentencia = "";
            string sentenciaVariables = "";
            foreach (var prop in typeof(T).GetProperties())
            {
                PropertyInfo propertyInfo = Element.GetType().GetProperty(prop.Name);
                ColumnDB hiddenAttribute = (ColumnDB)propertyInfo.GetCustomAttribute(typeof(ColumnDB));

                if (hiddenAttribute == null)
                {
                    throw new Exceptions.GpExceptions(string.Format("The attribute was not found in the attribute '{0}', if you don´t want to use mapTable, please set IsMappedByLabels = false", prop.Name));
                }
                if (tableDefinifiton.IsMappedByLabels)
                {
                    if (string.IsNullOrEmpty(hiddenAttribute.Name))
                    {
                        throw new Exceptions.GpExceptions(string.Format("The attribute {0} was setting like mapping column, the name is missing", prop.Name));
                    }
                }
                else
                {
                    hiddenAttribute.Name = prop.Name;
                }


                if (dbManagerTypes == DbManagerTypes.Add)
                {
                    if (!hiddenAttribute.IsKey && hiddenAttribute.IsMapped)
                    {
                        sentencia += hiddenAttribute.Name + ",";
                        sentenciaVariables += "@" + hiddenAttribute.Name + ",";
                    }
                }
                else if (dbManagerTypes == DbManagerTypes.Update)
                {
                    if (!hiddenAttribute.IsKey && hiddenAttribute.IsMapped)
                    {
                        sentencia += hiddenAttribute.Name + " = @" + hiddenAttribute.Name + ",";
                    }
                    else if(hiddenAttribute.IsKey && hiddenAttribute.IsMapped)
                    {
                        sentenciaVariables = hiddenAttribute.Name + " = @" + hiddenAttribute.Name + "";
                    }
                    else
                    {
                        
                    }
                }
                else if (dbManagerTypes == DbManagerTypes.Delete)
                {
                    if (hiddenAttribute.IsKey)
                    {
                        sentenciaVariables = hiddenAttribute.Name + " = @" + hiddenAttribute.Name + "";
                    }
                }
                else
                {
                    throw new Exceptions.GpExceptions(string.Format("Delete action is not active"));
                }
            }

            if (dbManagerTypes == DbManagerTypes.Add)
            {
                string Statement = string.Format("INSERT INTO {0}({1}) VALUES({2})", Nametable, sentencia.Substring(0, sentencia.Length - 1), sentenciaVariables.Substring(0, sentenciaVariables.Length - 1));
                List<ProcedureModel> procedureModels = new List<ProcedureModel>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    PropertyInfo propertyInfo = Element.GetType().GetProperty(prop.Name);
                    ColumnDB hiddenAttribute = (ColumnDB)propertyInfo.GetCustomAttribute(typeof(ColumnDB));

                    if (!hiddenAttribute.IsKey && hiddenAttribute.IsMapped)
                    {
                        procedureModels.Add(new ProcedureModel { Namefield = tableDefinifiton.IsMappedByLabels ? hiddenAttribute.Name : prop.Name, value = propertyInfo.GetValue(Element) });
                    }
                }

                dBConnection.StartInsert(Statement, procedureModels);
                result = true;
            }
            else if (dbManagerTypes == DbManagerTypes.Update)
            {
                string Statement = string.Format("UPDATE {0} SET {1} WHERE {2} ", Nametable, sentencia.Substring(0, sentencia.Length - 1), sentenciaVariables);
                List<ProcedureModel> procedureModels = new List<ProcedureModel>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    PropertyInfo propertyInfo = Element.GetType().GetProperty(prop.Name);
                    ColumnDB hiddenAttribute = (ColumnDB)propertyInfo.GetCustomAttribute(typeof(ColumnDB));

                    if (hiddenAttribute.IsMapped)
                    {
                        procedureModels.Add(new ProcedureModel { Namefield = tableDefinifiton.IsMappedByLabels ? hiddenAttribute.Name : prop.Name, value = propertyInfo.GetValue(Element) });
                    }
                }

                dBConnection.StartUpdate(Statement, procedureModels);
                result = true;
            }
            else if (dbManagerTypes == DbManagerTypes.Delete)
            {
                string Statement = string.Format("DELETE FROM  {0} WHERE {1} ", Nametable, sentenciaVariables);
                List<ProcedureModel> procedureModels = new List<ProcedureModel>();
                foreach (var prop in typeof(T).GetProperties())
                {
                    PropertyInfo propertyInfo = Element.GetType().GetProperty(prop.Name);
                    ColumnDB hiddenAttribute = (ColumnDB)propertyInfo.GetCustomAttribute(typeof(ColumnDB));

                    if (hiddenAttribute.IsMapped && hiddenAttribute.IsKey)
                    {
                        procedureModels.Add(new ProcedureModel { Namefield = tableDefinifiton.IsMappedByLabels ? hiddenAttribute.Name : prop.Name, value = propertyInfo.GetValue(Element) });
                    }
                }

                dBConnection.StartDelete(Statement, procedureModels);
                result = true;
            }
            else
            {
                throw new Exceptions.GpExceptions(string.Format("Delete action is not active"));
            }

            return result;
        }

        private TableDB GetClassAttribute()
        {
            TableDB tableDefinifiton = (TableDB)Attribute.GetCustomAttribute(typeof(T), typeof(TableDB));

            if (tableDefinifiton == null)
            {
                throw new Exceptions.GpExceptions(string.Format("The attribute was not found in the class '{0}'.", typeof(T).Name));
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