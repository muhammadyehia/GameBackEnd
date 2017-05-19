using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GreentubeGameTask.Core.Entities;

namespace GreentubeGameTask.Infrastructure.EntityConfigurations
{
    public class UserGameCommentRateEntityConfiguration : EntityTypeConfiguration<UserGameCommentRate>
    {
        public UserGameCommentRateEntityConfiguration()
        {
            ToTable("UsersGamesCommentsRates");
            HasKey(s => s.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.UserId)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("UC_UsersGamesComments") {IsUnique = true, Order = 1}));
            Property(p => p.GameId)
                .IsRequired()
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("UC_UsersGamesComments") {IsUnique = true, Order = 2}));
            Property(p => p.CreatedOn).HasColumnType("datetime2");
            Property(p => p.CreatedOn).IsRequired();
            Property(p => p.Comment).IsOptional();
            Property(p => p.Rate).IsRequired();
        }
    }
}