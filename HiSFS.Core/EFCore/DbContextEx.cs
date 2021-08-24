using HiSFS.Api.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HiSFS.Core.EFCore
{
    /// <summary>
    /// JSON 컬럼 및 다양한 확장 지원
    /// </summary>
    public class DbContextEx : DbContext
    {
        public DbContextEx()
        {
        }

        public DbContextEx([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Custom Attribute 처리
            var ds = this.GetType().GetProperties()
                .Where(x => x.PropertyType.GetInterface(nameof(IQueryable)) == typeof(IQueryable))
                .Select(x => x.PropertyType.GetGenericArguments()[0]);
            foreach (var d in ds)
            {
                foreach (var p in d.GetProperties())
                {
                    var ca = p.GetCustomAttributes();

                    // JsonColumn 처리
                    if (ca.Any(x => x is JsonColumnAttribute) == true)
                    {
                        var jsonValueConverterType = typeof(JsonValueConverter<>).MakeGenericType(p.PropertyType);
                        var jsonOptions = new JsonSerializerOptions
                        {
                            //WriteIndented = true,                                   // Indent the JSON to make it easier for humans to interpret.
                            IgnoreNullValues = true,                                // Targets with NULL values are ignored in serialization.
                            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping   // Use less strict JavaScriptEncoder to read non-ASCII languages such as Chinese, Korean, and Japanese when converted to JSON.
                        };
                        var jsonValueConverter = Activator.CreateInstance(jsonValueConverterType, new object[] { jsonOptions, null }) as ValueConverter;

                        modelBuilder
                            .Entity(d)
                            .Property(p.PropertyType, p.Name)
                            .HasConversion(jsonValueConverter);
                    }
                }
            }
        }

        public override int SaveChanges()
        {
            return SaveChanges("SYSTEM");
        }

        public int SaveChanges(string userId)
        {
            ModifyUpdateInfo(userId);

            return base.SaveChanges();
        }

        private void ModifyUpdateInfo(string userId)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ModelBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = entityEntry.Entity as ModelBase;

                entity.UpdateTime = DateTime.Now;
                entity.UpdateId = userId;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreateTime = DateTime.Now;
                    entity.CreateId = userId;
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync("SYSTEM", cancellationToken);
        }

        public Task<int> SaveChangesAsync(string userId, CancellationToken cancellationToken = default)
        {
            ModifyUpdateInfo(userId);

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
