using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GreentubeGameTask.Core.Entities;

namespace GreentubeGameTask.Infrastructure.EntityConfigurations
{
    public class GameEntityConfiguration : EntityTypeConfiguration<Game>
    {
        public GameEntityConfiguration()
        {
            ToTable("Games");
            HasKey(s => s.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            Property(p => p.CreatedOn).HasColumnType("datetime2");
            Property(p => p.CreatedOn).IsRequired();
            Property(p => p.OverAllRate).IsOptional().HasColumnType("float");
            Property(p => p.NumberOfVotes).IsOptional();
            HasMany(e => e.UserGameCommentsRates).WithRequired(e => e.Game).WillCascadeOnDelete(false);
        }
    }
}