using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Semogly.Core.Domain.AccountContext.Entities;

namespace Semogly.Core.Infrastructure.Data.Mappings;

public class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("account");

        builder.HasKey(a => a.Id).HasName("pk_account");
        builder.Property(a => a.Id)
            .HasColumnType("int")
            .HasColumnName("id")
            .UseIdentityColumn();        

        builder.OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("first_name")
                .HasColumnType("varchar(20)")
                .IsRequired();
            
            name.Property(n => n.MiddleName)
                .HasColumnName("middle_name")
                .HasColumnType("varchar(30)")
                .IsRequired(false);

            name.Property(n => n.LastName)
                .HasColumnName("last_name")
                .HasColumnType("varchar(30)")
                .IsRequired();
        });

        builder.OwnsOne(a => a.Email, email =>
        {
            email.Property(e => e.Address)
                .HasColumnName("email_address")
                .HasColumnType("varchar(50)")
                .IsRequired();
        });

        builder.OwnsOne(a => a.VerificationCode, verificationCode =>
        {
            verificationCode.Property(vc => vc.Code)
                .HasColumnName("verification_code")
                .HasColumnType("char(6)")
                .IsRequired();
            
            verificationCode.Property(vc => vc.ExpiresAtUtc)
                .HasColumnName("verification_expires_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            verificationCode.Property(vc => vc.VerifiedAtUtc)
                .HasColumnName("verification_verified_at")
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);
        });

        builder.OwnsOne(a => a.Password, password =>
        {
            password.Property(p => p.HashText)
                .HasColumnName("password_hashed")
                .IsRequired();
        });

        builder.OwnsOne(a => a.Lockout, lockout =>
        {
            lockout.Property(l => l.LockOutEndUtc)
                .HasColumnName("lockout_end")
                .HasColumnType("timestamp with time zone")
                .IsRequired(false);

            lockout.Property(l => l.LockOutReason)
                .HasColumnName("lockout_reason")
                .IsRequired(false);
        });
    }
}