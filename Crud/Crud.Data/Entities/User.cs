using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Data.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string UserSystem { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength]
        public byte[] PasswordHash { get; set; }
        [Required]
        [MaxLength]
        public byte[] PasswordSalt { get; set; }
        public bool Status { get; set; }
        public bool Gender { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
