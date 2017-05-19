using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GreentubeGameTask.Core.Entities;

namespace GreentubeGameTask.Infrastructure.EntityConfigurations
{
    public class UserEntityConfiguration : EntityTypeConfiguration<User>
    {
        public UserEntityConfiguration()
        {
            ToTable("Users");
            HasKey(s => s.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("UC_User_Name") {IsUnique = true, Order = 1}));
            HasMany(e => e.UserGameCommentsRates).WithRequired(e => e.User).WillCascadeOnDelete(false);
        }
    }
}