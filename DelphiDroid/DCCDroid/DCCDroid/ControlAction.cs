using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCCDroid.Controls;
using System.Reflection;
using DCCDroid.Events.Interface;
using DCCDroid.Property;
using DCCDroid.Logic.Exceptions;

namespace DCCDroid
{
    public sealed class ControlAction
    {
        #region Singleton

        class Singleton
        {
            static Singleton()
            { }
            internal static readonly ControlAction instance = new ControlAction();
        }

        private ControlAction()
        { }

        public static ControlAction Instance
        {
            get
            {
                return Singleton.instance;
            }
        }

        #endregion

        /// <summary>
        /// Get the control from the form layout
        /// </summary>
        /// <param name="aCurrentLine"></param>
        /// <returns></returns>
        public Control SetupControl(string aCurrentLine)
        {
            Control result = null;

            String[] st = aCurrentLine.Split(':');
            String controlName = st[0].Trim().Replace("object", "").Replace(" ", "").Replace("'", "");
            String controlTypeString = st[1].Trim();

            // we don't want to hard code the list of controls so we get use some reflection magic
            // to get all controls that got the Control abstract class as a base class type
            List<Control> controls = new List<Control>();
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Type[] types = myAssembly.GetTypes();
            for(int i = 0; i < types.Length; i++)
            {
                if (types[i].BaseType == typeof(Control))
                    controls.Add((Control) Activator.CreateInstance(types[i]));
            }
                            
            foreach(Control currentControl in controls)
            {
                if (currentControl.IsControl(aCurrentLine))
                {
                    result = (Control)Activator.CreateInstance(currentControl.GetType());
                    result.ControlName = controlName;
                    break;
                }
            }

            if (result == null)
               throw new CompileErrorException("Control: '" + controlTypeString + "' not supported. ");

            return result;
        }

        /// <summary>
        /// Return the main form control
        /// </summary>
        /// <returns></returns>
        public Control GetMainForm()
        {
            Control result = (from c in AppConfiguration.Instance.ControlsList
                              where c.GetType() == typeof(TForm)
                              select c).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Add Java import for all the controls
        /// </summary>
        public void AddControlImports()
        {
            foreach (Control currentControl in AppConfiguration.Instance.ControlsList)
            {
                if (currentControl.ControlImportLocation() != null)
                {
                    List<String> import = new List<String>();
                    import.Add("import " + currentControl.ControlImportLocation() + ";");
                    String[] codeAsArray = AppConfiguration.Instance.ActivityCodeLines.ToArray();
                    for (int i = 0; i < codeAsArray.Length; i++)
                    {
                        if (codeAsArray[i].IndexOf("//<import>") > -1)
                            AppConfiguration.Instance.ActivityCodeLines.InsertRange(i, import);
                    }
                }
            }
        }

        /// <summary>
        /// Add event handlers for all controls
        /// This process is clever where you indicate an event handler on the control
        /// The control doesn't worry about the event handler code since that get handled by the handler 
        /// We just want to check if the control does have the event
        /// </summary>
        public void AddEventHandlers()
        {
            // Find all the event handlers
            List<IEventHandler> eventHandlersList = new List<IEventHandler>();
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Type[] types = myAssembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                bool isEventHandler = (from inter in types[i].GetInterfaces()
                                       where inter.Name.CompareTo(typeof(IEventHandler).Name) == 0
                                       select inter).FirstOrDefault() != null;
                if (isEventHandler)
                    eventHandlersList.Add((IEventHandler)Activator.CreateInstance(types[i]));
            }

            // Run thru all the controls, check what events they support and write the event handling code
            if (eventHandlersList.Count > 0)
            {
                List<IEventHandler> handled = new List<IEventHandler>(); 
                foreach (Control currentControl in AppConfiguration.Instance.ControlsList)
                {
                    bool isFirstEvent = true;
                    foreach (IEventHandler currentEventHandler in eventHandlersList)
                    {
                        if (currentEventHandler.HasEvent(currentControl))
                        {
                            bool alreadyHandled = (from e in handled
                                                   where e == currentEventHandler
                                                   select e).FirstOrDefault() != null;

                            if (!alreadyHandled)
                            {
                                if (isFirstEvent)
                                    CodeInject.Instance.Add("//<interfaces>", " implements ", true);
                                else
                                    CodeInject.Instance.Add("//<interfaces>", ", ", true);
                            }

                            currentEventHandler.Process(currentControl, alreadyHandled);

                            if (!alreadyHandled)
                                handled.Add(currentEventHandler);

                            isFirstEvent = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Return the control by name
        /// </summary>
        /// <param name="aControlName"></param>
        /// <returns></returns>
        public Control GetControlByName(string aControlName)
        {
            Control result = (from c in AppConfiguration.Instance.ControlsList
                              where c.ControlName.CompareTo(aControlName) == 0
                              select c).FirstOrDefault();
            
            return result;
        }
        
        /// <summary>
        /// Return the property for the given control
        /// </summary>
        /// <param name="aControl"></param>
        /// <param name="aPropertyName"></param>
        /// <returns></returns>
        public IPropertyType GetProperty(Control aControl, String aPropertyName)
        {
            IPropertyType[] controlProperties = aControl.Properties();
            if (controlProperties == null || controlProperties.Length == 0)
                throw new CompileErrorException("Control '" + aControl.ControlClass() + "' doesn't have any properties");

            IPropertyType property = (from c in controlProperties
                                                where c.GetType().Name.CompareTo(aPropertyName) == 0
                                                select c).FirstOrDefault();

            return property;
        }
    }
}
