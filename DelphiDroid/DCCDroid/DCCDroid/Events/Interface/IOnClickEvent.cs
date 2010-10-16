using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCCDroid.Events.Interface
{
    /// <summary>
    /// Just add this interface to any control(s) that got an OnClick event to indicate
    /// that it can have the event
    /// 
    /// The code will resolve and call the correct handler
    /// </summary>
    public interface IOnClickEvent
    {}
}
