//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jobify.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class JobApply
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> ApplyAt { get; set; }
        public Nullable<int> ResumeId { get; set; }
        public Nullable<int> JobId { get; set; }
        public string Why_Should_we_hire_you_ { get; set; }
        public string Whats_your_dream_job_ { get; set; }
        public string JobStatus { get; set; }
    
        public virtual Job Job { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
