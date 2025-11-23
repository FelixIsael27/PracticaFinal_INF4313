using System.ComponentModel.DataAnnotations;

namespace AgenciadeTours.Models
{
    public class Pais
    {
        [Key]
        public int PaisID { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        public ICollection<Destino> Destinos { get; set; }
    }
}
