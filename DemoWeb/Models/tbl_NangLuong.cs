namespace DemoWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_NangLuong
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TEN_TIEUDE { get; set; }

        [StringLength(50)]
        public string CS_HIENTAI { get; set; }

        [StringLength(50)]
        public string CS_MAX { get; set; }

        [StringLength(50)]
        public string CS_THIETKE { get; set; }

        [StringLength(50)]
        public string SANLUONG_NGAY { get; set; }

        public DateTime? TGian { get; set; }
    }
}
