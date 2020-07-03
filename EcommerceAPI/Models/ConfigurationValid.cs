using EcommerceAPI.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EcommerceAPI.Models
{
    public class ConfigurationValid
    {
        private ConfigurationDinamic Data { get; set; }
        private ConfigurationModel ModelView { get; set; }
        private MatchCollection matchesGroups { get; set; }
        public string ItemCode { get; set; }
        public ConfigurationValid()
        {
            ModelView = new ConfigurationModel();
        }
        public void LoadData()
        {
            var owners = System.IO.File.ReadAllText(@"C:\Users\Luis Martinez\Desktop\jumpersMonomodo.json");
            Data = JsonConvert.DeserializeObject<ConfigurationDinamic>(owners);
        }
        public bool GetGroupsValid()
        {
            ValidData();
            ModelView.ItemCode = ItemCode;

            Regex expression = new Regex(Data.Expresion);
            matchesGroups = expression.Matches(ItemCode);

            Match matchgeneral = expression.Match(ItemCode);
            if (matchgeneral.Success)
            {
                ModelView.Optionss = new List<GroupView>();
                //foreach (Match m in matchesGroups.Reverse())
                foreach (Match m in matchesGroups)
                {
                    foreach (string group in expression.GetGroupNames())
                    {
                        if (group != "0")
                        {
                            GroupView groupView = new GroupView();
                            Data.Groups.Where(gr => gr.Node == group).ToList().ForEach(a => {
                                groupView.Name = a.Node;
                                groupView.ValueSelected = m.Groups[group].Value.Trim();
                                groupView.TipeGroup = !a.ValueFixed ? "Optional" : "Fixed";
                                groupView.Mask = a.MaskExpresion;
                                groupView.IsSelect = false;
                                groupView.Values = new List<Values>();
                                a.Values.ForEach(val => {
                                    Values values = new Values();
                                    values.Name = val.Name;
                                    values.Value = val.Value;
                                    groupView.Values.Add(values);
                                });
                            });
                            ModelView.Optionss.Add(groupView);
                        }
                    }
                }
                ModelView.Isvalid = true;
            }
            else
            {
                ModelView.Isvalid = false;
            }
            return ModelView.Isvalid;
        }
        public void ApplyRestrictions()
        {

            Data.Groups.Where(nodo => nodo.Values.Count > 0 && nodo.Values.Where(valor => valor.Restrictions.Count > 0).ToList().Count > 0).ToList().ForEach(nodo => {
                nodo.Values.ForEach(valor => {
                    //datos del valor
                    valor.Restrictions.ForEach(restriccion => {
                        string ValueSelected = ModelView.Optionss.Find(ab => ab.Name == restriccion.Node).ValueSelected;
                        //ModelView.Optionss.Find(ab => ab.Name == nodo.Node).Values.Remove(valor1 => valor1.Value = valor.Value);
                        
                        //ddatos de las restricciones
                        if (!restriccion.ValuesAcepted.Contains(ValueSelected))
                        {
                            GroupView Lista = ModelView.Optionss.Find(ab => ab.Name == nodo.Node);
                            int positionValue = Lista.Values.FindIndex(ab => ab.Value == valor.Value);
                            Lista.Values.RemoveAt(positionValue);

                        }
                    });

                });
            });
        }
        public ConfigurationModel GetModelView()
        {
            return ModelView;
        }
        private void ValidData()
        {
            if (Data == null)
            {
                throw new Exception("please run the method LoadData");
            }
            if (ItemCode.Trim() == "START")
            {
                ItemCode = Data.ItemCodeexample;
            }


        }
    }
}
