using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDo.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class Student
    {
        [Required]
        public string Sname { get; set; }

        public int SID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Roll must be greater than 0")]
        public int Roll { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Address cannot exceed 50 characters.")]
        public string Address { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public List<Student> StudentList { get; set; }
    }
}
