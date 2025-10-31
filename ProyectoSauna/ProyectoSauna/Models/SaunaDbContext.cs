using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoSauna.Models.Entities;

namespace ProyectoSauna.Models;

public partial class SaunaDbContext : DbContext
{
    public SaunaDbContext()
    {
    }

    public SaunaDbContext(DbContextOptions<SaunaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaProducto> CategoriaProducto { get; set; }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<Comprobante> Comprobante { get; set; }

    public virtual DbSet<Cuenta> Cuenta { get; set; }

    public virtual DbSet<DetalleConsumo> DetalleConsumo { get; set; }

    public virtual DbSet<Egreso> Egreso { get; set; }

    public virtual DbSet<EstadoCuenta> EstadoCuenta { get; set; }

    public virtual DbSet<MetodoPago> MetodoPago { get; set; }

    public virtual DbSet<MovimientoInventario> MovimientoInventario { get; set; }

    public virtual DbSet<Pago> Pago { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<ProgramaFidelizacion> ProgramaFidelizacion { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<TipoComprobante> TipoComprobante { get; set; }

    public virtual DbSet<TipoEgreso> TipoEgreso { get; set; }

    public virtual DbSet<TipoMovimiento> TipoMovimiento { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ProyectoSauna;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.idCategoriaProducto).HasName("PK__Categori__88F047C555FCF468");

            entity.Property(e => e.nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.idCliente).HasName("PK__Cliente__885457EE4CCC735D");

            entity.HasIndex(e => e.numeroDocumento, "UQ__Cliente__4CC511E4F1D4E1D3").IsUnique();

            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.fechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.numeroDocumento)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.idProgramaNavigation).WithMany(p => p.Cliente)
                .HasForeignKey(d => d.idPrograma)
                .HasConstraintName("FK_Cliente_ProgramaFidelizacion");
        });

        modelBuilder.Entity<Comprobante>(entity =>
        {
            entity.HasKey(e => e.idComprobante).HasName("PK__Comproba__BF4D8CF34B5B65EF");

            entity.HasIndex(e => new { e.serie, e.numero, e.idTipoComprobante }, "UQ_Comprobante_SerieNumero").IsUnique();

            entity.Property(e => e.fechaEmision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.igv).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.numero)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.serie)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.subtotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.idCuentaNavigation).WithMany(p => p.Comprobante)
                .HasForeignKey(d => d.idCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comprobante_Cuenta");

            entity.HasOne(d => d.idTipoComprobanteNavigation).WithMany(p => p.Comprobante)
                .HasForeignKey(d => d.idTipoComprobante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comprobante_TipoComprobante");
        });

        modelBuilder.Entity<Cuenta>(entity =>
        {
            entity.HasKey(e => e.idCuenta).HasName("PK__Cuenta__BBC6DF323D9E8792");

            entity.Property(e => e.descuentos).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.fechaHoraIngreso)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.fechaHoraSalida).HasColumnType("datetime");
            entity.Property(e => e.horaEntrada).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.montoPagado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.saldo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.subtotalConsumos).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.idClienteNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.idCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cuenta_Cliente");

            entity.HasOne(d => d.idEstadoCuentaNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.idEstadoCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cuenta_EstadoCuenta");

            entity.HasOne(d => d.idUsuarioCreadorNavigation).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.idUsuarioCreador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cuenta_Usuario");
        });

        modelBuilder.Entity<DetalleConsumo>(entity =>
        {
            entity.HasKey(e => e.idDetalle).HasName("PK__DetalleC__49CAE2FBBE720DAD");

            entity.Property(e => e.precioUnitario).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.subtotal).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.idCuentaNavigation).WithMany(p => p.DetalleConsumo)
                .HasForeignKey(d => d.idCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleConsumo_Cuenta");

            entity.HasOne(d => d.idProductoNavigation).WithMany(p => p.DetalleConsumo)
                .HasForeignKey(d => d.idProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleConsumo_Producto");
        });

        modelBuilder.Entity<Egreso>(entity =>
        {
            entity.HasKey(e => e.idEgreso).HasName("PK__Egreso__0542D3E11B5F617F");

            entity.Property(e => e.comprobanteRuta)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.concepto)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.monto).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.idTipoEgresoNavigation).WithMany(p => p.Egreso)
                .HasForeignKey(d => d.idTipoEgreso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Egreso_TipoEgreso");

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.Egreso)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Egreso_Usuario");
        });

        modelBuilder.Entity<EstadoCuenta>(entity =>
        {
            entity.HasKey(e => e.idEstadoCuenta).HasName("PK__EstadoCu__45723170F126FA03");

            entity.Property(e => e.nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.idMetodoPago).HasName("PK__MetodoPa__817BFC325A832D1B");

            entity.Property(e => e.nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MovimientoInventario>(entity =>
        {
            entity.HasKey(e => e.idMovimiento).HasName("PK__Movimien__628521734216A6B1");

            entity.Property(e => e.costoTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.costoUnitario).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.idProductoNavigation).WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.idProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientoInventario_Producto");

            entity.HasOne(d => d.idTipoMovimientoNavigation).WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.idTipoMovimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientoInventario_TipoMovimiento");

            entity.HasOne(d => d.idUsuarioNavigation).WithMany(p => p.MovimientoInventario)
                .HasForeignKey(d => d.idUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientoInventario_Usuario");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.idPago).HasName("PK__Pago__BD2295AD2A69BA71");

            entity.Property(e => e.fechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.monto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.numeroReferencia)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.idCuentaNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.idCuenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Cuenta");

            entity.HasOne(d => d.idMetodoPagoNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.idMetodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_MetodoPago");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.idProducto).HasName("PK__Producto__07F4A132DC32ABB4");

            entity.HasIndex(e => e.codigo, "UQ__Producto__40F9A2069487323C").IsUnique();

            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.codigo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.precioCompra).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.precioVenta).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.idCategoriaProductoNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.idCategoriaProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_CategoriaProducto");
        });

        modelBuilder.Entity<ProgramaFidelizacion>(entity =>
        {
            entity.HasKey(e => e.idPrograma).HasName("PK__Programa__467DDFD690C1A36F");

            entity.Property(e => e.montoDescuentoCumpleanos).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.porcentajeDescuento).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.idRol).HasName("PK__Rol__3C872F76BB123BAD");

            entity.Property(e => e.nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoComprobante>(entity =>
        {
            entity.HasKey(e => e.idTipoComprobante).HasName("PK__TipoComp__C77122F31032E1F6");

            entity.Property(e => e.nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoEgreso>(entity =>
        {
            entity.HasKey(e => e.idTipoEgreso).HasName("PK__TipoEgre__BE0BB29E60E7EC30");

            entity.Property(e => e.nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoMovimiento>(entity =>
        {
            entity.HasKey(e => e.idTipoMovimiento).HasName("PK__TipoMovi__F3C074E106B9EC6E");

            entity.Property(e => e.nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.tipo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.idUsuario).HasName("PK__Usuario__645723A6863FD33F");

            entity.HasIndex(e => e.nombreUsuario, "UQ__Usuario__A0436BD7A476B26F").IsUnique();

            entity.Property(e => e.activo).HasDefaultValue(true);
            entity.Property(e => e.contraseniaHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.correo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.fechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.nombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.idRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.idRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
