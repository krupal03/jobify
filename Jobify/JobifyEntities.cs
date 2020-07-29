using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jobify.Models
{ public enum genders { Male,Female,Other}
    public class Resume
    {
        public int Id { get; set; }
        public string Name {get;set;}
        public DateTime Dob { get; set; }
        public int age { get; set; }
        public genders Gender { get; set; }
        public int Experience { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string Qualification { get; set; }
        public string City { get; set; }
        public ICollection<AspNetUser> User { get; set; }
        public ResumeFile Resumefile { get; set; }
    }
    public class ResumeFile
    {
        public int Id { get; set; }
        public DateTime UploadedAt { get; set; }
        public Resume Resume { get; set; }
        public string Path { get; set; }

    }
    public class Job
    {  
        public int Id { get; set; }
        public string JobType { get; set; }
        public string Joblocation { get; set; }
        public string Jobtitle { get; set; }
        public int Salary { get; set; }
        public int Experience { get; set; }
        public int Jobseeker { get; set; }
        public ICollection<Recruiter> Recruiter { get; set; }
    }
    public class Recruiter
    {   public int Id { get; set;}
        public AspNetUser RecruiterDetail { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
    public class JobApply
    {
        public int Id { get; set; }
        public DateTime AppltyAt { get; set; }
        public ICollection<Job> Job { get; set; }
        public ICollection<Resume> Resume { get; set; }
    }
}