using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Importames.Models
{
    [Table("clientes")]
    public class ClienteModel
    {
            [Key]
            [Column("id_cliente")]
            public int IdCliente { get; set; }

            [Column("nombre")]
            public string Nombre { get; set; }

            [Column("apellido")]
            public string Apellido { get; set; }

            [Column("telefono")]
            public string Telefono { get; set; }

            [Column("correo")]
            public string Correo { get; set; }

            [Column("direccion")]
            public string Direccion { get; set; }

            [Column("fecha_registro")]
            public DateTime FechaRegistro { get; set; }

            [Column("dui")]
            public string Dui { get; set; }
            public ICollection<VehiculoModel> Vehiculos { get; set; }
        }
}
