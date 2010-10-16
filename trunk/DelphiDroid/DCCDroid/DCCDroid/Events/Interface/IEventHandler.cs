using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCCDroid.Events.Interface
{
    public interface IEventHandler
    {
        /// <summary>
        /// Indicate that the control does have the indicated event
        /// </summary>
        /// <param name="aControl"></param>
        /// <returns></returns>
        bool HasEvent(Control aControl);

        /// <summary>
        /// Process the event handler by writting the required code
        /// </summary>
        /// <param name="aControl"></param>
        /// <param name="aAlreadyHandled"></param>
        void Process(Control aControl, bool aAlreadyHandled);
    }
}
