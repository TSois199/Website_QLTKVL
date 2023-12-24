using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite_DKTKViecLam.Models;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
namespace WebSite_DKTKViecLam.Controllers
{
    public class DoanhNghiepController : Controller
    {

        QL_TKVLDataContext db = new QL_TKVLDataContext();


        // GET: DoanhNghiep
        public ActionResult IndexDN()
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

        //======================== Load Index =========================
        public ActionResult LoadDN()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XL_LoadDN(FormCollection fc)
        {

            string madn = fc["ma"].ToLower();
            List<BAIDANG> lst = db.BAIDANGs.ToList();
            if (!string.IsNullOrEmpty(madn))
            {
                lst = lst.Where(l => l.DOANHNGHIEP.MaDN.ToLower().Contains(madn)).ToList();
            }
            return View("IndexDN", lst);
        }

        //Xem chi tiết bài đã dăng của doanh nghiệp
        public ActionResult XemChiTietBaiDang_DN_DaDang(string id)
        {
            BAIDANG bddd = db.BAIDANGs.FirstOrDefault(i => i.MaBD == id);
            return View(bddd);
        }

        //Thêm Bài Đăng
        //public ActionResult Them_BaiDang()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Them_BaiDang(FormCollection col)
        //{
        //    string latestID = db.BAIDANGs.OrderByDescending(k => k.MaBD).Select(k => k.MaBD).FirstOrDefault();
        //    string newID = "BD001"; // Giá trị mặc định
        //    if (!string.IsNullOrEmpty(latestID))
        //    {
        //        int number = int.Parse(latestID.Substring(1));
        //        number++;
        //        newID = "BD" + number.ToString("D3");
        //    }
        //    var tbd = col["tenbd"];
        //    var abd = col["anhbd"];
        //    var mdn = col["madn"];
        //    var ml = col["mloai"];
        //    var mt = col["mta"];
        //    var pl = col["ploi"];
        //    var vt = col["vtri"];
        //    var kn = col["knghiem"];
        //    var dtlv = col["dtlviec"];
        //    var mlu = col["mluong"];
        //    var ndb = col["ndbai"];
        //    var yc = col["ycau"];
        //    BAIDANG a = new BAIDANG();
        //    a.MaBD = newID;
        //    a.TenBD = col["tenbd"];
        //    a.Anh_BD = col["anhbd"];
        //    a.MaDN = col["madn"];
        //    a.Maloai = col["mloai"];
        //    a.Mota = col["mta"];
        //    a.Phucloi = col["ploi"];
        //    a.Vitri = col["vtri"];
        //    a.Kinhnghiem = col["knghiem"];
        //    a.Dotuoilamviec = col["dtlviec"];
        //    a.Mucluong = col["mluong"];
        //    //a.Ngaydangbai = col["ndbai"];
        //    a.Yeucau = col["ycau"];
        //    db.BAIDANGs.InsertOnSubmit(a);
        //    db.SubmitChanges();
        //    return RedirectToAction("DoanhNghiep", "IndexDN");
        //}

        //======Thêm Bài đăng============
        public ActionResult Them_BaiDang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Them_BaiDang(FormCollection col)
        {
            string latestID = db.BAIDANGs.OrderByDescending(k => k.MaBD).Select(k => k.MaBD).FirstOrDefault();
            string newID = "BD001"; // Giá trị mặc định
            string dateString = col["ndbai"];
            DateTime date;
            if (!string.IsNullOrEmpty(latestID))
            {
                int number = int.Parse(latestID.Substring(3));
                number++;
                newID = "BD" + number.ToString("D3");
            }
            var tt = "Chưa Duyệt";
            var tbd = col["tenbd"];
            var mdn = col["madn"];
            var ml = col["mloai"];
            var mt = col["mta"];
            var pl = col["ploi"];
            var vt = col["vtri"];
            var kn = col["knghiem"];
            var dtlv = col["dtlviec"];
            var mlu = col["mluong"];
            var ndb = col["ndbai"];
            var yc = col["ycau"];
            if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                BAIDANG a = new BAIDANG();
                a.MaBD = newID;
                a.TenBD = col["tenbd"];
                a.MaDN = col["madn"];
                a.Maloai = col["mloai"];
                a.Mota = col["mta"];
                a.Phucloi = col["ploi"];
                a.Vitri = col["vtri"];
                a.Kinhnghiem = col["knghiem"];
                a.Dotuoilamviec = col["dtlviec"];
                a.Mucluong = col["mluong"];
                a.Ngaydangbai = date;
                a.Yeucau = col["ycau"];
                a.Trangthai_BD = tt;
                db.BAIDANGs.InsertOnSubmit(a);
                db.SubmitChanges();
                return RedirectToAction("IndexDN", "DoanhNghiep");
            }
            else
            {
                ModelState.AddModelError("Error", "Ngày đăng bài không hợp lệ");
                return View();
            }

        }

