using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DCCDroid
{
    public sealed class AndroidProjectManager
    {
        #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly AndroidProjectManager instance = new AndroidProjectManager();
        }

        private AndroidProjectManager()
        { }

        public static AndroidProjectManager Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        /// <summary>
        /// Create the Android project solution
        /// </summary>
        public void CreateProject()
        {
            if (!File.Exists(AppConfiguration.Instance.OutputPath + "AndroidManifest.xml"))
            {
                Console.WriteLine("\nCreate Android project...\n");

                StringBuilder createCommand = new StringBuilder();
                createCommand.Append(AppConfiguration.Instance.Config_AndroidSDKToolDirectory);
                createCommand.Append("android create project ");
                createCommand.Append("--target 10 ");
                createCommand.Append("--name ").Append(AppConfiguration.Instance.ProjectName).Append(" ");
                createCommand.Append("--path ").Append(AppConfiguration.Instance.OutputPath).Append(" ");
                createCommand.Append("--activity ").Append(AppConfiguration.Instance.ActivityName).Append(" ");
                createCommand.Append("--package ").Append(AppConfiguration.Instance.PackageName).Append(" ");

                Create(AppConfiguration.Instance.OutputPath, createCommand.ToString());
            }
            else
            {
                Console.WriteLine("\nAndroid project already exists\n");
            }
        }

        /// <summary>
        /// Run the tool to create
        /// </summary>
        /// <param name="aOutputPath"></param>
        /// <param name="aCommand"></param>
        private void Create(String aOutputPath, String aCommand)
        {
            StringBuilder compileBatFile = new StringBuilder();
            StreamWriter file = null;
            try
            {
                compileBatFile.Append("create_project.bat");

                if (File.Exists(compileBatFile.ToString()))
                    File.Delete(compileBatFile.ToString());

                file = new StreamWriter(compileBatFile.ToString(), true);
                file.WriteLine(aCommand);
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
}
