using System;
using System.Collections.Generic;

namespace ProyectoSauna.Models.Entities;

public partial class ProgramaFidelizacion
{
    public int idPrograma { get; set; }

    public int visitasParaDescuento { get; set; }

    public decimal porcentajeDescuento { get; set; }

    public bool descuentoCumpleanos { get; set; }

    public decimal montoDescuentoCumpleanos { get; set; }

    public virtual ICollection<Cliente> Cliente { get; set; } = new List<Cliente>();
}
