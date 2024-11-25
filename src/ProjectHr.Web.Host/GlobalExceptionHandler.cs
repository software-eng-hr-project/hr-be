using Abp;
using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;
using Bugsnag;

namespace ProjectHr.Web.Host
{
    public class GlobalExceptionHandler : IEventHandler<AbpHandledExceptionData>, ITransientDependency {
        
        private readonly IClient _bugsnag;
        
        public GlobalExceptionHandler(IClient bugsnag)
        {
            _bugsnag = bugsnag;
        }
        public void HandleEvent(AbpHandledExceptionData eventData)
        {
            var ex = eventData.Exception;
            if (!(ex is AbpException))
                _bugsnag.Notify(eventData.Exception);
        }
    }
}