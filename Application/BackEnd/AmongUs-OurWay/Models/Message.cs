using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmongUs_OurWay.Models
{
    [Table("Message")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType("int")]
        [Column("Id")]
        public int Id { get; set; }
        
        [DataType(DataType.Text)]
        [Column("UserSent")]
        public string UserSent { get; set; }
        
        [DataType(DataType.Text)]
        [Column("UserReceived")]
        public string UserReceived { get; set; }
        
        [DataType(DataType.Text)]
        [Column("Content")]
        public string Content { get; set; }
        
        [DataType(DataType.Text)]
        [Column("SentTime")]
        public string SentTime { get; set; }
   }
}