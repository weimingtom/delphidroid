using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DCCDroid.Logic.Exceptions;
using DCCDroid.Property;

namespace DCCDroid
{
    public sealed class SourceCodeManager
    {
        #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly SourceCodeManager instance = new SourceCodeManager();
        }

        private SourceCodeManager()
        { }

        public static SourceCodeManager Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        public List<String> GetEventHandler(Control aControl)
        {
            List<String> result = new List<String>(); ;
            Control mainForm = ControlAction.Instance.GetMainForm();

            bool foundEventHandler = false;

            FileInfo[] pascalSourceFiles = AppUtils.GetFiles(AppConfiguration.Instance.InputPath, "*.pas", "Pascal source");
            foreach (FileInfo file in pascalSourceFiles)
            {
                List<String> sourceCodeList = GetSourceCode(file.FullName);
                bool afterBegin = false;
                foreach (String code in sourceCodeList)
                {
                    if (afterBegin && code.Trim().StartsWith("end"))
                        break;

                    if (afterBegin && !code.Trim().StartsWith("end"))
                    {
                        // TODO: This should actually be done by the Object Pascal language compiler (thats still outstanding)
                        result.Add(Parse(code));
                    }

                    if (code.IndexOf(mainForm.ControlName + "." + aControl.Prop_OnClick) > -1)
                        foundEventHandler = true;

                    if (foundEventHandler && code.Trim().StartsWith("begin"))
                        afterBegin = true;
                }
            }

            if (!foundEventHandler)
                throw new ArgumentException("Can't find event handler '" + aControl.Prop_OnClick + "' for control: " + aControl.ControlName);

            return result;
        }

        // TODO: FROM WHERE WE'RE GOING TO BUILD THE LANGUAGE PARSER
        private string Parse(string code)
        {
            String result = code;

            // Just some simple parsing
            result = result.Replace("'", "\"");
            result = result.Replace(":=", "=");
            result = result.Replace("Android_", "new Android(" + AppConfiguration.Instance.ActivityName + ".this).").Replace("'", "\"");

            // If assignment then process 
            String[] st = result.Split(' ');
            int totalAssignments = st.Count(s => s.CompareTo("=") == 0); // TODO: Change "=" into ":="
            if (totalAssignments > 1)
                throw new CompileErrorException("Can only have one assignment per statement line.");
            if (totalAssignments == 1)
            {
                String[] exp = result.Split('='); // TODO: Split base upon :=
                if (exp.Length == 2 && exp[0].IndexOf(".") > -1)
                {
                    String exp1 = exp[0].Trim();
                    String exp2 = exp[1].Trim();

                    String[] controlAssignmentExpression = exp1.Split('.');
                    String controlName = controlAssignmentExpression[0];
                    String propertyName = controlAssignmentExpression[1];
                    
                    Control assignmentControl = ControlAction.Instance.GetControlByName(controlName);
                    if (assignmentControl == null)
                        throw new CompileErrorException("Can't find control '" + controlName + "'");

                    IPropertyType assignmentProperty = ControlAction.Instance.GetProperty(assignmentControl, propertyName);
                    result = PropertiesManager.Instance.AssignmentStatement(assignmentControl, assignmentProperty, PropertiesManager.Instance.GetValue(exp2));

                }
            }

            result = PropertiesManager.Instance.GetValue(result);

            return result;
        }

        // TODO: ALL THE CODE LIKE THIS SHOULD GET REFACTORED 
        private List<string> GetSourceCode(String aFileName)
        {
            List<String> result = new List<String>();
            StreamReader file = null;
            try
            {
                file = new StreamReader(aFileName);
                String line = null;
                while ((line = file.ReadLine()) != null)
                    result.Add(line);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
            return result;
        }

        /// <summary>
        /// Copy the Android Framework JAR to the Android solution
        /// </summary>
        public void CopyFramework()
        {
            // TODO: Need to implement still when we converted the Android framework in a JAR lib
            Console.WriteLine("Warning: You need to manually copy the Android framework library before compile...");
        }
    }
}
