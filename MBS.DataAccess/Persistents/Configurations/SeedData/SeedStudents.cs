using MBS.Core.Entities;
using MBS.DataAccess;
using MBS.DataAccess.Persistents.Configurations.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;

public class SeedStudents
{
    private readonly MBSContext _mBSContext;
    private readonly DbInitializer _dbInitializer;

    public SeedStudents(MBSContext mBSContext, DbInitializer dbInitializer)
    {
        _mBSContext = mBSContext;
        _dbInitializer = dbInitializer;
    }

    public void SeedingStudents()
    {
        var seMajorId = Guid.Parse("903b6085-4cc3-47f3-bbdd-0f8319e5aabb"); // SE
        var ssMajorId = Guid.Parse("71577eaf-ebf1-4b23-a48d-cf8561b1c7db"); // SS
        var saMajorId = Guid.Parse("dfdb83a4-18e0-447e-9ec8-7c8b39ee6f3a"); // SA

        var studentData = new List<(Guid userId, Guid majorId)>
        {
            (Guid.Parse("aa2a71b6-4f0b-453e-bd09-1b1d8b2ab978"), seMajorId), // Pham Hoang Minh Khoi
            (Guid.Parse("b47d87c9-f755-4989-9092-6481f517b435"), ssMajorId), // Nguyen Thanh Tu
            (Guid.Parse("bf1a21d1-c349-4d80-9382-1f215d52923f"), saMajorId), // Le Minh Duc
            (Guid.Parse("df8d2a56-5b3e-4d5a-8cba-5d5b7a2d9e31"), seMajorId)  // Tran Van Ha
        };

        var students = new List<Student>();

        foreach (var (userId, majorId) in studentData)
        {
            if (!_mBSContext.Students.Any(s => s.UserId == userId.ToString()))
            {
                students.Add(new Student
                {
                    UserId = userId.ToString(),
                    University = "FPT University",
                    WalletPoint = 0,
                    MajorId = majorId
                });
            }
        }
        if (students.Any())
        {
            _dbInitializer.Initialize(students);
        }
    }

    
}
