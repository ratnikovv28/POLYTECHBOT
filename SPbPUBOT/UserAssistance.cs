using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPbPUBOT
{
    public class UserAssistance
    {
        [Key]
        public int BidID { get; set; }

        public long UserID { get; set; }

        public string Username { get; set; }

        public long? OperatorID { get; set; }
    }
}
