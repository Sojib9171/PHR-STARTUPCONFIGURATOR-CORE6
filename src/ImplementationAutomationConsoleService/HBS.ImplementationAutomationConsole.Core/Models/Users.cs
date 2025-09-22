using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace HBS.ImplementationAutomationConsole.Core.Models
{
    [Table("HS_HR_IA_USERS")]
    public class Users
    {
        [Key]
        [Required]
        public string? USER_ID { get; set; }
        public string? PASSWORD { get; set; }
        public string? NAME { get; set; }
        public string? USERNAME { get; set; }
        public bool USER_ISACTIVE { get; set; }
        public string? USER_TYPE_CODE { get; set; }

        public Users ConvertToSingleUserModel(DataTable dt)
        {
            List<Users> users = new List<Users>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Users user = new Users();
                user.USER_ID = Convert.ToString(dt.Rows[i]["USER_ID"]);
                user.PASSWORD = Convert.ToString(dt.Rows[i]["PASSWORD"]);
                user.NAME = Convert.ToString(dt.Rows[i]["NAME"]);
                user.USERNAME = Convert.ToString(dt.Rows[i]["USERNAME"]);
                user.USER_ISACTIVE = Convert.ToBoolean(dt.Rows[i]["USER_ISACTIVE"]);
                user.USER_TYPE_CODE = Convert.ToString(dt.Rows[i]["USER_TYPE_CODE"]);

                users.Add(user);
            }
            return users.FirstOrDefault();
        }
    }
}