using System.ComponentModel.DataAnnotations;

namespace CGUManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        [MaxLength(255, ErrorMessage = "L'email ne peut pas dépasser 255 caractères")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [MaxLength(255, ErrorMessage = "Le hash du mot de passe ne peut pas dépasser 255 caractères")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le rôle est obligatoire")]
        [MaxLength(50, ErrorMessage = "Le rôle ne peut pas dépasser 50 caractères")]
        public string Role { get; set; } = "User"; // "User" ou "Admin"

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [MaxLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [MaxLength(100, ErrorMessage = "Le prénom ne peut pas dépasser 100 caractères")]
        public string Prenom { get; set; } = string.Empty;

        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        public DateTime? LastCGUAcceptanceDate { get; set; } // null si jamais accepté

        // Propriété calculée pour obtenir le nom complet
        public string NomComplet => $"{Prenom} {Nom}";

        // Propriété pour vérifier si l'utilisateur est admin
        public bool IsAdmin => Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);

        // Propriété pour vérifier si l'utilisateur a accepté les CGU
        public bool HasAcceptedCGU => LastCGUAcceptanceDate.HasValue;
    }
}