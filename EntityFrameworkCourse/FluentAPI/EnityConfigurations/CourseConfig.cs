﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAPI.EnityConfigurations
{
    public class CourseConfig : EntityTypeConfiguration<Course>
    {
        public CourseConfig()
        {
            //tableoverrides
            //ToTable("tbl_Course");
            
            //keys
            HasKey(c => c.Id);

            //property config
            Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(2000);

            Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255);

            //relationships
            HasRequired(c => c.Author)
            .WithMany(a => a.Courses)
            .HasForeignKey(c => c.AuthorId)
            .WillCascadeOnDelete(false);

            HasRequired(c => c.Cover)
            .WithRequiredPrincipal(c => c.Course);

            HasMany(c => c.Tags)
            .WithMany(t => t.Courses)
            .Map(m =>
            {
                m.ToTable("CourseTags");
                m.MapLeftKey("CourseId");
                m.MapRightKey("TagId");
            });
        }
    }
}
