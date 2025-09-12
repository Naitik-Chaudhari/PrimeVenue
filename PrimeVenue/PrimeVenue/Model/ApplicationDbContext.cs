using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrimeVenue.Model;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<EventRequest> EventRequests { get; set; }
    public DbSet<EventTemplate> EventTemplates { get; set; }
    public DbSet<VendorService> VendorServices { get; set; }
    public DbSet<TemplateVendor> TemplateVendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EventRequest>()
            .Property(e => e.Status)
            .HasDefaultValue("Pending");

        modelBuilder.Entity<EventRequest>()
            .Property(e => e.IsOrganized)
            .HasDefaultValue(false);

        modelBuilder.Entity<EventRequest>()
            .Property(e => e.Rating)
            .IsRequired(false);

        modelBuilder.Entity<EventTemplate>()
            .Property(e => e.Status)
            .HasDefaultValue("Draft");

        modelBuilder.Entity<VendorService>()
            .Property(v => v.PriceEstimate)
            .HasDefaultValue(0);

        modelBuilder.Entity<VendorService>()
            .Property(v => v.Rating)
            .HasDefaultValue(0);

        modelBuilder.Entity<TemplateVendor>()
            .Property(tv => tv.Status)
            .HasDefaultValue("Pending");

        // Category - SubCategory
        modelBuilder.Entity<SubCategory>()
            .HasOne(s => s.Category)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Customer (User) - EventRequest (1:N)
        modelBuilder.Entity<EventRequest>()
            .HasOne(r => r.Customer)
            .WithMany(u => u.EventRequests)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // EventRequest - EventTemplate (1:N)
        modelBuilder.Entity<EventTemplate>()
            .HasOne(t => t.EventRequest)
            .WithMany(r => r.Templates)
            .HasForeignKey(t => t.EventRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Organizer (User) - EventTemplate (1:N)
        modelBuilder.Entity<EventTemplate>()
            .HasOne(t => t.Organizer)
            .WithMany()
            .HasForeignKey(t => t.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Vendor (User) - VendorService (1:N)
        modelBuilder.Entity<VendorService>()
            .HasOne(v => v.Vendor)
            .WithMany(u => u.Services)
            .HasForeignKey(v => v.VendorId)
            .OnDelete(DeleteBehavior.Cascade);

        // EventTemplate - VendorService (M:N via TemplateVendor)
        modelBuilder.Entity<TemplateVendor>()
            .HasOne(tv => tv.EventTemplate)
            .WithMany(t => t.TemplateVendors)
            .HasForeignKey(tv => tv.EventTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TemplateVendor>()
            .HasOne(tv => tv.VendorService)
            .WithMany()
            .HasForeignKey(tv => tv.VendorServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}