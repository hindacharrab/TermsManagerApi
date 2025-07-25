namespace TermsManagerAPI.Dtos
{
    public class UserWithCGUStatusDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }

        // Statut CGU
        public bool RequiresCGUAcceptance { get; set; }
        public string? AcceptedCGUVersion { get; set; }
        public DateTime? LastCGUAcceptanceDate { get; set; }
        public DateTime? AcceptedCGUDatePublication { get; set; }

        // Indique si l'utilisateur est un admin (ou validé par admin)
        public bool AcceptedByAdmin { get; set; }

        // Propriété calculée pratique
        public string NomComplet => $"{Prenom} {Nom}";
    }
}
