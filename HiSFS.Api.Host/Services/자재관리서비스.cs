using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;
using HiSFS.Api.Shared.Models.View_DZICUBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HiSFS.Api.Host.Services
{
    public class 자재관리서비스 : I자재관리서비스
    {
        private readonly IContextProvider dcp;



        public 자재관리서비스(IContextProvider dbContextProvider)
        {
            this.dcp = dbContextProvider;
        }


        public Task<IEnumerable<보유품목정보>> 자재현황_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목정보
                    .Include(x => x.품목)
                    .Include(x => x.장소).Where(x => x.회사코드 == 회사코드)
                    .Include(x => x.장소위치)
                    .ThenInclude(y => y.위치상세목록).Where(y => y.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Where(x => x.품목.품목구분코드 != "B1205" && x.회사코드 == 회사코드)     // 설비는 제외
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<보유품목이력>> 자재현황_이력_조회(string 보유품목코드, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목이력
                .Include(x => x.장소).Where(x => x.회사코드 == 회사코드 && x.보유품목코드 == 보유품목코드)
                .Include(x => x.변경유형).Where(x => x.회사코드 == 회사코드 && x.보유품목코드 == 보유품목코드)
                .Include(x => x.위치)
                .ThenInclude(y => y.위치상세목록).Where(y => y.회사코드 == 회사코드 && y.보유품목코드 == 보유품목코드)
                    .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<보유품목이력>> 자재현황_입고_조회(string 변경유형코드, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목이력
                .Include(x => x.장소)
                .Include(x => x.위치)
                .Include(x => x.연계보유품목)
                    .ThenInclude(x => x.품목)
                    .Where(x => x.변경유형코드 == 변경유형코드 && x.회사코드 == 회사코드)
                    .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }



        public Task 보유품목코드_저장(보유품목정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyMMdd = now.ToString("yyMMdd");
            var result = (from t in dc.보유품목정보
                          where t.보유품목코드 == info.품목.품목코드
                          select t).DefaultIfEmpty().Single();

            var s_info = new 보유품목정보
            {
                보유품목코드 = info.품목.품목코드,
                품목코드 = info.품목.품목코드,
                품목구분코드 = info.품목.품목구분코드,
                보유년월일 = yyMMdd,
                순번 = 1,
                보유일 = now,
                수량 = info.수량
            };

            if (result != null)
                dc.보유품목정보.Update(s_info);
            else
                dc.보유품목정보.Add(s_info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<string> 보유품목_발행(품목정보 품목, 보유품목정보 보유품목)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyMMdd = now.ToString("yyMMdd");
            var no = dc.보유품목정보.Count(x => x.품목코드 == 품목.품목코드) + 1;
            var result = dc.보유품목정보.Where(x => x.품목코드 == 품목.품목코드).FirstOrDefault();

            var info = new 보유품목정보
            {
                //보유품목코드 = $"{품목.품목코드}:{yyMMdd}:{no}",
                보유품목코드 = 품목.품목코드,
                품목코드 = 품목.품목코드,
                보유년월일 = 보유품목.보유년월일,
                순번 = no,
                보유일 = now,
                수량 = result != null ? result.수량 + 보유품목.수량 : 보유품목.수량,
                품목구분코드 = 품목.품목구분코드,
            };

            if (result != null)
                dc.보유품목정보.Update(info);
            else
                dc.보유품목정보.Add(info);

            dc.SaveChanges();

            return Task.FromResult(info.보유품목코드);
        }


        public Task 보유품목일지_저장(품목정보 품목, 보유품목정보 보유품목)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            var no = dc.보유품목일지.OrderByDescending(x => x.순번).Where(x => x.품목코드 == 품목.품목코드).FirstOrDefault();

            int num = 0;
            if (no != null)
                num = no.순번 + 1;
            else
                num = 1;

            var info = new 보유품목일지
            {
                //보유품목코드 = $"{품목.품목코드}:{yyMMdd}:{no}",
                보유품목코드 = 품목.품목코드,
                품목코드 = 품목.품목코드,
                보유년월일 = 보유품목.보유년월일,
                순번 = num,
                보유일 = now,
                수량 = 보유품목.수량,
                거래처 = null,
                보유품목일지코드 = $"{ 품목.품목코드}:{보유품목.보유년월일}:{보유품목.수량}:{num}",
            };

            dc.보유품목일지.Add(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<보유품목일지>> 보유품목일지_조회(string 보유품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목일지
                    .Include(x => x.품목)
                .Where_미삭제_사용()
                .Order_등록최신()
                .Where(x => x.보유품목코드 == 보유품목코드).ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task<List<보유품목일련정보>> 보유품목일련정보_상세(string 보유품목일지코드, string 품목코드, string 보유년월일)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목일련정보
                .Include(x => x.품목)
                .Include(x => x.생산지시)
                .Where_미삭제_사용()
                //.Order_등록최신()
                .Where(x => x.보유품목일지코드 == 보유품목일지코드 && x.보유년월일 == 보유년월일).ToList();


            return Task.FromResult(result.ToList());
        }

        public Task<int> 보유품목일련정보_마지막순번조회(string 품목코드, DateTime 보유년월일)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var yyyy = 보유년월일.ToString("yyyy");
            string str_result;

            try
            {
                str_result = dc.보유품목일련정보
               //.Where_미삭제_사용()
               //.Order_등록최신()
               .OrderByDescending(x => x.순번)
               .Where(x => x.품목코드 == 품목코드 && x.보유년월일.Contains(yyyy)).FirstOrDefault().일년번호;
            }
            catch (Exception)
            {
                str_result = "";

            }
            if (str_result != "")
            {
                //5 - 000 - 000 - 00:20210406:00014
                string[] array = str_result.Split(":");

                str_result = array[2];

            }
            else
            {
                str_result = "0";

            }
            var result = Convert.ToInt32(str_result);


            return Task.FromResult(result);
        }


        public Task 자재관리_보유품목일련번호생성_저장(품목정보 품목, 보유품목정보 보유품목, DateTime 보유년월일, DateTime 생산년월일, string 사용자일련번호)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            //생산지시코드 = "PP00000:4";
            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");

            var str보유년월일 = 보유년월일.ToString("yyyyMMdd");
            var str생산년월일 = 생산년월일.ToString("yyyyMMdd");

            bool newYear = false;
            DateTime dt = DateTime.Now;



            var 보유일데이터유무 = from t in dc.보유품목일련정보
                           where t.보유년월일 == str보유년월일
                           orderby t.보유년월일 descending
                           select t.보유년월일;

            var 전체데이터유무 = (from t in dc.보유품목일련정보
                           select t).OrderByDescending(c => c.보유년월일).FirstOrDefault();

            if (보유일데이터유무.Count() == 0)
            {
                string year = 전체데이터유무 != null ? 전체데이터유무.보유년월일 : str보유년월일;
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

            var 일년번호 = (from t in dc.보유품목일련정보
                        where t.품목코드 == 품목.품목코드
                        select t).OrderByDescending(c => c.순번).FirstOrDefault();

            var 보유품목일지 = (from t in dc.보유품목일지
                          where t.품목코드 == 품목.품목코드
                          select t).OrderByDescending(c => c.순번).FirstOrDefault();

            //var no = dc.보유품목일련정보.OrderByDescending(x => x.품목코드 == 품목.품목코드 && x.보유년월일.Contains(보유품목.보유년월일.Substring(0, 4))) ;
            var no = dc.보유품목일련정보.OrderByDescending(x => x.순번).Where(x => x.품목코드 == 품목.품목코드 && x.보유년월일.Contains(보유품목.보유년월일.Substring(0, 4))).FirstOrDefault();

            for (int j = 1; j <= 보유품목.수량; j++)
            {

                if (사용자일련번호 != "")
                {
                    var info2 = new 보유품목일련정보
                    {
                        품목코드 = 품목.품목코드,
                        보유년월일 = str보유년월일,
                        //순번 = newYear == false ? (no != null ? no + j : j) : j,
                        순번 = no != null ? no.순번 + j : j,
                        보유일 = now,
                        일년번호 = $"{품목.품목코드}:{str보유년월일}:{(newYear == false ? (사용자일련번호 != "" ? Convert.ToInt32(사용자일련번호) - 1 + j : j) : j):00000}",
                        생산년월일 = str생산년월일,
                        보유품목일지코드 = $"{ 품목.품목코드 }:{str보유년월일}:{ 보유품목.수량 }:{보유품목일지.순번}",

                    };
                    dc.보유품목일련정보.Add(info2);

                }
                //            else
                //{
                //                var info2 = new 보유품목일련정보
                //                {
                //                    품목코드 = 품목.품목코드,
                //                    보유년월일 = str보유년월일,
                //                    순번 = newYear == false ? (일년번호 != null ? 일년번호.순번 + j : j) : j,
                //                    보유일 = now,
                //                    일년번호 = $"{품목.품목코드}:{str보유년월일}:{(newYear == false ? (일년번호 != null ? 일년번호.순번 + j : j) : j):00000}",
                //                    생산년월일 = str생산년월일,
                //                    보유품목일지코드 = $"{ 품목.품목코드 }:{str보유년월일}:{ 보유품목.수량 }:{보유품목일지.순번}",

                //                };
                //                dc.보유품목일련정보.Add(info2);

                //            }




            }
            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 자재관리_보유품목_입고(string 보유품목코드, decimal 수량, string 장소코드, string 사유)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            /*
             var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드);

             // 보유품목 등록이 되어 있지 않으면 무시한다.
             if (info == default)
                 return Task.CompletedTask;

             // 보유품목 수량 변경
             info.수량 += 수량;
             // 장소 변경
             info.장소코드 = 장소코드;
             dc.보유품목정보.Update(info);
            */

            // 보유품목 이력 추가
            var log = new 보유품목이력
            {
                보유품목코드 = 보유품목코드,
                연계보유품목코드 = 보유품목코드,
                변경유형코드 = "B1701",    // 입고
                장소코드 = 장소코드,
                장소위치코드 = null,
                변경수량 = 수량,
                변경사유 = "입고 처리",
                유형사유 = 사유,
                변경일시 = DateTime.Now
            };
            dc.보유품목이력.Add(log);

            dc.SaveChanges();

            return Task.CompletedTask; ;
        }




        public void 보유품목_입고(string 보유품목코드, decimal 수량, string 장소코드, string 사유)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // 보유품목 선택
            var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드);
            // 보유품목 등록이 되어 있지 않으면 무시한다.
            if (info == default)
                return;

            // 보유품목 수량 변경
            info.수량 += 수량;
            // 장소 변경
            info.장소코드 = 장소코드;
            dc.보유품목정보.Update(info);

            // 보유품목 이력 추가
            var log = new 보유품목이력
            {
                보유품목코드 = 보유품목코드,
                연계보유품목코드 = 보유품목코드,
                변경유형코드 = "B1701",    // 입고
                장소코드 = 장소코드,
                장소위치코드 = null,
                변경수량 = 수량,
                변경사유 = "입고 처리",
                유형사유 = 사유,
                변경일시 = DateTime.Now
            };
            dc.보유품목이력.Add(log);

            dc.SaveChanges();
        }

        public void 보유품목_출고(string 보유품목코드, decimal 수량, string 장소코드, string 위치코드, string 사유)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // 보유품목 선택
            var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드);
            // 보유품목 등록이 되어 있지 않으면 무시한다.
            if (info == default)
                return;

            // 보유품목 수량 변경
            //var pum_수량 = info.수량;
            info.수량 -= 수량;
            // 장소 변경
            info.장소코드 = 장소코드;
            dc.보유품목정보.Update(info);
            dc.SaveChanges();

            if (위치코드 != null) //위치반출일 경우 위치 수량 변경
            {
                var w_info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드 && x.장소위치코드 == 위치코드);
                if (w_info != default)
                {

                    w_info.수량 -= 수량;
                    if (w_info.수량 > 0)
                    {
                        if (w_info.수량 > info.수량)
                            w_info.수량 = info.수량;

                        dc.보유품목위치정보.Update(w_info);
                        dc.SaveChanges();
                    }
                }
            }

            // 보유품목 이력 추가
            var log = new 보유품목이력
            {
                보유품목코드 = 보유품목코드,
                연계보유품목코드 = 보유품목코드,
                변경유형코드 = "B1702",    // 출고
                장소코드 = 장소코드,
                장소위치코드 = 위치코드,
                변경수량 = 수량,
                변경사유 = "출고 처리",
                유형사유 = 사유,
                변경일시 = DateTime.Now
            };
            dc.보유품목이력.Add(log);

            dc.SaveChanges();

        }

        public void 보유품목_불량등록(string 보유품목코드, decimal 수량, string 불량유형코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드);
            // 보유품목이 없을 경우 무시한다.
            if (보유품목 == default)
                return;

            var info = dc.보유품목불량정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드 && x.불량유형코드 == 불량유형코드);
            // 추가
            if (info == default)
            {
                info = new 보유품목불량정보
                {
                    보유품목코드 = 보유품목코드,
                    불량유형코드 = 불량유형코드,
                    불량등록일시 = DateTime.Now,
                    불량변경일시 = null,
                    수량 = 수량
                };
                dc.보유품목불량정보.Add(info);
            }
            // 변경
            else
            {
                info.수량 += 수량;
                if (보유품목.수량 > info.수량)
                    info.수량 = 보유품목.수량;
                info.불량변경일시 = DateTime.Now;

                dc.보유품목불량정보.Update(info);
            }

            dc.SaveChanges();
        }

        /// 2021.03.15
        public void 보유품목_위치등록(string 보유품목코드, decimal 수량, string 장소위치코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드);
            // 보유품목이 없을 경우 무시한다.
            if (보유품목 == default)
                return;

            var info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드 && x.장소위치코드 == 장소위치코드);
            // 추가
            if (info == default)
            {
                info = new 보유품목위치정보
                {
                    보유품목코드 = 보유품목코드,
                    장소위치코드 = 장소위치코드,
                    수량 = 수량
                };
                dc.보유품목위치정보.Add(info);
            }
            // 변경
            else
            {
                info.수량 += 수량;
                if (보유품목.수량 > info.수량)
                    info.수량 = 보유품목.수량;

                dc.보유품목위치정보.Update(info);
            }

            var log = new 보유품목이력
            {
                보유품목코드 = 보유품목코드,
                연계보유품목코드 = 보유품목코드,
                변경유형코드 = "B1701",    // 입고
                장소코드 = null,
                장소위치코드 = 장소위치코드,
                변경수량 = 수량,
                변경사유 = "위치배치",
                유형사유 = "S921902",
                변경일시 = DateTime.Now
            };
            dc.보유품목이력.Add(log);


            dc.SaveChanges();
        }




        // [추가] 2021.03.15 
        public Task<IEnumerable<보유품목이력>> 자재현황_입고분기_조회(string 변경유형코드, string datetime, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            DateTime now = new DateTime();
            DateTime now2 = new DateTime();
            now = DateTime.Now;
            if (datetime == "one")
                now2 = now.AddMonths(-1);
            else if (datetime == "three")
                now2 = now.AddMonths(-3);
            else if (datetime == "six")
                now2 = now.AddMonths(-6);
            else if (datetime == "twelve")
                now2 = now.AddMonths(-12);

            if (datetime == "all")
            {

                var result = dc.보유품목이력
               .Include(x => x.장소)
               .Include(x => x.위치)
               .Include(x => x.연계보유품목)
                   .ThenInclude(x => x.품목)
                   .Where(x => x.변경유형코드 == 변경유형코드 && x.회사코드 == 회사코드)
                   .Where_미삭제_사용()
               .Order_등록최신()
               .ToList();

                return Task.FromResult(result.AsEnumerable());
            }
            else
            {
                var result = dc.보유품목이력
            .Include(x => x.장소)
            .Include(x => x.위치)
            .Include(x => x.연계보유품목)
                .ThenInclude(x => x.품목)
                .Where(x => x.변경유형코드 == 변경유형코드 && x.변경일시 >= now2 && x.회사코드 == 회사코드)
                .Where_미삭제_사용()
            .Order_등록최신()
            .ToList();

                return Task.FromResult(result.AsEnumerable());
            }

        }

        // 2021.04.01 PDA 출고
        public Task 자재관리_보유품목일련번호생성_Update(string _보유품목코드, decimal 수량, string 장소코드, string 위치코드, string 사유)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            //생산지시코드 = "PP00000:4";
            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");

            var 보유품목 = _보유품목코드.Split(':');
            var 품목코드 = 보유품목[0];
            var 보유년월 = 보유품목[1];
            var 일련번호자리수 = 보유품목[2].Length;

            bool 일련번호_flag = false;
            if (일련번호자리수 == 5)
                일련번호_flag = true;

            var 일련번호 = Convert.ToInt32(보유품목[2]);

            if (일련번호_flag)
            {
                //var no = dc.보유품목정보.Count(x => x.품목코드 == 품목.품목코드 && x.보유년월일 == yyMMdd) + 1;
                //decimal count = dc.보유품목정보.Select(x => x.보유품목코드 == 생산품코드);

                var result = (from t in dc.보유품목일련정보
                              where t.일년번호 == _보유품목코드
                              select t).DefaultIfEmpty().Single();


                if (result != null)
                {
                    result.출고년월일 = yyyyMMdd;
                    result.거래처코드 = null;
                    dc.보유품목일련정보.Update(result);
                }
            }
            else
            {
                var result = (from t in dc.보유품목일지
                              where t.보유품목일지코드 == _보유품목코드
                              select t).DefaultIfEmpty().Single();

                if (result != null)
                {
                    result.출고년월일 = yyyyMMdd;
                    result.거래처코드 = null;
                    dc.보유품목일지.Update(result);

                    var result2 = (from t in dc.보유품목일련정보
                                   where t.보유품목일지코드 == _보유품목코드
                                   select t).ToList();


                    if (result2 != null)
                    {
                        foreach (var item in result2)
                        {
                            item.출고년월일 = yyyyMMdd;
                            item.거래처코드 = null;

                            dc.보유품목일련정보.Update(item);
                        }

                    }
                }




            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task 자재관리_보유품목일련번호생성_PDA입고(string 보유품목코드, decimal 수량) //, DateTime 보유년월일, DateTime 생산년월일)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            //생산지시코드 = "PP00000:4";
            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");

            var str보유년월일 = yyyyMMdd; // 보유년월일.ToString("yyyyMMdd");
            var str생산년월일 = yyyyMMdd; // 생산년월일.ToString("yyyyMMdd");

            bool newYear = false;
            DateTime dt = DateTime.Now;

            int pum_Count = Convert.ToInt32(수량);

            var 보유일데이터유무 = from t in dc.보유품목일련정보
                           where t.보유년월일 == str보유년월일
                           orderby t.보유년월일 descending
                           select t.보유년월일;

            var 전체데이터유무 = (from t in dc.보유품목일련정보
                           select t).OrderByDescending(c => c.보유년월일).FirstOrDefault();

            if (보유일데이터유무.Count() == 0)
            {
                string year = 전체데이터유무 != null ? 전체데이터유무.보유년월일 : str보유년월일;
                var yyyy = now.ToString(year);

                if (dt.Year > Convert.ToInt32(year.Substring(0, 4)))
                {
                    newYear = true;
                }

                //테스트
                //if (dt.AddYears(1).Year > Convert.ToInt32(year.Substring(0, 4)))
                //{
                //   newYear = true;
                //}
            }

            var 일년번호 = (from t in dc.보유품목일련정보
                        where t.품목코드 == 보유품목코드
                        select t).OrderByDescending(c => c.순번).FirstOrDefault();

            var 보유품목일지 = (from t in dc.보유품목일지
                          where t.품목코드 == 보유품목코드
                          select t).OrderByDescending(c => c.순번).FirstOrDefault();

            for (int j = 1; j <= pum_Count; j++)
            {
                var info2 = new 보유품목일련정보
                {
                    품목코드 = 보유품목코드,
                    보유년월일 = str보유년월일,
                    순번 = newYear == false ? (일년번호 != null ? 일년번호.순번 + j : j) : j,
                    보유일 = now,
                    일년번호 = $"{보유품목코드}:{str보유년월일}:{(newYear == false ? (일년번호 != null ? 일년번호.순번 + j : j) : j):00000}",
                    생산년월일 = str생산년월일,
                    보유품목일지코드 = $"{ 보유품목코드 }:{str보유년월일}:{ pum_Count }:{보유품목일지.순번}",
                };

                dc.보유품목일련정보.Add(info2);
            }
            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 보유품목일지_PDA저장(string 보유품목코드, decimal 수량)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            var no = dc.보유품목일지.Count(x => x.품목코드 == 보유품목코드) + 1;

            var info = new 보유품목일지
            {
                //보유품목코드 = $"{품목.품목코드}:{yyMMdd}:{no}",
                보유품목코드 = 보유품목코드,
                품목코드 = 보유품목코드,
                보유년월일 = yyyyMMdd,
                순번 = no,
                보유일 = now,
                수량 = 수량,
                거래처 = null,
                보유품목일지코드 = $"{보유품목코드}:{yyyyMMdd}:{수량}:{no}",
            };

            dc.보유품목일지.Add(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }



        public Task<bool> 보유품목일지_삭제(보유품목일지 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool completed = false;

            try
            {
                var now = DateTime.Now;
                var hhmmss = now.ToString("hhmmss");


                var 보유품목일련정보 = dc.보유품목일련정보.Where(x => x.보유품목일지코드 == info.보유품목일지코드);

                var 보유품목정보 = (from t in dc.보유품목정보
                              where t.보유품목코드 == info.보유품목코드
                              select t);

                foreach (var item in 보유품목정보)
                {
                    item.수량 = item.수량 - info.수량;

                    dc.보유품목정보.Update(item);
                }
                var 보유품목삭제일지 = new 보유품목삭제일지()
                {

                    보유년월일 = info.보유년월일,
                    생산년월일 = info.생산년월일,
                    출고년월일 = info.출고년월일,
                    보유일 = info.보유일,
                    보유품목 = info.보유품목,
                    보유품목코드 = info.보유품목코드,
                    수량 = info.수량,
                    순번 = info.순번,
                    보유품목일지코드 = $"{ info.보유품목일지코드}:{hhmmss}"
                };
                dc.보유품목삭제일지.Add(보유품목삭제일지);

                if (보유품목일련정보.Count() > 0)
                {
                    foreach (var item in 보유품목일련정보)
                    {

                        item.보유품목일지 = null;
                        dc.보유품목일련정보.Remove(item);

                    }

                }


                info.품목 = null;
                dc.보유품목일지.Remove(info);
                dc.SaveChanges();

                completed = true;

            }
            catch (Exception ex)
            {
                completed = false;
            }


            return Task.FromResult(completed);
        }


        // ERP 보유품목 입고 처리  4.26 수정

        //ERP 입고이력등록
        public Task 자재관리_입고관리보유품목이력_등록(보유품목정보 보유품목, decimal 수량, string 장소코드, string 사유)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            // 보유품목 이력 추가
            var log = new 보유품목이력
            {
                회사코드 = 보유품목.회사코드,
                보유품목코드 = 보유품목.품목코드,
                연계보유품목코드 = 보유품목.품목코드,
                변경유형코드 = "B1701",    // 입고
                장소코드 = 장소코드,
                장소위치코드 = null,
                변경수량 = 수량,
                변경사유 = "입고 처리",
                유형사유 = 사유,
                변경일시 = DateTime.Now
            };
            dc.보유품목이력.Add(log);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task<string> 입고관리보유품목입고_등록(입고처리상세정보 상세, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            var no = dc.보유품목정보.Count(x => x.품목코드 == 상세.품번 && x.회사코드 == 상세.회사코드) + 1;
            var result = dc.보유품목정보.Where(x => x.품목코드 == 상세.품번 && x.회사코드 == 상세.회사코드).FirstOrDefault();

            var 입고처리상세 = dc.입고처리상세정보.Where(x => x.품번 == 상세.품번 && x.회사코드 == 상세.회사코드 &&
                            x.작업순번 == 상세.작업순번).FirstOrDefault();

            var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 상세.품번).FirstOrDefault();
            decimal 입고상세수량 = 0;
            if (입고처리상세 != null)
                입고상세수량 = 상세.입고수량_관리단위 - 입고처리상세.입고수량_관리단위;

            string 장소코드 = "";
            string 장소위치코드 = "";

            장소코드 = 상세.입고장소코드.Substring(0, 4);


            var info = new 보유품목정보
            {
                회사코드 = 상세.회사코드,
                보유품목코드 = 상세.품번,
                품목코드 = 품목정보.품목코드,
                보유년월일 = result != null ? result.보유년월일 : yyyyMMdd,
                조정년월일 = null,//result.조정년월일,
                보유일 = now,
                수량 = isAdd == true ? 상세.입고수량_관리단위 : (result != null ? result.수량 + 입고상세수량 : 입고상세수량),
                품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                장소코드 = result != null ? result.장소코드 : 장소코드,
                장소위치코드 = result != null ? result.장소위치코드 : 상세.입고장소코드,
            };

            if (result == null)
            {
                dc.보유품목정보.Add(info);
                dc.SaveChanges();
            }
            else
            {
                dc.보유품목정보.Update(info);
                dc.SaveChanges();
            }


            return Task.FromResult(info.보유품목코드);
        }


        public Task<string> 출고관리보유품목입고_등록(출고처리상세정보 상세, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            var no = dc.보유품목정보.Count(x => x.품목코드 == 상세.품번 && x.회사코드 == 상세.회사코드) + 1;
            var result = dc.보유품목정보.Where(x => x.품목코드 == 상세.품번 && x.회사코드 == 상세.회사코드).FirstOrDefault();

            if (result == null)
                return Task.FromResult("false");

            var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 상세.품번).FirstOrDefault();

            var 출고처리상세 = dc.출고처리상세정보.Where(x => x.품번 == 상세.품번 && x.회사코드 == 상세.회사코드 &&
                           x.작업순번 == 상세.작업순번).FirstOrDefault();

            decimal 출고처리상세수량 = 0;
            if (출고처리상세 != null)
                출고처리상세수량 = 상세.출고수량_관리단위 - 출고처리상세.출고수량_관리단위;

            string 장소코드 = "";

            장소코드 = 상세.장소코드.Substring(0, 4);

            if (result.수량 < 상세.출고수량_관리단위)
                return Task.FromResult("false");

            var info = new 보유품목정보
            {
                회사코드 = 상세.회사코드,
                보유품목코드 = 상세.품번,
                품목코드 = 상세.품번,
                보유년월일 = result != null ? result.보유년월일 : yyyyMMdd,
                조정년월일 = null,//result.조정년월일,
                순번 = no,
                보유일 = now,
                수량 = result.수량 - 상세.출고수량_관리단위,
                품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                //장소코드 = 장소코드,
                //장소위치코드 = 상세.장소코드 ,

            };

            dc.보유품목정보.Update(info);
            dc.SaveChanges();

            return Task.FromResult(info.보유품목코드);
        }


        public Task<bool> 보유품목입고_위치등록(입고처리상세정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            try
            {
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                var info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);

                if (isAdd)
                {
                    if (info == default)
                    {
                        info = new 보유품목위치정보
                        {
                            회사코드 = 보유품목_P.회사코드,
                            보유품목코드 = 보유품목_P.품번,
                            장소위치코드 = 보유품목_P.입고장소코드,
                            수량 = 보유품목_P.입고수량_관리단위,

                        };
                        dc.보유품목위치정보.Add(info);
                    }
                    // 변경
                    else
                    {

                        info.수량 = info.수량 + 보유품목_P.입고수량_관리단위;
                        dc.보유품목위치정보.Update(info);
                    }
                }
                else
                {
                    if (info == default)
                    {
                        info = new 보유품목위치정보
                        {
                            회사코드 = 보유품목_P.회사코드,
                            보유품목코드 = 보유품목_P.품번,
                            장소위치코드 = 보유품목_P.입고장소코드,
                            수량 = 보유품목_P.입고수량_관리단위,

                        };
                        dc.보유품목위치정보.Add(info);

                        var info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == P_장소위치코드);
                        info1.수량 = info1.수량 - (info1.수량 - 보유품목_P.입고수량_관리단위);
                        dc.보유품목위치정보.Update(info1);

                    }
                    // 변경
                    else
                    {
                        var info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == P_장소위치코드);

                        info.수량 = info1.수량 - (info1.수량 - 보유품목_P.입고수량_관리단위);

                        dc.보유품목위치정보.Update(info);

                    }
                }

                dc.SaveChanges();

                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);

                var log = new 보유품목이력
                {
                    회사코드 = 보유품목_P.회사코드,
                    보유품목코드 = 보유품목_P.품번,
                    연계보유품목코드 = 보유품목_P.품번,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = 보유품목_P.입고장소코드.Substring(0, 4),
                    장소위치코드 = 보유품목_P.입고장소코드,
                    변경수량 = 보유품목_P.입고수량_관리단위,
                    변경사유 = 입출고여부,
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                if (보유이력 == null)
                    dc.보유품목이력.Add(log);

                dc.SaveChanges();

            }
            catch (Exception ex)
            {
                result = false;
            }


            result = true;

            return Task.FromResult(result);


        }



        public Task<bool> 보유품목출고_위치등록(보유품목정보 보유품목_P, string 입출고여부, string 사유)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            try
            {
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                var info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드 && x.장소위치코드 == 보유품목_P.장소위치코드);
                // 추가
                //if (info == default)
                //{
                //    info = new 보유품목위치정보
                //    {
                //        회사코드 = 보유품목_P.회사코드,
                //        보유품목코드 = 보유품목_P.보유품목코드,
                //        장소위치코드 = 보유품목_P.장소위치코드,
                //        수량 = -보유품목_P.수량
                //    };
                //    dc.보유품목위치정보.Add(info);
                //}
                // 변경
                if (info != null)
                {

                    info.수량 = info.수량 - 보유품목_P.수량;

                    dc.보유품목위치정보.Update(info);
                }

                dc.SaveChanges();
                var log = new 보유품목이력
                {
                    회사코드 = 보유품목_P.회사코드,
                    보유품목코드 = 보유품목_P.보유품목코드,
                    연계보유품목코드 = 보유품목_P.보유품목코드,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = 보유품목_P.장소코드,
                    장소위치코드 = 보유품목_P.장소위치코드,
                    변경수량 = 보유품목_P.수량,
                    변경사유 = 입출고여부,
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                dc.보유품목이력.Add(log);


                dc.SaveChanges();

            }
            catch (Exception ex)
            {
                result = false;
            }


            result = true;

            return Task.FromResult(result);


        }




        public Task 보유품목_품목출고(string 보유품목코드, decimal 수량, string 사유)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // 보유품목 선택
            var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드);
            // 보유품목 등록이 되어 있지 않으면 무시한다.
            if (info == default)
                return Task.CompletedTask;

            // 보유품목 수량 변경
            //var pum_수량 = info.수량;
            info.수량 -= 수량;

            // 장소 변경
            //info.장소코드 = 장소코드;

            dc.보유품목정보.Update(info);
            dc.SaveChanges();

            //2021.04.15 
            var 위치코드 = info.장소위치코드;
            var 장소코드 = info.장소코드;

            if (위치코드 != null) //위치반출일 경우 위치 수량 변경
            {
                var w_info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드 && x.장소위치코드 == 위치코드);
                if (w_info != default)
                {

                    w_info.수량 -= 수량;
                    if (w_info.수량 > 0)
                    {
                        if (w_info.수량 > info.수량)
                            w_info.수량 = info.수량;

                        dc.보유품목위치정보.Update(w_info);
                        dc.SaveChanges();
                    }
                }
            }

            // 보유품목 이력 추가
            var log = new 보유품목이력
            {
                보유품목코드 = 보유품목코드,
                연계보유품목코드 = 보유품목코드,
                변경유형코드 = "B1702",    // 출고
                장소코드 = 장소코드,
                장소위치코드 = 위치코드,
                변경수량 = 수량,
                변경사유 = "출고 처리",
                유형사유 = 사유,
                변경일시 = DateTime.Now
            };
            dc.보유품목이력.Add(log);

            //dc.SaveChanges();
            return Task.CompletedTask;
        }




        //2021.05.10
        public Task<string> 품목코드_바코드발급(string 품목코드, string 회사코드, string 수량, string sawon, bool YN, string 구분)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            string result = "";

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            //var no = dc.품목정보.Count(x => x.품목코드 == 품목코드) + 1;
            var lotNO = dc.바코드발급정보.Count(x => x.품목코드 == 품목코드 && x.회사코드 == 회사코드 && x.LOT번호 != "00000") + 1;
            var lot_no = $"{lotNO:00000}";

            var info = new 바코드발급정보
            {
                //보유품목코드 = $"{품목.품목코드}:{yyMMdd}:{no}",
                회사코드 = 회사코드,
                품목코드 = 품목코드,
                생성일자 = yyyyMMdd,
                사원코드 = sawon,
                수량 = 수량,
                구분 = 구분,
                LOT번호 = $"{(YN == true ? lot_no : "00000")}", // $"{yyMMdd}:{수량}:{lotNO:00000 }",
                입고유무 = false,
            };

            dc.바코드발급정보.Add(info);

            dc.SaveChanges();

            result = info.LOT번호;

            return Task.FromResult(result);
        }

        //2021.05.12
        public Task<bool> 보유품목LOT입고_위치등록(입고처리상세정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            보유품목위치정보 info;
            try
            {
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                if (P_장소위치코드 == null)
                {
                    info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);
                }
                else
                {
                    info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번
                                    && x.장소위치코드 == 보유품목_P.입고장소코드 && x.위치상세코드 == P_장소위치코드);
                }


                if (isAdd)
                {
                    if (info == default)
                    {
                        info = new 보유품목위치정보
                        {
                            회사코드 = 보유품목_P.회사코드,
                            보유품목코드 = 보유품목_P.품번,
                            장소위치코드 = 보유품목_P.입고장소코드,
                            수량 = 보유품목_P.입고수량_관리단위,
                            LOT번호 = 보유품목_P.LOT번호,
                            품목_LOT번호 = 보유품목_P.비고,
                            위치상세코드 = P_장소위치코드,

                        };
                        dc.보유품목위치정보.Add(info);
                    }
                    // 변경
                    else
                    {

                        info.수량 = info.수량 + 보유품목_P.입고수량_관리단위;
                        dc.보유품목위치정보.Update(info);
                    }
                }
                else
                {
                    if (info == default)
                    {
                        info = new 보유품목위치정보
                        {
                            회사코드 = 보유품목_P.회사코드,
                            보유품목코드 = 보유품목_P.품번,
                            장소위치코드 = 보유품목_P.입고장소코드,
                            수량 = 보유품목_P.입고수량_관리단위,
                            LOT번호 = 보유품목_P.LOT번호,
                            품목_LOT번호 = 보유품목_P.비고,

                        };
                        dc.보유품목위치정보.Add(info);

                        var info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == P_장소위치코드);
                        info1.수량 = info1.수량 - (info1.수량 - 보유품목_P.입고수량_관리단위);
                        dc.보유품목위치정보.Update(info1);

                    }
                    // 변경
                    else
                    {
                        var info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == P_장소위치코드);

                        info.수량 = info1.수량 - (info1.수량 - 보유품목_P.입고수량_관리단위);

                        dc.보유품목위치정보.Update(info);

                    }
                }

                dc.SaveChanges();

                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);

                var log = new 보유품목이력
                {
                    회사코드 = 보유품목_P.회사코드,
                    보유품목코드 = 보유품목_P.품번,
                    연계보유품목코드 = 보유품목_P.품번,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = 보유품목_P.입고장소코드.Substring(0, 4),
                    장소위치코드 = 보유품목_P.입고장소코드,
                    변경수량 = 보유품목_P.입고수량_관리단위,
                    변경사유 = 입출고여부,
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now,
                    LOT번호 = 보유품목_P.LOT번호,
                    품목_LOT번호 = 보유품목_P.비고,
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                //if (보유이력 == null)
                dc.보유품목이력.Add(log);

                dc.SaveChanges();


                // 2021.05.31
                var 바코드발급 = dc.바코드발급정보
                    .Include(x => x.품목)
                    .Where(x => x.품목코드 == 보유품목_P.품번 && x.LOT번호 == 보유품목_P.LOT번호 && x.회사코드 == 보유품목_P.회사코드)
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





        public Task<IEnumerable<보유품목위치정보>> 자재위치현황_조회(string 회사코드, string 보유품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목위치정보
                .Include(x => x.장소위치)
                .Where(x => x.회사코드 == 회사코드 && x.보유품목코드 == 보유품목코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task<IEnumerable<보유품목위치정보>> 자재장소위치별품목현황_조회(string 회사코드, string 장소위치코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목위치정보
                .Include(x => x.장소위치)
                .Where(x => x.회사코드 == 회사코드 && x.장소위치코드 == 장소위치코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        //2021.05.13
        public Task<bool> 보유품목LOT출고_위치등록(보유품목정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            보유품목위치정보 info;

            try
            {
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                if (P_장소위치코드 == null)
                {
                    info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드 && x.장소위치코드 == 보유품목_P.장소위치코드);
                }
                else
                {
                    info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드
                                    && x.장소위치코드 == 보유품목_P.장소위치코드 && x.위치상세코드 == P_장소위치코드);
                }

                if (info == default)
                {
                    info = new 보유품목위치정보
                    {
                        회사코드 = 보유품목_P.회사코드,
                        보유품목코드 = 보유품목_P.보유품목코드,
                        장소위치코드 = 보유품목_P.장소위치코드,
                        수량 = 보유품목_P.수량,
                        LOT번호 = 보유품목_P.LOT번호,
                        품목_LOT번호 = 보유품목_P.품목_LOT번호,
                        위치상세코드 = P_장소위치코드,

                    };
                    dc.보유품목위치정보.Add(info);
                }
                // 변경
                else
                {

                    info.수량 = info.수량 - 보유품목_P.수량;
                    if (info.수량 < 0)
                        info.수량 = 0;
                    dc.보유품목위치정보.Update(info);
                }

                // 추가
                //if (info == default)
                //{
                //    info = new 보유품목위치정보
                //    {
                //        회사코드 = 보유품목_P.회사코드,
                //        보유품목코드 = 보유품목_P.보유품목코드,
                //        장소위치코드 = 보유품목_P.장소위치코드,
                //        수량 = -보유품목_P.수량
                //    };
                //    dc.보유품목위치정보.Add(info);
                //}
                // 변경
                //if (info != null)
                //{

                //    info.수량 = info.수량 - 보유품목_P.수량;

                //    dc.보유품목위치정보.Update(info);
                //}

                dc.SaveChanges();

                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드 && x.장소위치코드 == 보유품목_P.장소위치코드);
                var log = new 보유품목이력
                {
                    회사코드 = 보유품목_P.회사코드,
                    보유품목코드 = 보유품목_P.보유품목코드,
                    연계보유품목코드 = 보유품목_P.보유품목코드,
                    변경유형코드 = "B1702",    // 출고
                    장소코드 = 보유품목_P.장소코드,
                    장소위치코드 = 보유품목_P.장소위치코드,
                    변경수량 = 보유품목_P.수량,
                    변경사유 = 입출고여부,
                    유형사유 = "B1702",
                    변경일시 = DateTime.Now
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                dc.보유품목이력.Add(log);


                dc.SaveChanges();

            }
            catch (Exception ex)
            {
                result = false;
            }


            result = true;

            return Task.FromResult(result);


        }


        public Task<bool> 자재이동_임시등록(입고처리상세정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, string 지시서)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            보유품목위치정보 info;
            보유품목임시위치정보 info임시;

            string WO_NB = null;
            if (지시서 != null)
            {
                var 지시서_Ary = 지시서.Split(':');
                WO_NB = 지시서_Ary[0];
            }


            try
            {
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);

                //if (P_장소위치코드 == null)
                //{
                //    info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);
                //}
                //else
                //{
                //    info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번
                //                    && x.장소위치코드 == 보유품목_P.입고장소코드 && x.위치상세코드 == P_장소위치코드);
                //}

                if (info != null)
                {
                    info임시 = new 보유품목임시위치정보
                    {
                        회사코드 = 보유품목_P.회사코드,
                        보유품목코드 = 보유품목_P.품번,
                        장소위치코드 = 보유품목_P.입고장소코드,
                        수량 = 보유품목_P.입고수량_관리단위,
                        LOT번호 = 보유품목_P.LOT번호,
                        품목_LOT번호 = 보유품목_P.비고,
                        위치상세코드 = P_장소위치코드,
                        사유 = 사유,
                        지시서 = WO_NB,

                    };
                    dc.보유품목임시위치정보.Add(info임시);

                    //2021.05.28
                    var info외주 = new 외주생산위치정보
                    {
                        회사코드 = 보유품목_P.회사코드,
                        보유품목코드 = 보유품목_P.품번,
                        장소위치코드 = 보유품목_P.입고장소코드,
                        수량 = 보유품목_P.입고수량_관리단위,
                        LOT번호 = 보유품목_P.LOT번호,
                        품목_LOT번호 = 보유품목_P.비고,
                        위치상세코드 = P_장소위치코드,
                        사유 = 사유,
                        지시서 = WO_NB,

                    };
                    dc.외주생산위치정보.Add(info외주);
                    ////////////////////////////////////////

                    info.수량 = info.수량 - 보유품목_P.입고수량_관리단위;
                    if (info.수량 < 0)
                        info.수량 = 0;
                    dc.보유품목위치정보.Update(info);
                }
                // 변경
                else
                {
                    result = false;
                    return Task.FromResult(result);
                    //info.수량 = info.수량 + 보유품목_P.입고수량_관리단위;
                    //dc.보유품목위치정보.Update(info);
                }



                dc.SaveChanges();

                string 이동구분 = "";
                if (사유 == "5")
                    이동구분 = "자재이동출고";
                else if (사유 == "0")
                    이동구분 = "생산이동출고";
                else if (사유 == "1")
                    이동구분 = "외주이동출고";


                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);

                var log = new 보유품목이력
                {
                    회사코드 = 보유품목_P.회사코드,
                    보유품목코드 = 보유품목_P.품번,
                    연계보유품목코드 = 보유품목_P.품번,
                    변경유형코드 = "B1702",    // 출고
                    장소코드 = 보유품목_P.입고장소코드.Substring(0, 4),
                    장소위치코드 = 보유품목_P.입고장소코드,
                    변경수량 = 보유품목_P.입고수량_관리단위,
                    변경사유 = 이동구분,
                    유형사유 = "B1702",
                    변경일시 = DateTime.Now,
                    LOT번호 = 보유품목_P.LOT번호,
                    품목_LOT번호 = 보유품목_P.비고,
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                //if (보유이력 == null)
                dc.보유품목이력.Add(log);

                dc.SaveChanges();

            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

            return Task.FromResult(result);
        }

        //2021.05.14

        //2021.05.14
        public Task<bool> 자재이동_임시등록_해제(보유품목임시위치정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, string 사원코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;
            보유품목위치정보 info;
            보유품목위치정보 info1;
            보유품목임시위치정보 info임시;

            //var 출고추가수정유무 = dc.출고처리헤더정보.Count(x => x.작업번호 == 출고처리.작업번호);
            var 순번 = dc.재고이동헤더정보.Count(x => x.회사코드 == 보유품목_P.회사코드) + 1;

            try
            {

                string 장소위치 = P_장소위치코드;
                string 지시서 = "";
                string 이유 = "";

                if (P_장소위치코드.Length > 8)
                {
                    장소위치 = P_장소위치코드.Substring(0, 8);
                }


                if (보유품목_P != null)
                {
                    info = new 보유품목위치정보
                    {
                        회사코드 = 보유품목_P.회사코드,
                        보유품목코드 = 보유품목_P.보유품목코드,
                        장소위치코드 = 장소위치,
                        수량 = 보유품목_P.수량,
                        LOT번호 = 보유품목_P.LOT번호,
                        품목_LOT번호 = 보유품목_P.품목_LOT번호,
                        위치상세코드 = P_장소위치코드,
                        //사유 = 사유,

                    };

                    info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드 && x.장소위치코드 == 장소위치
                                                                     && x.회사코드 == 보유품목_P.회사코드);
                    //if (P_장소위치코드 == null)
                    //{
                    //    info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드 && x.장소위치코드 == 장소위치
                    //                                                 && x.회사코드 == 보유품목_P.회사코드);
                    //}
                    //else
                    //{
                    //    info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드
                    //                    && x.위치상세코드 == P_장소위치코드 && x.회사코드 == 보유품목_P.회사코드);
                    //}
                    if (info1 == default)
                    {
                        dc.보유품목위치정보.Add(info);
                    }
                    else
                    {
                        info1.수량 = info1.수량 + info.수량;
                        dc.보유품목위치정보.Update(info1);
                    }

                    dc.보유품목임시위치정보.Remove(보유품목_P);
                }
                // 변경
                else
                {
                    result = false;
                    return Task.FromResult(result);
                    //info.수량 = info.수량 + 보유품목_P.입고수량_관리단위;
                    //dc.보유품목위치정보.Update(info);
                }



                dc.SaveChanges();

                string 이동구분 = "";
                if (사유 == "5")
                    이동구분 = "자재이동입고";
                else if (사유 == "0")
                    이동구분 = "생산이동입고";
                else if (사유 == "1")
                    이동구분 = "외주이동입고";

                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드 && x.장소위치코드 == 보유품목_P.장소위치코드);

                var log = new 보유품목이력
                {
                    회사코드 = 보유품목_P.회사코드,
                    보유품목코드 = 보유품목_P.보유품목코드,
                    연계보유품목코드 = 보유품목_P.보유품목코드,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = 장소위치.Substring(0, 4),
                    장소위치코드 = 장소위치,
                    변경수량 = 보유품목_P.수량,
                    변경사유 = 이동구분,
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now,
                    LOT번호 = 보유품목_P.LOT번호,
                    품목_LOT번호 = 보유품목_P.품목_LOT번호,
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                //if (보유이력 == null)
                dc.보유품목이력.Add(log);

                dc.SaveChanges();


                // 2021.05.15
                // 재고이동 헤더 정보 및 재고이동 상세 처리
                //보유품목_P.사유
                //await Remote.Command.기준정보.MES재고이동_재고이동헤더정보_등록(newRow, args.Action == "Add" ? true : false);

                //await Remote.Command.기준정보.MES재고이동_재고이동상세정보_등록(newRow, args.Action == "Add" ? true : false);
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호 = "";
                작업번호 = $"{"MV"}{yyyy}{순번:000000}";
                //사원코드로 부서 찾는다
                var sawonRec = dc.직원정보
                    .Where(x => x.회사코드 == 보유품목_P.회사코드 && x.사번 == 사원코드)
                    .FirstOrDefault();

                var booseo = sawonRec.부서코드;
                var 출고창고 = 보유품목_P.위치상세코드.Substring(0, 4);
                var 출고장소 = 보유품목_P.위치상세코드.Substring(4, 4);
                var 출고위치 = 보유품목_P.위치상세코드;
                var 입고창고 = P_장소위치코드.Substring(0, 4);
                var 입고장소 = P_장소위치코드.Substring(4, 4);
                var 입고위치 = P_장소위치코드;

                var 재고이동 = new 재고이동헤더정보
                {
                    회사코드 = 보유품목_P.회사코드,
                    작업번호 = 작업번호,
                    작업일자 = DateTime.Now,
                    이동일자 = DateTime.Now,
                    이동구분 = 보유품목_P.사유,
                    사원코드 = 사원코드,
                    부서코드 = booseo,
                    사업장코드 = "1000",
                    출고창고코드 = 출고창고,
                    출고장소코드 = 출고장소,
                    출고장소위치상세코드 = 출고위치,
                    입고공정_창고코드 = 입고창고,
                    입고작업장_장소코드 = 입고장소,
                    입고장소위치상세코드 = 입고위치,

                };

                var BARPLUS_LSTKMOVE = new BARPLUS_LSTKMOVE
                {
                    CO_CD = 재고이동.회사코드,
                    WORK_NB = 작업번호,
                    WORK_DT = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(재고이동.작업일자.ToString())),
                    //MOVE_NB  이동번호
                    MOVE_DT = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(재고이동.이동일자.ToString())),
                    GRP_FG = 재고이동.이동구분,
                    IO_FG = "2",
                    EMP_CD = 재고이동.사원코드,
                    DEPT_CD = 재고이동.부서코드,
                    DIV_CD = 재고이동.사업장코드,
                    FWH_CD = 재고이동.출고창고코드,
                    FLC_CD = 재고이동.출고장소코드,
                    TWH_CD = 재고이동.입고공정_창고코드,
                    TLC_CD = 재고이동.입고작업장_장소코드,
                    MOVE_FG = "1",
                    APP_FG = "0",
                };

                dc.재고이동헤더정보.Add(재고이동);

                dc_D.BARPLUS_LSTKMOVE.Add(BARPLUS_LSTKMOVE);

                dc.SaveChanges();

                dc_D.SaveChanges();

                // 이동구분 - "1" : 외주이동일 경우 청구수량 외주 주문서에서 가져온다
                decimal 요구수량 = 보유품목_P.수량;
                if (이동구분 == "1")
                {
                    var P지시서 = 보유품목_P.지시서;
                    var P지시저_Ary = P지시서.Split(':');
                    var WO_CD = P지시저_Ary[0];
                    var ITEM_CD = P지시저_Ary[1];
                    var 외주지시서 = dc_D.VL_MES_WO_WF
                        .Where(x => x.CO_CD == 보유품목_P.회사코드 && x.WO_CD == WO_CD && x.ITEM_CD == ITEM_CD)
                        .FirstOrDefault();

                    if (외주지시서 != default)
                    {
                        요구수량 = 외주지시서.ITEM_QT;
                    }
                }

                // 재고이동상세
                순번 = dc.재고이동상세정보.Count(x => x.회사코드 == 보유품목_P.회사코드 &&
                                        x.작업번호 == 작업번호) + 1;
                var 이동처리 = new 재고이동상세정보
                {
                    회사코드 = 보유품목_P.회사코드,
                    작업번호 = 작업번호,
                    작업순번 = 순번,
                    품번 = 보유품목_P.보유품목코드,
                    청구수량 = 요구수량,
                    이동수량 = 보유품목_P.수량,
                    재공운영여부 = "1",
                    APP_FG = "0",
                    사용여부 = "1",
                    만료여부 = "1",
                    LOT번호 = 보유품목_P.LOT번호,
                    품목_LOT번호 = 보유품목_P.품목_LOT번호,
                    청구순번 = 순번,

                };

                var BARPLUS_LSTKMOVE_D = new BARPLUS_LSTKMOVE_D
                {
                    CO_CD = 이동처리.회사코드,
                    WORK_NB = 이동처리.작업번호,
                    WORK_SQ = 순번,
                    ITEM_CD = 이동처리.품번,
                    REQ_QT = 요구수량,
                    MOVE_QT = 이동처리.이동수량,
                    WIP_YN = 이동처리.재공운영여부,
                    APP_FG = 이동처리.APP_FG,
                    USE_YN = 이동처리.사용여부,
                    EXPIRE_YN = 이동처리.만료여부,
                    LOT_NB = 이동처리.LOT번호,
                };

                dc.재고이동상세정보.Add(이동처리);

                dc_D.BARPLUS_LSTKMOVE_D.Add(BARPLUS_LSTKMOVE_D);

                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

            return Task.FromResult(result);
        }


        //2021.05.18
        public Task<string> 입고관리보유품목입고Action_등록(입고처리상세정보 상세, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            var no = dc.보유품목정보.Count(x => x.품목코드 == 상세.품번 && x.회사코드 == 상세.회사코드) + 1;
            var result = dc.보유품목정보.Where(x => x.품목코드 == 상세.품번 && x.회사코드 == 상세.회사코드).FirstOrDefault();

            //var 입고처리상세 = dc.입고처리상세정보.Where(x => x.품번 == 상세.품번 && x.회사코드 == 상세.회사코드 &&
            //                x.작업순번 == 상세.작업순번).FirstOrDefault();
            //var 입고처리상세 = dc.입고처리상세정보.Where(x => x.품번 == 상세.품번 && x.회사코드 == 상세.회사코드)
            //                     .FirstOrDefault();

            var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 상세.품번).FirstOrDefault();
            decimal 입고상세수량 = 상세.입고수량_관리단위;
            //if (입고처리상세 != null)
            //    입고상세수량 = 상세.입고수량_관리단위; // - 입고처리상세.입고수량_관리단위;

            string 장소코드 = "";
            string 장소위치코드 = "";

            장소코드 = 상세.입고장소코드.Substring(0, 4);


            var info = new 보유품목정보
            {
                회사코드 = 상세.회사코드,
                보유품목코드 = 상세.품번,
                품목코드 = 품목정보.품목코드,
                보유년월일 = result != null ? result.보유년월일 : yyyyMMdd,
                조정년월일 = null,//result.조정년월일,
                보유일 = now,
                수량 = isAdd == true ? 상세.입고수량_관리단위 : (result != null ? result.수량 + 입고상세수량 : 입고상세수량),
                품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                장소코드 = result != null ? result.장소코드 : 장소코드,
                장소위치코드 = result != null ? result.장소위치코드 : 상세.입고장소코드,
            };

            if (result == null)
            {
                dc.보유품목정보.Add(info);
                dc.SaveChanges();
            }
            else
            {
                dc.보유품목정보.Update(info);
                dc.SaveChanges();
            }


            return Task.FromResult(info.보유품목코드);
        }


        public Task<string> 품목코드_NOT바코드발급(string 품목코드, string 회사코드, string 수량, string sawon, bool YN)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            string result = "";

            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");
            //var no = dc.품목정보.Count(x => x.품목코드 == 품목코드) + 1;
            var lotNO = dc.바코드발급정보.Count(x => x.품목코드 == 품목코드 && x.회사코드 == 회사코드 && x.LOT번호 != "00000") + 1;
            var lot_no = $"{lotNO:00000}";

            var info = new 바코드발급정보
            {
                //보유품목코드 = $"{품목.품목코드}:{yyMMdd}:{no}",
                회사코드 = 회사코드,
                품목코드 = 품목코드,
                생성일자 = yyyyMMdd,
                사원코드 = sawon,
                수량 = 수량,
                LOT번호 = $"{(YN == true ? lot_no : "00000")}", // $"{yyMMdd}:{수량}:{lotNO:00000 }",
            };

            dc.바코드발급정보.Add(info);

            dc.SaveChanges();

            result = info.LOT번호;

            return Task.FromResult(result);
        }

        /*
        public Task<bool> 생산이동_임시등록(입고처리상세정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, string 지시서)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            보유품목위치정보 info;
            보유품목임시위치정보 info임시;
            try
            {
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                if (P_장소위치코드 == null)
                {
                    info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);
                }
                else
                {
                    info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번
                                    && x.장소위치코드 == 보유품목_P.입고장소코드 && x.위치상세코드 == P_장소위치코드);
                }

                if (info != null)
                {
                    info임시 = new 보유품목임시위치정보
                    {
                        회사코드 = 보유품목_P.회사코드,
                        보유품목코드 = 보유품목_P.품번,
                        장소위치코드 = 보유품목_P.입고장소코드,
                        수량 = 보유품목_P.입고수량_관리단위,
                        LOT번호 = 보유품목_P.LOT번호,
                        품목_LOT번호 = 보유품목_P.비고,
                        위치상세코드 = P_장소위치코드,
                        사유 = 사유,
                        지시서 = 지시서,

                    };
                    dc.보유품목임시위치정보.Add(info임시);
                }
                // 변경
                else
                {
                    result = false;
                    return Task.FromResult(result);
                    //info.수량 = info.수량 + 보유품목_P.입고수량_관리단위;
                    //dc.보유품목위치정보.Update(info);
                }



                dc.SaveChanges();

                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.품번 && x.장소위치코드 == 보유품목_P.입고장소코드);

                var log = new 보유품목이력
                {
                    회사코드 = 보유품목_P.회사코드,
                    보유품목코드 = 보유품목_P.품번,
                    연계보유품목코드 = 보유품목_P.품번,
                    변경유형코드 = "B1702",    // 출고
                    장소코드 = 보유품목_P.입고장소코드.Substring(0, 4),
                    장소위치코드 = 보유품목_P.입고장소코드,
                    변경수량 = 보유품목_P.입고수량_관리단위,
                    변경사유 = "생산이동출고",
                    유형사유 = "B1702",
                    변경일시 = DateTime.Now,
                    LOT번호 = 보유품목_P.LOT번호,
                    품목_LOT번호 = 보유품목_P.비고,
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                //if (보유이력 == null)
                dc.보유품목이력.Add(log);

                dc.SaveChanges();

            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

            return Task.FromResult(result);
        }

        public Task<bool> 생산이동_임시등록_해제(보유품목임시위치정보 보유품목_P, string 입출고여부, string 사유, string P_장소위치코드, string 사원코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;
            보유품목위치정보 info;
            보유품목위치정보 info1;
            보유품목임시위치정보 info임시;

            //var 출고추가수정유무 = dc.출고처리헤더정보.Count(x => x.작업번호 == 출고처리.작업번호);
            var 순번 = dc.재고이동헤더정보.Count(x => x.회사코드 == 보유품목_P.회사코드) + 1;

            try
            {

                string 장소위치 = "";
                string 지시서 = "";
                string 이유 = "";

                if (P_장소위치코드.Length > 8)
                {
                    장소위치 = P_장소위치코드.Substring(0, 8);
                }


                if (보유품목_P != null)
                {
                    info = new 보유품목위치정보
                    {
                        회사코드 = 보유품목_P.회사코드,
                        보유품목코드 = 보유품목_P.보유품목코드,
                        장소위치코드 = 장소위치,
                        수량 = 보유품목_P.수량,
                        LOT번호 = 보유품목_P.LOT번호,
                        품목_LOT번호 = 보유품목_P.품목_LOT번호,
                        위치상세코드 = P_장소위치코드,
                        //사유 = 사유,

                    };

                    이유 = 보유품목_P.사유;
                    지시서 = 보유품목_P.지시서;

                    if (P_장소위치코드 == null)
                    {
                        info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드 && x.장소위치코드 == 장소위치
                                                                     && x.회사코드 == 보유품목_P.회사코드);
                    }
                    else
                    {
                        info1 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드
                                        && x.위치상세코드 == P_장소위치코드 && x.회사코드 == 보유품목_P.회사코드);
                    }
                    if (info1 == default)
                    {
                        dc.보유품목위치정보.Add(info);
                    }
                    else
                    {
                        info1.수량 = info1.수량 + info.수량;
                        dc.보유품목위치정보.Update(info1);
                    }

                    dc.보유품목임시위치정보.Remove(보유품목_P);
                }
                // 변경
                else
                {
                    result = false;
                    return Task.FromResult(result);
                    //info.수량 = info.수량 + 보유품목_P.입고수량_관리단위;
                    //dc.보유품목위치정보.Update(info);
                }



                dc.SaveChanges();

                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 보유품목_P.보유품목코드 && x.장소위치코드 == 보유품목_P.장소위치코드);

                var log = new 보유품목이력
                {
                    회사코드 = 보유품목_P.회사코드,
                    보유품목코드 = 보유품목_P.보유품목코드,
                    연계보유품목코드 = 보유품목_P.보유품목코드,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = 장소위치.Substring(0, 4),
                    장소위치코드 = 장소위치,
                    변경수량 = 보유품목_P.수량,
                    변경사유 = "생산이동입고",
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now,
                    LOT번호 = 보유품목_P.LOT번호,
                    품목_LOT번호 = 보유품목_P.품목_LOT번호,
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                //if (보유이력 == null)
                dc.보유품목이력.Add(log);

                dc.SaveChanges();


                // 2021.05.15
                // 재고이동 헤더 정보 및 재고이동 상세 처리
                //보유품목_P.사유
                //await Remote.Command.기준정보.MES재고이동_재고이동헤더정보_등록(newRow, args.Action == "Add" ? true : false);

                //await Remote.Command.기준정보.MES재고이동_재고이동상세정보_등록(newRow, args.Action == "Add" ? true : false);
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호 = "";
                작업번호 = $"{"MV"}{yyyy}{순번:000000}";
                //사원코드로 부서 찾는다
                var sawonRec = dc.직원정보
                    .Where(x => x.회사코드 == 보유품목_P.회사코드 && x.사번 == 사원코드)
                    .FirstOrDefault();

                var booseo = sawonRec.부서코드;
                var 출고창고 = 보유품목_P.위치상세코드.Substring(0, 4);
                var 출고장소 = 보유품목_P.위치상세코드.Substring(4, 4);
                var 출고위치 = 보유품목_P.위치상세코드;
                var 입고창고 = P_장소위치코드.Substring(0, 4);
                var 입고장소 = P_장소위치코드.Substring(4, 4);
                var 입고위치 = P_장소위치코드;

                var 재고이동 = new 재고이동헤더정보
                {
                    회사코드 = 보유품목_P.회사코드,
                    작업번호 = 작업번호,
                    작업일자 = DateTime.Now,
                    이동일자 = DateTime.Now,
                    이동구분 = 보유품목_P.사유,
                    사원코드 = 사원코드,
                    부서코드 = booseo,
                    사업장코드 = "1000",
                    출고창고코드 = 출고창고,
                    출고장소코드 = 출고장소,
                    출고장소위치상세코드 = 출고위치,
                    입고공정_창고코드 = 입고창고,
                    입고작업장_장소코드 = 입고창고,
                    입고장소위치상세코드 = 입고위치,

                };

                var BARPLUS_LSTKMOVE = new BARPLUS_LSTKMOVE
                {
                    CO_CD = 재고이동.회사코드,
                    WORK_NB = 작업번호,
                    WORK_DT = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(재고이동.작업일자.ToString())),
                    //MOVE_NB  이동번호
                    MOVE_DT = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(재고이동.이동일자.ToString())),
                    GRP_FG = 재고이동.이동구분,
                    IO_FG = "2",
                    EMP_CD = 재고이동.사원코드,
                    DEPT_CD = 재고이동.부서코드,
                    DIV_CD = 재고이동.사업장코드,
                    FWH_CD = 재고이동.출고창고코드,
                    FLC_CD = 재고이동.출고장소코드,
                    TWH_CD = 재고이동.입고공정_창고코드,
                    TLC_CD = 재고이동.입고작업장_장소코드,
                    MOVE_FG = "1",
                    APP_FG = "0",
                };

                dc.재고이동헤더정보.Add(재고이동);

                dc_D.BARPLUS_LSTKMOVE.Add(BARPLUS_LSTKMOVE);


                // 재고이동상세
                순번 = dc.재고이동상세정보.Count(x => x.회사코드 == 보유품목_P.회사코드 &&
                                        x.작업번호 == 작업번호) + 1;
                var 이동처리 = new 재고이동상세정보
                {
                    회사코드 = 보유품목_P.회사코드,
                    작업번호 = 작업번호,
                    작업순번 = 순번,
                    품번 = 보유품목_P.보유품목코드,
                    이동수량 = 보유품목_P.수량,
                    재공운영여부 = "1",
                    APP_FG = "0",
                    사용여부 = "1",
                    만료여부 = "1",
                    LOT번호 = 보유품목_P.LOT번호,
                    품목_LOT번호 = 보유품목_P.품목_LOT번호,
                    청구순번 = 순번,

                };

                var BARPLUS_LSTKMOVE_D = new BARPLUS_LSTKMOVE_D
                {
                    CO_CD = 이동처리.회사코드,
                    WORK_NB = 이동처리.작업번호,
                    WORK_SQ = 순번,
                    ITEM_CD = 이동처리.품번,
                    MOVE_QT = 이동처리.이동수량,
                    WIP_YN = 이동처리.재공운영여부,
                    APP_FG = 이동처리.APP_FG,
                    USE_YN = 이동처리.사용여부,
                    EXPIRE_YN = 이동처리.만료여부,
                    LOT_NB = 이동처리.LOT번호,
                };

                dc.재고이동상세정보.Add(이동처리);

                dc_D.BARPLUS_LSTKMOVE_D.Add(BARPLUS_LSTKMOVE_D);


            }
            catch (Exception ex)
            {
                result = false;
            }

            result = true;

            return Task.FromResult(result);
        }
        */

    }
}
