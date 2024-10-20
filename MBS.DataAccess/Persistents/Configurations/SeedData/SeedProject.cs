using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess;
using MBS.DataAccess.Persistents.Configurations.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;

public class SeedProjects
{
    private readonly MBSContext _mBSContext;
    private readonly DbInitializer _dbInitializer;

    public SeedProjects(MBSContext mBSContext, DbInitializer dbInitializer)
    {
        _mBSContext = mBSContext;
        _dbInitializer = dbInitializer;
    }

    public void SeedingProjects()
    {
        var projectData = new List<(Guid id, string title, string description, DateTime dueDate, string semester, string mentorId, ProjectStatusEnum status)>
        {
            (Guid.Parse("d1f47f88-c7e2-41cb-bb8d-e1acb1e342af"), "AI Research", "Research in Artificial Intelligence and Machine Learning.", new DateTime(2024, 12, 15), "FA24", "a1d2c1f5-45f7-4db2-b67b-69edab349e2f", ProjectStatusEnum.Activated), // Mentor: hoangnguyen
            (Guid.Parse("e2b9b3e1-45b7-4ad8-b763-541cb2134c0d"), "Blockchain Development", "Developing a decentralized blockchain application.", new DateTime(2023, 7, 30), "SU22", "c7bcd8a8-f33f-4ec8-98f7-1293b215d511", ProjectStatusEnum.Activated),   // Mentor: danghuong
            (Guid.Parse("c4f0e9b9-46f8-4d97-bbb5-647bbf6e427d"), "Cloud Computing", "Building scalable cloud infrastructure.", new DateTime(2023, 5, 15), "SP23", "b4a11fdd-fd15-4c7d-931f-4e35b733da12", ProjectStatusEnum.Activated)  // Mentor: tranthilinh
        };

        var projects = new List<Project>();

        foreach (var (id, title, description, dueDate, semester, mentorId, status) in projectData)
        {
            projects.Add(new Project
            {
                Id = id,
                Title = title,
                Description = description,
                DueDate = dueDate,
                Semester = semester,
                MentorId = mentorId,
                Status = status,
                CreatedOn = DateTime.Now,
                CreatedBy = "system" // Default value for CreatedBy
            });
        }

            _dbInitializer.Initialize(projects);
    }
}