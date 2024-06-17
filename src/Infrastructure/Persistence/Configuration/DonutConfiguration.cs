using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class DonutConfiguration : IEntityTypeConfiguration<Donut>
    {
        public void Configure(EntityTypeBuilder<Donut> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(50);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(19,6)");
        }
    }
}
