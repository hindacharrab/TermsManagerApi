namespace CGUManagementAPI.Dtos
{
    public class CGUStatusDto
    {
        public bool RequiresCGUAcceptance { get; set; }
        public string CurrentCGUVersion { get; set; } = string.Empty;
        public string? UserAcceptedVersion { get; set; }
        public string Reason { get; set; } = string.Empty; // "NEVER_ACCEPTED", "OUTDATED_VERSION", "UP_TO_DATE"
        public DateTime? LastAcceptanceDate { get; set; }
    }

    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime DateCreation { get; set; }
        public bool RequiresCGUAcceptance { get; set; }
        public string? AcceptedCGUVersion { get; set; }
        public DateTime? LastCGUAcceptanceDate { get; set; }
        public bool AcceptedByAdmin { get; set; }
    }
}