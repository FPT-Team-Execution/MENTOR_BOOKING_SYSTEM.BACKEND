using MBS.Core.Entities;
using MBS.DataAccess;
using MBS.DataAccess.Persistents.Configurations.SeedData;
using System;
using System.Collections.Generic;

public class SeedSkills
{
    private readonly MBSContext _mBSContext;
    private readonly DbInitializer _dbInitializer;

    public SeedSkills(MBSContext mBSContext, DbInitializer dbInitializer)
    {
        _mBSContext = mBSContext;
        _dbInitializer = dbInitializer;
    }

    public void SeedingSkills()
    {
        var skillData = new List<(Guid id, string name, string mentorId)>
        {
            (Guid.NewGuid(), "Artificial Intelligence", "a1d2c1f5-45f7-4db2-b67b-69edab349e2f"), // Mentor: hoangnguyen
            (Guid.NewGuid(), "Blockchain Technology", "c7bcd8a8-f33f-4ec8-98f7-1293b215d511"), // Mentor: danghuong
            (Guid.NewGuid(), "Cloud Solutions", "b4a11fdd-fd15-4c7d-931f-4e35b733da12")  // Mentor: tranthilinh
        };

        var skills = new List<Skill>();

        foreach (var (id, name, mentorId) in skillData)
        {
            skills.Add(new Skill
            {
                Id = id,
                Name = name,
                MentorId = mentorId
            });
        }

        _dbInitializer.Initialize(skills);
    }
}
