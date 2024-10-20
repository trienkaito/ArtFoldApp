using ArtFold.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace ArtFold.Data
{
    public class ArtFoldDbContext : IdentityDbContext<User> 
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CheckOutProduct> CheckOutProducts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CheckOut> CheckOuts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentImage> CommentsImage { get; set; }

        public ArtFoldDbContext(DbContextOptions<ArtFoldDbContext> options) : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CartProduct>()
                .HasKey(cp => new { cp.CartID, cp.ProductID });

            builder.Entity<CartProduct>()
                .HasOne(cp => cp.Cart)
                .WithMany(c => c.CartProducts)
                .HasForeignKey(cp => cp.CartID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.CartProducts)
                .HasForeignKey(cp => cp.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CheckOut>()
                .HasKey(co => co.CheckOutID);

            builder.Entity<CheckOut>()
                .HasOne(co => co.User)
                .WithMany(u => u.CheckOuts) // Assuming User has a collection of CheckOuts
                .HasForeignKey(co => co.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CheckOutProduct>()
               .HasKey(cop => cop.CheckOutProductID);

            builder.Entity<CheckOutProduct>()
                .HasOne(cop => cop.CheckOut)
                .WithMany(co => co.CheckOutProducts)
                .HasForeignKey(cop => cop.CheckOutID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CheckOutProduct>()
                .HasOne(cop => cop.Product)
                .WithMany() // Assuming you don't need a collection in Product for CheckOutProducts
                .HasForeignKey(cop => cop.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment relationship
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // CommentImage relationship
            builder.Entity<CommentImage>()
                .HasOne(ci => ci.Comment)
                .WithMany(c => c.CommentImages)
                .HasForeignKey(ci => ci.CommentID)
                .OnDelete(DeleteBehavior.Cascade);


            SeedData(builder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var adminRoleId = Guid.NewGuid().ToString();
            var customerRoleId = Guid.NewGuid().ToString();

            var hasher = new PasswordHasher<User>();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = customerRoleId,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                }
            );

            var users = new List<User>
            {
                new User { Id = Guid.NewGuid().ToString(), UserName = "Admin", Email = "bluegameming292003@gmail.com", EmailConfirmed = true, FullName = "Trần Minh Quốc Khánh", PhoneNumber = "0934763210", CreatedAt = DateTime.Now, PasswordHash = hasher.HashPassword(null, "admin") },
                new User { Id = Guid.NewGuid().ToString(), UserName = "TaiModel", Email = "taimodel@gmail.com", EmailConfirmed = true, FullName = "Nguyễn Lương Tài", PhoneNumber = "0123456789", CreatedAt = DateTime.Now, PasswordHash = hasher.HashPassword(null, "tai123") },
                new User { Id = Guid.NewGuid().ToString(), UserName = "MinhThu", Email = "dinhuynhminhthu@gmail.com", EmailConfirmed = true, FullName = "Đinh Huỳnh Minh Thư", PhoneNumber = "0123456789", CreatedAt = DateTime.Now, PasswordHash = hasher.HashPassword(null, "thu123") },
                new User { Id = Guid.NewGuid().ToString(), UserName = "NgocHa", Email = "ngocha@gmail.com", EmailConfirmed = true, FullName = "Ngọc Hà", PhoneNumber = "0123456789", CreatedAt = DateTime.Now, PasswordHash = hasher.HashPassword(null, "ha123") },
                new User { Id = Guid.NewGuid().ToString(), UserName = "NgocHan", Email = "nguyenvungochan@gmail.com", EmailConfirmed = true, FullName = "Nguyễn Vũ Ngọc Hân", PhoneNumber = "0123456789", CreatedAt = DateTime.Now, PasswordHash = hasher.HashPassword(null, "han123") },
                new User { Id = Guid.NewGuid().ToString(), UserName = "ThuIT", Email = "thuIT@gmail.com", EmailConfirmed = true, FullName = "Thư AI", PhoneNumber = "0123456789", CreatedAt = DateTime.Now, PasswordHash = hasher.HashPassword(null, "thuit123") }
            };


            var carts = new List<Cart>
            {
                new Cart { CartID = Guid.NewGuid(), UserID = users[0].Id },
                new Cart { CartID = Guid.NewGuid(), UserID = users[1].Id },
                new Cart { CartID = Guid.NewGuid(), UserID = users[2].Id },
                new Cart { CartID = Guid.NewGuid(), UserID = users[3].Id },
                new Cart { CartID = Guid.NewGuid(), UserID = users[4].Id },
                new Cart { CartID = Guid.NewGuid(), UserID = users[5].Id }
            };


            modelBuilder.Entity<IdentityUserRole<string>>().HasData
            (
                new IdentityUserRole<string> { RoleId = adminRoleId, UserId = users[0].Id },
                new IdentityUserRole<string> { RoleId = customerRoleId, UserId = users[1].Id },
                new IdentityUserRole<string> { RoleId = customerRoleId, UserId = users[2].Id },
                new IdentityUserRole<string> { RoleId = customerRoleId, UserId = users[3].Id },
                new IdentityUserRole<string> { RoleId = customerRoleId, UserId = users[4].Id },
                new IdentityUserRole<string> { RoleId = customerRoleId, UserId = users[5].Id }  
            );

            var categories = new List<Category>
            {
                new Category { CategoryID = Guid.NewGuid(), CategoryName = "Anime" },
                new Category { CategoryID = Guid.NewGuid(), CategoryName = "Vehicle" },
                new Category { CategoryID = Guid.NewGuid(), CategoryName = "Marvel" },
                new Category { CategoryID = Guid.NewGuid(), CategoryName = "Architecture" }
            };


            var products = new List<Product>
            {
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Anime").CategoryID,
                    Name = "Son Goku",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/ea93877ccd8d3700b6b9ede4220df541.webp",
                    PrintPaperType = "A4",
                    Price = 50000.0,
                    ProductQuantity = 50,
                    Description = "Bộ sản phẩm Mô hình giấy Anime Goku SSJ HD – Dragon Ball bao gồm:\r\n- 25 tờ kit mô hình.\r\n- Kích thước: Cao: 55,5cm x Rộng: 13,4cm x Sâu: 23,9cm",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,   
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Anime").CategoryID,
                    Name = "Monkey D. Luffy",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/e82a586f3d146ea83a3b6303b4668914.webp",
                    PrintPaperType = "A4",
                    Price = 55000.0,
                    ProductQuantity = 100,
                    Description = "Bộ sản phẩm Mô hình giấy Anime Chibi Monkey D Luffy - One Piece bao gồm:\r\n- 18 tờ kit mô hình.\r\n- Kích thước: Cao: 40cm x Rộng: 23,4cm x Sâu: 21,6cm",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Anime").CategoryID,
                    Name = "Uzumaki Naruto",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/sg-11134201-22110-igsmlbzefhkvf0.webp",
                    PrintPaperType = "A4",
                    Price = 42000.0,
                    ProductQuantity = 30,
                    Description = "Bộ sản phẩm Mô hình giấy Anime Game Uzumaki Naruto ver 3 bao gồm:\r\n- 6 tờ kit mô hình.\r\n(Mặc định bản kit sẽ được in bản có line, nếu bạn muốn in bản ko line trong đơn hàng bạn ghi chú là \"in bản ko line\" để shop cho in nhé)\r\n- Kích thước A4: Cao: 17cm x Rộng: 20,1cm x Sâu: 28,3cm.\r\nXuất xứ: Việt Nam",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Anime").CategoryID,
                    Name = "Pikachu Polygon ver 2",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-ls9lvceatuah97@resize_w450_nl.webp",
                    PrintPaperType = "A4",
                    Price = 59000.0,
                    ProductQuantity = 30,
                    Description = "Bộ sản phẩm Mô hình giấy Anime Game Pokemon Pikachu Polygon ver 2 bao gồm:\r\n- 9 tờ kit mô hình in mực Dầu trên giấy Màu.\r\n- 4 tờ hướng dẫn lắp ráp.\r\n- Kích thước A4: Cao: 33cm x Rộng: 30cm x Sâu: 34cm.\r\nXuất xứ: Việt Nam",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Anime").CategoryID,
                    Name = "Chibi Levi Ackerman",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/a6da3b4677bd9309784051610617a5e7@resize_w450_nl.webp",
                    PrintPaperType = "A4",
                    Price = 14000.0,
                    ProductQuantity = 80,
                    Description = "Bộ sản phẩm Mô hình giấy Anime Chibi Levi Ackerman ver 3 – Attack on Titan bao gồm:\r\n- 6 tờ kit mô hình.\r\n- Kích thước: Cao: 20,3cm x Rộng: 11,1cm x Sâu: 18cm",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Vehicle").CategoryID,
                    Name = "Space Shuttle Atlantis",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/4ed6a6e35f435d28286762c02db7f911.webp",
                    PrintPaperType = "A4",
                    Price = 72000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy phi thuyền không gian vũ trụ tàu con thoi Space Shuttle Atlantis bao gồm:\r\n- 11 tờ kit mô hình.\r\n- 1 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Vehicle").CategoryID,
                    Name = "Lamborghini Sesto Elemento",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/966ca26a8de1b2f34c66449cc74e48bd.webp",
                    PrintPaperType = "A4",
                    Price = 69000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy xe ô tô Lamborghini Sesto Elemento bao gồm:\r\n- 3 tờ kit mô hình.\r\n- Kích thước: Cao: 4,9cm x Rộng: 8,6cm x Sâu: 18,1cm",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Vehicle").CategoryID,
                    Name = "Prototype Technology Group BMW",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/9fb112bf0fe8b6b773c0aa7411a2392c.webp",
                    PrintPaperType = "A4",
                    Price = 79000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy xe ô tô Prototype Technology Group BMW bao gồm:\r\n- 6 tờ kit mô hình.\r\n- 1 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Vehicle").CategoryID,
                    Name = "Mille Miglia Custom Chopper Bike",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/2fbbe89ee72a717b7f2bed3a84d8b259.webp",
                    PrintPaperType = "A4",
                    Price = 149000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy xe máy Mille Miglia Custom Chopper bao gồm:\r\n- 24 tờ kit mô hình.\r\n- 8 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Vehicle").CategoryID,
                    Name = "Boeing 777-200 British Airways",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/a09cfa936019a5e6c493acafbd4a13e1.webp",
                    PrintPaperType = "A4",
                    Price = 58000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy máy bay Boeing 777-200 British Airways bao gồm:\r\n- 8 tờ kit mô hình.\r\n- 1 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Marvel").CategoryID,
                    Name = "Robot Iron Man Mark VII",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/5fc4fc6d877bc7c905b6f92eeb951a94.webp",
                    PrintPaperType = "A4",
                    Price = 105000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy Marvel Avenger Robot Iron Man Mark VII bao gồm:\r\n- 16 tờ kit mô hình.\r\n- 3 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Marvel").CategoryID,
                    Name = "Chibi Thor ",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-lmua3ev8pza778.webp",
                    PrintPaperType = "A4",
                    Price = 50000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy Anime Game Chibi Thor mập - Marvel bao gồm:\r\n- 8 tờ kit mô hình in mực Dầu trên giấy Màu.\r\n- 2 tờ hướng dẫn lắp ráp.\r\n- Kích thước : Cao 15,5cm x Rộng 13cm x Sâu 9cm.\r\nXuất xứ: Việt Nam",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Marvel").CategoryID,
                    Name = "Marvel Hulk Wall Hanging",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/8aedf29f64c9de9ac7ec2b3f48182f7b.webp",
                    PrintPaperType = "A4",
                    Price = 83000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy Anime Game Marvel Hulk Treo tường ver 3 bao gồm:\r\n– 17 tờ kit mô hình in trên giấy A4 Ford màu định lượng 180gsm (so với giấy photo là 70gsm) + scan code xem hướng dẫn.\r\n- Kích thước: Cao: khoảng 40cm",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Marvel").CategoryID,
                    Name = "Marvel Avengers Iron Spider",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/4b925257b8c606d8ba5549860b146ad1.webp",
                    PrintPaperType = "A4",
                    Price = 100000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy Marvel Avengers Iron Spider bao gồm:\r\n- 15 tờ kit mô hình.\r\n- Kích thước: Cao: 38cm x Rộng: 30,7cm x Sâu: 34,5cm",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Marvel").CategoryID,
                    Name = "Chibi Doctor Strange",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/vn-11134207-7r98o-lzad737x2krla7@resize_w450_nl.webp",
                    PrintPaperType = "A4",
                    Price = 25000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy Anime Game Chibi Doctor Strange - Marvel bao gồm:\r\n- 2 tờ kit mô hình + kèm scan code xem video hướng dẫn lắp ráp.\r\n* Xuất xứ: Việt Nam",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Architecture").CategoryID,
                    Name = "Neuschwanstein Castle - Germany",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/d50b7f9c059c8cb8e7c0654954a08ab1.webp",
                    PrintPaperType = "A4",
                    Price = 55000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy kiến trúc lâu đài Đức Neuschwanstein Castle - Germany bao gồm:\r\n- 8 tờ kit mô hình.\r\n- 2 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Architecture").CategoryID,
                    Name = "Tower of London – England",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/5e96e9613e2fd22d255d9d90159d19ce.webp",
                    PrintPaperType = "A4",
                    Price = 65000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy kiến trúc Tháp Luân Đôn Tower of London – England bao gồm:\r\n- 10 tờ kit mô hình.\r\n- 2 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Architecture").CategoryID,
                    Name = "Eiffel Tower",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/a077c0d85e3866a441e4b1e76ab69dbb.webp",
                    PrintPaperType = "A4",
                    Price = 60000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy kiến trúc Pháp tháp Eiffel Tower bao gồm:\r\n- 9 tờ kit mô hình.\r\n- 1 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Architecture").CategoryID,
                    Name = "Cambuchia Angkor Wat",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/edb6286c7abf2d62a36a911b5d0983d4.webp",
                    PrintPaperType = "A4",
                    Price = 156000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy kiến trúc Cambuchia Angkor Wat bao gồm:\r\n- 24 tờ kit mô hình.\r\n- 3 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Product
                {
                    ProductID = Guid.NewGuid(),
                    CategoryID = categories.First(c => c.CategoryName == "Architecture").CategoryID,
                    Name = "Siena Cathedral - Italy",
                    ImgUrl = "https://down-vn.img.susercontent.com/file/e7ac1e43b3160334e9ca1fc66da7f34a.webp",
                    PrintPaperType = "A4",
                    Price = 124000.0,
                    ProductQuantity = 10,
                    Description = "Bộ sản phẩm Mô hình giấy kiến trúc Nhà thờ chính Siena Cathedral - Italy bao gồm:\r\n- 19 tờ kit mô hình.\r\n- 4 tờ hướng dẫn lắp ráp.",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
            };

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Product>().HasData(products);
            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity<Cart>().HasData(carts);
        }
    }
}
