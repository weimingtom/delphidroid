using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace DCCDroid
{
    public sealed class AppUtils
    {
        /// <summary>
        /// Get the assembly version number
        /// </summary>
        /// <returns></returns>
        public static String GetAppVersionNumber()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            AssemblyName myAssemblyName = myAssembly.GetName();
            return myAssemblyName.Version.ToString();
        }

        /// <summary>
        /// Get the directory from command line
        /// </summary>
        /// <param name="aArgs"></param>
        /// <param name="aArgumentLocation"></param>
        /// <param name="aCreateIfNotExist"></param>
        /// <returns></returns>
        public static String GetDirectory(String[] aArgs, int aArgumentLocation, bool aCreateIfNotExist)
        {
            String directoryName = null;

            #if DEBUG
                directoryName = System.Configuration.ConfigurationManager.AppSettings["TestProject"];
            #else
            directoryName = aArgs[aArgumentLocation].Trim();
            #endif

            if (directoryName == null)
                throw new ArgumentException(String.Format("Directory doesn't exist: {0}", directoryName));

            if (aCreateIfNotExist && !Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            if (!directoryName.EndsWith(@"\"))
                directoryName += @"\";

            return directoryName;
        }


        /// <summary>
        /// Get the source directory fields
        /// </summary>
        /// <param name="aDirectoryName"></param>
        /// <param name="aExt"></param>
        /// <param name="aDescription"></param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(String aDirectoryName, String aExt, String aDescription)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(aDirectoryName);
            FileInfo[] sourceFileList = sourceDirectory.GetFiles(aExt);
            if (sourceFileList == null || sourceFileList.Length == 0)
                new ArgumentException(String.Format("No " + aDescription + " files found to compile in directory: {0}", aDirectoryName));
            return sourceFileList;
        }

        /// <summary>
        /// Return the raw lines (text format) from the given file
        /// </summary>
        /// <param name="aFileName"></param>
        /// <returns></returns>
        public static List<string> GetRawFileLines(string aFileName)
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
        /// Write lines of strings to the given file name
        /// </summary>
        /// <param name="aFileName"></param>
        /// <param name="aCodeLines"></param>
        public static void WriteFile(String aFileName, List<String> aCodeLines)
        {
            StreamWriter file = null;
            try
            {
                file = new StreamWriter(aFileName, true);

                foreach (String line in aCodeLines)
                    file.WriteLine(line);
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
