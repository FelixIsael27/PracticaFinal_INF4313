using System;
using System.Collections.Generic;

namespace AgenciadeTours.Models.AgenciadeTours;

public partial class Destino
{
    public int DestinoId { get; set; }

    public string Nombre { get; set; } = null!;

    public int PaisId { get; set; }

    public int DiasDuracion { get; set; }

    public int HorasDuracion { get; set; }

    public virtual Pais Pais { get; set; } = null!;

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
