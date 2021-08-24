using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;


namespace HiSFS.Core.EFCore
{
    public class JsonValueConverter<TEntity> : ValueConverter<TEntity, string>
    {
        public JsonValueConverter(JsonSerializerOptions options = null,
                                          ConverterMappingHints mappingHints = null)
                    : base(model => JsonSerializer.Serialize(model, options),
                           value => JsonSerializer.Deserialize<TEntity>(value, options),
                           mappingHints)
        {
            //No ctor body; everything is passed through the call to base()
        }

        public static ValueConverter Default { get; } = new JsonValueConverter<TEntity>(null, null);

        public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(TEntity), typeof(string), i => new JsonValueConverter<TEntity>(null, i.MappingHints));
    }
}
