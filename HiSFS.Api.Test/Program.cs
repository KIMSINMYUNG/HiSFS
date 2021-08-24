using CsvHelper;

using HiSFS.Agent.Service.Devices;
using HiSFS.Api.Host.Data;
using HiSFS.Api.Shared.Models;
using HiSFS.Api.Shared.Services;
using HiSFS.Shared.Wamp;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Syncfusion.Blazor.FileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Client;
using WampSharp.V2.Fluent;

namespace HiSFS.Api.Test
{
    class Program
    {
        private static readonly int DefaultPort = 31200;
        private static readonly string DefaultServiceUri = $"ws://localhost:{DefaultPort}/";
        private static readonly string DefaultRealm = "HiSFS.Api";
        private static readonly string ConnectionString = "Server=server.maum.in,31233\\SQLEXPRESS;Database=HiSFS;User Id=HiSFSDev;Password=dev3123";

        static async Task Main(string[] args)
        {
            //await 부서_등록();
            //await 직원_등록();
            //await PrintBarcode();
            await Task.Yield();

            var builder = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(ConnectionString);
            using var dc = new ApiDbContext(builder.Options);
            dc.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var fi = new System.IO.FileInfo(@"W:\MyOrder\HiSFS\개발관련자료\기준정보생선_200928\SYX0310-BOM등록EXCEL_IMPORT처리_20191016115937(최종)0915 (1).xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(fi);
            var sheet = package.Workbook.Worksheets[0];

            var rowNum = 4;
            var c = sheet.Cells;
            while (true)
            {
                var check = c[rowNum, 1].Value;
                if (string.IsNullOrWhiteSpace($"{check}") == true)
                    break;

                //var BOM = new BOM정보
                //{
                //    품목코드 = $"{c[rowNum, 4].Value.ToString().Trim()}:1",
                //    //상위품목코드 = $"{c[rowNum, 2].Value.ToString().Trim()}:1",
                //    정미수량 = decimal.Parse(c[rowNum, 5].Value.ToString().Trim()),
                //    로스율 = (decimal)0,
                //    필요수량 = decimal.Parse(c[rowNum, 7].Value.ToString().Trim())
                //};

                var 상위품목코드 = $"{c[rowNum, 2].Value.ToString().Trim()}:1";
                var 상위BOM순번 = dc.BOM정보.Where(x => x.품목코드 == 상위품목코드).Select(x => x.BOM순번).FirstOrDefault();
                if (상위BOM순번 == 0)
                {
                    var 상위BOM = new BOM정보
                    {
                        품목코드 = $"{c[rowNum, 2].Value.ToString().Trim()}:1",
                        상위BOM순번 = null,
                        정미수량 = 1,
                        로스율 = 0,
                        필요수량 = 1
                    };
                    dc.BOM정보.Add(상위BOM);
                    dc.SaveChanges();
                }
                상위BOM순번 = dc.BOM정보.Where(x => x.품목코드 == 상위품목코드).Select(x => x.BOM순번).FirstOrDefault();
                var BOM = new BOM정보
                {
                    품목코드 = $"{c[rowNum, 4].Value.ToString().Trim()}:1",
                    상위BOM순번 = 상위BOM순번 == 0 ? null : 상위BOM순번,
                    정미수량 = decimal.Parse(c[rowNum, 5].Value.ToString().Trim()),
                    로스율 = (decimal)0,
                    필요수량 = decimal.Parse(c[rowNum, 7].Value.ToString().Trim())
                };

                dc.BOM정보.Add(BOM);
                dc.SaveChanges();

                rowNum++;
            }
        }
        private static void 품목_등록2()
        {
            // 품목항목과 CSV를 가져와 있는 목록만 적용한다.

            var builder = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(ConnectionString);
            using var dc = new ApiDbContext(builder.Options);
            dc.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var sourceFile = @"W:\MyOrder\HiSFS\개발관련자료\기준정보생성_200901\strade.csv";
            using var reader = new StreamReader(sourceFile);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var tradeRecords = csv.GetRecords<Trade>().ToList();

            var fi = new System.IO.FileInfo(@"W:\MyOrder\HiSFS\개발관련자료\기준정보생성_200901\SYX0210_품목EXCEL_IMPORT_20191114141107(최종)0813.xlsx");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(fi);
            var sheet = package.Workbook.Worksheets[0];

            var rowNum = 4;
            var c = sheet.Cells;
            while (true)
            {
                try
                {
                    var check = c[rowNum, 1].Value;
                    if (string.IsNullOrWhiteSpace($"{check}") == true)
                        break;

                    var 원품목코드 = GST("B");
                    var 관리차수 = 1;
                    var 품목코드 = $"{원품목코드}:{관리차수}";
                    var 주거래처코드 = GST("P");
                    if (string.IsNullOrWhiteSpace(주거래처코드) == true)
                        continue;

                    var 품목 = dc.품목정보.FirstOrDefault(x => x.품목코드 == 품목코드);
                    var 거래처 = dc.거래처정보.FirstOrDefault(x => x.거래처코드 == 주거래처코드);
                    if (거래처 == default)
                    {
                        var ft = tradeRecords.FirstOrDefault(x => x.TR_CD == 주거래처코드);
                        if (ft == default)
                        {
                            Console.WriteLine(주거래처코드);
                            continue;
                        }
                        else
                        {
                            거래처 = new 거래처정보
                            {
                                거래처코드 = 주거래처코드,
                                거래처명 = ft.TR_NM,
                                주소 = $"{ft.DIV_ADDR1} {ft.ADDR2}",
                                대표연락처 = ft.TEL,
                                거래처구분코드 = "B0399"
                            };

                            dc.거래처정보.Add(거래처);
                            dc.SaveChanges();
                        }
                    }

                    품목.거래처코드 = 주거래처코드;
                    dc.품목정보.Update(품목);
                    dc.SaveChanges();
                }
                finally
                {
                    Console.WriteLine(rowNum);
                    rowNum++;
                }
            }


            // --------------------------------

            object G(string c)
            {
                return sheet.Cells[$"{c}{rowNum}"].Value;
            }

            string GST(string c)
            {
                return ST(G(c));
            }

            string ST(object v)
            {
                return v?.ToString().Trim();
            }
        }

        private static void 품목2_등록()
        {
            var builder = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(ConnectionString);
            using var dc = new ApiDbContext(builder.Options);
            dc.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var fi = new System.IO.FileInfo(@"W:\MyOrder\HiSFS\개발관련자료\기준정보생성_200901\SYX0210_품목EXCEL_IMPORT_20191114141107(최종)0813.xlsx");
            using var package = new ExcelPackage(fi);
            var sheet = package.Workbook.Worksheets[0];

            var rowNum = 4;
            var c = sheet.Cells;
            while (true)
            {
                var check = c[rowNum, 1].Value;
                if (string.IsNullOrWhiteSpace($"{check}") == true)
                    break;

                var 원품목코드 = GST("B");
                var 관리차수 = 1;
                var 품목코드 = $"{원품목코드}:{관리차수}";
                var newRow = new 품목정보
                {
                    품목코드 = 품목코드,
                    원품목코드 = 원품목코드,
                    관리차수 = 관리차수,
                    품목명 = GST("C"),
                    규격 = GST("D"),
                    품목구분코드 = GST("E") switch { "0" => "B1201", "1" => "B1202", "2" => "B1203", "4" => "B1204", "5" => "B1207",  _ => "B1201" },
                    조달구분코드 = GST("F") switch { "0" => "B1601", "1" => "B1602", _ => "B1601" },
                    단위코드 = GST("H") switch { "EA" => "B1101", _ => "B1101" },
                    LOT여부 = GST("M") switch { "1" => true, _ => false },
                    LOT기본수량 = 1,
                    비고 = GST("BQ")
                };

                dc.품목정보.Add(newRow);

                rowNum++;
            }

            dc.SaveChanges();

            // --------------------------------

            object G(string c)
            {
                return sheet.Cells[$"{c}{rowNum}"].Value;
            }

            string GST(string c)
            {
                return ST(G(c));
            }

            string ST(object v)
            {
                return v?.ToString().Trim();
            }
        }

        private static async Task PrintBarcode()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            await Task.Yield();


            var builder = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging(true)
                .UseSqlServer("Server=server.maum.in,31233\\SQLEXPRESS;Database=HiSFS;User Id=HiSFSDev;Password=dev312");
            var dc = new ApiDbContext(builder.Options);
            dc.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var zebraPrinter = new ZebraPrinter();
            zebraPrinter.Open();

            var result = await dc.보유품목정보
                    .Include(x => x.품목)
                    .Include(x => x.장소)
                    .Include(x => x.장소위치)
                .Where_미삭제_사용()
                .Where(x => x.품목.품목구분코드 == "B1205")     // 설비만 조회
                .ToListAsync();

            foreach (var item in result)
            {
                if (string.IsNullOrWhiteSpace(item.보유명) == true)
                    continue;

                Console.WriteLine($"{item.보유명} : {item.보유품목코드}");

                if (item.보유명 != "CNC 1호기")
                    continue;

                //zebraPrinter.Write(zebraPrinter.GetQRCodeLabel(item.보유품목코드, item.보유명));
                //zebraPrinter.Write(zebraPrinter.GetQRCodeLabel(item.보유품목코드, "CNC"));
                zebraPrinter.Write(zebraPrinter.GetQRCodeLabel(item.보유품목코드, "#######", 1));

                break;
            }

            zebraPrinter.Close();
        }

