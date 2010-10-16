using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCCDroid.Property;
using DCCDroid.Logic.Exceptions;
using System.Text.RegularExpressions;

namespace DCCDroid
{
    public sealed class PropertiesManager
    {
        #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly PropertiesManager instance = new PropertiesManager();
        }

        private PropertiesManager()
        { }

        public static PropertiesManager Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        public String AssignmentStatement(Control aControl, IPropertyType aProperty, String aValue)
        {
            String result = aProperty.Set();
            if (aValue.EndsWith(";"))
                aValue = aValue.Remove(aValue.Length - 1, 1);

            result = result.Replace("$ControlClass", aControl.ControlClass());
            result = result.Replace("$ControlName", aControl.ControlName);
            result = result.Replace("$Value", aValue);
            return result;
        }

        public String GetValue(String aCodeLine)
        {
            String result = aCodeLine;

            Regex exp = new Regex(@"([\w.]+)\s*", RegexOptions.IgnoreCase);
            MatchCollection MatchList = exp.Matches(aCodeLine);
            List<String> matchExpressions = new List<String>();
            foreach (Match m in MatchList)
            {
                if (m.Value.IndexOf(".") > -1)
                    matchExpressions.Add(m.Value);
            }

            foreach (String expression in matchExpressions)
            {
                String[] controlAssignmentExpression = expression.Split('.');
                if (controlAssignmentExpression.Length == 2)
                {
                    String controlName = controlAssignmentExpression[0];
                    String propertyName = controlAssignmentExpression[1];

                    Control control = ControlAction.Instance.GetControlByName(controlName);
                    if (control != null)
                    {
                        IPropertyType property = ControlAction.Instance.GetProperty(control, propertyName);
                        result = result.Replace(expression, GetProperty(property, control));
                    }
                }
            }

            return result;
        }

        private String GetProperty(IPropertyType aProperty, Control aControl)
        {
            String result = aProperty.Get();
            result = result.Replace("$ControlClass", aControl.ControlClass());
            result = result.Replace("$ControlName", aControl.ControlName);
            return result;
        }
    }
}