        // Xóa bài đăng của DN
        public ActionResult Xoa_BaiDang_DN(String id)
        {
            BAIDANG bd = db.BAIDANGs.Where(row => row.MaBD == id).FirstOrDefault();
            return View(bd);
        }
        [HttpPost]
        public ActionResult Xoa_BaiDang_DN(BAIDANG bdang)
        {
            BAIDANG bd = db.BAIDANGs.Where(row => row.MaBD == bdang.MaBD).FirstOrDefault();
            db.BAIDANGs.DeleteOnSubmit(bd);
            db.SubmitChanges();
            return RedirectToAction("IndexDN");
        }

        //======Sửa bài đăng của doanh nghiệp============

        public ActionResult Sua_BaiDang(String id)
        {
            BAIDANG bd = db.BAIDANGs.Where(row => row.MaBD == id).FirstOrDefault();
            return View(bd);
        }
        [HttpPost]
        public ActionResult Sua_BaiDang(BAIDANG bdang)
        {
            BAIDANG bd = db.BAIDANGs.FirstOrDefault(row => row.MaBD == bdang.MaBD);
            if (bd == null)
            {
                ModelState.AddModelError("Error", "Không có dữ liệu");
                return View();
            }
            else
            {
                bd.TenBD = bdang.TenBD;
                bd.Mota = bdang.Mota;
                bd.Phucloi = bdang.Phucloi;
                bd.Vitri = bdang.Vitri;
                bd.Kinhnghiem = bdang.Kinhnghiem;
                bd.Dotuoilamviec = bdang.Dotuoilamviec;
                bd.Mucluong = bdang.Mucluong;
                bd.Ngaydangbai = bdang.Ngaydangbai;
                bd.Yeucau = bdang.Yeucau;
                db.SubmitChanges();
                //return View();
                return RedirectToAction("IndexDN");
            }
        }


