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
    public class AdminController : Controller
    {
        QL_TKVLDataContext db = new QL_TKVLDataContext();
        // GET: Admin
        public ActionResult IndexADMIN()
        {
            return View();
        }

        //=========================Register/Login/Logout======================
        [HttpGet]
        public ActionResult DangNhapAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapAdmin(FormCollection  col)
        {
            TK_Admin ad = db.TK_Admins.FirstOrDefault(a => a.Email == col["email"] && a.Password_AD == col["pswd"]);
            if (ad != null)
            {
                Session["ad"] = ad;
                Session["tenTK"] = ad.Hoten;
                Session["maAD"] = ad.MaAD;
                return RedirectToAction("IndexADMIN", "Admin");
            }
            else
            {
                ModelState.AddModelError("myError", "Invalue email and password.");
            }
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["ad"] = null;
            Session["tenTK"] = null;
            return RedirectToAction("/Home/Index");
        }


        //=========================QL_DoanhNghiep======================

        public ActionResult QL_DoanhNghiep()
        {
            List<DOANHNGHIEP> qldn = db.DOANHNGHIEPs.ToList();
            return View(qldn);
        }


        //Thêm doanh nghiep moi

        public ActionResult Them_DN()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Them_DN(FormCollection col)
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
                return View("QL_DoanhNghiep");
            }
            else
            {
                ModelState.AddModelError("Error", "Mật khẩu xác nhận sai!");
                return View();
            }
            return View();
        }

        //Xóa doanh nghiep 
        public ActionResult Xoa_DoanhNghiep(String id)
        {
            DOANHNGHIEP doanh = db.DOANHNGHIEPs.Where(row => row.MaDN == id).FirstOrDefault();
            return View(doanh);
        }
        [HttpPost]
        public ActionResult Xoa_DoanhNghiep(DOANHNGHIEP dn)
        {
            DOANHNGHIEP doanh = db.DOANHNGHIEPs.Where(row => row.MaDN == dn.MaDN).FirstOrDefault();
            db.DOANHNGHIEPs.DeleteOnSubmit(doanh);
            db.SubmitChanges();
            return RedirectToAction("QL_DoanhNghiep");
        }

        // Sửa Doanh Nghiep
        public ActionResult Sua_DN(String id)
        {
            DOANHNGHIEP dn = db.DOANHNGHIEPs.Where(row => row.MaDN == id).FirstOrDefault();
            return View(dn);
        }
        [HttpPost]
        public ActionResult Sua_DN(DOANHNGHIEP dn)
        {
            DOANHNGHIEP dnn = db.DOANHNGHIEPs.FirstOrDefault(row => row.MaDN == dn.MaDN);
            if (dnn == null)
            {
                ModelState.AddModelError("Error","Không có dữ liệu");
                return View();
            }
            else
            {
                dnn.TenDN = dn.TenDN;
                dnn.Diachi = dn.Diachi;
                dnn.SDT = dn.SDT;
                dnn.Tinhthanh = dn.Tinhthanh;
                db.SubmitChanges();
                return View();
            }
        }


        //=========================QL_UngVien======================
        public ActionResult QL_UngVien()
        {
            List<NGUOIUNGTUYEN> nut = db.NGUOIUNGTUYENs.ToList();
            return View(nut);
        }

        // Thêm ứng viên mới 
        public ActionResult Them_UV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Them_UV(FormCollection col)
        {
            string latestID = db.NGUOIUNGTUYENs.OrderByDescending(k => k.MaNUT).Select(k => k.MaNUT).FirstOrDefault();
            string newID = "NUT001"; // Giá trị mặc định
            if (!string.IsNullOrEmpty(latestID))
            {
                int number = int.Parse(latestID.Substring(3));
                number++;
                newID = "NUT" + number.ToString("D3");
            }

            string dateString = col["dkyngaysinhuv"];
            DateTime date;

            string a = col["dkyemailuv"];
            string a1 = col["dkymatkhauuv"];
            string a2 = col["dkytenuv"];
            string a3 = col["dkydiachiuv"];
            string a4 = col["dkysdtuv"];
            string a5 = col["dkygioitinhuv"];
            string a6 = col["dkyngaysinhuv"];

            NGUOIUNGTUYEN b1 = db.NGUOIUNGTUYENs.FirstOrDefault(t => t.Email.Contains(col["dkyemailuv"]));
            NGUOIUNGTUYEN b2 = db.NGUOIUNGTUYENs.FirstOrDefault(t => t.SDT.Contains(col["dkysdtuv"]));

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
            else if (col["dkymatkhauuv"] == col["dkyxacnhanmatkhauuv"])
            {
                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    NGUOIUNGTUYEN nut = new NGUOIUNGTUYEN();
                    nut.MaNUT = newID;
                    nut.Email = col["dkyemailuv"];
                    nut.Password_NUT = col["dkymatkhauuv"];
                    nut.Hoten = col["dkytenuv"];
                    nut.Diachi = col["dkydiachiuv"];
                    nut.SDT = col["dkysdtuv"];
                    nut.Gioitinh = col["dkygioitinhuv"];
                    nut.Ngaysinh = date;

                    db.NGUOIUNGTUYENs.InsertOnSubmit(nut);
                    db.SubmitChanges();
                    return RedirectToAction("QL_UngVien", "Admin");
                }
                else
                {
                    ModelState.AddModelError("Error", "Ngày sinh không hợp lệ");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Mật khẩu xác nhận sai!");
            }
            return View();
        }

        // Xóa ứng viên
        public ActionResult Xoa_UngVien(String id)
        {
            NGUOIUNGTUYEN nut = db.NGUOIUNGTUYENs.Where(row => row.MaNUT == id).FirstOrDefault();
            return View(nut);
        }
        [HttpPost]
        public ActionResult Xoa_UngVien(NGUOIUNGTUYEN nguoiut)
        {
            NGUOIUNGTUYEN nut = db.NGUOIUNGTUYENs.Where(row => row.MaNUT == nguoiut.MaNUT).FirstOrDefault();
            db.NGUOIUNGTUYENs.DeleteOnSubmit(nut);
            db.SubmitChanges();
            return RedirectToAction("QL_UngVien");
        }

        //Sửa ứng viên
        public ActionResult Sua_UV(String id)
        {
            NGUOIUNGTUYEN nut = db.NGUOIUNGTUYENs.Where(row => row.MaNUT == id).FirstOrDefault();
            return View(nut);
        }
        [HttpPost]
        public ActionResult Sua_UV(NGUOIUNGTUYEN nu)
        {
            NGUOIUNGTUYEN nut = db.NGUOIUNGTUYENs.FirstOrDefault(row => row.MaNUT == nu.MaNUT);
            if (nut == null)
            {
                ModelState.AddModelError("Error", "Sửa không thành công");
                return View();
            }
            else
            {
                nut.Hoten = nu.Hoten;
                nut.Diachi = nu.Diachi;
                nut.SDT = nu.SDT;
                nut.Gioitinh = nu.Gioitinh;
                nut.Ngaysinh = nu.Ngaysinh;
                nut.Password_NUT = nu.Password_NUT;

                db.SubmitChanges();
                return View();
            }
        }
        //Chi Tiết Ứng Viên
        public ActionResult ChiTiet_UV(string id)
        {
            NGUOIUNGTUYEN nut = db.NGUOIUNGTUYENs.Where(row => row.MaNUT == id).FirstOrDefault();
            return View(nut);
        }

        //=========================QL_CongViec======================
        public ActionResult QL_CongViec()
        {
            List<LOAICONGVIEC> cv = db.LOAICONGVIECs.ToList();
            return View(cv);
        }
        //======Thêm Loại Công Việc============
        public ActionResult Them_Congviec()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Them_Congviec(FormCollection col, HttpPostedFileBase uploadhinh)
        {
            string latestID = db.LOAICONGVIECs.OrderByDescending(k => k.Maloai).Select(k => k.Maloai).FirstOrDefault();
            string newID = "L001"; // Giá trị mặc định
            if (!string.IsNullOrEmpty(latestID))
            {
                int number = int.Parse(latestID.Substring(1));
                number++;
                newID = "L" + number.ToString("D3");
            }
            var b = col["congviec"];
            var c = col["acongviec"];
            LOAICONGVIEC a = new LOAICONGVIEC();
            a.Maloai = newID;
            a.Tenloai = col["congviec"];
            a.Anh_Cviec = col["acongviec"];
            db.LOAICONGVIECs.InsertOnSubmit(a);
            db.SubmitChanges();

            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
                string id = db.LOAICONGVIECs.ToList().Last().Maloai.ToString();
                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "c" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/HinhAnhBaiDang"), _FileName);
                uploadhinh.SaveAs(_path);

                LOAICONGVIEC lcv = db.LOAICONGVIECs.FirstOrDefault(x => x.Maloai == id);
                lcv.Anh_Cviec = _FileName;
                db.SubmitChanges();
            }

            return RedirectToAction("QL_CongViec", "Admin");
        }

        //======Xóa Loại Công Việc============
        public ActionResult Xoa_LoaiCongViec(String id)
        {
            LOAICONGVIEC loaicv = db.LOAICONGVIECs.Where(row => row.Maloai == id).FirstOrDefault();
            return View(loaicv);
        }
        [HttpPost]
        public ActionResult Xoa_LoaiCongViec(LOAICONGVIEC loai)
        {
            LOAICONGVIEC loaicv = db.LOAICONGVIECs.Where(row => row.Maloai == loai.Maloai).FirstOrDefault();
            db.LOAICONGVIECs.DeleteOnSubmit(loaicv);
            db.SubmitChanges();
            return RedirectToAction("QL_CongViec");
        }

        //======Sửa Loại Công Việc============

        public ActionResult Sua_LoaiCongViec(String id)
        {
            LOAICONGVIEC cv = db.LOAICONGVIECs.Where(row => row.Maloai == id).FirstOrDefault();
            return View(cv);
        }
        [HttpPost]
        public ActionResult Sua_LoaiCongViec(LOAICONGVIEC lcv, HttpPostedFileBase uploadhinh)
        {
            LOAICONGVIEC locov = db.LOAICONGVIECs.FirstOrDefault(x => x.Maloai == lcv.Maloai);
            locov.Tenloai = lcv.Tenloai;

            if (uploadhinh != null && uploadhinh.ContentLength > 0)
            {
                string id = lcv.Maloai;

                string _FileName = "";
                int index = uploadhinh.FileName.IndexOf('.');
                _FileName = "lcv" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                string _path = Path.Combine(Server.MapPath("~/HinhAnhBaiDang"), _FileName);
                uploadhinh.SaveAs(_path);
                locov.Anh_Cviec = _FileName;
            }

            db.SubmitChanges();

            return RedirectToAction("QL_CongViec");
        }

        //=========================QL_BaiDang======================
        public ActionResult QL_BaiDang()
        {
            List<BAIDANG> bdang = db.BAIDANGs.ToList();
            return View(bdang);
        }
        //Xem chi tiet bai dang
        public ActionResult XemChiTietBaiDang_AD(string id)
        {
            BAIDANG s = db.BAIDANGs.FirstOrDefault(i => i.MaBD == id);
            return View(s);
        }

        // Xóa bài đăng của AD
        public ActionResult Xoa_BaiDang_AD(String id)
        {
            BAIDANG bd = db.BAIDANGs.Where(row => row.MaBD == id).FirstOrDefault();
            return View(bd);
        }
        [HttpPost]
        public ActionResult Xoa_BaiDang_AD(BAIDANG bdang)
        {
            BAIDANG bd = db.BAIDANGs.Where(row => row.MaBD == bdang.MaBD).FirstOrDefault();
            db.BAIDANGs.DeleteOnSubmit(bd);
            db.SubmitChanges();
            return RedirectToAction("QL_BaiDang");
        }


        //Thêm Loại Bài Đăng
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
        //    var ml = col["mluong"];
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
        //    a.Ngaydangbai = col["ndbai"];
        //    a.Yeucau = col["ycau"];
        //    db.LOAICONGVIECs.InsertOnSubmit(a);
        //    db.SubmitChanges();
        //    return RedirectToAction("QL_CongViec", "Admin");
        //}


        ////======Thêm Bài đăng============
        //public ActionResult Them_BaiDang()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Them_BaiDang(FormCollection col)
        //{
        //    string latestID = db.BAIDANGs.OrderByDescending(k => k.MaBD).Select(k => k.MaBD).FirstOrDefault();
        //    string newID = "BD001"; // Giá trị mặc định
        //    string dateString = col["ndbai"];
        //    DateTime date;
        //    if (!string.IsNullOrEmpty(latestID))
        //    {
        //        int number = int.Parse(latestID.Substring(1));
        //        number++;
        //        newID = "BD" + number.ToString("D3");
        //    }
        //    var tbd = col["tenbd"];
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
        //    if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //    {
        //        BAIDANG a = new BAIDANG();
        //        a.MaBD = newID;
        //        a.TenBD = col["tenbd"];
        //        a.MaDN = col["madn"];
        //        a.Maloai = col["mloai"];
        //        a.Mota = col["mta"];
        //        a.Phucloi = col["ploi"];
        //        a.Vitri = col["vtri"];
        //        a.Kinhnghiem = col["knghiem"];
        //        a.Dotuoilamviec = col["dtlviec"];
        //        a.Mucluong = col["mluong"];
        //        a.Ngaydangbai = date;
        //        a.Yeucau = col["ycau"];

        //        db.BAIDANGs.InsertOnSubmit(a);
        //        db.SubmitChanges();
        //        return RedirectToAction("QL_BaiDang", "Admin");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("Error", "Ngày đăng bài không hợp lệ");
        //        return View();
        //    }

        //}

        //=====================CHỨC NĂNG DUYỆT===================
        //List danh sách bài đăng chưa duyệt
        public ActionResult IndexADMIN_Duyetbaidang()
        {
            List<BAIDANG> bd = db.BAIDANGs.ToList();
            return View(bd);
        }

        //Duyệt bài đăng
        public ActionResult Duyet_BD()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Duyet_BD(FormCollection col)
        {
            string mabd = col["maBD"];
            Session["mabd"] = mabd;

            return RedirectToAction("Duyet_BD", "Admin");
        }

        [HttpPost]
        public ActionResult Duyet_BD_OK(FormCollection col)
        {
            db.sp_Duyet_Insert(col["maBD"], col["maAD"]);
            db.SubmitChanges();
            return RedirectToAction("IndexADMIN_Duyetbaidang", "Admin");
        }

        //Duyet doanh nghiệp
        public ActionResult IndexADMIN_DuyetDN()
        {
            List<DOANHNGHIEP> dn = db.DOANHNGHIEPs.ToList();
            return View(dn);
        }

        public ActionResult XemChiTietBaiDang_AD_Duyet(string id)
        {
            BAIDANG bdd = db.BAIDANGs.FirstOrDefault(i => i.MaBD == id);
            return View(bdd);
        }

        //Duyệt doanh nghiệp

        public ActionResult Duyet_DN(String id)
        {
            DOANHNGHIEP dn = db.DOANHNGHIEPs.Where(row => row.MaDN == id).FirstOrDefault();
            return View(dn);
        }
        [HttpPost]
        public ActionResult Duyet_DN(DOANHNGHIEP dn)
        {
            DOANHNGHIEP dnn = db.DOANHNGHIEPs.FirstOrDefault(row => row.MaDN == dn.MaDN);
            if (dnn == null)
            {
                ModelState.AddModelError("Error", "Duyệt không thành công");
                return View();
            }
            else
            {
                dnn.Trangthai_DN = dn.Trangthai_DN;
                db.SubmitChanges();
                return View();
            }
        }


        //======================== Tìm kiếm Quản Lý Ứng Viên =========================
        public ActionResult TimKiemUV()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XL_TimKiemUV(FormCollection fc)
        {
            string maUV = fc["maUV"].ToLower();
            string tenUV = fc["tenUV"].ToLower();
            string emailUV = fc["emailUV"].ToLower();
            List<NGUOIUNGTUYEN> nut = db.NGUOIUNGTUYENs.ToList();
            if (!string.IsNullOrEmpty(maUV))
            {
                nut = nut.Where(l => l.MaNUT.ToLower().Contains(maUV)).ToList();
            }
            if (!string.IsNullOrEmpty(tenUV))
            {
                nut = nut.Where(l => l.Hoten.ToLower().Contains(tenUV)).ToList();
            }
            if (!string.IsNullOrEmpty(emailUV))
            {
                nut = nut.Where(l => l.Email.ToLower().Contains(emailUV)).ToList();
            }
            return View("QL_UngVien", nut);

        }


        //======================== Tìm kiếm Quản lý bài đăng =========================
        public ActionResult TimKiemBD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XL_TimKiemBD(FormCollection fc)
        {
            string maBD = fc["maBD"].ToLower();
            string tenBD = fc["tenBD"].ToLower();
            string tenDN = fc["tenDN"].ToLower();
            List<BAIDANG> bd = db.BAIDANGs.ToList();
            if (!string.IsNullOrEmpty(maBD))
            {
                bd = bd.Where(l => l.MaBD.ToLower().Contains(maBD)).ToList();
            }
            if (!string.IsNullOrEmpty(tenBD))
            {
                bd = bd.Where(l => l.TenBD.ToLower().Contains(tenBD)).ToList();
            }
            if (!string.IsNullOrEmpty(tenDN))
            {
                bd = bd.Where(l => l.DOANHNGHIEP.TenDN.ToLower().Contains(tenDN)).ToList();
            }
            return View("QL_BaiDang", bd);

        }

        //======================== Tìm kiếm Quản lý doanh nghiệp =========================
        public ActionResult TimKiemDN()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XL_TimKiemDN(FormCollection fc)
        {
            string maDN = fc["maDN"].ToLower();
            string tenDN = fc["tenDN"].ToLower();
            string emailDN = fc["emailDN"].ToLower();
            List<DOANHNGHIEP> dn = db.DOANHNGHIEPs.ToList();
            if (!string.IsNullOrEmpty(maDN))
            {
                dn = dn.Where(l => l.MaDN.ToLower().Contains(maDN)).ToList();
            }
            if (!string.IsNullOrEmpty(tenDN))
            {
                dn = dn.Where(l => l.TenDN.ToLower().Contains(tenDN)).ToList();
            }
            if (!string.IsNullOrEmpty(emailDN))
            {
                dn = dn.Where(l => l.Email.ToLower().Contains(emailDN)).ToList();
            }
            return View("QL_DoanhNghiep", dn);

        }


        //======================== Tìm kiếm Quản lý bài đăng đang chờ duyệt =========================
        public ActionResult TimKiemBD_ChoDuyet()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XL_TimKiemBD_ChoDuyet(FormCollection fc)
        {
            string maBD = fc["maBD"].ToLower();
            string tenBD = fc["tenBD"].ToLower();
            string tenDN = fc["tenDN"].ToLower();
            List<BAIDANG> bd = db.BAIDANGs.ToList();
            if (!string.IsNullOrEmpty(maBD))
            {
                bd = bd.Where(l => l.MaBD.ToLower().Contains(maBD)).ToList();
            }
            if (!string.IsNullOrEmpty(tenBD))
            {
                bd = bd.Where(l => l.TenBD.ToLower().Contains(tenBD)).ToList();
            }
            if (!string.IsNullOrEmpty(tenDN))
            {
                bd = bd.Where(l => l.DOANHNGHIEP.TenDN.ToLower().Contains(tenDN)).ToList();
            }
            return View("IndexADMIN_Duyetbaidang", bd);

        }


        //======================== Tìm kiếm Quản lý doanh nghiệp đang chờ duyệt =========================
        public ActionResult TimKiemDN_ChoDuyet()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XL_TimKiemDN_ChoDuyet(FormCollection fc)
        {
            string maDN = fc["maDN"].ToLower();
            string tenDN = fc["tenDN"].ToLower();
            string emailDN = fc["emailDN"].ToLower();
            List<DOANHNGHIEP> dn = db.DOANHNGHIEPs.ToList();
            if (!string.IsNullOrEmpty(maDN))
            {
                dn = dn.Where(l => l.MaDN.ToLower().Contains(maDN)).ToList();
            }
            if (!string.IsNullOrEmpty(tenDN))
            {
                dn = dn.Where(l => l.TenDN.ToLower().Contains(tenDN)).ToList();
            }
            if (!string.IsNullOrEmpty(emailDN))
            {
                dn = dn.Where(l => l.Email.ToLower().Contains(emailDN)).ToList();
            }
            return View("IndexADMIN_DuyetDN", dn);

        }

                //======================== Load trang cá nhân =========================
        public ActionResult TrangCaNhan_AD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TrangCaNhan_AD(FormCollection fc)
        {

            string maad = fc["ma"].ToLower();
            List<TK_Admin> lst = db.TK_Admins.ToList();
            if (!string.IsNullOrEmpty(maad))
            {
                lst = lst.Where(l => l.MaAD.ToLower().Contains(maad)).ToList();
            }
            return View("TrangCaNhan_AD", lst);
        }

        // Sửa Thông tin cá nhân
        public ActionResult Sua_ThongTinCaNhan_AD(String id)
        {
            TK_Admin uv = db.TK_Admins.Where(row => row.MaAD == id).FirstOrDefault();
            return View(uv);
        }
        [HttpPost]
        public ActionResult Sua_ThongTinCaNhan_AD(TK_Admin tkadmin)
        {
            TK_Admin ad = db.TK_Admins.FirstOrDefault(row => row.MaAD == tkadmin.MaAD);
            if (ad == null)
            {
                ModelState.AddModelError("Error", "Không có dữ liệu");
                return View();
            }
            else
            {
                ad.Hoten = tkadmin.Hoten;
                ad.Diachi = tkadmin.Diachi;
                ad.SDT = tkadmin.SDT;
                ad.Ngaysinh = tkadmin.Ngaysinh;
                ad.Gioitinh = tkadmin.Gioitinh;
                db.SubmitChanges();
                return View("IndexADMIN");
            }
        }


        // Đổi mật khẩu
        public ActionResult Doi_MatKhau_AD(String id)
        {
            TK_Admin uv = db.TK_Admins.Where(row => row.MaAD == id).FirstOrDefault();
            return View(uv);
        }
        [HttpPost]
        public ActionResult Doi_MatKhau_AD(TK_Admin tkadmin)
        {
            TK_Admin ad = db.TK_Admins.FirstOrDefault(row => row.MaAD == tkadmin.MaAD);
            if (ad == null)
            {
                ModelState.AddModelError("Error", "Không có dữ liệu");
                return View();
            }
            else
            {
                ad.Password_AD = tkadmin.Password_AD;
                db.SubmitChanges();
                return TrangCaNhan_AD();
            }
        }
    }
}