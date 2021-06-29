using PaymentService.Classes;
using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Collections.Specialized;
namespace PaymentService.Controllers
{
    public class DataEntryController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult StoringCC()
        {
            string hash_key = "7ZIQHB7YYN";
            String resStr = "";

            string sSignature = Signature.GenerateSHA256("3355796" + hash_key);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //------------- building url string to send
            String sendStr;
            sendStr = "https://process.coriunder.cloud/member/store_card.asp?";
            sendStr += "CompanyNum=" + Server.UrlEncode("3355796") + "&";
            sendStr += "CardNum=" + Server.UrlEncode("4580000000000000") + "&";
            sendStr += "ExpMonth=" + Server.UrlEncode("06") + "&";
            sendStr += "ExpYear=" + Server.UrlEncode("2026") + "&";
            sendStr += "CHFullName=" + Server.UrlEncode("Mahdi Asali") + "&";
            sendStr += "Signature=" + Server.UrlEncode(sSignature);

            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";

            //------------- checking the response
            try
            {
                CookieContainer cookieContainer = new CookieContainer();
                webReq.CookieContainer = cookieContainer;
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                resStr = sr.ReadToEnd();
                TempData["WebRequestHPP"] = sendStr.ToString();
                TempData["ResultHTML"] = resStr.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View();
        }
        public ActionResult Recurring()
        {
            string transID = "";
            //first we decrypt the data

            //this is they key that provided by the company.
            string hash_key = "7ZIQHB7YYN";

            string sSignature = Signature.GenerateSHA256("3355796" + "0" + "1" +
                "1" + "0" + "4580000000000000" + hash_key);

            //now we creating a post request url that contains paramaters that suitable for company.
            String sendStr;
            sendStr = "https://process.coriunder.cloud/member/remote_charge.asp?";
            sendStr += "CompanyNum=" + Server.UrlEncode("3355796") + "&";
            sendStr += "TransType=" + Server.UrlEncode("0") + "&";
            sendStr += "CardNum=" + Server.UrlEncode("4580000000000000") + "&";
            sendStr += "ExpMonth=" + Server.UrlEncode("06") + "&";
            sendStr += "ExpYear=" + Server.UrlEncode("2026") + "&";
            sendStr += "Member=" + Server.UrlEncode("Mahdi") + "&";
            sendStr += "TypeCredit=" + Server.UrlEncode("1") + "&";
            sendStr += "Payments=" + Server.UrlEncode("1") + "&";
            sendStr += "Amount=" + Server.UrlEncode("1") + "&";
            sendStr += "Currency=" + Server.UrlEncode("0") + "&";
            sendStr += "CVV2=" + Server.UrlEncode("123") + "&";
            sendStr += "Email=" + Server.UrlEncode("mahdi@coriunder.com") + "&";
            sendStr += "ClientIP=" + Server.UrlEncode("222.11.545.166") + "&";
            sendStr += "PhoneNumber=" + Server.UrlEncode("2142424222") + "&";
            sendStr += "StoreCc=" + Server.UrlEncode("1") + "&"; //For Charge
            sendStr += "BillingAddress1=" + Server.UrlEncode("Kafr Qaraa") + "&";
            sendStr += "BillingCity=" + Server.UrlEncode("Kafr Qaraa2") + "&";
            sendStr += "Recurring1=" + Server.UrlEncode("12M1") + "&";
            sendStr += "Signature=" + Server.UrlEncode(sSignature);

            //fixing the issue of denied access / ssl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";
            //------------- checking the response
            try
            {
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                String result = sr.ReadToEnd();
                //UpdateLocalDB(trans, result); //updating the local database with the encrypted informations.
                //return RedirectToAction("ResultPage", new { result = result, req = sendStr });
                transID=GetTransID("https://google.com?"+result);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }


            String resStr = "";
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //------------- building url string to send
            String sendStr2;
            sendStr2 = "https://process.coriunder.cloud/member/recurring_modify.aspx?";
            sendStr2 += "MerchantNumber=" + Server.UrlEncode("3355796") + "&";
            sendStr2 += "TransID=" + Server.UrlEncode(transID) + "&";
            sendStr2 += "Action=" + Server.UrlEncode("RESUME");

            //------------- creating the request
            HttpWebRequest webReq2 = (HttpWebRequest)WebRequest.Create(sendStr2);
            webReq2.Method = "GET";

            //------------- checking the response
            try
            {
                CookieContainer cookieContainer = new CookieContainer();
                webReq2.CookieContainer = cookieContainer;
                HttpWebResponse webRes = (HttpWebResponse)webReq2.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                resStr = sr.ReadToEnd();
                TempData["WebRequestHPP"] = sendStr2.ToString();
                TempData["ResultHTML"] = resStr.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View();
        }

        public ActionResult Refund_Request_SP()
        {
            string transID = "";
            //first we decrypt the data

            //this is they key that provided by the company.
            string hash_key = "7ZIQHB7YYN";

            string sSignature = Signature.GenerateSHA256("3355796" + "0" + "1" +
                "100" + "0" + "4580000000000000" + hash_key);

            //now we creating a post request url that contains paramaters that suitable for company.
            String sendStr;
            sendStr = "https://process.coriunder.cloud/member/remote_charge.asp?";
            sendStr += "CompanyNum=" + Server.UrlEncode("3355796") + "&";
            sendStr += "TransType=" + Server.UrlEncode("0") + "&";
            sendStr += "CardNum=" + Server.UrlEncode("4580000000000000") + "&";
            sendStr += "ExpMonth=" + Server.UrlEncode("06") + "&";
            sendStr += "ExpYear=" + Server.UrlEncode("2026") + "&";
            sendStr += "Member=" + Server.UrlEncode("Mahdi") + "&";
            sendStr += "TypeCredit=" + Server.UrlEncode("1") + "&";
            sendStr += "Payments=" + Server.UrlEncode("1") + "&";
            sendStr += "Amount=" + Server.UrlEncode("100") + "&";
            sendStr += "Currency=" + Server.UrlEncode("0") + "&";
            sendStr += "CVV2=" + Server.UrlEncode("123") + "&";
            sendStr += "Email=" + Server.UrlEncode("mahdi@coriunder.com") + "&";
            sendStr += "ClientIP=" + Server.UrlEncode("222.11.545.166") + "&";
            sendStr += "PhoneNumber=" + Server.UrlEncode("2142424222") + "&";
            sendStr += "BillingAddress1=" + Server.UrlEncode("Kafr Qaraa") + "&";
            sendStr += "BillingCity=" + Server.UrlEncode("Kafr Qaraa2") + "&";
            sendStr += "Signature=" + Server.UrlEncode(sSignature);

            //fixing the issue of denied access / ssl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";
            //------------- checking the response
            try
            {
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                String result = sr.ReadToEnd();
                //UpdateLocalDB(trans, result); //updating the local database with the encrypted informations.
                //return RedirectToAction("ResultPage", new { result = result, req = sendStr });
                transID = GetTransID("https://google.com?" + result);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }


            String resStr = "";
            sSignature = Signature.GenerateSHA256("3355796" + hash_key);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //------------- building url string to send
            String sendStr2;
            sendStr2 = "https://process.coriunder.cloud/member/RefundRequest.aspx?";
            sendStr2 += "MerchantNumber=" + Server.UrlEncode("3355796") + "&";
            sendStr2 += "RefTransID=" + Server.UrlEncode(transID) + "&";
            sendStr2 += "Action=" + Server.UrlEncode("CREATE") + "&";
            sendStr2 += "Signature=" + Server.UrlEncode(sSignature);

            //------------- creating the request
            HttpWebRequest webReq2 = (HttpWebRequest)WebRequest.Create(sendStr2);
            webReq2.Method = "GET";

            //------------- checking the response
            try
            {
                CookieContainer cookieContainer = new CookieContainer();
                webReq2.CookieContainer = cookieContainer;
                HttpWebResponse webRes = (HttpWebResponse)webReq2.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                resStr = sr.ReadToEnd();
                TempData["WebRequestHPP"] = sendStr.ToString();
                TempData["ResultHTML"] = resStr.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View();
        }

        public ActionResult Refund_SilentPost()
        {
            return View(new transactions());
        }

        [HttpPost]
        public ActionResult Refund_SilentPost(transactions trans)
        {
            //first we decrypt the data
            //var card_number = AESEncryption.DecryptStringAES(trans.card_number);
            //var cvv = AESEncryption.DecryptStringAES(trans.cvv);
            //var expdate_month = AESEncryption.DecryptStringAES(trans.expdate_month);
            //var expdate_year = AESEncryption.DecryptStringAES(trans.expdate_year);
            //this is they key that provided by the company.
            string hash_key = "7ZIQHB7YYN";
            /*              ##Signature building##
            *   We build the signature by concrating user information one by one 
            *   in addation we add the hash key that provided by the service company.
            *   MerchantID,TransType,TypeCredit,Amount,Currency,CardNumber,RefTransID,HashKey.
            *   CompanyNum + TransType + TypeCredit + Amount + Currency + CardNum + RefTransID + HashKey
            */
            string sSignature = Signature.GenerateSHA256("3355796"+ hash_key);
            //now we creating a post request url that contains paramaters that suitable for company.
            String sendStr;
            sendStr = "https://process.coriunder.cloud/member/RefundRequest.aspx?";
            sendStr += "MerchantNumber=" + Server.UrlEncode("3355796") + "&";
            sendStr += "RefTransID=" + Server.UrlEncode(trans.TransRefID) + "&"; //For Charge
            sendStr += "Action=" + Server.UrlEncode("CREATE") + "&"; //For Charge
            sendStr += "Signature=" + Server.UrlEncode(sSignature);

            //fixing the issue of denied access / ssl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";
            //------------- checking the response
            try
            {
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                String result = sr.ReadToEnd();
                //UpdateLocalDB(trans, result); //updating the local database with the encrypted informations.
                return RedirectToAction("ResultPage", new { result = result, req = sendStr });
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            //if (card_number == "keyError" && cvv == "keyError")
            return View();
        }


        public ActionResult HostedPagePayment()
        {
            return View(new transactions());
        }
        [HttpPost]
        public ActionResult HostedPagePayment(transactions trans)
        {
            return RedirectToAction("PreviewPage_HostedPage", trans);
        }

        public ActionResult ACH_SilentPost()
        {
            return View(new transactions());
        }

        [HttpPost]
        public ActionResult ACH_SilentPost(transactions trans)
        {
            return RedirectToAction("PreviewPage_ACH_SP", trans);
        }
        public ActionResult PreviewPage_ACH_SP(transactions trans)
        {
            return View(trans);
        }
        [HttpPost]
        public ActionResult PreviewPage_ACH_SP(transactions trans, int id = 0)
        {
            //first we decrypt the data
            var card_number = AESEncryption.DecryptStringAES(trans.card_number);
            var cvv = AESEncryption.DecryptStringAES(trans.cvv);
            var expdate_month = AESEncryption.DecryptStringAES(trans.expdate_month);
            var expdate_year = AESEncryption.DecryptStringAES(trans.expdate_year);
            //this is they key that provided by the company.
            string hash_key = "7ZIQHB7YYN";
            string sSignature = Signature.GenerateSHA256("3355796" + trans.card_holder_name+ "42424" + "123456789"+ "123456789" +
                "1"+ "Bank Leumi" + "Kafr Qaraa"+trans.phone+trans.phone+ trans.email 
                + "AF" + "IL" + trans.amount + "0" + "1"+ "1995/02/22" + hash_key);
            //now we creating a post request url that contains paramaters that suitable for company.
            String sendStr;
            sendStr = "https://process.coriunder.cloud/member/remoteCharge_echeck.asp?";
            sendStr += "CompanyNum=" + Server.UrlEncode("3355796") + "&";
            sendStr += "AccountName=" + Server.UrlEncode(trans.card_holder_name) + "&";
            sendStr += "CheckNumber=" + Server.UrlEncode("42424") + "&";
            sendStr += "RoutingNumber=" + Server.UrlEncode("123456789") + "&";
            sendStr += "AccountNumber=" + Server.UrlEncode("123456789") + "&";
            sendStr += "BankAccountType=" + Server.UrlEncode("1") + "&";
            sendStr += "BankName=" + Server.UrlEncode("Bank Leumi") + "&";
            sendStr += "BankCity=" + Server.UrlEncode("Kafr Qaraa") + "&";
            sendStr += "BankPhone=" + Server.UrlEncode(trans.phone) + "&";
            sendStr += "PhoneNumber=" + Server.UrlEncode(trans.phone) + "&";
            sendStr += "Email=" + Server.UrlEncode(trans.email) + "&";
            sendStr += "BankState=" + Server.UrlEncode("AF") + "&";
            sendStr += "BankCountry=" + Server.UrlEncode("IL") + "&";
            sendStr += "Amount=" + Server.UrlEncode(trans.amount.ToString()) + "&";
            sendStr += "Currency=" + Server.UrlEncode("0") + "&";
            sendStr += "CreditType=" + Server.UrlEncode("1") + "&"; //For Charge
            sendStr += "BirthDate=" + Server.UrlEncode("1995/02/22") + "&";
            //sendStr += "Signature=" + Server.UrlEncode(sSignature);

            //fixing the issue of denied access / ssl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";
            //------------- checking the response
            try
            {
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                String result = sr.ReadToEnd();
                return RedirectToAction("ResultPage", new { result = result, req = sendStr });
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View();
        }


        public ActionResult PreviewPage_HostedPage(transactions trans)
        {
            string hash_key = "7ZIQHB7YYN";
            String resStr = "";

            string sSignature = Signature.GenerateSHA256("3355796"+"1"+"0" +
                trans.amount.ToString() + "EUR" +
                "Purchase" +"0" +"1"+ "www.coriunder.com" + "NTPW,CC" + "en-gb" + "auto"+ "https://google.com" + hash_key
                );
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //------------- building url string to send
            String sendStr;
            sendStr = "https://uiservices.coriunder.cloud/hosted/?";
            sendStr += "merchantID=" + Server.UrlEncode("3355796") + "&";
            sendStr += "trans_installments=" + Server.UrlEncode("1") + "&";
            sendStr += "trans_type=" + Server.UrlEncode("0") + "&";
            sendStr += "trans_amount=" + Server.UrlEncode(trans.amount.ToString()) + "&";
            sendStr += "trans_currency=" + Server.UrlEncode("EUR") + "&";
            sendStr += "disp_payFor=" + Server.UrlEncode("Purchase") + "&";
            sendStr += "disp_recurring=" + Server.UrlEncode("0") + "&";
            sendStr += "trans_storePm=" + Server.UrlEncode("1") + "&"; //storing the credit card
            sendStr += "url_redirect=" + Server.UrlEncode("www.coriunder.com") + "&"; //storing the credit card
            sendStr += "disp_paymentType=" + Server.UrlEncode("NTPW,CC") + "&"; //payment type
            sendStr += "disp_lng=" + Server.UrlEncode("en-gb") + "&";
            sendStr += "disp_mobile=" + Server.UrlEncode("auto") + "&";
            sendStr += "notification_url=" + Server.UrlEncode("https://google.com") + "&";
            sendStr += "signature=" + Server.UrlEncode(sSignature);

            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";

            //------------- checking the response
            try
            {
                CookieContainer cookieContainer = new CookieContainer();
                webReq.CookieContainer = cookieContainer;
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                resStr = sr.ReadToEnd();
                TempData["WebRequestHPP"] = sendStr.ToString();
                TempData["ResultHTML"] = resStr.ToString();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return View();
        }


        //Payment Page
        public ActionResult Index()
        {
            return View(new transactions());
        }
        /*
         * this function recieves client details (it should be encrypted)
         * then it's send it to the Preview page as object.
        */
        [HttpPost]
        public ActionResult Index(transactions trans)
        {
            return RedirectToAction("PreviewPage", trans);
        }
        /*
         *the preview page are the page moment before the payment 
         *the client preview the details then if they willing to pay 
         *he/she should press to the pay button 
        */
        public ActionResult PreviewPage(transactions trans)
        {
            return View(trans);
        }
        //function that stores information to local db.
        public void UpdateLocalDB(transactions trans, string result)
        {
            using (Model1 db = new Model1())
            {
                try
                {
                    var rand = new Random();
                    var obj = new transactions();
                    obj.card_holder_name = trans.card_holder_name;
                    obj.amount = trans.amount;
                    obj.address = trans.address;
                    obj.card_number = trans.card_number;
                    obj.cvv = trans.cvv;
                    obj.email = trans.email;
                    obj.expdate_month = trans.expdate_month;
                    obj.expdate_year = trans.expdate_year;
                    obj.phone = trans.phone;
                    obj.zip = trans.zip;
                    obj.country = trans.country;
                    obj.city = trans.city;
                    obj.response_cod = getCode(result).ToString();
                    obj.transaction_cod = rand.Next().ToString();
                    obj.status = "";
                    db.transactions.Add(obj);
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
        }
        public int getCode(string result)
        {
            if (result.Contains("Reply=663"))
            {
                //Transaction still in processing mode
                return 663;
            }
            else if (result.Contains("Reply=525"))
            {
                //Daily volume limit exceeded
                return 525;
            }
            else if (result.Contains("Reply=000"))
            {
                return 000;
                //sucess
            }
            else if (result.Contains("Reply=507"))
            {
                //invalid card
                return 507;
            }
            else if (result.Contains("Reply=001"))
            {
                //pending
                return 001;
            }
            else
            {
                return 999; ///unknown (didnt mentioned all the cases)
            }
        }
        string GetTransID(string result)
        {
            Uri targetUri = new Uri(result);
            string TransApprovalID = HttpUtility.ParseQueryString(targetUri.Query).Get("TransID");
            return TransApprovalID;
        }
        string getCardID(string result)
        {
            Uri targetUri = new Uri(result);
            string TransApprovalID = HttpUtility.ParseQueryString(targetUri.Query).Get("CcStorageID");
            return TransApprovalID;
        }
        string SecondRequest(string result, transactions trans)
        {
            string url = "https://process.coriunder.cloud/member/remote_charge.asp?" + result;
            //getting the transaction ID from result that received.
            string TransID = GetTransID(url);

            var card_number = AESEncryption.DecryptStringAES(trans.card_number);
            var cvv = AESEncryption.DecryptStringAES(trans.cvv);
            var expdate_month = AESEncryption.DecryptStringAES(trans.expdate_month);
            var expdate_year = AESEncryption.DecryptStringAES(trans.expdate_year);
            string hash_key = "7ZIQHB7YYN";
            string sSignature = Signature.GenerateSHA256("3355796" + "2" + "1" +
                trans.amount + "0" + card_number + hash_key);
            //building second request.
            String sendStr;
            sendStr = "https://process.coriunder.cloud/member/remote_charge.asp?";
            sendStr += "CompanyNum=" + Server.UrlEncode("3355796") + "&";
            sendStr += "TransType=" + Server.UrlEncode("2") + "&";
            sendStr += "TransApprovalID=" + Server.UrlEncode(TransID) + "&"; //TransApprovalID in Capture
            sendStr += "CardNum=" + Server.UrlEncode(card_number) + "&";
            sendStr += "ExpMonth=" + Server.UrlEncode(expdate_month) + "&";
            sendStr += "ExpYear=" + Server.UrlEncode(expdate_year) + "&";
            sendStr += "Member=" + Server.UrlEncode(trans.card_holder_name) + "&";
            sendStr += "TypeCredit=" + Server.UrlEncode("1") + "&";
            sendStr += "Payments=" + Server.UrlEncode("1") + "&";
            sendStr += "Amount=" + Server.UrlEncode(trans.amount.ToString()) + "&";
            sendStr += "Currency=" + Server.UrlEncode("0") + "&";
            sendStr += "CVV2=" + Server.UrlEncode(cvv) + "&";
            sendStr += "Email=" + Server.UrlEncode(trans.email) + "&";
            sendStr += "ClientIP=" + Server.UrlEncode("222.11.545.166") + "&";
            sendStr += "PhoneNumber=" + Server.UrlEncode("2142424222") + "&";
            sendStr += "BillingAddress1=" + Server.UrlEncode(trans.address) + "&";
            sendStr += "BillingCity=" + Server.UrlEncode(trans.city) + "&";
            sendStr += "Signature=" + Server.UrlEncode(sSignature);

            //fixing the issue of denied access / ssl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";
            //------------- checking the response
            try
            {
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                String result2 = sr.ReadToEnd();
                //return RedirectToAction("ResultPage", new { result = result2, req = sendStr });
                return result2;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return "";
        }
        string ChargeRequest(string result, transactions trans)
        {
            string url = "https://process.coriunder.cloud/member/remote_charge.asp?" + result;
            //getting the transaction ID from result that received.
            string CcStorageID = getCardID(url);

            var card_number = AESEncryption.DecryptStringAES(trans.card_number);
            var cvv = AESEncryption.DecryptStringAES(trans.cvv);
            var expdate_month = AESEncryption.DecryptStringAES(trans.expdate_month);
            var expdate_year = AESEncryption.DecryptStringAES(trans.expdate_year);
            string hash_key = "7ZIQHB7YYN";
            string sSignature = Signature.GenerateSHA256("3355796" + "3" + "1" +
                trans.amount + "0" + card_number + hash_key);
            //building second request.
            String sendStr;
            sendStr = "https://process.coriunder.cloud/member/remote_charge.asp?";
            sendStr += "CompanyNum=" + Server.UrlEncode("3355796") + "&";
            sendStr += "TransType=" + Server.UrlEncode("3") + "&";
            sendStr += "CardNum=" + Server.UrlEncode(card_number) + "&";
            sendStr += "ExpMonth=" + Server.UrlEncode(expdate_month) + "&";
            sendStr += "ExpYear=" + Server.UrlEncode(expdate_year) + "&";
            sendStr += "Member=" + Server.UrlEncode(trans.card_holder_name) + "&";
            sendStr += "TypeCredit=" + Server.UrlEncode("1") + "&";
            sendStr += "Payments=" + Server.UrlEncode("1") + "&";
            sendStr += "Amount=" + Server.UrlEncode(trans.amount.ToString()) + "&";
            sendStr += "Currency=" + Server.UrlEncode("0") + "&";
            sendStr += "CcStorageID=" + Server.UrlEncode(CcStorageID.ToString()) + "&"; //For Charge
            sendStr += "CVV2=" + Server.UrlEncode(cvv) + "&";
            sendStr += "Email=" + Server.UrlEncode(trans.email) + "&";
            sendStr += "ClientIP=" + Server.UrlEncode("222.11.545.166") + "&";
            sendStr += "PhoneNumber=" + Server.UrlEncode("2142424222") + "&";
            sendStr += "BillingAddress1=" + Server.UrlEncode(trans.address) + "&";
            sendStr += "BillingCity=" + Server.UrlEncode(trans.city) + "&";
            //sendStr += "RetURL=" + Server.UrlEncode("www.coriunder.com") + " & ";
            sendStr += "Signature=" + Server.UrlEncode(sSignature);
            //fixing the issue of denied access / ssl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";
            //------------- checking the response
            try
            {
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                String result2 = sr.ReadToEnd();
                //return RedirectToAction("ResultPage", new { result = result2, req = sendStr });
                return result2;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            return "";
        }
        /*
         * We should get the Encrypted Information and server should send post request
         * to the company server , there the server checks weither the details of specific
         * client are correct, the local server receives from the company server 
         * response code if the response was success ,the local server stores the information to the database
         * and send it to the final page result .
        */
        [HttpPost]
        public ActionResult PreviewPage(transactions trans, int id = 0)
        {
            //first we decrypt the data
            var card_number = AESEncryption.DecryptStringAES(trans.card_number);
            var cvv = AESEncryption.DecryptStringAES(trans.cvv);
            var expdate_month = AESEncryption.DecryptStringAES(trans.expdate_month);
            var expdate_year = AESEncryption.DecryptStringAES(trans.expdate_year);
            //this is they key that provided by the company.
            string hash_key = "7ZIQHB7YYN";
            /*              ##Signature building##
            *   We build the signature by concrating user information one by one 
            *   in addation we add the hash key that provided by the service company.
            *   MerchantID,TransType,TypeCredit,Amount,Currency,CardNumber,RefTransID,HashKey.
            *   CompanyNum + TransType + TypeCredit + Amount + Currency + CardNum + RefTransID + HashKey
            */
            //string sSignature = Signature.GenerateSHA256("3355796" + "1" + "1" +
            //    trans.amount + "0" + card_number + hash_key);

            string sSignature = Signature.GenerateSHA256("3355796" + "3" + "1" +
                trans.amount + "2" + hash_key);
            //now we creating a post request url that contains paramaters that suitable for company.
            String sendStr;
            //sendStr = "https://process.coriunder.cloud/member/remote_charge.asp?";
            //sendStr += "CompanyNum=" + Server.UrlEncode("3355796") + "&";
            //sendStr += "TransType=" + Server.UrlEncode("1") + "&";
            //sendStr += "CcStorageID=" + Server.UrlEncode("76") + "&";
            //sendStr += "CardNum=" + Server.UrlEncode(card_number) + "&";
            //sendStr += "ExpMonth=" + Server.UrlEncode(expdate_month) + "&";
            //sendStr += "ExpYear=" + Server.UrlEncode(expdate_year) + "&";
            //sendStr += "Member=" + Server.UrlEncode(trans.card_holder_name) + "&";
            //sendStr += "TypeCredit=" + Server.UrlEncode("1") + "&";
            //sendStr += "Payments=" + Server.UrlEncode("1") + "&";
            //sendStr += "Amount=" + Server.UrlEncode(trans.amount.ToString()) + "&";
            //sendStr += "Currency=" + Server.UrlEncode("0") + "&";
            //sendStr += "CVV2=" + Server.UrlEncode(cvv) + "&";
            //sendStr += "Email=" + Server.UrlEncode(trans.email) + "&";
            //sendStr += "ClientIP=" + Server.UrlEncode("222.11.545.166") + "&";
            //sendStr += "PhoneNumber=" + Server.UrlEncode("2142424222") + "&";
            //sendStr += "StoreCc=" + Server.UrlEncode("1") + "&"; //For Charge
            //sendStr += "BillingAddress1=" + Server.UrlEncode(trans.address) + "&";
            //sendStr += "BillingCity=" + Server.UrlEncode(trans.city) + "&";
            //sendStr += "Signature=" + Server.UrlEncode(sSignature);

            sendStr = "https://process.coriunder.cloud/member/remote_charge.asp?";
            sendStr += "CompanyNum=" + Server.UrlEncode("3355796") + "&";
            sendStr += "TransType=" + Server.UrlEncode("3") + "&";
            sendStr += "CcStorageID=" + Server.UrlEncode("76") + "&";
            sendStr += "Member=" + Server.UrlEncode(trans.card_holder_name) + "&";
            sendStr += "TypeCredit=" + Server.UrlEncode("1") + "&";
            sendStr += "Payments=" + Server.UrlEncode("1") + "&";
            sendStr += "Amount=" + Server.UrlEncode(trans.amount.ToString()) + "&";
            sendStr += "Currency=" + Server.UrlEncode("2") + "&";
            sendStr += "Email=" + Server.UrlEncode(trans.email) + "&";
            sendStr += "ClientIP=" + Server.UrlEncode("222.11.545.166") + "&";
            sendStr += "PhoneNumber=" + Server.UrlEncode("2142424222") + "&";
            sendStr += "BillingAddress1=" + Server.UrlEncode(trans.address) + "&";
            sendStr += "BillingCity=" + Server.UrlEncode(trans.city) + "&";
            sendStr += "Signature=" + Server.UrlEncode(sSignature);

            //fixing the issue of denied access / ssl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //------------- creating the request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(sendStr);
            webReq.Method = "GET";
            //------------- checking the response
            try
            {
                HttpWebResponse webRes = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(webRes.GetResponseStream());
                String result = sr.ReadToEnd();

                //Capture
                if (trans.TransType == "2")
                {
                   string TransTypeResult = SecondRequest(result,trans);
                   return RedirectToAction("ResultPage", new { result = TransTypeResult, req = sendStr });
                }
                //Charge
                else if (trans.TransType == "3")
                {
                    string TransTypeResult = ChargeRequest(result, trans);
                    return RedirectToAction("ResultPage", new { result = TransTypeResult, req = sendStr });

                }
                //UpdateLocalDB(trans, result); //updating the local database with the encrypted informations.
                return RedirectToAction("ResultPage", new { result = result, req = sendStr });
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            //if (card_number == "keyError" && cvv == "keyError")
            return View();
        }
        [HttpGet]
        //this function receive the final result (Accepted/Declined).
        public ActionResult ResultPage(string result, string req)
        {
            TempData["response"] = result.ToString();
            TempData["request"] = req.ToString();

            if (result.Contains("Reply=663"))
            {
                //Transaction still in processing mode
                TempData["Result"] = "STILL";
                TempData["Code"] = 663;
            }
            else if (result.Contains("Reply=525"))
            {
                //Daily volume limit exceeded
                TempData["Result"] = "EXCEED";
                TempData["Code"] = 525;

            }
            else if (result.Contains("Reply=000"))
            {
                TempData["Result"] = "SUCCESS";
                TempData["Code"] = 000;


                //sucess
            }
            else if (result.Contains("Reply=507"))
            {
                //invalid card
                TempData["Result"] = "INVALIDCARD";
                TempData["Code"] = 507;


            }
            else if (result.Contains("Reply=001"))
            {
                //pending
                TempData["Result"] = "PENDING";
                TempData["Code"] = 001;


            }
            else
            {
                //other case (not included)
                TempData["Result"] = "OTHER";
                TempData["Code"] = 999;

            }
            return View();
        }
    }
}