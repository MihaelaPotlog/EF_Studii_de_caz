using System.ComponentModel.DataAnnotations.Schema;

namespace EF_StudiiDeCaz.Caz4
{
    [Table("eCommerce", Schema = "BazaDeDate")]
    public class eCommerce : Business
    {
        public string URL { get; set; }
    }
}
