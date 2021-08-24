using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.Parameters;
using HiSFS.Api.Shared.Services;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.Api.Host.Services
{
    /// <summary>
    /// 품질관리 API 서비스
    /// 
    /// 기준정보 성격은 차후에 기준정보로 이동할 수 있음
    /// </summary>
    public class 품질관리서비스 : I품질관리서비스
    {
        private readonly IContextProvider dcp;


        public 품질관리서비스(IContextProvider dbContextProvider)
        {
            this.dcp = dbContextProvider;
        }

        public Task<IEnumerable<품질검사정보>> 품질검사_조회(검색정보 검색)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.품질검사정보
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task 품질검사_저장(품질검사정보 info, bool isAdded)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isAdded == true)
            {
                var code = $"Q{dc.품질검사정보.Count() + 1:000000}";
                info.품질검사코드 = code;
                dc.품질검사정보.Add(info);
            }
            else
            {
                dc.품질검사정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 품질검사_삭제(품질검사정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.품질검사정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.품질검사정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<보유품목불량정보>> 수입검사_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목불량정보
                .Include(x => x.보유품목)
                .Include(x => x.보유품목).ThenInclude(x => x.품목)
                .Include(x => x.보유품목).ThenInclude(x => x.장소)
                .Include(x => x.보유품목).ThenInclude(x => x.장소위치)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task<IEnumerable<보유품목검사정보>> 제품검사_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            /*
                public 보유품목정보 보유품목 { get; set; }
                public 품질검사정보 품질검사 { get; set; }
                [ForeignKey("공정단위코드, 품질검사코드")]
                public 공정단위검사정보 검사정보 { get; set; }
                public 공통코드 검사결과 { get; set; }
                public 공통코드 불량유형 { get; set; }
            */

            var result = dc.보유품목검사정보
                .Include(x => x.보유품목).ThenInclude(x => x.품목)
                .Include(x => x.보유품목).ThenInclude(x => x.장소)
                .Include(x => x.보유품목).ThenInclude(x => x.장소위치)
                .Include(x => x.품질검사)
                .Include(x => x.검사정보).ThenInclude(x => x.공정검사장비목록).ThenInclude(x => x.검사장비)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        internal void 제품검사_등록(보유품목검사정보 보유품목검사정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dc.보유품목검사정보.Add(보유품목검사정보);
            dc.SaveChanges();
        }

        /////////////////////////////////// 추가 2021.02.16  ( 보유품목코드)  ////////////////////////////////////////////////
        public Task<bool> 품질검사측정정보_저장(품질검사측정정보 info, bool isAdded)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var 품질검사측정_업데이트 = dc.품질검사측정정보.FirstOrDefault(x => x.시리얼넘버 == info.시리얼넘버 && x.회사코드 == info.회사코드 && x.생산지시코드 == info.생산지시코드 );

            try
            {
                if (isAdded == true)
                {
                    dc.품질검사측정정보.Add(info);
                }
                else
                {
                    dc.Entry(품질검사측정_업데이트).Property("RowVersion").OriginalValue = info.RowVersion;
                    dc.품질검사측정정보.Update(info);
                }
                dc.SaveChanges();
            }

            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (품질검사측정정보)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    return Task.FromResult(false);
                }

                var dbValues = (품질검사측정정보)databaseEntry.ToObject();

                // Save the current RowVersion so next postback
                // matches unless an new concurrency issue happens.
                info.RowVersion = (byte[])dbValues.RowVersion;
                // Clear the model error for the next postback.
            }

            return Task.FromResult(true);
        }

        // 수정 2021.03.08 생산지시코드 파라메터 추가
        public Task<IEnumerable<품질검사측정정보>> 품질검사측정정보_조회(string 생산지시코드, int seq)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.시리얼넘버 == seq && x.생산지시코드 == 생산지시코드)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<품질검사측정정보> 품질검사측정정보유무_조회(int seq, 품질검사측정정보 품질검사측정)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.품질검사측정정보
                .Where_미삭제_사용()
                .FirstOrDefault(x => x.시리얼넘버 == seq && x.품질검사코드 == 품질검사측정.품질검사코드 && x.회사코드 == 품질검사측정.회사코드 && x.생산지시코드 == 품질검사측정.생산지시코드);

            return Task.FromResult(result);

        }


        /////////////////////////////////// 추가 2021.03.08  ( 보유품목 수량 제외)  ////////////////////////////////////////////////
        public Task 품질검사측정_보유품목코드_저장(string 생산품코드, string 품목구분코드, int seq)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyMMdd = now.ToString("yyMMdd");
            //var no = dc.보유품목정보.Count(x => x.품목코드 == 품목.품목코드 && x.보유년월일 == yyMMdd) + 1;
            //decimal count = dc.보유품목정보.Select(x => x.보유품목코드 == 생산품코드);

            var result = (from t in dc.보유품목정보
                          where t.보유품목코드 == 생산품코드
                          select t).DefaultIfEmpty().Single();

            //var result2 = dc.보유품목일련정보.GroupBy(x => x.품목코드 == 생산품코드 && x.순번 == seq).Count();

            var info = new 보유품목정보
            {
                보유품목코드 = 생산품코드,
                품목코드 = 생산품코드,
                보유년월일 = yyMMdd,
                순번 = seq,
                보유일 = now,
                품목구분코드 = 품목구분코드,
                //수량 = result == null ? (decimal)1.000 : result.수량 + (decimal)1.000,
            };

            if (result != null)
                dc.보유품목정보.Update(info);
            else
                dc.보유품목정보.Add(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        // 품질검사 시작
        public Task 품질검사시작_보유품목코드_저장(string 생산품코드, string 품목구분코드, decimal 수량,string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyMMdd = now.ToString("yyMMdd");
            //var no = dc.보유품목정보.Count(x => x.품목코드 == 품목.품목코드 && x.보유년월일 == yyMMdd) + 1;
            //decimal count = dc.보유품목정보.Select(x => x.보유품목코드 == 생산품코드);

            var result = (from t in dc.보유품목정보
                          where t.보유품목코드 == 생산품코드
                          select t).DefaultIfEmpty().Single();

            //var result2 = dc.보유품목일련정보.GroupBy(x => x.품목코드 == 생산품코드 && x.순번 == seq).Count();

            var info = new 보유품목정보
            {
                회사코드 = 회사코드,
                보유품목코드 = 생산품코드,
                품목코드 = 생산품코드,
                보유년월일 = yyMMdd,
                순번 = 1,
                보유일 = now,
                품목구분코드 = 품목구분코드,
                //수량 = 수량,
            };

            if (result != null)
                dc.보유품목정보.Update(info);
            else
                dc.보유품목정보.Add(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        /////////////////////////////////// 추가 2021.03.08  ( 보유품목 수량 제외)  ////////////////////////////////////////////////


        public Task<IEnumerable<품질검사측정정보>> 품질검사측정완료유무_조회(string 생산지시코드, string  회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            var result = dc.품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.생산지시코드 == 생산지시코드 && x.회사코드 == 회사코드)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task 품질검사측정_생산지시측정수량_저장(string 생산지시코드, string 합격여부, int 총품질검사수량)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            var result = (from t in dc.생산지시정보
                          where t.생산지시코드 == 생산지시코드
                          select t).DefaultIfEmpty().Single();


            if (합격여부.Equals("합격"))
            {
                result.검사수량 = result.검사수량 + (decimal)1.000;
                result.합격수량 = result.합격수량 + (decimal)1.000;
            }

            else if (합격여부.Equals("불합격"))
            {
                result.검사수량 = result.검사수량 + (decimal)1.000;
                result.불량수량 = result.불량수량 + (decimal)1.000;
            }
            result.실생산량 = 총품질검사수량;


            dc.생산지시정보.Update(result);

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        /////////////////////////////////// 추가 2021.02.16  ( 보유품목코드)  ////////////////////////////////////////////////




        /////////////////////////////////// 추가 2021.03.02  ( 품질검사측정_보유품목일련정보_저장)  ////////////////////////////////////////////////

        public Task 품질검사측정_보유품목일련정보_저장(string 생산품코드, int seq)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");

            DateTime dt = DateTime.Now;
            //var no = dc.보유품목정보.Count(x => x.품목코드 == 품목.품목코드 && x.보유년월일 == yyMMdd) + 1;
            //decimal count = dc.보유품목정보.Select(x => x.보유품목코드 == 생산품코드);

            var result = from t in dc.보유품목일련정보
                         where t.품목코드 == 생산품코드
                         orderby t.보유년월일 descending
                         select t.보유년월일;

            if (result.First() != null)
            {
                string year = result.First();
                var yyyy = now.ToString(year);
                if (dt.Year > Convert.ToInt32(yyyy))
                {
                }
            }
            var result2 = (from t in dc.보유품목일련정보
                           where t.품목코드 == 생산품코드 && t.순번 == seq
                           select t).DefaultIfEmpty().Single();

            var info2 = new 보유품목일련정보
            {
                품목코드 = 생산품코드,
                보유년월일 = yyyyMMdd,
                순번 = seq,
                보유일 = now,
                일년번호 = $"{생산품코드}:{yyyyMMdd}:{seq:00000}",
            };

            if (result2 != null)
                dc.보유품목일련정보.Update(info2);
            else
                dc.보유품목일련정보.Add(info2);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<List<보유품목일련정보>> 품질검사측정_보유품목일련정보_조회(string 품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목일련정보
                .Include(x => x.품목)
                .Where_미삭제_사용()
                .Order_등록최신()
                .Where(x => x.품목코드 == 품목코드).ToList();


            return Task.FromResult(result.ToList());
        }

        public Task<List<보유품목일련정보>> 품질검사측정_보유품목일련정보_상세(string 품목코드, string 보유년월일)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목일련정보
                .Include(x => x.품목)
                .Include(x => x.생산지시)
                .Where_미삭제_사용()
                //.Order_등록최신()
                .Where(x => x.품목코드 == 품목코드 && x.보유년월일 == 보유년월일).ToList();


            return Task.FromResult(result.ToList());
        }


        public Task 품질검사측정_보유품목일련번호생성_저장(string 생산지시코드, string 생산품코드, int count)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            //생산지시코드 = "PP00000:4";
            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            bool newYear = false;
            DateTime dt = DateTime.Now;
            //테스트
            //var yyyyMMdd = now.ToString("20220402");

            var 보유일데이터유무 = from t in dc.보유품목일련정보
                           where t.보유년월일 == yyyyMMdd
                           orderby t.보유년월일 descending
                           select t.보유년월일;


            var 전체데이터유무 = (from t in dc.보유품목일련정보
                           select t).OrderByDescending(c => c.보유년월일).FirstOrDefault();

            if (보유일데이터유무.Count() == 0)
            {
                string year = 전체데이터유무 != null ? 전체데이터유무.보유년월일 : yyyyMMdd;
                var yyyy = now.ToString(year);

                if (dt.Year > Convert.ToInt32(year.Substring(0, 4)))
                {
                    newYear = true;
                }
                //테스트
                //if (dt.AddYears(1).Year > Convert.ToInt32(year.Substring(0, 4)))
                //{
                //	newYear = true;
                //}
            }

            var 생산지시유무체크 = (from t in dc.보유품목일련정보
                            where t.품목코드 == 생산품코드 && t.보유년월일 == yyyyMMdd && t.생산지시코드 == 생산지시코드
                            orderby t.순번 descending
                            select new
                            {
                                순번 = t.순번,
                                보유년월일 = t.보유년월일,
                                생산지시코드 = t.생산지시코드
                            }).FirstOrDefault();     //DefaultIfEmpty().Single();


            if (생산지시유무체크 != null)
            {
                if (생산지시코드 == 생산지시유무체크.생산지시코드 && newYear == false)
                    return Task.CompletedTask;
            }

            var 일년번호 = (from t in dc.보유품목일련정보
                        where t.품목코드 == 생산품코드
                        select t).OrderByDescending(c => c.순번).FirstOrDefault();

            var 보유품목일지 = (from t in dc.보유품목일지
                          where t.품목코드 == 생산품코드
                          select t).OrderByDescending(c => c.순번).FirstOrDefault();


            for (int j = 1; j <= count; j++)
            {
                var info2 = new 보유품목일련정보
                {
                    품목코드 = 생산품코드,
                    보유년월일 = yyyyMMdd,
                    생산년월일 = yyyyMMdd,
                    순번 = newYear == false ? (일년번호 != null ? 일년번호.순번 + j : j) : j,
                    보유일 = now,
                    생산지시코드 = 생산지시코드,
                    일년번호 = $"{생산품코드}:{yyyyMMdd}:{(newYear == false ? (일년번호 != null ? 일년번호.순번 + j : j) : j):00000}",
                    보유품목일지코드 = $"{생산품코드}:{yyyyMMdd}:{count}:{보유품목일지.순번}",
                };

                if (생산지시유무체크 != null)
                    return Task.CompletedTask;
                else
                    dc.보유품목일련정보.Add(info2);
            }
            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task 수입검사_저장(보유품목불량정보 info, bool isAdded)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isAdded == true)
            {
                info.보유품목 = null;
                info.불량유형 = null;
                dc.보유품목불량정보.Add(info);
            }
            else
            {
                dc.보유품목불량정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 수입검사_삭제(보유품목불량정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dc.보유품목불량정보.Remove(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task 보유품목일지_저장(string 생산품코드, int 수량, string 생산지시코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            var no = dc.보유품목일지.Count(x => x.품목코드 == 생산품코드) + 1;

            var info = new 보유품목일지
            {
                //보유품목코드 = $"{품목.품목코드}:{yyMMdd}:{no}",
                보유품목코드 = 생산품코드,
                품목코드 = 생산품코드,
                보유년월일 = yyyyMMdd,
                순번 = no,
                보유일 = now,
                수량 = 수량,
                거래처 = null,
                보유품목일지코드 = $"{생산품코드}:{yyyyMMdd}:{수량}:{no}",
            };

            dc.보유품목일지.Add(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }








        /////////////////////////////
        ///20210531 외주품질검사측정
        ////////////////////////////

        public Task<외주품질검사측정정보> 외주품질검사측정정보유무_조회(int seq, 외주품질검사측정정보 품질검사측정)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.외주품질검사측정정보
                .Where_미삭제_사용()
                .FirstOrDefault(x => x.시리얼넘버 == seq && x.품질검사코드 == 품질검사측정.품질검사코드 && x.회사코드 == 품질검사측정.회사코드 && x.지시번호 == 품질검사측정.지시번호);

            return Task.FromResult(result);
        }

        public Task<IEnumerable<외주품질검사측정정보>> 외주품질검사측정정보_조회(string 지시번호, int seq)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.외주품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.시리얼넘버 == seq && x.지시번호 == 지시번호)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<bool> 외주품질검사측정정보_저장(외주품질검사측정정보 info, bool isAdded)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var 품질검사측정_업데이트 = dc.외주품질검사측정정보.FirstOrDefault(x => x.시리얼넘버 == info.시리얼넘버 && x.회사코드 == info.회사코드 && x.지시번호 == info.지시번호);

            try
            {
                if (isAdded == true)
                {
                    dc.외주품질검사측정정보.Add(info);
                }
                else
                {
                    dc.Entry(품질검사측정_업데이트).Property("RowVersion").OriginalValue = info.RowVersion;
                    dc.외주품질검사측정정보.Update(info);
                }
                dc.SaveChanges();
            }

            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (외주품질검사측정정보)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    return Task.FromResult(false);
                }

                var dbValues = (외주품질검사측정정보)databaseEntry.ToObject();

                // Save the current RowVersion so next postback
                // matches unless an new concurrency issue happens.
                info.RowVersion = (byte[])dbValues.RowVersion;
                // Clear the model error for the next postback.
            }

            return Task.FromResult(true);
        }


        public Task<IEnumerable<외주품질검사측정정보>> 외주품질검사측정완료유무_조회(string 지시번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            var result = dc.외주품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.지시번호 == 지시번호 && x.회사코드 == 회사코드)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task 외주품질검사측정_외주지시측정수량_저장(string 지시번호, string 합격여부, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var 더존외주 = dcDz.VL_MES_WO_WF
                .Where(x => x.CO_CD == 회사코드 && x.WO_CD == 지시번호)
                .FirstOrDefault();

            var result = dc.외주작업지시서품검정보
                .Where(x => x.회사코드 == 회사코드 && x.지시번호 == 지시번호)
                .FirstOrDefault();


            if (result == null)
            {
                var 외주작업지시서 = new 외주작업지시서품검정보
                {
                    회사코드 = 더존외주.CO_CD,
                    지시번호 = 더존외주.WO_CD,
                    지시일 = DateTime.ParseExact(더존외주.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    완료일 = DateTime.ParseExact(더존외주.COMP_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    품번 = 더존외주.ITEM_CD,
                    품명 = 더존외주.ITEM_NM,
                    규격 = 더존외주.ITEM_DC,
                    관리단위 = 더존외주.UNIT_DC,
                    수량 = 더존외주.ITEM_QT,
                    전개순번 = 더존외주.WOOP_SQ,
                    공정 = 더존외주.BASELOC_CD,
                    공정명 = 더존외주.BASELOC_NM,
                    작업장 = 더존외주.LOC_CD,
                    작업장명 = 더존외주.LOC_NM,
                    외주단가 = 더존외주.LBR_UM,
                    외주금액 = 더존외주.LBR_AM,
                    설비코드 = 더존외주.EQUIP_CD,
                    설비명 = 더존외주.EQUIP_NM,
                    비고_DOC_DC = 더존외주.DOC_DC,
                    지시상태 = 더존외주.DOC_ST,
                    지시상태명 = 더존외주.DOC_ST_NM,
                    지시구분 = 더존외주.WOC_FG,
                    지시구분명 = 더존외주.WOC_FG_NM,
                    생산외주구분 = 더존외주.DOC_FG,
                    생산외주구분명 = 더존외주.DOC_FG_NM,
                    처리구분 = 더존외주.WF_FG,
                    처리구분명 = 더존외주.WF_FG_NM,
                    검사구분 = 더존외주.QC_FG,
                    검사구분명 = 더존외주.QC_FG_NM,
                    LOT번호 = 더존외주.LOT_NB,
                    거래처코드 = 더존외주.TR_CD,
                    거래처명 = 더존외주.TR_NM,
                    거래처약칭 = 더존외주.ATTR_NM,
                    주문번호 = 더존외주.SO_NB,
                    주문순번 = 더존외주.LN_SQ,
                    사업장코드 = 더존외주.DIV_CD,
                    작업팀 = 더존외주.WTEAM_CD,
                    작업팀명 = 더존외주.WTEAM_NM,
                    작업조 = 더존외주.WSHFT_CD,
                    작업조명 = 더존외주.WSHFT_NM,
                    비고 = 더존외주.REMARK_DC,
                   
                };

                if (합격여부.Equals("합격"))
                {
                    외주작업지시서.검사수량 =  (decimal)1.000;
                    외주작업지시서.합격수량 =  (decimal)1.000;
                }
                else if (합격여부.Equals("불합격"))
                {
                    외주작업지시서.검사수량 = (decimal)1.000;
                    외주작업지시서.불량수량 =  (decimal)1.000;
                }
                dc.외주작업지시서품검정보.Add(외주작업지시서);
            }
            else
            {
                if (합격여부.Equals("합격"))
                {
                    result.검사수량 = result.검사수량 + (decimal)1.000;
                    result.합격수량 = result.합격수량 + (decimal)1.000;
                }

                else if (합격여부.Equals("불합격"))
                {
                    result.검사수량 = result.검사수량 + (decimal)1.000;
                    result.불량수량 = result.불량수량 + (decimal)1.000;
                }
                dc.외주작업지시서품검정보.Update(result);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        //////// 수입검사
        ///

        public Task<발주서별품질검사측정정보> 수입검사품질검사측정정보유무_조회(int seq, 발주서별품질검사측정정보 품질검사측정)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.발주서별품질검사측정정보
                .Where_미삭제_사용()
                .FirstOrDefault(x => x.시리얼넘버 == seq && x.품질검사코드 == 품질검사측정.품질검사코드
                && x.회사코드 == 품질검사측정.회사코드 && x.발주번호 == 품질검사측정.발주번호 && x.발주순번 == 품질검사측정.발주순번 );

            return Task.FromResult(result);
        }

        public Task<IEnumerable<발주서별품질검사측정정보>> 수입검사품질검사측정정보_조회(string 발주번호,decimal 발주순번, int seq)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.발주서별품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.시리얼넘버 == seq && x.발주번호 == 발주번호 && x.발주순번 == 발주순번)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<bool> 수입검사품질검사측정정보_저장(발주서별품질검사측정정보 info, bool isAdded)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var 품질검사측정_업데이트 = dc.발주서별품질검사측정정보.FirstOrDefault(x => x.시리얼넘버 == info.시리얼넘버 
            && x.회사코드 == info.회사코드 && x.발주번호 == info.발주번호 && x.발주순번 == info.발주순번);

            try
            {
                if (isAdded == true)
                {
                    dc.발주서별품질검사측정정보.Add(info);
                }
                else
                {
                    dc.Entry(품질검사측정_업데이트).Property("RowVersion").OriginalValue = info.RowVersion;
                    dc.발주서별품질검사측정정보.Update(info);
                }
                dc.SaveChanges();
            }

            catch (DbUpdateConcurrencyException ex)
            {
                var exceptionEntry = ex.Entries.Single();
                var clientValues = (발주서별품질검사측정정보)exceptionEntry.Entity;
                var databaseEntry = exceptionEntry.GetDatabaseValues();
                if (databaseEntry == null)
                {
                    return Task.FromResult(false);
                }

                var dbValues = (발주서별품질검사측정정보)databaseEntry.ToObject();

                // Save the current RowVersion so next postback
                // matches unless an new concurrency issue happens.
                info.RowVersion = (byte[])dbValues.RowVersion;
                // Clear the model error for the next postback.
            }

            return Task.FromResult(true);
        }

        public Task<IEnumerable<발주서별품질검사측정정보>> 수입검사품질검사측정완료유무_조회(string 발주번호,decimal 발주순번, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            var result = dc.발주서별품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.발주번호 == 발주번호 && x.발주순번 == 발주순번 && x.회사코드 == 회사코드)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task 수입검사품질검사측정_측정수량_저장(string 발주번호, decimal 발주순번, string 합격여부, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var 더존발주 = dcDz.VL_MES_PO
               .Where(x => x.CO_CD == 회사코드 && x.PO_NB == 발주번호 && x.PO_SQ == 발주순번)
               .FirstOrDefault();

            var result = dc.발주서별수입검사
                .Where(x => x.회사코드 == 회사코드 && x.발주번호 == 발주번호 && x.발주순번 == 발주순번)
                .FirstOrDefault();

            if (result == null)
            {
                var 발주서수입 = new 발주서별수입검사
                {
                    회사코드 = 더존발주.CO_CD,
                    사업장코드 = 더존발주.DIV_CD,
                    부서코드 = 더존발주.DEPT_CD,
                    사원코드 = 더존발주.EMP_CD,
                    발주번호 = 더존발주.PO_NB,
                    발주일 = 더존발주.PO_DT,
                    거래처코드 = 더존발주.TR_CD,
                    거래처명 = 더존발주.TR_NM,
                    거래구분 = 더존발주.PO_FG,
                    검사구분 = 더존발주.QC_FG,
                    과세구분 = 더존발주.VAT_FG,
                    과세구분명 = 더존발주.VAT_NM,
                    담당자코드 = 더존발주.PLN_CD,
                    담당자명 = 더존발주.PLN_NM,
                    비고 = 더존발주.REMARK_DC,
                    발주순번 = 더존발주.PO_SQ,
                    품번 = 더존발주.ITEM_CD,
                    품명 = 더존발주.ITEM_NM,
                    규격 = 더존발주.ITEM_DC,
                    관리단위 = 더존발주.UNITMANG_DC,
                    납기일 = 더존발주.DUE_DT,
                    출하예정일 = 더존발주.SHIPREQ_DT,
                    발주수량 = 더존발주.PO_QT,
                    발주단가 = 더존발주.PO_UM,
                    공급가 = 더존발주.POG_AM,
                    부가세 = 더존발주.POGV_AM1,
                    합계액 = 더존발주.POGH_AM1,
                    관리구분코드 = 더존발주.MGMT_CD,
                    관리구분명 = 더존발주.MGM_NM,
                    프로젝트 = 더존발주.PJT_CD,
                    프록젝트명 = 더존발주.PJT_NM,
                    비고_내역 = 더존발주.REMARK_DC_D,
                    환종 = 더존발주.EXCH_CD,
                    부가세구분 = 더존발주.UMVAT_FG,

                 };

                if (합격여부.Equals("합격"))
                {
                    발주서수입.검사수량 = (decimal)1.000;
                    발주서수입.합격수량 = (decimal)1.000;
                }
                else if (합격여부.Equals("불합격"))
                {
                    발주서수입.검사수량 = (decimal)1.000;
                    발주서수입.불량수량 = (decimal)1.000;
                }
                dc.발주서별수입검사.Add(발주서수입);
            }
            else
            {
                if (합격여부.Equals("합격"))
                {
                    result.검사수량 = result.검사수량 + (decimal)1.000;
                    result.합격수량 = result.합격수량 + (decimal)1.000;
                }

                else if (합격여부.Equals("불합격"))
                {
                    result.검사수량 = result.검사수량 + (decimal)1.000;
                    result.불량수량 = result.불량수량 + (decimal)1.000;
                }
                dc.발주서별수입검사.Update(result);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }



        public Task<IEnumerable<발주서별품질검사측정정보>> 발주서별품질검사측정완료유무_조회(string 발주번호,decimal 발주순번, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.발주서별품질검사측정정보
                .Where_미삭제_사용()
                .Where(x => x.발주번호 == 발주번호 && x.발주순번 == 발주순번 && x.회사코드 == 회사코드)
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


      
    }
}
