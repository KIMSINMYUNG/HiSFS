using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Core.Contracts;

namespace HiSFS.Shared.Wamp
{
    public class PrefixCalleeProxyInterceptor : CalleeProxyInterceptor
    {
        private readonly string identifier;

        public PrefixCalleeProxyInterceptor(string identifier) : base(new CallOptions { DiscloseMe = true })
        {
            this.identifier = identifier;
        }

        public override string GetProcedureUri(MethodInfo method)
        {
            var result = $"{identifier}.{base.GetProcedureUri(method)}";
            return result;
        }
    }
}
