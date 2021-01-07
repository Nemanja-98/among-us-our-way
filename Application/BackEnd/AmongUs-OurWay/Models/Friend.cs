using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmongUs_OurWay.Models
{
    [Table("Friend")]
    public class Friend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType("int")]
        [Column("Id")]
        public int Id { get; set; }
        
        [DataType(DataType.Text)]
        [Column("User1Ref")]
        public string User1Ref { get; set; }
        
        [NotMapped]
        public virtual User User1 { get; set; }
        
        [DataType(DataType.Text)]
        [Column("User2Ref")]
        public string User2Ref { get; set; }
    }
}