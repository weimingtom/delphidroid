using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCCDroid.Property;

namespace DCCDroid.Controls
{
    public sealed class TForm: Control
    {
        public TForm()
        {}

        public override bool IsControl(String aCurrentLine)
        {
            return aCurrentLine.IndexOf(this.GetType().Name) > -1;
        }

        public override string ControlImportLocation()
        {
            return null;
        }

        public override string ControlClass()
        {
            return null;
        }

        /// <summary>
        /// The form don't have a layout
        /// </summary>
        /// <returns></returns>
        public override String ScreenXMLLayout()
        {
            return null;
        }

        public override IPropertyType[] Properties()
        {
            return null;
        }
    }
}
