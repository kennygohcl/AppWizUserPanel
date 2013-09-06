using System.Data.Entity;
using dFrontierAppWizard.Core.Model;


namespace dFrontierAppWizard.Data
{
    public class Db : DbContext
    {
        public Db()
        {
            Database.SetInitializer<Db>(null);
        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<ConsumerFeedback> ConsumerFeedbacks { get; set; }
        public DbSet<ConsumerLoyalty> ConsumerLoyalties { get; set; }
        public DbSet<Font> Fonts { get; set; }
        public DbSet<FontSize> FontSizes { get; set; }
        public DbSet<Frequency> Frequencies { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<MediaDocument> MediaDocuments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<NotificationGrouping> NotificationGroupings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PaymentGateway> PaymentGateways { get; set; }
        public DbSet<PushNotificationFrequency> PushNotificationFrequencies { get; set; }
        public DbSet<PushNotificationRegUser> PushNotificationRegUser { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<ServiceUpdate> ServiceUpdates { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<TriggerRadius> TriggerRadius { get; set; }
        public DbSet<UserBillingInformation> UserBillingInformations { get; set; }
        public DbSet<ConsumerBillingInformation> ConsumerBillingInformations { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Promotion>().HasMany(r => r.Products).WithMany(o => o.Promotions).Map(f =>
            {
                f.MapLeftKey("PromotionId");
                f.MapRightKey("ProductId");
            });

            modelBuilder.Entity<User>().HasMany(r => r.Roles).WithMany(o => o.Users).Map(f =>
            {
                f.MapLeftKey("UserId");
                f.MapRightKey("RoleId");
            });

            modelBuilder.Entity<User>().HasMany(r => r.Industries).WithMany(o => o.Users).Map(f =>
            {
                f.MapLeftKey("UserId");
                f.MapRightKey("IndustryId");
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}