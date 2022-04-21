using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPbPUBOT
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long UserID { get; set; }
        
        public string FirstName { get; set; }
        
        public string SecondName { get; set; }

        public string UserName { get; set; }

        public long? operatorID { get; set; }
    }
}
