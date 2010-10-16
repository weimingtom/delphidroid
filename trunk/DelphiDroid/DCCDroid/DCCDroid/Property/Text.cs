using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCCDroid.Property
{
    public class Text: IPropertyType
    {
        public string Get()
        {
            return "(($ControlClass) findViewById(R.id.$ControlName)).getText().toString()";
        }

        public string Set()
        {
            return "(($ControlClass) findViewById(R.id.$ControlName)).setText($Value);";
        }
    }
}
