using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamesLibrary.DTOs
{
    public record GameModelDTO
    {
        [Required]
        public string? GameID{ get; init; }

        [Required]
        public string? GameName {get; init;}

        [Required]
        public double? Rating {get; init;}

        [Required]
        public string? Description {get; init;}

        [Required]
        public string? ImgUrl {get; init;}
    }
}