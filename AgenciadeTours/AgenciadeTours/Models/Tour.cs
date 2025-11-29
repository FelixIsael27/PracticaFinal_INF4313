using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciadeTours.Models
{
    public class Tour
    {
        [Key]
        public int TourID { get; set; }

        [Required, StringLength(200)]
        public string Nombre { get; set; }

        [Required]
        public int PaisID { get; set; }
        public Pais Pais { get; set; }

        [Required]
        public int DestinoID { get; set; }
        public Destino Destino { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Precio { get; set; }

        [NotMapped]
        public decimal ITBIS { get; set; }

        [NotMapped]
        public TimeSpan Duracion { get; set; }

        [NotMapped]
        public DateTime FechaFinal
        {
            get
            { 
                return CalcularFechaFinal(); 
            }
            set { }
        }

        [NotMapped]
        public string Estado
        {
            get 
            { 
                return CalcularEstado(); 
            }
            set { }
        }

        private TimeSpan CalcularDuracion()
        {
            if (Destino == null) return TimeSpan.Zero;
            return TimeSpan.FromDays(Destino.Dias_Duracion) + TimeSpan.FromHours(Destino.Horas_Duracion);
        }

        private DateTime CalcularFechaFinal()
        {
            var start = Fecha.Date + Hora;
            return start + Duracion;
        }

        private string CalcularEstado()
        {
            var now = DateTime.Now;
            var start = Fecha.Date + Hora;
            return (start >= now) ? "Vigente" : "No vigente";
        }
    }
}
