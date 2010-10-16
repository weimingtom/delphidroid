using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DCCDroid
{
    public sealed class AppConfiguration
    {
         #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly AppConfiguration instance = new AppConfiguration();
        }

        private AppConfiguration()
        { }

        public static AppConfiguration Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        #region Command line Arguments 

        private String _projectName;
        private String _activityName;
        private String _packageName;
        private String _inputPath;
        private String _outputPath;

        public String ProjectName 
        { 
            get
            {
              return _projectName;
            }
            private set
            { 
                _projectName = value; 
            }
        }

        public String ActivityName
        {
            get
            {
                return _activityName;
            }
            private set
            {
                _activityName = value;
            }
        }

        public String PackageName
        {
            get
            {
                return _packageName;
            }
            private set
            {
                _packageName = value;
            }
        }

        public String InputPath
        {
            get
            {
                return _inputPath;
            }
            private set
            {
                _inputPath = value;
            }
        }

        public String OutputPath
        {
            get
            {
                return _outputPath;
            }
            private set
            {
                _outputPath = value;
            }
        }

        #endregion

        #region Configuration Properties

        public String Config_AndroidSDKToolDirectory
        {
            get
            {
                String dir = System.Configuration.ConfigurationManager.AppSettings["AndroidSDKDirectory"];
                if (!dir.EndsWith(@"\"))
                    dir += @"\";

                return dir + @"tools\";
            }
        }

        public String Config_DelphiBinFolder
        {
            get
            {
                String dir = System.Configuration.ConfigurationManager.AppSettings["DelphiBinFolder"];
                if (!dir.EndsWith(@"\"))
                    dir += @"\";

                return dir;
            }
        }

        #endregion

        public List<Control> ControlsList {get; set;}
        public List<String> ActivityCodeLines { get; set; }

        /// <summary>
        /// Set the application configuration properties
        /// </summary>
        /// <param name="aProjectName"></param>
        /// <param name="aActivityName"></param>
        /// <param name="aPackageName"></param>
        /// <param name="aInputPath"></param>
        /// <param name="aOutputPath"></param>
        public void SetValues(String aProjectName, String aActivityName, String aPackageName, String aInputPath, String aOutputPath)
        {
            ProjectName = aProjectName;
            ActivityName = aActivityName;
            PackageName = aPackageName;
            InputPath = aInputPath;
            OutputPath = aOutputPath;
        }

        public void Validate()
        {
            if (!Directory.Exists(Config_AndroidSDKToolDirectory) || !File.Exists(Config_AndroidSDKToolDirectory + "adb.exe"))
                throw new ArgumentException("Invalid Android SDK directory, please make sure that the 'AndroidSDKDirectory' application config points to a valid Android SDK location.");

            if (!Directory.Exists(Config_DelphiBinFolder) || !File.Exists(Config_DelphiBinFolder + "convert.exe"))
                throw new ArgumentException("Invalid Delphi bin folder, please make sure that the 'DelphiBinFolder' application config points to a valid Delphi installation bin folder.");
        }
    }
}
