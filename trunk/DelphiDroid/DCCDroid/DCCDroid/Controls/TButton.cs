using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCCDroid.Events.Interface;
using DCCDroid.Property;

namespace DCCDroid.Controls
{
    public sealed class TButton: Control, IOnClickEvent
    {
        public TButton()
        {}

        public override bool IsControl(string aCurrentLine)
        {
            return aCurrentLine.IndexOf(this.GetType().Name) > -1;
        }

        public override string ControlImportLocation()
        {
            return "android.widget.Button";
        }

        public override string ControlClass()
        {
            return "Button";
        }

        /// <summary>
        /// Return the button XML layout
        /// </summary>
        /// <returns></returns>
        public override String ScreenXMLLayout()
        {
            StringBuilder xml = new StringBuilder();

            xml.Append("<Button").Append("\n");
            xml.Append("android:id=\"@+id/").Append(this.ControlName).Append("\"").Append("\n");
            xml.Append("android:layout_width=\"wrap_content\"").Append("\n");
            xml.Append("android:layout_height=\"wrap_content\"").Append("\n");
            xml.Append("android:text=\"").Append(this.Prop_Caption).Append("\"").Append("\n");
            xml.Append("android:layout_x=\"").Append(this.Prop_Left).Append("px\"").Append("\n");
            xml.Append("android:layout_y=\"").Append(this.Prop_Top).Append("px\"").Append("\n");
            xml.Append(">").Append("\n");
            xml.Append("</Button>").Append("\n");

            return xml.ToString();
        }

        public override IPropertyType[] Properties()
        {
            return null;
        }
    }
}
