using System.ComponentModel.DataAnnotations;

namespace GreentubeGameTask.Application.Models
{
    public class GameRateDto
    {
        [Required]
        public long GameId { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public int Rate { get; set; }
    }
}