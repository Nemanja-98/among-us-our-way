using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace AmongUs_OurWay.Models
{
    [Table("Game")]
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType("int")]
        [Column("Id")]
        public int Id { get; set; }
        
        [DataType(DataType.DateTime)]
        [Column("DateStarted")]
        public System.DateTime DateStarted { get; set; }
        
        [NotMapped]
        public virtual ICollection<PlayerAction> Actions { get; set; }
        
        [NotMapped]
        public virtual ICollection<GameHistory> Players { get; set; }
    }
}