using System;
using System.Collections.Generic;

namespace AgenciadeTours.Models.AgenciadeTours;

public partial class Tour
{
    public int TourId { get; set; }

    public string Nombre { get; set; } = null!;

    public int PaisId { get; set; }

    public int DestinoId { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public decimal Precio { get; set; }

    public virtual Destino Destino { get; set; } = null!;

    public virtual Pais Pais { get; set; } = null!;
}
