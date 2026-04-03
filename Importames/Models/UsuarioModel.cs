using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Importames.Models
{
    [Table("usuarios")]
    public class UsuarioModel
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("apellido")]
        public string Apellido { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("rol")]
        public string Rol { get; set; }

        [Column("telefono")]
        public string Telefono { get; set; }

        [Column("correo")]
        public string Correo { get; set; }

        public ICollection<HistorialEstadoModel> Historiales { get; set; }
    }
}
