using System.ComponentModel.DataAnnotations;

namespace TermsManagerAPI.Dtos
{
    public class UpdateProfileRequest
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        public string Prenom { get; set; } = string.Empty;

        public string? NewPassword { get; set; }
    }
}
