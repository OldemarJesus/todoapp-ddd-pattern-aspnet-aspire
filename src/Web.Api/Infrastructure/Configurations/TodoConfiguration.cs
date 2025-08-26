using Microsoft.EntityFrameworkCore;
using Web.Api.Domains;

namespace Web.Api.Infrastructure.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                todoItemId => todoItemId.Value,
                id => TodoItemId.FromGuid(id)
            )
            .IsRequired();

        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
        builder.Property(x => x.IsCompleted).IsRequired();
        builder.Property(x => x.MemberId)
            .HasConversion(
                memberId => memberId.Value,
                id => MemberId.FromGuid(id)
            )
            .IsRequired();
    }
}
