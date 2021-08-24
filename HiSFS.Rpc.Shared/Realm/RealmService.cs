using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2;

namespace HiSFS.Rpc.Shared.Realm
{
    public class RealmService : IRealmService
    {
        private readonly IWampRealmServiceProvider sp;

        public RealmService(IWampRealmServiceProvider serviceProvider)
        {
            this.sp = serviceProvider;
        }

        public TProxy GetCalleeProxy<TProxy>() where TProxy : class => sp.GetCalleeProxy<TProxy>();

        public TProxy GetCalleeProxy<TProxy>(ICalleeProxyInterceptor interceptor) where TProxy : class => sp.GetCalleeProxy<TProxy>(interceptor);

        public ISubject<TEvent> GetSubject<TEvent>(string topicUri) => sp.GetSubject<TEvent>(topicUri);

        public IWampSubject GetSubject(string topicUri) => sp.GetSubject(topicUri);

        public ISubject<TTuple> GetSubject<TTuple>(string topicUri, IWampEventValueTupleConverter<TTuple> tupleConverter) => sp.GetSubject<TTuple>(topicUri, tupleConverter);

        public Task<IAsyncDisposable> RegisterCallee(object instance) => sp.RegisterCallee(instance);

        public Task<IAsyncDisposable> RegisterCallee(object instance, ICalleeRegistrationInterceptor interceptor) => sp.RegisterCallee(instance, interceptor);

        public Task<IAsyncDisposable> RegisterCallee(Type serviceType, Func<object> instanceProvider) => sp.RegisterCallee(serviceType, instanceProvider);

        public Task<IAsyncDisposable> RegisterCallee(Type serviceType, Func<object> instanceProvider, ICalleeRegistrationInterceptor interceptor) => sp.RegisterCallee(serviceType, instanceProvider, interceptor);

        public Task<IAsyncDisposable> RegisterCallee<TService>(Func<TService> instanceProvider) where TService : class => sp.RegisterCallee<TService>(instanceProvider);
        public Task<IAsyncDisposable> RegisterCallee<TService>(Func<TService> instanceProvider, ICalleeRegistrationInterceptor interceptor) where TService : class => sp.RegisterCallee<TService>(instanceProvider, interceptor);

        public IDisposable RegisterPublisher(object instance) => sp.RegisterPublisher(instance);

        public IDisposable RegisterPublisher(object instance, IPublisherRegistrationInterceptor interceptor) => sp.RegisterPublisher(instance, interceptor);

        public Task<IAsyncDisposable> RegisterSubscriber(object instance) => sp.RegisterSubscriber(instance);

        public Task<IAsyncDisposable> RegisterSubscriber(object instance, ISubscriberRegistrationInterceptor interceptor) => sp.RegisterSubscriber(instance, interceptor);
    }
}
