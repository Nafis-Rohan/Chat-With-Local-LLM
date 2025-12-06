using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Tables
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string UserName { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Password { get; set; }

        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        public virtual ICollection<ChatMessage> ChatMessages { get; set; }

        public User()
        {
            ChatMessages = new List<ChatMessage>();
        }
    }
}
