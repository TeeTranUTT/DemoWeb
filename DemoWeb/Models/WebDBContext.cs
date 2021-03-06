using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DemoWeb.Models
{
    public partial class WebDBContext : DbContext
    {
        public WebDBContext()
            : base("name=WebDBContext1")
        {
        }

        public virtual DbSet<tbl_NangLuong> tbl_NangLuong { get; set; }
        public virtual DbSet<tbl_TaiKhoan> tbl_TaiKhoan { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
