using Abp.Dependency;
using Abp.Events.Bus.Exceptions;
using Abp.Events.Bus.Handlers;

namespace ProjectHr.Web.Host
{
    public class GlobalExceptionHandler : IEventHandler<AbpHandledExceptionData>, ITransientDependency {
        public void HandleEvent(AbpHandledExceptionData eventData)
        {
            
            // TODO: will be add bugsnag or 3rd party error logging
            var ex = eventData.Exception;

        }
    }
}