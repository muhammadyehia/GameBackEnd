using System.ComponentModel.DataAnnotations;

namespace GreentubeGameTask.Application.Models
{
    public class GameCommentDto
    {
        [Required]
        public long GameId { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}