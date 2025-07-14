namespace CGUManagementAPI.Models
{
    public class CGU
    {
        public int Id { get; set; }                        // Identifiant unique
        public string Content { get; set; } = string.Empty; // Texte des CGU
        public DateTime DatePublication { get; set; }       // Date de publication
        public string Version { get; set; } = "1.0";        // Numéro de version
    }
}
