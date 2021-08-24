using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.Parameters;
using HiSFS.Api.Shared.Models.View;
using HiSFS.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HiSFS.Api.Shared.Models.View_DZICUBE;

namespace HiSFS.Api.Host.Services
{
    public class 생산관리서비스 : I생산관리서비스
    {
        private readonly IContextProvider dcp;


        public 생산관리서비스(IContextProvider dbContextProvider)
        {
            this.dcp = dbContextProvider;
        }

        public Task<IEnumerable<보유품목정보>> 설비현황_조회(string 회사코드, bool isOnlyUse)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목정보
                    .Include(x => x.품목)
                    .Include(x => x.장소)
                    .Include(x => x.장소위치)
                    .Include(x => x.설비가동현황)
                .Where(x => (isOnlyUse == true && x.사용유무 == true) || isOnlyUse == false)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .Where(x => x.품목.품목구분코드 == "B1205")    // 설비만 조회
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }
        public Task<IEnumerable<보유품목정보>> 보유설비현황_조회(string 회사코드, bool isOnlyUse, string 품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목정보
                    .Include(x => x.품목)
                    .Include(x => x.장소)
                    .Include(x => x.장소위치)
                    .Include(x => x.설비가동현황)
                .Where(x => (isOnlyUse == true && x.사용유무 == true) || isOnlyUse == false)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .Where(x => x.품목구분코드 == "B1205" && x.보유품목코드 == 품목코드)     // 설비만 조회
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<생산품공정정보>> 생산품공정_조회(string 회사코드, 검색정보 검색)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var query = dc.생산품공정정보
                .Include(x => x.생산품)
                .Include(x => x.생산품공정차수목록.Where(x => x.삭제유무 != true).OrderBy(x => x.공정차수)).ThenInclude(y => y.공정단위).ThenInclude(z => z.공정)
                .Include(x => x.생산품공정차수목록).ThenInclude(y => y.공정단위).ThenInclude(z => z.도면)
                .Where(x => x.회사코드 == 회사코드)
                .OrderByDescending(x => x.CreateTime)
                .Where_미삭제();

            //if (검색?.유무(검색대상.사용) == true)
            //    query = query.Where_사용();

            var result = query.ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task 생산품공정_저장(생산품공정정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.생산품코드 = info.생산품?.품목코드;
            info.생산품 = null;
            info.생산품공정차수목록 = null;
            // }}}

            if (isAdd == true)
            {
                info.생산품공정코드 = $"{info.생산품코드}:{info.관리차수}";
                dc.생산품공정정보.Add(info);
            }
            else
                dc.생산품공정정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 생산품공정_삭제(생산품공정정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.생산품 = null;
            info.생산품공정차수목록 = null;
            //

            if (isCompletely == true)
                dc.생산품공정정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.생산품공정정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<생산품공정차수정보>> 생산품공정차수_조회(string 생산품공정코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산품공정차수정보
                .Include(x => x.공정단위).ThenInclude(x => x.공정)
                .Where_미삭제_사용()
                .Where(x => x.생산품공정코드 == 생산품공정코드)
                .OrderBy(x => x.공정차수)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }
        public Task 생산품공정차수_저장(생산품공정차수정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.생산품공정 = null;
            info.공정단위코드 = info.공정단위?.공정단위코드;
            info.공정단위 = null;

            // }}}

            if (isAdd == true)
            {
                info.순번 = dc.생산품공정차수정보.Count(x => x.생산품공정코드 == info.생산품공정코드 && x.사용유무 == true) + 1;
                info.공정차수 = dc.생산품공정차수정보.Count(x => x.생산품공정코드 == info.생산품공정코드 && x.사용유무 == true) + 1;
                dc.생산품공정차수정보.Add(info);
            }
            else
                dc.생산품공정차수정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 생산품공정차수_삭제(생산품공정차수정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.생산품공정차수정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.생산품공정차수정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<공정단위정보>> 공정단위_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정단위정보
                .Include(x => x.공정품)
                .Include(x => x.완제품)
                .Include(x => x.도면)
                .Include(x => x.공정)
                .Include(x => x.공정자재목록.Where(x => x.회사코드 == 회사코드)).ThenInclude(y => y.자재)
                .Include(x => x.공정설비목록.Where(x => x.회사코드 == 회사코드)).ThenInclude(y => y.설비).ThenInclude(z => z.품목)
                .Include(x => x.공정설비목록.Where(x => x.회사코드 == 회사코드)).ThenInclude(y => y.설비).ThenInclude(z => z.장소)
                .Include(x => x.공정설비목록.Where(x => x.회사코드 == 회사코드)).ThenInclude(y => y.설비).ThenInclude(z => z.장소위치)

                .Include(x => x.공정검사목록.Where(x => x.회사코드 == 회사코드)).ThenInclude(y => y.품질검사)
                .Include(x => x.공정검사목록.Where(x => x.회사코드 == 회사코드)).ThenInclude(x => x.공정검사장비목록.Where(x => x.회사코드 == 회사코드)).ThenInclude(y => y.검사장비)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task 공정단위_저장(공정단위정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            string 공정유형코드 = info.공정 != null ? info.공정.공정유형코드 : "";
            // track 방지 {{{
            info.공정품코드 = info.공정품?.품목코드;
            info.공정품 = null;
            info.완제품코드 = info.완제품?.품목코드;
            info.완제품 = null;
            info.도면코드 = info.도면?.도면코드;
            info.도면 = null;
            info.공정코드 = info.공정?.공정코드;
            info.공정 = null;
            info.공정자재목록 = null;
            info.공정설비목록 = null;
            info.공정품유형 = null;
            info.공정자재목록 = null;
            info.공정설비목록 = null;
            info.공정검사목록 = null;
            // }}}

            if (isAdd == true)
            {
                var max원공정단위코드 = dc.공정단위정보.DefaultIfEmpty().Max(x => x.원공정단위코드);
                var lastCount = 0;
                if (max원공정단위코드 != default)
                    lastCount = int.Parse(max원공정단위코드[2..]);
                info.원공정단위코드 = $"PU{lastCount + 1:0000}";
                info.공정단위코드 = $"{info.원공정단위코드}:{info.관리차수}";
                dc.공정단위정보.Add(info);
            }
            else
                dc.공정단위정보.Update(info);

            //공정유형이 검사가 아닌경우  공정유형코드 검사  "B0205" 
            if (공정유형코드 != "B0205")
            {

                // 공정단위관리에서  BOM정보 연동 공정품코드 필수 선택 
                var result = (from t in dc.BOM품목정보상세
                              where t.BOM품목정보코드 == info.공정품코드
                              select t);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        item.공정단위코드 = info.공정단위코드;
                        dc.BOM품목정보상세.Update(item);
                    }

                }
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공정단위_삭제(공정단위정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.공정품 = null;
            info.완제품 = null;
            info.공정품유형 = null;
            info.도면 = null;
            info.공정 = null;
            info.공정자재목록 = null;
            info.공정설비목록 = null;
            info.공정자재목록 = null;
            info.공정설비목록 = null;
            info.공정검사목록 = null;
            // }}}

            if (isCompletely == true)
                dc.공정단위정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.공정단위정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        /// <summary>
        /// 공정단위에서 사용 가능한 설비 목록을 가져온다.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<보유품목정보>> 공정단위_설비목록()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목정보
                    .Include(x => x.품목)
                    .Include(x => x.장소)
                    .Include(x => x.장소위치)
                .Where_미삭제_사용()
                .Where(x => x.품목.품목구분코드 == "B1205")     // 설비만 조회
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task 공정단위자재_저장(공정단위자재정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            info.공정단위 = null;
            info.자재 = null;

            if (isAdd == true)
                dc.공정단위자재정보.Add(info);
            else
                dc.공정단위자재정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공정단위자재_삭제(공정단위자재정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.공정단위자재정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.공정단위자재정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        // 수정 2020.02.01 파라메터추가 
        public Task<IEnumerable<공정단위검사정보>> 공정단위검사_조회(string 공정단위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정단위검사정보
                .Include(x => x.품질검사)
                .Include(x => x.공정검사장비목록).ThenInclude(x => x.검사장비)
            .Where_미삭제_사용()
            .Where(x => x.공정단위코드 == 공정단위코드)
            .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task 공정단위검사_저장(공정단위검사정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.품질검사 = null;
            info.검사단위 = null;
            // }}}

            if (isAdd == true)
                dc.공정단위검사정보.Add(info);
            else
            {
                var 공정단위검사 = dc.공정단위검사정보.Where(x => x.회사코드 == info.회사코드 && x.품질검사코드 == info.품질검사코드).FirstOrDefault();
                공정단위검사.검사기준값 = info.검사기준값;
                공정단위검사.검사단위 = info.검사단위;
                공정단위검사.검사단위코드 = info.검사단위코드;
                공정단위검사.검사측정값 = info.검사측정값;
                공정단위검사.공정단위코드 = info.공정단위코드;
                공정단위검사.오차범위 = info.오차범위;
                공정단위검사.오차범위상한 = info.오차범위상한;
                공정단위검사.오차범위하한 = info.오차범위하한;
                공정단위검사.회사코드 = info.회사코드;
                공정단위검사.사용유무 = info.사용유무;
                공정단위검사.삭제유무 = info.삭제유무;
                dc.공정단위검사정보.Update(공정단위검사);
            }


            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공정단위검사_삭제(공정단위검사정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.공정단위검사정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.공정단위검사정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공정단위검사장비_저장(공정단위검사장비 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.검사정보 = null;
            info.검사장비 = null;
            // }}}

            if (isAdd == true)
                dc.공정단위검사장비.Add(info);
            else
                dc.공정단위검사장비.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공정단위검사장비_삭제(공정단위검사장비 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.공정단위검사장비.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.공정단위검사장비.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공정단위설비_저장(공정단위설비정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            info.공정단위 = null;
            info.설비 = null;

            if (isAdd == true)
                dc.공정단위설비정보.Add(info);
            else
                dc.공정단위설비정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공정단위설비_삭제(공정단위설비정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.공정단위설비정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.공정단위설비정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<생산계획정보>> 생산계획_조회(string 회사코드, 검색정보 검색)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var query1 = dc.생산계획정보
                .Include(x => x.발주처)
                .Include(x => x.생산품)
                .Include(x => x.생산품공정)
                .Include(x => x.생산책임자)
                .Include(x => x.생산지시목록)
                .Where(x => x.회사코드 == 회사코드);
            var query2 = query1.Where_미삭제().OrderByDescending(x => x.CreateTime);
            //if (검색?.유무(검색대상.사용) == true)
            //    query2 = query2.Where_사용();

            var result = query2.ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task 생산계획_저장(생산계획정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            info.발주처 = null;
            info.생산품 = null;
            info.생산품공정 = null;
            info.생산책임자 = null;

            if (isAdd == true)
            {
                var key =  $"PP{dc.생산계획정보.Count(x => x.회사코드 == info.회사코드) + 1:00000}";
                info.생산계획코드 = key;
                dc.생산계획정보.Add(info);
            }
            else
                dc.생산계획정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 생산계획_삭제(생산계획정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.생산계획정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.생산계획정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<생산계획정보> 생산계획상세_조회(string 생산계획코드, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산계획정보
                .Include(x => x.발주처)
                .Include(x => x.생산품)
                .Include(x => x.생산품공정).ThenInclude(x => x.생산품)
                .Include(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정)
                .Include(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정자재목록).ThenInclude(d => d.자재)
                .Include(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정설비목록).ThenInclude(d => d.설비)
                .Include(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정설비목록).ThenInclude(d => d.설비).ThenInclude(e => e.품목)
                .Include(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정설비목록).ThenInclude(d => d.설비).ThenInclude(e => e.장소)
                .Include(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정설비목록).ThenInclude(d => d.설비).ThenInclude(e => e.장소위치)

                .Include(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정검사목록).ThenInclude(d => d.품질검사)
                .Include(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정검사목록).ThenInclude(d => d.공정검사장비목록).ThenInclude(e => e.검사장비)

                .Include(x => x.생산책임자)
                .Include(x => x.생산계획기본).ThenInclude(y => y.계획자)
                .Include(x => x.생산계획기본).ThenInclude(y => y.검토자)
                .Include(x => x.생산계획영업).ThenInclude(y => y.계획자)
                .Include(x => x.생산계획영업).ThenInclude(y => y.검토자)
                .Include(x => x.생산계획연구소).ThenInclude(y => y.계획자)
                .Include(x => x.생산계획연구소).ThenInclude(y => y.검토자)
                .Include(x => x.생산계획구매).ThenInclude(y => y.계획자)
                .Include(x => x.생산계획구매).ThenInclude(y => y.검토자)
                .Include(x => x.생산계획생산).ThenInclude(y => y.계획자)
                .Include(x => x.생산계획생산).ThenInclude(y => y.검토자)
                .Include(x => x.생산계획품질).ThenInclude(y => y.계획자)
                .Include(x => x.생산계획품질).ThenInclude(y => y.검토자)
                .Include(x => x.생산계획생산관리).ThenInclude(y => y.계획자)
                .Include(x => x.생산계획생산관리).ThenInclude(y => y.검토자)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                 .FirstOrDefault(x => x.생산계획코드 == 생산계획코드);

            return Task.FromResult(result);
        }

        public Task 생산계획상세_저장(생산계획정보 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.발주처코드 = info.발주처?.거래처코드;
            info.발주처 = null;
            info.생산품코드 = info.생산품?.품목코드;
            info.생산품 = null;
            info.생산품공정코드 = info.생산품공정?.생산품공정코드;
            info.생산품공정 = null;
            info.생산책임자사번 = info.생산책임자?.사번;
            info.생산책임자 = null;


            info.생산계획기본.계획자사번 = info.생산계획기본.계획자?.사번;
            //info.생산계획기본.계획자 = null;
            info.생산계획기본.검토자사번 = info.생산계획기본.검토자?.사번;
            //info.생산계획기본.검토자 = null;


            if (info.생산계획기본.생산계획코드 == null)
            {
                info.생산계획기본.회사코드 = info.회사코드;
                info.생산계획기본.생산계획코드 = info.생산계획코드;
                dc.생산계획기본정보.Add(info.생산계획기본);
            }
            else
                dc.생산계획기본정보.Update(info.생산계획기본);

            info.생산계획영업.계획자사번 = info.생산계획영업.계획자?.사번;
            //info.생산계획영업.계획자 = null;
            info.생산계획영업.검토자사번 = info.생산계획영업.검토자?.사번;
            //info.생산계획영업.검토자 = null;


            if (info.생산계획영업.생산계획코드 == null)
            {
                info.생산계획영업.회사코드 = info.회사코드;
                info.생산계획영업.생산계획코드 = info.생산계획코드;
                dc.생산계획영업정보.Add(info.생산계획영업);
            }
            else
                dc.생산계획영업정보.Update(info.생산계획영업);


            info.생산계획연구소.계획자사번 = info.생산계획연구소.계획자?.사번;
            //info.생산계획연구소.계획자 = null;
            info.생산계획연구소.검토자사번 = info.생산계획연구소.검토자?.사번;
            // info.생산계획연구소.검토자 = null;

            if (info.생산계획연구소.생산계획코드 == null)
            {
                info.생산계획연구소.회사코드 = info.회사코드;
                info.생산계획연구소.생산계획코드 = info.생산계획코드;
                dc.생산계획연구소정보.Add(info.생산계획연구소);
            }

            else
                dc.생산계획연구소정보.Update(info.생산계획연구소);



            info.생산계획구매.계획자사번 = info.생산계획구매.계획자?.사번;
            //info.생산계획구매.계획자 = null;
            info.생산계획구매.검토자사번 = info.생산계획구매.검토자?.사번;
            //info.생산계획구매.검토자 = null;

            if (info.생산계획구매.생산계획코드 == null)
            {
                info.생산계획구매.회사코드 = info.회사코드;
                info.생산계획구매.생산계획코드 = info.생산계획코드;
                dc.생산계획구매정보.Add(info.생산계획구매);

            }

            else
                dc.생산계획구매정보.Update(info.생산계획구매);


            info.생산계획생산.계획자사번 = info.생산계획생산.계획자?.사번;
            //info.생산계획생산.계획자 = null;
            info.생산계획생산.검토자사번 = info.생산계획생산.검토자?.사번;
            //info.생산계획생산.검토자 = null;



            if (info.생산계획생산.생산계획코드 == null)
            {
                info.생산계획생산.회사코드 = info.회사코드;
                info.생산계획생산.생산계획코드 = info.생산계획코드;
                dc.생산계획생산정보.Add(info.생산계획생산);
            }
            else
                dc.생산계획생산정보.Update(info.생산계획생산);


            info.생산계획품질.계획자사번 = info.생산계획품질.계획자?.사번;
            //info.생산계획품질.계획자 = null;
            info.생산계획품질.검토자사번 = info.생산계획품질.검토자?.사번;
            //info.생산계획품질.검토자 = null;



            if (info.생산계획품질.생산계획코드 == null)
            {
                info.생산계획품질.회사코드 = info.회사코드;
                info.생산계획품질.생산계획코드 = info.생산계획코드;
                dc.생산계획품질정보.Add(info.생산계획품질);
            }
            else
                dc.생산계획품질정보.Update(info.생산계획품질);



            info.생산계획생산관리.계획자사번 = info.생산계획생산관리.계획자?.사번;
            //info.생산계획생산관리.계획자 = null;
            info.생산계획생산관리.검토자사번 = info.생산계획생산관리.검토자?.사번;
            //info.생산계획생산관리.검토자 = null;


            if (info.생산계획생산관리.생산계획코드 == null)
            {
                info.생산계획생산관리.회사코드 = info.회사코드;
                info.생산계획생산관리.생산계획코드 = info.생산계획코드;
                dc.생산계획생산관리정보.Add(info.생산계획생산관리);
            }
            else
                dc.생산계획생산관리정보.Update(info.생산계획생산관리);

            dc.SaveChanges();


            dc.생산계획정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<생산지시정보>> 작업지시_조회(string 회사코드, 검색정보 검색)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var query1 = dc.생산지시정보
                .Include(x => x.생산계획).Where(x => x.회사코드 == 회사코드);
            var query2 = query1.Where_미삭제();
            if (검색?.유무(검색대상.사용) == true)
                query2 = query2.Where_사용();
            if (검색?.유무(검색대상.상태) == true)
                query2 = query2.Where(x => x.실행상태코드 == 검색[검색대상.상태]);

            var result = query2.ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task 작업지시_저장(생산지시정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            info.생산계획코드 = info.생산계획?.생산계획코드;
            info.생산계획 = null;

            if (isAdd == true)
            {
                var 순번 = dc.생산지시정보.Count(x => x.생산계획코드 == info.생산계획코드) + 1;
                info.생산지시코드 = $"{info.생산계획코드:00000}:{순번}";
                info.순번 = 순번;
                dc.생산지시정보.Add(info);
            }
            else
                dc.생산지시정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 작업지시_삭제(생산지시정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.생산지시정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.생산지시정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<생산지시정보> 작업지시상세_조회(string 작업지시코드, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산지시정보
                .Include(x => x.생산계획).ThenInclude(x => x.발주처)
                .Include(x => x.생산계획).ThenInclude(x => x.생산품)
                .Include(x => x.생산계획).ThenInclude(x => x.생산품공정).ThenInclude(x => x.생산품)
                .Include(x => x.생산계획).ThenInclude(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정)
                .Include(x => x.생산계획).ThenInclude(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정검사목록).ThenInclude(d => d.품질검사)
                .Include(x => x.생산계획).ThenInclude(x => x.생산책임자)
                .Include(x => x.생산지시공정차수목록.OrderBy(x => x.공정차수)).ThenInclude(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.공정)
                .Include(x => x.생산지시공정차수목록.OrderBy(x => x.공정차수)).ThenInclude(x => x.생산품공정차수).ThenInclude(x => x.생산품공정)
                .Include(x => x.생산지시공정차수목록).ThenInclude(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.도면)
                .Include(x => x.생산지시공정차수목록).ThenInclude(x => x.작업자)
                .Include(x => x.생산지시공정차수목록).ThenInclude(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.공정설비목록)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                 .FirstOrDefault(x => x.생산지시코드 == 작업지시코드);

            // 최초 공정차수목록이 없을 경우 생산품공정차수목록기준으로 생성한다.
            if (result.생산지시공정차수목록 == null || result.생산지시공정차수목록.Count == 0)
            {
                try
                {
                    result.생산지시공정차수목록 = dc.생산품공정차수정보
                   .Where_미삭제_사용()
                   .Where(x => x.생산품공정코드 == result.생산계획.생산품공정코드 && x.회사코드 == 회사코드)
                   .OrderBy(x => x.공정차수)
                   .ToList()
                   .Select(x => new 생산지시공정차수정보
                   {
                       회사코드 = result.회사코드,
                       생산지시코드 = result.생산지시코드,
                       공정차수 = x.공정차수,
                       생산품공정코드 = x.생산품공정코드,
                       생산품공정차수 = result.생산계획.생산품공정.생산품공정차수목록.FirstOrDefault(y => y.생산품공정코드 == x.생산품공정코드 && y.공정차수 == x.공정차수 && y.회사코드 == x.회사코드),
                   }).ToList();
                }
                catch (Exception)
                {
                    result = null;

                }

            }

            return Task.FromResult(result);
        }

        public Task 작업지시상세_저장(생산지시정보 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.생산계획 = null;
            //info.생산지시공정차수목록 = null;
            foreach (var 생산지시공정차수 in info.생산지시공정차수목록)
            {
                생산지시공정차수.생산지시 = null;
                생산지시공정차수.생산품공정차수순번 = 생산지시공정차수.생산품공정차수.순번;
                생산지시공정차수.생산품공정차수 = null;
                생산지시공정차수.작업자사번 = 생산지시공정차수.작업자?.사번;
                생산지시공정차수.작업자 = null;
            }

            var c = dc.생산지시공정차수.Count(x => x.생산지시코드 == info.생산지시코드 && x.회사코드 == info.회사코드);
            if (c == 0)
                dc.생산지시공정차수.AddRange(info.생산지시공정차수목록);
            // }}}

            dc.생산지시정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        // -------------------------------------
        public Task<IEnumerable<생산계획자재보유현황>> 생산계획생산자재소요현황_조회(string 생산계획코드, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // 계획에 등록된 생산품공정 기준 필요 원자재/부품/반제품 목록을 얻은 뒤, 현 보유품목을 통해 수량 파악 한다. {{{

            // 필요자재목록을 가져온다.
            var 필요자재목록 = dc.생산계획정보
                .Include(x => x.생산품공정)
                    .ThenInclude(x => x.생산품공정차수목록)
                        .ThenInclude(x => x.공정단위)
                            .ThenInclude(x => x.공정자재목록)
                                .ThenInclude(x => x.자재)                             // 자재까지 목록 생성
                .Where(x => x.생산계획코드 == 생산계획코드 && x.회사코드 == 회사코드)                           // 생산계획코드 항목만 취득
                .Select(x => x.생산품공정.생산품공정차수목록).SelectMany(x => x)
                .Select(x => x.공정단위.공정자재목록).SelectMany(x => x).ToList();            // 그 중 공정 필요 자재만 획득

            var result = (
                    from a in 필요자재목록                                               // 필요 자재 대비 보유 자재로 수량 파악 -- 생산계획소요현황View 생성
                     join b in dc.보유품목정보 on a.자재코드 equals b.품목코드 into ps
                    from p in ps.DefaultIfEmpty()
                    select new 생산계획자재보유현황
                    {
                        생산계획코드 = 생산계획코드,
                        필요자재코드 = a.자재코드,
                        필요자재 = a.자재,
                        자재유형코드 = a.자재.품목구분코드,
                        필요수량 = a.수량,
                        보유수량 = p?.수량 ?? 0,
                         // 없음 / 심각 / 부족 / 충분 / 과보유
                     }
                ).ToList();
            /*
            var 필요자재목록 = dc.생산계획정보
              .Include(x => x.생산품공정)
                  .ThenInclude(x => x.생산품공정차수목록)
                      .ThenInclude(x => x.공정단위)
                          .ThenInclude(x => x.S_BOM품목정보상세)
                              .ThenInclude(x => x.품목)                             // 자재까지 목록 생성
              .Where(x => x.생산계획코드 == 생산계획코드 )                           // 생산계획코드 항목만 취득
              .Select(x => x.생산품공정.생산품공정차수목록).SelectMany(x => x)
              .Select(x => x.공정단위.S_BOM품목정보상세).SelectMany(x => x).ToList();            // 그 중 공정 필요 자재만 획득


            var result = (
                   from a in 필요자재목록                                               // 필요 자재 대비 보유 자재로 수량 파악 -- 생산계획소요현황View 생성
                    join b in dc.보유품목정보 on a.품목코드 equals b.품목코드 into ps
                   from p in ps.DefaultIfEmpty()
                   select new 생산계획자재보유현황
                   {
                       생산계획코드 = 생산계획코드,
                       필요자재코드 = a.품목코드,
                       필요자재 = a.품목,
                       자재유형코드 = a.품목.품목구분코드,
                       필요수량 = a.필요수량,
                       보유수량 = p?.수량 ?? 0,
                        // 없음 / 심각 / 부족 / 충분 / 과보유
                    }
               ).ToList();
            */
            foreach (var info in result)
            {
                if (info.필요수량 == 0)
                    info.보유상태코드 = "B1903";  // 충분
                else
                {
                    var diff = info.보유수량 / info.필요수량;
                    info.보유상태코드 = diff switch
                    {
                        < 0.4m => "B1901",
                        < 1m => "B1902",
                        < 1.5m => "B1903",
                        >= 1.5m => "B1904"
                    };
                }
            }

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<생산계획작업지시현황>> 생산계획생산지시현황_조회(string 생산계획코드, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.생산지시정보
                .Include(x => x.생산계획)
                .Where(x => x.생산계획코드 == 생산계획코드)
                .Where_미삭제()
                .Select(x => new 생산계획작업지시현황
                {
                    작업지시코드 = x.생산지시코드,
                    작업지시명 = x.생산지시명,
                    생산지시유형코드 = x.생산지시유형코드,
                    실행상태코드 = x.실행상태코드,
                    시작일 = x.시작일,
                    완료목표일 = x.완료목표일,
                    목표수량 = x.생산수량,
                    양산수량 = 0,
                    불량수량 = 0
                })
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }



        /*//
        public Task<string> 작업지시_완제품_발행(string 작업지시코드, int 공정차수, 품목정보 품목)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyMMdd = now.ToString("yyMMdd");
            var no = dc.보유품목정보.Count(x => x.품목코드 == 품목.품목코드 && x.보유년월일 == yyMMdd) + 1;
            var info = new 보유품목정보
            {
                보유품목코드 = $"{품목.품목코드}:{yyMMdd}:{no}",
                품목코드 = 품목.품목코드,
                보유년월일 = yyMMdd,
                순번 = no,
                보유일 = now
            };
            dc.보유품목정보.Add(info);

            var 보유품목발행이력 = new 보유품목발행이력
            {
                보유품목코드 = info.보유품목코드,
                작업지시코드 = 작업지시코드,
                공정차수 = 공정차수
            };
            dc.보유품목발행이력.Add(보유품목발행이력);

            dc.SaveChanges();

            return Task.FromResult(info.보유품목코드);
        }

        public Task<보유품목정보> 작업지시_완제품_조회(string 작업지시코드, int 공정차수)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var info = dc.보유품목발행이력
                .Include(x => x.보유품목)
                .FirstOrDefault(x => x.작업지시코드 == 작업지시코드 && x.공정차수 == 공정차수);

            return Task.FromResult(info?.보유품목);
        }
        //*/
        ///////////////////////////////// 2021.02.08  작업지시품질검사 엑셀 export 작업

        public Task<IEnumerable<생산지시정보>> 작업지시품질검사_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            IEnumerable<생산지시정보> ienum;
            List<생산지시정보> list = new List<생산지시정보>();

            var result = dc.생산지시정보
                .Include(x => x.생산계획).ThenInclude(x => x.발주처)
                .Include(x => x.생산계획).ThenInclude(x => x.생산품)
                .Include(x => x.생산계획).ThenInclude(x => x.생산품공정).ThenInclude(x => x.생산품)
                .Include(x => x.생산계획).ThenInclude(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정)
                .Include(x => x.생산계획).ThenInclude(x => x.생산품공정).ThenInclude(a => a.생산품공정차수목록).ThenInclude(b => b.공정단위).ThenInclude(c => c.공정검사목록).ThenInclude(d => d.품질검사)
                .Include(x => x.생산계획).ThenInclude(x => x.생산책임자)
                .Where(x => x.회사코드 == 회사코드)
                //.Include(x => x.생산지시공정차수목록.OrderBy(x => x.공정차수)).ThenInclude(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.공정)
                //.Include(x => x.생산지시공정차수목록.OrderBy(x => x.공정차수)).ThenInclude(x => x.생산품공정차수).ThenInclude(x => x.생산품공정)
                //.Include(x => x.생산지시공정차수목록).ThenInclude(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.도면)
                //.Include(x => x.생산지시공정차수목록).ThenInclude(x => x.작업자)
                .Where_미삭제()
                .ToList();
            //if (result != null)
            //{
            //    if (result.생산지시공정차수목록.Count > 0)
            //    {
            //        result.생산지시공정차수목록 = dc.생산품공정차수정보
            //            .Where_미삭제_사용()
            //            .Where(x => x.생산품공정코드 == result.생산계획.생산품공정코드)
            //            .OrderBy(x => x.공정차수)
            //            .ToList()
            //            .Select(x => new 생산지시공정차수정보
            //            {
            //                생산지시코드 = result.생산지시코드,
            //                공정차수 = x.공정차수,
            //                생산품공정코드 = x.생산품공정코드,
            //                생산품공정차수 = result.생산계획.생산품공정.생산품공정차수목록.FirstOrDefault(y => y.생산품공정코드 == x.생산품공정코드 && y.공정차수 == x.공정차수 && y.공정단위.공정.공정유형코드 == "B0205"),
            //            }).ToList();
            //    }
            //    list.Add(result);
            //}


            //ienum = (IEnumerable<생산지시정보>)list.AsEnumerable();

            return Task.FromResult(result.AsEnumerable());
        }



        public Task<IEnumerable<작업지시공정현황>> 작업지시품질검사현황_조회(string 생산지시코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            //int 양산수량 = dc.품질검사측정정보.Where(x => x.합격여부 == "합격").Count();
            //int 불량수량 = dc.품질검사측정정보.Where(x => x.합격여부 == "불합격").Count();


            var list = dc.생산지시공정차수
                .Include(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.공정품)
                .Include(x => x.생산지시)
                .Where(x => x.생산지시코드 == 생산지시코드)
                .Where(x => x.생산품공정차수.공정단위.공정.공정유형코드 == "B0205")
                .Where_미삭제()
                .OrderBy(x => x.공정차수)
                .Select(x => new 작업지시공정현황
                {
                    생산지시코드 = x.생산지시코드,
                    공정차수 = x.공정차수,
                    공정명 = x.생산품공정차수.공정단위.공정단위명,
                    공정품명 = x.생산품공정차수.생산품공정.생산품코드,
                    시작일 = x.생산지시.시작일,
                    완료목표일 = x.생산지시.완료목표일,
                    목표수량 = x.생산지시.생산수량,
                    //검사수량 = dc.품질검사측정정보.Where(x => x.합격여부 != "").GroupBy(m => m.시리얼넘버).Count(),
                    //합격수량 = dc.품질검사측정정보.Where(x => x.합격여부 == "합격").GroupBy(m => m.시리얼넘버).Count(),
                    //불량수량 = dc.품질검사측정정보.Where(x => x.합격여부 == "불합격").GroupBy(m => m.시리얼넘버).Count(),

                    검사수량 = x.생산지시.검사수량,
                    합격수량 = x.생산지시.합격수량,
                    불량수량 = x.생산지시.불량수량,

                    //합격률 = Convert.ToInt32((x.생산지시.합격수량 / x.생산지시.검사수량) * 100),
                    불량률 = (x.생산지시.불량수량 > 0 && x.생산지시.검사수량 > 0) ? Convert.ToInt32((x.생산지시.불량수량 / x.생산지시.검사수량) * 100) : 0,
                    공정단위코드 = x.생산품공정차수.공정단위코드,
                }).ToList();

            //.FirstOrDefault(x => x.공정단위코드 == "PU0001:1");

            return Task.FromResult(list.AsEnumerable());
        }


        public Task<IEnumerable<품질검사측정정보>> 작업지시품질검사항목_조회(string 생산지시코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.품질검사측정정보
                .Include(x => x.품질검사)
                .Where_미삭제_사용()
                .Where(x => x.생산지시코드 == 생산지시코드).ToList();

            return Task.FromResult(result.AsEnumerable());

        }

        public Task<List<품질검사측정정보>> 작업지시품질검사품목_조회(string 생산지시코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.품질검사측정정보
                .Include(x => x.품질검사)
                .Where_미삭제_사용()
                .Where(x => x.생산지시코드 == 생산지시코드).ToList();


            return Task.FromResult(result.ToList());


        }

        ///////////////////////////////////////////////////////////////////////////////
        ///

        // [추가 ] 2021.03.17 

        public Task 공정이력정보_저장(생산지시정보 info, 생산지시공정차수정보 info2, int 생산수량, string 공정상태, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var 공정이력정보 = new 공정이력정보
            {
                회사코드 = info.회사코드,
                생산지시코드 = info.생산지시코드,
                생산지시명 = info.생산지시명,
                공정단위코드 = info2.생산품공정차수.공정단위코드,
                //설비코드 = info.생산계획.생산품코드,
                //20210514 설비코드 수정
                설비코드 = info2.생산품공정차수.공정단위.공정설비목록.Count > 0 ? info2.생산품공정차수.공정단위.공정설비목록[0].설비코드 : null,
                생산품공정코드 = info.생산계획.생산품공정코드,
                작업자사번 = info2.작업자사번,
                공정상태 = 공정상태,
                생산수량 = 생산수량,
                목표수량 = info.생산수량,
                시작일 = info.시작일,
                완료목표일 = info.완료목표일,
            };

            if (isAdd == true)
                dc.공정이력정보.Add(공정이력정보);
            else
                dc.공정이력정보.Update(공정이력정보);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        // [추가 ] 2021.03.22
        // 

        public Task<IEnumerable<공정이력정보>> 작업지시공정이력현황_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정이력정보
                //.Include(x => x.보유품목)
                .Include(x => x.생산품공정)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(result.AsEnumerable());

        }

        public Task<IEnumerable<공정이력상세정보>> 작업지시공정이력상세_조회(int 공정이력인덱스)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정이력상세정보
                .Include(x => x.작업자)
                .Where(x => x.공정이력인덱스 == 공정이력인덱스)
                .Where_미삭제_사용()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(result.AsEnumerable());

        }


        public Task<공정이력정보> 작업지시공정완료이력현황_조회(string 회사코드, string 생산지시코드 , string 공정단위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정이력정보
                .Include(x => x.생산품공정)
                .Where(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드 && x.공정단위코드 == 공정단위코드).FirstOrDefault();

            return Task.FromResult(result);

        }



        public Task<bool> 작업생산실적_저장(생산실적헤더정보 헤더, 생산실적상세정보 상세, List<공정단위자재현황> listBom)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                var 생산지시유무 = dc.생산실적헤더정보.Where(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드  ).FirstOrDefault();

                //var 작업순번 = dc.생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드 ) + 1;
                var 작업자생산실적 = new 작업자생산실적정보
                {
                    회사코드 = 헤더.회사코드,
                    생산지시코드 = 헤더.생산지시코드,
                    생산지시명 = 헤더.생산지시명,
                    작업순번 = dc.작업자생산실적정보.Count(x => x.회사코드 == 헤더.회사코드) + 1,
                    공정단위코드 = 헤더.공정단위코드,
                    작업자사번 = 상세.작업자사번,
                    생산품코드 = 헤더.생산품코드,
                    실적수량 = 헤더.실적수량,
                    불량수량 = 헤더.불량수량,
                    실적등록일 = 상세.실적등록일,

                };
                dc.작업자생산실적정보.Add(작업자생산실적);

                if (생산지시유무 == null)
                    dc.생산실적헤더정보.Add(헤더);
                else
                {
                    생산지시유무.실적수량 =  헤더.실적수량;
                    생산지시유무.불량수량 =  헤더.불량수량;

                    dc.생산실적헤더정보.Update(생산지시유무);
                }

                dc.SaveChanges();
                foreach (var item in listBom)
                {
                    var 작업순번 = dc.생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드) + 1;
                    상세.사용수량 = item.필요수량 * 헤더.실적수량;
                    상세.불량수량 = item.필요수량 * 헤더.불량수량;
                    상세.사용품번 = item.자재코드;

                    상세.작업순번 = 작업순번;
                    dc.생산실적상세정보.Add(상세);

                    dc.SaveChanges();
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }



            return Task.FromResult(result);
        }



        public Task<IEnumerable<생산지시정보>> 재고이동작업지시_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산지시정보
                .Include(x => x.생산계획).Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .Where(x => x.실행상태코드 == "B2001").ToList();


            return Task.FromResult(result.AsEnumerable());
        }


        public Task 공정단위자재BOM정보_저장(List<공정단위자재정보> info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            foreach (var item in info)
            {
                var 공정단위자재정보 = new 공정단위자재정보
                {
                    회사코드 = item.회사코드,
                    공정단위코드 = item.공정단위코드,
                    수량 = item.수량,
                    자재코드 = item.자재코드,

                };
                dc.공정단위자재정보.Add(공정단위자재정보);
            }

            //if (isAdd == true)
            //    dc.공정단위자재정보.Add(info);
            //else
            //    dc.공정단위자재정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task BOM정보를공정단위자재_저장(List<공정단위자재정보> info, IList<공정단위자재정보> remove)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            foreach (var item in remove)
            {
                var 공정단위자재삭제 = new 공정단위자재정보
                {
                    회사코드 = item.회사코드,
                    공정단위코드 = item.공정단위코드,
                    자재코드 = item.자재코드,
                };
                dc.공정단위자재정보.Remove(공정단위자재삭제);
                dc.SaveChanges();
            }


            foreach (var item in info)
            {
                var 공정단위자재등록 = new 공정단위자재정보
                {
                    회사코드 = item.회사코드,
                    공정단위코드 = item.공정단위코드,
                    수량 = item.수량,
                    자재코드 = item.자재코드,

                };

                dc.공정단위자재정보.Add(공정단위자재등록);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }





        public Task<List<생산실적헤더정보>> 작업지시공정별실적현황_조회(string 회사코드, 작업지시공정현황 작업지시공정)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산실적헤더정보
                .Where(x => x.회사코드 == 회사코드 && x.공정단위코드 == 작업지시공정.공정단위코드 && x.생산지시코드 == 작업지시공정.생산지시코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);

        }


        public Task<IEnumerable<생산지시정보>> 작업지시기준_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산지시정보
                .Include(x => x.생산계획).Where(x => x.회사코드 == 회사코드)
                .Where_사용()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task<IEnumerable<작업지시공정현황>> 작업지시공정현황_조회(string 회사코드, string 생산지시코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.생산지시공정차수
                .Include(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.공정품)
                .Include(x => x.생산지시)
                .Where(x => x.생산지시코드 == 생산지시코드 && x.회사코드 == 회사코드)
                .Where_미삭제()
                .OrderBy(x => x.공정차수)
                .Select(x => new 작업지시공정현황
                {
                    생산지시코드 = x.생산지시코드,
                    공정차수 = x.공정차수,
                    공정단위코드 = x.생산품공정차수.공정단위코드,
                    공정코드 = x.생산품공정차수.공정단위.공정코드,
                    공정명 = x.생산품공정차수.공정단위.공정단위명,
                    공정품명 = x.생산품공정차수.공정단위.공정품.품목명,
                    시작일 = x.생산지시.시작일,
                    완료목표일 = x.생산지시.완료목표일,
                    목표수량 = x.생산지시.생산수량,
                    양산수량 = 0,
                    검사수량 = x.생산지시.검사수량,
                    합격수량 = x.생산지시.합격수량,
                    불량수량 = x.생산지시.불량수량,
                    불량률 = (x.생산지시.불량수량 > 0 && x.생산지시.검사수량 > 0) ? Convert.ToInt32((x.생산지시.불량수량 / x.생산지시.검사수량) * 100) : 0,
                })
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }



        public Task<IEnumerable<작업지시공정현황>> 작업지시공정별품질검사현황_조회(string 회사코드, 작업지시공정현황 작업지시공정)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.생산지시공정차수
                 .Include(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.공정품)
                 .Include(x => x.생산지시)
                 .Where(x => x.생산지시코드 == 작업지시공정.생산지시코드 && x.회사코드 == 회사코드)
                 .Where(x => x.생산품공정차수.공정단위.공정.공정유형코드 == "B0205")
                 .Where_미삭제()
                 .OrderBy(x => x.공정차수)
                 .Select(x => new 작업지시공정현황
                 {
                     생산지시코드 = x.생산지시코드,
                     공정차수 = x.공정차수,
                     공정명 = x.생산품공정차수.공정단위.공정단위명,
                     공정품명 = x.생산품공정차수.생산품공정.생산품코드,
                     시작일 = x.생산지시.시작일,
                     완료목표일 = x.생산지시.완료목표일,
                     목표수량 = x.생산지시.생산수량,

                     검사수량 = x.생산지시.검사수량,
                     합격수량 = x.생산지시.합격수량,
                     불량수량 = x.생산지시.불량수량,
                     //합격률 = Convert.ToInt32((x.생산지시.합격수량 / x.생산지시.검사수량) * 100),
                     불량률 = (x.생산지시.불량수량 > 0 && x.생산지시.검사수량 > 0) ? Convert.ToInt32((x.생산지시.불량수량 / x.생산지시.검사수량) * 100) : 0,
                     공정단위코드 = x.생산품공정차수.공정단위코드,
                 }).ToList();

            //.FirstOrDefault(x => x.공정단위코드 == "PU0001:1");
            return Task.FromResult(list.AsEnumerable());

        }

        public Task<IEnumerable<생산지시정보>> 작업지시Action_조회(string 회사코드, 검색정보 검색)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var 기준day = now.AddDays(-7);

            var query1 = dc.생산지시정보
                .Include(x => x.생산계획).Where(x => x.회사코드 == 회사코드);
            var query2 = query1.Where_미삭제();
            if (검색?.유무(검색대상.사용) == true)
                query2 = query2.Where_사용();
            if (검색?.유무(검색대상.상태) == true)
                query2 = query2.Where(x => x.실행상태코드 == 검색[검색대상.상태] && (DateTime.Compare(기준day, x.CreateTime) < 0));

            var result = query2.ToList();

            return Task.FromResult(result.AsEnumerable());
        }





        public Task<List<작업지시공정현황>> 작업지시별실적현황_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.생산지시공정차수
                .Include(x => x.생산품공정차수).ThenInclude(x => x.공정단위).ThenInclude(x => x.공정품)
                .Include(x => x.생산지시)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .OrderBy(x => x.공정차수)
                .Select(x => new 작업지시공정현황
                {
                    생산지시코드 = x.생산지시코드,
                    생산지시명 = x.생산지시.생산지시명,
                    공정차수 = x.공정차수,
                    공정단위코드 = x.생산품공정차수.공정단위코드,
                    공정코드 = x.생산품공정차수.공정단위.공정코드,
                    공정명 = x.생산품공정차수.공정단위.공정단위명,
                    공정품명 = x.생산품공정차수.공정단위.공정품.품목명,
                    시작일 = x.생산지시.시작일,
                    완료목표일 = x.생산지시.완료목표일,
                    목표수량 = x.생산지시.생산수량,
                    양산수량 = 0,
                    검사수량 = x.생산지시.검사수량,
                    합격수량 = x.생산지시.합격수량,
                    불량수량 = x.생산지시.불량수량,
                    불량률 = (x.생산지시.불량수량 > 0 && x.생산지시.검사수량 > 0) ? Convert.ToInt32((x.생산지시.불량수량 / x.생산지시.검사수량) * 100) : 0,
                })
                .ToList();

            return Task.FromResult(list);
        }


        public Task<List<작업지시생산실적현황>> 작업지시별생산실적현황_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산실적헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();


            var result2 = result.GroupBy(x => new { x.생산지시코드, x.회사코드 }).Select(g => g.First()).ToList();

            List<작업지시생산실적현황> 작업지시생산실적 = new List<작업지시생산실적현황>();
            foreach (var item in result2)
            {
                var 수량 = dc.생산지시정보.Where(x => x.생산지시코드 == item.생산지시코드 && x.회사코드 == 회사코드).FirstOrDefault();
                var 작업지시생산실적현황 = new 작업지시생산실적현황
                {
                    생산지시코드 = item.생산지시코드,
                    생산지시명 = item.생산지시명,
                    생산품코드 = item.생산품코드,
                    공정단위코드 = item.공정단위코드,
                    생산품공정코드 = item.생산품공정코드,
                    사업장코드 = item.사업장코드,
                    실적공정코드_창고코드 = item.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = item.실적작업장코드_장소코드,
                    재작업여부 = item.재작업여부,
                    LOTNO = item.LOTNO,
                    일괄생산등록유무 = item.일괄생산등록유무,
                    작업번호 = item.작업번호,
                    목표수량 = 수량 != null ? 수량.생산수량 : 0,
                    실적수량 = item.실적수량,
                    불량수량 = item.불량수량,

                };
                작업지시생산실적.Add(작업지시생산실적현황);
            }

            return Task.FromResult(작업지시생산실적.ToList());

        }



        public Task<bool> 공정이력완료정보_저장(공정이력정보 공정이력정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            try
            {
                dc.공정이력정보.Add(공정이력정보);

                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }


        public Task NEW공정단위_저장(공정단위정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            string 공정유형코드 = info.공정 != null ? info.공정.공정유형코드 : "";
            // track 방지 {{{
            info.공정품코드 = info.공정품?.품목코드;
            info.공정품 = null;
            info.완제품코드 = info.완제품?.품목코드;
            info.완제품 = null;
            info.도면코드 = info.도면?.도면코드;
            info.도면 = null;
            info.공정코드 = info.공정?.공정코드;
            info.공정 = null;
            info.공정자재목록 = null;
            info.공정설비목록 = null;
            info.공정품유형 = null;
            info.공정자재목록 = null;
            info.공정설비목록 = null;
            info.공정검사목록 = null;
            // }}}

            if (isAdd == true)
            {
                var max원공정단위코드 = dc.공정단위정보.DefaultIfEmpty().Max(x => x.원공정단위코드);
                var lastCount = 0;
                if (max원공정단위코드 != default)
                    lastCount = int.Parse(max원공정단위코드[2..]);
                info.원공정단위코드 = $"PU{lastCount + 1:0000}";
                info.공정단위코드 = $"{info.원공정단위코드}:{info.관리차수}";
                dc.공정단위정보.Add(info);
            }
            else
                dc.공정단위정보.Update(info);

            dc.SaveChanges();

            //공정유형이 검사가 아닌경우  공정유형코드 검사  "B0205" 

            if (isAdd == true)
            {
                var 공정자재목록 = dc.BOM_정보.Where(x => x.모품번 == info.공정품코드 && x.회사코드 == info.회사코드).ToList();

                foreach (var item in 공정자재목록)
                {
                    var 공정단위자재 = new 공정단위자재정보
                    {
                        회사코드 = info.회사코드,
                        공정단위코드 = info.공정단위코드,
                        자재코드 = item.자품번,
                        수량 = item.필요수량,
                    };
                    dc.공정단위자재정보.Add(공정단위자재);
                }
            }
            else
            {
                var 공정자재삭제목록 = dc.공정단위자재정보.Where(x => x.공정단위코드 == info.공정단위코드 && x.회사코드 == info.회사코드).FirstOrDefault();
                if (공정자재삭제목록 != null)
                    dc.공정단위자재정보.Remove(공정자재삭제목록);

                var 공정자재목록 = dc.BOM_정보.Where(x => x.모품번 == info.공정품코드 && x.회사코드 == info.회사코드).ToList();

                foreach (var item in 공정자재목록)
                {
                    var 공정단위자재 = new 공정단위자재정보
                    {
                        회사코드 = info.회사코드,
                        공정단위코드 = info.공정단위코드,
                        자재코드 = item.자품번,
                        수량 = item.필요수량,
                    };
                    dc.공정단위자재정보.Add(공정단위자재);
                }
            }

            dc.SaveChanges();


            return Task.CompletedTask;
        }




        public Task<bool> 생산제품입고처리_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            try
            {
                DateTime 보유년월일 = DateTime.Now;
                var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == 생산실적헤더.생산품코드 && x.회사코드 == 생산실적헤더.회사코드).FirstOrDefault();
                var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 생산실적헤더.생산품코드).FirstOrDefault();

                string 입고장소위치코드 = "";


               if (품목정보.품목구분코드 == "B1202" || 품목정보.품목구분코드 == "B1204")
                    입고장소위치코드 = "10001000";
                else if (품목정보.품목구분코드 == "B1203")
                    입고장소위치코드 = "10001001";

                if (보유품목 == null)
                {
                    var info = new 보유품목정보
                    {
                        회사코드 = 생산실적헤더.회사코드,
                        보유품목코드 = 생산실적헤더.생산품코드,
                        품목코드 = 생산실적헤더.생산품코드,
                        보유년월일 = 보유년월일.ToString("yyyyMMdd"),
                        조정년월일 = null,
                        보유일 = 보유년월일,
                        순번 = 1,
                        수량 = 생산실적헤더.실적수량 ,
                        실제수량 = 생산실적헤더.실적수량,
                        품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                        장소코드 = "1000",
                        장소위치코드 = 입고장소위치코드,
                        LOT번호 = 생산실적헤더.LOTNO,
                        품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}"
                    };
                    dc.보유품목정보.Add(info);
                }
                else
                {
                    //보유품목.LOT번호 = 생산실적헤더.LOTNO;
                    //보유품목.품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}";
                    보유품목.장소코드 = "1000";
                    보유품목.장소위치코드 = 입고장소위치코드;
                    보유품목.수량 = 보유품목.수량 + 생산실적헤더.실적수량;
                    보유품목.실제수량 = 보유품목.수량 + 생산실적헤더.실적수량;
                    dc.보유품목정보.Update(보유품목);
                }

                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            result = true;
            return Task.FromResult(result);
        }


        public Task<bool> 생산제품_위치등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 생산실적헤더.생산품코드).FirstOrDefault();
                string 입고장소위치코드 = "";


                if (품목정보.품목구분코드 == "B1202" || 품목정보.품목구분코드 == "B1204")
                    입고장소위치코드 = "10001000";
                else if (품목정보.품목구분코드 == "B1203")
                    입고장소위치코드 = "10001001";

                var 장소위치코드 = 입고장소위치코드;
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 생산실적헤더.생산품코드);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                var info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 생산실적헤더.생산품코드 && x.장소위치코드 == 장소위치코드 );

                if (info == default)
                {
                    info = new 보유품목위치정보
                    {
                        회사코드 = 생산실적헤더.회사코드,
                        보유품목코드 = 생산실적헤더.생산품코드,
                        장소위치코드 = 장소위치코드,
                        수량 = 생산실적헤더.실적수량,
                        LOT번호 = 생산실적헤더.LOTNO,
                        품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}",
                    };
                    dc.보유품목위치정보.Add(info);
                }
                // 변경
                else
                {
                    info.수량 = info.수량 + 생산실적헤더.실적수량;
                    info.LOT번호 = 생산실적헤더.LOTNO;
                    info.품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}";
                    dc.보유품목위치정보.Update(info);
                }

                var info외주 = new 외주생산위치정보
                {
                    회사코드 = 생산실적헤더.회사코드,
                    보유품목코드 = 생산실적헤더.생산품코드,
                    장소위치코드 = "10001000",
                    수량 = 생산실적헤더.실적수량,
                    LOT번호 = 생산실적헤더.LOTNO,
                    품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}",
                    위치상세코드 = null,
                    사유 = "공정이동",
                    지시서 = 생산실적헤더.주문번호,

                };
                dc.외주생산위치정보.Add(info외주);

                dc.SaveChanges();

                #region 불량품 창고넣기

                if (생산실적헤더.불량수량 > 0)
                {
                    string 불량품창고 = "10001009";
                    //불량품등록
                    var info2 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 생산실적헤더.생산품코드 && x.장소위치코드 == 불량품창고);

                    if (info2 == default)
                    {
                        info2 = new 보유품목위치정보
                        {
                            회사코드 = 생산실적헤더.회사코드,
                            보유품목코드 = 생산실적헤더.생산품코드,
                            장소위치코드 = 불량품창고,
                            수량 = 생산실적헤더.불량수량,
                            LOT번호 = 생산실적헤더.LOTNO,
                            품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}",
                        };
                        dc.보유품목위치정보.Add(info2);
                    }
                    // 변경
                    else
                    {
                        info2.수량 = info2.수량 + 생산실적헤더.불량수량;
                        info2.LOT번호 = 생산실적헤더.LOTNO;
                        info2.품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}";
                        dc.보유품목위치정보.Update(info2);
                    }

                    dc.SaveChanges();
                }
                #endregion


                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 생산실적헤더.생산품코드 && x.장소위치코드 == 장소위치코드);

                var log = new 보유품목이력
                {
                    회사코드 = 생산실적헤더.회사코드,
                    보유품목코드 = 생산실적헤더.생산품코드,
                    연계보유품목코드 = 생산실적헤더.생산품코드,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = "1000",
                    장소위치코드 = 장소위치코드,
                    변경수량 = 생산실적헤더.실적수량,
                    변경사유 = "생산입고",
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now,
                    LOT번호 = 생산실적헤더.LOTNO,
                    품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}",
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                if (보유이력 == null)
                    dc.보유품목이력.Add(log);

                dc.SaveChanges();


                var 바코드발급 = dc.바코드발급정보
                  .Include(x => x.품목)
                  .Where(x => x.품목코드 == 생산실적헤더.생산품코드 && x.LOT번호 == 생산실적헤더.LOTNO && x.회사코드 == 생산실적헤더.회사코드)
                  .FirstOrDefault();
                if (바코드발급 != default)
                {
                    바코드발급.입고유무 = true;
                    바코드발급.입고일자 = DateTime.Now.ToString("yyyyMMdd");
                    dc.바코드발급정보.Update(바코드발급);
                    dc.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

            return Task.FromResult(result);

        }


        public Task<bool> 더존_생산실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 생산실적헤더.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";

                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 생산실적헤더.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DIV_CD = 생산실적헤더.사업장코드,
                    DEPT_CD = 생산실적상세.부서코드,
                    EMP_CD = 생산실적상세.작업자사번,
                    BASELOC_CD = 생산실적헤더.실적공정코드_창고코드,
                    LOC_CD = 생산실적헤더.실적작업장코드_장소코드,
                    REWORK_YN = 생산실적헤더.재작업여부,
                    PITEM_CD = 생산실적헤더.생산품코드,
                    ITEM_QT = 생산실적헤더.실적수량,
                    BASELOC_FG = 생산실적헤더.실적구분,
                    WR_WH_CD = 생산실적헤더.실적공정코드,
                    WR_LC_CD = 생산실적헤더.실적작업장코드,
                    PLN_CD = "",
                    REMARK_DC = "",
                    LOT_NB = 생산실적헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 생산실적상세.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 생산실적상세.실적등록일,
                    실적일자 = 생산실적상세.실적등록일,
                    사업장코드 = 생산실적헤더.사업장코드,
                    부서코드 = 생산실적상세.부서코드,
                    사원코드 = 생산실적상세.작업자사번,
                    실적공정코드_창고코드 = 생산실적헤더.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 생산실적헤더.실적작업장코드_장소코드,
                    재작업여부 = 생산실적헤더.재작업여부,
                    실적품번 = 생산실적헤더.생산품코드,
                    실적수량 = 생산실적헤더.실적수량,
                    실적구분 = 생산실적헤더.실적구분,
                    실적공정코드 = 생산실적헤더.실적공정코드,
                    실적작업장코드 = 생산실적헤더.실적작업장코드,
                    작업자코드 = "",
                    비고 = "",
                    LOTNO = 생산실적상세.LOT번호,
                };

