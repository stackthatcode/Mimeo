using System.ComponentModel.DataAnnotations;

namespace Mimeo.Middle.Config
{
    public class ProtectedConfigSettings 
    {
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public string ApiPassword { get; set; }
        [Required]
        public string ApiSecret { get; set; }
    }
}

