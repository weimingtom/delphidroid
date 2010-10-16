using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using DCCDroid.Property;

namespace DCCDroid
{
    public abstract class Control
    {
        private const int DEFAULT_WIDTH = 120;
        private const int DEFAULT_HEIGHT = 50;

        public String ControlName {get; set;}

        public String Prop_Left {get; set;}
        public String Prop_Top {get; set;}
        public String Prop_Color {get; set;}
        public String Prop_FontCharset {get; set;}
        public String Prop_FontColor {get; set;}
        public String Prop_FontHeight {get; set;}
        public String Prop_FontName {get; set;}
        public String Prop_FontStyle { get; set; }

        public String Prop_OldCreateOrder { get; set; }
        public String Prop_PixelsPerInch { get; set; }
        public String Prop_TextHeight { get; set; }

        public String Prop_TabOrder {get; set;}
        public String Prop_OnClick { get; set; }

        private String _prop_Width = null;
        public String Prop_Width
        {
            get
            {
                int width = Convert.ToInt32(_prop_Width);
                if (width < DEFAULT_WIDTH)
                    _prop_Width = Convert.ToString(DEFAULT_WIDTH);

                _prop_Width = (HasValue(Prop_Caption) || HasValue(Prop_Text)) ? "wrap_content" : _prop_Width + "px";
                return _prop_Width;
            }
            set
            { 
                _prop_Width = value; 
            }
        }
        
        private String _prop_Height = null;
        public String Prop_Height
        {
            get
            {
                int height = Convert.ToInt32(_prop_Height);
                if (height < DEFAULT_HEIGHT)
                    _prop_Height = Convert.ToString(DEFAULT_HEIGHT);

                _prop_Height = (HasValue(Prop_Caption) || HasValue(Prop_Text)) ? "wrap_content" : _prop_Height + "px";
                return _prop_Height;
            }
            set
            { 
                _prop_Height = value; 
            }
        }

        private String _prop_Caption = null;
        public String Prop_Caption 
        {
            get
            {
                return _prop_Caption;
            }
            set
            { 
                _prop_Caption = getPlainText(value); 
            }
        }

        private String _prop_Text = null;
        public String Prop_Text
        {
            get
            {
                return _prop_Text;
            }
            set
            { 
                _prop_Text = getPlainText(value); 
            }
        }

        /// <summary>
        /// The Delphi form layout file convert special characters like a single quote (') into 
        /// its ASCII code with a # prefix for example: "I'm your best friend" becomes "I#39m your best friend". This process 
        /// takes any such codes and convert them into the correct display. It also cater for cases where the ASCII code can be more
        /// than 2 digits long for example: #128
        /// </summary>
        /// <param name="aTextValue"></param>
        /// <returns></returns>
        private String getPlainText(String aTextValue)
        {
            String result = aTextValue;
            if (!String.IsNullOrEmpty(aTextValue))
            {
                result = result.Replace("'", "");
                int number = 0;
                while (result != null && result.IndexOf("#") > -1)
                {
                    String asciiCode = null;
                    int pos = result.IndexOf("#") + 1;
                    while (IsNumber(result.Substring(pos, 1)))
                    {
                        asciiCode += result.Substring(pos, 1);
                        pos++;
                        if (pos >= result.Length)
                            break;
                    }
                    number = Convert.ToInt32(asciiCode);
                    result = result.Replace("#" + number, Convert.ToChar(number).ToString());
                }
            }

            return result;
        }

        private bool HasValue(string aTextValue)
        {
            return !String.IsNullOrEmpty(aTextValue) && aTextValue.Length > 0;
        }

        bool IsNumber(string text)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text);
        }

        /// <summary>
        /// Set the control's property and values
        /// </summary>
        /// <param name="aRawControlProperty"></param>
        public void setProperty(string aRawControlProperty)
        {
            String[] st = aRawControlProperty.Split('=');
            String propertyName =  "Prop_" + st[0].Trim().Replace(".", "");
            String propertyValue = st[1].Trim().Replace("'", "");

            PropertyInfo[] properties = this.GetType().GetProperties();
            //bool foundProperty = false;
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.CompareTo(propertyName) == 0)
                {
                    properties[i].SetValue(this, propertyValue, null);
                    //foundProperty = true;
                }
            }

            //if (!foundProperty)
            //    Console.WriteLine("Warning: Can't find property: " + propertyName + " for control: " + this.ControlName);
        }

        public abstract bool IsControl(String aCurrentLine);
        public abstract String ControlImportLocation();
        public abstract String ControlClass();
        public abstract String ScreenXMLLayout();
        public abstract IPropertyType[] Properties();
    }
}
