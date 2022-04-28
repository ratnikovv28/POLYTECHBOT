using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPbPUBOT
{
    public class Operator
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long OperatorID { get; set; }

        public string Username { get; set; }

        public bool isMain { get; set; }

        public long? UserID { get; set; }
    }
}
