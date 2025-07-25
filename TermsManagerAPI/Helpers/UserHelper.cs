using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CGUManagementAPI.Dtos;
using CGUManagementAPI.Models;
using TermsManagerAPI.Dtos;

namespace TermsManagerAPI.Helpers
{
    public static class UserHelper
    {
        // ✅ Vérifie si l'utilisateur a le rôle "Admin"
        public static bool IsAdmin(User user)
        {
            return user.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            // OrdinalIgnoreCase: permeter de comparer deux chaine
        }

        // ✅ Vérifie si l'utilisateur doit accepter la dernière version des CGU
        public static bool RequiresCGUAcceptance(User user, string currentCGUVersion)
        {
            if (string.IsNullOrEmpty(user.AcceptedCGUVersion))
                return true;
            if (!string.Equals(user.AcceptedCGUVersion, currentCGUVersion, StringComparison.Ordinal))
                return true;

            return false;
        }

        // ✅ Applique une acceptation de CGU
        public static void AcceptCGU(User user, string version)
        {
            user.AcceptedCGUVersion = version;
            user.LastCGUAcceptanceDate = DateTime.UtcNow;
        }

        // ✅ Mappe une liste de User vers UserWithCGUStatusDto avec statut CGU et admin
        public static List<UserWithCGUStatusDto> MapUsersWithCGUStatus(List<User> users, string currentCGUVersion, IMapper mapper)
        {
            var dtos = mapper.Map<List<UserWithCGUStatusDto>>(users);

            foreach (var dto in dtos)
            {
                var user = users.FirstOrDefault(u => u.Id == dto.Id);
                dto.RequiresCGUAcceptance = RequiresCGUAcceptance(user, currentCGUVersion);
                dto.AcceptedByAdmin = IsAdmin(user);
            }

            return dtos;
        }
        public static User CreateUserFromRequest(CreateUserRequest request, IMapper mapper)
        {
            // Map DTO vers User
            var user = mapper.Map<User>(request);

            // Hachage du mot de passe (attention: ici request.Password est mappé en PasswordHash)
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Initialisation des propriétés CGU et dates à null
            user.LastCGUAcceptanceDate = null;
            user.AcceptedCGUVersion = null;
            user.AcceptedCGUId = null;
            user.AcceptedCGU = null;

            // Initialisation de la date de création si tu en as une
            user.DateCreation = DateTime.UtcNow;

            return user;
        }
        public static CGUStatusDto BuildCGUStatus(User user, string currentCGUVersion)
        {

            var dto = new CGUStatusDto
            {
                CurrentCGUVersion = currentCGUVersion,
                UserAcceptedVersion = user.AcceptedCGUVersion,
                LastAcceptanceDate = user.LastCGUAcceptanceDate

            };

            if (string.IsNullOrEmpty(user.AcceptedCGUVersion))
            {
                dto.RequiresCGUAcceptance = true;
                dto.Reason = "NEVER_ACCEPTED";

            }
            else if (!user.AcceptedCGUVersion.Equals(currentCGUVersion, StringComparison.Ordinal))
            {
                dto.RequiresCGUAcceptance |= true;
                dto.Reason = "OUTDATED_VERSION";
            }

            else
            {
                dto.RequiresCGUAcceptance = false;
                dto.Reason = "UP_TO_DATE";
            }
            return dto;



        }
    }
}