using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Importames.Models
{
    public class VehiculoModel
    {
        [Key]
        [Column("id_vehiculo")]
        public int IdVehiculo { get; set; }
        public int id_cliente { get; set; }
        public int id_estado { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public int anio { get; set; }
        public string color { get; set; }
        public string vin { get; set; }
        public decimal costo { get; set; }
        public DateTime fecha_ingreso { get; set; }
    }
}