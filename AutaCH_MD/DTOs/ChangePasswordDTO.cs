using System.ComponentModel.DataAnnotations;

namespace AutaCH_MD.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public string CurrentPass {  get; set; }
        [Required]
        public string NewPass { get; set; }
        [Required]
        public string ConfirmNewPass {  get; set; }
    }
}
