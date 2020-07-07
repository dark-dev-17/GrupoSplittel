using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Configurables.Configurador
{
    public class EditorConfigurable
    {
        public ConfigurableConf ConfigurableConf { get; private set; }
        public string Mesage { get; private set; }
        public EditorConfigurable(ConfigurableConf ConfigurableConf)
        {
            this.ConfigurableConf = ConfigurableConf;
        }



        #region Campos de usuario reglas
        public void UpdatedRestriccionFree(RestriccionCampoUsuario restriccionCampoUsuario, int IndexRestriccionCampoUsuario)
        {
            if (restriccionCampoUsuario == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio restriccionCampoUsuario");
            }

            RestriccionCampoUsuario RestriccionCampo = ConfigurableConf.FieldsFree.ElementAt(IndexRestriccionCampoUsuario);
            RestriccionCampo.BlockKey = restriccionCampoUsuario.BlockKey;
            RestriccionCampo.HasCerosMask = restriccionCampoUsuario.HasCerosMask;
            RestriccionCampo.IsRange = restriccionCampoUsuario.IsRange;
            RestriccionCampo.NumberCeros = restriccionCampoUsuario.NumberCeros;
            RestriccionCampo.NumeroMult = restriccionCampoUsuario.NumeroMult;
            RestriccionCampo.RangeFrom = restriccionCampoUsuario.RangeFrom;
            RestriccionCampo.RangeTo = restriccionCampoUsuario.RangeTo;
            RestriccionCampo.Type = restriccionCampoUsuario.Type;
            RestriccionCampo.UnitMesureUser = restriccionCampoUsuario.UnitMesureUser;

        }
        public void AddRestriccionFree(RestriccionCampoUsuario restriccionCampoUsuario)
        {
            if (restriccionCampoUsuario == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio restriccionCampoUsuario");
            }
            if(!ConfigurableConf.Blocks.Exists(free => free.IsOpenUser && free.Key == restriccionCampoUsuario.BlockKey))
            {
                throw new Exception("El elemento " + restriccionCampoUsuario.BlockKey + " no es un elemento abierto al usuario" );
            }
            if (!ConfigurableConf.FieldsFree.Exists(Free => Free.BlockKey == restriccionCampoUsuario.BlockKey))
            {
                ConfigurableConf.FieldsFree.Add(restriccionCampoUsuario);
            }
            else
            {
                throw new Exception("Ya existe una restriccion para el elemento: " + restriccionCampoUsuario.BlockKey);
            }
        }
        public void DeletedRestriccionFree(RestriccionCampoUsuario restriccionCampoUsuario)
        {
            if (restriccionCampoUsuario == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio restriccionCampoUsuario");
            }
            if(ConfigurableConf.FieldsFree.Exists(Free => Free.BlockKey == restriccionCampoUsuario.BlockKey && Free.Type == restriccionCampoUsuario.Type))
            {
                int Index = ConfigurableConf.FieldsFree.FindIndex(Free => Free.BlockKey == restriccionCampoUsuario.BlockKey && Free.Type == restriccionCampoUsuario.Type);
                ConfigurableConf.FieldsFree.RemoveAt(Index);
            }
            else
            {
                throw new Exception("No fue eliminado la restriccion de datos");
            }
        }
        #endregion

        #region Reglas de restricciones
        public void UpdateRegla(int IndexRelga, int IndexRestriccion, Regla regla)
        {
            if (ConfigurableConf.Rectrictions.Count - 1 < IndexRestriccion)
            {
                throw new Exception("el IndexRestriccion a modifcar opciones esta fuera de rango");
            }
            if (ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Rules.Count - 1 < IndexRelga)
            {
                throw new Exception("el IndexRelga a modifcar opciones esta fuera de rango");
            }
            if (regla == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio regla");
            }
            regla.BlockValues.ForEach(valor => {
                if(ConfigurableConf.Blocks.Where(bl => bl.Key == ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Block).ToList().ElementAt(0).Options.Where(op => op.Key == valor).ToList().Count == 0)
                {
                    throw new Exception(string.Format(" BlockValues - El valor {0} no se encuentra en el bloque {1}", valor, ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Block));
                }
            });
            regla.ValuesAcepted.ForEach(valor => {
                if (ConfigurableConf.Blocks.Where(bl => bl.Key == regla.BlockApply).ToList().ElementAt(0).Options.Where(op => op.Key == valor).ToList().Count == 0)
                {
                    throw new Exception(string.Format("ValuesAcepted - El valor {0} no se encuentra en el bloque {1}", valor, regla.BlockApply));
                }
            });
            int Index = 0;

            ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Rules.ForEach(rul => {
                if(Index == IndexRelga)
                {
                    rul.BlockApply = regla.BlockApply;
                    rul.Type = regla.Type;
                    rul.BlockValues = regla.BlockValues;
                    rul.ValuesAcepted = regla.ValuesAcepted;
                }
                Index++;
            });
        }
        public void AddRegla(int IndexRestriccion, Regla regla)
        {
            if (regla == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio regla");
            }
            if (ConfigurableConf.Rectrictions.Count - 1 < IndexRestriccion)
            {
                throw new Exception("el IndexRestriccion a modifcar opciones esta fuera de rango");
            }
            regla.BlockValues.ForEach(valor => {
                if (ConfigurableConf.Blocks.Where(bl => bl.Key == ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Block).ToList().ElementAt(0).Options.Where(op => op.Key == valor).ToList().Count == 0)
                {
                    throw new Exception(string.Format(" BlockValues - El valor {0} no se encuentra en el bloque {1}", valor, ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Block));
                }
            });
            regla.ValuesAcepted.ForEach(valor => {
                if (ConfigurableConf.Blocks.Where(bl => bl.Key == regla.BlockApply).ToList().ElementAt(0).Options.Where(op => op.Key == valor).ToList().Count == 0)
                {
                    throw new Exception(string.Format("ValuesAcepted - El valor {0} no se encuentra en el bloque {1}", valor, regla.BlockApply));
                }
            });
            if (ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Rules.Where(rul => rul.BlockApply == regla.BlockApply && rul.BlockValues == regla.BlockValues).ToList().Count == 0)
            {
                ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Rules.Add(regla);
            }
            else
            {
                throw new Exception("Ya existe una regla para el bloque " + regla.BlockApply);
            }
        }
        public void DeleteRegla(Regla regla, int IndexRestriccion)
        {
            if (ConfigurableConf.Rectrictions.Count - 1 < IndexRestriccion)
            {
                throw new Exception("el IndexRestriccion a modifcar opciones esta fuera de rango");
            }
            if (ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Rules.Exists(rul => rul.BlockApply == regla.BlockApply && rul.Type == regla.Type) )
            {
                int IndexRelga = ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Rules.FindIndex(rul => rul.BlockApply == regla.BlockApply && rul.Type == regla.Type);
                ConfigurableConf.Rectrictions.ElementAt(IndexRestriccion).Rules.RemoveAt(IndexRelga);
            }
            else
            {
                throw new Exception("No se encontro la restriccion");
            }
        }
        #endregion

        #region Restricciones a campos opcionales
        public bool UpdateRestrcition(int IndexRestriccion, RestriccionElemento restriccionElemento)
        {
            if (restriccionElemento == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio RestriccionElemento");
            }
            if (ConfigurableConf.Rectrictions.Count - 1 < IndexRestriccion)
            {
                throw new Exception("el IndexRestriccion a modifcar opciones esta fuera de rango");
            }
            if (!CheckElement(restriccionElemento.Block))
            {
                throw new Exception(string.Format("El bloque {0} no fue encontrado", restriccionElemento.Block));
            }
            if (ConfigurableConf.Blocks.Where(blo => blo.IsFixed && blo.Key == restriccionElemento.Block || blo.IsOpenUser && blo.Key == restriccionElemento.Block).ToList().Count == 1)
            {
                throw new Exception("N se pueden asignar reglas a elementos abiertos al usuaro o con valor fijo");
            }
            int Index = 0;
            ConfigurableConf.Rectrictions.ForEach(rest => {
                if (Index == IndexRestriccion)
                {
                    rest.Restriccion = restriccionElemento.Restriccion;
                    rest.Block = restriccionElemento.Block;
                }
                Index++;
            });
            return true;
        }
        public bool DeleteRestrcition(RestriccionElemento restriccionElemento)
        {
            if (restriccionElemento == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio RestriccionElemento");
            }
            if(ConfigurableConf.Rectrictions.Exists(res => res.Block == restriccionElemento.Block))
            {
                int index = ConfigurableConf.Rectrictions.FindIndex(res => res.Block == restriccionElemento.Block);
                ConfigurableConf.Rectrictions.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
            
            
        }
        public bool AddRestrcition(RestriccionElemento restriccionElemento)
        {
            if(restriccionElemento == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio RestriccionElemento");
            }
            if (!CheckElement(restriccionElemento.Block))
            {
                throw new Exception(string.Format("El bloque {0} no fue encontrado", restriccionElemento.Block));
            }
            if(ConfigurableConf.Blocks.Where(blo => blo.IsFixed &&  blo.Key == restriccionElemento.Block || blo.IsOpenUser && blo.Key == restriccionElemento.Block).ToList().Count == 1)
            {
                throw new Exception("N se pueden asignar reglas a elementos abiertos al usuaro o con valor fijo");
            }
            if (ConfigurableConf.Rectrictions.Where(rest => rest.Block == restriccionElemento.Block).ToList().Count == 0)
            {
                ConfigurableConf.Rectrictions.Add(restriccionElemento);
                return true;
            }
            else
            {
                throw new Exception("Ya existe una restriccion para el bloque " + restriccionElemento.Block);
            }
        }
        #endregion

        #region Valores de elementos de Codigo opcionales
        public bool UpdateValuePartCodigo(int IndexValue, int IndexPartCodigo, OpcionesSelect opcionesSelect)
        {
            if (ConfigurableConf.Blocks.Count - 1 < IndexPartCodigo)
            {
                throw new Exception("el elemento a modifcar opciones esta fuera de rango");
            }
            if (opcionesSelect == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio");
            }

            ElementCode elementCode = ConfigurableConf.Blocks.ElementAt(IndexPartCodigo);
            if (elementCode.Options.Count - 1 < IndexValue)
            {
                throw new Exception("el valor a modifcar opciones esta fuera de rango");
            }
            if (elementCode.Options.ElementAt(IndexValue) != null)
            {
                /// verificar que no se repita en otros nodos
                int Index = 0;
                elementCode.Options.ForEach(op => {
                    if(Index == IndexValue)
                    {
                        op.Key = opcionesSelect.Key;
                        op.Option = opcionesSelect.Option;
                    }
                    Index++;
                });
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public bool DeleteValuePartCodigo(OpcionesSelect opcionesSelect, int IndexPartCodigo)
        {

            if (ConfigurableConf.Blocks.Count - 1 < IndexPartCodigo)
            {
                throw new Exception("el elemento a modifcar opciones esta fuera de rango");
            }
            ElementCode elementCode = ConfigurableConf.Blocks.ElementAt(IndexPartCodigo);

            if (elementCode.Options.Exists(val => val.Key == opcionesSelect.Key))
            {
                int index = elementCode.Options.FindIndex(val => val.Key == opcionesSelect.Key);
                elementCode.Options.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddValuePartCodigo(int IndexPartCodigo, OpcionesSelect opcionesSelect)
        {
            if (ConfigurableConf.Blocks.Count - 1 < IndexPartCodigo)
            {
                throw new Exception("el elemento a modifcar opciones esta fuera de rango");
            }
            if (opcionesSelect == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio");
            }

            ElementCode elementCode = ConfigurableConf.Blocks.ElementAt(IndexPartCodigo);
            
            if (elementCode.Options.Where(op => op.Key == opcionesSelect.Key).ToList().Count == 0)
            {
                elementCode.Options.Add(opcionesSelect);
                return true;
            }
            else
            {
                Mesage = string.Format("Ya se encuentra una opcion registrada con la misma clave: {0}", opcionesSelect.Key);
                return false;
            }
        }
        
        #endregion

        #region Elementos del codigo
        public bool AddElementoCodigo(ElementCode elementCode)
        {
            if(elementCode == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio");
            }
            if (elementCode.IsFixed && string.IsNullOrEmpty(elementCode.FixedValue))
            {
                throw new Exception("Por favor introduce el valor fijo");
            }
            if (ConfigurableConf.Blocks.Where(elem => elem.Key == elementCode.Key).ToList().Count == 0)
            {
                ConfigurableConf.Blocks.Add(elementCode);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteElementoCodigo(ElementCode elementCode)
        {
            if(elementCode == null)
            {
                throw new Exception("Introduce el nodo a eliminar");
            }
            if(ConfigurableConf.Blocks.Exists(element => element.Key == elementCode.Key) )
            {
                int index = ConfigurableConf.Blocks.FindIndex(element => element.Key == elementCode.Key);
                ConfigurableConf.Blocks.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateElementoCodigo(ElementCode elementCode,int element)
        {
            if (elementCode == null)
            {
                throw new Exception("Por favor introduce un elemento valido, campo vacio");
            }
            if (ConfigurableConf.Blocks.Count - 1 < element)
            {
                throw new Exception("el elemento a actualizar esta fuera del rango de lista");
            }
            if (elementCode.IsFixed && string.IsNullOrEmpty(elementCode.FixedValue))
            {
                throw new Exception("Por favor introduce el valor fijo");
            }
            if (ConfigurableConf.Blocks.ElementAt(element) != null)
            {
                /// verificar que no se repita en otros nodos
                int index = 0;
                ConfigurableConf.Blocks.ForEach(elem => { 

                    if(index == element)
                    {
                        elem.Block = elementCode.Block;
                        elem.Key = elementCode.Key;
                        elem.IsFixed = elementCode.IsFixed;
                        elem.IsOptional = elementCode.IsOptional;
                        elem.IsOpenUser = elementCode.IsOpenUser;
                        elem.Type = elementCode.Type;
                        elem.FixedValue = elementCode.FixedValue;
                        elem.Options = elementCode.Options;
                    }
                    index++;
                });

                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        private bool CheckElement(string key)
        {
            if(ConfigurableConf.Blocks.Where(bloq => bloq.Key == key).ToList().Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void validateCodeExample()
        {
            if (string.IsNullOrEmpty(ConfigurableConf.ItemCodeexample.Trim()))
            {
                throw new Exception("codigo vacio");
            }
            Regex Expresion = new Regex(ConfigurableConf.Expresion);
            Match mc = Expresion.Match(ConfigurableConf.ItemCodeexample.Trim());
            ConfigurableConf.ItemCode = ConfigurableConf.ItemCodeexample.Trim();
            if (!mc.Success)
            {
                throw new Exception("No es valido el codigo: " + ConfigurableConf.ItemCodeexample.Trim());
            }
        }
    }
}
