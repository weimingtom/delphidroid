using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DCCDroid.Controls;

namespace DCCDroid
{
    public sealed class FormLayoutManager
    {
        #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly FormLayoutManager instance = new FormLayoutManager();
        }

        private FormLayoutManager()
        { }

        public static FormLayoutManager Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        /// <summary>
        /// Convert the Delphi form (DFM) to text
        /// </summary>
        public void CompileToText()
        {
            Console.WriteLine("\nConvert DFM to text...\n");

            FileInfo[] dfmFiles = AppUtils.GetFiles(AppConfiguration.Instance.InputPath, "*.dfm", "Delphi form (DFM)");

            if (dfmFiles.Length > 1)
                throw new ArgumentException("Error: Only one Delphi form supported at this time. ");

            for (int i = 0; i < dfmFiles.Length; i++)
            {
                StringBuilder command = new StringBuilder();
                command.Append("\"").Append(AppConfiguration.Instance.Config_DelphiBinFolder);
                command.Append("convert.exe\"").Append(" -t \"").Append(AppConfiguration.Instance.InputPath).Append(dfmFiles[i]).Append("\"");

                StringBuilder compileBatFile = new StringBuilder();
                StreamWriter file = null;
                try
                {
                    compileBatFile.Append("dfm2txt.bat");

                    if (File.Exists(compileBatFile.ToString()))
                        File.Delete(compileBatFile.ToString());

                    file = new StreamWriter(compileBatFile.ToString(), true);
                    file.WriteLine(command.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }

                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(compileBatFile.ToString());
                psi.RedirectStandardOutput = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                System.Diagnostics.Process listFiles;
                listFiles = System.Diagnostics.Process.Start(psi);
                System.IO.StreamReader myOutput = listFiles.StandardOutput;
                listFiles.WaitForExit(2000);
            }
        }

        /// <summary>
        /// Get the form's layout by returning the controls (and properties) that they contain
        /// </summary>
        /// <returns></returns>
        public List<Control> GetFormLayout()
        {
            List<Control> result = new List<Control>();

            Console.WriteLine("Get Form Layout...");

            FileInfo[] txtDfmFiles = AppUtils.GetFiles(AppConfiguration.Instance.InputPath, "*.txt", "Delphi form layout (TXT)");
            for (int i = 0; i < txtDfmFiles.Length; i++)
            {
                List<String> lines = AppUtils.GetRawFileLines(txtDfmFiles[i].FullName);
                Control control = null;
                foreach (String currentLine in lines)
                {
                    if (control != null && currentLine.Trim().StartsWith("object") && currentLine.IndexOf("TForm") == -1)
                    {
                        result.Add(control);
                        control = null;
                    }

                    if (control == null && currentLine.Trim().StartsWith("object"))
                        control = ControlAction.Instance.SetupControl(currentLine);

                    else if (!currentLine.Trim().StartsWith("end"))
                        control.setProperty(currentLine);

                    if (control != null && currentLine.Trim().StartsWith("end"))
                    {
                        result.Add(control);
                        control = null;
                    }
                }
            }

            return result;
        }
    }
}
