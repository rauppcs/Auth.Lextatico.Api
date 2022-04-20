using Auth.Lextatico.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Lextatico.Infra.Data.Extensions
{
    public static class EntityFramework
    {
        /// <summary>
        /// Define some default fields for the model.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="nameColumnId">Property name Id for the database. Default: "Id"</param>
        /// <typeparam name="T">Type of model to be defined.</typeparam>
        public static void DefineDefaultFields<T>(this EntityTypeBuilder<T> builder, string tableName = "", string nameColumnId = "Id") where T : Base
        {
            if (string.IsNullOrEmpty(tableName))
                tableName = typeof(T).Name;

            builder.ToTable(tableName);

            builder.Property(model => model.Id)
                .HasColumnName(nameColumnId)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(model => model.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()")
                .HasColumnType("DATETIME");

            builder.Property(model => model.UpdatedAt)
                .ValueGeneratedOnUpdate()
                .HasDefaultValueSql("GETDATE()")
                .HasColumnType("DATETIME");
        }
    }
}
