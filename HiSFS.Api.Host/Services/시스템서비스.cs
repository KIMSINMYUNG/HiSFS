using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.Parameters;
using HiSFS.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HiSFS.Api.Host.Services
{
    public class 시스템서비스 : I시스템서비스
    {
        private readonly IContextProvider dcp;


        public 시스템서비스(IContextProvider dbContextProvider)
        {
            this.dcp = dbContextProvider;
        }

        public Task<IEnumerable<메뉴정보>> 메뉴유형별권한_조회(string 권한유형코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.메뉴정보
                .Include(x => x.메뉴유형별권한목록.Where(x => x.권한유형코드 == 권한유형코드))
                .Where_미삭제()
                .Where(x => string.IsNullOrEmpty(x.메뉴명) == false)
                .OrderBy(x => x.정렬순번)
                .ToList();

            foreach (var item in list)
            {
                item.메뉴유형별권한 = item.메뉴유형별권한목록.FirstOrDefault();
                item.메뉴유형별권한목록 = null;
            }

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 메뉴유형별권한_저장(IEnumerable<메뉴유형별권한정보> list)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            foreach (var info in list)
            {
                var found = dc.메뉴유형별권한정보.Any(x => x.메뉴순번 == info.메뉴순번 && x.권한유형코드 == info.권한유형코드);
                if (found == true)
                    dc.메뉴유형별권한정보.Update(info);
                else
                    dc.메뉴유형별권한정보.Add(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<공통코드>> 공통코드_조회(string 상위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.공통코드
                .Where_미삭제_사용()
                .Where(x => x.상위코드 == 상위코드)
                .OrderBy(x => x.정렬순번)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task<IEnumerable<메뉴정보>> 메뉴직원권한_조회(string 직원사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var 직원정보 = dc.직원정보.FirstOrDefault(x => x.사번 == 직원사번);

            var list = dc.메뉴정보
                .Include(x => x.메뉴직원권한목록.Where(x => x.직원사번== 직원사번))
                .Include(x => x.메뉴부서권한목록.Where(y => y.부서코드 == 직원정보.부서코드))
                .Where_미삭제()
                .Where(x => string.IsNullOrEmpty(x.메뉴명) == false)
                .OrderBy(x => x.정렬순번)
                .ToList();

            foreach (var item in list)
            {
                item.메뉴부서권한 = item.메뉴부서권한목록.FirstOrDefault();
                item.메뉴부서권한목록 = null;

                item.메뉴직원권한 = item.메뉴직원권한목록.FirstOrDefault();
                item.메뉴직원권한목록 = null;
            }

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 메뉴직원권한_저장(IEnumerable<메뉴직원권한정보> list)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            foreach (var info in list)
            {
                var found = dc.메뉴직원권한정보.Any(x => x.메뉴순번 == info.메뉴순번 && x.직원사번 == info.직원사번);
                if (found == true)
                    dc.메뉴직원권한정보.Update(info);
                else
                    dc.메뉴직원권한정보.Add(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<메뉴정보>> 메뉴부서권한_조회(string 부서코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.메뉴정보
                .Include(x => x.메뉴부서권한목록.Where(x => x.부서코드 == 부서코드))
                .Where_미삭제()
                .Where(x => string.IsNullOrEmpty(x.메뉴명) == false)
                .OrderBy(x => x.정렬순번)
                .ToList();

            foreach (var item in list)
            {
                item.메뉴부서권한 = item.메뉴부서권한목록.FirstOrDefault();
                item.메뉴부서권한목록 = null;
            }

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 메뉴부서권한_저장(IEnumerable<메뉴부서권한정보> list)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            foreach (var info in list)
            {
                var found = dc.메뉴부서권한정보.Any(x => x.메뉴순번 == info.메뉴순번 && x.부서코드 == info.부서코드); 
                if (found == true)
                    dc.메뉴부서권한정보.Update(info);
                else
                    dc.메뉴부서권한정보.Add(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<메뉴정보>> 메뉴_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.메뉴정보
                .Include(x => x.상위메뉴)
                .Include(x => x.하위메뉴목록.Where(x => x.삭제유무 != true).OrderBy(x => x.정렬순번))
                .OrderBy(x => x.상위메뉴순번)
                .ThenBy(x => x.정렬순번)
                .Where(x => x.뎁스 == 0)
                .Where_미삭제()
                .ToList();

            var result = SearchDeep(list).ToList();
            result.ForEach(x => x.하위메뉴목록 = null);
            
            return Task.FromResult(result.AsEnumerable());
        }

        private static IEnumerable<메뉴정보> SearchDeep(IEnumerable<메뉴정보> source)
        {
            if (source == null)
                yield break;

            foreach (var node in source)
            {
                yield return node;
                foreach (var childNode in SearchDeep(node.하위메뉴목록))
                    yield return childNode;
            }
        }

        public Task 메뉴_저장(메뉴정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            info.상위메뉴 = null;

            if (isAdd == true)
                dc.메뉴정보.Add(info);
            else
                dc.메뉴정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 메뉴_삭제(메뉴정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.메뉴정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.메뉴정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<연동장비정보>> 연동장비_조회(검색정보 검색)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var query = dc.연동장비정보
                .OrderByDescending(x => x.등록시각)
                .Where_미삭제();
            if (검색?.유무(검색대상.사용) == true)
                query = query.Where_사용();
               
            var list = query.ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 연동장비_승인(연동장비정보 info, bool isApproval)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var target = dc.연동장비정보.FirstOrDefault(x => x.식별번호 == info.식별번호);
            if (target == default)
                return Task.CompletedTask;

            target.사용유무 = isApproval;
            target.승인시각 = isApproval == true ? DateTime.Now : null;
            dc.연동장비정보.Update(target);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<액션로그>> 액션로그_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.액션로그
                .Include(x => x.직원)
                .Include(x => x.액션)
                .Include(x => x.연동장비)
                .Order_등록최신()
                .Where_미삭제()
                .Take(1000)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        
    }
}
