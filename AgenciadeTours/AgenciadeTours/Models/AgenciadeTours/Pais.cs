using System;
using System.Collections.Generic;

namespace AgenciadeTours.Models.AgenciadeTours;

public partial class Pais
{
    public int PaisId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Destino> Destinos { get; set; } = new List<Destino>();

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
