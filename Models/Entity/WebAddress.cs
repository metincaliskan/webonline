using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebControlApp.Models.Entity
{
    public class WebAddress
    {
        public int WebAddressId { get; set; }
        [Required(ErrorMessage = "Name alanı boşgeçilemez")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Url alanı boşgeçilemez")]
        [RegularExpression(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$", ErrorMessage = "Geçerli bir web adresi giriniz!")]
        public string Url { get; set; }
        [Required(ErrorMessage = "Interval alanı boşgeçilemez")]
        public int Interval { get; set; }
        [DefaultValue(false)]
        public bool IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? IdUser { get; set; }
        [DefaultValue(true)]
        public bool IsOnline { get; set; }
    }
}
