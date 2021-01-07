using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmongUs_OurWay.Models
{   
    [Table("PlayerAction")]
    public class PlayerAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType("int")]
        [Column("Id")]
        public int Id { get; set; }
        
        [DataType(DataType.Text)]
        [Column("UserId")]
        public string UserId { get; set; }

        [NotMapped]
        public virtual User User { get; set; }
        
        [DataType("int")]
        [Column("GameId")]
        public int GameId { get; set; }

        [NotMapped]
        public virtual Game Game { get; set; }

        [DataType("int")]
        [Column("Action")]
        public action Action { get; set; }
        
        [DataType(DataType.Time)]
        [Column("Time")]
        public System.TimeSpan Time { get; set; }
    }
}