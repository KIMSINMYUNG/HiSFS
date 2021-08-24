using HiSFS.Api.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.WebApp.Services
{
    public class CacheService : IDisposable
    {
        private readonly CommonCodeDictionary<공통코드> commonCodeMap = new CommonCodeDictionary<공통코드>();
        private readonly CommonCodeDictionary<IList<공통코드>> commonCodeNodesMap = new CommonCodeDictionary<IList<공통코드>>();
        private readonly CommonCodeDictionary<IList<공통코드>> commonCodeSiblingMap = new CommonCodeDictionary<IList<공통코드>>();

        private RemoteService remoteService;

        public CommonCodeDictionary<공통코드> 코드 => commonCodeMap;
        public CommonCodeDictionary<IList<공통코드>> 코드목록 => commonCodeNodesMap;
        public CommonCodeDictionary<IList<공통코드>> 코드형제목록 => commonCodeSiblingMap;
        
        public IEnumerable<메뉴정보> 메뉴목록 { get; private set; }

        public CacheService(RemoteService remote)
        {
            remoteService = remote;

            //remoteService.ReadyEvent += Remote_ReadyEvent;

            _ = Task.Run(async () =>
            {
                var result = await remoteService.WaitForReadyRemoteService(5000);
                if (result == true)
                    Remote_ReadyEvent(this, new RemoteServiceReadyEventArgs(true));
            });
        }

        public void Dispose()
        {
            //remoteService.ReadyEvent -= Remote_ReadyEvent;
        }

        private async void Remote_ReadyEvent(object sender, RemoteServiceReadyEventArgs e)
        {
            if (e.IsReady == false)
                return;

            var list = await remoteService.Command.공통.공통코드_조회();
            ModifyCommonCode(list);
        }

        private void ModifyCommonCode(IEnumerable<공통코드> list)
        {
            commonCodeMap.Clear();
            commonCodeNodesMap.Clear();
            commonCodeSiblingMap.Clear();

            
            // 목록은 정렬되었다고 가정한다.
            foreach (var cc in list)
            {
                commonCodeMap[cc.코드] = cc;

                if (cc.상위코드 != null)
                {
                    if (commonCodeNodesMap.ContainsKey(cc.상위코드) == false)
                        commonCodeNodesMap[cc.상위코드] = new List<공통코드>();

                    commonCodeNodesMap[cc.상위코드].Add(cc);
                }
            }

            foreach (var nodes in commonCodeNodesMap.Values)
            {
                foreach (var node in nodes)
                    commonCodeSiblingMap[node.코드] = nodes;
            }
        }

        public void ModifyMenus(IEnumerable<메뉴정보> menuList)
        {
            메뉴목록 = menuList;
        }
    }

    public class CommonCodeDictionary<TValue> : ConcurrentDictionary<string, TValue>
        where TValue : class
    {
        public new TValue this[string key]
        {
            get
            {
                if (ContainsKey(key) == false)
                    return null;

                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }
    }
}