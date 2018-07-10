using Nancy;

namespace NETCore_Angular1.Modules
{
    public abstract class BaseModule : NancyModule
    {
        protected BaseModule(string route) : base(route)
        {
            
        }
    }
}
