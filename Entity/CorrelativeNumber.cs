using System;
using System.Collections.Generic;

namespace Entity;

public partial class CorrelativeNumber
{
    public int CorrelativeNumberId { get; set; }

    public int? LastNumber { get; set; }

    public int? DigitsQuantity { get; set; }

    public string? Management { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
