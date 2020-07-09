using Configurables.Configurador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Configurables.Render
{
    public class Maker
    {
        private ConfigurableConf Configuracion;
        private ConfigurationUser Render;
        private RenderData renderData;

        public Maker(ConfigurableConf Configuracion)
        {
            this.Configuracion = Configuracion;
            Render = new ConfigurationUser();
            renderData = RenderData.Start;
            FillRender();
        }

        public Maker(ConfigurableConf Configuracion, ConfigurationUser Render)
        {
            this.Configuracion = Configuracion;
            this.Render = Render;
            renderData = RenderData.Alter;
            ValidateEntersModificating();
            GenerarteCOde();
        }

        public void FillRender()
        {
            Render.Blocks = new List<BloquesForm>();
            Configuracion.Blocks.ForEach(conf_elemento => {
                BloquesForm user_elemento = new BloquesForm();
                user_elemento.BlockKey = conf_elemento.Key;
                user_elemento.BlockName = conf_elemento.Block;
                user_elemento.IsModificating = false;
                user_elemento.IsOpenUser = conf_elemento.IsOpenUser;
                user_elemento.KeySelected = "";
                user_elemento.KeySelectedUser = "";
                user_elemento.FormOption = new List<BloqueFormOptions>();
                conf_elemento.Options.ForEach(conf_opction => {
                    user_elemento.FormOption.Add(new BloqueFormOptions { Active = true, Key = conf_opction.Key, Option = conf_opction.Option });
                });
                //user_elemento = null;
                Render.Blocks.Add(user_elemento);
            });
            ValidateCode();
            ValidateEnters();
            GenerarteCOde();
        }
        private void ValidateEnters()
        {
            Render.Blocks.ForEach(user_elemento => {
                //extraer informacion de configuracion
                ElementCode conf_elemento = Configuracion.Blocks.Find(el => el.Key == user_elemento.BlockKey);

                if (conf_elemento == null)
                    throw new Exception(string.Format("No existe una configuración para el campo: {0}", user_elemento.BlockKey));

                if (conf_elemento.IsFixed && !conf_elemento.IsOpenUser)
                {
                    List<string> Values = new List<string>();
                    Values.Add(conf_elemento.FixedValue);
                    user_elemento.FormOption.ForEach(user_opcion => {
                        if (Values.Contains(user_opcion.Key))
                        {
                            user_opcion.Active = true;
                        }
                        else
                        {
                            user_opcion.Active = false;
                        }
                    });
                }
                //extraer restricciones de campo de usario
                if (!conf_elemento.IsFixed && conf_elemento.IsOpenUser)
                {
                    //extraer restricciones a campo(reglas)
                    RestriccionCampoUsuario conf_restriccionUsuario = Configuracion.FieldsFree.Find(FreeUser => FreeUser.BlockKey == conf_elemento.Key);
                    //procesar elemento
                    ProcessFieldUser(user_elemento,conf_restriccionUsuario);

                }
                if (!conf_elemento.IsFixed && !conf_elemento.IsOpenUser)
                {
                    //extraer restricciones
                    RestriccionElemento conf_restriccion = Configuracion.Rectrictions.Find(rest => rest.Block == user_elemento.BlockKey);
                    ProccesElementsOptional(user_elemento,conf_restriccion);
                }
            });
        }

        private void ValidateEntersModificating()
        {
            Render.Blocks.Where(user_Render => user_Render.IsModificating).ToList().ForEach(user_elemento => {
                //extraer informacion de configuracion
                ElementCode conf_elemento = Configuracion.Blocks.Find(el => el.Key == user_elemento.BlockKey);

                if (conf_elemento == null)
                    throw new Exception(string.Format("No existe una configuración para el campo: {0}", user_elemento.BlockKey));

                if (conf_elemento.IsFixed && !conf_elemento.IsOpenUser)
                {
                    List<string> Values = new List<string>();
                    Values.Add(conf_elemento.FixedValue);
                    user_elemento.FormOption.ForEach(user_opcion => {
                        if (Values.Contains(user_opcion.Key))
                        {
                            user_opcion.Active = true;
                        }
                        else
                        {
                            user_opcion.Active = false;
                        }
                    });
                }
                //extraer restricciones de campo de usario
                if (!conf_elemento.IsFixed && conf_elemento.IsOpenUser)
                {
                    //extraer restricciones a campo(reglas)
                    RestriccionCampoUsuario conf_restriccionUsuario = Configuracion.FieldsFree.Find(FreeUser => FreeUser.BlockKey == conf_elemento.Key);
                    //procesar elemento
                    ProcessFieldUser(user_elemento, conf_restriccionUsuario);

                }
                if (!conf_elemento.IsFixed && !conf_elemento.IsOpenUser)
                {
                    //extraer restricciones
                    RestriccionElemento conf_restriccion = Configuracion.Rectrictions.Find(rest => rest.Block == user_elemento.BlockKey);
                    ProccesElementsOptional(user_elemento, conf_restriccion);
                }
            });
        }

        private void ProccesElementsOptional(BloquesForm user_elemento,RestriccionElemento conf_restriccion)
        {
            if(conf_restriccion != null)
            {
                conf_restriccion.Rules.ForEach(conf_regla => {
                    if (conf_regla.BlockValues.Contains(user_elemento.KeySelected))
                    {
                        List<string> valuesAcepted = new List<string>();

                        BloquesForm user_elementoRest = Render.Blocks.Find(Elemt => Elemt.BlockKey == conf_regla.BlockApply);
                        if (user_elementoRest == null)
                            throw new Exception(string.Format("El elemento {0} no fue encontrado", conf_regla.BlockApply.Trim()));

                        if (conf_regla.Type.Trim() == "HabilitarElementos")
                        {
                            ActiveElements(user_elementoRest, conf_regla.ValuesAcepted);
                        }
                        else if (conf_regla.Type.Trim() == "RemoveElements")
                        {
                            InActiveElements(user_elementoRest, conf_regla.ValuesAcepted);
                        }
                        else if (conf_regla.Type.Trim() == "Active_Acepted_Inactive_NoAcepted")
                        {
                            ActiveElementsDouble(user_elementoRest, conf_regla.ValuesAcepted);
                        }
                        else if (conf_regla.Type.Trim() == "Active_NoAcepted_Inactive_Acepted")
                        {
                            InActiveElementsDouble(user_elementoRest, conf_regla.ValuesAcepted);
                        }
                        else
                        {
                            throw new Exception(string.Format("restricion {0} no valida", conf_regla.Type.Trim()));
                        }
                    }
                });
            }
            else
            {
                //user_elemento.FormOption.ForEach(opciones => opciones.Active = true);
            }
            
        }

        private void ProcessFieldUser(BloquesForm user_elemento, RestriccionCampoUsuario conf_restriccionUsuario)
        {
            //verificar que el campo tenga una restriccion de usuario
            if (conf_restriccionUsuario == null)
                throw new Exception(string.Format("No existe una configuración para e campo de usario: {0}", user_elemento.BlockKey));

            //generar restriccion
            if(conf_restriccionUsuario.Type.Trim() == "number")
            {
                //user_elemento.KeySelected = user_elemento.KeySelectedUser;

                if(renderData == RenderData.Start)
                {
                    double realValue = (double)(double.Parse(user_elemento.KeySelected) / (double)conf_restriccionUsuario.NumeroMult);
                    if (conf_restriccionUsuario.RangeFrom <= double.Parse(user_elemento.KeySelected) && double.Parse(user_elemento.KeySelected) <= conf_restriccionUsuario.RangeTo)
                    {
                        user_elemento.KeySelectedUser = realValue + "";
                    }
                    else
                    {
                        throw new Exception(string.Format("El valor {0} no cumple el rango {1}-{2} del campo: {3}", realValue, conf_restriccionUsuario.RangeFrom, conf_restriccionUsuario.RangeTo, user_elemento.BlockKey));
                    }
                }

                if (renderData == RenderData.Alter)
                {
                    decimal valor = 0;
                    if (!decimal.TryParse(user_elemento.KeySelectedUser, out valor))
                    {
                        user_elemento.KeySelectedUser = "1";
                    }
                    else
                    {
                        if (valor == 0)
                        {
                            user_elemento.KeySelectedUser = "1";
                        }
                    }
                    if (conf_restriccionUsuario.IsRange)
                    {
                        if (conf_restriccionUsuario.HasCerosMask)
                        {
                            if (conf_restriccionUsuario.RangeFrom <= double.Parse(user_elemento.KeySelectedUser) && double.Parse(user_elemento.KeySelectedUser) <= conf_restriccionUsuario.RangeTo)
                            {
                                if (conf_restriccionUsuario.HasCerosMask)
                                {
                                    string test = (int)(double.Parse(user_elemento.KeySelectedUser) * (double)conf_restriccionUsuario.NumeroMult) + "";
                                    test = test.PadLeft(conf_restriccionUsuario.NumberCeros, '0');
                                    user_elemento.KeySelected = test;
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format("El valor {0} no cumple el rango {1}-{2}", user_elemento.KeySelected, conf_restriccionUsuario.RangeFrom, conf_restriccionUsuario.RangeTo));
                            }
                        }
                    }
                }
            }
        }
        private void ActiveElements(BloquesForm user_elemento, List<string> valuesAcepted)
        {
            user_elemento.FormOption.ForEach(user_opcion => {
                if (valuesAcepted.Contains(user_opcion.Key))
                {
                    user_opcion.Active = true;
                }
            });
        }
        private void InActiveElements(BloquesForm user_elemento, List<string> valuesAcepted)
        {
            user_elemento.FormOption.ForEach(user_opcion => {
                if (valuesAcepted.Contains(user_opcion.Key))
                {
                    user_opcion.Active = false;
                }
            });
        }
        private void ActiveElementsDouble(BloquesForm user_elemento, List<string> valuesAcepted)
        {
            user_elemento.FormOption.ForEach(user_opcion => {
                if (valuesAcepted.Contains(user_opcion.Key))
                {
                    user_opcion.Active = true;
                }
                else
                {
                    user_opcion.Active = false;
                }
            });
        }
        private void InActiveElementsDouble(BloquesForm user_elemento, List<string> valuesAcepted)
        {
            user_elemento.FormOption.ForEach(user_opcion => {
                if (valuesAcepted.Contains(user_opcion.Key))
                {
                    user_opcion.Active = false;
                }
                else
                {
                    user_opcion.Active = true;
                }
            });
        }
        public ConfigurationUser GetConfigurationUser()
        {
            GetDescription();
            return Render;
        }
        public void GetDescription()
        {
            Render.Description = new List<DescriptionElement>();
            Render.Blocks.ForEach(element => {
                element.IsModificating = false;
                DescriptionElement descriptionElement = new DescriptionElement();
                descriptionElement.Bloque = element.BlockName;
                if (!element.IsOpenUser)
                {
                    descriptionElement.Selected = element.FormOption.Find(ab => ab.Key == element.KeySelected).Option;
                }
                else
                {
                    if (Configuracion.FieldsFree.Where(ab => ab.BlockKey == element.BlockKey).ToList().Count == 1)
                    {
                        descriptionElement.Selected = element.KeySelectedUser + " " + Configuracion.FieldsFree.Where(ab => ab.BlockKey == element.BlockKey).ToList().ElementAt(0).UnitMesureUser;
                    }
                    else
                    {
                        descriptionElement.Selected = element.KeySelectedUser;
                    }

                }
                Render.Description.Add(descriptionElement);
            });
        }
        private void GenerarteCOde()
        {
            Render.ItemCode = "";
            Render.Blocks.ForEach(ab =>
            {
                if(ab.FormOption.Where(val => val.Key == ab.KeySelected && val.Active).ToList().Count == 0 && !ab.IsOpenUser)
                {   
                    if(ab.FormOption.Where(val => val.Active).ToList().Count > 0)
                    {
                        ab.KeySelected = ab.FormOption.Where(val => val.Active).ToList().ElementAt(0).Key;
                    }
                    else
                    {
                        throw new Exception("No existe un valor activo para asignacion default en codigo: " + ab.BlockName);
                    }
                   
                }
            });
            Render.Blocks.ForEach(ab =>
            {
                Render.ItemCode += ab.KeySelected;
            });
        }
        private void ValidateCode()
        {
            if (string.IsNullOrEmpty(Render.ItemCode))
            {
                Render.ItemCode = Configuracion.ItemCodeexample;
            }
            Regex Expresion = new Regex(Configuracion.Expresion);
            Match mc = Expresion.Match(Render.ItemCode);
            if (mc.Success)
            {
                MatchCollection mcs = Expresion.Matches(Render.ItemCode);
                foreach (Match m in mcs)
                {
                    foreach (string group in Expresion.GetGroupNames())
                    {
                        if (group != "0")
                        {
                            Render.Blocks.Find(ab => ab.BlockKey == group).KeySelected = m.Groups[group].Value.Trim();
                        }
                    }

                }
            }
            else
            {
                throw new Exception("No es valido el codigo: " + Render.ItemCode);
            }
        }
    }

    public enum RenderData
    {
        Start = 1,
        Alter = 2
    }
}
