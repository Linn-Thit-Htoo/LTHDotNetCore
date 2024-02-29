using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LTHDotNetCore.RealtimeChartUsingSignalR.Models
{
    [Table("Tbl_Team")]
    public class TeamDataModel
    {
        [Key]
        public int Id { get; set; }

        public string TeamName { get; set; }

        public int Score { get; set; }
    }
}
