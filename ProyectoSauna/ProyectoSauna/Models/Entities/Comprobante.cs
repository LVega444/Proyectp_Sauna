﻿using System;
using System.Collections.Generic;

namespace ProyectoSauna.Models.Entities;

public partial class Comprobante
{
    public int idComprobante { get; set; }

    public string serie { get; set; } = null!;

    public string numero { get; set; } = null!;

    public DateTime fechaEmision { get; set; }

    public decimal subtotal { get; set; }

    public decimal igv { get; set; }

    public decimal total { get; set; }

    public int idTipoComprobante { get; set; }

    public int idCuenta { get; set; }

    public virtual Cuenta idCuentaNavigation { get; set; } = null!;

    public virtual TipoComprobante idTipoComprobanteNavigation { get; set; } = null!;
}