        private static async Task 직원_등록()
        {
            await Task.Yield();

            var rootPath = @"W:\MyOrder\HiSFS\문서";
            var sourceFile = @$"{rootPath}\SEMP.csv";
            var lines = File.ReadAllLines(sourceFile);
            var rows = new List<string[]>();

            lines = lines[1..];
            foreach (var line in lines)
            {
                var cells = line.Split(',');
                rows.Add(cells);
            }

            // 동일한 EMP_CD 중 가장 큰 CO_CD를 선택함.
            var result = rows
                .GroupBy(x => x[1])
                .Select(x => x.OrderByDescending(x => x[0]).FirstOrDefault())
                .ToList();

            var builder = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(ConnectionString);

            using var dc = new ApiDbContext(builder.Options);
            dc.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            foreach (var row in result)
            {
                if (IsNull(row[8]) == true)
                    continue;

                var newRow = new 직원정보
                {
                    사번 = row[1],
                    사용자명 = row[2],
                    부서코드 = row[12],
                    입사일 = DateTime.ParseExact(row[8], "yyyyMMdd", CultureInfo.InvariantCulture),
                    퇴사일 = IsNull(row[10]) == true ? null : (DateTime?)DateTime.ParseExact(row[10], "yyyyMMdd", CultureInfo.InvariantCulture),
                    주소 = row[27],
                    상세주소 = row[28],
                    연락처1 = row[24],
                    이메일 = row[25],
                    사용유무 = IsNull(row[10]) == true ? true : false,
                    삭제유무 = false
                };

                dc.직원정보.Add(newRow);

                var newRow2 = new 직원권한정보
                {
                    사번 = newRow.사번,
                    암호 = newRow.사번,
                    암호암호화유무 = false,

                    사용유무 = newRow.사용유무,
                    삭제유무 = false
                };
                dc.직원권한정보.Add(newRow2);
            }

            await dc.SaveChangesAsync();
        }

