using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmongUs_OurWay.Models
{
    public class PendingRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType("int")]
        [Column("Id")]
        public int Id { get; set; }
        
        [DataType(DataType.Text)]
        [Column("UserSentRef")]
        public string UserSentRef { get; set; }
        
        [NotMapped]
        public virtual User UserSent { get; set; }
        
        [DataType(DataType.Text)]
        [Column("UserReceivedRef")]
        public string UserReceivedRef { get; set; }
        
        [NotMapped]
        public virtual User UserReceived { get; set; }
    }
}