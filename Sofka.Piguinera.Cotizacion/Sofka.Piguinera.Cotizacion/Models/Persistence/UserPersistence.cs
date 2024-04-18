using System.ComponentModel.DataAnnotations.Schema;

namespace Sofka.Piguinera.Cotizacion.Models.Persistence
{
    public class UserPersistence
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        [Column("registration_date")]
        public DateTime? RegistrationDate { get; set; }
        public int? Seniority { get; set; }

        public UserPersistence(string email, string password, string userName)
        {
            Email = email;
            Password = password;
            UserName = userName;
        }
    }
}
