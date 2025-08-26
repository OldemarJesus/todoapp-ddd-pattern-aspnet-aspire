using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Web.Api.Domains;

namespace Web.Api.Infrastructure.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasConversion(
                memberId => memberId.Value,
                id => MemberId.FromGuid(id)
            )
            .IsRequired();

        builder.Property(m => m.Username).IsRequired().HasMaxLength(100);
        builder.Property(m => m.FullName).IsRequired().HasMaxLength(255);
        builder.Property(m => m.Password).IsRequired();

        builder.HasMany(m => m.TodoItems)
            .WithOne()
            .HasForeignKey(ti => ti.MemberId)
            .IsRequired();
    }
}
