using Configurables.Configurador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Configurables.Render
{
    public class ProcesatorConfig
    {
        private ConfigurableConf ConfiguracionOp;
        private ConfigurationUser ConfigurationUser;
        private ProcesatorConfigMode procesatorConfigMode;

        public ProcesatorConfig(ProcesatorConfigMode procesatorConfigMode, ConfigurableConf ConfiguracionOp)
        {
            this.procesatorConfigMode = procesatorConfigMode;
            this.ConfiguracionOp = ConfiguracionOp;
            ConfigurationUser = new ConfigurationUser();
            FillBloquesForm();
        }
        public ProcesatorConfig(ConfigurableConf ConfiguracionOp)
        {
            this.ConfiguracionOp = ConfiguracionOp;
        }
        public ProcesatorConfig(ConfigurationUser configurationUser, ConfigurableConf ConfiguracionOp)
        {
            this.ConfigurationUser = configurationUser;
            this.ConfiguracionOp = ConfiguracionOp;
        }
        #region Elementos conf tecnica
        public void AddValueElement(OpcionesSelect Optionenew, int Index)
        {
            ElementCode blocke = ConfiguracionOp.Blocks.ElementAt(Index);
            if (blocke.Options.Where(op => op.Key == Optionenew.Key).ToList().Count == 0)
            {
                blocke.Options.Add(Optionenew);
            }
            else
            {
                throw new Exception(string.Format("Ya existe un elemento con la misma clave: {0}", Optionenew.Key));
            }
        }
        public void DeleteValueElement(int IndexSelected, int Index)
        {
            ElementCode blocke = ConfiguracionOp.Blocks.ElementAt(Index);
            if (blocke.Options.ElementAt(IndexSelected) != null)
            {
                blocke.Options.RemoveAt(IndexSelected);
            }
            else
            {
                throw new Exception(string.Format("No se encontro el valor"));
            }
        }
        public void UpdateValueElement(int IndexSelected, int Index, OpcionesSelect Optionenew)
        {
            ElementCode blocke = ConfiguracionOp.Blocks.ElementAt(Index);
            if (blocke.Options.ElementAt(IndexSelected) != null)
            {
                OpcionesSelect optione = blocke.Options.ElementAt(IndexSelected);
                optione = Optionenew;
            }
            else
            {
                throw new Exception(string.Format("No se encontro el valor"));
            }
        }
        public void AddRestriction(ElementCode bloqueNew)
        {
            if (ConfiguracionOp.Blocks.Where(bloque => bloque.Key == bloqueNew.Key).ToList().Count == 0)
            {
                ConfiguracionOp.Blocks.Add(bloqueNew);
            }
            else
            {
                throw new Exception(string.Format("Ya existe un elemento con la misma clave: {0}", bloqueNew.Key));
            }
        }
        public void DeleteRestriction(ElementCode bloqueNew)
        {
            int index = 0;
            int indexDelete = 0;
            bool found = false;
            if (ConfiguracionOp.Blocks.Where(bloque => bloque.Key == bloqueNew.Key).ToList().Count == 1)
            {
                ConfiguracionOp.Blocks.ForEach(bk => {
                    if (bk.Key == bloqueNew.Key)
                    {
                        found = true;
                        indexDelete = index;
                    }
                    index++;
                });

            }
            else
            {
                throw new Exception(string.Format("no existe un elemento con la clave: {0}", bloqueNew.Key));
            }
            if (found)
            {
                ConfiguracionOp.Blocks.RemoveAt(indexDelete);
            }
        }
        #endregion

        #region Version1 
        public void ApplyRuleForElement()
        {
            //obtiene elementos modificados
            BloquesForm bloquesFormSelected = ConfigurationUser.Blocks.Where(bloque => bloque.IsModificating == true).ToList().ElementAt(0);
            if (!bloquesFormSelected.IsOpenUser)
            {
                // obtiene reglas que apliquen a bloque accion from
                List<RestriccionElemento> restrictions = ConfiguracionOp.Rectrictions.Where(rest => rest.Block == bloquesFormSelected.BlockKey).ToList();
                //obtiene restricciones indirectas
                List<RestriccionElemento> restrictionsIndirect = ConfiguracionOp.Rectrictions.Where(rest => rest.Rules.Where(rul => rul.BlockApply == bloquesFormSelected.BlockKey).ToList().Count > 0).ToList();

                if (restrictions.Count > 0)
                {
                    //recorre cada restriccion
                    restrictions.ForEach(rest =>
                    {
                        //recorre cada regla
                        rest.Rules.ForEach(rul =>
                        {
                            //verifica que el valor seleccionando este dentro del los valores detonantes de la regla
                            if (rul.BlockValues.Contains(bloquesFormSelected.KeySelected))
                            {
                                //verifica que sea accion de eliminar elementos no aplicados
                                if (rul.Type == "HabilitarElementos")
                                {
                                    //obtiene el elemento a restringir opciones
                                    BloquesForm bloquesFormApplyRule = ConfigurationUser.Blocks.Where(bloque => bloque.BlockKey == rul.BlockApply).ToList().ElementAt(0);
                                    //recorre cada valor del elemento a restringir elelemtno to
                                    bloquesFormApplyRule.FormOption.ForEach(elemt =>
                                    {
                                        //verifica que el valor se encuentre en la lista de valores aceptados
                                        if (rul.ValuesAcepted.Contains(elemt.Key))
                                        {
                                            elemt.Active = true;
                                        }
                                        else
                                        {
                                            elemt.Active = false;
                                        }
                                    });
                                    //verificar que el valor seleccionado este dentro de los permitidos,  sino existe asigna el primer valor aceptado despues de la restriccion
                                    if (bloquesFormApplyRule.FormOption.Where(bfpr => bfpr.Key == bloquesFormApplyRule.KeySelected && bfpr.Active).ToList().Count == 0)
                                    {
                                        bloquesFormApplyRule.KeySelected = bloquesFormApplyRule.FormOption.Where(bfpr => bfpr.Active).ToList().ElementAt(0).Key;
                                    }
                                }
                                if (rul.Type == "RemoveElements")
                                {
                                    //obtiene el elemento a restringir opciones
                                    BloquesForm bloquesFormApplyRule = ConfigurationUser.Blocks.Where(bloque => bloque.BlockKey == rul.BlockApply).ToList().ElementAt(0);
                                    //recorre cada valor del elemento a restringir elelemtno to
                                    bloquesFormApplyRule.FormOption.ForEach(elemt =>
                                    {
                                        //verifica que el valor se encuentre en la lista de valores aceptados
                                        if (rul.ValuesAcepted.Contains(elemt.Key))
                                        {
                                            elemt.Active = false;
                                        }
                                        else
                                        {
                                            elemt.Active = true;
                                        }
                                    });
                                    //verificar que el valor seleccionado este dentro de los permitidos,  sino existe asigna el primer valor aceptado despues de la restriccion
                                    if (bloquesFormApplyRule.FormOption.Where(bfpr => bfpr.Key == bloquesFormApplyRule.KeySelected && bfpr.Active).ToList().Count == 0)
                                    {
                                        bloquesFormApplyRule.KeySelected = bloquesFormApplyRule.FormOption.Where(bfpr => bfpr.Active).ToList().ElementAt(0).Key;
                                    }
                                }
                            }
                        });
                    });
                }

                if (restrictionsIndirect.Count > 0)
                {
                    //List<BloquesForm> bloquesForms = new List<BloquesForm>();
                    //restrictionsIndirect.ForEach(rest =>
                    //{
                    //    rest.Rules.ForEach(rul =>
                    //    {
                    //        BloquesForm bloquesFormApplyRule = ConfigurationUser.Blocks.Where(bloque => bloque.BlockKey == rest.Block).ToList().ElementAt(0);
                    //        if (bloquesForms.Where(bf => bf.BlockKey == bloquesFormApplyRule.BlockKey).ToList().Count == 1)
                    //        {
                    //            bloquesFormApplyRule = bloquesForms.Where(bf => bf.BlockKey == bloquesFormApplyRule.BlockKey).ToList().ElementAt(0);
                    //        }
                    //        else
                    //        {
                    //            bloquesFormApplyRule.FormOption.ForEach(ele => ele.Active = false);
                    //            bloquesForms.Add(bloquesFormApplyRule);
                    //        }
                    //        if (rul.ValuesAcepted.Contains(bloquesFormSelected.KeySelected))
                    //        {
                    //            if (rul.Type == "HabilitarElementos")
                    //            {
                    //                bloquesFormApplyRule.FormOption.ForEach(elemt =>
                    //                {
                    //                    if (rul.BlockValues.Contains(elemt.Key))
                    //                    {
                    //                        elemt.Active = true;
                    //                    }
                    //                });
                    //            }
                    //            if (rul.Type == "RemoveElements")
                    //            {
                    //                bloquesFormApplyRule.FormOption.ForEach(elemt =>
                    //                {
                    //                    if (rul.BlockValues.Contains(elemt.Key))
                    //                    {
                    //                        elemt.Active = false;
                    //                    }
                    //                });
                    //            }
                    //        }
                    //    });
                    //});
                    //bloquesForms.ForEach(ele => {
                    //    BloquesForm bloquesFormApplyRule = ConfigurationUser.Blocks.Where(bloque => bloque.BlockKey == ele.BlockKey).ToList().ElementAt(0);
                    //    if (bloquesFormApplyRule.FormOption.Where(bfpr => bfpr.Key == bloquesFormApplyRule.KeySelected && bfpr.Active).ToList().Count == 0)
                    //    {
                    //        if (bloquesFormApplyRule.FormOption.Where(bfpr => bfpr.Active).ToList().Count > 0)
                    //        {
                    //            bloquesFormApplyRule.KeySelected = bloquesFormApplyRule.FormOption.Where(bfpr => bfpr.Active).ToList().ElementAt(0).Key;
                    //        }
                    //        else
                    //        {
                    //            throw new Exception("error al asignar nuevo valor a:" + bloquesFormApplyRule.BlockName);
                    //        }
                    //    }
                    //});
                }
            }
            else
            {
                ConfiguracionOp.FieldsFree.Where(fieldFree => fieldFree.BlockKey == bloquesFormSelected.BlockKey).ToList().ForEach(fieldFree => {

                    if (fieldFree.Type == "number")
                    {
                        bloquesFormSelected.KeySelected = bloquesFormSelected.KeySelectedUser;
                        decimal valor = 0;
                        if (!decimal.TryParse(bloquesFormSelected.KeySelected, out valor))
                        {
                            bloquesFormSelected.KeySelected = "1";
                        }
                        else
                        {
                            if (valor == 0)
                            {
                                bloquesFormSelected.KeySelected = "1";
                            }
                        }
                        if (fieldFree.IsRange)
                        {
                            if (fieldFree.RangeFrom <= double.Parse(bloquesFormSelected.KeySelected) && double.Parse(bloquesFormSelected.KeySelected) <= fieldFree.RangeTo)
                            {
                                if (fieldFree.HasCerosMask)
                                {
                                    string test = (int)(double.Parse(bloquesFormSelected.KeySelected) * (double)fieldFree.NumeroMult) + "";
                                    test = test.PadLeft(fieldFree.NumberCeros, '0');
                                    bloquesFormSelected.KeySelected = test;
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format("El valor {0} no cumple el rango {1}-{2}", bloquesFormSelected.KeySelected, fieldFree.RangeFrom, fieldFree.RangeTo));
                            }
                        }
                    }
                });
            }

            ConfigurationUser.ItemCode = GenerarteCOde();
            ConfigurationUser.ItemCodeexample = GenerarteCOde();
        }
        private void FillBloquesForm()
        {
            ConfigurationUser.Configurable = ConfiguracionOp.Configurable;
            ConfigurationUser.ItemCode = ConfiguracionOp.ItemCodeexample;
            ConfigurationUser.ItemCodeexample = ConfiguracionOp.ItemCodeexample;
            ConfigurationUser.Blocks = new List<BloquesForm>();
            ConfiguracionOp.Blocks.ForEach(bloque =>
            {
                BloquesForm blocke = new BloquesForm();
                blocke.BlockName = bloque.Block;
                blocke.BlockKey = bloque.Key;
                blocke.IsOpenUser = bloque.IsOpenUser;
                blocke.IsModificating = false;
                blocke.FormOption = new List<BloqueFormOptions>();
                bloque.Options.ForEach(option =>
                {
                    BloqueFormOptions bloqueFormOptions = new BloqueFormOptions();
                    bloqueFormOptions.Key = option.Key;
                    bloqueFormOptions.Option = option.Option;
                    bloqueFormOptions.Active = true;
                    blocke.FormOption.Add(bloqueFormOptions);
                });
                blocke.KeySelected = (string.IsNullOrEmpty(bloque.FixedValue) && !bloque.IsOpenUser ? bloque.Options.ElementAt(0).Key : bloque.FixedValue);
                ConfigurationUser.Blocks.Add(blocke);
            });
        }
        public void ValidCode()
        {
            if (procesatorConfigMode == ProcesatorConfigMode.WithoutCode)
            {
                ConfigurationUser.ItemCode = ConfigurationUser.ItemCodeexample;
            }
            else
            {

            }
            Regex Expresion = new Regex(ConfiguracionOp.Expresion);
            Match mc = Expresion.Match(ConfigurationUser.ItemCode);
            if (mc.Success)
            {
                MatchCollection mcs = Expresion.Matches(ConfigurationUser.ItemCode);
                foreach (Match m in mcs)
                {
                    foreach (string group in Expresion.GetGroupNames())
                    {
                        if (group != "0")
                        {
                            ConfigurationUser.Blocks.Find(ab => ab.BlockKey == group).KeySelected = m.Groups[group].Value.Trim();
                        }
                    }

                }
            }
            else
            {
                throw new Exception("No es valido el codigo: " + ConfigurationUser.ItemCode);
            }
        }
        public void ConfigureForm()
        {
            ConfiguracionOp.Rectrictions.ForEach(restriction => {
                //valor actual del bloque a validar rule from 
                string ValorActual = ConfigurationUser.Blocks.Find(ab => ab.BlockKey == restriction.Block).KeySelected;
                //procesar cada regla
                restriction.Rules.ForEach(rule => {
                    //verificar que el nodo Rectrictions> Rules > ValuesAcepted
                    if (rule.BlockValues.Contains(ValorActual))
                    {
                        if (rule.Type == "HabilitarElementos")
                        {
                            //List<BloqueFormOptions> FormOption = new List<BloqueFormOptions>();
                            BloquesForm bloquesForm = ConfigurationUser.Blocks.Find(ab => ab.BlockKey == rule.BlockApply);
                            bloquesForm.FormOption.ForEach(ab =>
                            {
                                if (rule.ValuesAcepted.Contains(ab.Key))
                                {
                                    ab.Active = true;
                                }
                                else
                                {
                                    ab.Active = false;
                                }
                            });
                            if (bloquesForm.FormOption.Where(ab => ab.Key == bloquesForm.KeySelected && ab.Active).ToList().Count == 0)
                            {
                                bloquesForm.KeySelected = bloquesForm.FormOption.Where(ab => ab.Active).ToList().ElementAt(0).Key;
                                // regenerar codigo
                            }
                            bloquesForm = null;// liberar memoria
                        }
                        if (rule.Type == "RemoveElements")
                        {
                            //List<BloqueFormOptions> FormOption = new List<BloqueFormOptions>();
                            BloquesForm bloquesForm = ConfigurationUser.Blocks.Find(ab => ab.BlockKey == rule.BlockApply);
                            bloquesForm.FormOption.ForEach(ab =>
                            {
                                if (rule.ValuesAcepted.Contains(ab.Key))
                                {
                                    ab.Active = false;
                                }
                                else
                                {
                                    ab.Active = true;
                                }
                            });
                            if (bloquesForm.FormOption.Where(ab => ab.Key == bloquesForm.KeySelected && ab.Active).ToList().Count == 0)
                            {
                                bloquesForm.KeySelected = bloquesForm.FormOption.Where(ab => ab.Active).ToList().ElementAt(0).Key;
                                // regenerar codigo
                            }
                            bloquesForm = null;// liberar memoria
                        }
                    }
                });
            });
            ConfiguracionOp.FieldsFree.ForEach(fieldFree => {
                if (fieldFree.Type == "number")
                {
                    if (fieldFree.IsRange)
                    {
                        BloquesForm bloquesForm = ConfigurationUser.Blocks.Find(bloc => bloc.BlockKey == fieldFree.BlockKey);
                        if (fieldFree.HasCerosMask)
                        {
                            double realValue = (int)(double.Parse(bloquesForm.KeySelected) / (double)fieldFree.NumeroMult);
                            if (fieldFree.RangeFrom <= double.Parse(bloquesForm.KeySelected) && double.Parse(bloquesForm.KeySelected) <= fieldFree.RangeTo)
                            {
                                bloquesForm.KeySelectedUser = realValue + "";
                            }
                            else
                            {
                                bloquesForm.KeySelectedUser = "1";
                                string test = (int)(double.Parse(bloquesForm.KeySelected) * (double)fieldFree.NumeroMult) + "";
                                test = test.PadLeft(fieldFree.NumberCeros, '0');
                                bloquesForm.KeySelected = test;
                            }
                        }
                        bloquesForm = null; // liberar memoria
                    }
                }
            });
            ConfigurationUser.ItemCode = GenerarteCOde();
        }
        private string GenerarteCOde()
        {
            string result = ""; ;
            ConfigurationUser.Blocks.ForEach(ab =>
            {
                result += ab.KeySelected;
            });
            return result;
        }
       
        public ConfigurationUser GetConfigurationUser()
        {
            ConfigurationUser.Description = new List<DescriptionElement>();
            ConfigurationUser.Blocks.ForEach(element => {
                element.IsModificating = false;
                DescriptionElement descriptionElement = new DescriptionElement();
                descriptionElement.Bloque = element.BlockName;
                if (!element.IsOpenUser)
                {
                    descriptionElement.Selected = element.FormOption.Find(ab => ab.Key == element.KeySelected).Option;
                }
                else
                {
                    if (ConfiguracionOp.FieldsFree.Where(ab => ab.BlockKey == element.BlockKey).ToList().Count == 1)
                    {
                        descriptionElement.Selected = element.KeySelectedUser + " " + ConfiguracionOp.FieldsFree.Where(ab => ab.BlockKey == element.BlockKey).ToList().ElementAt(0).UnitMesureUser;
                    }
                    else
                    {
                        descriptionElement.Selected = element.KeySelectedUser;
                    }

                }
                ConfigurationUser.Description.Add(descriptionElement);
                descriptionElement = null;// liberar memoria
            });
            return ConfigurationUser;
        }
        #endregion

    }

    public enum ProcesatorConfigMode
    {
        WithoutCode = 1,
        WithCode = 2,
    }
}
