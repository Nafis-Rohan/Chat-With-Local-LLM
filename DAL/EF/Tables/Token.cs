using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF.Tables
{
    public class Token
    {
        [Key]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string TKey { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ExpireAt { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "VARCHAR")]
        public string UserName { get; set; }
    }
}
