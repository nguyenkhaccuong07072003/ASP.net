using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VNPAY_CS_ASPX;
using Advanced.Models;

namespace Advanced.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (TempData["SuccessMessageBuy"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessageBuy"].ToString();
            }
            if (TempData["ErrorMessageBuy"] != null)
            {
                ViewBag.ErrorMessageBuy = TempData["ErrorMessageBuy"].ToString();
            }
            return View();
        }
        static string GenerateRandomString(int length, string characters)
        {
            Random random = new Random();
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                // Chọn một ký tự ngẫu nhiên từ chuỗi characters
                int index = random.Next(characters.Length);
                char randomChar = characters[index];

                // Thêm ký tự ngẫu nhiên vào chuỗi kết quả
                result.Append(randomChar);
            }

            return result.ToString();
        }
        public ActionResult Onepay(int id)
        {
            string userid = User.Identity.GetUserId();
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser ngdung = UserManager.FindById(userid);
            Khoahoc kh = db.Khoahocs.FirstOrDefault(m => m.kh_id == id);
            ViewBag.FullName = ngdung.Fullname;
            return View(kh);
        }
        [HttpPost]
        public ActionResult Twopay(string TypePaymentVN, int id)
        {
            int length = 6;
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = GenerateRandomString(length, characters);
            int typePaymentVNInt = int.Parse(TypePaymentVN);
            var code = new { Success = true, Code = typePaymentVNInt, Url = "" };
            Khoahoc kh = db.Khoahocs.FirstOrDefault(m => m.kh_id == id);
            string userid = User.Identity.GetUserId();
            var url = UrlPayment(kh, 2, randomString);
            code = new { Success = true, Code = typePaymentVNInt, Url = url };
            DatHang dathang = new DatHang
            {
                MaDonHang = randomString,
                TongTien = kh.Price,
                UserId = userid,
                NgayMua = DateTime.Now,
                TrangThai = true,
                kh_id = kh.kh_id,
            };
            db.DatHang.Add(dathang);
            db.SaveChanges();
            return Redirect(code.Url);
        }
        public string UrlPayment(Khoahoc kh, int typementVn, string id)
        {
            // Tính tổng tiền

            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Get payment input



            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (kh.Price * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (typementVn == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (typementVn == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (typementVn == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang::" + id);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", id);
            // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return paymentUrl;
        }
        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];

                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;

                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        TempData["SuccessMessageBuy"] = "Total (VND):" + vnp_Amount.ToString();
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        TempData["ErrorMessageBuy"] = "Error: " + vnp_ResponseCode;
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}