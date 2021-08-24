using System;
using System.Collections.Generic;
using System.Text;

namespace HiSFS.Rpc.Shared.Realm
{
    public class Realm : IRealm
    {
        private IRealmService service;


        public IRealmService Service => service;


        public Realm(IRealmService service)
        {
            this.service = service;
        }
    }
}