        private static bool IsNull(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
                return true;

            if (text == "NULL")
                return true;

            return false;
        }

        private static async Task 부서_등록()
        {
            await Task.Yield();

            var rootPath = @"W:\MyOrder\HiSFS\문서";
            var sourceFile = @$"{rootPath}\SDEPT.csv";
            var lines = File.ReadAllLines(sourceFile);
            var rows = new List<string[]>();

            lines = lines[1..];
            foreach (var line in lines)
            {
                var cells = line.Split(',');
                rows.Add(cells);
            }

            // 동일한 DEPT_CD 중 가장 큰 CO_CD를 선택함.
            var result = rows
                .GroupBy(x => x[0])
                .Select(x => x.OrderByDescending(x => x[1]).FirstOrDefault())
                .ToList();

            var builder = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging(true)
                .UseSqlServer(ConnectionString);

            using var dc = new ApiDbContext(builder.Options);
            dc.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var sortNo = 1;
            foreach (var row in result)
            {
                var newRow = new 부서정보
                {
                    부서코드 = row[0],
                    부서명 = row[3],
                    사용유무 = true,
                    삭제유무 = false,
                    정렬순번 = sortNo
                };
                sortNo++;

                dc.부서정보.Add(newRow);
            }

            await dc.SaveChangesAsync();
        }


