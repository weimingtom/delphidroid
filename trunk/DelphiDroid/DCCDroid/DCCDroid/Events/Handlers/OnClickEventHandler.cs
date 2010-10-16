using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCCDroid.Events.Interface;

namespace DCCDroid.Events
{
    public sealed class OnClickEventHandler: IEventHandler
    {
        #region IEventHandler Members

        public bool HasEvent(Control aControl)
        {
            return aControl.Prop_OnClick != null && aControl.GetType().GetInterface(typeof(IOnClickEvent).Name) != null;
        }

        public void Process(Control aControl, bool aAlreadyHandled)
        {
            List<String> code = new List<String>();

            // Step 1: Add the event handle interface 
            if (!aAlreadyHandled)
                CodeInject.Instance.Add("//<interfaces>", "View.OnClickListener", true);

            // Step 2: Add to constructor to hook event to control
            code.Add(aControl.ControlClass() + " " + aControl.ControlName.ToLower() + " = (" + aControl.ControlClass() + ")findViewById(R.id." + aControl.ControlName + ");");
            code.Add(aControl.ControlName.ToLower() + ".setOnClickListener(this);");
            code = CodeInject.Instance.AddLinesAndClear("//<event_handlers>", code);

            // Step 3: Add the actual event handler methods
            if (!aAlreadyHandled)
            {
                code.Add("public void onClick(View view) ");
                code.Add("{ ");
                code.Add("switch (view.getId()) ");
                code.Add("{ ");
                code.Add("case R.id." + aControl.ControlName + ": ");
                code.Add(aControl.ControlName.ToLower() + "OnClick(view);");
                code.Add("break;");
                code.Add("//<onclick_next_here>");
                code.Add("}");
                code.Add("}");
            }
            else
            {
                code.Add("case R.id." + aControl.ControlName + ": ");
                code.Add(aControl.ControlName.ToLower() + "OnClick(view);");
                code.Add("break;");
                code = CodeInject.Instance.AddLinesAndClear("//<onclick_next_here>", code);
            }

            code.Add("\n");
            code.Add("private void " + aControl.ControlName.ToLower() + "OnClick(View view)");
            code.Add("{");
            code.AddRange(SourceCodeManager.Instance.GetEventHandler(aControl));
            code.Add("}");

            code = CodeInject.Instance.AddLinesAndClear("//<event_callers>", code);
        }

        #endregion
    }
}
