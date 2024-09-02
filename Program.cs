using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HW_UserAuthApp_01_09
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //using (ApplicationContext db = new ApplicationContext())
            //{
            //    db.Database.EnsureCreated();
            //    //db.Database.EnsureDeleted();
            //}

            UserManager um = new UserManager();

            //um.UserRegistration("Max","max123","1234");

            var result = um.UserLogin("max123", "1234");

            //Console.WriteLine(result.Name);




            
            
            
        }
    }



    internal class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-R3LQDV9;Database = testDB2;Trusted_Connection =True;TrustServerCertificate=True");
        }

    }

    internal class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Password { get; private set; }
        public string Email { get; set; }

        private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        public bool CheckPassword(string password)
        {
            var result = passwordHasher.VerifyHashedPassword(this, Password, password);
            return result == PasswordVerificationResult.Success;
        }
        public void SetPassword(string password)
        {
            Password = passwordHasher.HashPassword(this, password);
        }
    }


    internal class UserManager
    {

        public bool UserRegistration(string name,string email,string pass)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if(db.Users.Any(e=>e.Email == email))
                {
                    return false;
                }


                var resultUser = new User 
                {
                    Name = name,
                    Email = email 
                };

                resultUser.SetPassword(pass);

                AddUser(resultUser);

                return true;


            }
        }



        public User? UserLogin(string email, string pass)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User? user = db.Users.FirstOrDefault(e => e.Email == email);

                if(user != null && user.CheckPassword(pass))
                {
                    return user;
                }

                return null;
            }
        }

        public void AddUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            { 
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
    }
}
