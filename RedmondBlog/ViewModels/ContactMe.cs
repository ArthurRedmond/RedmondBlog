using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedmondBlog.ViewModels
{
    public class ContactMe
    {
        [Required]
        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters and no more than {1}", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Required]
        [StringLength(80, ErrorMessage = "The {0} must be at least {2} characters and no more than {1}", MinimumLength = 2)]
        public string Subject { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters and no more than {1}", MinimumLength = 2)]
        public string Message { get; set; }
    }
}
