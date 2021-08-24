using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2;

namespace HiSFS.Rpc.Shared.Realm
{
    public interface IRealmService
    {
        TProxy GetCalleeProxy<TProxy>() where TProxy : class;
        TProxy GetCalleeProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class;
        ISubject<TEvent> GetSubject<TEvent>(string topicUri);
        IWampSubject GetSubject(string topicUri);
        ISubject<TTuple> GetSubject<TTuple>(string topicUri, IWampEventValueTupleConverter<TTuple> tupleConverter);
        Task<IAsyncDisposable> RegisterCallee(object instance);
        Task<IAsyncDisposable> RegisterCallee(object instance, ICalleeRegistrationInterceptor interceptor);
        Task<IAsyncDisposable> RegisterCallee(Type serviceType, Func<object> instanceProvider);
        Task<IAsyncDisposable> RegisterCallee(Type serviceType, Func<object> instanceProvider, ICalleeRegistrationInterceptor interceptor);
        Task<IAsyncDisposable> RegisterCallee<TService>(Func<TService> instanceProvider) where TService : class;
        Task<IAsyncDisposable> RegisterCallee<TService>(Func<TService> instanceProvider, ICalleeRegistrationInterceptor interceptor) where TService : class;
        IDisposable RegisterPublisher(object instance);
        IDisposable RegisterPublisher(object instance, IPublisherRegistrationInterceptor interceptor);
        Task<IAsyncDisposable> RegisterSubscriber(object instance);
        Task<IAsyncDisposable> RegisterSubscriber(object instance, ISubscriberRegistrationInterceptor interceptor);
    }
}
