using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess;
using MBS.DataAccess.Persistents.Configurations.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;

public class SeedGroups
{
    private readonly MBSContext _mBSContext;
    private readonly DbInitializer _dbInitializer;

    public SeedGroups(MBSContext mBSContext, DbInitializer dbInitializer)
    {
        _mBSContext = mBSContext;
        _dbInitializer = dbInitializer;
    }

    public void SeedingGroups()
    {
        var groupData = new List<(Guid id, Guid projectId, string studentId, Guid positionId)>
        {
            // Group for "AI Research"
            (Guid.Parse("ad5a5f1a-c292-44e9-bc7f-9b1c5cb40f32"), Guid.Parse("d1f47f88-c7e2-41cb-bb8d-e1acb1e342af"), "aa2a71b6-4f0b-453e-bd09-1b1d8b2ab978", Guid.Parse("F6964510-6671-43ED-B0DC-BDB413C56FD5")),  // Leader - Pham Hoang Minh Khoi
            (Guid.Parse("bba90b55-8f55-4df2-9267-5b7fa25d6f9c"), Guid.Parse("d1f47f88-c7e2-41cb-bb8d-e1acb1e342af"), "b47d87c9-f755-4989-9092-6481f517b435", Guid.Parse("D90A1DBA-CC6C-466C-96E5-8EAF98809D8D")),  // Member - Nguyen Thanh Tu
            (Guid.Parse("cc68b091-d67c-4b4b-bbb5-961d9b0e02d9"), Guid.Parse("d1f47f88-c7e2-41cb-bb8d-e1acb1e342af"), "bf1a21d1-c349-4d80-9382-1f215d52923f", Guid.Parse("D90A1DBA-CC6C-466C-96E5-8EAF98809D8D")),  // Member - Le Minh Duc
            (Guid.Parse("da4f678c-3f5b-4dfe-82d5-e6f9d5fb3f78"), Guid.Parse("d1f47f88-c7e2-41cb-bb8d-e1acb1e342af"), "df8d2a56-5b3e-4d5a-8cba-5d5b7a2d9e31", Guid.Parse("D90A1DBA-CC6C-466C-96E5-8EAF98809D8D")),  // Member - Tran Van Ha
        };

        var groups = new List<Group>();

        foreach (var (id, projectId, studentId, positionId) in groupData)
        {
            groups.Add(new Group
            {
                Id = id,
                ProjectId = projectId,
                StudentId = studentId,
                PositionId = positionId,
            });
        }

        if (groups.Any())
        {
            _dbInitializer.Initialize(groups);
        }
    }
}
