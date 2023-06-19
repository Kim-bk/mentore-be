using API.Model.Entities;
using DAL.Entities;
using Mentore.Models.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Mentore.Models
{
    public partial class MentoreContext : DbContext
    {
        public MentoreContext()
        {
        }

        public MentoreContext(DbContextOptions<MentoreContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("data source=.\\SQLEXPRESS14;initial catalog=Mentore;Trusted_Connection=True");
            }
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<BankType> BankType { get; set; }
        public virtual DbSet<Credential> Credential { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<Field> Field { get; set; }
        public virtual DbSet<EntityField> EntityField { get; set; }
        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<Comment> Item { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Mentor> Mentor { get; set; }
        public virtual DbSet<Mentee> Mentee { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<Workshop> Workshop { get; set; }
        public virtual DbSet<Participant> Participant { get; set; }
        public virtual DbSet<Conversation> Coversation { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<MentorPosition> MentorPosition { get; set; }
    }
}
