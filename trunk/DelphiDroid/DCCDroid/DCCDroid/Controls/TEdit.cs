using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCCDroid.Events.Interface;
using DCCDroid.Property;

namespace DCCDroid.Controls
{
    public sealed class TEdit : Control, IOnClickEvent
    {
        public TEdit()
        { }

        public override bool IsControl(string aCurrentLine)
        {
            return aCurrentLine.IndexOf(this.GetType().Name) > -1;
        }

        public override string ControlImportLocation()
        {
            return "android.widget.EditText";
        }

        public override string ControlClass()
        {
            return "EditText";
        }

        public override IPropertyType[] Properties()
        {
            return new IPropertyType[] {new Text()};
        }

        public override string ScreenXMLLayout()
        {
            StringBuilder xml = new StringBuilder();

            xml.Append("<EditText").Append("\n");
            xml.Append("android:id=\"@+id/").Append(this.ControlName).Append("\"").Append("\n");
            xml.Append("android:layout_width=\"").Append(this.Prop_Width).Append("\"\n");
            xml.Append("android:layout_height=\"").Append(this.Prop_Height).Append("\"\n");
            xml.Append("android:text=\"").Append(this.Prop_Text).Append("\"").Append("\n");
            xml.Append("android:layout_x=\"").Append(this.Prop_Left).Append("px\"").Append("\n");
            xml.Append("android:layout_y=\"").Append(this.Prop_Top).Append("px\"").Append("\n");
            xml.Append(">").Append("\n");
            xml.Append("</EditText>").Append("\n");

            return xml.ToString();
        }
    }
}