        private async Task 품목_등록()
        {
            //await Test0618();
            await Task.Yield();

            var columns = new int[] {
                2,  // 품목코드
                3,  // 품목명
                4,  // 규격
                5,  // 품목구분
                6,  // 조달구분
                8,  // 단위
                9,  // 단위
                11, // 사용여부
                13, // LOT여부
                16  // 주거래처
            };

            // DbContextOptionsBuilder.EnableSensitiveDataLogging
            var builder = new DbContextOptionsBuilder()
                .EnableSensitiveDataLogging(true)
                .UseSqlServer("Server=server.maum.in,31233\\SQLEXPRESS;Database=HiSFS;User Id=HiSFSDev;Password=dev312");

            using var dc = new ApiDbContext(builder.Options);
            dc.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            //var a = dc.품목정보.First();


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var fi = new System.IO.FileInfo(@"W:\MyOrder\HiSFS\자료\품목_200618.xlsx");
            using var package = new ExcelPackage(fi);
            var sheet = package.Workbook.Worksheets[0];
            var result = sheet.Cells[5, 1].Value;

            var rowNum = 5;
            var isLastRow = false;
            var items = new List<품목정보>();
            while (true)
            {
                isLastRow = string.IsNullOrEmpty(sheet.Cells[rowNum, 1].Value?.ToString() ?? "") == true;
                if (isLastRow == true)
                    break;

                var code = sheet.Cells[rowNum, 2].Text.Trim();
                var num = 1;
                var row = new 품목정보
                {
                    품목코드 = $"{code}:{num}",
                    원품목코드 = code,
                    관리차수 = num,
                    품목명 = sheet.Cells[rowNum, 3].Text.Trim(),
                    품목영문명 = sheet.Cells[rowNum, 3].Text.Trim(),
                    규격 = sheet.Cells[rowNum, 4].Text.Trim(),
                    품목구분코드 = sheet.Cells[rowNum, 5].Text.Trim() switch { "0" => "B1201", "2" => "B1203", "4" => "B1204", _ => null },
                    //품목구분코드 = sheet.Cells[rowNum, 5].Text.Trim() switch { "0" => "B1201", "1" => "B1202", "2" => "B1203", "4" => "B1204", "5" => "B1207", _ => "B1201" },
                    조달구분코드 = sheet.Cells[rowNum, 6].Text.Trim() switch { "0" => "B1601", "1" => "B1602", _ => null },
                    단위코드 = sheet.Cells[rowNum, 9].Text.Trim() switch { "EA" => "B1101", _ => null },
                    사용유무 = sheet.Cells[rowNum, 11].Text.Trim() == "1",
                    LOT여부 = sheet.Cells[rowNum, 13].Text.Trim() == "1",
                    //주거래처 = sheet.Cells[rowNum, 16].Text.Trim(),
                };

                rowNum++;

                if (row.품목구분코드 == null)
                    continue;

                items.Add(row);
            }

            //items.Select(x => x.품목코드).Distinct()

            //var errorList = items.GroupBy(x => x.품목코드).Where(x => x.Count() > 1).Select(x => x.First()).ToList();
            //foreach (var err in errorList)
            //{
            //    Console.WriteLine(err.원품목코드);
            //}

            items = items.GroupBy(x => x.품목코드).Select(grp => grp.First()).ToList();

            dc.품목정보.AddRange(items);
            dc.SaveChanges();
        }

        private static async Task Test0618()
        {
            //var name = nameof(공정정보.공정설비.품목명);
            //Console.WriteLine(name);

            await Task.Yield();
        }

        private static async Task<ApiDbContext> TestEfCore()
        {
            var builder = new DbContextOptionsBuilder()
                .UseSqlServer("Server=localhost;Database=HiSFS;;User Id=HiSFSDev;Password=dev312;Trusted_Connection=True;MultipleActiveResultSets=true");
            var dc = new ApiDbContext(builder.Options);

            var menuList = await dc.메뉴정보
                .OrderBy(x => x.뎁스)
                .ThenBy(x => x.순번)
                .ToListAsync();

            foreach (var menu in menuList)
            {
                Console.WriteLine($"{menu.메뉴명} : {menu.뎁스}, {menu.정렬순번}");
            }

            return dc;
        }

        private static async Task TestApi()
        {
            var channel = new DefaultWampChannelFactory().CreateMsgpackChannel(DefaultServiceUri, DefaultRealm);
            //var channelFactory = new WampChannelFactory()
            //.RawSocketTransport("127.0.0.1", 31200)
            ////.AutoPing(TimeSpan.FromSeconds(5))
            //.MsgpackSerialization()
            //.Build();

            //var channelFactory = new DefaultWampChannelFactory();
            //var channel = channelFactory.CreateJsonChannel("wss://localhost:31200/", "HiSFS.Api");

            await channel.Open();

            var realm = channel.RealmProxy;

            var 공통서비스 = realm.Services.GetCalleeProxy<I공통서비스>(new PrefixCalleeProxyInterceptor("공통서비스"));

            var menuList = await 공통서비스.메뉴_조회();

            //var sw = Stopwatch.StartNew();
            //for (var i = 0; i < 10000; i++)
            //{
            //    var menuList = await 공통서비스.메뉴_목록조회();
            //}
            //sw.Stop();

            //Console.WriteLine(sw.ElapsedMilliseconds);

            channel.Close();
        }
    }

    public class Trade
    {
        public string TR_CD { get; set; }
        public string TR_NM { get; set; }
        public string DIV_ADDR1 { get; set; }
        public string ADDR2 { get; set; }
        public string TEL { get; set; }
    }
}
