﻿using MBS.Core.Entities;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess
{
    public class MBSContextSeed
    {
        public static async Task SeedUserAsync(MBSContext context, UserManager<ApplicationUser> userManager)
        {
            var adminRoleId = "8fa7c7bb-daa5-a660-bf02-82301a5eb327";
            var hasher = new PasswordHasher<ApplicationUser>();
            if (!userManager.Users.Any())
            {
                var adminUser = new ApplicationUser
                {
                    Id = adminRoleId,
                    Gender = "Male", // Set appropriate value
                    FullName = "Admin User",
                    AvatarUrl = "https://example.com/avatar.png", // Set appropriate value
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    //PasswordHash = hasher.HashPassword(null, "123456"),
                    //PasswordHash = "123456",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                };
                //var adminUser = new ApplicationUser { UserName = "admin", Email = "admin@admin.com", EmailConfirmed = false };
                var result = await userManager.CreateAsync(adminUser, "123456aA@");
                //await context.SaveChangesAsync();
                //assign role to admin user
                var role = UserRoleEnum.Admin.ToString();
                await userManager.AddToRoleAsync(adminUser, role);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedRoleAsync(MBSContext context, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Id = "67c005ba-ad72-4c3e-8a96-deb253d8bb12",
                        ConcurrencyStamp = UserRoleEnum.Admin.ToString(),
                        Name = UserRoleEnum.Admin.ToString(),
                        NormalizedName = UserRoleEnum.Admin.ToString(),
                    },
                    new IdentityRole
                    {
                        Id = "06b905a4-d0e8-4b19-b0e0-09208504bdbb",
                        ConcurrencyStamp = UserRoleEnum.Student.ToString(),
                        Name = UserRoleEnum.Student.ToString(),
                        NormalizedName = UserRoleEnum.Student.ToString(),
                    },
                    new IdentityRole
                    {
                        Id = "8d2e6f28-0a37-42d7-8452-a560fe694ac7",
                        ConcurrencyStamp = UserRoleEnum.Mentor.ToString(),
                        Name = UserRoleEnum.Mentor.ToString(),
                        NormalizedName = UserRoleEnum.Mentor.ToString(),
                    },
                };

                for (int i = 0; i < roles.Count; i++)
                {
                    await roleManager.CreateAsync(roles[i]);
                }
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedPositionAsync(ModelBuilder modelBuilder)
        {
            List<Position> defaultPosition =
            [
                new Position
                {
                    Id = Guid.Parse("f6964510-6671-43ed-b0dc-bdb413c56fd5"),
                    Name = PositionNameEnum.Leader.ToString(),
                    Description = PositionNameEnum.Leader.ToString(),
                    Status = StatusEnum.Activated
                },
                new Position
                {
                    Id = Guid.Parse("d90a1dba-cc6c-466c-96e5-8eaf98809d8d"),
                    Name = PositionNameEnum.Member.ToString(),
                    Description = PositionNameEnum.Member.ToString(),
                    Status = StatusEnum.Activated
                },
                new Position
                {
                    Id = Guid.Parse("ce90cd84-42f4-461e-8c92-1a1854dc52ac"),
                    Name = PositionNameEnum.Read.ToString(),
                    Description = PositionNameEnum.Read.ToString(),
                    Status = StatusEnum.Activated
                }
            ];
            modelBuilder.Entity<Position>().HasData(defaultPosition);
        }

        //public static async Task SeedMajorAsync(ModelBuilder modelBuilder)
        //{
        //    List<Major> defaultMajor =
        //        [
        //        new Major
        //        {
        //            Id = Guid.Parse("903b6085-4cc3-47f3-bbdd-0f8319e5aabb"),
        //            Name = "SE",
        //            Status = StatusEnum.Activated
        //        },
        //        new Major
        //        {
        //            Id = Guid.Parse("71577eaf-ebf1-4b23-a48d-cf8561b1c7db"),
        //            Name = "SS",
        //            Status = StatusEnum.Activated
        //        },
        //        new Major
        //        {
        //            Id = Guid.Parse("dfdb83a4-18e0-447e-9ec8-7c8b39ee6f3a"),
        //            Name = "SA",
        //            Status = StatusEnum.Activated
        //        }
        //        ];
        //     modelBuilder.Entity<Major>().HasData(defaultMajor);
        //}
    }
}