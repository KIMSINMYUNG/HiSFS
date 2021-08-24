using HiSFS.Api.Host.Data;
using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Models.Parameters;
using HiSFS.Api.Shared.Models.View;
using HiSFS.Api.Shared.Models.View_DZICUBE;
using HiSFS.Api.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HiSFS.Api.Host.Services
{
    public class 기준정보서비스 : I기준정보서비스
    {
        private readonly IContextProvider dcp;


        public 기준정보서비스(IContextProvider dbContextProvider)
        {
            this.dcp = dbContextProvider;
        }

        // 부서 {{{



        public Task<IEnumerable<부서정보>> 부서_조회(검색정보 검색)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            /*
                        var query = dc.부서정보
                            .Include(x => x.하위부서목록.Where(x => x.삭제유무 != true))
                                .ThenInclude(x => x.하위부서목록.Where(x => x.삭제유무 != true))
                                    .ThenInclude(x => x.하위부서목록.Where(x => x.삭제유무 != true))
                            .Where_미삭제();
                        if (검색?.유무(검색대상.사용) == true)
                            query = query.Where_사용();

                        var list = query.OrderBy(x => x.상위부서코드)
                            .ThenBy(x => x.정렬순번)
                            .ToList();

            return Task.FromResult(list.AsEnumerable());

            */

            List<부서정보> 부서정보 = new List<부서정보>();
            return Task.FromResult(부서정보.AsEnumerable());

        }

        public Task 부서_저장(부서정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isAdd == true)
            {
                var key = "";
                //if (string.IsNullOrWhiteSpace(info.상위부서코드) == false)
                //    key = $"{info.상위부서코드}{dc.부서정보.Count(x => x.부서코드 == info.상위부서코드) + 1:00}";
                //else
                //    key = $"D{dc.부서정보.Count(x => x.상위부서코드 == null) + 1:00}";

                key = $"D{dc.부서정보.Count() + 1:0000}";
                info.부서코드 = key;
                dc.부서정보.Add(info);
            }
            else
                dc.부서정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 부서_삭제(부서정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.부서정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.부서정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}

        // 직원 {{{
        public Task<IEnumerable<직원정보>> 직원_조회(bool isOnlyUse, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.직원정보
                .Include(x => x.부서)
                .Where(x => (isOnlyUse == true && x.사용유무 == true && x.회사코드 == 회사코드) || isOnlyUse == false && x.회사코드 == 회사코드)
                .Where_미삭제()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 직원_저장(직원정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            if (info.부서 != null)
            {
                info.부서코드 = info.부서.부서코드;
                info.부서 = null;
                info.권한 = null;
                info.직급 = null;
                info.직책 = null;
                info.권한정보 = null;
            }
            // }}}

            if (isAdd == true)
            {
                info.식별번호 = info.사번;        // 기본 식별번호는 사번이다.
                dc.직원정보.Add(info);
            }
            else
                dc.직원정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 직원_삭제(직원정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.직원정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.직원정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}

        // 장소, 장소위치 {{{
        public Task<IEnumerable<장소정보>> 장소_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.장소정보
                .Include(x => x.장소위치목록.Where(x => x.삭제유무 != true && x.회사코드 == 회사코드))
                .ThenInclude(x => x.위치상세목록).Where(x => x.회사코드 == 회사코드)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 장소_저장(장소정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isAdd == true)
            {
                var key = $"Z{dc.장소정보.Count() + 1:000}";
                info.장소코드 = key;

                dc.장소정보.Add(info);
            }
            else
                dc.장소정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 장소_삭제(장소정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.장소유형 = null;
            info.장소위치목록 = null;
            // }}}

            if (isCompletely == true)
                dc.장소정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.장소정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<장소위치정보>> 장소위치_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.장소위치정보
                .Include(x => x.장소).Where(x => x.회사코드 == 회사코드)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 장소위치_저장(장소위치정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            info.장소 = null;

            if (isAdd == true)
            {
                var 위치코드 = $"{dc.장소위치정보.Count(x => x.장소코드 == info.장소코드) + 1:00}";
                var key = $"{info.장소코드}{위치코드}";
                info.장소위치코드 = key;
                info.위치코드 = 위치코드;

                dc.장소위치정보.Add(info);
            }
            else
                dc.장소위치정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 장소위치_삭제(장소위치정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.장소위치정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.장소위치정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        // }}}

        // 거래처 {{{
        public Task<IEnumerable<거래처정보>> 거래처_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.거래처정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 거래처_저장(거래처정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 동작 막음 {{{
            info.거래처구분 = null;
            // }}}

            if (isAdd == true)
            {
                var code = $"A{dc.거래처정보.Count() + 1:0000}";
                info.거래처코드 = code;

                dc.거래처정보.Add(info);
            }
            else
                dc.거래처정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 거래처_삭제(거래처정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.거래처정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.거래처정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}

        // 원자재 {{{
        public Task<IEnumerable<품목정보>> 원자재_조회(bool isOnlyUse, bool 반제품포함유무)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.품목정보
                .Include(x => x.거래처)
                .Where(x => (isOnlyUse == true && x.사용유무 == true) || (isOnlyUse == false))
                .Where_미삭제()
                .Where(x => x.품목구분코드 == "B1201" || x.품목구분코드 == "B1202" || (반제품포함유무 == true && x.품목구분코드 == "B1204")) // 원자재, 부품
                .OrderByDescending(x => x.CreateTime)
                .ToList(); ;

            return Task.FromResult(list.AsEnumerable());
        }

        public Task<IEnumerable<품목정보>> 자재부품설비_조회(bool isOnlyUse, bool 반제품포함유무, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.품목정보
                .Include(x => x.거래처).Where(x => x.회사코드 == 회사코드)
                .Where(x => (isOnlyUse == true && x.사용유무 == true) || (isOnlyUse == false))
                .Where(x => 회사코드 == 회사코드)
                .Where(x => x.품목구분코드 == "B1201" || x.품목구분코드 == "B1202" || (반제품포함유무 == true && x.품목구분코드 == "B1204") || x.품목구분코드 == "B1205" || x.품목구분코드 == "B1207" || x.품목구분코드 == "B1203") // 원자재, 부품, 반제품, 설비, 생산품
                .Where_미삭제()
                .OrderByDescending(x => x.CreateTime)
                .ToList(); ;

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 원자재_저장(품목정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.품목구분 = null;
            info.품목유형 = null;
            info.소재 = null;
            info.규격종류 = null;
            info.조달구분 = null;
            info.단위 = null;
            if (info.거래처?.거래처코드 != null)
            {
                info.거래처코드 = info.거래처?.거래처코드;
                info.거래처 = null;
            }
            // }}}

            if (isAdd == true)
            {
                info.품목코드 = $"{info.원품목코드}:{info.관리차수}";
                dc.품목정보.Add(info);
            }
            else
                dc.품목정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 원자재_삭제(품목정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.품목정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.품목정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}


        // }}}

        // 생산품 {{{
        public Task<IEnumerable<품목정보>> 생산품_조회(bool isOnlyUse, bool 생산품만)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.품목정보
                .Include(x => x.거래처)
                .Where(x => (isOnlyUse == true && x.사용유무 == true) || isOnlyUse == false)
                .Where_미삭제()
                .Where(x => (생산품만 == true && x.품목구분코드 == "B1203") || (생산품만 == false && (x.품목구분코드 == "B1203" || x.품목구분코드 == "B1204"))) // 생산품, 반제품
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 생산품_저장(품목정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.품목구분 = null;
            info.품목유형 = null;
            info.소재 = null;
            info.규격종류 = null;
            info.조달구분 = null;
            info.단위 = null;
            if (info.거래처?.거래처코드 != null)
            {
                info.거래처코드 = info.거래처?.거래처코드;
                info.거래처 = null;
            }
            // }}}

            if (isAdd == true)
            {
                info.품목코드 = $"{info.원품목코드}:{info.관리차수}";
                dc.품목정보.Add(info);
            }
            else
                dc.품목정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 생산품_삭제(품목정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.품목정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.품목정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}

        // 생산품 {{{
        public Task<IEnumerable<품목정보>> 품목_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.품목정보
                .Include(x => x.거래처)
                //.Where(x => (isOnlyUse == true && x.사용유무 == true) || isOnlyUse == false)
                .Where_미삭제()
                //.Where(x => (생산품만 == true && x.품목구분코드 == "B1203") || (생산품만 == false && (x.품목구분코드 == "B1203" || x.품목구분코드 == "B1204"))) // 생산품, 반제품
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 품목_저장(품목정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.품목구분 = null;
            info.품목유형 = null;
            info.소재 = null;
            info.규격종류 = null;
            info.조달구분 = null;
            info.단위 = null;
            if (info.거래처?.거래처코드 != null)
            {
                info.거래처코드 = info.거래처?.거래처코드;
                info.거래처 = null;
            }
            // }}}

            if (isAdd == true)
            {
                //info.품목코드 = $"{info.원품목코드}:{info.관리차수}";
                info.품목코드 = info.원품목코드;
                dc.품목정보.Add(info);
            }
            else
                dc.품목정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 품목_삭제(품목정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.품목정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.품목정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}

        // 도면 {{{
        public Task<IEnumerable<도면정보>> 도면_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.도면정보
                .Include(x => x.파일폴더)
                    .ThenInclude(y => y.파일목록)
                .Where_미삭제()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }
        public Task 도면_저장(도면정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // 파일 저장 처리 필요

            // track 방지 {{{
            info.도면종류 = null;
            // }}}

            // 파일 처리 {{{
            if (info.파일폴더 != null)
            {
                var targetRoot = scope.Config.GetSection("RemoteApi")["StorageRoot"];
                foreach (var file in info.파일폴더.파일목록)
                {
                    // 상호참조에 의해 제거된 연결 복구
                    if (file.폴더순번 == 0 && file.폴더 == null)
                        file.폴더 = info.파일폴더;

                    if (file.삭제유무 == true)
                        dc.파일정보.Remove(file);
                    // 임시경로의 파일을 실 경로로 이동한다.
                    else if (string.IsNullOrWhiteSpace(file.임시경로) == false)
                    {
                        var source = file.임시경로;
                        var targetPath = Path.Combine(targetRoot, info.파일폴더.폴더경로);
                        if (Directory.Exists(targetPath) == false)
                            Directory.CreateDirectory(targetPath);

                        File.Move(source, Path.Combine(targetPath, file.파일이름), true);

                        file.경로 = $"{info.파일폴더.폴더경로}/{file.파일이름}";
                        file.임시경로 = null;
                    }
                }
                info.파일폴더.파일목록 = info.파일폴더.파일목록.Where(x => x.삭제유무 != true).ToList();
            }
            // }}

            if (isAdd == true)
            {
                info.원도면코드 = $"D{dc.도면정보.Count() + 1:00000}";
                info.도면코드 = $"{info.원도면코드}:{info.관리차수}";

                dc.도면정보.Add(info);
            }
            else
                dc.도면정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        public Task 도면_삭제(도면정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // 파일삭제 처리 필요

            if (isCompletely == true)
                dc.도면정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.도면정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}

        // 설비 {{{
        public Task<IEnumerable<품목정보>> 설비_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.품목정보
                .Include(x => x.거래처)
                .Where_미삭제()
                .Where(x => x.품목구분코드 == "B1205")    // 설비
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }
        public Task 설비_저장(품목정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.품목구분 = null;
            info.품목유형 = null;
            info.소재 = null;
            info.규격종류 = null;
            info.조달구분 = null;
            info.단위 = null;
            if (info.거래처?.거래처코드 != null)
            {
                info.거래처코드 = info.거래처?.거래처코드;
                info.거래처 = null;
            }
            // }}}

            if (isAdd == true)
            {
                //info.품목코드 = $"{info.원품목코드}:{info.관리차수}";
                info.품목코드 = info.원품목코드;
                dc.품목정보.Add(info);
            }
            else
                dc.품목정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 설비_삭제(품목정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            dc.품목정보.Remove(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 보유설비_저장(보유품목정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            //info.원보유품목 = null;
            info.품목 = null;
            info.장소 = null;
            info.장소위치 = null;
            info.보유품목이력 = null;
            // }}}
            var now = DateTime.Now;
            var yyyyMMdd = now.ToString("yyyyMMdd");



            var result = (from t in dc.보유품목정보
                          where t.품목코드 == info.품목코드
                          select t).Count();


            if (isAdd == true)
            {

                info.보유품목코드 = $"{info.보유품목코드}:{yyyyMMdd}:{result + 1}";
                dc.보유품목정보.Add(info);
            }
            else
            {
                // 설비명(보유명)만 변경한다.
                var 설비명 = info.보유명;
                info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == info.보유품목코드);
                info.보유명 = 설비명;
                dc.보유품목정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 보유설비_삭제(보유품목정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            info.품목구분 = null;
            info.장소 = null;
            //info.원보유품목 = null;
            info.품목 = null;
            info.장소 = null;
            info.장소위치 = null;
            info.보유품목이력 = null;
            info.설비가동현황 = null;
            info.보유품목위치모두 = null;

            if (isCompletely == true)
                dc.보유품목정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.보유품목정보.Update(info);
            }


            dc.SaveChanges();

            return Task.CompletedTask;
        }

        // }}}

        // 공정 {{{
        public Task<IEnumerable<공정정보>> 공정_조회(bool isOnlyUse)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.공정정보
                .Where(x => (isOnlyUse == true && x.사용유무 == true) || isOnlyUse == false)
                .Where_미삭제()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task 공정_저장(공정정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isAdd == true)
            {
                var key = $"P{dc.공정정보.Count() + 1:0000}";
                info.공정코드 = key;

                dc.공정정보.Add(info);
            }
            else
                dc.공정정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task 공정_삭제(공정정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.공정정보.Remove(info);
            else
            {
                info.삭제유무 = true;
                dc.공정정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }
        // }}}

        public Task<IEnumerable<BOM정보>> BOM_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.BOM정보
                .Include(x => x.품목)
                .Where_미삭제_사용()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        /////////////////////////// 2021.03.10 추가 ///////////////////////////
        public Task<IEnumerable<BOM품목정보현황>> BOM품목정보_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.BOM품목정보
                .Include(x => x.S_BOM정보상세)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();
            var result = (from BOM in dc.BOM품목정보
                          join ITEM in dc.품목정보 on BOM.BOM품목정보코드 equals ITEM.품목코드 into ps
                          from p in ps.DefaultIfEmpty()
                          select new BOM품목정보현황
                          {
                              BOM품목정보코드 = BOM.BOM품목정보코드,
                              품목구분코드 = BOM.품목구분코드,
                              품목구분명 = p.품목구분.코드명,
                              품목명 = p.품목명,
                              규격 = p.규격,
                              단위명 = p.단위.코드명
                          }).ToList();



            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<BOM품목정보상세>> BOM품목정보상세_조회(string 품목코드, string 공정단위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            List<BOM품목정보상세> list = new List<BOM품목정보상세>();
            if (공정단위코드 != "")
            {
                list = dc.BOM품목정보상세
                   .Include(x => x.품목)
                   .Where(x => x.BOM품목정보코드 == 품목코드 && x.공정단위코드 == 공정단위코드)
                   .Where_미삭제_사용()
                   .Order_등록최신()
                   .ToList();
            }
            else
            {
                list = dc.BOM품목정보상세
                    .Include(x => x.품목)
                    .Where(x => x.BOM품목정보코드 == 품목코드)
                    .Where_미삭제_사용()
                    .Order_등록최신()
                    .ToList();

            }
            return Task.FromResult(list.AsEnumerable());
        }

        public Task BOM품목정보상세_저장(BOM품목정보상세 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = (from t in dc.BOM품목정보상세
                          where t.품목코드 == info.품목코드 && t.BOM품목정보코드 == info.BOM품목정보코드
                          select t).DefaultIfEmpty().Single();

            var s_info = new BOM품목정보상세
            {
                품목코드 = info.품목코드,
                BOM품목정보코드 = info.BOM품목정보코드,
                공정단위코드 = info.공정단위코드,
                //정미수량 = info.정미수량,
                //로스율 = info.로스율,
                필요수량 = info.필요수량,
                레벨 = info.레벨
            };


            if (result != null)
                dc.BOM품목정보상세.Update(s_info);
            else
                dc.BOM품목정보상세.Add(s_info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task BOM품목정보상세공정단위_저장(string 공정단위코드, string 품목코드, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = (from t in dc.BOM품목정보상세
                          where t.BOM품목정보코드 == 품목코드
                          select t);

            if (result != null)
            {
                foreach (var item in result)
                {
                    item.공정단위코드 = 공정단위코드;
                    dc.BOM품목정보상세.Update(item);
                }


                dc.SaveChanges();
            }

            return Task.CompletedTask;
        }

        // [추가] 2021.03.25
        public Task BOM품목정보_저장(BOM품목정보현황 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result1 = (from t in dc.BOM품목정보
                           where t.BOM품목정보코드 == info.BOM품목정보코드
                           select t).DefaultIfEmpty().Single();


            var s_info = new BOM품목정보
            {
                BOM품목정보코드 = info.BOM품목정보코드,
                품목구분코드 = info.품목.품목구분코드,
                //정미수량 = info.정미수량,
                //로스율 = info.로스율,
                필요수량 = info.필요수량,
            };

            if (result1 != null)
                dc.BOM품목정보.Update(s_info);
            else
                dc.BOM품목정보.Add(s_info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task BOM품목정보_삭제(BOM품목정보현황 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = (from t in dc.BOM품목정보상세
                          where t.BOM품목정보코드 == info.BOM품목정보코드
                          select t);
            var info1 = new BOM품목정보
            {
                BOM품목정보코드 = info.BOM품목정보코드,
            };
            foreach (var item in result)
            {
                dc.BOM품목정보상세.Remove(item);
            }


            dc.BOM품목정보.Remove(info1);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task BOM품목정보상세_삭제(BOM품목정보상세 info)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dc.BOM품목정보상세.Remove(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        /////////////////////////// 2021.03.11 추가 ///////////////////////////

        /////////////////////////// 2021.03.11 추가 ///////////////////////////
        public Task<IEnumerable<발주정보>> 발주정보_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.발주정보
                .Include(x => x.거래처)
                .Include(x => x.S_발주정보상세)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();
            //var result = (from BOM in dc.발주정보
            //              join ITEM in dc.거래처정보 on BOM.거래처코드 equals ITEM.거래처코드 into ps
            //              from p in ps.DefaultIfEmpty()
            //              select new 발주정보현황
            //              {
            //                  발주순번 = BOM.발주순번,
            //                  발주서명 = BOM.발주서명,
            //                  거래처명 = p.거래처명,
            //                  발주상태 = BOM.발주상태코드,
            //                  발주일시 = BOM.발주일시,
            //                  입고예정일시 = BOM.입고예정일시,
            //                  비고 = BOM.비고
            //              }).ToList();



            return Task.FromResult(list.AsEnumerable());
        }

        public Task 발주정보_저장(발주정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            info.거래처 = null;

            if (isAdd == true)
            {
                info.사용유무 = true;
                dc.발주정보.Add(info);
            }
            else
                dc.발주정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<IEnumerable<발주정보상세>> 발주정보상세_조회(int 발주순번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            List<발주정보상세> list = new List<발주정보상세>();

            list = dc.발주정보상세
                .Include(x => x.품목)
                .Where(x => x.발주순번 == 발주순번)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();


            return Task.FromResult(list.AsEnumerable());
        }

        public Task 발주정보상세_저장(발주정보상세 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            //var result = (from t in dc.발주정보상세
            //              where t.발주순번 == info.발주순번
            //              select t).DefaultIfEmpty().Single();

            var s_info = new 발주정보상세
            {
                발주순번 = info.발주순번,
                품목코드 = info.품목코드,
                품목구분코드 = info.품목구분코드,
                발주수량 = info.발주수량,
                입고수량 = info.입고수량
            };


            if (isAdd == true)
                dc.발주정보상세.Add(s_info);
            else
                dc.발주정보상세.Update(s_info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task 발주정보_삭제(발주정보 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            if (isCompletely == true)
                dc.발주정보.Remove(info);

            else
            {
                info.삭제유무 = true;
                dc.발주정보.Update(info);
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task 발주정보상세_삭제(발주정보상세 info, bool isCompletely)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            dc.발주정보상세.Remove(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }






        // 2021 04 08  더존 데이터 조회
        public Task<IEnumerable<VL_MES_EMP>> VL_MES_EMP()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_EMP> list = new List<VL_MES_EMP>();

            list = dc.VL_MES_EMP.Select(x => x)
                .ToList();


            return Task.FromResult(list.AsEnumerable());
        }

        //창고VIEW
        public Task<IEnumerable<VL_MES_LOC>> VL_MES_LOC()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_LOC> list = new List<VL_MES_LOC>();

            list = dc.VL_MES_LOC.Select(x => x)
                .ToList();


            return Task.FromResult(list.AsEnumerable());
        }

        //품목VIEW
        public Task<IEnumerable<VL_MES_ITEM>> VL_MES_ITEM()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_ITEM> list = new List<VL_MES_ITEM>();

            //var result = dc.VL_MES_ITEM.Select(x => x)
            //   .FirstOrDefault();

            list = dc.VL_MES_ITEM.Select(x => x)
                .ToList();


            return Task.FromResult(list.AsEnumerable());
        }


        public Task<IEnumerable<VL_MES_DIV>> VL_MES_DIV()
        {

            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_DIV> list = new List<VL_MES_DIV>();

            list = dc.VL_MES_DIV.Select(x => x)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        //부서
        public Task<IEnumerable<VL_MES_DEPT>> VL_MES_DEPT()
        {

            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_DEPT> list = new List<VL_MES_DEPT>();

            list = dc.VL_MES_DEPT.Select(x => x)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }


        //거래처
        public Task<IEnumerable<VL_MES_CUST>> VL_MES_CUST()
        {

            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_CUST> list = new List<VL_MES_CUST>();

            list = dc.VL_MES_CUST.Select(x => x)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }


        public Task<IEnumerable<VL_MES_BOM>> VL_MES_BOM()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_BOM> list = new List<VL_MES_BOM>();

            list = dc.VL_MES_BOM.Select(x => x)
                .ToList();

            return Task.FromResult(list.AsEnumerable());

        }

        //주문VIEW
        public Task<IEnumerable<VL_MES_SO>> VL_MES_SO()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_SO> list = new List<VL_MES_SO>();

            list = dc.VL_MES_SO.Select(x => x)
                .ToList();

            return Task.FromResult(list.AsEnumerable());

        }

        //발주VIEW
        public Task<IEnumerable<VL_MES_PO>> VL_MES_PO()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_PO> list = new List<VL_MES_PO>();

            list = dc.VL_MES_PO.Select(x => x)
                .ToList();

            return Task.FromResult(list.AsEnumerable());

        }


        //외주지시VIEW
        public Task<IEnumerable<VL_MES_WO_WF>> VL_MES_WO_WF()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_WO_WF> list = new List<VL_MES_WO_WF>();

            list = dc.VL_MES_WO_WF.Select(x => x)
                .ToList();

            return Task.FromResult(list.AsEnumerable());

        }

        //물류담당자 정보 VIEW
        public Task<IEnumerable<VL_MES_PLN>> VL_MES_PLN()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_PLN> list = new List<VL_MES_PLN>();

            list = dc.VL_MES_PLN.Select(x => x)
                .ToList();

            return Task.FromResult(list.AsEnumerable());

        }

        //
        public Task<IEnumerable<VL_MES_ADJUST>> VL_MES_ADJUST()
        {
            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            List<VL_MES_ADJUST> list = new List<VL_MES_ADJUST>();

            list = dc.VL_MES_ADJUST.Select(x => x)
                .ToList();


            return Task.FromResult(list.AsEnumerable());

        }

        public Task<bool> VL_MES_CUST_반영(List<VL_MES_CUST> 더존거래처)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            bool result = false;

            try
            {

                var sql = "DELETE FROM 거래처정보";
                //dc.Database.ExecuteSqlRaw(sql);


                // List<거래처정보> 거래처정보 = new List<거래처정보>();
                foreach (var item in 더존거래처)
                {
                    string 거래처구분코드 = "";
                    switch (item.TR_FG)
                    {
                        case "1":
                            거래처구분코드 = "B0301";
                            break;
                        case "2":
                            거래처구분코드 = "B0302";
                            break;
                        case "3":
                            거래처구분코드 = "B0303";
                            break;
                        case "4":
                            거래처구분코드 = "B0304";
                            break;
                        case "5":
                            거래처구분코드 = "B0305";
                            break;
                        case "6":
                            거래처구분코드 = "B0306";
                            break;
                        case "7":
                            거래처구분코드 = "B0307";
                            break;
                        case "8":
                            거래처구분코드 = "B0308";
                            break;
                        case "9":
                            거래처구분코드 = "B0309";
                            break;

                    }

                    var 거래처정보 = new 거래처정보()
                    {
                        회사코드 = item.CO_CD,
                        거래처코드 = item.TR_CD,
                        거래처명 = item.TR_NM,
                        거래처약칭 = item.ATTR_NM,
                        거래처구분코드 = 거래처구분코드,
                        거래처구분명 = item.TR_FG_NM,
                        사용유무 = item.USE_YN == "0" ? true : false,
                        등록번호 = item.REG_NB,
                        담당자 = item.CEO_NM,
                        업태 = item.BUSINESS,
                        종목 = item.JONGMOK,
                        주소 = item.ADDR1 + " " + item.ADDR2
                    };

                    var found = dc.거래처정보.Where(x => x.거래처코드 == item.TR_CD && x.회사코드 == item.CO_CD).FirstOrDefault();
                    if (found == null)
                        dc.거래처정보.Add(거래처정보);
                    else
                        dc.거래처정보.Update(거래처정보);

                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }



        public Task<bool> VL_MES_EMP_반영(List<VL_MES_EMP> 더존사원정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {

                //var sql = "DELETE FROM 직원정보 WHERE 사번 <> 'SYSTEM'";
                //dc.Database.ExecuteSqlRaw(sql);

                // List<거래처정보> 거래처정보 = new List<거래처정보>();
                foreach (var item in 더존사원정보)
                {

                    var joinDate = string.Format("{0:yyyy-MM-dd}", item.JOIN_DT);

                    var rtrDate = string.Format("{0:yyyy-MM-dd}", item.RTR_DT);

                    var 직원정보 = new 직원정보()
                    {
                        회사코드 = item.CO_CD,
                        사번 = item.EMP_CD,
                        사용자명 = item.KOR_NM,
                        부서코드 = item.DEPT_CD,
                        입사일 = joinDate,
                        퇴사일 = rtrDate,
                        사용유무 = item.USR_YN == "0" ? true : false,
                        식별인자 = item.CO_CD + item.EMP_CD,
                        식별번호 = item.EMP_CD,

                    };

                    var newRow2 = new 직원권한정보
                    {
                        //사번 = newRow.사번,
                        회사코드 = item.CO_CD,
                        암호 = item.EMP_CD,
                        암호암호화유무 = false,
                        식별인자 = item.CO_CD + item.EMP_CD,
                        사용유무 = true,
                        삭제유무 = false
                    };

                    var found2 = dc.직원권한정보.Where(x => x.식별인자 == newRow2.식별인자 ).FirstOrDefault();

                    if (found2 == null)
                        dc.직원권한정보.Add(newRow2);



                    var found = dc.직원정보.Where(x => x.사번 == item.EMP_CD && x.회사코드 == item.CO_CD).FirstOrDefault();
                    if (found == null)
                        dc.직원정보.Add(직원정보);
                    else
                    {
                        found.회사코드 = item.CO_CD;
                        found.사번 = item.EMP_CD;
                        found.사용자명 = item.KOR_NM;
                        found.부서코드 = item.DEPT_CD;
                        found.입사일 = joinDate;
                        found.퇴사일 = rtrDate;
                        found.사용유무 = item.USR_YN == "0" ? true : false;
                        found.식별인자 = item.CO_CD + item.EMP_CD;
                        found.식별번호 = item.EMP_CD;
                        dc.직원정보.Update(found);
                    }

                    //dc.직원정보.Add(직원정보);


                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }


        public Task<bool> VL_MES_DEPT_반영(List<VL_MES_DEPT> 더존부서정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {

                var sql = "DELETE FROM 부서정보 WHERE 부서코드 <> '9000' ";
                //dc.Database.ExecuteSqlRaw(sql);

                foreach (var item in 더존부서정보)
                {
                    var 부서정보 = new 부서정보()
                    {
                        회사코드 = item.CO_CD,
                        부서코드 = item.DEPT_CD,
                        부문코드 = item.SECT_CD,
                        부문명 = item.SECT_NM,
                        사업장코드 = item.DIV_CD,
                        부서명 = item.DEPT_NM
                    };

                    var found = dc.부서정보.Where(x => x.부서코드 == item.DEPT_CD && x.회사코드 == item.CO_CD && x.부문코드 == item.SECT_CD).FirstOrDefault();
                    if (found == null)
                        dc.부서정보.Add(부서정보);
                    else
                    {
                        found.회사코드 = item.CO_CD;
                        found.부서코드 = item.DEPT_CD;
                        found.부문코드 = item.SECT_CD;
                        found.부문명 = item.SECT_NM;
                        found.사업장코드 = item.DIV_CD;
                        found.부서명 = item.DEPT_NM;

                        dc.부서정보.Update(found);
                    }


                    //dc.부서정보.Add(부서정보);


                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }


        public Task<bool> VL_MES_ITEM_반영(List<VL_MES_ITEM> 더존품목정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {

                //var sql = "DELETE FROM 품목정보";
                //dc.Database.ExecuteSqlRaw(sql);

                var 품목 = 더존품목정보.Where(x => x.CO_CD == "2265" && x.ITEM_CD != "").ToList();

                foreach (var item in 품목)
                {
                    //생산품 - > 제품  부품 -> 부재료
                    string 단위코드 = "";
                    string 조달구분 = "";
                    string 품목구분코드 = "";

                    if (item.UNIT_DC.Contains("EA", StringComparison.OrdinalIgnoreCase))
                    {
                        단위코드 = "B1101";
                    }
                    else if (item.UNIT_DC.Contains("won", StringComparison.OrdinalIgnoreCase))
                    {
                        단위코드 = "B1102";
                    }
                    else if (item.UNIT_DC.Contains("SET", StringComparison.OrdinalIgnoreCase))
                    {
                        단위코드 = "B1101";
                    }


                    if (item.ODR_FG.Contains("0"))
                    {
                        조달구분 = "B1601";
                    }
                    else if (item.ODR_FG.Contains("1"))
                    {
                        조달구분 = "B1602";
                    }
                    else if (item.ODR_FG.Contains("2"))
                    {
                        조달구분 = "B1608";
                    }

                    //0.원재료 1.부재료 2.제품 4.반제품 5.상품 6.저장품 7.비용 8.수익

                    if (item.ACCT_FG.Contains("0"))
                    {
                        품목구분코드 = "B1201";
                    }
                    else if (item.ACCT_FG.Contains("1"))
                    {
                        품목구분코드 = "B1202";
                    }
                    else if (item.ACCT_FG.Contains("2"))
                    {
                        품목구분코드 = "B1203";
                    }
                    //else if(item.ACCT_FG.Contains("3"))
                    //{
                    //    품목구분코드 = "B1204";
                    //}
                    else if (item.ACCT_FG.Contains("4"))
                    {
                        품목구분코드 = "B1204";
                    }
                    else if (item.ACCT_FG.Contains("5"))
                    {
                        품목구분코드 = "B1207";
                    }
                    else if (item.ACCT_FG.Contains("6"))
                    {
                        품목구분코드 = "B1208";
                    }
                    else if (item.ACCT_FG.Contains("7"))
                    {
                        품목구분코드 = "B1206";
                    }
                    else if (item.ACCT_FG.Contains("8"))
                    {
                        품목구분코드 = "B1209";
                    }

                    var 품목정보 = new 품목정보()
                    {

                        회사코드 = item.CO_CD,
                        품목코드 = item.ITEM_CD,
                        원품목코드 = item.ITEM_CD,
                        품목명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        품목구분코드 = 품목구분코드,
                        조달구분코드 = 조달구분,
                        단위코드 = 단위코드,
                        재고단위 = item.UNIT_DC,
                        LOT여부 = item.LOT_FG == "0" ? false : true,
                        사용유무 = item.USE_YN == "0" ? false : true,
                        거래처코드 = item.TRMAIN_CD,
                    };

                    var found = dc.품목정보.Where(x => x.품목코드 == item.ITEM_CD).FirstOrDefault();

                    if (found == null)
                        dc.품목정보.Add(품목정보);
                    else
                        dc.품목정보.Update(품목정보);

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



        public Task<bool> VL_MES_LOC_반영(List<VL_MES_LOC> 더존창고정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {

                var sql = "DELETE FROM 장소정보";
                //dc.Database.ExecuteSqlRaw(sql);

                var sql2 = "DELETE FROM 장소위치정보";
                //dc.Database.ExecuteSqlRaw(sql2);

                //var 창고 = 더존창고정보.Where(x => x.CO_CD == "2265").ToList();
                var 창고 = 더존창고정보.ToList();

                //var 창고2 = 창고.GroupBy(x => x.BASELOC_CD).Select(g => g.First()).ToList();
                var 장소 = 창고.GroupBy(x => new { x.CO_CD, x.BASELOC_CD }).Select(g => g.First()).ToList();
                foreach (var item in 장소)
                {

                    string 공정구분코드 = "";
                    if (item.BASELOC_FG.Contains("0"))
                    {
                        공정구분코드 = "B3001";
                    }
                    else if (item.BASELOC_FG.Contains("1"))
                    {
                        공정구분코드 = "B3002";
                    }
                    else if (item.BASELOC_FG.Contains("2"))
                    {
                        공정구분코드 = "B3003";
                    }

                    var 장소정보 = new 장소정보()
                    {
                        회사코드 = item.CO_CD,
                        공정구분코드 = 공정구분코드,

                        장소코드 = item.BASELOC_CD,
                        장소명 = item.BASELOC_NM,
                        장소사용여부 = item.BASELOC_USE_YN,
                        //장소유형코드 = 장소유형코드,
                        사업장코드 = item.DIV_CD
                    };

                    var resp = dc.장소정보.Where(x => x.회사코드 == item.CO_CD && x.장소코드 == item.BASELOC_CD).FirstOrDefault();
                    if (resp == null)
                        dc.장소정보.Add(장소정보);

                }
                dc.SaveChanges();

                foreach (var item2 in 창고)
                {

                    var 장소위치정보 = new 장소위치정보()
                    {
                        회사코드 = item2.CO_CD,
                        장소코드 = item2.BASELOC_CD,
                        장소위치코드 = $"{item2.BASELOC_CD}{item2.LOC_CD}",
                        위치코드 = item2.LOC_CD,
                        위치명 = item2.LOC_NM,
                        //작업장사용여부 = item.LOC_USE_YN,

                    };
                    var resp2 = dc.장소위치정보.Where(x => x.회사코드 == item2.CO_CD && x.장소코드 == item2.BASELOC_CD
                                 && x.장소위치코드 == 장소위치정보.장소위치코드).FirstOrDefault();
                    if (resp2 == null)
                        dc.장소위치정보.Add(장소위치정보);
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


        public Task<bool> VL_MES_BOM_반영(List<VL_MES_BOM> 더존BOM정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {

                var sql = "DELETE FROM BOM_정보 ";
                dc.Database.ExecuteSqlRaw(sql);

                foreach (var item in 더존BOM정보)
                {

                    var BOM_정보 = new BOM_정보()
                    {
                        회사코드 = item.CO_CD,
                        모품번 = item.ITEMPARENT_CD,
                        모품명 = item.ITEMPARENT_NM,
                        모규격 = item.ITEMPARENT_DC,
                        모품목재고단위 = item.ITEMPARENT_UNIT_DC,

                        순번 = item.BOM_SQ,
                        자품번 = item.ITEMCHILD_CD,
                        자품명 = item.ITEMCHILD_NM,
                        자규격 = item.ITEMCHILD_DC,
                        자품목재고단위 = item.ITEMCHILD_UNIT_DC,

                        정미수량 = item.JUST_QT,
                        LOSS율 = item.LOSS_RT,
                        필요수량 = item.REAL_QT,
                        외주구분 = item.OUT_FG,
                        임가공구분 = item.ODR_FG,
                        주거래처코드 = item.TRMAIN_CD,
                        주거래처명 = item.ATTR_NM,
                        시작일자 = item.START_DT,
                        종료일자 = item.END_DT,
                        사용여부 = item.USE_YN,
                    };

                    var found = dc.BOM_정보.Where(x => x.모품번 == item.ITEMPARENT_CD && x.자품번 == item.ITEMCHILD_NM && x.회사코드 == item.CO_CD).FirstOrDefault();

                    if (found == null)
                        dc.BOM_정보.Add(BOM_정보);
                    else
                        dc.BOM_정보.Update(BOM_정보);


                    //var 품목정보 = new 품목정보()
                    //{

                    //    회사코드 = item.CO_CD,
                    //    품목코드 = item.ITEMPARENT_CD,
                    //    원품목코드 = item.ITEMPARENT_CD,
                    //    품목명 = item.ITEMPARENT_NM,
                    //    규격 = item.ITEMPARENT_DC,
                    //    품목구분코드 = "B1202",
                    //    조달구분코드 = "B1601",
                    //    단위코드 = "B1101",
                    //    재고단위 = item.ITEMPARENT_UNIT_DC,
                    //};

                    //var found1 = dc.품목정보.Where(x => x.품목코드 == item.ITEMPARENT_CD).FirstOrDefault();

                    //if (found1 == null)
                    //    dc.품목정보.Add(품목정보);



                    //dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            try
            {
                foreach (var item in 더존BOM정보)
                {
                    var 품목정보 = new 품목정보()
                    {

                        회사코드 = item.CO_CD,
                        품목코드 = item.ITEMPARENT_CD,
                        원품목코드 = item.ITEMPARENT_CD,
                        품목명 = item.ITEMPARENT_NM,
                        규격 = item.ITEMPARENT_DC,
                        품목구분코드 = "B1202",
                        조달구분코드 = "B1601",
                        단위코드 = "B1101",
                        재고단위 = item.ITEMPARENT_UNIT_DC,
                    };

                    var found1 = dc.품목정보.Where(x => x.품목코드 == item.ITEMPARENT_CD).FirstOrDefault();

                    if (found1 == null)
                    {
                        dc.품목정보.Add(품목정보);
                        dc.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            try
            {
                foreach (var item in 더존BOM정보)
                {
                    var 품목정보 = new 품목정보()
                    {

                        회사코드 = item.CO_CD,
                        품목코드 = item.ITEMCHILD_CD,
                        원품목코드 = item.ITEMCHILD_CD,
                        품목명 = item.ITEMCHILD_NM,
                        규격 = item.ITEMCHILD_DC,
                        품목구분코드 = "B1202",
                        조달구분코드 = "B1601",
                        단위코드 = "B1101",
                        재고단위 = item.ITEMCHILD_UNIT_DC,
                    };

                    var found = dc.품목정보.Where(x => x.품목코드 == item.ITEMCHILD_CD).FirstOrDefault();

                    if (found == null)
                        dc.품목정보.Add(품목정보);
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();
            result = true;

            return Task.FromResult(result);
        }

        public Task<bool> VL_MES_ADJUST_반영(List<VL_MES_ADJUST> 더존재고조정현황)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                //var sql = "DELETE FROM 부서정보 WHERE 부서코드 <> '9000'";
                //dc.Database.ExecuteSqlRaw(sql);

                var 품목정보 = dc.품목정보.ToList();

                var 재고조종 = 더존재고조정현황.ToList();

                //var 재고조정품목정보 = dc.재고조정품목정보.ToList();
                foreach (var item in 재고조종)
                {
                    var 품목코드 = 품목정보.Where(x => x.품목코드 == item.ITEM_CD).FirstOrDefault();

                    var 재고조정정보유무 = dc.재고조정정보이력.Where(x => x.CO_CD == item.CO_CD && x.ADJUST_NB == item.ADJUST_NB && x.ADJUST_SQ == item.ADJUST_SQ).FirstOrDefault();

                    //var 재고조정품목 = dc.재고조정품목정보.Where(x => x.품목코드 == item.ITEM_CD).FirstOrDefault();

                    if (item.ITEM_CD == null)
                        continue;

                    if (item.CO_CD == "9998")
                        재고조정품목_저장(item);

                    if (재고조정정보유무 != null)
                        continue;

                    if (품목코드 != null)
                    {

                        var 보유품목발행 = 재고조정_보유품목_발행(품목코드, item);
                        //재고조정보이력 쌓는다.

                        var 재고조정정보이력 = new 재고조정정보이력
                        {
                            CO_CD = item.CO_CD,
                            ADJUST_NB = item.ADJUST_NB,
                            ADJUST_SQ = Convert.ToDecimal(item.ADJUST_SQ),
                            ADJUST_FG = item.ADJUST_FG,
                            ADJUST_FG_NM = item.ADJUST_FG_NM,
                            ADJUST_DT = item.ADJUST_DT,
                            WH_CD = item.WH_CD,
                            WH_NM = item.WH_NM,
                            LC_CD = item.LC_CD,
                            LC_NM = item.LC_NM,
                            PLN_CD = item.PLN_CD,
                            PLN_NM = item.PLN_NM,
                            ITEM_CD = item.ITEM_CD,
                            ITEM_NM = item.ITEM_NM,
                            ITEM_DC = item.ITEM_DC,
                            UNIT_DC = item.UNIT_DC,
                            UNITMANG_DC = item.UNITMANG_DC,
                            UNITCHNG_NB = Convert.ToDecimal(item.UNITCHNG_NB),
                            QT = Convert.ToDecimal(item.QT),
                            ADJUST_UM = Convert.ToDecimal(item.ADJUST_UM),
                            ADJUST_AM = Convert.ToDecimal(item.ADJUST_AM),
                            LOT_NB = item.LOT_NB,
                            MGMT_CD = item.MGMT_CD,
                            MGM_NM = item.MGM_NM,
                            PJT_CD = item.PJT_CD,
                            PJT_NM = item.PJT_NM,
                            REMARK_DC_H = item.REMARK_DC_H,
                            REMARK_DC_D = item.REMARK_DC_D,
                            TR_CD = item.TR_CD,
                            TR_NM = item.TR_NM,

                        };
                        dc.재고조정정보이력.Add(재고조정정보이력);

                    }
                    else
                    {
                        //확인후 적용함.
                        if (item.ITEM_CD != null)
                        {
                            // 재고조정_품목_저장(품목코드, item);
                            // var 보유품목발행 = 재고조정_보유품목_발행(품목코드, item);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }

        public void 재고조정품목_저장(VL_MES_ADJUST 재고조정)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            try
            {
                var 재고조정품목정보 = new 재고조정품목정보
                {
                    회사코드 = 재고조정.CO_CD,
                    품목코드 = 재고조정.ITEM_CD,
                    원품목코드 = 재고조정.ITEM_CD,
                    관리차수 = 1,
                    품목명 = 재고조정.ITEM_NM,
                    품목구분코드 = 품목구분(재고조정.ADJUST_FG),  //임시
                    계정구분코드 = 재고조정.ADJUST_FG,
                    조달분류 = "1",
                    조달구분코드 = 조달구분("1"),
                    재고단위 = 재고조정.UNIT_DC,
                    단위코드 = 단위코드(재고조정.UNIT_DC),
                    조정수량 = 재고조정.QT,
                };

                var result = dc.재고조정품목정보.Where(x => x.품목코드 == 재고조정.ITEM_CD).FirstOrDefault();

                if (result == null)
                {
                    //info.품목코드 = $"{info.원품목코드}:{info.관리차수}";
                    dc.재고조정품목정보.Add(재고조정품목정보);
                }
                else
                    dc.재고조정품목정보.Update(재고조정품목정보);
            }
            catch (Exception ex)
            {
            }


            dc.SaveChanges();

        }


        public bool 재고조정_보유품목_발행(품목정보 품목, VL_MES_ADJUST 재고조정)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var now = DateTime.Now;
            //var yyMMdd = now.ToString("yyMMdd");
            //var no = dc.보유품목정보.Count(x => x.품목코드 == 품목.품목코드);
            var result = dc.보유품목정보.Where(x => x.품목코드 == 품목.품목코드 && x.회사코드 == 재고조정.CO_CD).FirstOrDefault();


            var info = new 보유품목정보
            {
                //보유품목코드 = $"{품목.품목코드}:{yyMMdd}:{no}",
                보유품목코드 = 품목.품목코드,
                품목코드 = 품목.품목코드,
                보유년월일 = 재고조정.ADJUST_DT,
                조정년월일 = 재고조정.ADJUST_DT,
                순번 = Convert.ToInt32(재고조정.ADJUST_SQ),
                보유일 = now,
                수량 = Convert.ToDecimal(재고조정.QT),
                품목구분코드 = 품목.품목구분코드,
                회사코드 = 재고조정.CO_CD,
                //원보유품목코드 = 재고조정.ITEM_CD,
                장소코드 = 재고조정.WH_CD,
                장소위치코드 = $"{재고조정.WH_CD}{재고조정.LC_CD}",
            };

            if (result != null)
                dc.보유품목정보.Update(info);
            else
                dc.보유품목정보.Add(info);
            dc.SaveChanges();


            return true;
        }


        public string 재고조정_품목_저장(품목정보 info, VL_MES_ADJUST 재고조정)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var 품목정보 = new 품목정보
            {
                회사코드 = 재고조정.CO_CD,
                품목코드 = 재고조정.ITEM_CD,
                원품목코드 = 재고조정.ITEM_CD,
                관리차수 = 1,
                품목명 = 재고조정.ITEM_NM,
                품목구분코드 = 품목구분(재고조정.ADJUST_FG),  //임시
                계정구분코드 = 재고조정.ADJUST_FG,
                조달분류 = "1",
                조달구분코드 = 조달구분("1"),
                재고단위 = 재고조정.UNIT_DC,
                단위코드 = 단위코드(재고조정.UNIT_DC),

            };

            var result = dc.품목정보.Where(x => x.품목코드 == 재고조정.ITEM_CD).FirstOrDefault();

            if (result == null)
            {
                //info.품목코드 = $"{info.원품목코드}:{info.관리차수}";
                dc.품목정보.Add(품목정보);
            }
            else
                dc.품목정보.Update(품목정보);

            dc.SaveChanges();

            return info.품목코드;
        }

        //주문서 적용
        public Task<bool> VL_MES_SO_반영(List<VL_MES_SO> 더존주문서정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                var sql = "DELETE FROM 주문서헤더정보 ";
                dc.Database.ExecuteSqlRaw(sql);
                //var 주문서 = 더존주문서정보.Where(x => x.CO_CD == "2265").ToList();
                var 주문서 = 더존주문서정보.ToList();

                //주문서헤더정보
                foreach (var item in 주문서)
                {

                    var 주문서헤더정보 = new 주문서헤더정보
                    {
                        회사코드 = item.CO_CD,
                        주문번호 = item.SO_NB,
                        주문일자 = item.SO_DT,
                        고객명 = item.TR_NM,
                        과세구분명 = item.VAT_NM,
                        납품처명 = item.SHIP_NM,
                        담당자명 = item.PLN_NM,
                        거래처코드 = item.TR_CD,
                        과세구분 = item.VAT_FG,
                        주문구분 = item.SO_FG,
                    };

                    var found = dc.주문서헤더정보.Where(x => x.회사코드 == item.CO_CD && x.주문번호 == item.SO_NB).FirstOrDefault();

                    if (found == null)
                    {
                        dc.주문서헤더정보.Add(주문서헤더정보);
                        dc.SaveChanges();
                    }


                }


                //주문서정보
                foreach (var item in 주문서)
                {

                    var 주문서정보 = new 주문서정보
                    {
                        회사코드 = item.CO_CD,
                        사업장코드 = item.DIV_CD,
                        부서코드 = item.DEPT_CD,
                        사원코드 = item.EMP_CD,
                        주문번호 = item.SO_NB,
                        주문일자 = item.SO_DT,
                        고객코드 = item.TR_CD,
                        고객명 = item.TR_NM,
                        주문구분 = item.SO_FG,
                        과세구분 = item.VAT_FG,
                        과세구분명 = item.VAT_NM,
                        단가구분 = item.UMVAT_FG,
                        단가구분명 = item.UMVAT_NM,
                        납품처코드 = item.SHIP_CD,
                        납품처명 = item.SHIP_NM,
                        담당자코드 = item.PLN_CD,
                        담당자명 = item.PLN_NM,
                        관리번호 = item.DUMMY1,
                        헤더비고 = item.REMARK_DC,
                        순번 = item.SO_SQ,
                        품목코드 = item.ITEM_CD,
                        품목명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        관리단위 = item.UNITMANG_DC,
                        납기일 = item.DUE_DT,
                        출하예정일 = item.SHIPREQ_DT,
                        수량 = item.SO_QT,
                        단가 = item.SO_UM,
                        부가세단가 = item.VAT_UM,
                        공급가 = item.SOG_AM,
                        SOV_AM = item.SOV_AM,
                        합계액 = item.SOH_AM,
                        관리구분코드 = item.MGMT_CD,
                        관리구분명 = item.MGM_NM,
                        프로젝트코드 = item.PJT_CD,
                        프로젝트명 = item.PJT_NM,
                        디테일비고 = item.REMARK_DC_D,
                        마감여부 = item.EXPIRE_YN,
                        검사구분 = item.QC_FG,
                        환종 = item.EXCH_CD,

                    };

                    var found = dc.주문서정보.Where(x => x.회사코드 == item.CO_CD && x.주문번호 == item.SO_NB && x.순번 == item.SO_SQ).FirstOrDefault();

                    if (found == null)
                        dc.주문서정보.Add(주문서정보);
                    else
                        dc.주문서정보.Update(주문서정보);

                    //dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }



        public Task<bool> VL_MES_PO_반영(List<VL_MES_PO> 더존발주서정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                var sql = "DELETE FROM 발주서헤더정보 ";
                dc.Database.ExecuteSqlRaw(sql);
                //var 발주서 = 더존발주서정보.Where(x => x.CO_CD == "2265").ToList();
                var 발주서 = 더존발주서정보.ToList();

                //발주서헤더정보
                foreach (var item in 발주서)
                {
                    var 발주서헤더정보 = new 발주서헤더정보
                    {
                        회사코드 = item.CO_CD,
                        발주번호 = item.PO_NB,
                        발주일 = item.PO_DT,
                        거래처명 = item.TR_NM,
                        과세구분명 = item.VAT_NM,
                        담당자명 = item.PLN_NM,
                        과세구분 = item.VAT_FG,
                        거래구분 = item.PO_FG,
                        거래처코드 = item.TR_CD,

                    };

                    var found = dc.발주서헤더정보.Where(x => x.회사코드 == item.CO_CD && x.발주번호 == item.PO_NB).FirstOrDefault();

                    if (found == null)
                    {
                        dc.발주서헤더정보.Add(발주서헤더정보);
                        dc.SaveChanges();
                    }


                    //dc.발주서헤더정보.Add(발주서헤더정보);
                }



                //발주서정보
                foreach (var item in 발주서)
                {

                    var 발주서정보 = new 발주서정보
                    {
                        회사코드 = item.CO_CD,
                        사업장코드 = item.DIV_CD,
                        부서코드 = item.DEPT_CD,
                        사원코드 = item.EMP_CD,
                        발주번호 = item.PO_NB,
                        발주일 = item.PO_DT,
                        거래처코드 = item.TR_CD,
                        거래처명 = item.TR_NM,
                        거래구분 = item.PO_FG,
                        검사구분 = item.QC_FG,
                        과세구분 = item.VAT_FG,
                        과세구분명 = item.VAT_NM,
                        담당자코드 = item.PLN_CD,
                        담당자명 = item.PLN_NM,
                        비고 = item.REMARK_DC,
                        발주순번 = item.PO_SQ,
                        품번 = item.ITEM_CD,
                        품명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        관리단위 = item.UNITMANG_DC,
                        납기일 = item.DUE_DT,
                        출하예정일 = item.SHIPREQ_DT,
                        발주수량 = item.PO_QT,
                        발주단가 = item.PO_UM,
                        공급가 = item.POG_AM,
                        부가세 = item.POGV_AM1,
                        합계액 = item.POGH_AM1,
                        관리구분코드 = item.MGMT_CD,
                        관리구분명 = item.MGM_NM,
                        프로젝트 = item.PJT_CD,
                        프록젝트명 = item.PJT_NM,
                        비고_내역 = item.REMARK_DC_D,
                        환종 = item.EXCH_CD,
                        부가세구분 = item.UMVAT_FG,

                    };

                    var found = dc.발주서정보.Where(x => x.회사코드 == item.CO_CD && x.발주번호 == item.PO_NB && x.발주순번 == item.PO_SQ).FirstOrDefault();

                    if (found == null)
                        dc.발주서정보.Add(발주서정보);
                    else
                        dc.발주서정보.Update(발주서정보);

                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            //dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }




        public Task<bool> VL_MES_PLN_반영(List<VL_MES_PLN> 더존물류담당자정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                var sql = "DELETE FROM 물류담당자정보 ";
                dc.Database.ExecuteSqlRaw(sql);

                var 물류담당자 = 더존물류담당자정보.ToList();

                foreach (var item in 물류담당자)
                {

                    var 물류담당자정보 = new 물류담당자정보
                    {
                        회사코드 = item.CO_CD,
                        담당자코드 = item.PLN_CD,
                        담당자명 = item.PLN_NM,
                        사원코드 = item.EMP_CD,
                        사원명 = item.EMP_NM,
                        전화번호 = item.PLN_TEL,
                        팩스번호 = item.PLN_FAX,
                        핸드폰번호 = item.PLN_CP,
                        담당그룹코드 = item.PLNS_CD,
                        담당그룹명 = item.PLNS_NM,
                        시작일 = item.FROM_DT,
                        종료일 = item.TO_DT,
                        사용여부 = item.USE_YN,

                    };

                    dc.물류담당자정보.Add(물류담당자정보);
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }

        public Task<bool> VL_MES_WO_WF_반영(List<VL_MES_WO_WF> 더존외주지시확정정보)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {

                //var sql = "DELETE FROM 외부작업지시정보 ";
                //dc.Database.ExecuteSqlRaw(sql);

                var 외주지시 = 더존외주지시확정정보.ToList();

                foreach (var item in 외주지시)
                {

                    var 외주작업지시서정보 = new 외주작업지시서정보
                    {
                        회사코드 = item.CO_CD,
                        지시번호 = item.WO_CD,
                        지시일 = DateTime.ParseExact(item.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        완료일 = DateTime.ParseExact(item.COMP_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        품번 = item.ITEM_CD,
                        품명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        관리단위 = item.UNIT_DC,
                        수량 = item.ITEM_QT,
                        전개순번 = item.WOOP_SQ,
                        공정 = item.BASELOC_CD,
                        공정명 = item.BASELOC_NM,
                        작업장 = item.LOC_CD,
                        작업장명 = item.LOC_NM,
                        외주단가 = item.LBR_UM,
                        외주금액 = item.LBR_AM,
                        설비코드 = item.EQUIP_CD,
                        설비명 = item.EQUIP_NM,
                        비고_DOC_DC = item.DOC_DC,
                        지시상태 = item.DOC_ST,
                        지시상태명 = item.DOC_ST_NM,
                        지시구분 = item.WOC_FG,
                        지시구분명 = item.WOC_FG_NM,
                        생산외주구분 = item.DOC_FG,
                        생산외주구분명 = item.DOC_FG_NM,
                        처리구분 = item.WF_FG,
                        처리구분명 = item.WF_FG_NM,
                        검사구분 = item.QC_FG,
                        검사구분명 = item.QC_FG_NM,
                        LOT번호 = item.LOT_NB,
                        거래처코드 = item.TR_CD,
                        거래처명 = item.TR_NM,
                        거래처약칭 = item.ATTR_NM,
                        주문번호 = item.SO_NB,
                        주문순번 = item.LN_SQ,
                        사업장코드 = item.DIV_CD,
                        작업팀 = item.WTEAM_CD,
                        작업팀명 = item.WTEAM_NM,
                        작업조 = item.WSHFT_CD,
                        작업조명 = item.WSHFT_NM,
                        비고 = item.REMARK_DC,



                    };

                    var found = dc.외주작업지시서정보.Where(x => x.회사코드 == item.CO_CD && x.지시번호 == item.WO_CD).FirstOrDefault();

                    if (found == null)
                        dc.외주작업지시서정보.Add(외주작업지시서정보);
                    else
                        dc.외주작업지시서정보.Update(외주작업지시서정보);
                }

            }
            catch
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;
            return Task.FromResult(result);
        }

        public string 품목구분(string ADJUST_FG)
        {
            //0.원재료 1.부재료 2.제품 4.반제품 5.상품 6.저장품 7.비용 8.수익
            string 품목구분코드 = "";
            if (ADJUST_FG.Contains("0"))
            {
                품목구분코드 = "B1201";
            }
            else if (ADJUST_FG.Contains("1"))
            {
                품목구분코드 = "B1202";
            }
            else if (ADJUST_FG.Contains("2"))
            {
                품목구분코드 = "B1203";
            }
            //else if(item.ACCT_FG.Contains("3"))
            //{
            //    품목구분코드 = "B1204";
            //}
            else if (ADJUST_FG.Contains("4"))
            {
                품목구분코드 = "B1204";
            }
            else if (ADJUST_FG.Contains("5"))
            {
                품목구분코드 = "B1207";
            }
            else if (ADJUST_FG.Contains("6"))
            {
                품목구분코드 = "B1208";
            }
            else if (ADJUST_FG.Contains("7"))
            {
                품목구분코드 = "B1206";
            }
            else if (ADJUST_FG.Contains("8"))
            {
                품목구분코드 = "B1209";
            }

            return 품목구분코드;
        }

        public string 조달구분(string 조달구분)
        {
            string 조달구분코드 = "";
            if (조달구분.Contains("0"))
            {
                조달구분코드 = "B1601";
            }
            else if (조달구분.Contains("1"))
            {
                조달구분코드 = "B1602";
            }
            else if (조달구분.Contains("2"))
            {
                조달구분코드 = "B1608";
            }
            return 조달구분코드;
        }

        public string 단위코드(string UNIT_DC)
        {
            string 단위코드 = "";
            if (UNIT_DC == null)
            {
                단위코드 = "B1101";
                return 단위코드;
            }
            if (UNIT_DC.Contains("EA", StringComparison.OrdinalIgnoreCase))
            {
                단위코드 = "B1101";
            }
            else if (UNIT_DC.Contains("won", StringComparison.OrdinalIgnoreCase))
            {
                단위코드 = "B1102";
            }
            return 단위코드;
        }





        public Task<IEnumerable<BOM_정보>> BOM_정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.BOM_정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task<IEnumerable<부서정보>> 부서정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.부서정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());


        }

        public Task<IEnumerable<물류담당자정보>> 물류담당자정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.물류담당자정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        //public Task<IEnumerable<발주서정보>> 발주서정보_조회(string 회사코드)
        //{
        //    using var scope = dcp.GetDbContextScope();
        //    var dc = scope.DbContext;
        //    var list = dc.발주서정보
        //        .Where(x => x.회사코드 == 회사코드)
        //        .Where_미삭제_사용()
        //        .Order_등록최신()
        //        .ToList();

        //    return Task.FromResult(list.AsEnumerable());
        //}

        //public Task<IEnumerable<주문서정보>> 주문서정보_조회(string 회사코드)
        //{
        //    using var scope = dcp.GetDbContextScope();
        //    var dc = scope.DbContext;
        //    var list = dc.주문서정보
        //        .Where(x => x.회사코드 == 회사코드)
        //        .Where_미삭제_사용()
        //        .Order_등록최신()
        //        .ToList();

        //    return Task.FromResult(list.AsEnumerable());
        //}









        // 더존에 데이터  Insert 

        public Task<bool> 더존입고처리_BARPLUS_LSTOCK(BARPLUS_LSTOCK 입고처리)
        {

            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;
            bool result = false;



            var 입고추가수정유무 = dc.BARPLUS_LSTOCK.Count(x => x.CO_CD == 입고처리.CO_CD && x.WORK_NB == 입고처리.WORK_NB);
            var 순번 = dc.BARPLUS_LSTOCK.Count(x => x.CO_CD == 입고처리.CO_CD) + 1;
            try
            {
                if (입고추가수정유무 == 0)
                {
                    var now = DateTime.Now;
                    var yyyy = now.ToString("yyyy");

                    입고처리.WORK_NB = $"{"RV"}{yyyy}{순번:000000}";
                    dc.BARPLUS_LSTOCK.Add(입고처리);
                }
                else
                {
                    dc.BARPLUS_LSTOCK.Update(입고처리);
                }

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }

        public Task<bool> MES입고처리_입고처리헤더정보_등록(입고처리헤더정보 입고처리, BARPLUS_LSTOCK 입고처리2)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;
            bool result = false;

            var 입고추가수정유무 = dc.입고처리헤더정보.Count(x => x.회사코드 == 입고처리.회사코드 && x.작업번호 == 입고처리.작업번호);
            var 순번 = dc.입고처리헤더정보.Count(x => x.회사코드 == 입고처리.회사코드) + 1;

            var 발주서 = dc_D.VL_MES_PO
                           .Where(x => x.CO_CD == 입고처리.회사코드 && x.PO_NB == 입고처리.발주번호)
                           .FirstOrDefault();
            var BARPLUS_LSTOCK = dc_D.BARPLUS_LSTOCK.Where(x => x.CO_CD == 입고처리.회사코드 && x.WORK_NB == 입고처리.작업번호).FirstOrDefault();
            try
            {
                if (입고추가수정유무 == 0)
                {
                    var now = DateTime.Now;
                    var yyyy = now.ToString("yyyy");
                    입고처리.프로젝트코드 = 발주서.PJT_CD;
                    입고처리2.PJT_CD = 발주서.PJT_CD;

                    입고처리.환종 = 발주서.EXCH_CD;
                    입고처리2.EXCH_CD = 발주서.EXCH_CD;

                    입고처리.거래처코드 = 발주서.TR_CD;
                    입고처리2.TR_CD = 발주서.TR_CD;

                    입고처리.환율 = 1;
                    입고처리2.EXCH_RT = 1;

                    입고처리.사원코드 = 발주서.EMP_CD;
                    입고처리2.EMP_CD = 발주서.EMP_CD;

                    입고처리.부서코드 = 발주서.DEPT_CD;
                    입고처리2.DEPT_CD = 발주서.DEPT_CD;

                    입고처리.거래구분 = 발주서.PO_FG;
                    입고처리2.PO_FG = 발주서.PO_FG;

                    입고처리.과세구분 = 발주서.VAT_FG;
                    입고처리2.VAT_FG = 발주서.VAT_FG;


                    입고처리.작업번호 = $"{"RV"}{yyyy}{순번:000000}";
                    입고처리2.WORK_NB = $"{"RV"}{yyyy}{순번:000000}";
                    dc.입고처리헤더정보.Add(입고처리);

                    if (BARPLUS_LSTOCK == null) { dc_D.BARPLUS_LSTOCK.Add(입고처리2); }
                    else { dc_D.BARPLUS_LSTOCK.Add(입고처리2); }
                }
                else
                {
                    dc.입고처리헤더정보.Update(입고처리);

                    dc_D.BARPLUS_LSTOCK.Update(입고처리2);
                }


            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            try
            {
                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }



        // 더존에 데이터  Insert 

        public Task<bool> 더존입고상세_BARPLUS_LSTOCK_D(BARPLUS_LSTOCK_D 입고상세처리)
        {

            using var scope = dcp.GetDbContextScopeDZ();
            var dc = scope.DbContext;

            bool result = false;

            var 입고상세추가수정유무 = dc.BARPLUS_LSTOCK_D.Count(x => x.CO_CD == 입고상세처리.CO_CD && x.WORK_NB == 입고상세처리.WORK_NB && x.WORK_SQ == 입고상세처리.WORK_SQ);
            var 작업순번 = dc.BARPLUS_LSTOCK_D.Count(x => x.CO_CD == 입고상세처리.CO_CD && x.WORK_NB == 입고상세처리.WORK_NB) + 1;

            var 발주서 = dc.VL_MES_PO
                        .Where(x => x.CO_CD == 입고상세처리.CO_CD && x.PO_NB == 입고상세처리.PO_NB && x.PO_SQ == 입고상세처리.PO_SQ)
                        .FirstOrDefault();

            try
            {
                if (입고상세추가수정유무 == 0)
                {
                    입고상세처리.PJT_CD = 발주서.PJT_CD;
                    입고상세처리.EXCH_CD = 발주서.EXCH_CD;
                    입고상세처리.EXCH_RT = 1;

                    입고상세처리.EXPIRE_YN = "Y";
                    입고상세처리.USE_YN = "0";
                    입고상세처리.CONF_NB3 = 0;

                    입고상세처리.WORK_SQ = 작업순번;
                    dc.BARPLUS_LSTOCK_D.Add(입고상세처리);
                }
                else
                {
                    dc.BARPLUS_LSTOCK_D.Update(입고상세처리);
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

        public Task<bool> MES입고처리_입고처리상세정보_등록(입고처리상세정보 입고상세처리)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            var 입고상세추가수정유무 = dc.입고처리상세정보.Count(x => x.회사코드 == 입고상세처리.회사코드 && x.작업번호 == 입고상세처리.작업번호 && x.작업순번 == 입고상세처리.작업순번);
            var 작업순번 = dc.입고처리상세정보.Count(x => x.회사코드 == 입고상세처리.회사코드 && x.작업번호 == 입고상세처리.작업번호) + 1;


            var 순번_R = dc.입고처리상세정보.Where(x => x.회사코드 == 입고상세처리.회사코드 && x.작업번호 == 입고상세처리.작업번호 && x.작업순번 == 입고상세처리.작업순번).FirstOrDefault();

            try
            {
                if (입고상세추가수정유무 == 0)
                {
                    입고상세처리.작업순번 = 작업순번;
                    dc.입고처리상세정보.Add(입고상세처리);
                }
                else
                {


                    //string 장소위치코드 = "";

                    //장소코드 = 상세.입고장소코드.Substring(0, 4);

                    var result2 = dc.보유품목정보.Where(x => x.품목코드 == 순번_R.품번 && x.회사코드 == 순번_R.회사코드).FirstOrDefault();


                    var info = new 보유품목정보
                    {
                        회사코드 = 입고상세처리.회사코드,
                        보유품목코드 = 입고상세처리.품번,
                        품목코드 = 입고상세처리.품번,
                        //보유년월일 = result != null ? result.보유년월일 : yyyyMMdd,
                        //조정년월일 = null,//result.조정년월일,
                        //보유일 = now,
                        수량 = result2 != null ? result2.수량 - (순번_R.입고수량_관리단위 - 입고상세처리.입고수량_관리단위) : 입고상세처리.입고수량_관리단위,
                        //품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                        //장소코드 = 입고상세처리.입고장소코드,
                        //장소위치코드 = 입고상세처리.입고장소코드,
                    };

                    dc.보유품목정보.Update(info);

                    dc.입고처리상세정보.Update(입고상세처리);
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }

        public Task<IEnumerable<입고처리헤더정보>> MES입고추가_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.입고처리헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }


        public Task<IEnumerable<입고처리상세정보>> MES입고상세_조회(string 작업번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.입고처리상세정보
                .Where(x => x.회사코드 == 회사코드 && x.작업번호 == 작업번호)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }




        public Task<IEnumerable<출고처리헤더정보>> MES출고추가_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.출고처리헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task<IEnumerable<출고처리상세정보>> MES출고상세_조회(string 작업번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.출고처리상세정보
                .Where(x => x.회사코드 == 회사코드 && x.작업번호 == 작업번호)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }





        public Task<bool> MES출고처리_출고처리헤더정보_등록(출고처리헤더정보 출고처리, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            //var 출고추가수정유무 = dc.출고처리헤더정보.Count(x => x.작업번호 == 출고처리.작업번호);
            var 순번 = dc.출고처리헤더정보.Count(x => x.회사코드 == 출고처리.회사코드) + 1;


            //var 더존출고추가수정유무 = dc.BARPLUS_LDELIVER.Count(x => x.WORK_NB == 출고처리.작업번호);
            var 순번_더존 = dc_D.BARPLUS_LDELIVER.Count(x => x.CO_CD == 출고처리.회사코드) + 1;


            var 주문서 = dc_D.VL_MES_SO
                           .Where(x => x.CO_CD == 출고처리.회사코드 && x.SO_NB == 출고처리.주문번호)
                           .FirstOrDefault();
            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                string 작업번호2 = "";
                작업번호1 = $"{"IS"}{yyyy}{순번:000000}";
                작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";

                출고처리.거래처코드 = 주문서.TR_CD;
                출고처리.과세구분 = 주문서.VAT_FG;
                출고처리.단가구분 = 주문서.UMVAT_FG;
                출고처리.납품처코드 = 주문서.SHIP_CD;
                출고처리.사원코드 = 주문서.EMP_CD;

                var BARPLUS_LDELIVER = new BARPLUS_LDELIVER
                {
                    CO_CD = 출고처리.회사코드,
                    WORK_NB = 작업번호2,
                    WORK_DT = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(출고처리.작업일자.ToString())),
                    ISU_FG = 출고처리.출고구분,
                    TR_CD = 출고처리.거래처코드,
                    ISU_DT = String.Format("{0:yyyyMMdd}", Convert.ToDateTime(출고처리.출고일자.ToString())),
                    WH_CD = 출고처리.창고코드,
                    SO_FG = 출고처리.거래구분,
                    EXCH_CD = 출고처리.환종,
                    EXCH_RT = 출고처리.환율,
                    EMP_CD = 출고처리.사원코드,
                    DEPT_CD = 출고처리.부서코드,
                    DIV_CD = 출고처리.사업장코드,
                    VAT_FG = 출고처리.과세구분,
                    UMVAT_FG = 출고처리.단가구분,
                    APP_FG = 출고처리.연동구분,
                    SHIP_CD = 출고처리.납품처코드,
                    REMARK_DC = "",
                    PLN_CD = "",

                };
                if (isAdd)
                {
                    출고처리.작업번호 = 작업번호1;
                    dc.출고처리헤더정보.Add(출고처리);
                    dc_D.BARPLUS_LDELIVER.Add(BARPLUS_LDELIVER);
                }
                else
                {
                    dc.출고처리헤더정보.Update(출고처리);

                    dc_D.BARPLUS_LDELIVER.Update(BARPLUS_LDELIVER);
                }


            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }



        public Task<bool> MES출고처리_출고처리상세정보_등록(출고처리상세정보 출고처리, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.출고처리상세정보.Count(x => x.회사코드 == 출고처리.회사코드 &&
                                        x.작업번호 == 출고처리.작업번호) + 1;
            //var 순번 = dc.출고처리상세정보.Count() + 1;
            var 순번_R = dc.출고처리상세정보.Where(x => x.회사코드 == 출고처리.회사코드 &&
                                        x.작업번호 == 출고처리.작업번호 && x.작업순번 == 출고처리.작업순번).FirstOrDefault();

            var 순번_더존 = dc_D.BARPLUS_LDELIVER_D.Count(x => x.CO_CD == 출고처리.회사코드 &&
                                        x.WORK_NB == 출고처리.작업번호);


            var 주문서 = dc_D.VL_MES_SO
                           .Where(x => x.CO_CD == 출고처리.회사코드 && x.SO_NB == 출고처리.주문번호 && x.SO_SQ == 출고처리.주문순번)
                           .FirstOrDefault();


            var bResult = dc.출고처리상세정보
                .Where(x => x.회사코드 == 출고처리.회사코드 && x.주문번호 == 출고처리.주문번호 && x.주문순번 == 출고처리.주문순번)
                 .Where_미삭제_사용()
                .Order_등록최신()
                .FirstOrDefault();

            if (bResult != null)
            {
                return Task.FromResult(result);
            }

            var 보유품목 = dc.보유품목정보
                                 .Where(x => x.회사코드 == 출고처리.회사코드 && x.보유품목코드 == 출고처리.품번)
                                 .FirstOrDefault();

            if (보유품목 == null)
            {
                return Task.FromResult(result);
            }
            else
            {
                var su = 보유품목.수량;
                if (출고처리.출고수량_관리단위 > su)
                {
                    return Task.FromResult(result);
                }
            }

            try
            {


                var BARPLUS_LDELIVER_D = new BARPLUS_LDELIVER_D
                {
                    CO_CD = 출고처리.회사코드,
                    WORK_NB = 출고처리.작업번호,
                    WORK_SQ = isAdd == true ? 순번_더존 + 1 : 순번_더존,
                    ITEM_CD = 출고처리.품번,
                    SO_QT = 출고처리.출고수량_관리단위,
                    ISU_QT = 출고처리.출고수량_재고단위,
                    SO_NB = 출고처리.주문번호,
                    SO_SQ = 출고처리.주문순번,
                    LC_CD = 출고처리.장소코드,
                    APP_FG = 출고처리.연동구분,
                    LOT_NB = 출고처리.LOT번호,
                };
                if (isAdd)
                {
                    출고처리.작업순번 = 순번;
                    dc.출고처리상세정보.Add(출고처리);
                    dc_D.BARPLUS_LDELIVER_D.Add(BARPLUS_LDELIVER_D);
                    보유품목.수량 = 보유품목.수량 - 출고처리.출고수량_관리단위;
                    if (보유품목.수량 < 0)
                        보유품목.수량 = 0;
                    dc.보유품목정보.Update(보유품목);
                }
                else
                {
                    string 장소코드 = "";

                    장소코드 = 출고처리.장소코드.Substring(0, 4);
                    var result2 = dc.보유품목정보.Where(x => x.품목코드 == 순번_R.품번 && x.회사코드 == 순번_R.회사코드).FirstOrDefault();

                    var info = new 보유품목정보
                    {
                        회사코드 = 출고처리.회사코드,
                        보유품목코드 = 출고처리.품번,
                        품목코드 = 출고처리.품번,
                        수량 = result2.수량 - 출고처리.출고수량_관리단위,
                        //장소코드 = 출고처리.장소코드,
                        //장소위치코드 = 출고처리.장소코드,

                    };
                    dc.보유품목정보.Update(info);

                    dc.출고처리상세정보.Update(출고처리);

                    dc_D.BARPLUS_LDELIVER_D.Update(BARPLUS_LDELIVER_D);
                }


            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }




        public Task<IEnumerable<발주서헤더정보>> 발주서헤더정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.발주서헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }


        public Task<IEnumerable<발주서정보>> 발주서정보_조회(string 발주번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.발주서정보
                .Where(x => x.회사코드 == 회사코드 && x.발주번호 == 발주번호)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }






        public Task<IEnumerable<주문서정보>> 주문서정보_조회(string 주문번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.주문서정보
                .Where(x => x.회사코드 == 회사코드 && x.주문번호 == 주문번호)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }


        public Task<IEnumerable<주문서헤더정보>> 주문서헤더정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.주문서헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task<bool> 보유품목_출고처리(string 보유품목코드, decimal 수량, string 장소코드, string 위치코드, string 사유)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            try
            {
                var info = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 보유품목코드);
                // 보유품목 등록이 되어 있지 않으면 무시한다.
                if (info == default)
                    return Task.FromResult(result);

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
            catch (Exception)
            {
                result = false;

            }
            // 보유품목 선택

            result = true;

            return Task.FromResult(result);

        }



        public Task<발주서정보> 발주서정보_순번조회(string 발주번호, string 회사코드, decimal 순번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.발주서정보
                .Where(x => x.회사코드 == 회사코드 && x.발주번호 == 발주번호 && x.발주순번 == 순번)
                .Where_미삭제_사용()
                .Order_등록최신()
                .FirstOrDefault();

            return Task.FromResult(list);
        }














        ///////////  4.27 재고이동


        public Task<bool> MES재고이동_재고이동헤더정보_등록(재고이동헤더정보 재고이동, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            //var 출고추가수정유무 = dc.출고처리헤더정보.Count(x => x.작업번호 == 출고처리.작업번호);
            var 순번 = dc.재고이동헤더정보.Count(x => x.회사코드 == 재고이동.회사코드) + 1;


            //var 더존출고추가수정유무 = dc.BARPLUS_LDELIVER.Count(x => x.WORK_NB == 출고처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LSTKMOVE.Count(x => x.CO_CD == 재고이동.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MV"}{yyyy}{순번:000000}";

                var BARPLUS_LSTKMOVE = new BARPLUS_LSTKMOVE
                {
                    CO_CD = 재고이동.회사코드,
                    WORK_NB = 작업번호1,
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

                if (isAdd)
                {
                    재고이동.작업번호 = 작업번호1;
                    dc.재고이동헤더정보.Add(재고이동);
                    dc_D.BARPLUS_LSTKMOVE.Add(BARPLUS_LSTKMOVE);
                }
                else
                {
                    dc.재고이동헤더정보.Update(재고이동);

                    dc_D.BARPLUS_LSTKMOVE.Update(BARPLUS_LSTKMOVE);
                }

                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }



            result = true;

            return Task.FromResult(result);
        }


        public Task<bool> MES재고이동_재고이동상세정보_등록(재고이동상세정보 이동처리, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.재고이동상세정보.Count(x => x.회사코드 == 이동처리.회사코드 &&
                                        x.작업번호 == 이동처리.작업번호) + 1;
            //var 순번 = dc.출고처리상세정보.Count() + 1;
            //var 순번_더존 = dc_D.BARPLUS_LSTKMOVE_D.Count(x => x.CO_CD == 이동처리.회사코드 &&    x.WORK_NB == 이동처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LDELIVER_D.Count() + 1;

            try
            {
                var 재고이동헤더 = dc.재고이동헤더정보.Where(x => x.회사코드 == 이동처리.회사코드 && x.작업번호 == 이동처리.작업번호).FirstOrDefault();
                string 이동할장소위치코드 = $"{재고이동헤더.입고공정_창고코드}{재고이동헤더.입고작업장_장소코드}";
                string 이동할위치상세코드 = 재고이동헤더.입고장소위치상세코드;

                var 보유품목정보 = dc.보유품목정보.Where(x => x.품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드).FirstOrDefault();
                var 현장소위치코드 = 보유품목정보.장소위치코드;

                var 보유품목현위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드 && x.장소위치코드 == 현장소위치코드).FirstOrDefault();

                var 보유품목이동위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드 && x.장소위치코드 == 이동할장소위치코드).FirstOrDefault();

                if (보유품목정보?.수량 < 이동처리.이동수량)
                    return Task.FromResult(result);

                if (보유품목현위치?.수량 < 이동처리.이동수량)
                    return Task.FromResult(result);

                //if (재고이동헤더.이동구분.Equals("5"))
                이동처리.재공운영여부 = "1";

                var BARPLUS_LSTKMOVE_D = new BARPLUS_LSTKMOVE_D
                {
                    CO_CD = 이동처리.회사코드,
                    WORK_NB = 이동처리.작업번호,
                    WORK_SQ = isAdd == true ? 순번 + 1 : 순번,
                    ITEM_CD = 이동처리.품번,
                    REQ_QT = 이동처리.청구수량,
                    MOVE_QT = 이동처리.이동수량,
                    WIP_YN = 이동처리.재공운영여부,
                    APP_FG = 이동처리.APP_FG,
                    USE_YN = 이동처리.사용여부,
                    EXPIRE_YN = 이동처리.만료여부,
                    LOT_NB = 보유품목현위치 != null ? 보유품목현위치.LOT번호 : 이동처리.LOT번호,
                };

                string 이동구분 = "";
                // 재고이동 내부이동
                if (재고이동헤더.이동구분 != "")
                {
                    if (보유품목정보 != null)
                    {
                        //보유품목정보.장소코드 = 재고이동헤더.입고공정_창고코드;
                        //보유품목정보.장소위치코드 = 이동할장소위치코드;
                        //dc.보유품목정보.Update(보유품목정보);
                        //dc.SaveChanges();
                    }
                    if (보유품목현위치 != null)
                    {
                        if (보유품목이동위치 == null)
                        {
                            var info = new 보유품목위치정보
                            {
                                회사코드 = 이동처리.회사코드,
                                보유품목코드 = 이동처리.품번,
                                장소위치코드 = 이동할장소위치코드,
                                //위치상세코드 = 이동할위치상세코드,
                                수량 = 이동처리.이동수량,
                                LOT번호 = 보유품목현위치 != null ? 보유품목현위치.LOT번호 : 이동처리.LOT번호,
                                품목_LOT번호 = 보유품목현위치 != null ? 보유품목현위치.품목_LOT번호 : 이동처리.품목_LOT번호,
                            };
                            dc.보유품목위치정보.Add(info);
                            dc.SaveChanges();

                        }
                        else
                        {
                            보유품목이동위치.회사코드 = 이동처리.회사코드;
                            보유품목이동위치.보유품목코드 = 이동처리.품번;
                            보유품목이동위치.장소위치코드 = 이동할장소위치코드;
                            //보유품목이동위치.위치상세코드 = 이동할위치상세코드;
                            보유품목이동위치.수량 = 보유품목이동위치.수량 + 이동처리.이동수량;
                            보유품목이동위치.LOT번호 = 보유품목현위치 != null ? 보유품목현위치.LOT번호 : 이동처리.LOT번호;
                            보유품목이동위치.품목_LOT번호 = 보유품목현위치 != null ? 보유품목현위치.품목_LOT번호 : 이동처리.품목_LOT번호;

                            dc.보유품목위치정보.Update(보유품목이동위치);
                            dc.SaveChanges();
                        }

                        보유품목현위치.수량 = 보유품목현위치.수량 - 이동처리.이동수량;
                        if (보유품목현위치.수량 == 0)
                            dc.보유품목위치정보.Remove(보유품목현위치);
                        else
                            dc.보유품목위치정보.Update(보유품목현위치);
                        dc.SaveChanges();


                    }
                    if (재고이동헤더.이동구분.Equals("5")) 이동구분 = "재고이동";
                    if (재고이동헤더.이동구분.Equals("0")) 이동구분 = "생산";
                    if (재고이동헤더.이동구분.Equals("1")) 이동구분 = "외주";
                    var 보유품목이력 = new 보유품목이력
                    {
                        회사코드 = 이동처리.회사코드,
                        보유품목코드 = 이동처리.품번,
                        연계보유품목코드 = 이동처리.품번,
                        변경유형코드 = "B1705",    // 입고
                        장소코드 = 재고이동헤더.입고공정_창고코드,
                        장소위치코드 = 이동할장소위치코드,
                        //위치상세코드 = 이동할위치상세코드,
                        변경수량 = 이동처리.이동수량,
                        변경사유 = "재고이동",
                        유형사유 = "B1705",
                        변경일시 = DateTime.Now,
                        LOT번호 = 보유품목현위치 != null ? 보유품목현위치.LOT번호 : 이동처리.LOT번호,
                        품목_LOT번호 = 보유품목현위치 != null ? 보유품목현위치.품목_LOT번호 : 이동처리.품목_LOT번호,
                    };
                    dc.보유품목이력.Add(보유품목이력);
                    dc.SaveChanges();
                }

                /*
                else 
                {
                    if (재고이동헤더.이동구분.Equals("0"))
                        이동구분 = "생산이동";
                    else if (재고이동헤더.이동구분.Equals("1"))
                        이동구분 = "외주이동";
                    if (보유품목정보 != null)
                    {
                        보유품목정보.수량 = 보유품목정보.수량 - 이동처리.이동수량;
                        //보유품목정보.장소코드 = 재고이동헤더.입고공정_창고코드;
                        //보유품목정보.장소위치코드 = 이동할장소위치코드;
                        dc.보유품목정보.Update(보유품목정보);
                        dc.SaveChanges();
                    }
                    if (보유품목현위치 != null)
                    {
                        if (보유품목이동위치 == null)
                        {
                            var info = new 보유품목위치정보
                            {
                                회사코드 = 이동처리.회사코드,
                                보유품목코드 = 이동처리.품번,
                                장소위치코드 = 이동할장소위치코드,
                                수량 = 이동처리.이동수량
                            };
                            dc.보유품목위치정보.Add(info);
                            dc.SaveChanges();

                        }
                        else
                        {
                            보유품목이동위치.회사코드 = 이동처리.회사코드;
                            보유품목이동위치.보유품목코드 = 이동처리.품번;
                            보유품목이동위치.장소위치코드 = 이동할장소위치코드;
                            보유품목이동위치.수량 = 보유품목이동위치.수량 + 이동처리.이동수량;

                            dc.보유품목위치정보.Update(보유품목이동위치);
                            dc.SaveChanges();
                        }

                        보유품목현위치.수량 = 보유품목현위치.수량 - 이동처리.이동수량;
                        dc.보유품목위치정보.Update(보유품목현위치);
                        dc.SaveChanges();


                    }
                    var 보유품목이력 = new 보유품목이력
                    {
                        회사코드 = 이동처리.회사코드,
                        보유품목코드 = 이동처리.품번,
                        연계보유품목코드 = 이동처리.품번,
                        변경유형코드 = "B1701",    // 입고
                        장소코드 = 재고이동헤더.입고공정_창고코드,
                        장소위치코드 = 이동할장소위치코드,
                        변경수량 = 이동처리.이동수량,
                        변경사유 = 이동구분,
                        유형사유 = "B1701",
                        변경일시 = DateTime.Now
                    };
                    dc.보유품목이력.Add(보유품목이력);
                    dc.SaveChanges();
                }
                */

                if (isAdd)
                {
                    //dc.보유품목정보.Add(info);

                    이동처리.작업순번 = 순번;
                    dc.재고이동상세정보.Add(이동처리);
                    dc_D.BARPLUS_LSTKMOVE_D.Add(BARPLUS_LSTKMOVE_D);
                }
                else
                {
                    dc.재고이동상세정보.Update(이동처리);
                    dc_D.BARPLUS_LSTKMOVE_D.Update(BARPLUS_LSTKMOVE_D);
                }


            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }


        public Task<List<재고이동헤더정보>> MES재고이동_재고이동헤더정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.재고이동헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);


        }


        public Task<List<재고이동상세정보>> MES재고이동상세_조회(string 작업번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.재고이동상세정보
                .Where(x => x.회사코드 == 회사코드 && x.작업번호 == 작업번호)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);
        }







        public Task<List<일괄생산실적헤더정보>> MES생산관리_일괄생산실적헤더정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.일괄생산실적헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);


        }


        public Task<List<일괄생산실적상세정보>> MES생산관리_일괄생산실적상세정보_조회(string 작업번호, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.일괄생산실적상세정보
                .Where(x => x.회사코드 == 회사코드 && x.작업번호 == 작업번호)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);
        }


        public Task<bool> MES재고이동_일괄생산실적헤더정보_등록(일괄생산실적헤더정보 생산실적, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            //var 출고추가수정유무 = dc.출고처리헤더정보.Count(x => x.작업번호 == 출고처리.작업번호);
            var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 생산실적.회사코드) + 1;

            //var 더존출고추가수정유무 = dc.BARPLUS_LDELIVER.Count(x => x.WORK_NB == 출고처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LPRODUCTION.Count(x => x.CO_CD == 생산실적.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";
                //작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";

                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 생산실적.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적.작업일자.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적.실적일자.ToString())),
                    DIV_CD = 생산실적.사업장코드,
                    DEPT_CD = 생산실적.부서코드,
                    EMP_CD = 생산실적.사원코드,
                    BASELOC_CD = 생산실적.실적공정코드_창고코드,
                    LOC_CD = 생산실적.실적작업장코드_장소코드,
                    REWORK_YN = 생산실적.재작업여부,
                    PITEM_CD = 생산실적.실적품번,
                    ITEM_QT = 생산실적.실적수량,

                    REMARK_DC = "",
                    BASELOC_FG = 생산실적.실적구분,
                    WR_WH_CD = 생산실적.실적공정코드,
                    WR_LC_CD = 생산실적.실적작업장코드,
                    PLN_CD = "",
                };

                if (isAdd)
                {
                    생산실적.작업번호 = 작업번호1;
                    dc.일괄생산실적헤더정보.Add(생산실적);
                    dc_D.BARPLUS_LPRODUCTION.Add(BARPLUS_LPRODUCTION);
                }
                else
                {
                    dc.일괄생산실적헤더정보.Update(생산실적);
                    dc_D.BARPLUS_LPRODUCTION.Update(BARPLUS_LPRODUCTION);
                }


            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }


        public Task<bool> MES재고이동_일괄생산실적상세정보_등록(일괄생산실적상세정보 생산실적상세, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 생산실적상세.회사코드 &&
                                        x.작업번호 == 생산실적상세.작업번호) + 1;
            //var 순번 = dc.출고처리상세정보.Count() + 1;


            var 순번_더존 = dc_D.BARPLUS_LPRODUCTION_D.Count(x => x.CO_CD == 생산실적상세.회사코드 &&
                                        x.WORK_NB == 생산실적상세.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LDELIVER_D.Count() + 1;

            try
            {
                var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                {
                    CO_CD = 생산실적상세.회사코드,
                    WORK_NB = 생산실적상세.작업번호,
                    WORK_SQ = isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                    CITEM_CD = 생산실적상세.사용품번,
                    BASELOC_FG = 생산실적상세.창고구분,
                    LOT_NB = 생산실적상세.LOTNO,
                    BASELOC_CD = 생산실적상세.사용공정_사용창고,
                    LOC_CD = 생산실적상세.사용작업장_사용장소,

                };
                if (isAdd)
                {
                    생산실적상세.작업순번 = 순번.ToString();
                    dc.일괄생산실적상세정보.Add(생산실적상세);
                    dc_D.BARPLUS_LPRODUCTION_D.Add(BARPLUS_LPRODUCTION_D);
                }
                else
                {
                    dc.일괄생산실적상세정보.Update(생산실적상세);

                    dc_D.BARPLUS_LPRODUCTION_D.Update(BARPLUS_LPRODUCTION_D);
                }


            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }


        public Task<IEnumerable<보유품목정보>> 품목장소위치Popup_조회(string 장소위치코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목정보
                    .Include(x => x.품목)
                    .Include(x => x.장소)
                    .Include(x => x.장소위치)
                .Where_미삭제_사용()
                .Where(x => x.회사코드 == "9998" && x.장소위치코드 == 장소위치코드)     // 설비는 제외
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }



        public Task<bool> MES생산실적_작업외주생산실적등록정보_등록(작업외주생산실적등록정보 생산실적, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            //var 출고추가수정유무 = dc.출고처리헤더정보.Count(x => x.작업번호 == 출고처리.작업번호);
            var 순번 = dc.작업외주생산실적등록정보.Count(x => x.회사코드 == 생산실적.회사코드) + 1;

            //var 더존출고추가수정유무 = dc.BARPLUS_LDELIVER.Count(x => x.WORK_NB == 출고처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LPRODUCTION.Count(x => x.CO_CD == 생산실적.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yymm = now.ToString("yyMM");
                string 작업번호1 = "";
                //작업번호1 = $"{"WR"}{yymm}{순번:000000}";
                작업번호1 = $"{"WO"}{yymm}{순번:000000}";
                //작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";

                var BARPLUS_LORCV_H = new BARPLUS_LORCV_H
                {
                    CO_CD = 생산실적.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적.작업일자.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적.실적일자.ToString())),
                    DIV_CD = 생산실적.사업장코드,
                    DEPT_CD = 생산실적.부서코드,
                    EMP_CD = 생산실적.사원코드,
                    MOVEBASELOC_CD = 생산실적.이동공정_입고창고코드,
                    MOVELOC_CD = 생산실적.이동작업장_입고장소코드,
                    REWORK_YN = 생산실적.재작업여부,
                    ITEM_QT = 생산실적.실적수량,
                    BAD_YN = 생산실적.실적구분,
                    WF_FG = 생산실적.처리구분,
                    WO_CD = 생산실적.지시번호,
                    WOOP_SQ = 생산실적.지시전개순번,

                    BASELOC_CD = 생산실적.실적공정코드,
                    LOC_CD = 생산실적.실적작업장코드,
                    LOT_NB = 생산실적.LOT번호,
                    EQUIP_CD = 생산실적.설비코드,
                };

                if (isAdd)
                {
                    생산실적.작업번호 = 작업번호1;
                    dc.작업외주생산실적등록정보.Add(생산실적);
                    dc_D.BARPLUS_LORCV_H.Add(BARPLUS_LORCV_H);
                }
                else
                {
                    dc.작업외주생산실적등록정보.Update(생산실적);
                    dc_D.BARPLUS_LORCV_H.Update(BARPLUS_LORCV_H);
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }
            try
            {
                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }



        public Task<List<작업외주생산실적등록정보>> MES생산관리_작업외주생산실적등록정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.작업외주생산실적등록정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);

        }

        public Task<List<사용자재보고정보>> MES생산관리_사용자재보고정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.사용자재보고정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);

        }


        public Task<IEnumerable<발주서헤더정보>> VL_MES_PO_View(string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            List<VL_MES_PO> list = new List<VL_MES_PO>();

            list = dcDz.VL_MES_PO.Select(x => x)
                .ToList();

            List<VL_MES_PO> listDz = new List<VL_MES_PO>();
            List<발주서헤더정보> 발주서헤더정보List = new List<발주서헤더정보>();

            listDz = list.Where(x => x.CO_CD == 회사코드).GroupBy(x => new { x.PO_NB }).Select(g => g.First()).ToList();

            // 발주서헤더정보
            foreach (var item in listDz)
            {
                var 발주서헤더정보 = new 발주서헤더정보
                {
                    회사코드 = item.CO_CD,
                    발주번호 = item.PO_NB,
                    발주일 = item.PO_DT,
                    거래처명 = item.TR_NM,
                    과세구분명 = item.VAT_NM,
                    담당자명 = item.PLN_NM,
                    과세구분 = item.VAT_FG,
                    거래구분 = item.PO_FG,
                    거래처코드 = item.TR_CD,

                };

                발주서헤더정보List.Add(발주서헤더정보);
            }

            var 발주서헤더 = 발주서헤더정보List.OrderByDescending(x => x.CreateTime).ToList();

            return Task.FromResult(발주서헤더.AsEnumerable());
        }

        public Task<IEnumerable<발주서정보>> 발주서정보_조회Dz(string 발주번호, string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var list = dcDz.VL_MES_PO
                .Where(x => x.CO_CD == 회사코드 && x.PO_NB == 발주번호)
                .ToList();

            List<발주서정보> 발주서정보List = new List<발주서정보>();
            //발주서정보
            foreach (var item in list)
            {

                var 발주서정보 = new 발주서정보
                {
                    회사코드 = item.CO_CD,
                    사업장코드 = item.DIV_CD,
                    부서코드 = item.DEPT_CD,
                    사원코드 = item.EMP_CD,
                    발주번호 = item.PO_NB,
                    발주일 = item.PO_DT,
                    거래처코드 = item.TR_CD,
                    거래처명 = item.TR_NM,
                    거래구분 = item.PO_FG,
                    검사구분 = item.QC_FG,
                    과세구분 = item.VAT_FG,
                    과세구분명 = item.VAT_NM,
                    담당자코드 = item.PLN_CD,
                    담당자명 = item.PLN_NM,
                    비고 = item.REMARK_DC,
                    발주순번 = item.PO_SQ,
                    품번 = item.ITEM_CD,
                    품명 = item.ITEM_NM,
                    규격 = item.ITEM_DC,
                    관리단위 = item.UNITMANG_DC,
                    납기일 = item.DUE_DT,
                    출하예정일 = item.SHIPREQ_DT,
                    발주수량 = item.PO_QT,
                    발주단가 = item.PO_UM,
                    공급가 = item.POG_AM,
                    부가세 = item.POGV_AM1,
                    합계액 = item.POGH_AM1,
                    관리구분코드 = item.MGMT_CD,
                    관리구분명 = item.MGM_NM,
                    프로젝트 = item.PJT_CD,
                    프록젝트명 = item.PJT_NM,
                    비고_내역 = item.REMARK_DC_D,
                    환종 = item.EXCH_CD,
                    부가세구분 = item.UMVAT_FG,

                };

                발주서정보List.Add(발주서정보);
            }


            return Task.FromResult(발주서정보List.AsEnumerable());
        }

        public Task<IEnumerable<주문서헤더정보>> VL_MES_SO_View(string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            List<VL_MES_SO> list = new List<VL_MES_SO>();

            list = dcDz.VL_MES_SO.Select(x => x)
                .ToList();

            List<VL_MES_SO> listDz = new List<VL_MES_SO>();
            List<주문서헤더정보> 주문서헤더정보List = new List<주문서헤더정보>();

            listDz = list.Where(x => x.CO_CD == 회사코드).GroupBy(x => new { x.SO_NB }).Select(g => g.First()).ToList();

            // 발주서헤더정보
            foreach (var item in listDz)
            {
                var 주문서헤더정보 = new 주문서헤더정보
                {
                    회사코드 = item.CO_CD,
                    주문번호 = item.SO_NB,
                    주문일자 = item.SO_DT,
                    고객명 = item.TR_NM,
                    과세구분명 = item.VAT_NM,
                    납품처명 = item.SHIP_NM,
                    담당자명 = item.PLN_NM,
                    거래처코드 = item.TR_CD,
                    과세구분 = item.VAT_FG,
                    주문구분 = item.SO_FG,
                };

                주문서헤더정보List.Add(주문서헤더정보);
            }

            var 주문서헤더 = 주문서헤더정보List.OrderByDescending(x => x.CreateTime).ToList();

            return Task.FromResult(주문서헤더.AsEnumerable());
        }

        public Task<IEnumerable<주문서정보>> 주문서정보_조회Dz(string 주문번호, string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var list = dcDz.VL_MES_SO
                .Where(x => x.CO_CD == 회사코드 && x.SO_NB == 주문번호)
                .ToList();

            List<주문서정보> 주문서정보List = new List<주문서정보>();
            //발주서정보
            foreach (var item in list)
            {

                var 주문서정보 = new 주문서정보
                {
                    회사코드 = item.CO_CD,
                    사업장코드 = item.DIV_CD,
                    부서코드 = item.DEPT_CD,
                    사원코드 = item.EMP_CD,
                    주문번호 = item.SO_NB,
                    주문일자 = item.SO_DT,
                    고객코드 = item.TR_CD,
                    고객명 = item.TR_NM,
                    주문구분 = item.SO_FG,
                    과세구분 = item.VAT_FG,
                    과세구분명 = item.VAT_NM,
                    단가구분 = item.UMVAT_FG,
                    단가구분명 = item.UMVAT_NM,
                    납품처코드 = item.SHIP_CD,
                    납품처명 = item.SHIP_NM,
                    담당자코드 = item.PLN_CD,
                    담당자명 = item.PLN_NM,
                    관리번호 = item.DUMMY1,
                    헤더비고 = item.REMARK_DC,
                    순번 = item.SO_SQ,
                    품목코드 = item.ITEM_CD,
                    품목명 = item.ITEM_NM,
                    규격 = item.ITEM_DC,
                    관리단위 = item.UNITMANG_DC,
                    납기일 = item.DUE_DT,
                    출하예정일 = item.SHIPREQ_DT,
                    수량 = item.SO_QT,
                    단가 = item.SO_UM,
                    부가세단가 = item.VAT_UM,
                    공급가 = item.SOG_AM,
                    SOV_AM = item.SOV_AM,
                    합계액 = item.SOH_AM,
                    관리구분코드 = item.MGMT_CD,
                    관리구분명 = item.MGM_NM,
                    프로젝트코드 = item.PJT_CD,
                    프로젝트명 = item.PJT_NM,
                    디테일비고 = item.REMARK_DC_D,
                    마감여부 = item.EXPIRE_YN,
                    검사구분 = item.QC_FG,
                    환종 = item.EXCH_CD,

                };

                주문서정보List.Add(주문서정보);
            }


            return Task.FromResult(주문서정보List.AsEnumerable());
        }

        public Task<bool> MES출고처리_출고처리헤더정보_등록_PDA(출고처리헤더정보 출고처리, BARPLUS_LDELIVER 출고처리2)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;
            bool result = false;

            var 순번_더존 = dc_D.BARPLUS_LDELIVER.Count(x => x.CO_CD == 출고처리.회사코드) + 1;
            try
            {

                dc.출고처리헤더정보.Add(출고처리);

                dc_D.BARPLUS_LDELIVER.Add(출고처리2);



            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            try
            {
                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            result = true;

            return Task.FromResult(result);
        }



        public Task<IEnumerable<외주작업지시헤더정보>> VL_MES_WO_WF_View(string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            List<VL_MES_WO_WF> list = new List<VL_MES_WO_WF>();

            list = dcDz.VL_MES_WO_WF.Select(x => x)
                .ToList();

            List<VL_MES_WO_WF> listDz = new List<VL_MES_WO_WF>();
            List<외주작업지시헤더정보> 외주작업지시헤더정보List = new List<외주작업지시헤더정보>();

            listDz = list.Where(x => x.CO_CD == 회사코드).GroupBy(x => new { x.WO_CD }).Select(g => g.First()).ToList();

            // 발주서헤더정보
            foreach (var item in listDz)
            {
                var 외주작업지시헤더 = new 외주작업지시헤더정보
                {
                    회사코드 = item.CO_CD,
                    지시번호 = item.WO_CD,
                    품번 = item.ITEM_CD,
                    품명 = item.ITEM_NM,
                    전개순번 = item.WOOP_SQ,
                    공정 = item.BASELOC_CD,
                    공정명 = item.BASELOC_NM,
                    지시구분 = item.WOC_FG,
                    지시구분명 = item.WOC_FG_NM,
                    생산외주구분 = item.DOC_FG,
                    생산외주구분명 = item.DOC_FG_NM,
                    수량 = item.ITEM_QT,

                };

                외주작업지시헤더정보List.Add(외주작업지시헤더);
            }

            return Task.FromResult(외주작업지시헤더정보List.AsEnumerable());
        }


        public Task<IEnumerable<외주작업지시서정보>> 외주작업지시서정보_조회Dz(string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var list = dcDz.VL_MES_WO_WF
                .Where(x => x.CO_CD == 회사코드)
                .ToList();

            List<외주작업지시서정보> 외주작업지시서정보List = new List<외주작업지시서정보>();
            //발주서정보
            foreach (var item in list)
            {

                var 외주작업지시서 = new 외주작업지시서정보
                {
                    회사코드 = item.CO_CD,
                    지시번호 = item.WO_CD,
                    지시일 = DateTime.ParseExact(item.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    완료일 = DateTime.ParseExact(item.COMP_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    품번 = item.ITEM_CD,
                    품명 = item.ITEM_NM,
                    규격 = item.ITEM_DC,
                    관리단위 = item.UNIT_DC,
                    수량 = item.ITEM_QT,
                    전개순번 = item.WOOP_SQ,
                    공정 = item.BASELOC_CD,
                    공정명 = item.BASELOC_NM,
                    작업장 = item.LOC_CD,
                    작업장명 = item.LOC_NM,
                    외주단가 = item.LBR_UM,
                    외주금액 = item.LBR_AM,
                    설비코드 = item.EQUIP_CD,
                    설비명 = item.EQUIP_NM,
                    비고_DOC_DC = item.DOC_DC,
                    지시상태 = item.DOC_ST,
                    지시상태명 = item.DOC_ST_NM,
                    지시구분 = item.WOC_FG,
                    지시구분명 = item.WOC_FG_NM,
                    생산외주구분 = item.DOC_FG,
                    생산외주구분명 = item.DOC_FG_NM,
                    처리구분 = item.WF_FG,
                    처리구분명 = item.WF_FG_NM,
                    검사구분 = item.QC_FG,
                    검사구분명 = item.QC_FG_NM,
                    LOT번호 = item.LOT_NB,
                    거래처코드 = item.TR_CD,
                    거래처명 = item.TR_NM,
                    거래처약칭 = item.ATTR_NM,
                    주문번호 = item.SO_NB,
                    주문순번 = item.LN_SQ,
                    사업장코드 = item.DIV_CD,
                    작업팀 = item.WTEAM_CD,
                    작업팀명 = item.WTEAM_NM,
                    작업조 = item.WSHFT_CD,
                    작업조명 = item.WSHFT_NM,
                    비고 = item.REMARK_DC,

                };

                외주작업지시서정보List.Add(외주작업지시서);
            }

            var 외주작업지시 = 외주작업지시서정보List.OrderByDescending(x => x.CreateTime).ToList();

            return Task.FromResult(외주작업지시.AsEnumerable());
        }


        public Task<bool> MES생산실적_사용자재보고정보_등록(사용자재보고정보 자재보고, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 생산실적 = dc_D.BARPLUS_LORCV_H.Where(x => x.WO_CD == 자재보고.지시번호 && x.WOOP_SQ == 자재보고.지시전개순번).FirstOrDefault();

            var 순번 = dc.사용자재보고정보.Count(x => x.회사코드 == 자재보고.회사코드 && x.작업번호 == 생산실적.WORK_NB) + 1;


            try
            {
                var now = DateTime.Now;
                var yymm = now.ToString("yyMM");
                //string 작업번호1 = "";
                //작업번호1 = $"{"WO"}{yymm}{순번:000000}";
                //작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";

                var BARPLUS_LMTL_USE = new BARPLUS_LMTL_USE
                {
                    CO_CD = 자재보고.회사코드,
                    DIV_CD = 자재보고.사업장코드,
                    WORK_NB = 생산실적.WORK_NB,
                    WORK_SQ = 순번,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(자재보고.작업일자.ToString())),
                    ITEM_CD = 자재보고.품번,
                    BASELOC_CD = 자재보고.사용공정,
                    LOC_CD = 자재보고.사용작업장,
                    WO_CD = 자재보고.지시번호,
                    WOC_FG = 자재보고.지시구분,
                    EMP_CD = 자재보고.사원코드,
                    DEPT_CD = 자재보고.부서코드,
                    USE_YN = 자재보고.사용여부,
                    UMU_FG = 자재보고.유무상구분,
                    EXPIRE_YN = 자재보고.유효여부,
                    WOOP_SQ = 자재보고.지시전개순번,

                    LOT_NB = 생산실적.LOT_NB,
                    WR_CD = 생산실적.DOC_CD,
                    USE_SQ = 1,
                    USE_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(자재보고.작업일자.ToString())),
                    USE_QT = 10,
                    WOBOM_SQ = 1,


                };

                if (isAdd)
                {
                    자재보고.작업순번 = 순번;
                    자재보고.LOT번호 = 생산실적.LOT_NB;
                    자재보고.작업번호 = 생산실적.WORK_NB;
                    dc.사용자재보고정보.Add(자재보고);
                    dc_D.BARPLUS_LMTL_USE.Add(BARPLUS_LMTL_USE);
                }
                else
                {
                    dc.사용자재보고정보.Update(자재보고);
                    dc_D.BARPLUS_LMTL_USE.Update(BARPLUS_LMTL_USE);
                }

                dc.SaveChanges();
                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            result = true;

            return Task.FromResult(result);
        }



        public Task<bool> MES입고처리_NEW입고처리상세정보_등록(입고처리상세정보 입고상세처리, BARPLUS_LSTOCK_D BLD, string 창고코드, string 위치코드, DateTime 보유년월일)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 입고상세추가수정유무 = dc.입고처리상세정보.Count(x => x.회사코드 == 입고상세처리.회사코드 && x.작업번호 == 입고상세처리.작업번호 && x.작업순번 == 입고상세처리.작업순번);
            var 작업순번 = dc.입고처리상세정보.Count(x => x.회사코드 == 입고상세처리.회사코드 && x.작업번호 == 입고상세처리.작업번호) + 1;

            var 순번_R = dc.입고처리상세정보.Where(x => x.회사코드 == 입고상세처리.회사코드 && x.작업번호 == 입고상세처리.작업번호 && x.작업순번 == 입고상세처리.작업순번).FirstOrDefault();

            var 발주서 = dc_D.VL_MES_PO
                       .Where(x => x.CO_CD == 입고상세처리.회사코드 && x.PO_NB == 입고상세처리.발주번호 && x.PO_SQ == 입고상세처리.발주순번)
                       .FirstOrDefault();


            try
            {
                if (입고상세추가수정유무 == 0)
                {
                    BLD.PJT_CD = 발주서.PJT_CD;
                    BLD.EXCH_CD = 발주서.EXCH_CD;
                    BLD.EXCH_RT = 1;
                    BLD.EXPIRE_YN = "Y";
                    BLD.USE_YN = "0";
                    BLD.CONF_NB3 = 0;
                    BLD.WORK_SQ = 작업순번;

                    입고상세처리.작업순번 = 작업순번;
                    dc.입고처리상세정보.Add(입고상세처리);
                    dc_D.BARPLUS_LSTOCK_D.Add(BLD);

                    var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == 입고상세처리.품번 && x.회사코드 == 입고상세처리.회사코드).FirstOrDefault();
                    var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 입고상세처리.품번).FirstOrDefault();
                    if (보유품목 == null)
                    {


                        var info = new 보유품목정보
                        {
                            회사코드 = 입고상세처리.회사코드,
                            보유품목코드 = 입고상세처리.품번,
                            품목코드 = 입고상세처리.품번,
                            보유년월일 = 보유년월일.ToString("yyyyMMdd"),
                            조정년월일 = null,
                            보유일 = 보유년월일,
                            순번 = 1,
                            수량 = 입고상세처리.입고수량_관리단위,
                            품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                            장소코드 = 창고코드,
                            장소위치코드 = $"{창고코드}{입고상세처리.입고장소코드}",
                        };

                        dc.보유품목정보.Add(info);
                    }
                    else
                    {

                        보유품목.보유년월일 = 보유년월일.ToString("yyyyMMdd");
                        보유품목.수량 = 보유품목.수량 + 입고상세처리.입고수량_관리단위;
                        dc.보유품목정보.Update(보유품목);

                    }
                }
                else
                {

                    var result2 = dc.보유품목정보.Where(x => x.품목코드 == 순번_R.품번 && x.회사코드 == 순번_R.회사코드).FirstOrDefault();

                    var info = new 보유품목정보
                    {
                        회사코드 = 입고상세처리.회사코드,
                        보유품목코드 = 입고상세처리.품번,
                        품목코드 = 입고상세처리.품번,
                        수량 = result2 != null ? result2.수량 - (순번_R.입고수량_관리단위 - 입고상세처리.입고수량_관리단위) : 입고상세처리.입고수량_관리단위,
                        보유년월일 = 보유년월일.ToString("yyyyMMdd"),
                        //조정년월일 = null,//result.조정년월일,
                        //보유일 = now,
                        //품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                        장소코드 = 창고코드,
                        장소위치코드 = $"{창고코드}{입고상세처리.입고장소코드}",
                    };

                    dc.보유품목정보.Update(info);

                    dc.입고처리상세정보.Update(입고상세처리);

                    dc_D.BARPLUS_LSTOCK_D.Update(BLD);
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }



        public Task<List<생산실적헤더정보>> MES생산관리_생산실적헤더정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산실적헤더정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .OrderBy(x => x.CreateTime)
                .ToList();

            return Task.FromResult(result);

        }


        public Task<List<생산실적상세정보>> MES생산관리_생산실적상세정보_조회(string 생산지시코드, string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산실적상세정보
                .Where(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);
        }





        public Task<bool> MES재고이동_생산실적헤더정보_등록(생산실적헤더정보 생산실적, 일괄생산실적헤더정보 일괄생산실적헤더, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적헤더정보.Count(x => x.회사코드 == 생산실적.회사코드) + 1;


            try
            {
                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");
                string 작업번호1 = "";
                작업번호1 = $"{"MF"}{yyyy}{순번:000000}";

                var BARPLUS_LPRODUCTION = new BARPLUS_LPRODUCTION
                {
                    CO_CD = 생산실적.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(일괄생산실적헤더.작업일자.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(일괄생산실적헤더.실적일자.ToString())),
                    DIV_CD = 생산실적.사업장코드,
                    DEPT_CD = 일괄생산실적헤더.부서코드,
                    EMP_CD = 일괄생산실적헤더.사원코드,
                    BASELOC_CD = 생산실적.실적공정코드_창고코드,
                    LOC_CD = 생산실적.실적작업장코드_장소코드,
                    REWORK_YN = 생산실적.재작업여부,
                    PITEM_CD = 생산실적.생산품코드,
                    ITEM_QT = 일괄생산실적헤더.실적수량,
                    BASELOC_FG = "1",
                    WR_WH_CD = "",
                    WR_LC_CD = "",
                    PLN_CD = "",
                    REMARK_DC = "",
                    LOT_NB = 일괄생산실적헤더.LOTNO,
                };

                var 일괄생산실적헤더정보 = new 일괄생산실적헤더정보
                {
                    회사코드 = 생산실적.회사코드,
                    작업번호 = 작업번호1,
                    작업일자 = 일괄생산실적헤더.작업일자,
                    실적일자 = 일괄생산실적헤더.실적일자,
                    사업장코드 = 생산실적.사업장코드,
                    부서코드 = 일괄생산실적헤더.부서코드,
                    사원코드 = 일괄생산실적헤더.사원코드,
                    실적공정코드_창고코드 = 생산실적.실적공정코드_창고코드,
                    실적작업장코드_장소코드 = 생산실적.실적작업장코드_장소코드,
                    재작업여부 = 생산실적.재작업여부,
                    실적품번 = 생산실적.생산품코드,
                    실적수량 = 일괄생산실적헤더.실적수량,
                    실적구분 = "1",
                    실적공정코드 = "",
                    실적작업장코드 = "",
                    작업자코드 = "",
                    비고 = "",
                    LOTNO = 일괄생산실적헤더.LOTNO,

                };

                var 생산실적헤더 = dc.생산실적헤더정보.Where(x => x.회사코드 == 생산실적.회사코드 && x.생산지시코드 == 생산실적.생산지시코드).FirstOrDefault();
                if (생산실적헤더 != null)
                {
                    생산실적헤더.일괄생산등록유무 = "1";
                    생산실적헤더.작업번호 = 작업번호1;
                }


                dc.생산실적헤더정보.Update(생산실적헤더);

                dc.Add(일괄생산실적헤더정보);
                dc_D.Add(BARPLUS_LPRODUCTION);

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }


        public Task<bool> MES재고이동_생산실적상세정보_등록(생산실적상세정보 생산실적상세, 일괄생산실적상세정보 일괄생산상세, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.일괄생산실적상세정보.Count(x => x.회사코드 == 생산실적상세.회사코드 && x.작업번호 == 일괄생산상세.작업번호) + 1;

            try
            {

                var BARPLUS_LPRODUCTION_D = new BARPLUS_LPRODUCTION_D
                {
                    CO_CD = 생산실적상세.회사코드,
                    WORK_NB = 일괄생산상세.작업번호,
                    WORK_SQ = 생산실적상세.작업순번.ToString(),  //isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                    CITEM_CD = 생산실적상세.사용품번,
                    BASELOC_FG = 일괄생산상세.창고구분,
                    USE_QT = 일괄생산상세.사용수량,
                    BASELOC_CD = 일괄생산상세.사용공정_사용창고,
                    LOC_CD = 일괄생산상세.사용작업장_사용장소,
                    LOT_NB = 일괄생산상세.LOTNO,

                };

                var 일괄생산실적상세정보 = new 일괄생산실적상세정보
                {
                    회사코드 = 생산실적상세.회사코드,
                    작업번호 = 일괄생산상세.작업번호,
                    작업순번 = 생산실적상세.작업순번.ToString(),//isAdd == true ? (순번 + 1).ToString() : 순번.ToString(),
                    사용품번 = 생산실적상세.사용품번,
                    사용수량 = 일괄생산상세.사용수량,


                    LOTNO = 일괄생산상세.LOTNO,
                    사용공정_사용창고 = 일괄생산상세.사용공정_사용창고,
                    사용작업장_사용장소 = 일괄생산상세.사용작업장_사용장소,
                    창고구분 = 일괄생산상세.창고구분,

                };

                var 생산실적상세정보 = dc.생산실적상세정보.Where(x => x.회사코드 == 생산실적상세.회사코드 && x.생산지시코드 == 생산실적상세.생산지시코드 &&
                x.작업순번 == 생산실적상세.작업순번).FirstOrDefault();
                if (생산실적상세정보 != null)
                {
                    생산실적상세정보.일괄생산등록유무 = "1";
                }

                dc.생산실적상세정보.Update(생산실적상세정보);


                dc_D.Add(BARPLUS_LPRODUCTION_D);
                dc.Add(일괄생산실적상세정보);

                // 보유품목등록
                // LOT번호 부여
                // 위치등록
                // 보유품목이력


            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }


        public Task 위치상세정보_저장(위치상세정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            //info.장소 = null;

            if (isAdd == true)
            {
                var 위치상세유무 = dc.위치상세정보.Where(x => x.위치명 == info.위치명).FirstOrDefault();

                if (위치상세유무 == null)
                {
                    var 상세코드 = $"{dc.위치상세정보.Count(x => x.장소위치코드 == info.장소위치코드) + 1:0000}";
                    var 위치코드상세 = $"{info.장소위치코드}{상세코드}";

                    info.상세코드 = 상세코드;
                    info.위치상세코드 = 위치코드상세;

                    dc.위치상세정보.Add(info);

                }
            }
            else
            {
                var 위치상세유무 = dc.위치상세정보.Where(x => x.위치상세코드 == info.위치상세코드).FirstOrDefault();

                if (위치상세유무 != null)
                {

                    dc.위치상세정보.Update(info);

                }
            }

            dc.SaveChanges();

            return Task.CompletedTask;
        }



        public Task<IEnumerable<장소위치정보>> 장소위치2_조회(string 회사코드, string 장소코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.장소위치정보
                .Include(x => x.장소).Where(x => x.회사코드 == 회사코드)
                .Where(x => x.회사코드 == 회사코드 && x.장소코드 == 장소코드)
                .Where_미삭제()
                .OrderBy(x => x.장소위치코드)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }

        public Task<IEnumerable<위치상세정보>> 위치상세정보_조회(string 회사코드, string 장소위치코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.위치상세정보
                .Where(x => x.회사코드 == 회사코드 && x.장소위치코드 == 장소위치코드)
                .Where_미삭제()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }



        //2021.05.11
        // 바코드발급조회 {{{
        public Task<IEnumerable<바코드발급정보>> 바코드발급정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.바코드발급정보
                .Where(x => x.회사코드 == 회사코드)
                //.Where(x => (생산품만 == true && x.품목구분코드 == "B1203") || (생산품만 == false && (x.품목구분코드 == "B1203" || x.품목구분코드 == "B1204"))) // 생산품, 반제품
                .OrderByDescending(x => x.생성일자)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }



        public Task<IEnumerable<위치상세정보>> 위치상세전체정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.위치상세정보
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제()
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }



        public Task<IEnumerable<보유품목위치정보>> 보유품목위치정보Popup_조회(string 회사코드, string 위치상세코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목위치정보
                    .Include(x => x.장소위치)
                    .Include(x => x.보유품목).ThenInclude(x => x.품목)
                .Where_미삭제_사용()
                .Where(x => x.회사코드 == 회사코드 && x.위치상세코드 == 위치상세코드)     // 설비는 제외
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<BOM_정보>> 공정단위BOM_정보_조회(string 회사코드, string 품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.BOM_정보
                   .Where(x => x.모품번 == 품목코드 && x.회사코드 == 회사코드)
                   .Where_미삭제_사용()
                   .Order_등록최신()
                   .ToList();

            return Task.FromResult(result.AsEnumerable());
        }

        public Task<List<공정단위자재정보>> 공정단위자재정보_조회(string 회사코드, string 공정단위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정단위자재정보
                .Where(x => x.회사코드 == 회사코드 && x.공정단위코드 == 공정단위코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);

        }

        public Task<List<공정단위자재현황>> 공정단위자재현황_조회(string 회사코드, string 공정단위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정단위자재정보
                .Where(x => x.회사코드 == 회사코드 && x.공정단위코드 == 공정단위코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            List<공정단위자재현황> list = new List<공정단위자재현황>();
            foreach (var item in result)
            {
                var 공정단위자재현황 = new 공정단위자재현황
                {
                    회사코드 = item.회사코드,
                    공정단위코드 = item.공정단위코드,
                    자재코드 = item.자재코드,
                    필요수량 = item.수량,
                };
                list.Add(공정단위자재현황);
            }


            return Task.FromResult(list);

        }


        // 20210516
        public Task<List<작업자생산실적정보>> MES생산관리_작업자생산실적정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.작업자생산실적정보
                .Include(x => x.작업자).ThenInclude(x => x.부서)
                .Where(x => x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();


            var result2 = result.Where(x => x.회사코드 == 회사코드).GroupBy(x => new { x.작업자사번 }).Select(g => g.First()).ToList();

            return Task.FromResult(result2);

        }

        public Task<List<작업자생산실적정보>> MES생산관리_작업자생산실적상세정보_조회(string 생산지시코드, string 회사코드, string 작업자사번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.작업자생산실적정보
                .Where(x => x.회사코드 == 회사코드 && x.작업자사번 == 작업자사번)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();


            return Task.FromResult(result);
        }



        public Task<IEnumerable<바코드발급정보>> 품목별바코드발급정보_조회(string 회사코드, string 품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.바코드발급정보
                .Where(x => x.회사코드 == 회사코드 && x.품목코드 == 품목코드)
                //.Where(x => (생산품만 == true && x.품목구분코드 == "B1203") || (생산품만 == false && (x.품목구분코드 == "B1203" || x.품목구분코드 == "B1204"))) // 생산품, 반제품
                .OrderByDescending(x => x.생성일자)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }



        public Task<bool> 생산실적헤더LOT번호_등록(생산실적헤더정보 생산실적)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            bool result = false;

            try
            {
                var 생산실적헤더 = dc.생산실적헤더정보.Where(x => x.회사코드 == 생산실적.회사코드 && x.생산지시코드 == 생산실적.생산지시코드).FirstOrDefault();

                if (생산실적헤더 != null)
                {
                    생산실적헤더.LOTNO = 생산실적.LOTNO;
                    dc.생산실적헤더정보.Update(생산실적헤더);
                }
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();


            result = true;

            return Task.FromResult(result);
        }


        public Task 재고조정품목_저장(재고조정품목정보 info, bool isAdd)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            // track 방지 {{{
            info.품목구분 = null;
            info.품목유형 = null;
            info.소재 = null;
            info.규격종류 = null;
            info.조달구분 = null;
            info.단위 = null;
            if (info.거래처?.거래처코드 != null)
            {
                info.거래처코드 = info.거래처?.거래처코드;
                info.거래처 = null;
            }
            // }}}

            if (isAdd == true)
            {
                //info.품목코드 = $"{info.원품목코드}:{info.관리차수}";
                info.품목코드 = info.원품목코드;
                dc.재고조정품목정보.Add(info);
            }
            else
                dc.재고조정품목정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;
        }


        public Task<IEnumerable<재고조정품목정보>> 재고조정품목_조회()
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dc.재고조정품목정보
                .Include(x => x.거래처)
                .Where(x => x.사용유무 == true)
                //.Where(x => (isOnlyUse == true && x.사용유무 == true) || isOnlyUse == false)
                .Where_미삭제()
                //.Where(x => (생산품만 == true && x.품목구분코드 == "B1203") || (생산품만 == false && (x.품목구분코드 == "B1203" || x.품목구분코드 == "B1204"))) // 생산품, 반제품
                .OrderByDescending(x => x.CreateTime)
                .ToList();

            return Task.FromResult(list.AsEnumerable());
        }


        public Task 재고조정품목_삭제(string 품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;


            var info = dc.재고조정품목정보.Where(x => x.품목코드 == 품목코드).FirstOrDefault();

            info.사용유무 = false;
            dc.재고조정품목정보.Update(info);

            dc.SaveChanges();

            return Task.CompletedTask;



        }


        public Task<IEnumerable<주문서정보>> 생산계획주문서정보_조회Dz(string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var list = dcDz.VL_MES_SO
                .Where(x => x.CO_CD == 회사코드)
                .ToList();

            List<주문서정보> 주문서정보List = new List<주문서정보>();
            //발주서정보
            foreach (var item in list)
            {

                var 주문서정보 = new 주문서정보
                {
                    회사코드 = item.CO_CD,
                    사업장코드 = item.DIV_CD,
                    부서코드 = item.DEPT_CD,
                    사원코드 = item.EMP_CD,
                    주문번호 = item.SO_NB,
                    주문일자 = item.SO_DT,
                    고객코드 = item.TR_CD,
                    고객명 = item.TR_NM,
                    주문구분 = item.SO_FG,
                    과세구분 = item.VAT_FG,
                    과세구분명 = item.VAT_NM,
                    단가구분 = item.UMVAT_FG,
                    단가구분명 = item.UMVAT_NM,
                    납품처코드 = item.SHIP_CD,
                    납품처명 = item.SHIP_NM,
                    담당자코드 = item.PLN_CD,
                    담당자명 = item.PLN_NM,
                    관리번호 = item.DUMMY1,
                    헤더비고 = item.REMARK_DC,
                    순번 = item.SO_SQ,
                    품목코드 = item.ITEM_CD,
                    품목명 = item.ITEM_NM,
                    규격 = item.ITEM_DC,
                    관리단위 = item.UNITMANG_DC,
                    납기일 = item.DUE_DT,
                    출하예정일 = item.SHIPREQ_DT,
                    수량 = item.SO_QT,
                    단가 = item.SO_UM,
                    부가세단가 = item.VAT_UM,
                    공급가 = item.SOG_AM,
                    SOV_AM = item.SOV_AM,
                    합계액 = item.SOH_AM,
                    관리구분코드 = item.MGMT_CD,
                    관리구분명 = item.MGM_NM,
                    프로젝트코드 = item.PJT_CD,
                    프로젝트명 = item.PJT_NM,
                    디테일비고 = item.REMARK_DC_D,
                    마감여부 = item.EXPIRE_YN,
                    검사구분 = item.QC_FG,
                    환종 = item.EXCH_CD,

                };

                주문서정보List.Add(주문서정보);
            }
            var 주문서 = 주문서정보List.OrderByDescending(x => x.CreateTime);
            return Task.FromResult(주문서.AsEnumerable());
        }


        public Task<IEnumerable<외주작업지시서정보>> 재고이동외주작업지시서정보_조회Dz(string 회사코드, string 처리구분)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var list = dcDz.VL_MES_WO_WF
                .Where(x => x.CO_CD == 회사코드 && x.WF_FG == 처리구분)
                .ToList();

            List<외주작업지시서정보> 외주작업지시서정보List = new List<외주작업지시서정보>();
            //발주서정보
            foreach (var item in list)
            {

                var 외주작업지시서 = new 외주작업지시서정보
                {
                    회사코드 = item.CO_CD,
                    지시번호 = item.WO_CD,
                    지시일 = DateTime.ParseExact(item.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    완료일 = DateTime.ParseExact(item.COMP_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    품번 = item.ITEM_CD,
                    품명 = item.ITEM_NM,
                    규격 = item.ITEM_DC,
                    관리단위 = item.UNIT_DC,
                    수량 = item.ITEM_QT,
                    전개순번 = item.WOOP_SQ,
                    공정 = item.BASELOC_CD,
                    공정명 = item.BASELOC_NM,
                    작업장 = item.LOC_CD,
                    작업장명 = item.LOC_NM,
                    외주단가 = item.LBR_UM,
                    외주금액 = item.LBR_AM,
                    설비코드 = item.EQUIP_CD,
                    설비명 = item.EQUIP_NM,
                    비고_DOC_DC = item.DOC_DC,
                    지시상태 = item.DOC_ST,
                    지시상태명 = item.DOC_ST_NM,
                    지시구분 = item.WOC_FG,
                    지시구분명 = item.WOC_FG_NM,
                    생산외주구분 = item.DOC_FG,
                    생산외주구분명 = item.DOC_FG_NM,
                    처리구분 = item.WF_FG,
                    처리구분명 = item.WF_FG_NM,
                    검사구분 = item.QC_FG,
                    검사구분명 = item.QC_FG_NM,
                    LOT번호 = item.LOT_NB,
                    거래처코드 = item.TR_CD,
                    거래처명 = item.TR_NM,
                    거래처약칭 = item.ATTR_NM,
                    주문번호 = item.SO_NB,
                    주문순번 = item.LN_SQ,
                    사업장코드 = item.DIV_CD,
                    작업팀 = item.WTEAM_CD,
                    작업팀명 = item.WTEAM_NM,
                    작업조 = item.WSHFT_CD,
                    작업조명 = item.WSHFT_NM,
                    비고 = item.REMARK_DC,

                };

                외주작업지시서정보List.Add(외주작업지시서);
            }
            var 외주작업 = 외주작업지시서정보List.OrderByDescending(x => x.CreateTime).ToList();
            return Task.FromResult(외주작업.AsEnumerable());
        }


        //외주지시VIEW
        public Task<IEnumerable<외주작업지시서정보>> 외주작업지시서입고정보_조회Dz(string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;
            var list = dcDz.VL_MES_WO_WF
               .Where(x => x.CO_CD == 회사코드)
               .ToList();

            List<외주작업지시서정보> 외주작업지시서정보List = new List<외주작업지시서정보>();
            //발주서정보
            foreach (var item in list)
            {

                var 외주작업지시서 = new 외주작업지시서정보
                {
                    회사코드 = item.CO_CD,
                    지시번호 = item.WO_CD,
                    지시일 = DateTime.ParseExact(item.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    완료일 = DateTime.ParseExact(item.COMP_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    품번 = item.ITEM_CD,
                    품명 = item.ITEM_NM,
                    규격 = item.ITEM_DC,
                    관리단위 = item.UNIT_DC,
                    수량 = item.ITEM_QT,
                    전개순번 = item.WOOP_SQ,
                    공정 = item.BASELOC_CD,
                    공정명 = item.BASELOC_NM,
                    작업장 = item.LOC_CD,
                    작업장명 = item.LOC_NM,
                    외주단가 = item.LBR_UM,
                    외주금액 = item.LBR_AM,
                    설비코드 = item.EQUIP_CD,
                    설비명 = item.EQUIP_NM,
                    비고_DOC_DC = item.DOC_DC,
                    지시상태 = item.DOC_ST,
                    지시상태명 = item.DOC_ST_NM,
                    지시구분 = item.WOC_FG,
                    지시구분명 = item.WOC_FG_NM,
                    생산외주구분 = item.DOC_FG,
                    생산외주구분명 = item.DOC_FG_NM,
                    처리구분 = item.WF_FG,
                    처리구분명 = item.WF_FG_NM,
                    검사구분 = item.QC_FG,
                    검사구분명 = item.QC_FG_NM,
                    LOT번호 = item.LOT_NB,
                    거래처코드 = item.TR_CD,
                    거래처명 = item.TR_NM,
                    거래처약칭 = item.ATTR_NM,
                    주문번호 = item.SO_NB,
                    주문순번 = item.LN_SQ,
                    사업장코드 = item.DIV_CD,
                    작업팀 = item.WTEAM_CD,
                    작업팀명 = item.WTEAM_NM,
                    작업조 = item.WSHFT_CD,
                    작업조명 = item.WSHFT_NM,
                    비고 = item.REMARK_DC,

                };

                외주작업지시서정보List.Add(외주작업지시서);
            }
            var orderbyList = 외주작업지시서정보List.OrderByDescending(x => x.CreateTime).ToList();
            return Task.FromResult(orderbyList.AsEnumerable());
        }



        public Task<IEnumerable<보유품목위치정보>> 보유품목장소위치Popup_조회(string 회사코드, string 장소위치코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.보유품목위치정보
                    .Include(x => x.장소위치)
                    .Include(x => x.보유품목).ThenInclude(x => x.품목)
                .Where_미삭제_사용()
                .Where(x => x.회사코드 == 회사코드 && x.장소위치코드 == 장소위치코드)     // 설비는 제외
                .Order_등록최신()
                .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task<bool> 외주제품입고처리_등록(작업외주생산실적등록정보 작업외주생산실적, 외주작업지시서품검정보 외주작업지시서)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            try
            {
                DateTime 보유년월일 = DateTime.Now;
                var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == 외주작업지시서.품번 && x.회사코드 == 외주작업지시서.회사코드).FirstOrDefault();
                var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 외주작업지시서.품번).FirstOrDefault();
                if (보유품목 == null)
                {
                    var info = new 보유품목정보
                    {
                        회사코드 = 작업외주생산실적.회사코드,
                        보유품목코드 = 외주작업지시서.품번,
                        품목코드 = 외주작업지시서.품번,
                        보유년월일 = 보유년월일.ToString("yyyyMMdd"),
                        조정년월일 = null,
                        보유일 = 보유년월일,
                        순번 = 1,
                        수량 = 작업외주생산실적.실적수량,
                        실제수량 = 작업외주생산실적.실적수량,
                        품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                        장소코드 = 작업외주생산실적.이동공정_입고창고코드,
                        장소위치코드 = $"{작업외주생산실적.이동공정_입고창고코드}{작업외주생산실적.이동작업장_입고장소코드}",
                        LOT번호 = 작업외주생산실적.LOT번호,
                        품목_LOT번호 = $"{외주작업지시서.품번}:{작업외주생산실적.LOT번호}"
                    };

                    dc.보유품목정보.Add(info);
                }
                else
                {
                    보유품목.LOT번호 = 작업외주생산실적.LOT번호;
                    보유품목.품목_LOT번호 = $"{외주작업지시서.품번}:{작업외주생산실적.LOT번호}";
                    보유품목.장소코드 = 작업외주생산실적.이동공정_입고창고코드;
                    보유품목.장소위치코드 = $"{작업외주생산실적.이동공정_입고창고코드}{작업외주생산실적.이동작업장_입고장소코드}";
                    보유품목.수량 = 보유품목.수량 + 작업외주생산실적.실적수량;
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


        public Task<bool> 외주제품_위치등록(작업외주생산실적등록정보 작업외주생산실적, 외주작업지시서품검정보 외주작업지시서,decimal 불량수량)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            try
            {
                var 장소위치코드 = $"{작업외주생산실적.이동공정_입고창고코드}{작업외주생산실적.이동작업장_입고장소코드}";
                var 보유품목 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 외주작업지시서.품번);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목 == default)
                    return Task.FromResult(result);

                var info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 외주작업지시서.품번 && x.장소위치코드 == 장소위치코드);


                if (info == default)
                {
                    info = new 보유품목위치정보
                    {
                        회사코드 = 외주작업지시서.회사코드,
                        보유품목코드 = 외주작업지시서.품번,
                        장소위치코드 = 장소위치코드,
                        수량 = 작업외주생산실적.실적수량,
                        LOT번호 = 작업외주생산실적.LOT번호,
                        품목_LOT번호 = $"{외주작업지시서.품번}:{작업외주생산실적.LOT번호}",

                    };
                    dc.보유품목위치정보.Add(info);
                }
                // 변경
                else
                {
                    info.수량 = info.수량 + 작업외주생산실적.실적수량;
                    info.LOT번호 = 작업외주생산실적.LOT번호;
                    info.품목_LOT번호 = $"{외주작업지시서.품번}:{작업외주생산실적.LOT번호}";
                    dc.보유품목위치정보.Update(info);
                }

                dc.SaveChanges();

                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 외주작업지시서.품번 && x.장소위치코드 == 장소위치코드);

                var log = new 보유품목이력
                {
                    회사코드 = 외주작업지시서.회사코드,
                    보유품목코드 = 외주작업지시서.품번,
                    연계보유품목코드 = 외주작업지시서.품번,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = 작업외주생산실적.이동공정_입고창고코드,
                    장소위치코드 = 장소위치코드,
                    변경수량 = 작업외주생산실적.실적수량,
                    변경사유 = "외주입고",
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now,
                    LOT번호 = 작업외주생산실적.LOT번호,
                    품목_LOT번호 = $"{외주작업지시서.품번}:{작업외주생산실적.LOT번호}",
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                if (보유이력 == null)
                    dc.보유품목이력.Add(log);


                dc.SaveChanges();

                // 2021.05.31
                var 바코드발급 = dc.바코드발급정보
                    .Include(x => x.품목)
                    .Where(x => x.품목코드 == 외주작업지시서.품번 && x.LOT번호 == 작업외주생산실적.LOT번호 && x.회사코드 == 외주작업지시서.회사코드)
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
                return Task.FromResult(result);
            }
            #region 불량품 창고넣기
            //불량처리
            try
            {
                string 불량장소위치 = "10001009";
                var 불량조회 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 외주작업지시서.품번 && x.장소위치코드 == 불량장소위치);

                if (불량조회 == default)
                {
                    불량조회 = new 보유품목위치정보
                    {
                        회사코드 = 외주작업지시서.회사코드,
                        보유품목코드 = 외주작업지시서.품번,
                        장소위치코드 = 불량장소위치,
                        수량 = 불량수량 > 0 ? 불량수량 : 외주작업지시서.불량수량,
                        LOT번호 = 작업외주생산실적.LOT번호,
                        품목_LOT번호 = $"{외주작업지시서.품번}:{작업외주생산실적.LOT번호}",


                    };
                    dc.보유품목위치정보.Add(불량조회);
                }
                // 변경
                else
                {
                    불량조회.수량 = 불량조회.수량 + (불량수량 > 0 ? 불량수량 : 외주작업지시서.불량수량);
                    불량조회.LOT번호 = 작업외주생산실적.LOT번호;
                    불량조회.품목_LOT번호 = $"{외주작업지시서.품번}:{작업외주생산실적.LOT번호}";
                    dc.보유품목위치정보.Update(불량조회);
                }

                dc.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            #endregion

            result = true;

            return Task.FromResult(result);

        }


       
        public Task<bool> MES재고이동생산외주_재고이동상세정보_등록(재고이동상세정보 이동처리, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.재고이동상세정보.Count(x => x.회사코드 == 이동처리.회사코드 &&
                                        x.작업번호 == 이동처리.작업번호) + 1;
            //var 순번 = dc.출고처리상세정보.Count() + 1;
            //var 순번_더존 = dc_D.BARPLUS_LSTKMOVE_D.Count(x => x.CO_CD == 이동처리.회사코드 &&    x.WORK_NB == 이동처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LDELIVER_D.Count() + 1;

            try
            {



                var BOM_Single = dc_D.VL_MES_BOM.Where(x => x.CO_CD == 이동처리.회사코드 && x.ITEMPARENT_CD == 이동처리.품번).FirstOrDefault();
                var 더존비오엠 = BOM_Single;
                if (BOM_Single != null)
                {
                    var CharCheck = BOM_Single.ITEMCHILD_CD.EndsWith("I");
                    if (CharCheck)
                    {
                        더존비오엠 = dc_D.VL_MES_BOM.Where(x => x.CO_CD == 이동처리.회사코드 && x.ITEMPARENT_CD == BOM_Single.ITEMCHILD_CD).FirstOrDefault();
                    }
                    else
                    {
                        더존비오엠 = BOM_Single;
                    }
                }
                var 재고이동헤더 = dc.재고이동헤더정보.Where(x => x.회사코드 == 이동처리.회사코드 && x.작업번호 == 이동처리.작업번호).FirstOrDefault();
                string 이동할장소위치코드 = $"{재고이동헤더.입고공정_창고코드}{재고이동헤더.입고작업장_장소코드}";
                string 이동할위치상세코드 = 재고이동헤더.입고장소위치상세코드;

                var 보유품목정보 = dc.보유품목정보.Where(x => x.품목코드 == 더존비오엠.ITEMCHILD_CD && x.회사코드 == 이동처리.회사코드).FirstOrDefault();
                var 현장소위치코드 = 보유품목정보.장소위치코드;

                var 보유품목현위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == 더존비오엠.ITEMCHILD_CD && x.회사코드 == 이동처리.회사코드 && x.장소위치코드 == 현장소위치코드).FirstOrDefault();

                var 보유품목이동위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == 더존비오엠.ITEMCHILD_CD && x.회사코드 == 이동처리.회사코드 && x.장소위치코드 == 이동할장소위치코드).FirstOrDefault();



                if (보유품목정보?.수량 < 이동처리.이동수량)
                    return Task.FromResult(result);

                if (보유품목현위치?.수량 < 이동처리.이동수량)
                    return Task.FromResult(result);

                //if (재고이동헤더.이동구분.Equals("5"))
                이동처리.재공운영여부 = "1";

                var BARPLUS_LSTKMOVE_D = new BARPLUS_LSTKMOVE_D
                {
                    CO_CD = 이동처리.회사코드,
                    WORK_NB = 이동처리.작업번호,
                    WORK_SQ = isAdd == true ? 순번 + 1 : 순번,
                    ITEM_CD = 더존비오엠.ITEMCHILD_CD,
                    REQ_QT = 이동처리.청구수량,
                    MOVE_QT = 이동처리.이동수량,
                    WIP_YN = 이동처리.재공운영여부,
                    APP_FG = 이동처리.APP_FG,
                    USE_YN = 이동처리.사용여부,
                    EXPIRE_YN = 이동처리.만료여부,
                    LOT_NB = 보유품목현위치 != null ? 보유품목현위치.LOT번호 : 이동처리.LOT번호,
                };

                string 이동구분 = "";
                // 재고이동 내부이동
                if (재고이동헤더.이동구분 != "")
                {
                    if (보유품목정보 != null)
                    {
                        //보유품목정보.장소코드 = 재고이동헤더.입고공정_창고코드;
                        //보유품목정보.장소위치코드 = 이동할장소위치코드;
                        //dc.보유품목정보.Update(보유품목정보);
                        //dc.SaveChanges();
                    }
                    if (보유품목현위치 != null)
                    {
                        if (보유품목이동위치 == null)
                        {
                            var info = new 보유품목위치정보
                            {
                                회사코드 = 이동처리.회사코드,
                                보유품목코드 = 더존비오엠.ITEMCHILD_CD,
                                장소위치코드 = 이동할장소위치코드,
                                //위치상세코드 = 이동할위치상세코드,
                                수량 = 이동처리.이동수량,
                                LOT번호 = 보유품목현위치 != null ? 보유품목현위치.LOT번호 : 이동처리.LOT번호,
                                품목_LOT번호 = 보유품목현위치 != null ? 보유품목현위치.품목_LOT번호 : 이동처리.품목_LOT번호,
                            };
                            dc.보유품목위치정보.Add(info);
                            dc.SaveChanges();

                        }
                        else
                        {
                            보유품목이동위치.회사코드 = 이동처리.회사코드;
                            보유품목이동위치.보유품목코드 = 더존비오엠.ITEMCHILD_CD;
                            보유품목이동위치.장소위치코드 = 이동할장소위치코드;
                            //보유품목이동위치.위치상세코드 = 이동할위치상세코드;
                            보유품목이동위치.수량 = 보유품목이동위치.수량 + 이동처리.이동수량;
                            보유품목이동위치.LOT번호 = 보유품목현위치 != null ? 보유품목현위치.LOT번호 : 이동처리.LOT번호;
                            보유품목이동위치.품목_LOT번호 = 보유품목현위치 != null ? 보유품목현위치.품목_LOT번호 : 이동처리.품목_LOT번호;

                            dc.보유품목위치정보.Update(보유품목이동위치);
                            dc.SaveChanges();
                        }

                        보유품목현위치.수량 = 보유품목현위치.수량 - 이동처리.이동수량;
                        if (보유품목현위치.수량 == 0)
                            dc.보유품목위치정보.Remove(보유품목현위치);
                        else
                            dc.보유품목위치정보.Update(보유품목현위치);
                        dc.SaveChanges();


                    }
                    if (재고이동헤더.이동구분.Equals("0")) 이동구분 = "생산이동";
                    if (재고이동헤더.이동구분.Equals("1")) 이동구분 = "외주이동";
                    var 보유품목이력 = new 보유품목이력
                    {
                        회사코드 = 이동처리.회사코드,
                        보유품목코드 = 더존비오엠.ITEMCHILD_CD,
                        연계보유품목코드 = 이동처리.품번,
                        변경유형코드 = "B1705",    // 입고
                        장소코드 = 재고이동헤더.입고공정_창고코드,
                        장소위치코드 = 이동할장소위치코드,
                        //위치상세코드 = 이동할위치상세코드,
                        변경수량 = 이동처리.이동수량,
                        변경사유 = 이동구분,
                        유형사유 = "B1705",
                        변경일시 = DateTime.Now,
                        LOT번호 = 보유품목현위치 != null ? 보유품목현위치.LOT번호 : 이동처리.LOT번호,
                        품목_LOT번호 = 보유품목현위치 != null ? 보유품목현위치.품목_LOT번호 : 이동처리.품목_LOT번호,
                    };
                    dc.보유품목이력.Add(보유품목이력);
                    dc.SaveChanges();
                }

                if (isAdd)
                {
                    //dc.보유품목정보.Add(info);

                    이동처리.작업순번 = 순번;
                    dc.재고이동상세정보.Add(이동처리);
                    dc_D.BARPLUS_LSTKMOVE_D.Add(BARPLUS_LSTKMOVE_D);
                }
                else
                {
                    dc.재고이동상세정보.Update(이동처리);
                    dc_D.BARPLUS_LSTKMOVE_D.Update(BARPLUS_LSTKMOVE_D);
                }


            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            dc.SaveChanges();

            dc_D.SaveChanges();

            result = true;

            return Task.FromResult(result);
        }

        public Task<IEnumerable<BOM_정보>> NEW공정단위BOM_정보_조회(string 회사코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.BOM_정보
                   .Where(x => x.회사코드 == 회사코드)
                   .Where_미삭제_사용()
                   .Order_등록최신()
                   .ToList();

            return Task.FromResult(result.AsEnumerable());
        }


        public Task<List<공정단위자재현황>> 작업지시공정단위자재현황_조회(string 회사코드, string 공정단위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정단위자재정보
                .Where(x => x.회사코드 == 회사코드 && x.공정단위코드 == 공정단위코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();
            List<공정단위자재현황> list = new List<공정단위자재현황>();
            foreach (var item in result)
            {
                var 공정단위자재 = new 공정단위자재현황
                {
                    회사코드 = item.회사코드,
                    공정단위코드 = item.공정단위코드,
                    자재코드 = item.자재코드,
                    필요수량 = item.수량,
                };
                list.Add(공정단위자재);
            }

            return Task.FromResult(list);

        }







        //20210529

        public Task<외주작업지시서품검정보> 외주작업지시서번호정보_조회Dz(string 회사코드, string 지시번호)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var list = dcDz.VL_MES_WO_WF
                .Where(x => x.CO_CD == 회사코드 && x.WO_CD == 지시번호)
                .ToList();

            var 외주작업 = dc.외주작업지시서품검정보
                .Where(x => x.회사코드 == 회사코드 && x.지시번호 == 지시번호).FirstOrDefault();

            List<외주작업지시서품검정보> 외주작업지시서정보List = new List<외주작업지시서품검정보>();

            외주작업지시서품검정보 외주작업지시서 = null;

            if (외주작업 != null)
            {
                외주작업지시서 = 외주작업;

            }
            else
            {
                foreach (var item in list)
                {
                    외주작업지시서 = new 외주작업지시서품검정보
                    {
                        회사코드 = item.CO_CD,
                        지시번호 = item.WO_CD,
                        지시일 = DateTime.ParseExact(item.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        완료일 = DateTime.ParseExact(item.COMP_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        품번 = item.ITEM_CD,
                        품명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        관리단위 = item.UNIT_DC,
                        수량 = item.ITEM_QT,
                        전개순번 = item.WOOP_SQ,
                        공정 = item.BASELOC_CD,
                        공정명 = item.BASELOC_NM,
                        작업장 = item.LOC_CD,
                        작업장명 = item.LOC_NM,
                        외주단가 = item.LBR_UM,
                        외주금액 = item.LBR_AM,
                        설비코드 = item.EQUIP_CD,
                        설비명 = item.EQUIP_NM,
                        비고_DOC_DC = item.DOC_DC,
                        지시상태 = item.DOC_ST,
                        지시상태명 = item.DOC_ST_NM,
                        지시구분 = item.WOC_FG,
                        지시구분명 = item.WOC_FG_NM,
                        생산외주구분 = item.DOC_FG,
                        생산외주구분명 = item.DOC_FG_NM,
                        처리구분 = item.WF_FG,
                        처리구분명 = item.WF_FG_NM,
                        검사구분 = item.QC_FG,
                        검사구분명 = item.QC_FG_NM,
                        LOT번호 = item.LOT_NB,
                        거래처코드 = item.TR_CD,
                        거래처명 = item.TR_NM,
                        거래처약칭 = item.ATTR_NM,
                        주문번호 = item.SO_NB,
                        주문순번 = item.LN_SQ,
                        사업장코드 = item.DIV_CD,
                        작업팀 = item.WTEAM_CD,
                        작업팀명 = item.WTEAM_NM,
                        작업조 = item.WSHFT_CD,
                        작업조명 = item.WSHFT_NM,
                        비고 = item.REMARK_DC,

                    };

                }
            }



            return Task.FromResult(외주작업지시서);
        }



        public Task<IEnumerable<외주작업지시서품검정보>> 외주작업지시서품검정보_조회Dz(string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dcDz.VL_MES_WO_WF
                .Where(x => x.CO_CD == 회사코드 && x.DOC_FG == "1")
                .ToList();

            List<외주작업지시서품검정보> 외주작업지시서정보List = new List<외주작업지시서품검정보>();
            외주작업지시서품검정보 외주작업지시서;
            //발주서정보
            foreach (var item in list)
            {
                var 외주품질검사완료여부 = dc.외주작업지시서품검정보.Where(x => x.회사코드 == 회사코드 && x.지시번호 == item.WO_CD).FirstOrDefault();

                외주작업지시서 = new 외주작업지시서품검정보
                {
                    회사코드 = item.CO_CD,
                    지시번호 = item.WO_CD,
                    지시일 = DateTime.ParseExact(item.ORD_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    완료일 = DateTime.ParseExact(item.COMP_DT, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None),
                    품번 = item.ITEM_CD,
                    품명 = item.ITEM_NM,
                    규격 = item.ITEM_DC,
                    관리단위 = item.UNIT_DC,
                    수량 = item.ITEM_QT,
                    전개순번 = item.WOOP_SQ,
                    공정 = item.BASELOC_CD,
                    공정명 = item.BASELOC_NM,
                    작업장 = item.LOC_CD,
                    작업장명 = item.LOC_NM,
                    외주단가 = item.LBR_UM,
                    외주금액 = item.LBR_AM,
                    설비코드 = item.EQUIP_CD,
                    설비명 = item.EQUIP_NM,
                    비고_DOC_DC = item.DOC_DC,
                    지시상태 = item.DOC_ST,
                    지시상태명 = item.DOC_ST_NM,
                    지시구분 = item.WOC_FG,
                    지시구분명 = item.WOC_FG_NM,
                    생산외주구분 = item.DOC_FG,
                    생산외주구분명 = item.DOC_FG_NM,
                    처리구분 = item.WF_FG,
                    처리구분명 = item.WF_FG_NM,
                    검사구분 = item.QC_FG,
                    검사구분명 = item.QC_FG_NM,
                    LOT번호 = item.LOT_NB,
                    거래처코드 = item.TR_CD,
                    거래처명 = item.TR_NM,
                    거래처약칭 = item.ATTR_NM,
                    주문번호 = item.SO_NB,
                    주문순번 = item.LN_SQ,
                    사업장코드 = item.DIV_CD,
                    작업팀 = item.WTEAM_CD,
                    작업팀명 = item.WTEAM_NM,
                    작업조 = item.WSHFT_CD,
                    작업조명 = item.WSHFT_NM,
                    비고 = item.REMARK_DC,

                };

                if (외주품질검사완료여부 != null)
                {
                    외주작업지시서.품질검사완료여부 = 외주품질검사완료여부.품질검사완료여부;
                    외주작업지시서.실행상태코드 = 외주품질검사완료여부.실행상태코드;

                }

                외주작업지시서정보List.Add(외주작업지시서);
            }

            var 외주작업지시 = 외주작업지시서정보List.OrderByDescending(x => x.CreateTime).ToList();

            return Task.FromResult(외주작업지시.AsEnumerable());
        }




        public Task<IEnumerable<품목정보>> VL_MES_ITEM_Auto()
        {
            //using var scopeDZ = dcp.GetDbContextScopeDZ();
            //var dcDZ = scopeDZ.DbContext;
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            //List<VL_MES_ITEM> 더존품목정보 = new List<VL_MES_ITEM>();
            ////var result = dc.VL_MES_ITEM.Select(x => x)	
            ////   .FirstOrDefault();	
            //더존품목정보 = dcDZ.VL_MES_ITEM.Select(x => x)
            //    .ToList();
            //bool result = false;
            //try
            //{

            //    //var sql = "DELETE FROM 품목정보";
            //    //dc.Database.ExecuteSqlRaw(sql);

            //    var 품목 = 더존품목정보.Where(x => x.CO_CD == "2265" && x.ITEM_CD != "").ToList();
            //    foreach (var item in 품목)
            //    {
            //        //생산품 - > 제품  부품 -> 부재료	
            //        string 단위코드 = "";
            //        string 조달구분 = "";
            //        string 품목구분코드 = "";
            //        if (item.UNIT_DC.Contains("EA", StringComparison.OrdinalIgnoreCase))
            //        {
            //            단위코드 = "B1101";
            //        }
            //        else if (item.UNIT_DC.Contains("won", StringComparison.OrdinalIgnoreCase))
            //        {
            //            단위코드 = "B1102";
            //        }
            //        else if (item.UNIT_DC.Contains("SET", StringComparison.OrdinalIgnoreCase))
            //        {
            //            단위코드 = "B1101";
            //        }
            //        if (item.ODR_FG.Contains("0"))
            //        {
            //            조달구분 = "B1601";
            //        }
            //        else if (item.ODR_FG.Contains("1"))
            //        {
            //            조달구분 = "B1602";
            //        }
            //        else if (item.ODR_FG.Contains("2"))
            //        {
            //            조달구분 = "B1608";
            //        }
            //        //0.원재료 1.부재료 2.제품 4.반제품 5.상품 6.저장품 7.비용 8.수익	
            //        if (item.ACCT_FG.Contains("0"))
            //        {
            //            품목구분코드 = "B1201";
            //        }
            //        else if (item.ACCT_FG.Contains("1"))
            //        {
            //            품목구분코드 = "B1202";
            //        }
            //        else if (item.ACCT_FG.Contains("2"))
            //        {
            //            품목구분코드 = "B1203";
            //        }
            //        //else if(item.ACCT_FG.Contains("3"))	
            //        //{	
            //        //    품목구분코드 = "B1204";	
            //        //}	
            //        else if (item.ACCT_FG.Contains("4"))
            //        {
            //            품목구분코드 = "B1204";
            //        }
            //        else if (item.ACCT_FG.Contains("5"))
            //        {
            //            품목구분코드 = "B1207";
            //        }
            //        else if (item.ACCT_FG.Contains("6"))
            //        {
            //            품목구분코드 = "B1208";
            //        }
            //        else if (item.ACCT_FG.Contains("7"))
            //        {
            //            품목구분코드 = "B1206";
            //        }
            //        else if (item.ACCT_FG.Contains("8"))
            //        {
            //            품목구분코드 = "B1209";
            //        }
            //        var 품목정보 = new 품목정보()
            //        {
            //            회사코드 = item.CO_CD,
            //            품목코드 = item.ITEM_CD,
            //            원품목코드 = item.ITEM_CD,
            //            품목명 = item.ITEM_NM,
            //            규격 = item.ITEM_DC,
            //            품목구분코드 = 품목구분코드,
            //            조달구분코드 = 조달구분,
            //            단위코드 = 단위코드,
            //            재고단위 = item.UNIT_DC,
            //            LOT여부 = item.LOT_FG == "0" ? false : true,
            //            사용유무 = item.USE_YN == "0" ? false : true,
            //            거래처코드 = item.TRMAIN_CD,
            //        };

            //        var found = dc.품목정보.Where(x => x.품목코드 == item.ITEM_CD).FirstOrDefault();
            //        if (found == null)
            //            dc.품목정보.Add(품목정보);
            //        else
            //            dc.품목정보.Update(품목정보);
            //    }
            //    dc.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //    //return Task.FromResult(result);	
            //}
            var list = dc.품목정보
                .Include(x => x.거래처)
                //.Where(x => (isOnlyUse == true && x.사용유무 == true) || isOnlyUse == false)	
                .Where_미삭제()
                //.Where(x => (생산품만 == true && x.품목구분코드 == "B1203") || (생산품만 == false && (x.품목구분코드 == "B1203" || x.품목구분코드 == "B1204"))) // 생산품, 반제품	
                .OrderByDescending(x => x.CreateTime)
                .ToList();
            return Task.FromResult(list.AsEnumerable());
        }


        public Task<IEnumerable<BOM_정보>> VL_MES_BOM_Auto(string 회사코드)
        {
            //using var scopeDZ = dcp.GetDbContextScopeDZ();
            //var dcDZ = scopeDZ.DbContext;
            //List<VL_MES_BOM> 더존BOM정보 = new List<VL_MES_BOM>();
            //더존BOM정보 = dcDZ.VL_MES_BOM.Select(x => x)
            //    .ToList();
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            //try
            //{
            //    var sql = "DELETE FROM BOM_정보 ";
            //    dc.Database.ExecuteSqlRaw(sql);

            //    sql = "DELETE FROM BOM정보 ";
            //    dc.Database.ExecuteSqlRaw(sql);
            //    foreach (var item in 더존BOM정보)
            //    {
            //        var BOM_정보 = new BOM_정보()
            //        {
            //            회사코드 = item.CO_CD,
            //            모품번 = item.ITEMPARENT_CD,
            //            모품명 = item.ITEMPARENT_NM,
            //            모규격 = item.ITEMPARENT_DC,
            //            모품목재고단위 = item.ITEMPARENT_UNIT_DC,
            //            순번 = item.BOM_SQ,
            //            자품번 = item.ITEMCHILD_CD,
            //            자품명 = item.ITEMCHILD_NM,
            //            자규격 = item.ITEMCHILD_DC,
            //            자품목재고단위 = item.ITEMCHILD_UNIT_DC,
            //            정미수량 = item.JUST_QT,
            //            LOSS율 = item.LOSS_RT,
            //            필요수량 = item.REAL_QT,
            //            외주구분 = item.OUT_FG,
            //            임가공구분 = item.ODR_FG,
            //            주거래처코드 = item.TRMAIN_CD,
            //            주거래처명 = item.ATTR_NM,
            //            시작일자 = item.START_DT,
            //            종료일자 = item.END_DT,
            //            사용여부 = item.USE_YN,
            //        };
            //        var found = dc.BOM_정보.Where(x => x.모품번 == item.ITEMPARENT_CD && x.자품번 == item.ITEMCHILD_NM && x.회사코드 == item.CO_CD).FirstOrDefault();
            //        if (found == null)
            //            dc.BOM_정보.Add(BOM_정보);
            //        else
            //            dc.BOM_정보.Update(BOM_정보);
            //        //var 품목정보 = new 품목정보()	
            //        //{	
            //        //    회사코드 = item.CO_CD,	
            //        //    품목코드 = item.ITEMPARENT_CD,	
            //        //    원품목코드 = item.ITEMPARENT_CD,	
            //        //    품목명 = item.ITEMPARENT_NM,	
            //        //    규격 = item.ITEMPARENT_DC,	
            //        //    품목구분코드 = "B1202",	
            //        //    조달구분코드 = "B1601",	
            //        //    단위코드 = "B1101",	
            //        //    재고단위 = item.ITEMPARENT_UNIT_DC,	
            //        //};	
            //        //var found1 = dc.품목정보.Where(x => x.품목코드 == item.ITEMPARENT_CD).FirstOrDefault();	
            //        //if (found1 == null)	
            //        //    dc.품목정보.Add(품목정보);	
            //        dc.SaveChanges();


            //        //2021.08.17 BOM정보 table에 추가작업
            //        // 트리구조 표현을 위함
            //        var 상위품목코드 = item.ITEMPARENT_CD;
            //        var 상위BOM순번 = dc.BOM정보.Where(x => x.품목코드 == 상위품목코드).Select(x => x.BOM순번).FirstOrDefault();
            //        if(상위BOM순번 == 0)
            //        {
            //            var 상위BOM = new BOM정보
            //            {
            //                품목코드 = item.ITEMPARENT_CD,
            //                상위BOM순번 = null,
            //                정미수량 = 1,
            //                로스율 = 0,
            //                필요수량 = 1
            //            };
            //            dc.BOM정보.Add(상위BOM);
            //            dc.SaveChanges();
            //        }
            //        상위BOM순번 = dc.BOM정보.Where(x => x.품목코드 == 상위품목코드).Select(x => x.BOM순번).FirstOrDefault();
            //        var 자BOM = new BOM정보
            //        {
            //            품목코드 = item.ITEMCHILD_CD,
            //            상위BOM순번 = 상위BOM순번 == 0 ? null : 상위BOM순번,
            //            정미수량 = item.JUST_QT,
            //            로스율 = item.LOSS_RT,
            //            필요수량 = item.REAL_QT
            //        };

            //        dc.BOM정보.Add(자BOM);
            //        dc.SaveChanges();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //    //return Task.FromResult(result);	
            //}
            //dc.SaveChanges();	
            var list = dc.BOM_정보
                 .Where(x => x.회사코드 == 회사코드)
                 .Where_미삭제_사용()
                 .Order_등록최신()
                 .ToList();
            return Task.FromResult(list.AsEnumerable());
        }



        public Task<bool> MES외주생산실적_작업외주생산실적등록정보_등록(작업외주생산실적등록정보 생산실적, bool isAdd)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            //var 출고추가수정유무 = dc.출고처리헤더정보.Count(x => x.작업번호 == 출고처리.작업번호);
            var 순번 = dc.작업외주생산실적등록정보.Count(x => x.회사코드 == 생산실적.회사코드) + 1;

            //var 더존출고추가수정유무 = dc.BARPLUS_LDELIVER.Count(x => x.WORK_NB == 출고처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LPRODUCTION.Count(x => x.CO_CD == 생산실적.회사코드) + 1;

            try
            {
                var now = DateTime.Now;
                var yymm = now.ToString("yyMM");
                string 작업번호1 = "";
                //작업번호1 = $"{"WR"}{yymm}{순번:000000}";
                작업번호1 = $"{"WO"}{yymm}{순번:000000}";
                //작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";

                var BARPLUS_LORCV_H = new BARPLUS_LORCV_H
                {
                    CO_CD = 생산실적.회사코드,
                    WORK_NB = 작업번호1,
                    WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적.작업일자.ToString())),
                    DOC_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적.실적일자.ToString())),
                    DIV_CD = 생산실적.사업장코드,
                    DEPT_CD = 생산실적.부서코드,
                    EMP_CD = 생산실적.사원코드,
                    MOVEBASELOC_CD = 생산실적.이동공정_입고창고코드,
                    MOVELOC_CD = 생산실적.이동작업장_입고장소코드,
                    REWORK_YN = 생산실적.재작업여부,
                    ITEM_QT = 생산실적.실적수량,
                    BAD_YN = 생산실적.실적구분,
                    WF_FG = 생산실적.처리구분,
                    WO_CD = 생산실적.지시번호,
                    WOOP_SQ = 생산실적.지시전개순번,

                    BASELOC_CD = 생산실적.실적공정코드,
                    LOC_CD = 생산실적.실적작업장코드,
                    LOT_NB = 생산실적.LOT번호,
                    EQUIP_CD = 생산실적.설비코드,
                };

                if (isAdd)
                {
                    생산실적.작업번호 = 작업번호1;
                    dc.작업외주생산실적등록정보.Add(생산실적);
                    dc_D.BARPLUS_LORCV_H.Add(BARPLUS_LORCV_H);
                }
                else
                {
                    dc.작업외주생산실적등록정보.Update(생산실적);
                    dc_D.BARPLUS_LORCV_H.Update(BARPLUS_LORCV_H);
                }

                dc.SaveChanges();
                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            result = true;
            return Task.FromResult(result);
        }







        public Task<bool> MES외주생산실적_외주생산실적소유자재털기_등록(작업외주생산실적등록정보 생산실적파라미터, 외주작업지시서품검정보 외주작업지시, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 생산실적 = dc_D.BARPLUS_LORCV_H.Where(x => x.WO_CD == 생산실적파라미터.지시번호 && x.WOOP_SQ == 생산실적파라미터.지시전개순번).FirstOrDefault();


            try
            {
                var now = DateTime.Now;
                var yymm = now.ToString("yyMM");

                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적파라미터.회사코드 && x.지시서 == 생산실적파라미터.지시번호 && x.보유품목코드 == listBom[0].자재코드).ToList();

                foreach (var item in 외주생산위치정보)
                {
                    var 순번 = dc.사용자재보고정보.Count(x => x.회사코드 == 생산실적파라미터.회사코드 && x.작업번호 == 생산실적.WORK_NB) + 1;
                    //string 작업번호1 = "";
                    //작업번호1 = $"{"WO"}{yymm}{순번:000000}";
                    //작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";

                    var BARPLUS_LMTL_USE = new BARPLUS_LMTL_USE
                    {
                        CO_CD = 생산실적파라미터.회사코드,
                        DIV_CD = 생산실적파라미터.사업장코드,
                        WORK_NB = 생산실적.WORK_NB,
                        WORK_SQ = 순번,
                        WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적파라미터.작업일자.ToString())),
                        ITEM_CD = item.보유품목코드,
                        BASELOC_CD = 생산실적파라미터.실적공정코드,
                        LOC_CD = 생산실적파라미터.실적작업장코드,
                        WO_CD = 생산실적파라미터.지시번호,
                        WOC_FG = 외주작업지시.지시구분,
                        EMP_CD = 생산실적파라미터.사원코드,
                        DEPT_CD = 생산실적파라미터.부서코드,
                        USE_YN = "1",
                        UMU_FG = "1",
                        EXPIRE_YN = "1",
                        WOOP_SQ = 생산실적파라미터.지시전개순번,

                        LOT_NB = item.LOT번호,
                        WR_CD = 생산실적.DOC_CD,
                        USE_SQ = item.보유품목위치순번,
                        USE_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(item.CreateTime.ToString())),
                        USE_QT = listBom[0].사용수량,
                        WOBOM_SQ = item.보유품목위치순번,
                    };

                    var 사용자재보고 = new 사용자재보고정보
                    {
                        회사코드 = 생산실적파라미터.회사코드,
                        사업장코드 = 생산실적파라미터.사업장코드,
                        작업번호 = 생산실적.WORK_NB,
                        작업순번 = 순번,
                        작업일자 = 생산실적파라미터.작업일자,
                        품번 = item.보유품목코드,
                        사용공정 = 생산실적파라미터.실적공정코드,
                        사용작업장 = 생산실적파라미터.실적작업장코드,
                        지시번호 = 생산실적파라미터.지시번호,
                        지시구분 = 외주작업지시.지시구분,
                        사원코드 = 생산실적파라미터.사원코드,
                        부서코드 = 생산실적파라미터.부서코드,
                        사용여부 = "1",
                        유무상구분 = "1",
                        유효여부 = "1",
                        지시전개순번 = 생산실적파라미터.지시전개순번,

                        LOT번호 = item.LOT번호,
                        실적번호 = 생산실적.DOC_CD,
                        사용순번 = item.보유품목위치순번,
                        사용일자 = item.CreateTime,
                        사용수량 = listBom[0].사용수량,
                        소요자재순번 = item.보유품목위치순번,
                    };

                    //var 소유자재조회 = dc_D.BARPLUS_LMTL_USE.Where(x => x.WO_CD == 생산실적파라미터.지시번호 && x.WOOP_SQ == 생산실적파라미터.지시전개순번).FirstOrDefault();

                    dc.사용자재보고정보.Add(사용자재보고);
                    dc_D.BARPLUS_LMTL_USE.Add(BARPLUS_LMTL_USE);

                    dc.SaveChanges();
                    dc_D.SaveChanges();

                    var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == item.보유품목코드 && x.회사코드 == item.회사코드).FirstOrDefault();

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

                    string 위치상세코드 = 생산실적파라미터.실적공정코드 + 생산실적파라미터.실적작업장코드;
                    var 보유품목위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == item.보유품목코드 && x.회사코드 == item.회사코드
                    && x.위치상세코드 == 위치상세코드).FirstOrDefault();

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

                var 외주지시서완료 = dc.외주작업지시서품검정보.Where(x => x.지시번호 == 외주작업지시.지시번호 && x.회사코드 == 생산실적파라미터.회사코드 ).FirstOrDefault();

                if (외주지시서완료 != null)
                {
                    외주지시서완료.품질검사완료여부 = "1";
                    외주지시서완료.실행상태코드 = "B2004";
                    dc.외주작업지시서품검정보.Update(외주지시서완료);
                }

                var 외주생산위치업데이트 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적파라미터.회사코드 && x.지시서 == 생산실적파라미터.지시번호 && x.보유품목코드 == listBom[0].자재코드).FirstOrDefault();

                if (외주생산위치업데이트 != null)
                {
                    
                    decimal 사용수량 = (listBom[0].사용수량 / listBom[0].필요수량);

                    외주생산위치업데이트.수량 = 외주생산위치업데이트.수량 - 사용수량;
                    
                    if(외주생산위치업데이트.수량 > 0 )
                    {
                        var 외주지시서 = dc_D.VL_MES_WO_WF.Where(x => x.CO_CD == 생산실적파라미터.회사코드 && x.WO_CD == 생산실적파라미터.지시번호).FirstOrDefault();
                        if (외주지시서 != null)
                        {
                            외주생산위치업데이트.외주창고코드 = 외주지시서.BASELOC_CD;
                            외주생산위치업데이트.외주장소코드 = 외주지시서.LOC_CD;
                            외주생산위치업데이트.외주작업장명 = 외주지시서.LOC_NM;
                            외주생산위치업데이트.필요수량 = listBom[0].필요수량;
                        }

                        외주생산위치업데이트.반입여부 = "반입";
                        dc.외주생산위치정보.Update(외주생산위치업데이트);
                    }
                    else
                        dc.외주생산위치정보.Remove(외주생산위치업데이트);

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



        public Task<bool> MES외주생산실적_멀티외주생산실적소유자재털기_등록(작업외주생산실적등록정보 생산실적파라미터, 외주작업지시서품검정보 외주작업지시, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 생산실적 = dc_D.BARPLUS_LORCV_H.Where(x => x.WO_CD == 생산실적파라미터.지시번호 && x.WOOP_SQ == 생산실적파라미터.지시전개순번).FirstOrDefault();


            try
            {
                var now = DateTime.Now;
                var yymm = now.ToString("yyMM");

                var 외주생산위치정보 = dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적파라미터.회사코드 && x.지시서 == 생산실적파라미터.지시번호).ToList();

                foreach (var item in 외주생산위치정보)
                {
                    var 순번 = dc.사용자재보고정보.Count(x => x.회사코드 == 생산실적파라미터.회사코드 && x.작업번호 == 생산실적.WORK_NB) + 1;
                    //string 작업번호1 = "";
                    //작업번호1 = $"{"WO"}{yymm}{순번:000000}";
                    //작업번호2 = $"{"IS"}{yyyy}{순번_더존:000000}";

                    var BARPLUS_LMTL_USE = new BARPLUS_LMTL_USE
                    {
                        CO_CD = 생산실적파라미터.회사코드,
                        DIV_CD = 생산실적파라미터.사업장코드,
                        WORK_NB = 생산실적.WORK_NB,
                        WORK_SQ = 순번,
                        WORK_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(생산실적파라미터.작업일자.ToString())),
                        ITEM_CD = item.보유품목코드,
                        BASELOC_CD = 생산실적파라미터.실적공정코드,
                        LOC_CD = 생산실적파라미터.실적작업장코드,
                        WO_CD = 생산실적파라미터.지시번호,
                        WOC_FG = 외주작업지시.지시구분,
                        EMP_CD = 생산실적파라미터.사원코드,
                        DEPT_CD = 생산실적파라미터.부서코드,
                        USE_YN = "1",
                        UMU_FG = "1",
                        EXPIRE_YN = "1",
                        WOOP_SQ = 생산실적파라미터.지시전개순번,

                        LOT_NB = item.LOT번호,
                        WR_CD = 생산실적.DOC_CD,
                        USE_SQ = item.보유품목위치순번,
                        USE_DT = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(item.CreateTime.ToString())),
                        USE_QT = listBom.Where(x => x.자재코드 == item.보유품목코드).FirstOrDefault().사용수량,
                        WOBOM_SQ = item.보유품목위치순번,
                    };

                    var 사용자재보고 = new 사용자재보고정보
                    {
                        회사코드 = 생산실적파라미터.회사코드,
                        사업장코드 = 생산실적파라미터.사업장코드,
                        작업번호 = 생산실적.WORK_NB,
                        작업순번 = 순번,
                        작업일자 = 생산실적파라미터.작업일자,
                        품번 = item.보유품목코드,
                        사용공정 = 생산실적파라미터.실적공정코드,
                        사용작업장 = 생산실적파라미터.실적작업장코드,
                        지시번호 = 생산실적파라미터.지시번호,
                        지시구분 = 외주작업지시.지시구분,
                        사원코드 = 생산실적파라미터.사원코드,
                        부서코드 = 생산실적파라미터.부서코드,
                        사용여부 = "1",
                        유무상구분 = "1",
                        유효여부 = "1",
                        지시전개순번 = 생산실적파라미터.지시전개순번,

                        LOT번호 = item.LOT번호,
                        실적번호 = 생산실적.DOC_CD,
                        사용순번 = item.보유품목위치순번,
                        사용일자 = item.CreateTime,
                        사용수량 = listBom.Where(x => x.자재코드 == item.보유품목코드).FirstOrDefault().사용수량,
                        소요자재순번 = item.보유품목위치순번,
                    };

                    //var 소유자재조회 = dc_D.BARPLUS_LMTL_USE.Where(x => x.WO_CD == 생산실적파라미터.지시번호 && x.WOOP_SQ == 생산실적파라미터.지시전개순번).FirstOrDefault();

                    dc.사용자재보고정보.Add(사용자재보고);
                    dc_D.BARPLUS_LMTL_USE.Add(BARPLUS_LMTL_USE);

                    dc.SaveChanges();
                    dc_D.SaveChanges();

                    var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == item.보유품목코드 && x.회사코드 == item.회사코드).FirstOrDefault();

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

                    string 위치상세코드 = 생산실적파라미터.실적공정코드 + 생산실적파라미터.실적작업장코드;
                    var 보유품목위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == item.보유품목코드 && x.회사코드 == item.회사코드
                    && x.위치상세코드 == 위치상세코드).FirstOrDefault();

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
                

               

                // dc.외주생산위치정보.Where(x => x.회사코드 == 생산실적파라미터.회사코드 && x.지시서 == 생산실적파라미터.지시번호 && x.보유품목코드 == item.보유품목코드).FirstOrDefault();

                var 외주생산위치업데이트 = 외주생산위치정보.Where(x => x.회사코드 == 생산실적파라미터.회사코드 && x.지시서 == 생산실적파라미터.지시번호 && x.보유품목코드 == item.보유품목코드).FirstOrDefault();
                if (외주생산위치업데이트 != null)
                {

                    decimal 사용수량 = (listBom.Where(x => x.자재코드 == item.보유품목코드).FirstOrDefault().사용수량 / listBom.Where(x => x.자재코드 == item.보유품목코드).FirstOrDefault().필요수량);

                    외주생산위치업데이트.수량 = 외주생산위치업데이트.수량 - 사용수량;
                    외주생산위치업데이트.필요수량 = listBom.Where(x => x.자재코드 == item.보유품목코드).FirstOrDefault().필요수량;
                    
                    if (외주생산위치업데이트.수량 > 0)
                    {
                        var 외주지시서 = dc_D.VL_MES_WO_WF.Where(x => x.CO_CD == 생산실적파라미터.회사코드 && x.WO_CD == 생산실적파라미터.지시번호).FirstOrDefault();
                        if (외주지시서 != null)
                        {
                            외주생산위치업데이트.외주창고코드 = 외주지시서.BASELOC_CD;
                            외주생산위치업데이트.외주장소코드 = 외주지시서.LOC_CD;
                            외주생산위치업데이트.외주작업장명 = 외주지시서.LOC_NM;
                        }

                        외주생산위치업데이트.반입여부 = "반입";
                        dc.외주생산위치정보.Update(외주생산위치업데이트);
                    }
                    else
                        dc.외주생산위치정보.Remove(외주생산위치업데이트);
                    }
                }
                              
                var 외주지시서완료 = dc.외주작업지시서품검정보.Where(x => x.지시번호 == 외주작업지시.지시번호 && x.회사코드 == 생산실적파라미터.회사코드).FirstOrDefault();

                if (외주지시서완료 != null)
                {
                    외주지시서완료.품질검사완료여부 = "1";
                    외주지시서완료.실행상태코드 = "B2004";
                    dc.외주작업지시서품검정보.Update(외주지시서완료);
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


        public Task<bool> 외주생산실적소유자재털기_등록(작업외주생산실적등록정보 생산실적파라미터, 외주작업지시서품검정보 외주작업지시, List<공정단위자재현황> listBom)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            //마무리
            try
            {


                dc.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }

            result = true;
            return Task.FromResult(result);

        }



        public Task<품목정보> 품목구분_조회(string 품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.품목정보
                .Where(x => x.품목코드 == 품목코드)
                .Where_미삭제()
                .OrderByDescending(x => x.CreateTime).FirstOrDefault();

            return Task.FromResult(list);
        }




        public Task<IEnumerable<발주서별수입검사>> 발주서별수입검사_조회Dz(string 회사코드)
        {
            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var list = dcDz.VL_MES_PO
                .Where(x => x.CO_CD == 회사코드)
                .ToList();

            List<발주서별수입검사> 발주서정보List = new List<발주서별수입검사>();
            발주서별수입검사 발주서별수입;
            //발주서정보
            foreach (var item in list)
            {

                var 수입검사완료여부 = dc.발주서별수입검사.Where(x => x.회사코드 == 회사코드 && x.발주번호 == item.PO_NB && x.발주순번 == item.PO_SQ).FirstOrDefault();
                
                if (수입검사완료여부 != null)
                {
                    발주서별수입 = new 발주서별수입검사
                    {
                        회사코드 = item.CO_CD,
                        사업장코드 = item.DIV_CD,
                        부서코드 = item.DEPT_CD,
                        사원코드 = item.EMP_CD,
                        발주번호 = item.PO_NB,
                        발주일 = item.PO_DT,
                        거래처코드 = item.TR_CD,
                        거래처명 = item.TR_NM,
                        거래구분 = item.PO_FG,
                        검사구분 = item.QC_FG,
                        과세구분 = item.VAT_FG,
                        과세구분명 = item.VAT_NM,
                        담당자코드 = item.PLN_CD,
                        담당자명 = item.PLN_NM,
                        비고 = item.REMARK_DC,
                        발주순번 = item.PO_SQ,
                        품번 = item.ITEM_CD,
                        품명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        관리단위 = item.UNITMANG_DC,
                        납기일 = item.DUE_DT,
                        출하예정일 = item.SHIPREQ_DT,
                        발주수량 = item.PO_QT,
                        발주단가 = item.PO_UM,
                        공급가 = item.POG_AM,
                        부가세 = item.POGV_AM1,
                        합계액 = item.POGH_AM1,
                        관리구분코드 = item.MGMT_CD,
                        관리구분명 = item.MGM_NM,
                        프로젝트 = item.PJT_CD,
                        프록젝트명 = item.PJT_NM,
                        비고_내역 = item.REMARK_DC_D,
                        환종 = item.EXCH_CD,
                        부가세구분 = item.UMVAT_FG,
                        품질검사완료여부 = 수입검사완료여부.품질검사완료여부,
                        입고여부 = 수입검사완료여부.입고여부,
                        실행상태코드 = 수입검사완료여부.실행상태코드

                    };
                }
                else
                {
                    발주서별수입 = new 발주서별수입검사
                    {
                        회사코드 = item.CO_CD,
                        사업장코드 = item.DIV_CD,
                        부서코드 = item.DEPT_CD,
                        사원코드 = item.EMP_CD,
                        발주번호 = item.PO_NB,
                        발주일 = item.PO_DT,
                        거래처코드 = item.TR_CD,
                        거래처명 = item.TR_NM,
                        거래구분 = item.PO_FG,
                        검사구분 = item.QC_FG,
                        과세구분 = item.VAT_FG,
                        과세구분명 = item.VAT_NM,
                        담당자코드 = item.PLN_CD,
                        담당자명 = item.PLN_NM,
                        비고 = item.REMARK_DC,
                        발주순번 = item.PO_SQ,
                        품번 = item.ITEM_CD,
                        품명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        관리단위 = item.UNITMANG_DC,
                        납기일 = item.DUE_DT,
                        출하예정일 = item.SHIPREQ_DT,
                        발주수량 = item.PO_QT,
                        발주단가 = item.PO_UM,
                        공급가 = item.POG_AM,
                        부가세 = item.POGV_AM1,
                        합계액 = item.POGH_AM1,
                        관리구분코드 = item.MGMT_CD,
                        관리구분명 = item.MGM_NM,
                        프로젝트 = item.PJT_CD,
                        프록젝트명 = item.PJT_NM,
                        비고_내역 = item.REMARK_DC_D,
                        환종 = item.EXCH_CD,
                        부가세구분 = item.UMVAT_FG,
                    };
                }
                

                발주서정보List.Add(발주서별수입);
            }
            var 발주서 = 발주서정보List.OrderByDescending(x => x.CreateTime).ToList();
            return Task.FromResult(발주서.AsEnumerable());
        }


        public Task<발주서별수입검사> 발주서별발주번호수입검사_조회Dz(string 회사코드, string 발주번호, decimal 발주순번)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scopeDz = dcp.GetDbContextScopeDZ();
            var dcDz = scopeDz.DbContext;

            var list = dcDz.VL_MES_PO
                 .Where(x => x.CO_CD == 회사코드 && x.PO_NB == 발주번호 && x.PO_SQ == 발주순번)
                 .ToList();

            var 발주서 = dc.발주서별수입검사
                .Where(x => x.회사코드 == 회사코드 && x.발주번호 == 발주번호 && x.발주순번 == 발주순번).FirstOrDefault();

            발주서별수입검사 발주서수입 = null;
            //발주서정보
            if (발주서 != null)
                발주서수입 = 발주서;
            else
            {
                foreach (var item in list)
                {
                    발주서수입 = new 발주서별수입검사
                    {
                        회사코드 = item.CO_CD,
                        사업장코드 = item.DIV_CD,
                        부서코드 = item.DEPT_CD,
                        사원코드 = item.EMP_CD,
                        발주번호 = item.PO_NB,
                        발주일 = item.PO_DT,
                        거래처코드 = item.TR_CD,
                        거래처명 = item.TR_NM,
                        거래구분 = item.PO_FG,
                        검사구분 = item.QC_FG,
                        과세구분 = item.VAT_FG,
                        과세구분명 = item.VAT_NM,
                        담당자코드 = item.PLN_CD,
                        담당자명 = item.PLN_NM,
                        비고 = item.REMARK_DC,
                        발주순번 = item.PO_SQ,
                        품번 = item.ITEM_CD,
                        품명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        관리단위 = item.UNITMANG_DC,
                        납기일 = item.DUE_DT,
                        출하예정일 = item.SHIPREQ_DT,
                        발주수량 = item.PO_QT,
                        발주단가 = item.PO_UM,
                        공급가 = item.POG_AM,
                        부가세 = item.POGV_AM1,
                        합계액 = item.POGH_AM1,
                        관리구분코드 = item.MGMT_CD,
                        관리구분명 = item.MGM_NM,
                        프로젝트 = item.PJT_CD,
                        프록젝트명 = item.PJT_NM,
                        비고_내역 = item.REMARK_DC_D,
                        환종 = item.EXCH_CD,
                        부가세구분 = item.UMVAT_FG,

                    };

                }
            }

            return Task.FromResult(발주서수입);
        }










        // 수입검사 입고처리

        public Task<bool> 수입검사입고처리_등록(수입실적등록정보 수입실적등록)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;

            try
            {
                DateTime 보유년월일 = DateTime.Now;
                var 보유품목 = dc.보유품목정보.Where(x => x.품목코드 == 수입실적등록.품번 && x.회사코드 == 수입실적등록.회사코드).FirstOrDefault();
                var 품목정보 = dc.품목정보.Where(x => x.품목코드 == 수입실적등록.품번).FirstOrDefault();
                if (보유품목 == null)
                {
                    var info = new 보유품목정보
                    {
                        회사코드 = 수입실적등록.회사코드,
                        보유품목코드 = 수입실적등록.품번,
                        품목코드 = 수입실적등록.품번,
                        보유년월일 = 보유년월일.ToString("yyyyMMdd"),
                        조정년월일 = null,
                        보유일 = 보유년월일,
                        순번 = 1,
                        수량 = 수입실적등록.실적수량,
                        실제수량 = 수입실적등록.실적수량,
                        품목구분코드 = 품목정보 != null ? 품목정보.품목구분코드 : null,
                        장소코드 = 수입실적등록.이동공정_입고창고코드,
                        장소위치코드 = $"{수입실적등록.이동공정_입고창고코드}{수입실적등록.이동작업장_입고장소코드}",
                        LOT번호 = 수입실적등록.LOT번호,
                        품목_LOT번호 = $"{수입실적등록.품번}:{수입실적등록.LOT번호}"
                    };

                    dc.보유품목정보.Add(info);
                }
                else
                {
                    보유품목.LOT번호 = 수입실적등록.LOT번호;
                    보유품목.품목_LOT번호 = $"{수입실적등록.품번}:{수입실적등록.LOT번호}";
                    보유품목.장소코드 = 수입실적등록.이동공정_입고창고코드;
                    보유품목.장소위치코드 = $"{수입실적등록.이동공정_입고창고코드}{수입실적등록.이동작업장_입고장소코드}";
                    보유품목.수량 = 보유품목.수량 + 수입실적등록.실적수량;
                    dc.보유품목정보.Update(보유품목);
                }

                dc.SaveChanges();

            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }
            //위치처리
            try
            {
                var 장소위치코드 = $"{수입실적등록.이동공정_입고창고코드}{수입실적등록.이동작업장_입고장소코드}";
                var 보유품목2 = dc.보유품목정보.FirstOrDefault(x => x.보유품목코드 == 수입실적등록.품번);
                // 보유품목이 없을 경우 무시한다.
                if (보유품목2 == default)
                    return Task.FromResult(result);

                var info = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 수입실적등록.품번 && x.장소위치코드 == 장소위치코드);


                if (info == default)
                {
                    info = new 보유품목위치정보
                    {
                        회사코드 = 수입실적등록.회사코드,
                        보유품목코드 = 수입실적등록.품번,
                        장소위치코드 = 장소위치코드,
                        수량 = 수입실적등록.실적수량,
                        LOT번호 = 수입실적등록.LOT번호,
                        품목_LOT번호 = $"{수입실적등록.품번}:{수입실적등록.LOT번호}",

                    };
                    dc.보유품목위치정보.Add(info);
                }
                // 변경
                else
                {
                    info.수량 = info.수량 + 수입실적등록.실적수량;
                    info.LOT번호 = 수입실적등록.LOT번호;
                    info.품목_LOT번호 = $"{수입실적등록.품번}:{수입실적등록.LOT번호}";
                    dc.보유품목위치정보.Update(info);
                }

                dc.SaveChanges();

                var 보유이력 = dc.보유품목이력.FirstOrDefault(x => x.보유품목코드 == 수입실적등록.품번 && x.장소위치코드 == 장소위치코드);

                var log = new 보유품목이력
                {
                    회사코드 = 수입실적등록.회사코드,
                    보유품목코드 = 수입실적등록.품번,
                    연계보유품목코드 = 수입실적등록.품번,
                    변경유형코드 = "B1701",    // 입고
                    장소코드 = 수입실적등록.이동공정_입고창고코드,
                    장소위치코드 = 장소위치코드,
                    변경수량 = 수입실적등록.실적수량,
                    변경사유 = "수입입고",
                    유형사유 = "B1701",
                    변경일시 = DateTime.Now,
                    LOT번호 = 수입실적등록.LOT번호,
                    품목_LOT번호 = $"{수입실적등록.품번}:{수입실적등록.LOT번호}",
                };
                //이동이아닌경우 업데이트 이동인경우 ADD
                if (보유이력 == null)
                    dc.보유품목이력.Add(log);


                dc.SaveChanges();

                // 2021.05.31
                var 바코드발급 = dc.바코드발급정보
                    .Include(x => x.품목)
                    .Where(x => x.품목코드 == 수입실적등록.품번 && x.LOT번호 == 수입실적등록.LOT번호 && x.회사코드 == 수입실적등록.회사코드)
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
                return Task.FromResult(result);
            }
            #region 불량품 창고넣기
            //불량처리
            try
            {
                string 불량장소위치 = "10001009";
                var 불량조회 = dc.보유품목위치정보.FirstOrDefault(x => x.보유품목코드 == 수입실적등록.품번 && x.장소위치코드 == 불량장소위치);

                if (불량조회 == default)
                {
                    불량조회 = new 보유품목위치정보
                    {
                        회사코드 = 수입실적등록.회사코드,
                        보유품목코드 = 수입실적등록.품번,
                        장소위치코드 = 불량장소위치,
                        수량 = 수입실적등록.불량수량,
                        LOT번호 = 수입실적등록.LOT번호,
                        품목_LOT번호 = $"{수입실적등록.품번}:{수입실적등록.LOT번호}",


                    };
                    dc.보유품목위치정보.Add(불량조회);
                }
                // 변경
                else
                {
                    불량조회.수량 = 불량조회.수량 + 수입실적등록.불량수량;
                    불량조회.LOT번호 = 수입실적등록.LOT번호;
                    불량조회.품목_LOT번호 = $"{수입실적등록.품번}:{수입실적등록.LOT번호}";
                    dc.보유품목위치정보.Update(불량조회);
                }

                dc.SaveChanges();
            }
            catch (Exception)
            {
                result = false;
                return Task.FromResult(result);
            }
            #endregion

            try
            {
                var 발주서완료 = dc.발주서별수입검사.Where(x => x.발주번호 == 수입실적등록.발주번호 && x.발주순번 == 수입실적등록.발주순번 && x.회사코드 == 수입실적등록.회사코드).FirstOrDefault();

                if (발주서완료 != null)
                {
                    발주서완료.품질검사완료여부 = "1";
                    발주서완료.입고여부 = "1";
                    발주서완료.실행상태코드 = "B2004";
                    발주서완료.검사수량 = 수입실적등록.검사수량;
                    발주서완료.합격수량 = 수입실적등록.합격수량;
                    발주서완료.실적수량 = 수입실적등록.실적수량;

                    dc.발주서별수입검사.Update(발주서완료);
                }

                dc.SaveChanges();
            }

            catch (Exception)
            {

                result = false;
                return Task.FromResult(result);
            }

            #region 더존 입고처리
            try
            {
                더존입고처리_입고헤더_등록(수입실적등록);



            }
            catch (Exception)
            {
                result = false;
                return Task.FromResult(result);
            }
            #endregion


            result = true;
            return Task.FromResult(result);
        }


        public Task<bool> 더존입고처리_입고헤더_등록(수입실적등록정보 수입실적등록)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;
            bool result = false;

            //var 입고추가수정유무 = dc.입고처리헤더정보.Count(x => x.회사코드 == 수입실적등록.회사코드 && x.작업번호 == 입고처리.작업번호);
            //var BARPLUS_LSTOCK = dc_D.BARPLUS_LSTOCK.Where(x => x.CO_CD == 수입실적등록.회사코드 && x.WORK_NB == 입고처리.작업번호).FirstOrDefault();


            var 순번 = dc.입고처리헤더정보.Count(x => x.회사코드 == 수입실적등록.회사코드) + 1;

            var 발주서 = dc_D.VL_MES_PO
                           .Where(x => x.CO_CD == 수입실적등록.회사코드 && x.PO_NB == 수입실적등록.발주번호 && x.PO_SQ == 수입실적등록.발주순번)
                           .FirstOrDefault();

            try
            {

                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");

                string 작업번호 = $"{"RV"}{yyyy}{순번:000000}";

                var 입고처리헤더정보 = new 입고처리헤더정보
                {
                    #region BARPLUS_LSTOCK
                    회사코드 = 수입실적등록.회사코드,  //	회사코드
                    작업번호 = 작업번호,
                    작업일자 = 수입실적등록.실적일자,  //	작업일자
                    입고구분 = "0",                                    //	입고구분
                    거래처코드 = 발주서.TR_CD,          //	거래처코드
                    입고일자 = 수입실적등록.실적일자,   //	입고일자
                    입고창고 = 수입실적등록.이동공정_입고창고코드,          //	입고창고
                    입고장소 = 수입실적등록.이동작업장_입고장소코드,         //	입고장소
                    발주번호 = 수입실적등록.발주번호,   //	발주번호
                    거래구분 = 발주서.PO_FG,//	거래구분
                    환종 = 발주서.EXCH_CD,//	환종
                    환율 = 1,//	환율
                    LC여부 = "",//	LC여부
                    사원코드 = 수입실적등록.사원코드,  //	사원코드
                    부서코드 = 수입실적등록.부서코드,  //	부서코드
                    사업장코드 = 발주서.DIV_CD, //	사업장코드
                    프로젝트코드 = 발주서.PJT_CD,//	프로젝트코드
                    과세구분 = 발주서.VAT_FG, //	과세구분
                    작업구분 = "", //	작업구분
                    관리구분코드 = 발주서.MGMT_CD, //	관리구분코드
                    EXCST_NB = "", //
                    배부여부 = "", //	배부여부
                    비고 = "", //	비고
                    최초입력사원코드 = "", //	최초입력사원코드
                    최초입력일 = null,   //	최초입력일
                    최초입력IP = "", //	최초입력IP
                    수정사원코드 = "", //	수정사원코드
                    수정일 = null,   //	수정일
                    수정IP = "", //	수정IP
                    DUMMY1 = "",//
                    DUMMY2 = "",    //
                    DUMMY3 = "",//
                    PLN_CD = "",    //
                    SO_NB3 = "",    //
                    UMVAT_FG = "0",     //
                    APP_FG = "0",  //

                    #endregion
                };


                //ERP  BARPLUS_LSTOCK

                var BARPLUS_LSTOCK = new BARPLUS_LSTOCK
                {
                    #region BARPLUS_LSTOCK

                    CO_CD = 수입실적등록.회사코드,  //	회사코드
                    WORK_NB = 작업번호,       //	작업번호
                    WORK_DT = string.Format("{0:yyyyMMdd}", 수입실적등록.실적일자),  //	작업일자
                    RCV_FG = "0",                                    //	입고구분
                    TR_CD = 발주서.TR_CD,          //	거래처코드
                    RCV_DT = string.Format("{0:yyyyMMdd}", 수입실적등록.실적일자),   //	입고일자
                    WH_CD = 수입실적등록.이동공정_입고창고코드,         //	입고창고
                    LC_CD = 수입실적등록.이동작업장_입고장소코드,          //	입고장소
                    PO_NB = 발주서.PO_NB, //	발주번호
                    PO_FG = 발주서.PO_FG,//	거래구분
                    EXCH_CD = 발주서.EXCH_CD,//	환종
                    EXCH_RT = 1,//	환율
                    LC_YN = "",//	LC여부

                    EMP_CD = 수입실적등록.사원코드,   //	사원코드
                    DEPT_CD = 수입실적등록.부서코드,  //	부서코드
                    DIV_CD = 발주서.DIV_CD, //	사업장코드
                    PJT_CD = 발주서.PJT_CD,//	프로젝트코드
                    VAT_FG = 발주서.VAT_FG, //	과세구분
                    MAP_FG = "", //	작업구분
                    MGMT_CD = "", //	관리구분코드
                    EXCST_NB = "", //
                    DIST_YN = "", //	배부여부
                    REMARK_DC = "", //	비고
                    INSERT_ID = "", //	최초입력사원코드
                    INSERT_DT = null,   //	최초입력일
                    INSERT_IP = "", //	최초입력IP
                    MODIFY_ID = "", //	수정사원코드
                    MODIFY_DT = null,   //	수정일
                    MODIFY_IP = "", //	수정IP
                    DUMMY1 = "",//
                    DUMMY2 = "",    //
                    DUMMY3 = "",//
                    PLN_CD = "",    //
                    SO_NB3 = "",    //
                    UMVAT_FG = "0",     //
                    APP_FG = "0",  //

                    #endregion

                };

                dc.입고처리헤더정보.Add(입고처리헤더정보);

                dc_D.BARPLUS_LSTOCK.Add(BARPLUS_LSTOCK);


                dc.SaveChanges();

                dc_D.SaveChanges();

                Task.Delay(300);
                더존입고처리_입고상세_등록(수입실적등록, BARPLUS_LSTOCK.WORK_NB);


                #region 입고처리상세정보

                /*
                var 작업순번 = dc.입고처리상세정보.Count(x => x.회사코드 == 수입실적등록.회사코드 && x.작업번호 == 작업번호) + 1;
                var 입고처리상세정보 = new 입고처리상세정보
                {
                    회사코드 = 수입실적등록.회사코드,
                    작업번호 = 작업번호,
                    작업순번  = 작업순번,
                    품번 = 수입실적등록.품번,
                    입고수량_관리단위 = 수입실적등록.실적수량 + 수입실적등록.불량수량,
                    입고수량_재고단위 = 수입실적등록.실적수량 + 수입실적등록.불량수량,
                    입고단가 = 발주서.PO_UM,
                    공급가 = 발주서.POG_AM,
                    부가세 = 발주서.POGV_AM1,
                    합계액 = 발주서.POGH_AM1,
                    환종 = 발주서.EXCH_CD,
                    환율 = 1,
                   // 외화단가 = 발주서.EXCH_UM,
                   // 외화금액 = 발주서.EXCH_AM,
                    LOT번호 = 수입실적등록.LOT번호,
                    발주번호 = 발주서.PO_NB,
                    발주순번 = 발주서.PO_SQ,

                    선적번호 = "",
                    선적순번 = 1,
                    //사용여부
                    //유효여부
                    //단가구분
                    입고장소코드 = 수입실적등록.이동작업장_입고장소코드,
                    비고 = 발주서.REMARK_DC_D,

                };
                #endregion

                #region BARPLUS_LSTOCK_D
                var BARPLUS_LSTOCK_D = new BARPLUS_LSTOCK_D
                {
                    CO_CD = 수입실적등록.회사코드,  //	회사코드	,
                    WORK_NB = 작업번호,          //	작업번호	,
                    WORK_SQ = 작업순번,    //	작업순번	,
                    ITEM_CD = 수입실적등록.품번,   //	품번	,
                    PO_QT = 수입실적등록.실적수량 + 수입실적등록.불량수량,  //	입고수량(관리단위)	,
                    RCV_QT = 수입실적등록.실적수량 + 수입실적등록.불량수량,//	입고수량(재고단위)	,
                    RCV_UM		= 발주서.PO_UM,//	입고단가	,
                    RCVG_AM		= 발주서.POG_AM,//	공급가	,
                    RCVV_AM		= 발주서.POGV_AM1, //	부가세	,
                    RCVH_AM		= 발주서.POGH_AM1,//	합계액	,
                    EXCH_CD = 발주서.EXCH_CD,
                    EXCH_RT = 1,
                   // EXCH_UM   =  발주서.EXCH_UM,//	외화단가	,
                   // EXCH_AM	  = 발주서.EXCH_AM,//	외화금액	,
                    LOT_NB	= 수입실적등록.LOT번호,	//	LOT번호	,

                    PO_NB = 발주서.PO_NB,
                    PO_SQ = 발주서.PO_SQ,
                    //REQ_NB		//	입고의뢰번호	,
                    //REQ_SQ		//	입고의뢰순번	,
                    //IBL_NB		//	선적번호	,
                    //IBL_SQ		//	선적순번	,
                    USE_YN = "0",//	사용여부	,
                    EXPIRE_YN = "Y",  //	유효여부	,
                    //UM_FG		//	단가구분	,
                    CONF_NB3 = 0,
                    LC_CD = 수입실적등록.이동작업장_입고장소코드, //	입고장소코드	,
                    REMARK_DC = 발주서.REMARK_DC_D,                      //	비고	,
                    APP_FG = "0",
                };
                
                

                dc.입고처리상세정보.Add(입고처리상세정보);

                dc_D.BARPLUS_LSTOCK_D.Add(BARPLUS_LSTOCK_D);

                dc.SaveChanges();

                dc_D.SaveChanges();
                */
                #endregion
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }



            result = true;

            return Task.FromResult(result);
        }


        public Task<bool> 더존입고처리_입고상세_등록(수입실적등록정보 수입실적등록, string 작업번호)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;
            bool result = false;

            //var 입고추가수정유무 = dc.입고처리헤더정보.Count(x => x.회사코드 == 수입실적등록.회사코드 && x.작업번호 == 입고처리.작업번호);
            var BARPLUS_헤더 = dc_D.BARPLUS_LSTOCK.Where(x => x.CO_CD == 수입실적등록.회사코드 && x.WORK_NB == 작업번호).FirstOrDefault();

            var 발주서 = dc_D.VL_MES_PO
                           .Where(x => x.CO_CD == 수입실적등록.회사코드 && x.PO_NB == 수입실적등록.발주번호 && x.PO_SQ == 수입실적등록.발주순번)
                           .FirstOrDefault();

            try
            {

                var now = DateTime.Now;
                var yyyy = now.ToString("yyyy");

                var 작업순번 = dc.입고처리상세정보.Count(x => x.회사코드 == 수입실적등록.회사코드 && x.작업번호 == BARPLUS_헤더.WORK_NB) + 1;


                #region 입고처리상세정보
                var 입고처리상세정보 = new 입고처리상세정보
                {
                    회사코드 = 수입실적등록.회사코드,
                    작업번호 = 작업번호,
                    작업순번 = 작업순번,
                    품번 = 수입실적등록.품번,
                    입고수량_관리단위 = 수입실적등록.실적수량 + 수입실적등록.불량수량,
                    입고수량_재고단위 = 수입실적등록.실적수량 + 수입실적등록.불량수량,
                    입고단가 = 발주서.PO_UM,
                    공급가 = 발주서.POG_AM,
                    부가세 = 발주서.POGV_AM1,
                    합계액 = 발주서.POGH_AM1,
                    환종 = 발주서.EXCH_CD,
                    환율 = 1,
                    // 외화단가 = 발주서.EXCH_UM,
                    // 외화금액 = 발주서.EXCH_AM,
                    LOT번호 = 수입실적등록.LOT번호,
                    발주번호 = 발주서.PO_NB,
                    발주순번 = 발주서.PO_SQ,

                    선적번호 = "",
                    선적순번 = 1,
                    //사용여부
                    //유효여부
                    //단가구분
                    입고장소코드 = 수입실적등록.이동작업장_입고장소코드,
                    비고 = 발주서.REMARK_DC_D,

                };
                #endregion

                #region BARPLUS_LSTOCK_D
                var BARPLUS_LSTOCK_D = new BARPLUS_LSTOCK_D
                {
                    CO_CD = 수입실적등록.회사코드,  //	회사코드	,
                    WORK_NB = 작업번호,          //	작업번호	,
                    WORK_SQ = 작업순번,    //	작업순번	,
                    ITEM_CD = 수입실적등록.품번,   //	품번	,
                    PO_QT = 수입실적등록.실적수량 + 수입실적등록.불량수량,  //	입고수량(관리단위)	,
                    RCV_QT = 수입실적등록.실적수량 + 수입실적등록.불량수량,//	입고수량(재고단위)	,
                    RCV_UM = 발주서.PO_UM,//	입고단가	,
                    RCVG_AM = 발주서.POG_AM,//	공급가	,
                    RCVV_AM = 발주서.POGV_AM1, //	부가세	,
                    RCVH_AM = 발주서.POGH_AM1,//	합계액	,
                    EXCH_CD = 발주서.EXCH_CD,
                    EXCH_RT = 1,
                    // EXCH_UM   =  발주서.EXCH_UM,//	외화단가	,
                    // EXCH_AM	  = 발주서.EXCH_AM,//	외화금액	,
                    LOT_NB = 수입실적등록.LOT번호,	//	LOT번호	,

                    PO_NB = 발주서.PO_NB,
                    PO_SQ = 발주서.PO_SQ,
                    //REQ_NB		//	입고의뢰번호	,
                    //REQ_SQ		//	입고의뢰순번	,
                    //IBL_NB		//	선적번호	,
                    //IBL_SQ		//	선적순번	,
                    USE_YN = "1",//	사용여부	,
                    EXPIRE_YN = "1",  //	유효여부	,
                    //UM_FG		//	단가구분	,
                    CONF_NB3 = 0,
                    LC_CD = 수입실적등록.이동작업장_입고장소코드, //	입고장소코드	,
                    REMARK_DC = 발주서.REMARK_DC_D,                      //	비고	,
                    APP_FG = "0",
                };
                #endregion

                dc.입고처리상세정보.Add(입고처리상세정보);

                dc_D.BARPLUS_LSTOCK_D.Add(BARPLUS_LSTOCK_D);

                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }




        public Task<IEnumerable<공정단위자재현황>> 작업지시공정단위자재현황2_조회(string 회사코드, string 공정단위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.공정단위자재정보
                .Where(x => x.회사코드 == 회사코드 && x.공정단위코드 == 공정단위코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();
            List<공정단위자재현황> list = new List<공정단위자재현황>();
            foreach (var item in result)
            {
                var 공정단위자재 = new 공정단위자재현황
                {
                    회사코드 = item.회사코드,
                    공정단위코드 = item.공정단위코드,
                    자재코드 = item.자재코드,
                    필요수량 = item.수량,
                };
                list.Add(공정단위자재);
            }

            return Task.FromResult(list.AsEnumerable());

        }







        public Task<List<생산실적헤더정보>> 작업지시상세_생산실적헤더정보_조회(string 회사코드, string 생산지시코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산실적헤더정보
                .Where(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드)
                .Where_미삭제_사용()
                .Order_등록최신().ToList();

            return Task.FromResult(result);

        }




        public Task<List<외주생산위치정보>> 외주생산위치정보_조회( string 회사코드, string 지시번호)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var list = dc.외주생산위치정보
                .Where(x => x.지시서 == 지시번호 && x.회사코드 == 회사코드)
                .Where_미삭제_사용()
                .Order_등록최신()
                .ToList();


            return Task.FromResult(list);
        }


        public Task<외주생산위치정보> 외주생산위치품목_조회(string 회사코드, string 지시번호, string 품목코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            var result = dc.외주생산위치정보
                .Where(x => x.지시서 == 지시번호 && x.회사코드 == 회사코드 && x.보유품목코드 == 품목코드).FirstOrDefault();
               

            return Task.FromResult(result);
        }






        public Task<string> 반입처리_재고이동헤더정보_등록(재고이동헤더정보 재고이동)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            //var 출고추가수정유무 = dc.출고처리헤더정보.Count(x => x.작업번호 == 출고처리.작업번호);
            var 순번 = dc.재고이동헤더정보.Count(x => x.회사코드 == 재고이동.회사코드) + 1;

            //var 더존출고추가수정유무 = dc.BARPLUS_LDELIVER.Count(x => x.WORK_NB == 출고처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LSTKMOVE.Count(x => x.CO_CD == 재고이동.회사코드) + 1;
            var now = DateTime.Now;
            var yyyy = now.ToString("yyyy");
            string 작업번호1 = "";
            작업번호1 = $"{"MV"}{yyyy}{순번:000000}";
            try
            {
             

                var BARPLUS_LSTKMOVE = new BARPLUS_LSTKMOVE
                {
                    CO_CD = 재고이동.회사코드,
                    WORK_NB = 작업번호1,
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

                재고이동.작업번호 = 작업번호1;
                dc.재고이동헤더정보.Add(재고이동);
                dc_D.BARPLUS_LSTKMOVE.Add(BARPLUS_LSTKMOVE);
                

                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(작업번호1);
            }


            result = true;

            return Task.FromResult(작업번호1);
        }

        public Task<bool> 반입처리_재고이동상세정보_등록(재고이동상세정보 이동처리, decimal 필요수량)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.재고이동상세정보.Count(x => x.회사코드 == 이동처리.회사코드 &&
                                        x.작업번호 == 이동처리.작업번호) + 1;
            //var 순번 = dc.출고처리상세정보.Count() + 1;
            //var 순번_더존 = dc_D.BARPLUS_LSTKMOVE_D.Count(x => x.CO_CD == 이동처리.회사코드 &&    x.WORK_NB == 이동처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LDELIVER_D.Count() + 1;

            try
            {
                var 재고이동헤더 = dc.재고이동헤더정보.Where(x => x.회사코드 == 이동처리.회사코드 && x.작업번호 == 이동처리.작업번호).FirstOrDefault();
                string 이동할장소위치코드 = $"{재고이동헤더.입고공정_창고코드}{재고이동헤더.입고작업장_장소코드}";
                string 이동할위치상세코드 = 재고이동헤더.입고장소위치상세코드;

                var 보유품목정보 = dc.보유품목정보.Where(x => x.품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드).FirstOrDefault();
                var 현장소위치코드 = 보유품목정보.장소위치코드;

                var 보유품목현위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드 && x.장소위치코드 == 현장소위치코드).FirstOrDefault();

                var 보유품목이동위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드 && x.장소위치코드 == 이동할장소위치코드).FirstOrDefault();

                /*
                if (보유품목정보?.수량 < 이동처리.이동수량)
                    return Task.FromResult(result);

                if (보유품목현위치?.수량 < 이동처리.이동수량)
                    return Task.FromResult(result);
                */

                이동처리.재공운영여부 = "1";

                var BARPLUS_LSTKMOVE_D = new BARPLUS_LSTKMOVE_D
                {
                    CO_CD = 이동처리.회사코드,
                    WORK_NB = 이동처리.작업번호,
                    WORK_SQ = 순번,
                    ITEM_CD = 이동처리.품번,
                    REQ_QT = 이동처리.청구수량,
                    MOVE_QT = 이동처리.이동수량,
                    WIP_YN = 이동처리.재공운영여부,
                    APP_FG = 이동처리.APP_FG,
                    USE_YN = 이동처리.사용여부,
                    EXPIRE_YN = 이동처리.만료여부,
                    LOT_NB =  이동처리.LOT번호,
                };
                string 이동구분 = "";
                decimal 사용수량 = (이동처리.이동수량 / 필요수량);
                // 재고이동 내부이동
                if (재고이동헤더.이동구분 != "")
                {
                    if (보유품목정보 != null)
                    {
                        //보유품목정보.장소코드 = 재고이동헤더.입고공정_창고코드;
                        //보유품목정보.장소위치코드 = 이동할장소위치코드;
                        보유품목정보.수량 = 보유품목정보.수량 + 사용수량;
                        dc.보유품목정보.Update(보유품목정보);
                        dc.SaveChanges();
                    }
                    if (보유품목현위치 != null)
                    {
                        if (보유품목이동위치 == null)
                        {
                            var info = new 보유품목위치정보
                            {
                                회사코드 = 이동처리.회사코드,
                                보유품목코드 = 이동처리.품번,
                                장소위치코드 = 이동할장소위치코드,
                                //위치상세코드 = 이동할위치상세코드,
                                수량 = 사용수량,
                                LOT번호 = 이동처리.LOT번호,
                                품목_LOT번호 =  이동처리.품목_LOT번호,
                            };
                            dc.보유품목위치정보.Add(info);
                            dc.SaveChanges();

                        }
                        else
                        {
                            보유품목이동위치.회사코드 = 이동처리.회사코드;
                            보유품목이동위치.보유품목코드 = 이동처리.품번;
                            보유품목이동위치.장소위치코드 = 이동할장소위치코드;
                            //보유품목이동위치.위치상세코드 = 이동할위치상세코드;
                            보유품목이동위치.수량 = 보유품목이동위치.수량 + 사용수량;
                            보유품목이동위치.LOT번호 = 이동처리.LOT번호;
                            보유품목이동위치.품목_LOT번호 = 이동처리.품목_LOT번호;

                            dc.보유품목위치정보.Update(보유품목이동위치);
                            dc.SaveChanges();
                        }

                    }

                    var 보유품목이력 = new 보유품목이력
                    {
                        회사코드 = 이동처리.회사코드,
                        보유품목코드 = 이동처리.품번,
                        연계보유품목코드 = 이동처리.품번,
                        변경유형코드 = "B1705",    // 입고
                        장소코드 = 재고이동헤더.입고공정_창고코드,
                        장소위치코드 = 이동할장소위치코드,
                        //위치상세코드 = 이동할위치상세코드,
                        변경수량 = 사용수량,
                        변경사유 = "반입처리",
                        유형사유 = "B1705",
                        변경일시 = DateTime.Now,
                        LOT번호 = 이동처리.LOT번호,
                        품목_LOT번호 =  이동처리.품목_LOT번호,
                    };
                    dc.보유품목이력.Add(보유품목이력);
                    dc.SaveChanges();
                }


                이동처리.작업순번 = 순번;
                dc.재고이동상세정보.Add(이동처리);
                dc_D.BARPLUS_LSTKMOVE_D.Add(BARPLUS_LSTKMOVE_D);

                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }
           

            result = true;

            return Task.FromResult(result);
        }





        public Task<bool> 불량반입처리_재고이동상세정보_등록(재고이동상세정보 이동처리, decimal 필요수량)
        {

            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            using var scope_D = dcp.GetDbContextScopeDZ();
            var dc_D = scope_D.DbContext;

            bool result = false;

            var 순번 = dc.재고이동상세정보.Count(x => x.회사코드 == 이동처리.회사코드 &&
                                        x.작업번호 == 이동처리.작업번호) + 1;
            //var 순번 = dc.출고처리상세정보.Count() + 1;
            //var 순번_더존 = dc_D.BARPLUS_LSTKMOVE_D.Count(x => x.CO_CD == 이동처리.회사코드 &&    x.WORK_NB == 이동처리.작업번호);
            //var 순번_더존 = dc_D.BARPLUS_LDELIVER_D.Count() + 1;

            try
            {
                var 재고이동헤더 = dc.재고이동헤더정보.Where(x => x.회사코드 == 이동처리.회사코드 && x.작업번호 == 이동처리.작업번호).FirstOrDefault();
                string 이동할장소위치코드 = $"{재고이동헤더.입고공정_창고코드}{재고이동헤더.입고작업장_장소코드}";
                string 이동할위치상세코드 = 재고이동헤더.입고장소위치상세코드;

                var 보유품목정보 = dc.보유품목정보.Where(x => x.품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드).FirstOrDefault();
                var 현장소위치코드 = 보유품목정보.장소위치코드;

                var 보유품목현위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드 && x.장소위치코드 == 현장소위치코드).FirstOrDefault();

                var 보유품목이동위치 = dc.보유품목위치정보.Where(x => x.보유품목코드 == 이동처리.품번 && x.회사코드 == 이동처리.회사코드 && x.장소위치코드 == 이동할장소위치코드).FirstOrDefault();

                /*
                if (보유품목정보?.수량 < 이동처리.이동수량)
                    return Task.FromResult(result);

                if (보유품목현위치?.수량 < 이동처리.이동수량)
                    return Task.FromResult(result);
                */

                이동처리.재공운영여부 = "1";

                var BARPLUS_LSTKMOVE_D = new BARPLUS_LSTKMOVE_D
                {
                    CO_CD = 이동처리.회사코드,
                    WORK_NB = 이동처리.작업번호,
                    WORK_SQ = 순번,
                    ITEM_CD = 이동처리.품번,
                    REQ_QT = 이동처리.청구수량,
                    MOVE_QT = 이동처리.이동수량,
                    WIP_YN = 이동처리.재공운영여부,
                    APP_FG = 이동처리.APP_FG,
                    USE_YN = 이동처리.사용여부,
                    EXPIRE_YN = 이동처리.만료여부,
                    LOT_NB = 이동처리.LOT번호,
                };
                string 이동구분 = "";
                decimal 사용수량 = (Math.Abs(이동처리.이동수량) / 필요수량);
                // 재고이동 내부이동
                if (재고이동헤더.이동구분 != "")
                {
                    if (보유품목정보 != null)
                    {
                        //보유품목정보.장소코드 = 재고이동헤더.입고공정_창고코드;
                        //보유품목정보.장소위치코드 = 이동할장소위치코드;
                        //보유품목정보.수량 = 보유품목정보.수량 + 사용수량;  
                        //dc.보유품목정보.Update(보유품목정보);
                        //dc.SaveChanges();
                    }
                    if (보유품목현위치 != null)
                    {
                        if (보유품목이동위치 == null)
                        {
                            var info = new 보유품목위치정보
                            {
                                회사코드 = 이동처리.회사코드,
                                보유품목코드 = 이동처리.품번,
                                장소위치코드 = 이동할장소위치코드,
                                //위치상세코드 = 이동할위치상세코드,
                                수량 = 사용수량,
                                LOT번호 = 이동처리.LOT번호,
                                품목_LOT번호 = 이동처리.품목_LOT번호,
                            };
                            dc.보유품목위치정보.Add(info);
                            dc.SaveChanges();

                        }
                        else
                        {
                            보유품목이동위치.회사코드 = 이동처리.회사코드;
                            보유품목이동위치.보유품목코드 = 이동처리.품번;
                            보유품목이동위치.장소위치코드 = 이동할장소위치코드;
                            //보유품목이동위치.위치상세코드 = 이동할위치상세코드;
                            보유품목이동위치.수량 = 보유품목이동위치.수량 + 사용수량;
                            보유품목이동위치.LOT번호 = 이동처리.LOT번호;
                            보유품목이동위치.품목_LOT번호 = 이동처리.품목_LOT번호;

                            dc.보유품목위치정보.Update(보유품목이동위치);
                            dc.SaveChanges();
                        }

                    }

                    //var 보유품목이력 = new 보유품목이력
                    //{
                    //    회사코드 = 이동처리.회사코드,
                    //    보유품목코드 = 이동처리.품번,
                    //    연계보유품목코드 = 이동처리.품번,
                    //    변경유형코드 = "B1705",    // 입고
                    //    장소코드 = 재고이동헤더.입고공정_창고코드,
                    //    장소위치코드 = 이동할장소위치코드,
                    //    //위치상세코드 = 이동할위치상세코드,
                    //    변경수량 = 사용수량,
                    //    변경사유 = "반입처리",
                    //    유형사유 = "B1705",
                    //    변경일시 = DateTime.Now,
                    //    LOT번호 = 이동처리.LOT번호,
                    //    품목_LOT번호 = 이동처리.품목_LOT번호,
                    //};
                    //dc.보유품목이력.Add(보유품목이력);
                    //dc.SaveChanges();
                }


                이동처리.작업순번 = 순번;
                dc.재고이동상세정보.Add(이동처리);
                dc_D.BARPLUS_LSTKMOVE_D.Add(BARPLUS_LSTKMOVE_D);

                dc.SaveChanges();

                dc_D.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                return Task.FromResult(result);
            }


            result = true;

            return Task.FromResult(result);
        }




        public Task<생산실적헤더정보> 공정단위생산실적헤더정보_조회(string 회사코드,string 생산지시코드, string 공정단위코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;

            var result = dc.생산실적헤더정보
                .Where(x => x.회사코드 == 회사코드 && x.생산지시코드 == 생산지시코드 && x.공정단위코드 == 공정단위코드 ).FirstOrDefault();
              

            return Task.FromResult(result);

        }



        public Task<IEnumerable<직원정보>> 직원부서_조회(bool isOnlyUse, string 회사코드, string 부서코드)
        {
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            List<직원정보> list = null;
            if (부서코드 != "")
            {
                list = dc.직원정보
                .Include(x => x.부서)
                .Where(x => (isOnlyUse == true && x.사용유무 == true && x.회사코드 == 회사코드 && x.부서코드 == 부서코드) || isOnlyUse == false && x.회사코드 == 회사코드 && x.부서코드 == 부서코드)
                .Where_미삭제()
                .Order_등록최신()
                .ToList();
            }
            
            else
            {
                list = dc.직원정보
                .Include(x => x.부서)
                .Where(x => (isOnlyUse == true && x.사용유무 == true && x.회사코드 == 회사코드) || isOnlyUse == false && x.회사코드 == 회사코드 )
                .Where_미삭제()
                .Order_등록최신()
                .ToList();
            }
            return Task.FromResult(list.AsEnumerable());
        }


        /// <summary>
        /// //2021.08.17
        /// </summary>

        public Task<IEnumerable<품목정보>> VL_MES_ITEM_Upload()
        {
            using var scopeDZ = dcp.GetDbContextScopeDZ();
            var dcDZ = scopeDZ.DbContext;
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            List<VL_MES_ITEM> 더존품목정보 = new List<VL_MES_ITEM>();
            //var result = dc.VL_MES_ITEM.Select(x => x)	
            //   .FirstOrDefault();	
            더존품목정보 = dcDZ.VL_MES_ITEM.Select(x => x)
                .ToList();
            bool result = false;
            try
            {

                //var sql = "DELETE FROM 품목정보";
                //dc.Database.ExecuteSqlRaw(sql);

                var 품목 = 더존품목정보.Where(x => x.CO_CD == "2265" && x.ITEM_CD != "").ToList();
                foreach (var item in 품목)
                {
                    //생산품 - > 제품  부품 -> 부재료	
                    string 단위코드 = "";
                    string 조달구분 = "";
                    string 품목구분코드 = "";
                    if (item.UNIT_DC.Contains("EA", StringComparison.OrdinalIgnoreCase))
                    {
                        단위코드 = "B1101";
                    }
                    else if (item.UNIT_DC.Contains("won", StringComparison.OrdinalIgnoreCase))
                    {
                        단위코드 = "B1102";
                    }
                    else if (item.UNIT_DC.Contains("SET", StringComparison.OrdinalIgnoreCase))
                    {
                        단위코드 = "B1101";
                    }
                    if (item.ODR_FG.Contains("0"))
                    {
                        조달구분 = "B1601";
                    }
                    else if (item.ODR_FG.Contains("1"))
                    {
                        조달구분 = "B1602";
                    }
                    else if (item.ODR_FG.Contains("2"))
                    {
                        조달구분 = "B1608";
                    }
                    //0.원재료 1.부재료 2.제품 4.반제품 5.상품 6.저장품 7.비용 8.수익	
                    if (item.ACCT_FG.Contains("0"))
                    {
                        품목구분코드 = "B1201";
                    }
                    else if (item.ACCT_FG.Contains("1"))
                    {
                        품목구분코드 = "B1202";
                    }
                    else if (item.ACCT_FG.Contains("2"))
                    {
                        품목구분코드 = "B1203";
                    }
                    //else if(item.ACCT_FG.Contains("3"))	
                    //{	
                    //    품목구분코드 = "B1204";	
                    //}	
                    else if (item.ACCT_FG.Contains("4"))
                    {
                        품목구분코드 = "B1204";
                    }
                    else if (item.ACCT_FG.Contains("5"))
                    {
                        품목구분코드 = "B1207";
                    }
                    else if (item.ACCT_FG.Contains("6"))
                    {
                        품목구분코드 = "B1208";
                    }
                    else if (item.ACCT_FG.Contains("7"))
                    {
                        품목구분코드 = "B1206";
                    }
                    else if (item.ACCT_FG.Contains("8"))
                    {
                        품목구분코드 = "B1209";
                    }
                    var 품목정보 = new 품목정보()
                    {
                        회사코드 = item.CO_CD,
                        품목코드 = item.ITEM_CD,
                        원품목코드 = item.ITEM_CD,
                        품목명 = item.ITEM_NM,
                        규격 = item.ITEM_DC,
                        품목구분코드 = 품목구분코드,
                        조달구분코드 = 조달구분,
                        단위코드 = 단위코드,
                        재고단위 = item.UNIT_DC,
                        LOT여부 = item.LOT_FG == "0" ? false : true,
                        사용유무 = item.USE_YN == "0" ? false : true,
                        거래처코드 = item.TRMAIN_CD,
                    };

                    var found = dc.품목정보.Where(x => x.품목코드 == item.ITEM_CD).FirstOrDefault();
                    if (found == null)
                        dc.품목정보.Add(품목정보);
                    else
                        dc.품목정보.Update(품목정보);
                }
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = false;
                //return Task.FromResult(result);	
            }
            var list = dc.품목정보
                .Include(x => x.거래처)
                //.Where(x => (isOnlyUse == true && x.사용유무 == true) || isOnlyUse == false)	
                .Where_미삭제()
                //.Where(x => (생산품만 == true && x.품목구분코드 == "B1203") || (생산품만 == false && (x.품목구분코드 == "B1203" || x.품목구분코드 == "B1204"))) // 생산품, 반제품	
                .OrderByDescending(x => x.CreateTime)
                .ToList();
            return Task.FromResult(list.AsEnumerable());
        }

        public Task<IEnumerable<BOM_정보>> VL_MES_BOM_Upload(string 회사코드)
        {
            using var scopeDZ = dcp.GetDbContextScopeDZ();
            var dcDZ = scopeDZ.DbContext;
            List<VL_MES_BOM> 더존BOM정보 = new List<VL_MES_BOM>();
            더존BOM정보 = dcDZ.VL_MES_BOM.Select(x => x)
                .ToList();
            using var scope = dcp.GetDbContextScope();
            var dc = scope.DbContext;
            bool result = false;
            try
            {
                var sql = "DELETE FROM BOM_정보 ";
                dc.Database.ExecuteSqlRaw(sql);

                sql = "DELETE FROM BOM정보 ";
                dc.Database.ExecuteSqlRaw(sql);
                foreach (var item in 더존BOM정보)
                {
                    var BOM_정보 = new BOM_정보()
                    {
                        회사코드 = item.CO_CD,
                        모품번 = item.ITEMPARENT_CD,
                        모품명 = item.ITEMPARENT_NM,
                        모규격 = item.ITEMPARENT_DC,
                        모품목재고단위 = item.ITEMPARENT_UNIT_DC,
                        순번 = item.BOM_SQ,
                        자품번 = item.ITEMCHILD_CD,
                        자품명 = item.ITEMCHILD_NM,
                        자규격 = item.ITEMCHILD_DC,
                        자품목재고단위 = item.ITEMCHILD_UNIT_DC,
                        정미수량 = item.JUST_QT,
                        LOSS율 = item.LOSS_RT,
                        필요수량 = item.REAL_QT,
                        외주구분 = item.OUT_FG,
                        임가공구분 = item.ODR_FG,
                        주거래처코드 = item.TRMAIN_CD,
                        주거래처명 = item.ATTR_NM,
                        시작일자 = item.START_DT,
                        종료일자 = item.END_DT,
                        사용여부 = item.USE_YN,
                    };
                    var found = dc.BOM_정보.Where(x => x.모품번 == item.ITEMPARENT_CD && x.자품번 == item.ITEMCHILD_NM && x.회사코드 == item.CO_CD).FirstOrDefault();
                    if (found == null)
                        dc.BOM_정보.Add(BOM_정보);
                    else
                        dc.BOM_정보.Update(BOM_정보);
                    //var 품목정보 = new 품목정보()	
                    //{	
                    //    회사코드 = item.CO_CD,	
                    //    품목코드 = item.ITEMPARENT_CD,	
                    //    원품목코드 = item.ITEMPARENT_CD,	
                    //    품목명 = item.ITEMPARENT_NM,	
                    //    규격 = item.ITEMPARENT_DC,	
                    //    품목구분코드 = "B1202",	
                    //    조달구분코드 = "B1601",	
                    //    단위코드 = "B1101",	
                    //    재고단위 = item.ITEMPARENT_UNIT_DC,	
                    //};	
                    //var found1 = dc.품목정보.Where(x => x.품목코드 == item.ITEMPARENT_CD).FirstOrDefault();	
                    //if (found1 == null)	
                    //    dc.품목정보.Add(품목정보);	
                    dc.SaveChanges();


                    //2021.08.17 BOM정보 table에 추가작업
                    // 트리구조 표현을 위함
                    var 상위품목코드 = item.ITEMPARENT_CD;
                    var 상위BOM순번 = dc.BOM정보.Where(x => x.품목코드 == 상위품목코드).Select(x => x.BOM순번).FirstOrDefault();
                    if (상위BOM순번 == 0)
                    {
                        var 상위BOM = new BOM정보
                        {
                            품목코드 = item.ITEMPARENT_CD,
                            상위BOM순번 = null,
                            정미수량 = 1,
                            로스율 = 0,
                            필요수량 = 1
                        };
                        dc.BOM정보.Add(상위BOM);
                        dc.SaveChanges();
                    }
                    상위BOM순번 = dc.BOM정보.Where(x => x.품목코드 == 상위품목코드).Select(x => x.BOM순번).FirstOrDefault();
                    var 자BOM = new BOM정보
                    {
                        품목코드 = item.ITEMCHILD_CD,
                        상위BOM순번 = 상위BOM순번 == 0 ? null : 상위BOM순번,
                        정미수량 = item.JUST_QT,
                        로스율 = item.LOSS_RT,
                        필요수량 = item.REAL_QT
                    };

                    dc.BOM정보.Add(자BOM);
                    dc.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result = false;
                //return Task.FromResult(result);	
            }
            //dc.SaveChanges();	
            var list = dc.BOM_정보
                 .Where(x => x.회사코드 == 회사코드)
                 .Where_미삭제_사용()
                 .Order_등록최신()
                 .ToList();
            return Task.FromResult(list.AsEnumerable());
        }

    }



}
