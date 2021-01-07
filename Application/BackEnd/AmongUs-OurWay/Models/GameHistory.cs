using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmongUs_OurWay.Models
{
    [Table("GameHistory")]
    public class GameHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType("int")]
        [Column("id")]
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
    }
}