                var 생산실적헤더Data = dc.생산실적헤더정보.Where(x => x.회사코드 == 생산실적헤더.회사코드 && x.생산지시코드 == 생산실적헤더.생산지시코드).FirstOrDefault();
                if (생산실적헤더Data != null)
                {
                    생산실적헤더Data.일괄생산등록유무 = "1";
                    생산실적헤더Data.작업번호 = 작업번호1;
                }

                dc.생산실적헤더정보.Update(생산실적헤더Data);

                dc.Add(일괄생산실적헤더정보);
                dc_D.Add(BARPLUS_LPRODUCTION);

                dc.SaveChanges();

                dc_D.SaveChanges();

                더존_생산실적상세정보_등록(생산실적헤더, 생산실적상세, 일괄생산실적헤더정보, listBom);

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }

        public void 더존_생산실적상세정보_등록(생산실적헤더정보 생산실적헤더정보, 생산실적상세정보 생산실적상세, 일괄생산실적헤더정보 일괄생산실적헤더, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

           
                try
                {
                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 
                && x.지시서 == 생산실적상세.주문번호 && x.사유 == "0" && x.보유품목코드 == listBom[0].자재코드).ToList();


                foreach (var item in 외주생산위치정보)
                {
                    var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 생산실적상세.회사코드 && x.작업번호 == 일괄생산실적헤더.작업번호) + 1;

                    var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                    {
                        CO_CD = 일괄생산실적헤더.회사코드,
                        WORK_NB = 일괄생산실적헤더.작업번호,
                        WORK_SQ = 순번.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                        CITEM_CD = item.보유품목코드,
                        USE_QT = listBom[0].사용수량,
                        BASELOC_CD = 생산실적상세.사용공정_사용창고,
                        LOC_CD = 생산실적상세.사용작업장_사용장소,
                        LOT_NB = item.LOT번호,
                        BASELOC_FG = "1",

                    };

                    var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                    {
                        회사코드 = 일괄생산실적헤더.회사코드,
                        작업번호 = 일괄생산실적헤더.작업번호,
                        작업순번 = 순번.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                        사용품번 = item.보유품목코드,
                        사용수량 = listBom[0].사용수량,

                        LOTNO = item.LOT번호,
                        사용공정_사용창고 = 생산실적상세.사용공정_사용창고,
                        사용작업장_사용장소 = 생산실적상세.사용작업장_사용장소,
                        창고구분 = "1",

                    };

                    dc_D.Add(BARPLUS_LPRODUCTION_D);
                    dc.Add(일괄생산실적상세정보);

                    // 보유품목등록
                    // LOT번호 부여
                    // 위치등록
                    // 보유품목이력
                    dc.SaveChanges();

                    dc_D.SaveChanges();
                }


                var 생산실적상세정보 = dc.생산실적상세정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.생산지시코드 == 생산실적상세.생산지시코드).ToList();
                if (생산실적상세정보 != null)
                {
                    foreach(var item in 생산실적상세정보)
                    {
                        item.일괄생산등록유무 = "1";
                        dc.생산실적상세정보.Update(item);
                    }
                }

                dc.SaveChanges();
                result = true;

            }
                catch (Exception ex)
                {
                    result = false;
                }

            result = true;

        }

        public Task<bool> 더존_불량실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 생산실적헤더.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";
                string 불량창고 = "1009";
                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 생산실적헤더.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DIV_CD = 생산실적헤더.사업장코드,
                    DEPT_CD = 생산실적상세.부서코드,
                    EMP_CD = 생산실적상세.작업자사번,
                    BASELOC_CD = 생산실적헤더.실적공정코드_창고코드,
                    LOC_CD = 불량창고,
                    REWORK_YN = 생산실적헤더.재작업여부,
                    PITEM_CD = 생산실적헤더.생산품코드,
                    ITEM_QT = 생산실적헤더.불량수량,
                    BASELOC_FG = 생산실적헤더.실적구분,
                    WR_WH_CD = 생산실적헤더.실적공정코드,
                    WR_LC_CD = 불량창고,
                    PLN_CD = "",
                    REMARK_DC = "불량",
                    LOT_NB = 생산실적헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 생산실적상세.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 생산실적상세.실적등록일,
                    실적일자 = 생산실적상세.실적등록일,
                    사업장코드 = 생산실적헤더.사업장코드,
                    부서코드 = 생산실적상세.부서코드,
                    사원코드 = 생산실적상세.작업자사번,
                    실적공정코드_창고코드 = 생산실적헤더.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 불량창고,
                    재작업여부 = 생산실적헤더.재작업여부,
                    실적품번 = 생산실적헤더.생산품코드,
                    실적수량 = 생산실적헤더.불량수량,
                    실적구분 = 생산실적헤더.실적구분,
                    실적공정코드 = 생산실적헤더.실적공정코드,
                    실적작업장코드 = 불량창고,
                    작업자코드 = "",
                    비고 = "불량",
                    LOTNO = 생산실적상세.LOT번호,
                };


                if (생산실적헤더.불량수량 > 0)
                {
                    dc.Add(일괄생산실적헤더정보);
                    dc_D.Add(BARPLUS_LPRODUCTION);
                    dc.SaveChanges();
                    dc_D.SaveChanges();
                }


                var 생산실적헤더Data = dc.생산실적헤더정보.Where(x => x.회사코드 == 생산실적헤더.회사코드 && x.생산지시코드 == 생산실적헤더.생산지시코드).FirstOrDefault();
                if (생산실적헤더Data != null)
                {
                    생산실적헤더Data.일괄생산등록유무 = "1";
                    생산실적헤더Data.작업번호 = 작업번호1;
                    dc.생산실적헤더정보.Update(생산실적헤더Data);
                    dc.SaveChanges();
                }

               
                더존_불량실적상세정보_등록(생산실적헤더, 생산실적상세, 일괄생산실적헤더정보, listBom);

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }

        public void 더존_불량실적상세정보_등록(생산실적헤더정보 생산실적헤더정보, 생산실적상세정보 생산실적상세, 일괄생산실적헤더정보 일괄생산실적헤더, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            

            try
            {
                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호
                && x.사유 == "0" && x.보유품목코드 == listBom[0].자재코드 ).ToList();

                if (listBom[0].불량수량 > 0)
                {
                    foreach (var item in 외주생산위치정보)
                    {
                        var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 생산실적상세.회사코드 && x.작업번호 == 일괄생산실적헤더.작업번호) + 1;

                        var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                        {
                            CO_CD = 일괄생산실적헤더.회사코드,
                            WORK_NB = 일괄생산실적헤더.작업번호,
                            WORK_SQ = 순번.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                            CITEM_CD = item.보유품목코드,

                            USE_QT = listBom[0].불량수량,
                            BASELOC_CD = 생산실적상세.사용공정_사용창고,
                            LOC_CD = 생산실적상세.사용작업장_사용장소,
                            LOT_NB = item.LOT번호,
                            BASELOC_FG = "1",
                            REMARK_DC = "불량",
                        };

                        var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                        {
                            회사코드 = 일괄생산실적헤더.회사코드,
                            작업번호 = 일괄생산실적헤더.작업번호,
                            작업순번 = 순번.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                            사용품번 = item.보유품목코드,
                            사용수량 = listBom[0].불량수량,

                            LOTNO = 일괄생산실적헤더.LOTNO,
                            사용공정_사용창고 = 생산실적상세.사용공정_사용창고,
                            사용작업장_사용장소 = 생산실적상세.사용작업장_사용장소,
                            창고구분 = "1",
                            비고 = "불량",

                        };

                        if (listBom[0].불량수량 > 0)
                        {
                            dc_D.Add(BARPLUS_LPRODUCTION_D);
                            dc.Add(일괄생산실적상세정보);
                            dc.SaveChanges();
                            dc_D.SaveChanges();
                        }
                    }
                }
                // 보유품목등록
                // LOT번호 부여
                // 위치등록
                // 보유품목이력
                string 위치상세코드 = 일괄생산실적헤더.실적공정코드_창고코드 + 일괄생산실적헤더.실적작업장코드_장소코드;

                foreach ( var item in 외주생산위치정보)
                {
                    var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == item.보유품목코드 && x.회사코드 == 생산실적상세.회사코드).FirstOrDefault();
                    if (보유품목 != null)
                    {
                        if (보유품목.수량 > 0)
                        {
                            보유품목.수량 = 보유품목.수량 - item.수량;
                        }
                        if (보유품목.수량 < 0)
                        {
                            보유품목.수량 = 0;
                        }

                        dc.보유품목정보.Update(보유품목);
                    }

                    var 보유품목위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == item.보유품목코드 && x.회사코드 == item.회사코드 && x.위치상세코드 == 위치상세코드).FirstOrDefault();

                    if (보유품목위치 != null)
                    {
                        if (보유품목위치.수량 > 0)
                        {
                            보유품목위치.수량 = 보유품목위치.수량 - item.수량;

                            if (보유품목위치.수량 < 0)
                            {
                                보유품목위치.수량 = 0;
                            }
                        }

                        dc.보유품목위치정보.Update(보유품목위치);
                    }
                }  


                
               

                var 외주생산위치 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호 && x.사유 == "0" && x.보유품목코드 == listBom[0].자재코드).FirstOrDefault();
                if (외주생산위치 != null)
                {
                    var 외주생산위치삭제 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호 && x.사유 == "0" && x.보유품목코드 == listBom[0].자재코드).ToList();
                    dc.외주생산위치정보.RemoveRange(외주생산위치삭제);
                }
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

        }


        public Task<bool> 더존멀티_생산실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 생산실적헤더.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";

                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 생산실적헤더.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DIV_CD = 생산실적헤더.사업장코드,
                    DEPT_CD = 생산실적상세.부서코드,
                    EMP_CD = 생산실적상세.작업자사번,
                    BASELOC_CD = 생산실적헤더.실적공정코드_창고코드,
                    LOC_CD = 생산실적헤더.실적작업장코드_장소코드,
                    REWORK_YN = 생산실적헤더.재작업여부,
                    PITEM_CD = 생산실적헤더.생산품코드,
                    ITEM_QT = 생산실적헤더.실적수량,
                    BASELOC_FG = 생산실적헤더.실적구분,
                    WR_WH_CD = 생산실적헤더.실적공정코드,
                    WR_LC_CD = 생산실적헤더.실적작업장코드,
                    PLN_CD = "",
                    REMARK_DC = "",
                    LOT_NB = 생산실적헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 생산실적상세.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 생산실적상세.실적등록일,
                    실적일자 = 생산실적상세.실적등록일,
                    사업장코드 = 생산실적헤더.사업장코드,
                    부서코드 = 생산실적상세.부서코드,
                    사원코드 = 생산실적상세.작업자사번,
                    실적공정코드_창고코드 = 생산실적헤더.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 생산실적헤더.실적작업장코드_장소코드,
                    재작업여부 = 생산실적헤더.재작업여부,
                    실적품번 = 생산실적헤더.생산품코드,
                    실적수량 = 생산실적헤더.실적수량,
                    실적구분 = 생산실적헤더.실적구분,
                    실적공정코드 = 생산실적헤더.실적공정코드,
                    실적작업장코드 = 생산실적헤더.실적작업장코드,
                    작업자코드 = "",
                    비고 = "",
                    LOTNO = 생산실적상세.LOT번호,
                };

                var 생산실적헤더Data = dc.생산실적헤더정보.Where(x => x.회사코드 == 생산실적헤더.회사코드 && x.생산지시코드 == 생산실적헤더.생산지시코드).FirstOrDefault();
                if (생산실적헤더Data != null)
                {
                    생산실적헤더Data.일괄생산등록유무 = "1";
                    생산실적헤더Data.작업번호 = 작업번호1;
                }

                dc.생산실적헤더정보.Update(생산실적헤더Data);

                dc.Add(일괄생산실적헤더정보);
                dc_D.Add(BARPLUS_LPRODUCTION);

                dc.SaveChanges();

                dc_D.SaveChanges();

                더존멀티_생산실적상세정보_등록(생산실적헤더, 생산실적상세, 일괄생산실적헤더정보, listBom);

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }

        public void 더존멀티_생산실적상세정보_등록(생산실적헤더정보 생산실적헤더정보, 생산실적상세정보 생산실적상세, 일괄생산실적헤더정보 일괄생산실적헤더, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;


            try
            {
                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드
                && x.지시서 == 생산실적상세.주문번호 && x.사유 == "0" && x.보유품목코드 == listBom[0].자재코드).ToList();


                foreach (var item in 외주생산위치정보)
                {
                    var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 생산실적상세.회사코드 && x.작업번호 == 일괄생산실적헤더.작업번호) + 1;

                    var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                    {
                        CO_CD = 일괄생산실적헤더.회사코드,
                        WORK_NB = 일괄생산실적헤더.작업번호,
                        WORK_SQ = 순번.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                        CITEM_CD = item.보유품목코드,
                        USE_QT = listBom[0].사용수량,
                        BASELOC_CD = 생산실적상세.사용공정_사용창고,
                        LOC_CD = 생산실적상세.사용작업장_사용장소,
                        LOT_NB = item.LOT번호,
                        BASELOC_FG = "1",

                    };

                    var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                    {
                        회사코드 = 일괄생산실적헤더.회사코드,
                        작업번호 = 일괄생산실적헤더.작업번호,
                        작업순번 = 순번.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                        사용품번 = item.보유품목코드,
                        사용수량 = listBom[0].사용수량,

                        LOTNO = item.LOT번호,
                        사용공정_사용창고 = 생산실적상세.사용공정_사용창고,
                        사용작업장_사용장소 = 생산실적상세.사용작업장_사용장소,
                        창고구분 = "1",

                    };

                    dc_D.Add(BARPLUS_LPRODUCTION_D);
                    dc.Add(일괄생산실적상세정보);

                    // 보유품목등록
                    // LOT번호 부여
                    // 위치등록
                    // 보유품목이력
                    dc.SaveChanges();

                    dc_D.SaveChanges();
                }


                var 생산실적상세정보 = dc.생산실적상세정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.생산지시코드 == 생산실적상세.생산지시코드).ToList();
                if (생산실적상세정보 != null)
                {
                    foreach (var item in 생산실적상세정보)
                    {
                        item.일괄생산등록유무 = "1";
                        dc.생산실적상세정보.Update(item);
                    }
                }

                dc.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

        }


        public Task<bool> 더존멀티_불량실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 생산실적헤더.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";
                string 불량창고 = "1009";
                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 생산실적헤더.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DIV_CD = 생산실적헤더.사업장코드,
                    DEPT_CD = 생산실적상세.부서코드,
                    EMP_CD = 생산실적상세.작업자사번,
                    BASELOC_CD = 생산실적헤더.실적공정코드_창고코드,
                    LOC_CD = 불량창고,
                    REWORK_YN = 생산실적헤더.재작업여부,
                    PITEM_CD = 생산실적헤더.생산품코드,
                    ITEM_QT = 생산실적헤더.불량수량,
                    BASELOC_FG = 생산실적헤더.실적구분,
                    WR_WH_CD = 생산실적헤더.실적공정코드,
                    WR_LC_CD = 불량창고,
                    PLN_CD = "",
                    REMARK_DC = "불량",
                    LOT_NB = 생산실적헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 생산실적상세.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 생산실적상세.실적등록일,
                    실적일자 = 생산실적상세.실적등록일,
                    사업장코드 = 생산실적헤더.사업장코드,
                    부서코드 = 생산실적상세.부서코드,
                    사원코드 = 생산실적상세.작업자사번,
                    실적공정코드_창고코드 = 생산실적헤더.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 불량창고,
                    재작업여부 = 생산실적헤더.재작업여부,
                    실적품번 = 생산실적헤더.생산품코드,
                    실적수량 = 생산실적헤더.불량수량,
                    실적구분 = 생산실적헤더.실적구분,
                    실적공정코드 = 생산실적헤더.실적공정코드,
                    실적작업장코드 = 불량창고,
                    작업자코드 = "",
                    비고 = "불량",
                    LOTNO = 생산실적상세.LOT번호,
                };


                if (생산실적헤더.불량수량 > 0)
                {
                    dc.Add(일괄생산실적헤더정보);
                    dc_D.Add(BARPLUS_LPRODUCTION);
                    dc.SaveChanges();
                    dc_D.SaveChanges();
                }


                var 생산실적헤더Data = dc.생산실적헤더정보.Where(x => x.회사코드 == 생산실적헤더.회사코드 && x.생산지시코드 == 생산실적헤더.생산지시코드).FirstOrDefault();
                if (생산실적헤더Data != null)
                {
                    생산실적헤더Data.일괄생산등록유무 = "1";
                    생산실적헤더Data.작업번호 = 작업번호1;
                    dc.생산실적헤더정보.Update(생산실적헤더Data);
                    dc.SaveChanges();
                }


                더존멀티_불량실적상세정보_등록(생산실적헤더, 생산실적상세, 일괄생산실적헤더정보, listBom);

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }


        public void 더존멀티_불량실적상세정보_등록(생산실적헤더정보 생산실적헤더정보, 생산실적상세정보 생산실적상세, 일괄생산실적헤더정보 일괄생산실적헤더, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;



            try
            {
                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호
                && x.사유 == "0" && x.보유품목코드 == listBom[0].자재코드).ToList();

                if (listBom[0].불량수량 > 0)
                {
                    foreach (var item in 외주생산위치정보)
                    {
                        var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 생산실적상세.회사코드 && x.작업번호 == 일괄생산실적헤더.작업번호) + 1;

                        var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                        {
                            CO_CD = 일괄생산실적헤더.회사코드,
                            WORK_NB = 일괄생산실적헤더.작업번호,
                            WORK_SQ = 순번.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                            CITEM_CD = item.보유품목코드,

                            USE_QT = listBom[0].불량수량,
                            BASELOC_CD = 생산실적상세.사용공정_사용창고,
                            LOC_CD = 생산실적상세.사용작업장_사용장소,
                            LOT_NB = item.LOT번호,
                            BASELOC_FG = "1",
                            REMARK_DC = "불량",
                        };

                        var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                        {
                            회사코드 = 일괄생산실적헤더.회사코드,
                            작업번호 = 일괄생산실적헤더.작업번호,
                            작업순번 = 순번.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                            사용품번 = item.보유품목코드,
                            사용수량 = listBom[0].불량수량,

                            LOTNO = 일괄생산실적헤더.LOTNO,
                            사용공정_사용창고 = 생산실적상세.사용공정_사용창고,
                            사용작업장_사용장소 = 생산실적상세.사용작업장_사용장소,
                            창고구분 = "1",
                            비고 = "불량",

                        };

                        if (listBom[0].불량수량 > 0)
                        {
                            dc_D.Add(BARPLUS_LPRODUCTION_D);
                            dc.Add(일괄생산실적상세정보);
                            dc.SaveChanges();
                            dc_D.SaveChanges();
                        }
                    }
                }
                // 보유품목등록
                // LOT번호 부여
                // 위치등록
                // 보유품목이력
                string 위치상세코드 = 일괄생산실적헤더.실적공정코드_창고코드 + 일괄생산실적헤더.실적작업장코드_장소코드;

                foreach (var item in 외주생산위치정보)
                {
                    var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == item.보유품목코드 && x.회사코드 == 생산실적상세.회사코드).FirstOrDefault();
                    if (보유품목 != null)
                    {
                        if (보유품목.수량 > 0)
                        {
                            보유품목.수량 = 보유품목.수량 - item.수량;
                        }
                        if (보유품목.수량 < 0)
                        {
                            보유품목.수량 = 0;
                        }

                        dc.보유품목정보.Update(보유품목);
                    }

                    var 보유품목위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == item.보유품목코드 && x.회사코드 == item.회사코드 && x.위치상세코드 == 위치상세코드).FirstOrDefault();

                    if (보유품목위치 != null)
                    {
                        if (보유품목위치.수량 > 0)
                        {
                            보유품목위치.수량 = 보유품목위치.수량 - item.수량;

                            if (보유품목위치.수량 < 0)
                            {
                                보유품목위치.수량 = 0;
                            }
                        }

                        dc.보유품목위치정보.Update(보유품목위치);
                    }
                }





                var 외주생산위치 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호 && x.사유 == "0" && x.보유품목코드 == listBom[0].자재코드).FirstOrDefault();
                if (외주생산위치 != null)
                {

                    var 외주생산위치삭제 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호 && x.사유 == "0" && x.보유품목코드 == listBom[0].자재코드).ToList();
                    dc.외주생산위치정보.RemoveRange(외주생산위치삭제);
                }
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

        }








        // 20210530
        public Task<IEnumerable<공정단위검사장비>> 공정단위검사장비_조회(string 공정단위코드, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정단위검사장비
                .Include(x => x.검사장비)
            .Where_미삭제_사용()
            .Where(x => x.공정단위코드 == 공정단위코드 && x.회사코드 == 회사코드)
            .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task<IEnumerable<외주지시별품질검사장비>> 외주지시별품질검사장비_조회(string 지시번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.외주지시별품질검사장비
                .Include(x => x.검사장비)
            .Where_미삭제_사용()
            .Where(x => x.지시번호 == 지시번호 &&  x.회사코드 == 회사코드)
            .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task 외주지시별품질검사장비_저장(외주지시별품질검사장비 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.검사정보 = null;
            info.검사장비 = null;
            // }}}

            if (isAdd == true)
                dc.외주지시별품질검사장비.Add(info);
            else
                dc.외주지시별품질검사장비.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task 외주지시별품질검사장비_삭제(외주지시별품질검사장비 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.외주지시별품질검사장비.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.외주지시별품질검사장비.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }




        public Task<IEnumerable<외주지시별검사정보>> 외주지시별검사정보_조회(string 지시번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.외주지시별검사정보
                .Include(x => x.품질검사)
                .Include(x => x.외주지시별품질검사장비목록).ThenInclude(x => x.검사장비)
            .Where_미삭제_사용()
            .Where(x => x.지시번호 == 지시번호 && x.회사코드 == 회사코드 )
            .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task 외주지시별검사정보_저장(외주지시별검사정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.품질검사 = null;
            info.검사단위 = null;
            // }}}

            if (isAdd == true)
                dc.외주지시별검사정보.Add(info);
            else
            {
                var 단위검사 = dc.외주지시별검사정보.Where(x => x.회사코드 == info.회사코드 && x.품질검사코드 == info.품질검사코드).FirstOrDefault();
                단위검사.검사기준값 = info.검사기준값;
                단위검사.검사단위 = info.검사단위;
                단위검사.검사단위코드 = info.검사단위코드;
                단위검사.검사측정값 = info.검사측정값;
                단위검사.지시번호 = info.지시번호;
                단위검사.오차범위 = info.오차범위;
                단위검사.오차범위상한 = info.오차범위상한;
                단위검사.오차범위하한 = info.오차범위하한;
                단위검사.회사코드 = info.회사코드;
                단위검사.사용유무 = info.사용유무;
                단위검사.삭제유무 = info.삭제유무;
                dc.외주지시별검사정보.Update(단위검사);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 외주지시별검사정보_삭제(외주지시별검사정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.외주지시별검사정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.외주지시별검사정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task<IEnumerable<발주서별품질검사장비>> 발주서별품질검사장비_조회(string 발주번호, decimal 발주순번, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.발주서별품질검사장비
                .Include(x => x.검사장비)
            .Where_미삭제_사용()
            .Where(x => x.발주번호 == 발주번호 && x.발주순번 == 발주순번 && x.회사코드 == 회사코드)
            .ToList();

            return Task.FromResult(result.AsEnumerable());
        }



        public Task<IEnumerable<발주서별품질검사정보>> 발주서별별검사정보_조회(string 발주번호, decimal 발주순번, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.발주서별품질검사정보
                .Include(x => x.품질검사)
                //.Include(x => x.발주서별품질검사장비목록).ThenInclude(x => x.검사장비)
            .Where_미삭제_사용()
            .Where(x => x.발주번호 == 발주번호 && x.발주순번 == 발주순번 && x.회사코드 == 회사코드)
            .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task 발주서별품질검사장비_삭제(발주서별품질검사장비 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.발주서별품질검사장비.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.발주서별품질검사장비.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 발주서별품질검사장비_저장(발주서별품질검사장비 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            //info.검사정보 = null;
            info.검사장비 = null;
            // }}}

            if (isAdd == true)
                dc.발주서별품질검사장비.Add(info);
            else
                dc.발주서별품질검사장비.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }



        public Task 발주서별품질검사정보_저장(발주서별품질검사정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.품질검사 = null;
            info.검사단위 = null;
            // }}}

            if (isAdd == true)
                dc.발주서별품질검사정보.Add(info);
            else
            {
                var 단위검사 = dc.발주서별품질검사정보.Where(x => x.회사코드 == info.회사코드 && x.품질검사코드 == info.품질검사코드).FirstOrDefault();
                단위검사.검사기준값 = info.검사기준값;
                단위검사.검사단위 = info.검사단위;
                단위검사.검사단위코드 = info.검사단위코드;
                단위검사.검사측정값 = info.검사측정값;
                단위검사.발주번호 = info.발주번호;
                단위검사.오차범위 = info.오차범위;
                단위검사.오차범위상한 = info.오차범위상한;
                단위검사.오차범위하한 = info.오차범위하한;
                단위검사.회사코드 = info.회사코드;
                단위검사.사용유무 = info.사용유무;
                단위검사.삭제유무 = info.삭제유무;
                dc.발주서별품질검사정보.Update(단위검사);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 발주서별품질검사정보_삭제(발주서별품질검사정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.발주서별품질검사정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.발주서별품질검사정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }





        public Task<IEnumerable<발주서별수입검사>> 수입검사품질검사현황_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.발주서별수입검사
                .Where(x => x.회사코드 == 회사코드)
                .ToList();

           
            return Task.FromResult(list.AsEnumerable());
        }


        public Task<List<발주서별품질검사측정정보>> 발주서별품질검사항목_조회(발주서별수입검사 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.발주서별품질검사측정정보
                .Include(x => x.품질검사)
                .Where_미삭제_사용()
                .Where(x => x.발주번호 == info.발주번호 && x.발주순번 == info.발주순번 && x.회사코드 == info.회사코드 ).ToList();

            return Task.FromResult(result.ToList());

        }

        public Task<IEnumerable<발주서별품질검사정보>> 발주서별품질검사_조회(발주서별수입검사 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.발주서별품질검사정보
            .Include(x => x.품질검사)
            .Where_미삭제_사용()
            .Where(x => x.발주번호 == info.발주번호 && x.발주순번 == info.발주순번 && x.회사코드 == info.회사코드)
            .ToList();

            return Task.FromResult(result.AsEnumerable());
        }



        public Task<IEnumerable<발주서별품질검사측정정보>> 발주서별품질검사측정정보_조회(발주서별품질검사측정정보 info, int seq)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.발주서별품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.시리얼넘버 == seq && x.발주번호 == info.발주번호 && x.발주순번 == info.발주순번)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }







        public Task<IEnumerable<외주작업지시서품검정보>> 외주작업지시서품검정보현황_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.외주작업지시서품검정보
                .Where(x => x.회사코드 == 회사코드)
                .ToList();


            return Task.FromResult(list.AsEnumerable());
        }




        public Task<List<외주품질검사측정정보>> 외주작업지시서품검항목_조회(외주작업지시서품검정보 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.외주품질검사측정정보
                .Include(x => x.품질검사)
                .Where_미삭제_사용()
                .Where(x => x.지시번호 == info.지시번호 && x.회사코드 == info.회사코드).ToList();

            return Task.FromResult(result.ToList());

        }



        public Task<IEnumerable<외주지시별검사정보>> 외주품질검사_조회(외주작업지시서품검정보 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.외주지시별검사정보
            .Include(x => x.품질검사)
            .Where_미삭제_사용()
            .Where(x => x.지시번호 == info.지시번호 && x.회사코드 == info.회사코드)
            .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task<IEnumerable<외주품질검사측정정보>> 외주품질검사측정정보_조회(외주품질검사측정정보 info, int seq)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.외주품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.시리얼넘버 == seq && x.지시번호 == info.지시번호 )
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

















        public Task<bool> 품질검사_작업생산실적_저장(생산실적헤더정보 헤더, 생산실적상세정보 상세, List<공정단위자재현황> listBom)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                var 생산지시유무 = dc.생산실적헤더정보.Where(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드).FirstOrDefault();

                //var 작업순번 = dc.생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드 ) + 1;
                var 작업자생산실적 = new 작업자생산실적정보
                {
                    회사코드 = 헤더.회사코드,
                    생산지시코드 = 헤더.생산지시코드,
                    생산지시명 = 헤더.생산지시명,
                    작업순번 = dc.작업자생산실적정보.Count(x => x.회사코드 == 헤더.회사코드) + 1,
                    공정단위코드 = 헤더.공정단위코드,
                    작업자사번 = 상세.작업자사번,
                    생산품코드 = 헤더.생산품코드,
                    실적수량 = 헤더.실적수량,
                    불량수량 = 헤더.불량수량,
                    실적등록일 = 상세.실적등록일,

                };
                dc.작업자생산실적정보.Add(작업자생산실적);

                if (생산지시유무 == null)
                    dc.생산실적헤더정보.Add(헤더);
                else
                {
                    생산지시유무.실적수량 = 생산지시유무.실적수량 + 헤더.실적수량;
                    생산지시유무.불량수량 = 생산지시유무.불량수량 + 헤더.불량수량;

                    dc.생산실적헤더정보.Update(생산지시유무);
                }

                dc.SaveChanges();
                foreach (var item in listBom)
                {
                    var 작업순번 = dc.생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드) + 1;
                    상세.사용수량 = item.필요수량 * 헤더.실적수량;
                    상세.불량수량 = item.필요수량 * 헤더.불량수량;
                    상세.사용품번 = item.자재코드;

                    상세.작업순번 = 작업순번;
                    dc.생산실적상세정보.Add(상세);

                    dc.SaveChanges();
                }

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }



            return Task.FromResult(result);
        }

        public Task<bool> 품질검사_생산제품입고처리_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            try
            {
                DateTime 보유년월일 = DateTime.Now;
                var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == 생산실적헤더.생산품코드 && x.회사코드 == 생산실적헤더.회사코드).FirstOrDefault();
                var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 생산실적헤더.생산품코드).FirstOrDefault();

                string 입고장소위치코드 = "";


                if (품목정보.품목구분코드 == "B1202" || 품목정보.품목구분코드 == "B1204")
                    입고장소위치코드 = "10001000";
                else if (품목정보.품목구분코드 == "B1203")
                    입고장소위치코드 = "10001001";

                if (보유품목 == null)
                {
                    var info = new 보유품목정보
                    {
                        회사코드 = 생산실적헤더.회사코드,
                        보유품목코드 = 생산실적헤더.생산품코드,
                        품목코드 = 생산실적헤더.생산품코드,
                        보유년월일 = 보유년월일.ToString("yyyyMMdd"),
                        조정년월일 = null,
                        보유일 = 보유년월일,
                        순번 = 1,
                        수량 = 생산실적헤더.실적수량,
                        실제수량 = 생산실적헤더.실적수량,
                        품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                        장소코드 = "1000",
                        장소위치코드 = 입고장소위치코드,
                        LOT번호 = 생산실적헤더.LOTNO,
                        품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}"
                    };
                    dc.보유품목정보.Add(info);
                }
                else
                {
                    //보유품목.LOT번호 = 생산실적헤더.LOTNO;
                    //보유품목.품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}";
                    보유품목.장소코드 = "1000";
                    보유품목.장소위치코드 = 입고장소위치코드;
                    보유품목.수량 = 보유품목.수량 + 생산실적헤더.실적수량;
                    보유품목.실제수량 = 보유품목.수량 + 생산실적헤더.실적수량;
                    dc.보유품목정보.Update(보유품목);
                }

                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            result = true;
            return Task.FromResult(result);
        }

        public Task<bool> 품질검사_생산제품_위치등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 생산실적헤더.생산품코드).FirstOrDefault();
                string 입고장소위치코드 = "";


                if (품목정보.품목구분코드 == "B1202" || 품목정보.품목구분코드 == "B1204")
                    입고장소위치코드 = "10001000";
                else if (품목정보.품목구분코드 == "B1203")
                    입고장소위치코드 = "10001001";

                var 장소위치코드 = 입고장소위치코드;
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 생산실적헤더.생산품코드);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                var info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 생산실적헤더.생산품코드 && x.장소위치코드 == 장소위치코드);

                if (info == default)
                {
                    info = new 보유품목위치정보
                    {
                        회사코드 = 생산실적헤더.회사코드,
                        보유품목코드 = 생산실적헤더.생산품코드,
                        장소위치코드 = 장소위치코드,
                        수량 = 생산실적헤더.실적수량,
                        LOT번호 = 생산실적헤더.LOTNO,
                        품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}",
                    };
                    dc.보유품목위치정보.Add(info);
                }
                // 변경
                else
                {
                    info.수량 = info.수량 + 생산실적헤더.실적수량;
                    info.LOT번호 = 생산실적헤더.LOTNO;
                    info.품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}";
                    dc.보유품목위치정보.Update(info);
                }
                dc.SaveChanges();

                #region 불량품 창고넣기

                if (생산실적헤더.불량수량 > 0)
                {
                    string 불량품창고 = "10001009";
                    //불량품등록
                    var info2 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 생산실적헤더.생산품코드 && x.장소위치코드 == 불량품창고);

                    if (info2 == default)
                    {
                        info2 = new 보유품목위치정보
                        {
                            회사코드 = 생산실적헤더.회사코드,
                            보유품목코드 = 생산실적헤더.생산품코드,
                            장소위치코드 = 불량품창고,
                            수량 = 생산실적헤더.불량수량,
                            LOT번호 = 생산실적헤더.LOTNO,
                            품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}",
                        };
                        dc.보유품목위치정보.Add(info2);
                    }
                    // 변경
                    else
                    {
                        info2.수량 = info2.수량 + 생산실적헤더.불량수량;
                        info2.LOT번호 = 생산실적헤더.LOTNO;
                        info2.품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}";
                        dc.보유품목위치정보.Update(info2);
                    }

                    dc.SaveChanges();
                }
                #endregion


                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 생산실적헤더.생산품코드 && x.장소위치코드 == 장소위치코드);

                var log = new 보유품목이력
                {
                    회사코드 = 생산실적헤더.회사코드,
                    보유품목코드 = 생산실적헤더.생산품코드,
                    연계보유품목코드 = 생산실적헤더.생산품코드,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = "1000",
                    장소위치코드 = 장소위치코드,
                    변경수량 = 생산실적헤더.실적수량,
                    변경사유 = "생산입고",
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now,
                    LOT번호 = 생산실적헤더.LOTNO,
                    품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}",
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                if (보유이력 == null)
                    dc.보유품목이력.Add(log);

                dc.SaveChanges();





                var 바코드발급 = dc.바코드발급정보
                  .Include(x => x.품목)
                  .Where(x => x.품목코드 == 생산실적헤더.생산품코드 && x.LOT번호 == 생산실적헤더.LOTNO && x.회사코드 == 생산실적헤더.회사코드)
                  .FirstOrDefault();
                if (바코드발급 != default)
                {
                    바코드발급.입고유무 = true;
                    바코드발급.입고일자 = DateTime.Now.ToString("yyyyMMdd");
                    dc.바코드발급정보.Update(바코드발급);
                    dc.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

            return Task.FromResult(result);

        }

        public Task<bool> 품질검사_더존_생산실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 생산실적헤더.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";

                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 생산실적헤더.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DIV_CD = 생산실적헤더.사업장코드,
                    DEPT_CD = 생산실적상세.부서코드,
                    EMP_CD = 생산실적상세.작업자사번,
                    BASELOC_CD = 생산실적헤더.실적공정코드_창고코드,
                    LOC_CD = 생산실적헤더.실적작업장코드_장소코드,
                    REWORK_YN = 생산실적헤더.재작업여부,
                    PITEM_CD = 생산실적헤더.생산품코드,
                    ITEM_QT = 생산실적헤더.실적수량,
                    BASELOC_FG = 생산실적헤더.실적구분,
                    WR_WH_CD = 생산실적헤더.실적공정코드,
                    WR_LC_CD = 생산실적헤더.실적작업장코드,
                    PLN_CD = "",
                    REMARK_DC = "",
                    LOT_NB = 생산실적헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 생산실적상세.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 생산실적상세.실적등록일,
                    실적일자 = 생산실적상세.실적등록일,
                    사업장코드 = 생산실적헤더.사업장코드,
                    부서코드 = 생산실적상세.부서코드,
                    사원코드 = 생산실적상세.작업자사번,
                    실적공정코드_창고코드 = 생산실적헤더.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 생산실적헤더.실적작업장코드_장소코드,
                    재작업여부 = 생산실적헤더.재작업여부,
                    실적품번 = 생산실적헤더.생산품코드,
                    실적수량 = 생산실적헤더.실적수량,
                    실적구분 = 생산실적헤더.실적구분,
                    실적공정코드 = 생산실적헤더.실적공정코드,
                    실적작업장코드 = 생산실적헤더.실적작업장코드,
                    작업자코드 = "",
                    비고 = "",
                    LOTNO = 생산실적상세.LOT번호,
                };

                var 생산실적헤더Data = dc.생산실적헤더정보.Where(x => x.회사코드 == 생산실적헤더.회사코드 && x.생산지시코드 == 생산실적헤더.생산지시코드).FirstOrDefault();
                if (생산실적헤더Data != null)
                {
                    생산실적헤더Data.일괄생산등록유무 = "1";
                    생산실적헤더Data.작업번호 = 작업번호1;
                }

                dc.생산실적헤더정보.Update(생산실적헤더Data);

                dc.Add(일괄생산실적헤더정보);
                dc_D.Add(BARPLUS_LPRODUCTION);

                dc.SaveChanges();

                dc_D.SaveChanges();

                품질검사_더존_생산실적상세정보_등록(생산실적헤더, 생산실적상세, 일괄생산실적헤더정보, listBom);

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }

        public void 품질검사_더존_생산실적상세정보_등록(생산실적헤더정보 생산실적헤더정보, 생산실적상세정보 생산실적상세, 일괄생산실적헤더정보 일괄생산실적헤더, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            try
            {
                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호 && x.사유 == "공정이동").ToList();

                foreach (var item in 외주생산위치정보)
                {
                    var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 생산실적상세.회사코드 && x.작업번호 == 일괄생산실적헤더.작업번호) + 1;

                    var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                    {
                        CO_CD = 일괄생산실적헤더.회사코드,
                        WORK_NB = 일괄생산실적헤더.작업번호,
                        WORK_SQ = 순번.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                        CITEM_CD = item.보유품목코드,
                        USE_QT = listBom[0].사용수량,
                        BASELOC_CD = 생산실적상세.사용공정_사용창고,
                        LOC_CD = 생산실적상세.사용작업장_사용장소,
                        LOT_NB = item.LOT번호,
                        BASELOC_FG = "1",

                    };

                    var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                    {
                        회사코드 = 일괄생산실적헤더.회사코드,
                        작업번호 = 일괄생산실적헤더.작업번호,
                        작업순번 = 순번.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                        사용품번 = item.보유품목코드,
                        사용수량 = listBom[0].사용수량,

                        LOTNO = item.LOT번호,
                        사용공정_사용창고 = 생산실적상세.사용공정_사용창고,
                        사용작업장_사용장소 = 생산실적상세.사용작업장_사용장소,
                        창고구분 = "1",

                    };

                    dc_D.Add(BARPLUS_LPRODUCTION_D);
                    dc.Add(일괄생산실적상세정보);

                    // 보유품목등록
                    // LOT번호 부여
                    // 위치등록
                    // 보유품목이력
                    dc.SaveChanges();

                    dc_D.SaveChanges();
                }


                var 생산실적상세정보 = dc.생산실적상세정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.생산지시코드 == 생산실적상세.생산지시코드).ToList();
                if (생산실적상세정보 != null)
                {
                    foreach (var item in 생산실적상세정보)
                    {
                        item.일괄생산등록유무 = "1";
                        dc.생산실적상세정보.Update(item);
                    }
                }

                dc.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

        }

        public Task<bool> 품질검사_더존_불량실적헤더정보_등록(생산실적헤더정보 생산실적헤더, 생산실적상세정보 생산실적상세, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 생산실적헤더.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";
                string 불량창고 = "1009";

                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 생산실적헤더.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적상세.실적등록일.ToString())),
                    DIV_CD = 생산실적헤더.사업장코드,
                    DEPT_CD = 생산실적상세.부서코드,
                    EMP_CD = 생산실적상세.작업자사번,
                    BASELOC_CD = 생산실적헤더.실적공정코드_창고코드,
                    LOC_CD = 불량창고,
                    REWORK_YN = 생산실적헤더.재작업여부,
                    PITEM_CD = 생산실적헤더.생산품코드,
                    ITEM_QT = 생산실적헤더.불량수량,
                    BASELOC_FG = 생산실적헤더.실적구분,
                    WR_WH_CD = 생산실적헤더.실적공정코드,
                    WR_LC_CD = 불량창고,
                    PLN_CD = "",
                    REMARK_DC = "불량",
                    LOT_NB = 생산실적헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 생산실적상세.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 생산실적상세.실적등록일,
                    실적일자 = 생산실적상세.실적등록일,
                    사업장코드 = 생산실적헤더.사업장코드,
                    부서코드 = 생산실적상세.부서코드,
                    사원코드 = 생산실적상세.작업자사번,
                    실적공정코드_창고코드 = 생산실적헤더.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 불량창고,
                    재작업여부 = 생산실적헤더.재작업여부,
                    실적품번 = 생산실적헤더.생산품코드,
                    실적수량 = 생산실적헤더.불량수량,
                    실적구분 = 생산실적헤더.실적구분,
                    실적공정코드 = 생산실적헤더.실적공정코드,
                    실적작업장코드 = 불량창고,
                    작업자코드 = "",
                    비고 = "불량",
                    LOTNO = 생산실적상세.LOT번호,
                };


                if (생산실적헤더.불량수량 > 0)
                {
                    dc.Add(일괄생산실적헤더정보);
                    dc_D.Add(BARPLUS_LPRODUCTION);
                    dc.SaveChanges();
                    dc_D.SaveChanges();
                }


                var 생산실적헤더Data = dc.생산실적헤더정보.Where(x => x.회사코드 == 생산실적헤더.회사코드 && x.생산지시코드 == 생산실적헤더.생산지시코드).FirstOrDefault();
                if (생산실적헤더Data != null)
                {
                    생산실적헤더Data.일괄생산등록유무 = "1";
                    생산실적헤더Data.작업번호 = 작업번호1;
                    dc.생산실적헤더정보.Update(생산실적헤더Data);
                    dc.SaveChanges();
                }


                품질검사_더존_불량실적상세정보_등록(생산실적헤더, 생산실적상세, 일괄생산실적헤더정보, listBom);

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }

        public void 품질검사_더존_불량실적상세정보_등록(생산실적헤더정보 생산실적헤더정보, 생산실적상세정보 생산실적상세, 일괄생산실적헤더정보 일괄생산실적헤더, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;



            try
            {
                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호 && x.사유 == "공정이동").ToList();

                if (listBom[0].불량수량 > 0)
                {
                    foreach (var item in 외주생산위치정보)
                    {
                        var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 생산실적상세.회사코드 && x.작업번호 == 일괄생산실적헤더.작업번호) + 1;

                        var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                        {
                            CO_CD = 일괄생산실적헤더.회사코드,
                            WORK_NB = 일괄생산실적헤더.작업번호,
                            WORK_SQ = 순번.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                            CITEM_CD = item.보유품목코드,

                            USE_QT = listBom[0].불량수량,
                            BASELOC_CD = 생산실적상세.사용공정_사용창고,
                            LOC_CD = 생산실적상세.사용작업장_사용장소,
                            LOT_NB = item.LOT번호,
                            BASELOC_FG = "1",
                            REMARK_DC = "불량",
                        };

                        var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                        {
                            회사코드 = 일괄생산실적헤더.회사코드,
                            작업번호 = 일괄생산실적헤더.작업번호,
                            작업순번 = 순번.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                            사용품번 = item.보유품목코드,
                            사용수량 = listBom[0].불량수량,

                            LOTNO = 일괄생산실적헤더.LOTNO,
                            사용공정_사용창고 = 생산실적상세.사용공정_사용창고,
                            사용작업장_사용장소 = 생산실적상세.사용작업장_사용장소,
                            창고구분 = "1",
                            비고 = "불량",

                        };

                        if (listBom[0].불량수량 > 0)
                        {
                            dc_D.Add(BARPLUS_LPRODUCTION_D);
                            dc.Add(일괄생산실적상세정보);
                            dc.SaveChanges();
                            dc_D.SaveChanges();
                        }
                    }
                }
                // 보유품목등록
                // LOT번호 부여
                // 위치등록
                // 보유품목이력
                string 위치상세코드 = 일괄생산실적헤더.실적공정코드_창고코드 + 일괄생산실적헤더.실적작업장코드_장소코드;

                foreach (var item in 외주생산위치정보)
                {
                    var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == item.보유품목코드 && x.회사코드 == 생산실적상세.회사코드).FirstOrDefault();
                    if (보유품목 != null)
                    {
                        if (보유품목.수량 > 0)
                        {
                            보유품목.수량 = 보유품목.수량 - item.수량;
                        }
                        if (보유품목.수량 < 0)
                        {
                            보유품목.수량 = 0;
                        }

                        dc.보유품목정보.Update(보유품목);
                    }

                    var 보유품목위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == item.보유품목코드 && x.회사코드 == item.회사코드 && x.위치상세코드 == 위치상세코드).FirstOrDefault();

                    if (보유품목위치 != null)
                    {
                        if (보유품목위치.수량 > 0)
                        {
                            보유품목위치.수량 = 보유품목위치.수량 - item.수량;

                            if (보유품목위치.수량 < 0)
                            {
                                보유품목위치.수량 = 0;
                            }
                        }

                        dc.보유품목위치정보.Update(보유품목위치);
                    }
                }





                var 외주생산위치 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호 && x.사유 == "공정이동").FirstOrDefault();
                if (외주생산위치 != null)
                {

                    var 외주생산위치삭제 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.지시서 == 생산실적상세.주문번호 && x.사유 == "공정이동").ToList();
                    dc.외주생산위치정보.RemoveRange(외주생산위치삭제);
                }
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

        }
















        public Task<bool> 공정단위실적처리_저장(생산실적헤더정보 헤더, 생산실적상세정보 상세, List<공정단위자재현황> listBom)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;


            bool result = false;
            try
            {

           

            try
            {
                var 생산지시유무 = dc.생산실적헤더정보.Where(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드).FirstOrDefault();

                //var 작업순번 = dc.생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드 ) + 1;
                var 작업자생산실적 = new 작업자생산실적정보
                {
                    회사코드 = 헤더.회사코드,
                    생산지시코드 = 헤더.생산지시코드,
                    생산지시명 = 헤더.생산지시명,
                    작업순번 = dc.작업자생산실적정보.Count(x => x.회사코드 == 헤더.회사코드) + 1,
                    공정단위코드 = 헤더.공정단위코드,
                    작업자사번 = 상세.작업자사번,
                    생산품코드 = 헤더.생산품코드,
                    실적수량 = 헤더.실적수량,
                    불량수량 = 헤더.불량수량,
                    실적등록일 = 상세.실적등록일,

                };
                dc.작업자생산실적정보.Add(작업자생산실적);

                if (생산지시유무 == null)
                {
                    헤더.일괄생산등록유무 = "1";
                    dc.생산실적헤더정보.Add(헤더);
                }
                else
                {
                    생산지시유무.실적수량 = 생산지시유무.실적수량 + 헤더.실적수량;
                    생산지시유무.불량수량 = 생산지시유무.불량수량 + 헤더.불량수량;
                    생산지시유무.일괄생산등록유무 = "1";
                    dc.생산실적헤더정보.Update(생산지시유무);
                }

                dc.SaveChanges();
                foreach (var item in listBom)
                {
                    var 작업순번 = dc.생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드) + 1;
                    상세.사용수량 = item.필요수량 * 헤더.실적수량;
                    상세.불량수량 = item.필요수량 * 헤더.불량수량;
                    상세.사용품번 = item.자재코드;

                    상세.작업순번 = 작업순번;
                    dc.생산실적상세정보.Add(상세);

                    dc.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                result = false;
            }



            try
            {
                DateTime 보유년월일 = DateTime.Now;
                var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == 헤더.생산품코드 && x.회사코드 == 헤더.회사코드).FirstOrDefault();
                var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 헤더.생산품코드).FirstOrDefault();

                string 입고장소위치코드 = "";

                if (품목정보.품목구분코드 == "B1202" || 품목정보.품목구분코드 == "B1204")
                    입고장소위치코드 = "10001000";
                else if (품목정보.품목구분코드 == "B1203")
                    입고장소위치코드 = "10001001";
               

                if (보유품목 == null)
                {
                    var info = new 보유품목정보
                    {
                        회사코드 = 헤더.회사코드,
                        보유품목코드 = 헤더.생산품코드,
                        품목코드 = 헤더.생산품코드,
                        보유년월일 = 보유년월일.ToString("yyyyMMdd"),
                        조정년월일 = null,
                        보유일 = 보유년월일,
                        순번 = 1,
                        수량 = 헤더.실적수량,
                        실제수량 = 헤더.실적수량,
                        품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                        장소코드 = "1000",
                        장소위치코드 = 입고장소위치코드,
                        LOT번호 = 헤더.LOTNO,
                        품목_LOT번호 = $"{헤더.생산품코드}:{헤더.LOTNO}"
                    };
                    dc.보유품목정보.Add(info);
                }
                else
                {
                    //보유품목.LOT번호 = 생산실적헤더.LOTNO;
                    //보유품목.품목_LOT번호 = $"{생산실적헤더.생산품코드}:{생산실적헤더.LOTNO}";
                    보유품목.장소코드 = "1000";
                    보유품목.장소위치코드 = 입고장소위치코드;
                    보유품목.수량 = 보유품목.수량 + 헤더.실적수량;
                    보유품목.실제수량 = 보유품목.수량 + 헤더.실적수량;
                    dc.보유품목정보.Update(보유품목);
                }

                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }



            try
            {
                var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 헤더.생산품코드).FirstOrDefault();
                string 입고장소위치코드 = "";

                if (품목정보.품목구분코드 == "B1202" || 품목정보.품목구분코드 == "B1204")
                    입고장소위치코드 = "10001000";
                else if (품목정보.품목구분코드 == "B1203")
                    입고장소위치코드 = "10001001";

                    var 장소위치코드 = 입고장소위치코드;
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 헤더.생산품코드);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                var info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 헤더.생산품코드 && x.장소위치코드 == 장소위치코드);

                if (info == default)
                {
                    info = new 보유품목위치정보
                    {
                        회사코드 = 헤더.회사코드,
                        보유품목코드 = 헤더.생산품코드,
                        장소위치코드 = 장소위치코드,
                        수량 = 헤더.실적수량,
                        LOT번호 = 헤더.LOTNO,
                        품목_LOT번호 = $"{헤더.생산품코드}:{헤더.LOTNO}",
                    };
                    dc.보유품목위치정보.Add(info);
                }
                // 변경
                else
                {
                    info.수량 = info.수량 + 헤더.실적수량;
                    info.LOT번호 = 헤더.LOTNO;
                    info.품목_LOT번호 = $"{헤더.생산품코드}:{헤더.LOTNO}";
                    dc.보유품목위치정보.Update(info);
                }
                dc.SaveChanges();

                #region 불량품 창고넣기

                if (헤더.불량수량 > 0)
                {
                    string 불량품창고 = "10001009";
                    //불량품등록
                    var info2 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 헤더.생산품코드 && x.장소위치코드 == 불량품창고);

                    if (info2 == default)
                    {
                        info2 = new 보유품목위치정보
                        {
                            회사코드 = 헤더.회사코드,
                            보유품목코드 = 헤더.생산품코드,
                            장소위치코드 = 불량품창고,
                            수량 = 헤더.불량수량,
                            LOT번호 = 헤더.LOTNO,
                            품목_LOT번호 = $"{헤더.생산품코드}:{헤더.LOTNO}",
                        };
                        dc.보유품목위치정보.Add(info2);
                    }
                    // 변경
                    else
                    {
                        info2.수량 = info2.수량 + 헤더.불량수량;
                        info2.LOT번호 = 헤더.LOTNO;
                        info2.품목_LOT번호 = $"{헤더.생산품코드}:{헤더.LOTNO}";
                        dc.보유품목위치정보.Update(info2);
                    }

                    dc.SaveChanges();
                }
                #endregion


                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 헤더.생산품코드 && x.장소위치코드 == 장소위치코드);

                var log = new 보유품목이력
                {
                    회사코드 = 헤더.회사코드,
                    보유품목코드 = 헤더.생산품코드,
                    연계보유품목코드 = 헤더.생산품코드,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = "1000",
                    장소위치코드 = 장소위치코드,
                    변경수량 = 헤더.실적수량,
                    변경사유 = "생산입고",
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now,
                    LOT번호 = 헤더.LOTNO,
                    품목_LOT번호 = $"{헤더.생산품코드}:{헤더.LOTNO}",
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                if (보유이력 == null)
                    dc.보유품목이력.Add(log);

                dc.SaveChanges();


                var 바코드발급 = dc.바코드발급정보
                  .Include(x => x.품목)
                  .Where(x => x.품목코드 == 헤더.생산품코드 && x.LOT번호 == 헤더.LOTNO && x.회사코드 == 헤더.회사코드)
                  .FirstOrDefault();
                if (바코드발급 != default)
                {
                    바코드발급.입고유무 = true;
                    바코드발급.입고일자 = DateTime.Now.ToString("yyyyMMdd");
                    dc.바코드발급정보.Update(바코드발급);
                    dc.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result = false;
            }

            try
            {
                var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 헤더.회사코드) + 1;

                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";

                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 헤더.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(상세.실적등록일.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(상세.실적등록일.ToString())),
                    DIV_CD = 헤더.사업장코드,
                    DEPT_CD = 상세.부서코드,
                    EMP_CD = 상세.작업자사번,
                    BASELOC_CD = 헤더.실적공정코드_창고코드,
                    LOC_CD = 헤더.실적작업장코드_장소코드,
                    REWORK_YN = 헤더.재작업여부,
                    PITEM_CD = 헤더.생산품코드,
                    ITEM_QT = 헤더.실적수량,
                    BASELOC_FG = 헤더.실적구분,
                    WR_WH_CD = 헤더.실적공정코드,
                    WR_LC_CD = 헤더.실적작업장코드,
                    PLN_CD = "",
                    REMARK_DC = "",
                    LOT_NB = 헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 헤더.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 상세.실적등록일,
                    실적일자 = 상세.실적등록일,
                    사업장코드 = 헤더.사업장코드,
                    부서코드 = 상세.부서코드,
                    사원코드 = 상세.작업자사번,
                    실적공정코드_창고코드 = 헤더.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 헤더.실적작업장코드_장소코드,
                    재작업여부 = 헤더.재작업여부,
                    실적품번 = 헤더.생산품코드,
                    실적수량 = 헤더.실적수량,
                    실적구분 = 헤더.실적구분,
                    실적공정코드 = 헤더.실적공정코드,
                    실적작업장코드 = 헤더.실적작업장코드,
                    작업자코드 = "",
                    비고 = "",
                    LOTNO = 상세.LOT번호,
                };

                

                dc.Add(일괄생산실적헤더정보);
                dc_D.Add(BARPLUS_LPRODUCTION);

                dc.SaveChanges();

                dc_D.SaveChanges();

                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 상세.회사코드 && x.지시서 == 상세.주문번호).ToList();


                foreach (var item in 외주생산위치정보)
                {
                    var 순번2 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.작업번호 == 작업번호1) + 1;

                    var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                    {
                        CO_CD = 헤더.회사코드,
                        WORK_NB = 헤더.작업번호,
                        WORK_SQ = 순번2.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                        CITEM_CD = item.보유품목코드,
                        USE_QT = listBom[0].실적수량,
                        BASELOC_CD = 상세.사용공정_사용창고,
                        LOC_CD = 상세.사용작업장_사용장소,
                        LOT_NB = item.LOT번호,
                        BASELOC_FG = "1",

                    };

                    var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                    {
                        회사코드 = 헤더.회사코드,
                        작업번호 = 헤더.작업번호,
                        작업순번 = 순번2.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                        사용품번 = item.보유품목코드,
                        사용수량 = listBom[0].실적수량,

                        LOTNO = item.LOT번호,
                        사용공정_사용창고 = 상세.사용공정_사용창고,
                        사용작업장_사용장소 = 상세.사용작업장_사용장소,
                        창고구분 = "1",

                    };

                    dc_D.Add(BARPLUS_LPRODUCTION_D);
                    dc.Add(일괄생산실적상세정보);

                    // 보유품목등록
                    // LOT번호 부여
                    // 위치등록
                    // 보유품목이력
                    dc.SaveChanges();

                    dc_D.SaveChanges();
                }

                var 생산실적상세정보 = dc.생산실적상세정보.Where(x => x.회사코드 == 상세.회사코드 && x.생산지시코드 == 상세.생산지시코드).ToList();
                if (생산실적상세정보 != null)
                {
                    foreach (var item in 생산실적상세정보)
                    {
                        item.일괄생산등록유무 = "1";
                        dc.생산실적상세정보.Update(item);
                    }
                }

                dc.SaveChanges();
                result = true;


                }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            

            try
            {
                var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 헤더.회사코드) + 1;
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";

                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 헤더.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(상세.실적등록일.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(상세.실적등록일.ToString())),
                    DIV_CD = 헤더.사업장코드,
                    DEPT_CD = 상세.부서코드,
                    EMP_CD = 상세.작업자사번,
                    BASELOC_CD = 헤더.실적공정코드_창고코드,
                    LOC_CD = 헤더.실적작업장코드_장소코드,
                    REWORK_YN = 헤더.재작업여부,
                    PITEM_CD = 헤더.생산품코드,
                    ITEM_QT = 헤더.불량수량,
                    BASELOC_FG = 헤더.실적구분,
                    WR_WH_CD = 헤더.실적공정코드,
                    WR_LC_CD = 헤더.실적작업장코드,
                    PLN_CD = "",
                    REMARK_DC = "불량",
                    LOT_NB = 헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 상세.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 상세.실적등록일,
                    실적일자 = 상세.실적등록일,
                    사업장코드 = 헤더.사업장코드,
                    부서코드 = 상세.부서코드,
                    사원코드 = 상세.작업자사번,
                    실적공정코드_창고코드 = 헤더.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 헤더.실적작업장코드_장소코드,
                    재작업여부 = 헤더.재작업여부,
                    실적품번 = 헤더.생산품코드,
                    실적수량 = 헤더.불량수량,
                    실적구분 = 헤더.실적구분,
                    실적공정코드 = 헤더.실적공정코드,
                    실적작업장코드 = 헤더.실적작업장코드,
                    작업자코드 = "",
                    비고 = "불량",
                    LOTNO = 상세.LOT번호,
                };


                if (헤더.불량수량 > 0)
                {
                    dc.Add(일괄생산실적헤더정보);
                    dc_D.Add(BARPLUS_LPRODUCTION);
                    dc.SaveChanges();
                    dc_D.SaveChanges();
                }


                var 생산실적헤더Data = dc.생산실적헤더정보.Where(x => x.회사코드 == 헤더.회사코드 && x.생산지시코드 == 헤더.생산지시코드).FirstOrDefault();
                if (생산실적헤더Data != null)
                {
                    생산실적헤더Data.일괄생산등록유무 = "1";
                    생산실적헤더Data.작업번호 = 작업번호1;
                    dc.생산실적헤더정보.Update(생산실적헤더Data);
                    dc.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result = false;
            }

            try
            {
                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 상세.회사코드 && x.지시서 == 상세.주문번호).ToList();

                if (listBom[0].불량수량 > 0)
                {
                    foreach (var item in 외주생산위치정보)
                    {
                        var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 상세.회사코드 && x.작업번호 == 헤더.작업번호) + 1;

                        var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                        {
                            CO_CD = 헤더.회사코드,
                            WORK_NB = 헤더.작업번호,
                            WORK_SQ = 순번.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                            CITEM_CD = item.보유품목코드,

                            USE_QT = listBom[0].불량수량,
                            BASELOC_CD = 상세.사용공정_사용창고,
                            LOC_CD = 상세.사용작업장_사용장소,
                            LOT_NB = item.LOT번호,
                            BASELOC_FG = "1",
                            REMARK_DC = "불량",
                        };

                        var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                        {
                            회사코드 = 헤더.회사코드,
                            작업번호 = 헤더.작업번호,
                            작업순번 = 순번.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                            사용품번 = item.보유품목코드,
                            사용수량 = listBom[0].불량수량,

                            LOTNO = 헤더.LOTNO,
                            사용공정_사용창고 = 상세.사용공정_사용창고,
                            사용작업장_사용장소 = 상세.사용작업장_사용장소,
                            창고구분 = "1",
                            비고 = "불량",

                        };

                        if (listBom[0].불량수량 > 0)
                        {
                            dc_D.Add(BARPLUS_LPRODUCTION_D);
                            dc.Add(일괄생산실적상세정보);
                            dc.SaveChanges();
                            dc_D.SaveChanges();
                        }
                    }
                }
                // 보유품목등록
                // LOT번호 부여
                // 위치등록
                // 보유품목이력
                string 위치상세코드 = 헤더.실적공정코드_창고코드 + 헤더.실적작업장코드_장소코드;

                foreach (var item in 외주생산위치정보)
                {
                    var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == item.보유품목코드 && x.회사코드 == 상세.회사코드).FirstOrDefault();
                    if (보유품목 != null)
                    {
                        if (보유품목.수량 > 0)
                        {
                            보유품목.수량 = 보유품목.수량 - item.수량;
                        }
                        if (보유품목.수량 < 0)
                        {
                            보유품목.수량 = 0;
                        }

                        dc.보유품목정보.Update(보유품목);
                    }

                    var 보유품목위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == item.보유품목코드 && x.회사코드 == item.회사코드 && x.위치상세코드 == 위치상세코드).FirstOrDefault();

                    if (보유품목위치 != null)
                    {
                        if (보유품목위치.수량 > 0)
                        {
                            보유품목위치.수량 = 보유품목위치.수량 - item.수량;

                            if (보유품목위치.수량 < 0)
                            {
                                보유품목위치.수량 = 0;
                            }
                        }

                        dc.보유품목위치정보.Update(보유품목위치);
                    }
                }


                var 외주생산위치 = dc.외주생산위치정보.Where(x => x.회사코드 == 상세.회사코드 && x.지시서 == 상세.주문번호).FirstOrDefault();
                if (외주생산위치 != null)
                {
                    //보유품목위치정보 털기
                    //생산이동시 이미 털었음
                    /*
                    var 보유품목위치 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 외주생산위치삭제.보유품목코드 && x.장소위치코드 == 외주생산위치삭제.장소위치코드);
                    if (보유품목위치 != default)
                    {

                        보유품목위치.수량 -= 외주생산위치삭제.수량;

                        dc.보유품목위치정보.Update(보유품목위치);
                        dc.SaveChanges();
                    }*/

                    var 외주생산위치삭제 = dc.외주생산위치정보.Where(x => x.회사코드 == 상세.회사코드 && x.지시서 == 상세.주문번호).ToList();
                    dc.외주생산위치정보.RemoveRange(외주생산위치삭제);
                }
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
            }



            }
            catch (Exception exe)
            {
                result = false;
            }
            return Task.FromResult(result);
        }

        public Task<List<작업지시생산실적현황>> 완료보고용생산실적현황_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산실적헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();


            var result2 = result.GroupBy(x => new { x.생산지시코드, x.회사코드 }).Select(g => g.First()).ToList();

            List<작업지시생산실적현황> 작업지시생산실적 = new List<작업지시생산실적현황>();
            foreach (var item in result2)
            {
                var 수량 = dc.생산지시정보.Where(x => x.생산지시코드 == item.생산지시코드 && x.회사코드 == 회사코드).FirstOrDefault();
                var 작업지시생산실적현황 = new 작업지시생산실적현황
                {
                    생산지시코드 = item.생산지시코드,
                    생산지시명 = item.생산지시명,
                    생산품코드 = item.생산품코드,
                    공정단위코드 = item.공정단위코드,
                    생산품공정코드 = item.생산품공정코드,
                    사업장코드 = item.사업장코드,
                    실적공정코드_창고코드 = item.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = item.실적작업장코드_장소코드,
                    재작업여부 = item.재작업여부,
                    LOTNO = item.LOTNO,
                    일괄생산등록유무 = item.일괄생산등록유무,
                    작업번호 = item.작업번호,
                    목표수량 = 수량 != null ? 수량.생산수량 : 0,
                    실적수량 = item.실적수량,
                    불량수량 = item.불량수량,

                };
                작업지시생산실적.Add(작업지시생산실적현황);
            }

            return Task.FromResult(작업지시생산실적.ToList());

        }







    }
}
