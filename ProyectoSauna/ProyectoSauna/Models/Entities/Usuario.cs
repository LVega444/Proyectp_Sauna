using System;
using System.Collections.Generic;

namespace ProyectoSauna.Models.Entities;

public partial class Usuario
{
    public int idUsuario { get; set; }

    public string nombreUsuario { get; set; } = null!;

    public string contraseniaHash { get; set; } = null!;

    public string correo { get; set; } = null!;

    public DateTime fechaCreacion { get; set; }

    public bool activo { get; set; }

    public int idRol { get; set; }

    public virtual ICollection<Cuenta> Cuenta { get; set; } = new List<Cuenta>();

    public virtual ICollection<Egreso> Egreso { get; set; } = new List<Egreso>();

    public virtual ICollection<MovimientoInventario> MovimientoInventario { get; set; } = new List<MovimientoInventario>();

    public virtual Rol idRolNavigation { get; set; } = null!;
}
