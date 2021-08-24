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
    public class PrefixCalleeRegistrationInterceptor : CalleeRegistrationInterceptor
    {
        private readonly string identifier;

        public PrefixCalleeRegistrationInterceptor(string identifier) : base(new RegisterOptions { DiscloseCaller = true })
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
