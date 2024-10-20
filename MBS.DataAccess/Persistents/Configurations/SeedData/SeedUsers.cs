using MBS.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBS.DataAccess.Persistents.Configurations.SeedData
{
    public class SeedUsers
    {
        private readonly DbInitializer _dbInitializer;
        private readonly PasswordHasher<ApplicationUser> _hasher = new PasswordHasher<ApplicationUser>();

        public SeedUsers(DbInitializer dbInitializer)
        {
            _dbInitializer = dbInitializer;
        }

        public void SeedingUsers()
        {
            // Seed Roles

            // Seed Students
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = Guid.Parse("aa2a71b6-4f0b-453e-bd09-1b1d8b2ab978").ToString(),
                    UserName = "phamhoangminhkhoi",
                    NormalizedUserName = "PHAMHOANGMINHKHOI",
                    Email = "phamhoangminhkhoi@gmail.com",
                    NormalizedEmail = "PHAMHOANGMINHKHOI@GMAIL.COM",
                    FullName = "Phạm Hoàng Minh Khôi",
                    Gender = "Male",
                    Birthday = new DateTime(2003, 5, 15),
                    AvatarUrl = "https://example.com/avatar1.png",
                    EmailConfirmed = true,
                    PasswordHash = _hasher.HashPassword(null, "123456aA@"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0123456789",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("b47d87c9-f755-4989-9092-6481f517b435").ToString(),
                    UserName = "nguyenthanhtu",
                    NormalizedUserName = "NGUYENTHANHTU",
                    Email = "nguyenthanhtu@gmail.com",
                    NormalizedEmail = "NGUYENTHANHTU@GMAIL.COM",
                    FullName = "Nguyễn Thanh Tú",
                    Gender = "Male",
                    Birthday = new DateTime(2003, 8, 22),
                    AvatarUrl = "https://example.com/avatar2.png",
                    EmailConfirmed = true,
                    PasswordHash = _hasher.HashPassword(null, "123456aA@"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0987654321",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("bf1a21d1-c349-4d80-9382-1f215d52923f").ToString(),
                    UserName = "leminhduc",
                    NormalizedUserName = "LEMINHDUC",
                    Email = "leminhduc@yahoo.com.vn",
                    NormalizedEmail = "LEMINHDUC@YAHOO.COM.VN",
                    FullName = "Lê Minh Đức",
                    Gender = "Male",
                    Birthday = new DateTime(2003, 12, 10),
                    AvatarUrl = "https://example.com/avatar3.png",
                    EmailConfirmed = true,
                    PasswordHash = _hasher.HashPassword(null, "123456aA@"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0912345678",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("df8d2a56-5b3e-4d5a-8cba-5d5b7a2d9e31").ToString(),
                    UserName = "tranvanha",
                    NormalizedUserName = "TRANVANHA",
                    Email = "tranvanha@gmail.com",
                    NormalizedEmail = "TRANVANHA@GMAIL.COM",
                    FullName = "Trần Văn Hà",
                    Gender = "Male",
                    Birthday = new DateTime(2003, 3, 5),
                    AvatarUrl = "https://example.com/avatar4.png",
                    EmailConfirmed = true,
                    PasswordHash = _hasher.HashPassword(null, "123456aA@"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0845678901",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("cfa39c57-e9d7-4f5d-bb7f-5c8b8fd02890").ToString(),
                    UserName = "ngocanh",
                    NormalizedUserName = "NGOCANH",
                    Email = "ngocanh@yahoo.com.vn",
                    NormalizedEmail = "NGOCANH@YAHOO.COM.VN",
                    FullName = "Ngọc Anh",
                    Gender = "Female",
                    Birthday = new DateTime(2003, 7, 19),
                    AvatarUrl = "https://example.com/avatar5.png",
                    EmailConfirmed = true,
                    PasswordHash = _hasher.HashPassword(null, "123456aA@"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0976543210",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("a1d2c1f5-45f7-4db2-b67b-69edab349e2f").ToString(),
                    UserName = "hoangnguyen",
                    NormalizedUserName = "HOANGNGUYEN",
                    Email = "hoangnguyen@gmail.com",
                    NormalizedEmail = "HOANGNGUYEN@GMAIL.COM",
                    FullName = "Hoàng Nguyễn",
                    Gender = "Male",
                    Birthday = new DateTime(1985, 4, 22),
                    AvatarUrl = "https://example.com/avatar6.png",
                    EmailConfirmed = true,
                    PasswordHash = _hasher.HashPassword(null, "123456aA@"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0908765432",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("b4a11fdd-fd15-4c7d-931f-4e35b733da12").ToString(),
                    UserName = "tranthilinh",
                    NormalizedUserName = "TRANTHILINH",
                    Email = "tranthilinh@yahoo.com.vn",
                    NormalizedEmail = "TRANTHILINH@YAHOO.COM.VN",
                    FullName = "Trần Thị Linh",
                    Gender = "Female",
                    Birthday = new DateTime(1990, 2, 11),
                    AvatarUrl = "https://example.com/avatar7.png",
                    EmailConfirmed = true,
                    PasswordHash = _hasher.HashPassword(null, "123456aA@"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0854321098",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                },
                new ApplicationUser
                {
                    Id = Guid.Parse("c7bcd8a8-f33f-4ec8-98f7-1293b215d511").ToString(),
                    UserName = "danghuong",
                    NormalizedUserName = "DANGHUONG",
                    Email = "danghuong@gmail.com",
                    NormalizedEmail = "DANGHUONG@GMAIL.COM",
                    FullName = "Đặng Hương",
                    Gender = "Female",
                    Birthday = new DateTime(1975, 6, 30),
                    AvatarUrl = "https://example.com/avatar8.png",
                    EmailConfirmed = true,
                    PasswordHash = _hasher.HashPassword(null, "123456aA@"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "0967890123",
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                }

            };

            _dbInitializer.Initialize(users);

        }

    }
}
