using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public partial class BookStoresContext : DbContext
{
    public BookStoresContext()
    {
    }

    public BookStoresContext(DbContextOptions<BookStoresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorBook> AuthorBooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BooksInventory> BooksInventories { get; set; }

    public virtual DbSet<BookstoreInfo> BookstoreInfos { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<InventoryBalance> InventoryBalances { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TitlesPerAuthor> TitlesPerAuthors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-PIO0F9QS;Initial Catalog=BookStores;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("Authors_pk");

            entity.Property(e => e.AuthorId)
                .ValueGeneratedNever()
                .HasColumnName("Author_id");
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_of_Birth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
        });

        modelBuilder.Entity<AuthorBook>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BookId)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("book_id");

            entity.HasOne(d => d.Author).WithMany()
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__AuthorBoo__autho__6E01572D");

            entity.HasOne(d => d.Book).WithMany()
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK__AuthorBoo__book___6EF57B66");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Books__3BF79E0300E916DC");

            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN13");
            entity.Property(e => e.AuthorNo).HasColumnName("Author_no");
            entity.Property(e => e.Language)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ReleaseDate).HasColumnName("Release_Date");
            entity.Property(e => e.Title).HasMaxLength(30);

            entity.HasOne(d => d.AuthorNoNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorNo)
                .HasConstraintName("FK__Books__Author_no__267ABA7A");
        });

        modelBuilder.Entity<BooksInventory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BooksInventory");

            entity.Property(e => e.Booktitle)
                .HasMaxLength(30)
                .HasColumnName("booktitle");
            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN13");
            entity.Property(e => e.NoOfProducts).HasColumnName("No_Of_Products");
            entity.Property(e => e.StoreId).HasColumnName("Store_id");
        });

        modelBuilder.Entity<BookstoreInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("BookstoreInfo");

            entity.Property(e => e.CustomerId).HasColumnName("Customer_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__8CB382B1F07522E7");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("Customer_id");
            entity.Property(e => e.City)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("street");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("zip_code");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C52E0BA82528F404");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("employee_id");
            entity.Property(e => e.Address)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contact_number");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.HireDate).HasColumnName("hire_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("position");
            entity.Property(e => e.StoreId).HasColumnName("Store_id");

            entity.HasOne(d => d.Store).WithMany(p => p.Employees)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Store__5BE2A6F2");
        });

        modelBuilder.Entity<InventoryBalance>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn13 }).HasName("PK__Inventor__834F1EF94752277B");

          

            entity.Property(e => e.StoreId).HasColumnName("Store_id");
            entity.Property(e => e.Isbn13)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("ISBN13");
            entity.Property(e => e.NoOfProducts).HasColumnName("No_Of_Products");

            entity.HasOne(d => d.Book).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__ISBN1__3F466844");

            entity.HasOne(d => d.Store).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Store__3E52440B");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__F1FF8453E1A1211A");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("Order_id");
            entity.Property(e => e.CustomerNo).HasColumnName("Customer_No");
            entity.Property(e => e.OrderDate).HasColumnName("Order_date");
            entity.Property(e => e.OrderPrice).HasColumnName("Order_Price");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Order_status");
            entity.Property(e => e.OrderTime).HasColumnName("Order_time");
            entity.Property(e => e.OrderType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Order_Type");
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("Reference_Number");
            entity.Property(e => e.StoreNo).HasColumnName("Store_No");

            entity.HasOne(d => d.CustomerNoNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerNo)
                .HasConstraintName("Orders_Customers_FK");

            entity.HasOne(d => d.ReferenceNumberNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ReferenceNumber)
                .HasConstraintName("Orders_Books_FK");

            entity.HasOne(d => d.StoreNoNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StoreNo)
                .HasConstraintName("Orders_Orders_FK");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.ShiftId).HasName("PK__Shifts__7B2672209195BAEE");

            entity.Property(e => e.ShiftId)
                .ValueGeneratedNever()
                .HasColumnName("shift_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.ShiftEnd)
                .HasColumnType("datetime")
                .HasColumnName("shift_end");
            entity.Property(e => e.ShiftStart)
                .HasColumnType("datetime")
                .HasColumnName("shift_start");

            entity.HasOne(d => d.Employee).WithMany(p => p.Shifts)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shifts__employee__5EBF139D");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreID).HasName("PK__Stores__A0F0677996872BAB");

            entity.Property(e => e.StoreID)
                .ValueGeneratedNever()
                .HasColumnName("Store_iD");
            entity.Property(e => e.Address)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.StoreName)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TitlesPerAuthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TitlesPerAuthor");

            entity.Property(e => e.InventoryValue).HasColumnName("Inventory value");
            entity.Property(e => e.Name)
                .HasMaxLength(61)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
