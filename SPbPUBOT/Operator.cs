using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPbPUBOT
{
    public class Operator
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long OperatorID { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string UserName { get; set; }

        public bool isMain { get; set; }

        public long? userID { get; set; }
    }
}
