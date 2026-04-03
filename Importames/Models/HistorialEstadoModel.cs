using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Importames.Models
{
    [Table("historial_estado")]
    public class HistorialEstadoModel
    {
        [Key]
        [Column("id_historial")]
        public int IdHistorial { get; set; }

        [Column("id_vehiculo")]
        public int IdVehiculo { get; set; }

        [ForeignKey("IdVehiculo")]
        public VehiculoModel Vehiculo { get; set; }

        [Column("id_estado")]
        public int IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public EstadosModel Estado { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public UsuarioModel Usuario { get; set; }

        [Column("fecha_cambio")]
        public DateTime FechaCambio { get; set; }
    }
}
