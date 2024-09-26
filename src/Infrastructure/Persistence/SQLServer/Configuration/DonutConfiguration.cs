using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.SQLServer.Configuration
{
    internal class DonutConfiguration : IEntityTypeConfiguration<Donut>
    {
        public void Configure(EntityTypeBuilder<Donut> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(6,2)");

            builder.Property(x => x.Description)
                .HasMaxLength(512);
        }
    }
}
