﻿using System;
using System.Collections.Generic;
using FundRaiser.Team5.Core.Entities;

namespace FundRaiser.Team5.Core.Options
{
    public class OptionProject
    {
        public int ProjectId { get; set; }

        public string Title { get; set; }

        public Category Category { get; set; }

        public string Description { get; set; }

        public List<FundingPackage> FundingPackages { get; set; }

        public List<ImagePath> Images { get; set; }

        public List<VideoPath> Videos { get; set; }

        public List<StatusUpdate> StatusUpdates { get; set; }

        public decimal FundingGoal { get; set; } //>0 

        public decimal CurrentFund { get; set; } = 0;

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime Deadline { get; set; } //>DateTime.Now

        public User Users { get; set; }

        public Project GetProject()
        {
            Project project = new()
            {
                ProjectId = ProjectId,
                Title = Title,
                Category = Category,
                Description = Description,
                FundingPackages = FundingPackages,
                Images = Images,
                Videos = Videos,
                StatusUpdates = StatusUpdates,
                FundingGoal = FundingGoal,
                CurrentFund = CurrentFund,
                DateCreated = DateCreated,
                Deadline = Deadline,
                Users = Users
            };
            return project;
        }
        public OptionProject() { }

        public OptionProject(Project project)
        {
            if (project != null)
            {
                ProjectId = project.ProjectId;
                Title = project.Title;
                Category = project.Category;
                Description = project.Description;
                FundingPackages = project.FundingPackages;
                Images = project.Images;
                Videos = project.Videos;
                StatusUpdates = project.StatusUpdates;
                FundingGoal = project.FundingGoal;
                CurrentFund = project.CurrentFund;
                DateCreated = project.DateCreated;
                Deadline = project.Deadline;
                Users = project.Users;
            }
        }
    }
}
