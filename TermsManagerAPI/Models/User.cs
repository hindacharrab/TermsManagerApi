using System;
using System.ComponentModel.DataAnnotations;

namespace CGUManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [RegularExpression("User|Admin", ErrorMessage = "Le rôle doit être 'User' ou 'Admin'")]
        public string Role { get; set; } = "User";

        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public string Prenom { get; set; } = string.Empty;

        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        public DateTime? LastCGUAcceptanceDate { get; set; }
        public string? AcceptedCGUVersion { get; set; }

        // --- Propriétés calculées ---

        public string NomComplet => $"{Prenom} {Nom}";
    
        // Clé étrangère (nullable si l'utilisateur n'a jamais accepté)
        public int? AcceptedCGUId { get; set; }

        // Propriété de navigation vers la CGU acceptée
        public CGU? AcceptedCGU { get; set; }


    }


}
