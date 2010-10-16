using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DCCDroid.Controls;
using System.Threading;
using DCCDroid.Logic.Exceptions;

namespace DCCDroid
{
    public sealed class AndroidLayoutManager
    {
        #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly AndroidLayoutManager instance = new AndroidLayoutManager();
        }

        private AndroidLayoutManager()
        { }

        public static AndroidLayoutManager Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        /// <summary>
        /// Create the form layout (main.xml)
        /// TODO: Later on we can extend this to support multiple forms
        /// NOTE: We will ALWAYS use the Absolute Layout for now
        /// </summary>
        public void CreateMainLayout()
        {
            String mainLayoutDirectory = AppConfiguration.Instance.OutputPath + @"res\layout";
            String mainXmlLayoutFile = mainLayoutDirectory + @"\main.xml";
            if (File.Exists(mainXmlLayoutFile))
                File.Delete(mainXmlLayoutFile);

            Control mainForm = ControlAction.Instance.GetMainForm();

            StringBuilder xml = new StringBuilder();
            xml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>").Append("\n");
            xml.Append("<AbsoluteLayout").Append("\n");
            xml.Append("android:id=\"@+id/").Append(mainForm.ControlName).Append("\"").Append("\n");
            xml.Append("android:layout_width=\"fill_parent\"").Append("\n");
            xml.Append("android:layout_height=\"fill_parent\"").Append("\n");
            xml.Append("xmlns:android=\"http://schemas.android.com/apk/res/android\"").Append("\n");
            xml.Append(">").Append("\n");

            foreach (Control control in AppConfiguration.Instance.ControlsList)
            {
               String controlLayout = control.ScreenXMLLayout();
               if (controlLayout != null)
                   xml.Append(controlLayout);
            }

            xml.Append("</AbsoluteLayout>");

            int i = 0;
            while (!Directory.Exists(mainLayoutDirectory))
            {
                Thread.Sleep(1000);
                i++;
                if (i == 1000)
                    break;
            }

            if (!Directory.Exists(mainLayoutDirectory))
                throw new CompileErrorException("Unable to create Android solution, please make sure that the Android SDK is properly setup.");

            StreamWriter file = null;
            try
            {
                file = new StreamWriter(mainXmlLayoutFile, true);
                file.WriteLine(xml.ToString());
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
        }
    }
}
