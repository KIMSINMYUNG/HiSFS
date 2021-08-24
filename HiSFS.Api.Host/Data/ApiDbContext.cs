using HiSFS.Api.Shared.Models;
using HiSFS.Core.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HiSFS.Api.Host.Data
{
	public class ApiDbContext : DbContextEx
	{
		public DbSet<장소정보> 장소정보 { get; set; }
		public DbSet<장소위치정보> 장소위치정보 { get; set; }
		public DbSet<거래처정보> 거래처정보 { get; set; }
		public DbSet<품목정보> 품목정보 { get; set; }
		public DbSet<도면정보> 도면정보 { get; set; }
		public DbSet<공정정보> 공정정보 { get; set; }
		public DbSet<공정단위정보> 공정단위정보 { get; set; }
		public DbSet<공정단위설비정보> 공정단위설비정보 { get; set; }
		public DbSet<공정단위자재정보> 공정단위자재정보 { get; set; }
		public DbSet<공정단위검사정보> 공정단위검사정보 { get; set; }
		public DbSet<공정단위검사장비> 공정단위검사장비 { get; set; }
		public DbSet<보유품목정보> 보유품목정보 { get; set; }
		public DbSet<보유품목이력> 보유품목이력 { get; set; }

		//public DbSet<보유품목발행이력> 보유품목발행이력 { get; set; }
		public DbSet<보유품목불량정보> 보유품목불량정보 { get; set; }
		public DbSet<보유품목검사정보> 보유품목검사정보 { get; set; }
		public DbSet<생산계획정보> 생산계획정보 { get; set; }
		public DbSet<생산계획기본정보> 생산계획기본정보 { get; set; }
		public DbSet<생산계획영업정보> 생산계획영업정보 { get; set; }
		public DbSet<생산계획연구소정보> 생산계획연구소정보 { get; set; }
		public DbSet<생산계획구매정보> 생산계획구매정보 { get; set; }
		public DbSet<생산계획생산정보> 생산계획생산정보 { get; set; }
		public DbSet<생산계획품질정보> 생산계획품질정보 { get; set; }
		public DbSet<생산계획생산관리정보> 생산계획생산관리정보 { get; set; }
		public DbSet<생산지시정보> 생산지시정보 { get; set; }
		public DbSet<품질검사정보> 품질검사정보 { get; set; }
		public DbSet<생산품공정정보> 생산품공정정보 { get; set; }
		public DbSet<생산품공정차수정보> 생산품공정차수정보 { get; set; }
		public DbSet<생산지시공정차수정보> 생산지시공정차수 { get; set; }
		public DbSet<설비가동현황정보> 설비가동현황정보 { get; set; }
		public DbSet<BOM정보> BOM정보 { get; set; }

		// ------------------------------------------------------------------------------------

		public DbSet<액션정보> 액션정보 { get; set; }
		public DbSet<액션로그> 액션로그 { get; set; }
		public DbSet<연동장비정보> 연동장비정보 { get; set; }
		public DbSet<파일폴더정보> 파일폴더정보 { get; set; }
		public DbSet<파일정보> 파일정보 { get; set; }
		public DbSet<메뉴정보> 메뉴정보 { get; set; }
		public DbSet<공통코드> 공통코드 { get; set; }
		public DbSet<부서정보> 부서정보 { get; set; }
		public DbSet<직원정보> 직원정보 { get; set; }
		public DbSet<직원권한정보> 직원권한정보 { get; set; }

		public DbSet<메뉴부서권한정보> 메뉴부서권한정보 { get; set; }
		public DbSet<메뉴직원권한정보> 메뉴직원권한정보 { get; set; }
		public DbSet<메뉴유형별권한정보> 메뉴유형별권한정보 { get; set; }
		public DbSet<메시지정보> 메시지정보 { get; set; }

		//추가 DbSet 2021.02.01
		public DbSet<품질검사측정정보> 품질검사측정정보 { get; set; }

		////추가 DbSet 2021.02.10
		public DbSet<보유품목위치정보> 보유품목위치정보 { get; set; }

		// 추가 DbSet 2021.03.02
		public DbSet<보유품목일련정보> 보유품목일련정보 { get; set; }


		// 추가 DbSet 2021.03.09
		public DbSet<BOM품목정보상세> BOM품목정보상세 { get; set; }
		public DbSet<BOM품목정보> BOM품목정보 { get; set; }

		// 추가 DbSet 2021.03.10
		public DbSet<발주정보> 발주정보 { get; set; }
		public DbSet<발주정보상세> 발주정보상세 { get; set; }

		public DbSet<공정이력정보> 공정이력정보 { get; set; }

		// 추가 DbSet 2021.04.02
		public DbSet<보유품목일지> 보유품목일지 { get; set; }

		public DbSet<보유품목삭제일지> 보유품목삭제일지 { get; set; }

		public DbSet<BOM_정보> BOM_정보 { get; set; }

		public DbSet<BOMALL_정보> BOMALL_정보 { get; set; }

		public DbSet<물류담당자정보> 물류담당자정보 { get; set; }

		public DbSet<사업장> 사업장 { get; set; }


		public DbSet<재고조정정보> 재고조정정보 { get; set; }


		public DbSet<입고처리헤더정보> 입고처리헤더정보 { get; set; }

		public DbSet<입고처리상세정보> 입고처리상세정보 { get; set; }

		public DbSet<출고처리헤더정보> 출고처리헤더정보 { get; set; }

		public DbSet<출고처리상세정보> 출고처리상세정보 { get; set; }



		//2021.04.26
		public DbSet<발주서정보> 발주서정보 { get; set; }
		public DbSet<발주서헤더정보> 발주서헤더정보 { get; set; }

		public DbSet<주문서정보> 주문서정보 { get; set; }

		public DbSet<주문서헤더정보> 주문서헤더정보 { get; set; }


		public DbSet<재고이동헤더정보> 재고이동헤더정보 { get; set; }

		public DbSet<재고이동상세정보> 재고이동상세정보 { get; set; }


		public DbSet<일괄생산실적헤더정보> 일괄생산실적헤더정보 { get; set; }

		public DbSet<일괄생산실적상세정보> 일괄생산실적상세정보 { get; set; }


		public DbSet<작업외주생산실적등록정보> 작업외주생산실적등록정보 { get; set; }

		public DbSet<사용자재보고정보> 사용자재보고정보 { get; set; }

		public DbSet<외주작업지시헤더정보> 외주작업지시헤더정보 { get; set; }
		public DbSet<외주작업지시서정보> 외주작업지시서정보 { get; set; }
		public DbSet<재고조정정보이력> 재고조정정보이력 { get; set; }

		public DbSet<위치상세정보> 위치상세정보 { get; set; }

		public DbSet<생산실적헤더정보> 생산실적헤더정보 { get; set; }

		public DbSet<생산실적상세정보> 생산실적상세정보 { get; set; }

		//2021.05.10
		public DbSet<바코드발급정보> 바코드발급정보 { get; set; }

		public DbSet<보유품목임시위치정보> 보유품목임시위치정보 { get; set; }


		public DbSet<작업자생산실적정보> 작업자생산실적정보 { get; set; }

		public DbSet<재고조정품목정보> 재고조정품목정보 { get; set; }


		public DbSet<외주생산위치정보> 외주생산위치정보 { get; set; }


		public DbSet<외주지시별검사정보> 외주지시별검사정보 { get; set; }
		public DbSet<외주지시별품질검사장비> 외주지시별품질검사장비 { get; set; }

		public DbSet<외주품질검사측정정보> 외주품질검사측정정보 { get; set; }


		public DbSet<외주작업지시서품검정보> 외주작업지시서품검정보 { get; set; }


		public DbSet<발주서별수입검사> 발주서별수입검사 { get; set; }

		public DbSet<발주서별품질검사정보> 발주서별품질검사정보 { get; set; }
		public DbSet<발주서별품질검사장비> 발주서별품질검사장비 { get; set; }

		public DbSet<발주서별품질검사측정정보> 발주서별품질검사측정정보 { get; set; }

		// 2021.06.02
		public DbSet<공정불량정보> 공정불량정보 { get; set; }


		public DbSet<공정이력상세정보> 공정이력상세정보 { get; set; }


		public ApiDbContext([NotNullAttribute] DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.Entity<공통코드>(entity =>
			{
				// 공통코드 (FK)
				entity
				.HasOne(x => x.상위)
				.WithMany(x => x.하위)
				.HasForeignKey(x => x.상위코드);

				entity
				 .HasIndex(x => new { x.코드명 })
				 .IsUnique();

				entity
				.HasOne(x => x.코드유형)
				.WithMany()
				.HasForeignKey(x => x.코드유형코드);
			});



			modelBuilder.Entity<공정단위정보>(entity =>
			{

				entity.HasKey(x => new { x.공정단위코드 });


				entity
				.HasOne(x => x.회사)
				.WithMany()
				.HasForeignKey(x => x.회사코드);

				entity
				.HasOne(x => x.공정품)
				.WithMany()
				.HasForeignKey(x => x.공정품코드);

				entity
				.HasOne(x => x.공정품유형)
				.WithMany()
				.HasForeignKey(x => x.공정품유형코드);

				entity
				.HasOne(x => x.도면)
				.WithMany()
				.HasForeignKey(x => x.도면코드).OnDelete(DeleteBehavior.SetNull);

				entity
				.HasOne(x => x.공정)
				.WithMany()
				.HasForeignKey(x => x.공정코드);

				entity
.HasOne(x => x.완제품)
.WithMany()
.HasForeignKey(x => x.완제품코드);


				entity.HasIndex(x => new { x.원공정단위코드, x.관리차수 })
				.IsUnique();
				entity.HasOne(x => x.공정품)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);
				entity.HasOne(x => x.공정)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);
				entity.HasOne(x => x.공정품유형)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);
				entity.HasOne(x => x.도면)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);

			});


			modelBuilder.Entity<공정단위설비정보>(entity =>
			{
				entity.HasKey(x => new { x.공정단위코드, x.설비코드 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.공정단위)
				 .WithMany()
				 .HasForeignKey(p => new { p.공정단위코드 });
			});


			modelBuilder.Entity<공정단위자재정보>(entity =>
			{
				entity.HasKey(x => new { x.공정단위코드, x.자재코드 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.공정단위)
				 .WithMany()
				 .HasForeignKey(p => new { p.공정단위코드 });

				entity.HasOne(p => p.공정단위)
				 .WithMany(x => x.공정자재목록)
				 .HasForeignKey(p => new { p.공정단위코드 });



			});


			modelBuilder.Entity<공정이력정보>(entity =>
			{
				entity.HasKey(x => new { x.인덱스 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });


				entity.HasOne(p => p.공정단위)
				 .WithMany()
				 .HasForeignKey(p => new { p.공정단위코드 });

				entity.HasOne(p => p.생산지시)
			 .WithMany()
			 .HasForeignKey(p => new { p.생산지시코드 });

				//entity.HasOne(p => p.보유품목)
				// .WithMany()
				// .HasForeignKey(p => new { p.회사코드, p.설비코드 });


				entity.HasOne(p => p.생산품)
				 .WithMany()
				 .HasForeignKey(p => new { p.생산품코드 });


				entity.HasOne(p => p.작업자)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.작업자사번 });


				//2021.05.17
				entity.HasIndex(p => p.생산지시코드);
				entity.HasIndex(p => p.공정단위코드);


			});

			modelBuilder.Entity<공정단위검사정보>(entity =>
			{
				entity.HasKey(x => new { x.공정단위코드, x.품질검사코드 });
				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

			});

			modelBuilder.Entity<공정단위검사장비>(entity =>
			{
				entity.HasKey(x => new { x.공정단위코드, x.품질검사코드, x.검사장비식별번호 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

			});

			modelBuilder.Entity<보유품목정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.보유품목코드 });
				entity.HasIndex(x => new { x.품목코드, x.보유년월일, x.순번 });


				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.장소)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.장소코드 });

				entity.HasOne(p => p.장소위치)
		 .WithMany()
		 .HasForeignKey(p => new { p.회사코드, p.장소위치코드 });





			});

			modelBuilder.Entity<설비가동현황정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.설비)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.설비코드 });


			});

			modelBuilder.Entity<공정단위설비정보>(entity =>
			{
				entity.HasKey(x => new { x.공정단위코드, x.설비코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.설비)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.설비코드 });

				entity.HasOne(p => p.공정단위)
					.WithMany(x => x.공정설비목록)
					.HasForeignKey(p => new { p.공정단위코드 });


			});

			modelBuilder.Entity<보유품목이력>(entity =>
			{
				entity.HasKey(x => new { x.보유품목코드, x.이력순번 });
				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.장소)
				.WithMany()
				.HasForeignKey(p => new { p.회사코드, p.장소코드 });

				entity.HasOne(p => p.위치)
				.WithMany()
				.HasForeignKey(p => new { p.회사코드, p.장소위치코드 });

				entity.HasOne(p => p.연계보유품목)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.연계보유품목코드 });


			});

			//modelBuilder.Entity<보유품목발행이력>(entity =>
			//{
			//	//entity.HasKey(x => new { x.회사코드, x.보유품목코드});
			//	entity.HasOne(p => p.회사)
			// .WithMany()
			// .HasForeignKey(p => new { p.회사코드 });

			//	entity.HasOne(p => p.보유품목)
			// .WithMany()
			// .HasForeignKey(p => new {p.회사코드, p.보유품목코드 });


			//	//entity.HasOne(p => p.연계보유품목)
			//	//.WithMany()
			//	//.HasForeignKey(p => new { p.회사코드, p.연계보유품목코드 });
			//});

			modelBuilder.Entity<보유품목삭제일지>(entity =>
			{
				entity.HasKey(x => new { x.보유품목일지코드 });
				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.보유품목)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.보유품목코드 });


				//entity.HasOne(p => p.연계보유품목)
				//.WithMany()
				//.HasForeignKey(p => new { p.회사코드, p.연계보유품목코드 });
			});

			modelBuilder.Entity<보유품목불량정보>(entity =>
			{
				entity.HasKey(x => new { x.보유품목코드, x.불량유형코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.보유품목)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.보유품목코드 });
			});

			modelBuilder.Entity<보유품목검사정보>(entity =>
			{
				entity.HasKey(x => new { x.보유품목코드, x.품질검사코드 });
				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.보유품목)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.보유품목코드 });
			});


			modelBuilder.Entity<장소위치정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.장소위치코드 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.장소)
				 .WithMany(x => x.장소위치목록)
				 .HasForeignKey(p => new { p.회사코드, p.장소코드 });

				entity.HasIndex(x => new { x.장소코드, x.위치코드 });



			});
			// 장소정보 
			modelBuilder.Entity<장소정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.장소코드 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });


				//entity.HasIndex(x => x.회사코드);

			});

			modelBuilder.Entity<위치상세정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.위치상세코드 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.장소위치)
				 .WithMany(x => x.위치상세목록)
				 .HasForeignKey(p => new { p.회사코드, p.장소위치코드 });

				//entity.HasIndex(x => new { x.장소코드, x.위치코드 });

			});

			modelBuilder.Entity<생산계획정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산계획코드 });
				entity.HasIndex(x => new { x.발주처코드 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.생산책임자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산책임자사번 });
				entity.HasOne(x => x.회사)
				.WithMany()
				.OnDelete(DeleteBehavior.NoAction);


				entity.HasOne(x => x.생산품)
				.WithMany()
				.OnDelete(DeleteBehavior.NoAction);
				entity.HasOne(x => x.생산품공정)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);
				entity.HasOne(x => x.생산책임자)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);
				entity.HasOne(x => x.생산유형)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);
				entity.HasOne(x => x.생산계획상태)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<생산지시정보>(entity =>
			{
				entity.HasIndex(x => new { x.생산계획코드, x.순번 })
				.IsUnique();

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.생산계획)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산계획코드 });
			});

			modelBuilder.Entity<생산품공정정보>(entity =>
			{
				entity.HasKey(x => new { x.생산품공정코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasIndex(x => new { x.생산품코드, x.관리차수 })
				.IsUnique();
				entity.HasOne(p => p.생산품)
			 .WithMany()
			 .HasForeignKey(p => new { p.생산품코드 });


				//	entity.HasMany(t => t.생산품공정차수목록)
				//.WithOne()
				//.HasForeignKey(t => new { t.생산품공정코드, t.회사코드 })
				////.HasPrincipalKey(t => new { t.생산품공정코드})
				//.IsRequired(false);

			});

			modelBuilder.Entity<생산품공정차수정보>(entity =>
			{
				entity.HasKey(x => new { x.생산품공정코드, x.순번 });
				//entity.HasIndex(x => new { x.공정단위코드 });

				//				modelBuilder.Entity<생산품공정차수정보>()
				//			.HasOne(x => x.생산품공정)
				//	.WithMany(x => x.생산품공정차수목록)
				//			.HasPrincipalKey<생산품공정차수정보>(x =>  x.생산품공정코드)
				//.HasForeignKey<생산품공정정보>(x => x.회사코드, x => x.생산품공정코드);

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.생산품공정)
			 .WithMany()
			 .HasForeignKey(p => new { p.생산품공정코드 });

				entity.HasOne(p => p.공정단위)
			 .WithMany()
			 .HasForeignKey(p => new { p.공정단위코드 });

				entity.HasOne(p => p.생산품공정)
		 .WithMany(x => x.생산품공정차수목록)
		 .HasForeignKey(p => new { p.생산품공정코드 });

			});

			modelBuilder.Entity<생산지시공정차수정보>(entity =>
			{
				entity.HasKey(x => new { x.생산지시코드, x.공정차수 });

				entity.HasOne(p => p.회사)
		 .WithMany()
		 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.작업자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.작업자사번 });
			});

			modelBuilder.Entity<BOM정보>(entity =>
			{
				entity.HasOne(x => x.상위BOM)
				.WithMany(x => x.하위BOM목록)
				.HasForeignKey(x => x.상위BOM순번)
				.OnDelete(DeleteBehavior.NoAction);
			});


			modelBuilder.Entity<메뉴직원권한정보>(entity =>
			{
				entity.HasKey(x => new { x.메뉴순번, x.직원사번 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.직원)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.직원사번 });

			});

			modelBuilder.Entity<메뉴유형별권한정보>(entity =>
			{
				entity.HasKey(x => new { x.메뉴순번, x.권한유형코드 });
			});

			modelBuilder.Entity<품질검사측정정보>(entity =>
			{
				entity.HasKey(x => new { x.시리얼넘버, x.품질검사코드, x.생산지시코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				//entity.HasOne(p => p.보유품목)
				//.WithMany()
				//.HasForeignKey(p => new { p.회사코드, p.보유품목코드 }).OnDelete(DeleteBehavior.SetNull);


			});

			modelBuilder.Entity<보유품목위치정보>(entity =>
			{
				entity.HasKey(x => new { x.보유품목위치순번 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.장소위치)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.장소위치코드 });

				entity.HasOne(p => p.보유품목)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.보유품목코드 });
			});


			modelBuilder.Entity<보유품목임시위치정보>(entity =>
			{
				entity.HasKey(x => new { x.보유품목위치순번 });

				entity.HasIndex(p => p.위치상세코드);

			});

			modelBuilder.Entity<외주생산위치정보>(entity =>
			{
				entity.HasKey(x => new { x.보유품목위치순번 });

				entity.HasIndex(p => p.장소위치코드);

			});

			modelBuilder.Entity<보유품목일련정보>(entity =>
			{
				entity.HasKey(x => new { x.보유년월일, x.순번, x.품목코드 });
			});


			modelBuilder.Entity<BOM품목정보>(entity =>
			{
				entity.HasKey(x => new { x.BOM품목정보코드 });

			});

			modelBuilder.Entity<BOM품목정보상세>(entity =>
			{
				entity.HasKey(x => new { x.품목코드, x.BOM품목정보코드 });
				entity.HasIndex(x => new { x.공정단위코드 });

			});

			// 거래처정보 
			modelBuilder.Entity<거래처정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.거래처코드 });
			});
			// BOM_정보 
			modelBuilder.Entity<BOM_정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.모품번, x.순번 });
			});

			// BOMOne_정보 
			modelBuilder.Entity<BOMALL_정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.모품번, x.순번 });
			});

			// 부서정보 
			modelBuilder.Entity<부서정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.부서코드 });
				entity.HasOne(p => p.회사)
		 .WithMany()
		 .HasForeignKey(p => new { p.회사코드 });
				//entity.HasIndex(x => x.회사코드);
			});
			// 사업장 
			modelBuilder.Entity<사업장>(entity =>
			{
				entity.HasKey(x => new { x.회사코드 });
			});



			// 품목정보 
			modelBuilder.Entity<품목정보>(entity =>
			{
				entity.HasIndex(x => new { x.원품목코드, x.관리차수 }).IsUnique();
				entity.HasKey(x => new { x.품목코드 });
				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasIndex(x => new { x.거래처코드 });


				//entity.HasIndex(x => x.회사코드);

			});

			modelBuilder.Entity<발주정보상세>(entity =>
			{
				entity.HasKey(x => new { x.발주순번, x.품목코드 });
			});

			modelBuilder.Entity<발주정보>(entity =>
			{
				entity.HasKey(x => new { x.발주순번, x.회사코드 });
				entity.HasIndex(x => new { x.거래처코드 });
			});

			modelBuilder.Entity<보유품목일련정보>(entity =>
			{
				entity.HasIndex(x => new { x.거래처코드 });
			});

			modelBuilder.Entity<보유품목일지>(entity =>
			{
				entity.HasIndex(x => new { x.보유품목일지코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.보유품목)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.보유품목코드 });

				entity.HasOne(p => p.품목)
			 .WithMany()
			 .HasForeignKey(p => new { p.품목코드 });

			});



			modelBuilder.Entity<발주정보상세>(entity =>
			{
				entity.HasIndex(x => new { x.발주순번 });
			});



			modelBuilder.Entity<메뉴부서권한정보>(entity =>
			{
				entity.HasKey(x => new { x.메뉴순번, x.부서코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.부서)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.부서코드 });
			});


			modelBuilder.Entity<물류담당자정보>(entity =>
			{
				entity.HasKey(x => new { x.물류담당자번호 });
				//entity.HasNoKey();
			});

			modelBuilder.Entity<주문서정보>(entity =>
			{
				entity.HasKey(x => new { x.주문서번호 });
				//entity.HasNoKey();

			});

			modelBuilder.Entity<발주서정보>(entity =>
			{
				entity.HasKey(x => new { x.발주서번호 });
				//entity.HasNoKey();
			});

			modelBuilder.Entity<재고조정정보>(entity =>
			{
				entity.HasNoKey();
			});

			modelBuilder.Entity<생산계획구매정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산계획코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.계획자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.계획자사번 });

				entity.HasOne(p => p.검토자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.검토자사번 });

				entity.HasOne(p => p.생산계획)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산계획코드 });
			});

			modelBuilder.Entity<생산계획기본정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산계획코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.계획자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.계획자사번 });

				entity.HasOne(p => p.검토자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.검토자사번 });

				entity.HasOne(p => p.생산계획)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산계획코드 });
			});

			modelBuilder.Entity<생산계획품질정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산계획코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.계획자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.계획자사번 });

				entity.HasOne(p => p.검토자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.검토자사번 });

				entity.HasOne(p => p.생산계획)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산계획코드 });
			});

			modelBuilder.Entity<생산계획생산정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산계획코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.계획자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.계획자사번 });

				entity.HasOne(p => p.검토자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.검토자사번 });

				entity.HasOne(p => p.생산계획)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산계획코드 });
			});

			modelBuilder.Entity<생산계획생산관리정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산계획코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.계획자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.계획자사번 });

				entity.HasOne(p => p.검토자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.검토자사번 });

				entity.HasOne(p => p.생산계획)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산계획코드 });
			});

			modelBuilder.Entity<생산계획연구소정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산계획코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.계획자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.계획자사번 });

				entity.HasOne(p => p.검토자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.검토자사번 });

				entity.HasOne(p => p.생산계획)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산계획코드 });
			});

			modelBuilder.Entity<생산계획영업정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산계획코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.계획자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.계획자사번 });

				entity.HasOne(p => p.검토자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.검토자사번 });

				entity.HasOne(p => p.생산계획)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.생산계획코드 });
			});

			modelBuilder.Entity<메시지정보>(entity =>
			{
				//entity.HasIndex(x => new { x.회사코드, x.생산계획코드 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.수신인)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.수신인사번 });

				entity.HasOne(p => p.발송인)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.발송인사번 });
			});
			modelBuilder.Entity<직원권한정보>(entity =>
			{
				entity.HasKey(x => new { x.식별인자 });
				//entity.HasKey(x => new { x.회사코드, x.사번 });

				//ntity.HasIndex(x => new { x.회사코드, x.사번 });

				entity.HasOne(p => p.회사)
				.WithMany()
				.HasForeignKey(p => new { p.회사코드 });

				//entity.HasOne(p => p.직원정보)
				//.WithMany()

				//.HasForeignKey(x => new { x.회사코드, x.사번 }).OnDelete(DeleteBehavior.NoAction);

			});
			modelBuilder.Entity<직원정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.사번 });
				//entity.HasAlternateKey(x => new { x.식별인자 });
				entity.HasOne(p => p.부서)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.부서코드 });

				entity.HasOne(p => p.권한정보)
				.WithMany()

				.HasForeignKey(x => new { x.식별인자 }).OnDelete(DeleteBehavior.NoAction);

				entity.HasIndex(x => x.식별번호)
					.IsUnique();

				//entity.HasIndex(x => x.식별인자)
				//	.IsUnique();
			});
			modelBuilder.Entity<액션로그>(entity =>
			{
				entity.HasIndex(x => new { x.회사코드, x.순번 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.직원)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.직원사번 });
			});


			modelBuilder.Entity<입고처리헤더정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호 });

			});

			modelBuilder.Entity<입고처리상세정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호, x.작업순번 });

				entity.HasOne(p => p.입고처리헤더)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.작업번호 });
			});


			modelBuilder.Entity<출고처리헤더정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호 });

			});

			modelBuilder.Entity<출고처리상세정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호, x.작업순번 });


				entity.HasOne(p => p.출고처리헤더)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.작업번호 });


			});


			//2021.04.26
			modelBuilder.Entity<발주서정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.발주번호, x.발주순번 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.헤더정보)
			 .WithMany(q => q.발주서정보상세)
			 .HasForeignKey(p => new { p.회사코드, p.발주번호 });
			});

			modelBuilder.Entity<발주서헤더정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.발주번호 });

				//entity.HasMany(p => p.발주서정보상세)
				//.WithOne()
				//.HasForeignKey(p => new { p.회사코드, p.발주번호 });
				//entity.HasMany(x => x.발주서정보상세);
				//entity.HasNoKey();
			});

			modelBuilder.Entity<주문서정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.주문번호, x.순번 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.헤더정보)
			 .WithMany(q => q.주문서정보상세)
			 .HasForeignKey(p => new { p.회사코드, p.주문번호 });
			});

			modelBuilder.Entity<주문서헤더정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.주문번호 });

			});

			modelBuilder.Entity<재고이동헤더정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호 });

			});

			modelBuilder.Entity<재고이동상세정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호, x.작업순번 });

				entity.HasOne(p => p.재고이동헤더)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.작업번호 });

			});

			modelBuilder.Entity<일괄생산실적헤더정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호 });

			});

			modelBuilder.Entity<일괄생산실적상세정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호, x.작업순번 });

				entity.HasOne(p => p.일괄생산실적헤더)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.작업번호 });

			});




			modelBuilder.Entity<작업외주생산실적등록정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호 });
			});


			modelBuilder.Entity<사용자재보고정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업번호, x.작업순번 });
			});

			modelBuilder.Entity<외주작업지시헤더정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.지시번호 });
			});


			modelBuilder.Entity<외주작업지시서정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.지시번호, x.전개순번 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.외주작업지시헤더)
			 .WithMany(q => q.외주작업지시서상세)
			 .HasForeignKey(p => new { p.회사코드, p.지시번호 });
			});

			modelBuilder.Entity<재고조정정보이력>(entity =>
			{
				entity.HasKey(x => new { x.CO_CD, x.ADJUST_NB, x.ADJUST_SQ });
			});


			modelBuilder.Entity<생산실적헤더정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산지시코드 });


				entity.HasOne(p => p.공정단위)
			 .WithMany()
			 .HasForeignKey(p => new { p.공정단위코드 });

				entity.HasOne(p => p.생산품)
				 .WithMany()
				 .HasForeignKey(p => new { p.생산품코드 });

				entity.HasOne(p => p.생산품공정)
				 .WithMany()
				 .HasForeignKey(p => new { p.생산품공정코드 });

				entity.HasOne(x => x.공정단위)
				.WithMany()
				.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(x => x.생산품)
				.WithMany()
				.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(x => x.생산품공정)
				.WithMany()
				.OnDelete(DeleteBehavior.NoAction);

			});

			modelBuilder.Entity<생산실적상세정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.생산지시코드, x.작업순번 });

				entity.HasOne(p => p.회사)
				.WithMany()
				.HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.생산실적헤더)
				.WithMany()
				.HasForeignKey(p => new { p.회사코드, p.생산지시코드 });



				entity.HasOne(p => p.작업자)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.작업자사번 });



				entity.HasOne(x => x.회사)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(x => x.생산실적헤더)
					.WithMany()
					.OnDelete(DeleteBehavior.NoAction);



				entity.HasOne(x => x.작업자)
				.WithMany()
				.OnDelete(DeleteBehavior.NoAction);


			});

			//2021.05.10
			modelBuilder.Entity<바코드발급정보>(entity =>
			{

				entity.HasOne(p => p.회사)
				.WithMany()
				.HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.품목)
				.WithMany()
				.HasForeignKey(p => new { p.품목코드 });

			});


			modelBuilder.Entity<작업자생산실적정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.작업순번 });

				//entity.HasOne(p => p.회사)
				//.WithMany()
				//.HasForeignKey(p => new { p.회사코드 });


				entity.HasOne(p => p.작업자)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.작업자사번 });

				//		entity.HasOne(p => p.생산품)
				//.WithMany()
				//.HasForeignKey(p => new { p.회사코드, p.생산품코드 });





				//entity.HasOne(x => x.회사)
				//    .WithMany()
				//    .OnDelete(DeleteBehavior.NoAction);


				//entity.HasOne(x => x.생산품)
				//            .WithMany()
				//            .OnDelete(DeleteBehavior.NoAction);

				//entity.HasOne(x => x.공정단위)
				//  .WithMany()
				//  .OnDelete(DeleteBehavior.NoAction);



			});



			// 품목정보 
			modelBuilder.Entity<재고조정품목정보>(entity =>
			{
				entity.HasIndex(x => new { x.원품목코드, x.관리차수 }).IsUnique();
				entity.HasKey(x => new { x.품목코드 });
				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasIndex(x => new { x.거래처코드 });


				//entity.HasIndex(x => x.회사코드);

			});




			modelBuilder.Entity<외주지시별검사정보>(entity =>
			{
				entity.HasKey(x => new { x.지시번호, x.품질검사코드 });
				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

				//entity.HasOne(p => p.품질검사)
				// .WithMany()
				// .HasForeignKey(p => new { p.회사코드, p.품질검사코드 });

			});

			modelBuilder.Entity<외주지시별품질검사장비>(entity =>
			{
				entity.HasKey(x => new { x.지시번호, x.검사장비식별번호 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

			});


			modelBuilder.Entity<외주품질검사측정정보>(entity =>
			{
				entity.HasKey(x => new { x.시리얼넘버, x.품질검사코드, x.지시번호 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				//entity.HasOne(p => p.보유품목)
				//.WithMany()
				//.HasForeignKey(p => new { p.회사코드, p.보유품목코드 }).OnDelete(DeleteBehavior.SetNull);

			});


			modelBuilder.Entity<외주작업지시서품검정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.지시번호, x.전개순번 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

			});



			modelBuilder.Entity<발주서별수입검사>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.발주서번호, x.발주번호, x.발주순번 });
			});

			modelBuilder.Entity<발주서별품질검사측정정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.시리얼넘버, x.품질검사코드, x.발주번호, x.발주순번 });

				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				//entity.HasOne(p => p.보유품목)
				//.WithMany()
				//.HasForeignKey(p => new { p.회사코드, p.보유품목코드 }).OnDelete(DeleteBehavior.SetNull);

			});

			modelBuilder.Entity<발주서별품질검사정보>(entity =>
			{
				entity.HasKey(x => new { x.회사코드, x.발주번호, x.발주순번, x.품질검사코드 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });

				//    entity.HasOne(p => p.품질검사)
				// .WithMany()
				// .HasForeignKey(p => new { p.회사코드, p.품질검사코드 });

				//    entity.HasOne(p => p.검사단위)
				//.WithMany()
				// .HasForeignKey(p => new { p.회사코드, p.검사단위코드 });



			});

			modelBuilder.Entity<발주서별품질검사장비>(entity =>
			{
				entity.HasKey(x => new { x.발주번호, x.발주순번, x.검사장비식별번호 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });


			});



			// 2021.06.02
			modelBuilder.Entity<공정불량정보>(entity =>
			{
				entity.HasKey(x => new { x.인덱스 });

				entity.HasOne(p => p.회사)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드 });


				entity.HasOne(p => p.공정단위)
				 .WithMany()
				 .HasForeignKey(p => new { p.공정단위코드 });

				entity.HasOne(p => p.생산지시)
			 .WithMany()
			 .HasForeignKey(p => new { p.생산지시코드 });

				entity.HasOne(p => p.보유품목)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.설비코드 });


				entity.HasOne(p => p.생산품)
				 .WithMany()
				 .HasForeignKey(p => new { p.생산품코드 });


				entity.HasOne(p => p.작업자)
				 .WithMany()
				 .HasForeignKey(p => new { p.회사코드, p.작업자사번 });


				//2021.05.17
				entity.HasIndex(p => p.생산지시코드);
				entity.HasIndex(p => p.공정단위코드);


			});



			modelBuilder.Entity<공정이력상세정보>(entity =>
			{
				entity.HasKey(x => new { x.인덱스 });


				entity.HasOne(p => p.회사)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드 });

				entity.HasOne(p => p.공정이력)
				 .WithMany()
				 .HasForeignKey(p => new { p.공정이력인덱스 });


				entity.HasOne(p => p.작업자)
			 .WithMany()
			 .HasForeignKey(p => new { p.회사코드, p.작업자사번 });
			});






			// 부서코드
			//modelBuilder.Entity<부서정보>()
			//    .HasOne(x => x.상위부서)
			//    .WithMany(x => x.하위부서목록)
			//    .HasForeignKey(x => x.상위부서코드);

			//modelBuilder.Entity<장소위치정보>()
			//    .HasOne(x => x.장소)
			//    .WithMany()
			//    .OnDelete(DeleteBehavior.NoAction);

			/////////////////////////////////////////////////////////////////////////////////////////////////////

			// 메뉴부서권한정보 (MULTI PK)
			//modelBuilder.Entity<메뉴부서권한정보>()
			//    .HasKey(x => new { x.메뉴순번, x.부서코드 });

			// modelBuilder.Entity<부서정보>()
			//.HasKey(c => new { c.회사코드, c.부서코드 });

			//*/
			/* 
			modelBuilder.HasSequence<int>("OrderNumbers", schema: "shared")
			  .StartsAt(1000)
			  .IncrementsBy(5);  
			  */

		}
	}
}
