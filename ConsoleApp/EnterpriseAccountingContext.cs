using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp;

public partial class EnterpriseAccountingContext : DbContext
{
	public EnterpriseAccountingContext()
	{
	}

	public EnterpriseAccountingContext(DbContextOptions<EnterpriseAccountingContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Account> Accounts { get; set; }

	public virtual DbSet<Category> Categories { get; set; }

	public virtual DbSet<Department> Departments { get; set; }

	public virtual DbSet<Employee> Employees { get; set; }

	public virtual DbSet<Operation> Operations { get; set; }

	public virtual DbSet<OperationType> OperationTypes { get; set; }

	public virtual DbSet<Transaction> Transactions { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Account>(entity =>
		{
			entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5869BED45FA");

			entity.Property(e => e.AccountId)
				.HasDefaultValueSql("(newid())")
				.HasColumnName("AccountID");
			entity.Property(e => e.BankName).HasMaxLength(255);
			entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
			entity.Property(e => e.Number).HasMaxLength(255);
			entity.Property(e => e.Type).HasMaxLength(255);

			entity.HasOne(d => d.Department).WithMany(p => p.Accounts)
				.HasForeignKey(d => d.DepartmentId)
				.HasConstraintName("FK__Accounts__Depart__405A880E");
		});

		modelBuilder.Entity<Category>(entity =>
		{
			entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B3EDE9278");

			entity.Property(e => e.CategoryId)
				.HasDefaultValueSql("(newid())")
				.HasColumnName("CategoryID");
			entity.Property(e => e.Description).HasMaxLength(255);
			entity.Property(e => e.Name).HasMaxLength(255);
		});

		modelBuilder.Entity<Department>(entity =>
		{
			entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD4A2AF788");

			entity.Property(e => e.DepartmentId)
				.HasDefaultValueSql("(newid())")
				.HasColumnName("DepartmentID");
			entity.Property(e => e.Name).HasMaxLength(255);
		});

		modelBuilder.Entity<Employee>(entity =>
		{
			entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1B39A0DAA");

			entity.Property(e => e.EmployeeId)
				.HasDefaultValueSql("(newid())")
				.HasColumnName("EmployeeID");
			entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
			entity.Property(e => e.Midname).HasMaxLength(255);
			entity.Property(e => e.Name).HasMaxLength(255);
			entity.Property(e => e.Position).HasMaxLength(255);
			entity.Property(e => e.Surname).HasMaxLength(255);

			entity.HasOne(d => d.Department).WithMany(p => p.Employees)
				.HasForeignKey(d => d.DepartmentId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Employees__Depar__36D11DD4");
		});

		modelBuilder.Entity<Operation>(entity =>
		{
			entity.HasKey(e => e.OperationId).HasName("PK__Operatio__A4F5FC648B735AA7");

			entity.Property(e => e.OperationId)
				.HasDefaultValueSql("(newid())")
				.HasColumnName("OperationID");
			entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
			entity.Property(e => e.Name).HasMaxLength(255);
			entity.Property(e => e.OperationTypeId).HasColumnName("OperationTypeID");

			entity.HasOne(d => d.Category).WithMany(p => p.Operations)
				.HasForeignKey(d => d.CategoryId)
				.HasConstraintName("FK__Operation__Categ__3AA1AEB8");

			entity.HasOne(d => d.OperationType).WithMany(p => p.Operations)
				.HasForeignKey(d => d.OperationTypeId)
				.HasConstraintName("FK__Operation__Opera__3B95D2F1");
		});

		modelBuilder.Entity<OperationType>(entity =>
		{
			entity.HasKey(e => e.OperationTypeId).HasName("PK__Operatio__FF7FE5334693496D");

			entity.Property(e => e.OperationTypeId)
				.HasDefaultValueSql("(newid())")
				.HasColumnName("OperationTypeID");
			entity.Property(e => e.Name).HasMaxLength(255);
		});

		modelBuilder.Entity<Transaction>(entity =>
		{
			entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B3C31C2A8");

			entity.Property(e => e.TransactionId)
				.HasDefaultValueSql("(newid())")
				.HasColumnName("TransactionID");
			entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
			entity.Property(e => e.OperationId).HasColumnName("OperationID");
			entity.Property(e => e.Type).HasMaxLength(255);

			entity.HasOne(d => d.Department).WithMany(p => p.Transactions)
				.HasForeignKey(d => d.DepartmentId)
				.HasConstraintName("FK__Transacti__Depar__451F3D2B");

			entity.HasOne(d => d.Operation).WithMany(p => p.Transactions)
				.HasForeignKey(d => d.OperationId)
				.HasConstraintName("FK__Transacti__Opera__442B18F2");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
