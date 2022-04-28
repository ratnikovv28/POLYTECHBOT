using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPbPUBOT
{
    public class UserAssistance
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserID { get; set; }

        public string Username { get; set; }

        public long? operatorID { get; set; }

        public int messageMenuID { get; set; }
    }
}
