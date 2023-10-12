using System;

namespace Kickstarter.Events
{
    public interface IServiceLocator
    {
        public void ImplementService(EventArgs args);
    }
}
