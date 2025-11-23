using System.ComponentModel.DataAnnotations;

namespace AgenciadeTours.Models
{
    public class Destino
    {
        [Key]
        public int DestinoID { get; set; }

        [Required, StringLength(150)]
        public string Nombre { get; set; }

        [Required]
        public int PaisID { get; set; }
        public Pais Pais { get; set; }

        [Range(0, 365)]
        public int Dias_Duracion { get; set; }

        [Range(0, 23)]
        public int Horas_Duracion { get; set; }
    }
}
