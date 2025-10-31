using System;
using System.Collections.Generic;

namespace ProyectoSauna.Models.Entities;

public partial class Egreso
{
    public int idEgreso { get; set; }

    public string concepto { get; set; } = null!;

    public DateTime fecha { get; set; }

    public decimal monto { get; set; }

    public bool recurrente { get; set; }

    public string? comprobanteRuta { get; set; }

    public int idTipoEgreso { get; set; }

    public int idUsuario { get; set; }

    public virtual TipoEgreso idTipoEgresoNavigation { get; set; } = null!;

    public virtual Usuario idUsuarioNavigation { get; set; } = null!;
}
