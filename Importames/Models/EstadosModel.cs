using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Importames.Models
{
    [Table("estados_veh")]
    public class EstadosModel
    {
        [Key]
        [Column("id_estado")]
        public int IdEstado { get; set; }

        [Column("nombre_estado")]
        public string NombreEstado { get; set; }

        public ICollection<VehiculoModel> Vehiculos { get; set; }
        public ICollection<HistorialEstadoModel> Historiales { get; set; }
    }
}
