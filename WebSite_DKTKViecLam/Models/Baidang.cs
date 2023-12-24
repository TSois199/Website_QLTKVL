using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite_DKTKViecLam.Models
{
    public class Baidang
    {
        QL_TKVLDataContext db = new QL_TKVLDataContext();
        public string MaB { get; set; }
        public string TenB { get; set; }
        public string MaD { get; set; }
        public string MaL { get; set; }
        public string MotaB { get; set; }
        public string PhucloiB { get; set; }
        public string VitriB { get; set; }
        public string KinhnghiemB { get; set; }
        public string DotuoilamviecB { get; set; }
        public string MucluongB { get; set; }
        public DateTime NgaydangbaiB { get; set; }
        public string YeucauB { get; set; }
        public string TrangthaiB { get; set; }

        public Baidang(string ms)
        {
            MaB = ms;
            BAIDANG b = db.BAIDANGs.FirstOrDefault(i => i.MaBD == ms);
            TenB = b.TenBD;
            //AnhB = b.Anh_BD;
            MucluongB = b.Mucluong;
            //Anh = b.HinhAnh;
            //DonGia = Convert.ToDouble(s.DonGia.ToString());
            //SoLuongMua = 1;
        }
    }
    
}