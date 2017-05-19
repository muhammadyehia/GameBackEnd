using System.ComponentModel.DataAnnotations;

namespace GreentubeGameTask.Application.Models
{
    public class UserDto
    {
        [Required]
        public string Name { get; set; }
    }
}