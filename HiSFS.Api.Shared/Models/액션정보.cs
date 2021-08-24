using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 액션정보 : 공통정보
    {
        [MaxLength(10)]
        [Key]
        public string 액션코드 { get; set; }

        [MaxLength(50)]
        public string 액션명 { get; set; }

        [MaxLength(10)]
        [ForeignKey("액션유형")]
        public string 액션유형코드 { get; set; }

        [MaxLength(50)]
        public string 액션인자 { get; set; }            // 복수개의 인자를 ,(컴마)로 표시

        [MaxLength(100)]
        public string 액션인자설명 { get; set; }

        public bool 변경액션유무 { get; set; }

        [MaxLength(10)]
        [ForeignKey("대체액션")]
        public string 대체액션코드 { get; set; }

        // ------
        public 공통코드 액션유형 { get; set; }
        public 액션정보 대체액션 { get; set; }
    }

    /*
        # 액션유형
            - A01(*작업자)                                                    : 작업자 선택
            - A02(*작업자)                                                    : 작업자 선택해제
            - A03(작업자, *장소, 보유품목, 수량)                              : 장소 입고
            - A04(작업자, *장소, 보유품목, 수량, 불량유형)                    : 장소 입고시 불량 입력
            - A05(작업자, *장소위치, 보유품목, 수량)                          : 장소 위치 배정
            - A06(작업자, *장소, 보유품목, 수량)                              : 장소 반출
            - A07(작업자, *장소, 보유품목, 수량)                              : 장소 출하 (외부로)                   --- 어디로 반출되는지는 별도 기록이 필요함. 반출대장 등
            - A08(작업자, *생산지시, *공정, 공정설비(or 없음)                 : 공정 생산 시작                       --- 공정설비가 없는 경우 고정값 필요
            - A09(작업자, *생산지시, *공정, 공정설비, 수량)                   : 공정 생산 종료
            - A10(작업자, *생산지시, *공정, 공정설비, 수량, 불량유형)         : 공정 생산시 공정 제품 불량 등록
            - A11(작업자, *생산지시, *공정, 공정설비, 수량, 불량유형)         : 공정 생산시 공정 자재 불량 등록      ※ 개별 자재 정보는 입력하지 않는다.
            - A12(작업자, *품질검사, 보유품목)                                : 품질 검사 시작
            - A13(작업자, *품질검사, 보유품목, 불량유형)                      : 품질 검사 종료
            - A14(작업자, *생산지시, 보유품목, 수량)                          : 완제품 등록
            - A15(작업자, *공정설비)                                          : 공정설비 선택
            - A16(작업자, *공정설비)                                          : 공정설비 선택해제
            - A17(작업자, *보유품목)                                          : 보유품목 선택
            - A18(작업자, *보유품목)                                          : 보유품목 선택해제

            ※ * 표시는 바코드에 입력되어야 할 항목
            ※ 공정 전 품목은 보유품목으로 추적
            ※ 공정 진행중인 건은 '공정 생산 종료'액션을 통해 반제품 보유품목으로 추적
            ※ 장소 입고시 최초 위치는 00임. 기본값 필요함
            ※ 장소 반출시 품목은 어디에 있는것인가? 기본값 필요함
            ※ 공정설비가 없는 경우? 기본값 필요함
            ※ 품질검사 수행과 상관없이 완제품 등록을 통해 공정을 통해 생산된 최종 품목이 출하 가능 상태로 만듬, 완제품 등록되지 않은 보유품목은 출하할 수 없음
            ※ 공정 과정에서 창고로 넣어야 할 반제품은 QR코드 발행하여 수량과 함께 관리 (QR코드 발행)

        # 액션인자
            - 작업자
            - 장소
            - 장소위치
            - 보유품목
            - 수량
            - 공정
            - 공정설비
            - 불량유무
            - 자재입고불량유형
            - 공정자재불량유형
            - 공정제품불량유형
            - 품질제품불량유형      --- 정상 유형 필요
            - 품질검사

        # QR코드 코드정의
            - QR코드 version2 25x25 cell 이용 : https://www.qrcode.com/ko/about/version.html

            ## Version:00 (42 Bytes)
                                                    0         1         2         3         4
                                                    0123456789012345678901234567890123456789012
                                                     2_     6_              16_              16
                                                    VV AAAAAA 1111111111111111 2222222222222222
                                                    ---------------------------------------------
            - [작업자 선택]                         00 S9101  1111111111111111                            ex) "00 S9101  I20200525001    "
            - [작업자 해제]                         00 S9102  1111111111111111

            - [장소 입고]                           00 S9103  1111111111111111                            ex) "00 S9103  Z02"
            - [장소 입고시 불량 입력]               00 S9104  1111111111111111                            ex) "00 S9104  Z02"
            - [장소 위치 배정]                      00 S9105  1111111111111111                            ex) "00 S9105  Z0201", "00 S9105  Z0202"
            - [장소 반출]                           00 S9106  1111111111111111                            ex) "00 S9106  Z02"
            - [장소 출하]                           00 S9107  1111111111111111                            ex) "00 S9107  Z02"

            - [공정 생산 시작]                      00 S9108  1111111111111111 2222222222222222           ex) "00 S9108  P200630:1:1      KMFA-420130:1:1" 
            - [공정 생산 종료]                      00 S9109  1111111111111111 2222222222222222           ex) "00 S9109  P200630:1:1      KMFA-420130:1:1" 
            - [공정 생산시 공정 제품 불량 등록]     00 S9110  1111111111111111 2222222222222222           ex) "00 S9110  P200630:1:1      KMFA-420130:1:1" 
            - [공정 생산시 공정 자재 불량 등록]     00 S9111  1111111111111111 2222222222222222           ex) "00 S9111  P200630:1:1      KMFA-420130:1:1" 

            - [품질 검사 시작]                      00 S9112  111111111111111111111111111111111           ex) "00 S9112  Q200630"
            - [품질 검사 종료]                      00 S9113  111111111111111111111111111111111           ex) "00 S9113  Q200630"

            - [완제품 등록]                         00 S9114  111111111111111111111111111111111           ex) "00 S9114  P200630:1:1"

            - [공정설비 선택]                       00 S9115  111111111111111111111111111111111           ex) "00 S9115  E200601:1:200624:1"
            - [공정설비 해제]                       00 S9116  111111111111111111111111111111111

            - [보유품목 선택]                       00 S9117  111111111111111111111111111111111           ex) "00 S9117  IJ-A00470:1:200624:1", "00 S9117  IJ-A00466:1:200624:1", "00 S9117  E200601:1:200624:1", "00 S9117  KMFA-420130:1:200630:1"
            - [보유품목 해제]                       00 S9118  111111111111111111111111111111111
     */

    public static class TagAction
    {
        public const string A01_작업자선택 = "S9101";
        public const string A02_작업자해제 = "S9102";
        public const string A03_장소입고 = "S9103";
        public const string A04_장소입고시불량입력 = "S9104";
        public const string A05_장소위치배치 = "S9105";
        public const string A06_장소반출 = "S9106";
        public const string A07_장소출하 = "S9107";
        public const string A08_공정생산시작 = "S9108";
        public const string A09_공정생산종료 = "S9109";
        public const string A10_공정생산시제품불량등록 = "S9110";
        public const string A11_공정생산시자재불량등록 = "S9111";
        public const string A12_품질검사시작 = "S9112";
        public const string A13_품질검사종료 = "S9113";
        public const string A14_완제품등록 = "S9114";
        public const string A15_공정설비선택 = "S9115";
        public const string A16_공정설비해제 = "S9116";
        public const string A17_보유품목선택 = "S9117";
        public const string A18_보유품목해제 = "S9118";
        public const string A06_위치반출 = "S9119";
        //2021.04.09
        public const string A06_위치입고 = "S9120";

        //2021.04.27
        public const string A06_품목출고 = "S9129";
        public const string A06_발주입고 = "S9121";

        //2021.04.30
        public const string A06_주문출고 = "S9122";

        //2021.05.13
        public const string A06_자재이동출고 = "S9123";
        public const string A06_자재이동입고 = "S9124";

        //2021.05.17
        public const string A06_생산이동출고 = "S9125";
        public const string A06_생산이동입고 = "S9126";

        public const string A06_외주이동출고 = "S9127";
        public const string A06_외주이동입고 = "S9128";

        //2021.04.29
        public const string A02_장소위치유형 = "S9220";

        public const string A09_공정생산실적 = "S9221";

        //2021.05.24
        public const string A03_외주입고 = "S9130";

    }

    public static class TagArg
    {
        public const string A01_작업자 = "S9201";
        public const string A02_장소 = "S9202";
        public const string A03_장소위치 = "S9203";
        public const string A04_보유품목 = "S9204";
        public const string A05_수량 = "S9205";
        public const string A06_공정 = "S9206";
        public const string A07_공정설비 = "S9207";
        public const string A08_불량유무 = "S9208";
        public const string A09_자재입고불량유형 = "S9209";
        public const string A10_공정자재불량유형 = "S9210";
        public const string A11_공정제품불량유형 = "S9211";
        public const string A12_품질제품불량유형 = "S9212";
        public const string A13_품질검사 = "S9213";
        public const string A14_생산지시 = "S9214";
        //2021.02,15 추가
        public const string A15_생산품목 = "S9215";
        public const string A16_검사수량 = "S9216";
        public const string A17_합격수량 = "S9217";

        //2021.03,08 추가
        public const string A18_자재출고유형 = "S9218";
        public const string A19_자재입고유형 = "S9219";

        ////2021.04.27
        //public const string A06_위치입고 = "S9120";
        //public const string A06_품목출고 = "S9129";

        //public const string A06_발주입고 = "S9121";
        ////2021.04.30
        //public const string A06_주문출고 = "S9122";




        //2021.04.29
        public const string A02_장소위치유형 = "S9220";


        //2021.05.18
        public const string A19_자재이동유형 = "S9222";
        public const string A19_생산이동유형 = "S9223";
        public const string A19_외주이동유형 = "S9224";

        //2021.05.22
        public const string A19_자재구매유형 = "S9225";

    }

    public static class ActionResult
    {
        public const string 부서 = "부서";
        public const string 작업자 = "작업자";
        public const string 대체액션 = "대체액션";
        public const string 장소 = "장소";
        public const string 장소코드 = "장소코드";
        // --------------------------------------------
        public const string 오류 = "ERROR";
        public const string 액션명 = "액션명";
        public const string 액션코드 = "액션코드";
        public const string 액션인자 = "액션인자";
        public const string 액션인자설명 = "액션인자설명";
    }
}