        //=========================Register/Login/Logout======================
        [HttpGet]
        public ActionResult DangNhapDN()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapDN(FormCollection col)
        {
            DOANHNGHIEP dn = db.DOANHNGHIEPs.FirstOrDefault(a => a.Email == col["email"] && a.Password_DN == col["pswd"]);

            if (dn != null)
            {
                if (dn.Trangthai_DN == "Chưa Duyệt")
                {
                    ModelState.AddModelError("Messages", "Vui lòng chờ duyệt trong vòng  5 ngày");
                    return View();
                }
                else
                {
                    Session["dn"] = dn;
                    Session["tenDN"] = dn.TenDN;
                    Session["maDN"] = dn.MaDN;
                    Session["emailDN"] = dn.Email;
                    return RedirectToAction("IndexDN", "DoanhNghiep");
                }
            }
            else
            {
                ModelState.AddModelError("myError", "Invalue email and password.");
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["dn"] = null;
            Session["tenDN"] = null;
            return RedirectToAction("/Home/Index");
        }


        //Dang Ky Tuc la them doanh nghiep moi

        public ActionResult DangKyDN()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKyDN(FormCollection col)
        {
            string latestID = db.DOANHNGHIEPs.OrderByDescending(k => k.MaDN).Select(k => k.MaDN).FirstOrDefault();
            string newID = "DN001"; // Giá trị mặc định
            if (!string.IsNullOrEmpty(latestID))
            {
                int number = int.Parse(latestID.Substring(2));
                number++;
                newID = "DN" + number.ToString("D3");
            }
            var tt = "Chưa Duyệt";

            DOANHNGHIEP b1 = db.DOANHNGHIEPs.FirstOrDefault(t => t.Email.Contains(col["dkyemaildn"]));
            DOANHNGHIEP b2 = db.DOANHNGHIEPs.FirstOrDefault(t => t.SDT.Contains(col["dkysdtdn"]));

            string a = col["dkyemaildn"];
            string a1 = col["dkymatkhaudn"];
            string a2 = col["dkytendn"];
            string a3 = col["dkydiachidn"];
            string a4 = col["dkysdtdn"];
            string a5 = col["dkytinhthanhdn"];
            string a6 = col["dkynamthanhlapdn"];


            if (a == "" || a1 == "" || a2 == "" || a3 == "" || a4 == "" || a5 == "" || a6 == "")
            {
                ModelState.AddModelError("myError", "Vui lòng nhập đầy đủ thông tin");
            }
            else if (b1 != null || b2 != null)
            {
                ModelState.AddModelError("myError", "Số đt hoặc email đã được đăng ký");
            }
            else if (a1.Length < 6 || a1.Length > 15)
            {
                ModelState.AddModelError("myError", "Mật khấu từ 6~15 ký tự");
            }
            else if (col["dkymatkhaudn"] == col["dkyxacnhanmatkhaudn"])
            {
                DOANHNGHIEP dn = new DOANHNGHIEP();
                dn.MaDN = newID;
                dn.Email = col["dkyemaildn"];
                dn.Password_DN = col["dkymatkhaudn"];
                dn.TenDN = col["dkytendn"];
                dn.Diachi = col["dkydiachidn"];
                dn.SDT = col["dkysdtdn"];
                dn.Tinhthanh = col["dkytinhthanhdn"];
                dn.Namthanhlap = col["dkynamthanhlapdn"];
                dn.Trangthai_DN = tt;
                db.DOANHNGHIEPs.InsertOnSubmit(dn);
                db.SubmitChanges();
                ModelState.AddModelError("Messages", "Đăng ký thành công. Vui lòng đợi xét duyệt trong vòng 5 ngày.");
                return View();
            }
            else
            {
                ModelState.AddModelError("Error", "Mật khẩu xác nhận sai!");
                return View();
            }
            return View();
        }

        //======================== Load trang doanh nghiệp =========================
        public ActionResult TrangDN()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TrangDN(FormCollection fc)
        {

            string mauv = fc["ma"].ToLower();
            List<DOANHNGHIEP> lst = db.DOANHNGHIEPs.ToList();
            if (!string.IsNullOrEmpty(mauv))
            {
                lst = lst.Where(l => l.MaDN.ToLower().Contains(mauv)).ToList();
            }
            return View("TrangDN", lst);
        }

        //Feedback Doanh nghiệp
        public ActionResult FeedbackDN()
        {
            return View();
        }


        // Sửa Thông tin cá nhân
        public ActionResult Sua_ThongTin_DN(String id)
        {
            DOANHNGHIEP dn = db.DOANHNGHIEPs.Where(row => row.MaDN == id).FirstOrDefault();
            return View(dn);
        }
        [HttpPost]
        public ActionResult Sua_ThongTin_DN(DOANHNGHIEP dn)
        {
            DOANHNGHIEP uv = db.DOANHNGHIEPs.FirstOrDefault(row => row.MaDN == dn.MaDN);
            if (uv == null)
            {
                ModelState.AddModelError("Error", "Không có dữ liệu");
                return View();
            }
            else
            {
                uv.TenDN = dn.TenDN;
                uv.Diachi = dn.Diachi;
                uv.SDT = dn.SDT;
                uv.Namthanhlap = dn.Namthanhlap;
                uv.Tinhthanh = dn.Tinhthanh;
                db.SubmitChanges();
                return RedirectToAction("IndexDN");
            }
        }


        // Đổi mật khẩu
        public ActionResult Doi_MatKhau_DN(String id)
        {
            DOANHNGHIEP uv = db.DOANHNGHIEPs.Where(row => row.MaDN == id).FirstOrDefault();
            return View(uv);
        }
        [HttpPost]
        public ActionResult Doi_MatKhau_DN(DOANHNGHIEP dn)
        {
            DOANHNGHIEP uv = db.DOANHNGHIEPs.FirstOrDefault(row => row.MaDN == dn.MaDN);
            if (uv == null)
            {
                ModelState.AddModelError("Error", "Không có dữ liệu");
                return View();
            }
            else
            {
                uv.Password_DN = dn.Password_DN;
                db.SubmitChanges();
                return RedirectToAction("IndexDN");
            }
        }
    }
}