namespace DemoWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_TaiKhoan
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string TEN_TAIKHOAN { get; set; }

        [StringLength(500)]
        public string MAT_KHAU { get; set; }

        [StringLength(50)]
        public string TEN_DANGNHAP { get; set; }

        public DateTime? NGAY_HIEULUC { get; set; }

        public int? TRANG_THAI { get; set; }
    }
}
