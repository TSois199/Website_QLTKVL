using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite_DKTKViecLam.Models;

namespace WebSite_DKTKViecLam.Controllers
{
    public class UngVienController : Controller
    {
        QL_TKVLDataContext db = new QL_TKVLDataContext();

        // GET: UngVien
        public ActionResult IndexUngVien()
        {
            List<BAIDANG> u = db.BAIDANGs.ToList();
            return View(u);
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
        //Xem chi tiết bài đăng ứng viên
        public ActionResult XemChiTietBaiDang_UV(string id)
        {
            BAIDANG s = db.BAIDANGs.FirstOrDefault(i => i.MaBD == id);
            Session["mabaidang"] = s.MaBD;
            Session["tenbaidang"] = s.TenBD;
            return View(s);
        }

        //=========================Register/Login/Logout======================
        [HttpGet]
        public ActionResult DangNhapUV()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapUV(FormCollection col)
        {
            NGUOIUNGTUYEN uv = db.NGUOIUNGTUYENs.FirstOrDefault(a => a.Email == col["email"] && a.Password_NUT == col["pswd"]);
            if (uv != null)
            {
                Session["uv"] = uv;
                Session["tenUV"] = uv.Hoten;
                Session["maUV"] = uv.MaNUT;
                Session["emailUV"] = uv.Email;
                Session["SDT"] = uv.SDT;
                Session["ngaysinh"] = uv.Ngaysinh;
                return RedirectToAction("IndexUngVien", "UngVien");
            }
            else
            {
                ModelState.AddModelError("myError", "Invalue email and password.");
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["uv"] = null;
            Session["tenUV"] = null;
            return RedirectToAction("/Home/Index");
        }

        //Dang Ky Tuc la them ung vien moi
        public ActionResult DangKyUV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKyUV(FormCollection col)
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
                    return RedirectToAction("DangNhapUV", "UngVien");

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


        //======================== Load trang cá nhân =========================
        public ActionResult TrangCaNhan()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TrangCaNhan(FormCollection fc)
        {

            string mauv = fc["ma"].ToLower();
            List<NGUOIUNGTUYEN> lst = db.NGUOIUNGTUYENs.ToList();
            if (!string.IsNullOrEmpty(mauv))
            {
                lst = lst.Where(l => l.MaNUT.ToLower().Contains(mauv)).ToList();
            }
            return View("TrangCaNhan", lst);
        }

        // Sửa Thông tin cá nhân
        public ActionResult Sua_ThongTinCaNhan_UngVien(String id)
        {
            NGUOIUNGTUYEN uv = db.NGUOIUNGTUYENs.Where(row => row.MaNUT == id).FirstOrDefault();
            return View(uv);
        }
        [HttpPost]
        public ActionResult Sua_ThongTinCaNhan_UngVien(NGUOIUNGTUYEN nut)
        {
            NGUOIUNGTUYEN uv = db.NGUOIUNGTUYENs.FirstOrDefault(row => row.MaNUT == nut.MaNUT);
            if (uv == null)
            {
                ModelState.AddModelError("Error", "Không có dữ liệu");
                return View();
            }
            else
            {
                uv.Hoten = nut.Hoten;
                uv.Diachi = nut.Diachi;
                uv.SDT = nut.SDT;
                uv.CV_NUT = nut.CV_NUT;
                uv.Ngaysinh = nut.Ngaysinh;
                uv.Gioitinh = nut.Gioitinh;
                db.SubmitChanges();
                return View("IndexUngVien");
            }
        }


        // Đổi mật khẩu
        public ActionResult Doi_MatKhau_UV(String id)
        {
            NGUOIUNGTUYEN uv = db.NGUOIUNGTUYENs.Where(row => row.MaNUT == id).FirstOrDefault();
            return View(uv);
        }
        [HttpPost]
        public ActionResult Doi_MatKhau_UV(NGUOIUNGTUYEN nut)
        {
            NGUOIUNGTUYEN uv = db.NGUOIUNGTUYENs.FirstOrDefault(row => row.MaNUT == nut.MaNUT);
            if (uv == null)
            {
                ModelState.AddModelError("Error", "Không có dữ liệu");
                return View();
            }
            else
            {
                uv.Password_NUT = nut.Password_NUT;
                db.SubmitChanges();
                List<BAIDANG> u = db.BAIDANGs.ToList();
                return TrangCaNhan();
            }
        }

        //Nộp CV
        public ActionResult UngTuyen_UV()
        {
            return View();
        }

        //Feedback Ứng viên
        public ActionResult FeedbackUV()
        {
            return View();
        }

        //Report bài đăng Ứng viên
        public ActionResult ReportUV()
        {
            return View();
        }

        //======================== Tìm kiếm nâng cao của Ứng Viên =========================
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
                lst = lst.Where(l => l.TenBD.ToLower().Contains(congviec)).ToList();
            }
            if (!string.IsNullOrEmpty(congty))
            {
                lst = lst.Where(l => l.DOANHNGHIEP.TenDN.ToLower().Contains(congty)).ToList();
            }
            return View("Index", lst);

        }
    }
}