namespace Web_Blog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yorum")]
    public partial class Yorum
    {
        [Key]
        public int Yorum_Id { get; set; }

        [StringLength(500)]
        public string Icerik { get; set; }

        public int? Uye_Id { get; set; }

        public int? Makale_Id { get; set; }

        public DateTime? Tarih { get; set; }

        public virtual Makale Makale { get; set; }

        public virtual Uye Uye { get; set; }
    }
}
