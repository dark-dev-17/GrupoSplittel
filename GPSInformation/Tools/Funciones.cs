using GPSInformation.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace GPSInformation.Tools
{
    public static class Funciones
    {
        private static string[] UNIDADES = { "", "uno ", "dos ", "tres ", "cuatro ", "cinco ", "seis ", "siete ", "ocho ", "nueve " };
        private static string[] DECENAS = {"diez ", "once ", "doce ", "trece ", "catorce ", "quince ", "dieciseis ",
        "diecisiete ", "dieciocho ", "diecinueve", "veinte ", "treinta ", "cuarenta ",
        "cincuenta ", "sesenta ", "setenta ", "ochenta ", "noventa "};
        private static string[] CENTENAS = {"", "ciento ", "doscientos ", "trecientos ", "cuatrocientos ", "quinientos ", "seiscientos ",
        "setecientos ", "ochocientos ", "novecientos "};

        private static Regex r;

        public static string Convertir(String numero, bool mayusculas, string moneda = "PESOS")
        {

            String literal = "";
            String parte_decimal;
            //si el numero utiliza (.) en lugar de (,) -> se reemplaza
            numero = numero.Replace(".", ",");

            //si el numero no tiene parte decimal, se le agrega ,00
            if (numero.IndexOf(",") == -1)
            {
                numero = numero + ",00";
            }
            //se valida formato de entrada -> 0,00 y 999 999 999,00
            r = new Regex(@"\d{1,9},\d{1,2}");
            MatchCollection mc = r.Matches(numero);
            if (mc.Count > 0)
            {
                //se divide el numero 0000000,00 -> entero y decimal
                String[] Num = numero.Split(',');

                string MN = " M.N.";
                if (moneda != "PESOS")
                    MN = "";

                //de da formato al numero decimal
                parte_decimal = moneda + " " + Num[1] + "/100" + MN;
                //se convierte el numero a literal
                if (int.Parse(Num[0]) == 0)
                {//si el valor es cero
                    literal = "cero ";
                }
                else if (int.Parse(Num[0]) > 999999)
                {//si es millon
                    literal = getMillones(Num[0]);
                }
                else if (int.Parse(Num[0]) > 999)
                {//si es miles
                    literal = getMiles(Num[0]);
                }
                else if (int.Parse(Num[0]) > 99)
                {//si es centena
                    literal = getCentenas(Num[0]);
                }
                else if (int.Parse(Num[0]) > 9)
                {//si es decena
                    literal = getDecenas(Num[0]);
                }
                else
                {//sino unidades -> 9
                    literal = getUnidades(Num[0]);
                }
                //devuelve el resultado en mayusculas o minusculas
                if (mayusculas)
                {
                    return (literal ).ToUpper();
                }
                else
                {
                    return (literal );
                }
            }
            else
            {//error, no se puede convertir
                return literal = null;
            }
        }

        /* funciones para convertir los numeros a literales */

        private static string getUnidades(String numero)
        {   // 1 - 9
            //si tuviera algun 0 antes se lo quita -> 09 = 9 o 009=9
            String num = numero.Substring(numero.Length - 1);
            return UNIDADES[int.Parse(num)];
        }

        private static string getDecenas(String num)
        {// 99
            int n = int.Parse(num);
            if (n < 10)
            {//para casos como -> 01 - 09
                return getUnidades(num);
            }
            else if (n > 19)
            {//para 20...99
                String u = getUnidades(num);
                if (u.Equals(""))
                { //para 20,30,40,50,60,70,80,90
                    return DECENAS[int.Parse(num.Substring(0, 1)) + 8];
                }
                else
                {
                    return DECENAS[int.Parse(num.Substring(0, 1)) + 8] + "y " + u;
                }
            }
            else
            {//numeros entre 11 y 19
                return DECENAS[n - 10];
            }
        }

        private static string getCentenas(String num)
        {// 999 o 099
            if (int.Parse(num) > 99)
            {//es centena
                if (int.Parse(num) == 100)
                {//caso especial
                    return " cien ";
                }
                else
                {
                    return CENTENAS[int.Parse(num.Substring(0, 1))] + getDecenas(num.Substring(1));
                }
            }
            else
            {//por Ej. 099
                //se quita el 0 antes de convertir a decenas
                return getDecenas(int.Parse(num) + "");
            }
        }

        private static string getMiles(String numero)
        {// 999 999
            //obtiene las centenas
            String c = numero.Substring(numero.Length - 3);
            //obtiene los miles
            String m = numero.Substring(0, numero.Length - 3);
            String n = "";
            //se comprueba que miles tenga valor entero
            if (int.Parse(m) > 0)
            {
                if(int.Parse(m) == 1)
                {
                    n = getCentenas(m);
                    return "mil " + getCentenas(c);
                }
                else
                {
                    n = getCentenas(m);
                    return n + "mil " + getCentenas(c);
                }
                
            }
            else
            {
                return "" + getCentenas(c);
            }

        }

        private static string getMillones(String numero)
        { //000 000 000
            //se obtiene los miles
            String miles = numero.Substring(numero.Length - 6);
            //se obtiene los millones
            String millon = numero.Substring(0, numero.Length - 6);
            String n = "";
            if (millon.Length > 1)
            {
                n = getCentenas(millon) + "millones ";
            }
            else
            {
                n = getUnidades(millon) + "millon ";
            }
            return n + getMiles(miles);
        }

    public static double GetAntiguedad(DateTime fechaInicio)
        {
            TimeSpan antiguedad = DateTime.Now - fechaInicio;
            return antiguedad.TotalDays / 365;
        }

        public static List<Registro> GetRegistrosInc()
        {
            List<Registro>  Nomenclatura = new List<Registro>();
            Nomenclatura.Add(new Registro { Clave = "S.E.5", Title = "Salario Emocional", TextColor = "#ffffff", Color = "#ffff00", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "VAC", Title = "Vacacione", TextColor = "#ffffff", Color = "#2962ff", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "PCG", Title = "Permiso con goce", TextColor = "#ffffff", Color = "#bf360c", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "VIS", Title = "Visitas a Cliente", TextColor = "#ffffff", Color = "#bdbdbd", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "FAL", Title = "Falta Injustificado", TextColor = "#ffffff", Color = "#0277bd", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "INC", Title = "Incapacidad Medico", TextColor = "#ffffff", Color = "#bf360c", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "GS", Title = "Guardia Sabatino", TextColor = "#000000", Color = "#e0e0e0", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "DXG", Title = "Descanso por Guardia Sabatina Horario Flotado", TextColor = "#ffffff", Color = "#aa00ff", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "DXC", Title = "Descanso por Cumpleaños", TextColor = "#ffffff", Color = "#009688", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "FES", Title = "Festivo", TextColor = "#ffffff", Color = "#76ff03", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "PSG", Title = "permiso sin goce de sueldo", TextColor = "#ffffff", Color = "#9ccc65", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "TXT", Title = "Tiempo x Tiempo", TextColor = "#ffffff", Color = "#f1948a ", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "SAL", Title = "SALIDA POR TEMA DE TRABAJO O CURSO", TextColor = "#ffffff", Color = "#aeea00", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "HO", Title = "Home Office", TextColor = "#ffffff", Color = "#186a3b", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "SNJ", Title = "Incidencia sin justificar", TextColor = "#ffffff", Color = "#dd2c00", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "INJ", Title = "Incidencia justificada", TextColor = "#ffffff", Color = "#000000", Tipo = 1 });
            Nomenclatura.Add(new Registro { Clave = "DES", Title = "Descanso", TextColor = "#fd4d45", Color = "#000000", Tipo = 1 });


            return Nomenclatura;
        }

        public static int MonthDifference(this DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        public static string GetDayName(DateTime Date)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;
            return cul.DateTimeFormat.GetDayName(Date.DayOfWeek);
        }

        public static string GetMotnhName(DateTime Date)
        {
            CultureInfo cul = CultureInfo.CurrentCulture;
            return cul.DateTimeFormat.GetMonthName(Date.Month);
        }

        public static string GetYearName(DateTime Date)
        {
            string Num2Text = "";
            int value = Date.Year;
            if (value < 0) return "menos " + Math.Abs(value).ToString();

            if (value == 0) Num2Text = "cero";
            else if (value == 1) Num2Text = "uno";
            else if (value == 2) Num2Text = "dos";
            else if (value == 3) Num2Text = "tres";
            else if (value == 4) Num2Text = "cuatro";
            else if (value == 5) Num2Text = "cinco";
            else if (value == 6) Num2Text = "seis";
            else if (value == 7) Num2Text = "siete";
            else if (value == 8) Num2Text = "ocho";
            else if (value == 9) Num2Text = "nueve";
            else if (value == 10) Num2Text = "diez";
            else if (value == 11) Num2Text = "once";
            else if (value == 12) Num2Text = "doce";
            else if (value == 13) Num2Text = "trece";
            else if (value == 14) Num2Text = "catorce";
            else if (value == 15) Num2Text = "quince";
            else if (value < 20) Num2Text = "dieci" + (value - 10).ToString();
            else if (value == 20) Num2Text = "veinte";
            else if (value < 30) Num2Text = "veinti" + (value - 20).ToString();
            else if (value == 30) Num2Text = "treinta";
            else if (value == 40) Num2Text = "cuarenta";
            else if (value == 50) Num2Text = "cincuenta";
            else if (value == 60) Num2Text = "sesenta";
            else if (value == 70) Num2Text = "setenta";
            else if (value == 80) Num2Text = "ochenta";
            else if (value == 90) Num2Text = "noventa";
            else if (value < 100)
            {
                int u = value % 10;
                Num2Text = string.Format("{0} y {1}", ((value / 10) * 10).ToString(), (u == 1 ? "un" : (value % 10).ToString()));
            }
            else if (value == 100) Num2Text = "cien";
            else if (value < 200) Num2Text = "ciento " + (value - 100).ToString();
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
                Num2Text = ((value / 100)).ToString() + "cientos";
            else if (value == 500) Num2Text = "quinientos";
            else if (value == 700) Num2Text = "setecientos";
            else if (value == 900) Num2Text = "novecientos";
            else if (value < 1000) Num2Text = string.Format("{0} {1}", ((value / 100) * 100).ToString(), (value % 100).ToString());
            else if (value == 1000) Num2Text = "mil";
            else if (value < 2000) Num2Text = "mil " + (value % 1000).ToString();
            else if (value < 1000000)
            {
                Num2Text = ((value / 1000)).ToString() + " mil";
                if ((value % 1000) > 0) Num2Text += " " + (value % 1000).ToString();
            }
            else if (value == 1000000) Num2Text = "un millón";
            else if (value < 2000000) Num2Text = "un millón " + (value % 1000000).ToString();
            else if (value < int.MaxValue)
            {
                Num2Text = ((value / 1000000)).ToString() + " millones";
                if ((value - (value / 1000000) * 1000000) > 0) Num2Text += " " + (value - (value / 1000000) * 1000000).ToString();
            }
            return Num2Text;

        }
        public static void EscribeLog(string mensaje)
        {
            try
            {
                if (!Directory.Exists("C:\\Splittel\\GestionPersonal\\Log\\"))
                    Directory.CreateDirectory("C:\\Splittel\\GestionPersonal\\Log\\");
                StreamWriter sw = new StreamWriter("C:\\Splittel\\GestionPersonal\\Log\\Log_" + DateTime.Today.ToString("MM_dd_yyyy") + ".log", true);
                sw.WriteLine(DateTime.Now.ToString() + " - " + mensaje);
                sw.Close();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

        }

        public static string ValidateEmail(string dataset)
        {
            string Malo = "";
            foreach (var a in GetEmails(dataset))
            {
                if (!IsValidEmail(a))
                {
                    Malo = a;
                    break;
                }
            }

            return Malo;
        }

        private static bool IsValidEmail(string email)
        {
            bool errorStatus = false;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (!re.IsMatch(email.Trim()))
                errorStatus = false;
            else
            {
                errorStatus = true;
            }
            return errorStatus;
        }
        private static List<string> GetEmails(string dataset)
        {
            List<string> list = new List<string>();
            string[] allAddresses = dataset.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string emailAddress in allAddresses)
            {
                list.Add(emailAddress);
            }

            return list;
        }
    }
}
