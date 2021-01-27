using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmongUs_OurWay.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DataType(DataType.Text)]
        [Column("Username")]
        public string Username { get; set; }
        
        [DataType(DataType.Password)]
        [Column("Password")]
        public string Password { get; set; }
        
        [DataType("int")]
        [Column("GamesPlayed")]
        public int GamesPlayed { get; set; }
        
        [DataType("int")]
        [Column("CrewmateGames")]
        public int CrewmateGames { get; set; }
        
        [DataType("int")]
        [Column("ImpostorGames")]
        public int ImpostorGames { get; set; }
        
        [DataType("int")]
        [Column("CrewmateWonGames")]
        public int CrewmateWonGames { get; set; }
        
        [DataType("int")]
        [Column("ImpostorWonGames")]
        public int ImpostorWonGames { get; set; }

        [DataType("int")]
        [Column("TasksCompleted")]
        public int TasksCompleted { get; set; }
        
        [DataType("int")]
        [Column("AllTaskCompleted")]
        public int AllTasksCompleted { get; set; }
        
        [DataType("int")]
        [Column("Kills")]
        public int Kills { get; set; }

        [NotMapped]
        public virtual ICollection<GameHistory> Games { get; set; }

        [NotMapped]
        public virtual ICollection<Friend> Friends { get; set; }
        
        [NotMapped]
        public virtual ICollection<PendingRequest> SentRequests { get; set; }
        
        [NotMapped]
        public virtual ICollection<PendingRequest> PendingRequests { get; set; }
    }
}