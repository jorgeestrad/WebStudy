namespace WebStudy.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using WebStudy.Data.Entities;

    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Mapear campos de tipo decimal
            //modelBuilder.Entity<ItemColecction>()
            //     .Property(p => p.Price)
            //     .HasColumnType("decimal(18,2)");
            #endregion
            #region Mapear campos de tipo Texto sin límite de logitud
            //modelBuilder.Entity<Document>()
            //  .Property(p => p.PreservationMetadata)
            //  .HasColumnType("varchar");
            #endregion
            #region Evitar eliminar en Cascada
            var cascadeFKs = modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()).Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
            #endregion
            #region Creación de los índices
            modelBuilder.Entity<Country>()
                .HasIndex(p => new { p.Name })
                .HasName("Uk_Country")
                .IsUnique();
            modelBuilder.Entity<City>()
             .HasIndex(p => new { p.Name })
             .HasName("Uk_City")
             .IsUnique();
            #endregion
        }

    }
}

/*
 * Crear la Base de Datos
 * -------------------------------------------------------------------------------------------------
 * Desde una ventana de comandos cmd y ubicado en el directorio del proyecto Gallery.Web
 * 1) dotnet ef database update
 * 2) dotnet ef migrations add InitialDb
 * 3) dotnet ef database update
 * 
 * 
 * Actualizar la Base de Datos
 * --------------------------------------------------------------------------------------------------
 * Desde una venntana de comandos o desde la ventaga package commadn (Consola de administración de paquetes)
 * 1) dotnet ef migrations add Nombre_dado_ala_migracion
 * 2) dotnet ef database update
 * 
 * Insertar datos (Alimentar la Base de Datos) datos Iniciales
 */
