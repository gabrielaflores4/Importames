using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Importames.Models
{
    [Table("vehiculos")]
    public class VehiculoModel
    {
        [Key]
        [Column("id_vehiculo")]
        public int IdVehiculo { get; set; }

        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public ClienteModel Cliente { get; set; }

        [Column("id_estado")]
        public int IdEstado { get; set; }

        [ForeignKey("IdEstado")]
        public EstadosModel Estado { get; set; }

        [Column("marca")]
        public string Marca { get; set; }

        [Column("modelo")]
        public string Modelo { get; set; }

        [Column("anio")]
        public int Anio { get; set; }

        [Column("color")]
        public string Color { get; set; }

        [Column("vin")]
        public string Vin { get; set; }

        [Column("costo")]
        public decimal Costo { get; set; }

        [Column("fecha_ingreso")]
        public DateTime FechaIngreso { get; set; }

        public ICollection<HistorialEstadoModel> Historiales { get; set; }
    }
}