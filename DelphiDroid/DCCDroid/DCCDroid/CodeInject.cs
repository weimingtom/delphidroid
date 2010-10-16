using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCCDroid
{
    public sealed class CodeInject
    {
         #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly CodeInject instance = new CodeInject();
        }

        private CodeInject()
        { }

        public static CodeInject Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        /// <summary>
        /// Add code to the given tag but don't keep the tag
        /// </summary>
        /// <param name="aTagName"></param>
        /// <param name="aValue"></param>
        public void Add(string aTagName, string aValue)
        {
            Add(aTagName, aValue, false);
        }

        /// <summary>
        /// Add code to given tag, can indicate if it should keep the tag
        /// </summary>
        /// <param name="aTagName"></param>
        /// <param name="aValue"></param>
        /// <param name="aKeepTag"></param>
        public void Add(string aTagName, string aValue, bool aKeepTag)
        {
            String[] allLines = AppConfiguration.Instance.ActivityCodeLines.ToArray();
            for (int i = 0; i < allLines.Length; i++)
            {
                String currentLine = allLines[i];

                if (currentLine.IndexOf(aTagName) > -1)
                    currentLine = currentLine.Replace(aTagName, aValue) + (aKeepTag ? aTagName : "");

                allLines[i] = currentLine;
            }
            AppConfiguration.Instance.ActivityCodeLines = allLines.ToList<String>();
        }

        /// <summary>
        /// Insert code lines at the tag and then return an empty code list so that you can populate it again
        /// </summary>
        /// <param name="aTagName"></param>
        /// <param name="aCode"></param>
        /// <returns></returns>
        public List<String> AddLinesAndClear(String aTagName, List<String> aCode)
        {
            String[] codeAsArray = AppConfiguration.Instance.ActivityCodeLines.ToArray();
            for (int i = 0; i < codeAsArray.Length; i++)
            {
                if (codeAsArray[i].IndexOf(aTagName) > -1)
                    AppConfiguration.Instance.ActivityCodeLines.InsertRange(i, aCode);
            }
            return new List<String>();
        }
    }
}
