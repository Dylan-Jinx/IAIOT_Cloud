using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IAIOT_alpha_0_1_1.Models
{
    public partial class IAIOTCloudContext : DbContext
    {
        public IAIOTCloudContext()
        {
        }

        public IAIOTCloudContext(DbContextOptions<IAIOTCloudContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TActuator> TActuator { get; set; }
        public virtual DbSet<TDevices> TDevices { get; set; }
        public virtual DbSet<TProjects> TProjects { get; set; }
        public virtual DbSet<TSensorData> TSensorData { get; set; }
        public virtual DbSet<TSensors> TSensors { get; set; }
        public virtual DbSet<TSysLog> TSysLog { get; set; }
        public virtual DbSet<TSysRole> TSysRole { get; set; }
        public virtual DbSet<TSysUsers> TSysUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=175.24.88.45;Initial Catalog=IAIOTCloud;User ID=sa;Password=Jinx13850526746");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TActuator>(entity =>
            {
                entity.HasKey(e => e.ActuatorId);

                entity.ToTable("tActuator");

                entity.Property(e => e.ActuatorName).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TDevices>(entity =>
            {
                entity.HasKey(e => e.DeviceId);

                entity.ToTable("tDevices");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DeviceTag).HasMaxLength(50);
            });

            modelBuilder.Entity<TProjects>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.ToTable("tProjects");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProjectTag)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProjectType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TSensorData>(entity =>
            {
                entity.HasKey(e => e.SensorDataId);

                entity.ToTable("tSensorData");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DeviceName).HasMaxLength(50);

                entity.Property(e => e.SensorData).HasMaxLength(50);

                entity.Property(e => e.SensorName).HasMaxLength(50);

                entity.Property(e => e.SensorTag).HasMaxLength(50);

                entity.Property(e => e.SensorUnit).HasMaxLength(50);
            });

            modelBuilder.Entity<TSensors>(entity =>
            {
                entity.HasKey(e => e.SensorId);

                entity.ToTable("tSensors");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DataType).HasMaxLength(50);

                entity.Property(e => e.SensorName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SensorTag)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SerializeNum).HasMaxLength(50);

                entity.Property(e => e.Unit).HasMaxLength(10);
            });

            modelBuilder.Entity<TSysLog>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.ToTable("tSysLog");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TSysRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("tSysRole");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TSysUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_tSystemUsers");

                entity.ToTable("tSysUsers");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.LastLogonDate).HasColumnType("datetime");

                entity.Property(e => e.Password).HasMaxLength(20);

                entity.Property(e => e.Telephone).HasMaxLength(11);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
