using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite_DKTKViecLam.Models;
using System.Net.Mail;

namespace WebSite_DKTKViecLam.Controllers
{
    public class HomeController : Controller
    {
        QL_TKVLDataContext db = new QL_TKVLDataContext();
        // GET: Home
        public ActionResult Index(/*int page = 1*/)     
        {
            List<BAIDANG> a = db.BAIDANGs.ToList();
            return View(a);
        }

        public ActionResult GioiThieu()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }

        public ActionResult ViecLamTopDau()
        {
            var result = (from bd in db.BAIDANGs
                          join lcv in db.LOAICONGVIECs on bd.Maloai equals lcv.Maloai
                          where bd.Trangthai_BD == "Đã duyệt"
                          group lcv by lcv.Tenloai into g
                          orderby g.Count() descending
                          select new BaiDangCount
                          {
                              SoBaiDang = g.Count(),
                              TenLoai = g.Key
                          }).Take(10).ToList();

            var viewModel = new ViecLamTopDauViewModel
            {
                BaiDangCounts = result
            };
            return View(viewModel);
        }

        public ActionResult DanhSachCongViecVaDoanhNghiep()
        {
            return View();
        }

        public ActionResult DS_CongViec()
        {
            List<LOAICONGVIEC> lcv = db.LOAICONGVIECs.ToList();
            return View(lcv); 
        }


        public ActionResult DS_DoanhNghiep()
        {
            List<DOANHNGHIEP> dn = db.DOANHNGHIEPs.ToList();
            return View(dn); 
        }

        public ActionResult GiaoDienDangKyTongHop()
        {
            return View();
        }

        public ActionResult GiaoDienDangNhapTongHop()
        {
            return View();
        }
        //Xem chi tiet bai dang
        public ActionResult XemChiTietBaiDang(string id)
        {
            BAIDANG s = db.BAIDANGs.FirstOrDefault(i => i.MaBD == id);
            Session["mabaidang"] = s.MaBD;
            Session["tenbaidang"] = s.TenBD;
            return View(s);
        }

        //======================== Tìm kiếm =========================
        public ActionResult TimKiemNC()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XL_TimKiemNC(FormCollection fc)
        {

            string tinhthanh = fc["tinhthanh"].ToLower();
            string congviec = fc["congviec"].ToLower();
            string congty = fc["congty"].ToLower();
            List<BAIDANG> lst = db.BAIDANGs.ToList();
            if (!string.IsNullOrEmpty(tinhthanh))
            {
                lst = lst.Where(l => l.DOANHNGHIEP.Tinhthanh.ToLower().Contains(tinhthanh)).ToList();
            }

            if (!string.IsNullOrEmpty(congviec))
            {
                lst = lst.Where(l => l.Maloai.ToLower().Contains(congviec)).ToList();
            }
            if (!string.IsNullOrEmpty(congty))
            {
                lst = lst.Where(l => l.DOANHNGHIEP.TenDN.ToLower().Contains(congty)).ToList();
            }
            return View("Index", lst);

        }

        //======================== Ứng tuyển =========================
        public ActionResult UngTuyen_Home()
        {
            return View();
        }

    }
}