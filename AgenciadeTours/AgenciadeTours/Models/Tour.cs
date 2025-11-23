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
        public decimal ITBIS => CalcularITBIS();

        [NotMapped]
        public TimeSpan Duracion => CalcularDuracion();

        [NotMapped]
        public DateTime FechaFinal => CalcularFechaFinal();

        [NotMapped]
        public string Estado => CalcularEstado();

        private decimal CalcularITBIS() => Math.Round(Precio * 0.18m, 2);

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
