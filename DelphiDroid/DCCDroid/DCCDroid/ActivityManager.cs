using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DCCDroid
{
    public sealed class ActivityManager
    {
         #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly ActivityManager instance = new ActivityManager();
        }

        private ActivityManager()
        { }

        public static ActivityManager Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        /// <summary>
        /// Update the activity code
        /// </summary>
        public void Update()
        {
            Console.WriteLine("Update Activity");

            StringBuilder activityFile = new StringBuilder();
            activityFile.Append(AppConfiguration.Instance.OutputPath).Append(@"src\").Append(AppConfiguration.Instance.PackageName.Replace(".", @"\")).Append(@"\");
            activityFile.Append(AppConfiguration.Instance.ActivityName).Append(".java");

            if (File.Exists(activityFile.ToString()))
                File.Delete(activityFile.ToString());

            AppConfiguration.Instance.ActivityCodeLines = AppUtils.GetRawFileLines("ActivityTemplate.txt");

            AddGeneral();
            ControlAction.Instance.AddControlImports();
            ControlAction.Instance.AddEventHandlers();

            AppUtils.WriteFile(activityFile.ToString(), AppConfiguration.Instance.ActivityCodeLines);
        }

        #region Private Methods

        private void AddGeneral()
        {
            CodeInject.Instance.Add("<packagename>", AppConfiguration.Instance.PackageName);
            CodeInject.Instance.Add("<activityname>", AppConfiguration.Instance.ActivityName);
        }

        #endregion
    }
}
