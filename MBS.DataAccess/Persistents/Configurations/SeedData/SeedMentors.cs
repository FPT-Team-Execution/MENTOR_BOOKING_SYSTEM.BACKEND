using MBS.Core.Entities;
using MBS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

public class SeedMentors
{
    private readonly MBSContext _mBSContext;

    public SeedMentors(MBSContext mBSContext)
    {
        _mBSContext = mBSContext;
    }

    public void SeedingMentors()
    {
        var mentorData = new List<(string userId, string industry, int consumePoint)>
        {
            (Guid.Parse("a1d2c1f5-45f7-4db2-b67b-69edab349e2f").ToString(), "FPT Software", 0), // hoangnguyen
            (Guid.Parse("c7bcd8a8-f33f-4ec8-98f7-1293b215d511").ToString(), "FPT Software", 0), // danghuong
            (Guid.Parse("b4a11fdd-fd15-4c7d-931f-4e35b733da12").ToString(), "FPT Software", 0)  // tranthilinh
        };

        var mentors = new List<Mentor>();

        foreach (var (userId, industry, consumePoint) in mentorData)
        {
            if (!_mBSContext.Mentors.Any(m => m.UserId == userId))
            {
                mentors.Add(new Mentor
                {
                    UserId = userId,
                    Industry = industry,
                    ConsumePoint = consumePoint
                });
            }
        }

        if (mentors.Any())
        {
            Initialize(mentors);
        }
    }

    public void Initialize<T>(List<T> entities) where T : class
    {
        try
        {
            foreach (var entity in entities)
            {
                if (!_mBSContext.Set<T>().Any(e => e.Equals(entity)))
                {
                    _mBSContext.Set<T>().Add(entity);
                }
            }
            _mBSContext.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while initializing {typeof(T).Name}: {ex.Message}");
        }
    }
}
