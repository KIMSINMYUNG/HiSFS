using HiSFS.Api.Shared.Models.View_DZICUBE;
using HiSFS.Core.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.Api.Host.Data
{
	public class ApiDbDZICUBEContext : DbContext
	{

		public DbSet<VL_MES_EMP> VL_MES_EMP { get; set; }

		public DbSet<VL_MES_LOC> VL_MES_LOC { get; set; }

		public DbSet<VL_MES_ITEM> VL_MES_ITEM { get; set; }

		public DbSet<VL_MES_DIV> VL_MES_DIV { get; set; }

		public DbSet<VL_MES_DEPT> VL_MES_DEPT { get; set; }

		public DbSet<VL_MES_CUST> VL_MES_CUST { get; set; }

		public DbSet<VL_MES_BOM> VL_MES_BOM { get; set; }


		public DbSet<VL_MES_SO> VL_MES_SO { get; set; }

		public DbSet<VL_MES_PO> VL_MES_PO { get; set; }

		public DbSet<VL_MES_WO_WF> VL_MES_WO_WF { get; set; }

		public DbSet<VL_MES_PLN> VL_MES_PLN { get; set; }

		public DbSet<VL_MES_ADJUST> VL_MES_ADJUST { get; set; }


		public DbSet<BARPLUS_LSTOCK> BARPLUS_LSTOCK { get; set; }

		public DbSet<BARPLUS_LSTOCK_D> BARPLUS_LSTOCK_D { get; set; }

		public DbSet<BARPLUS_LDELIVER> BARPLUS_LDELIVER { get; set; }

		public DbSet<BARPLUS_LDELIVER_D> BARPLUS_LDELIVER_D { get; set; }

		public DbSet<BARPLUS_LSTKMOVE> BARPLUS_LSTKMOVE { get; set; }
		public DbSet<BARPLUS_LSTKMOVE_D> BARPLUS_LSTKMOVE_D { get; set; }

		public DbSet<BARPLUS_LPRODUCTION> BARPLUS_LPRODUCTION { get; set; }
		public DbSet<BARPLUS_LPRODUCTION_D> BARPLUS_LPRODUCTION_D { get; set; }


		//작업실적등록/외주실적등록/생산실적등록
		public DbSet<BARPLUS_LORCV_H> BARPLUS_LORCV_H { get; set; }

		//사용자재보고
		public DbSet<BARPLUS_LMTL_USE> BARPLUS_LMTL_USE { get; set; }
		


		public ApiDbDZICUBEContext(DbContextOptions<ApiDbDZICUBEContext> options) : base(options)
		{
		}

		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder
		   .Entity<VL_MES_EMP>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_EMP");
		   });

			modelBuilder
		   .Entity<VL_MES_LOC>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_LOC");
		   });

			modelBuilder
		   .Entity<VL_MES_ITEM>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_ITEM");
		   });

			modelBuilder
		   .Entity<VL_MES_DIV>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_DIV");
		   });

			modelBuilder
		   .Entity<VL_MES_DEPT>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_DEPT");
		   });

			modelBuilder
		   .Entity<VL_MES_CUST>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_CUST");
		   });

			modelBuilder
		   .Entity<VL_MES_BOM>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_BOM");
		   });

			modelBuilder
		   .Entity<VL_MES_SO>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_SO");
		   });

			modelBuilder
		   .Entity<VL_MES_SO>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_SO");
		   });

			modelBuilder
		   .Entity<VL_MES_PO>(eb =>
		   {
			   eb.HasNoKey();
			   eb.ToView("VL_MES_PO");
		   });


			modelBuilder
	   .Entity<VL_MES_WO_WF>(eb =>
	   {
		   eb.HasNoKey();
		   eb.ToView("VL_MES_WO_WF");
	   });
			modelBuilder
	   .Entity<VL_MES_PLN>(eb =>
	   {
		   eb.HasNoKey();
		   eb.ToView("VL_MES_PLN");
	   });
			modelBuilder
	   .Entity<VL_MES_ADJUST>(eb =>
	   {
		   eb.HasNoKey();
		   eb.ToView("VL_MES_ADJUST");
	   });

		modelBuilder.Entity<BARPLUS_LSTOCK>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB });
				//eb.ToView("BARPLUS_LSTOCK");
			});

			modelBuilder.Entity<BARPLUS_LSTOCK_D>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB , x.WORK_SQ });
				//eb.ToView("BARPLUS_LSTOCK_D");
			});

			modelBuilder.Entity<BARPLUS_LDELIVER>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB });
				//eb.ToView("BARPLUS_LSTOCK");
			});

			modelBuilder.Entity<BARPLUS_LDELIVER_D>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB, x.WORK_SQ });
				//eb.ToView("BARPLUS_LSTOCK_D");
			});


			modelBuilder.Entity<BARPLUS_LSTKMOVE>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB });
			});

			modelBuilder.Entity<BARPLUS_LSTKMOVE_D>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB, x.WORK_SQ });
			});

			modelBuilder.Entity<BARPLUS_LPRODUCTION>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB });
			});

			modelBuilder.Entity<BARPLUS_LPRODUCTION_D>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB, x.WORK_SQ });
			});


			modelBuilder.Entity<BARPLUS_LORCV_H>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB });
			});

			modelBuilder.Entity<BARPLUS_LMTL_USE>(eb =>
			{
				eb.HasKey(x => new { x.CO_CD, x.WORK_NB , x.WORK_SQ });
			});

			
		}
	}
}
