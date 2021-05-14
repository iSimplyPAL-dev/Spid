using Italia.Spid.AspNet.WebApp.Models;
using Italia.Spid.Authentication;
using Italia.Spid.Authentication.IdP;
using Italia.Spid.Authentication.Saml;
using Italia.Spid.Authentication.Schema;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Fluentx.Mvc;
using MySql;
using MySql.Data.MySqlClient;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Text;

namespace Italia.Spid.AspNet.WebApp.Controllers
{
    public class ACSController : Controller
    {
        ILog log = LogManager.GetLogger(typeof(ACSController));
        private readonly string SPID_COOKIE = ConfigurationManager.AppSettings["SPID_COOKIE"];

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
    

            string IdPName;
            string spidAuthnRequestId;
            
            // Try to get auth requesta data from cookie
            HttpCookie cookie = Request.Cookies[SPID_COOKIE];
            if (cookie != null)
             {
                IdPName = cookie["IdPName"];
                spidAuthnRequestId = cookie["SpidAuthnRequestId"];
                log.Info($"Cookie {SPID_COOKIE} IdPName: {IdPName}, SpidAuthnRequestId: {spidAuthnRequestId}");
            }
            else
            {
                log.Error("Error on ACSController [HttpPost]Index method: Impossibile recuperare l'Id della sessione.");
                ViewData["Message"] = "Impossibile recuperare i dati della sessione (cookie scaduto).";
                return View("Error");
            }
            
            try
            {
                IdpAuthnResponse idpAuthnResponse = SamlHelper.GetAuthnResponse(formCollection["SAMLResponse"].ToString());
                
                if (!idpAuthnResponse.IsSuccessful)
                {
                    Session["AppUser"] = null;

                    log.Error($"Error on ACSController [HttpPost]Index method: La risposta dell'IdP riporta il seguente StatusCode: {idpAuthnResponse.StatusCodeInnerValue} con StatusMessage: {idpAuthnResponse.StatusMessage} e StatusDetail: {idpAuthnResponse.StatusDetail}.");
                    ViewData["Message"] = "La richiesta di identificazione è stata rifiutata.";
                    ViewData["ErrorMessage"] = $"StatusCode: {idpAuthnResponse.StatusCodeInnerValue} con StatusMessage: {idpAuthnResponse.StatusMessage} e StatusDetail: {idpAuthnResponse.StatusDetail}.";
                    return View("Error");
                }

                // Verifica la corrispondenza del valore di spidAuthnRequestId ricavato da cookie con quello restituito dalla risposta
                if (!SamlHelper.ValidAuthnResponse(idpAuthnResponse, spidAuthnRequestId, Request.Url.ToString()))
                {
                    Session["AppUser"] = null;

                    log.Error($"Error on ACSController [HttpPost]Index method: La risposta dell'IdP non è valida (InResponseTo != spidAuthnRequestId oppure SubjectConfirmationDataRecipient != requestPath).");
                    ViewData["Message"] = "La risposta dell'IdP non è valida perché non corrisponde alla richiesta.";
                    ViewData["ErrorMessage"] = $"RequestId: _{spidAuthnRequestId}, RequestPath: {Request.Url.ToString()}, InResponseTo: {idpAuthnResponse.InResponseTo}, Recipient: {idpAuthnResponse.SubjectConfirmationDataRecipient}.";
                    return View("Error");
                }

                // Save request and response data needed to eventually logout as a cookie
                cookie.Values["IdPName"] = IdPName;
                cookie.Values["SpidAuthnRequestId"] = spidAuthnRequestId;
                cookie.Values["SubjectNameId"] = idpAuthnResponse.SubjectNameId;
                cookie.Values["AuthnStatementSessionIndex"] = idpAuthnResponse.AuthnStatementSessionIndex;
                cookie.Expires = DateTime.Now.AddMinutes(20);
                Response.Cookies.Add(cookie);

                AppUser appUser = new AppUser
                {
                    Name = SpidUserInfoHelper.Name(idpAuthnResponse.SpidUserInfo),
                    Surname = SpidUserInfoHelper.FamilyName(idpAuthnResponse.SpidUserInfo),
                     FiscalNumber = SpidUserInfoHelper.FiscalNumber(idpAuthnResponse.SpidUserInfo),
                     Email =  SpidUserInfoHelper.Email(idpAuthnResponse.SpidUserInfo)
                };

                Session.Add("AppUser", appUser);
                ViewData["UserInfo"] = idpAuthnResponse.SpidUserInfo;
                             

            
                

                //Add To DB
               MySql.Data.MySqlClient.MySqlConnection conn;
                //string myConnectionString;
                //myConnectionString = "server=192.168.1.21;uid=root;" +
                //   "pwd=Andreani990;database=sp_spid_log";
                
                string myConnectionString = "server=" + ConfigurationManager.AppSettings["CONN_server"] +
                    "; uid=" + ConfigurationManager.AppSettings["CONN_uid"] + ";" + "pwd=" + ConfigurationManager.AppSettings["CONN_pass"] +
                    "; database=" + ConfigurationManager.AppSettings["CONN_db"];

                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
                    conn.Open();

                    //Update command
                    //AuthnRequest
                    //Response
                    //AuthnReq_ID
                    //AuthnReq_IssueInstant
                    //spidCode
                    //fiscalNumber
                    //ivaCode
                    //Data
                    //Ora
                    string mysql_query;
                    mysql_query = "INSERT INTO LOG (AuthnRequest,Response,AuthnReq_ID,AuthnReq_IssueInstant,spidCode,fiscalNumber,ivaCode,Data,Ora) " +
                                         "VALUES (@AuthnRequest,@Response,@AuthnReq_ID,@AuthnReq_IssueInstant,@spidCode,@fiscalNumber,@ivaCode,@Data,@Ora)";
                    
                   //"WHERE Inventorynumber =@numquery";
                    MySqlCommand cmd = new MySqlCommand(mysql_query, conn);

                    string stringa_1;
                    
                    stringa_1 = Encoding.UTF8.GetString(Convert.FromBase64String(Session["spidAuthnRequest"].ToString()));
                    cmd.Parameters.AddWithValue("@AuthnRequest", stringa_1);


                    stringa_1 = Encoding.UTF8.GetString(Convert.FromBase64String(formCollection["SAMLResponse"].ToString()));
                    cmd.Parameters.AddWithValue("@Response", stringa_1);
                    //Encoding.UTF8.GetString(Convert.FromBase64String(base64Response));
                    cmd.Parameters.AddWithValue("@AuthnReq_ID", spidAuthnRequestId);
                    cmd.Parameters.AddWithValue("@AuthnReq_IssueInstant", idpAuthnResponse.IssueInstant.UtcDateTime);
                    cmd.Parameters.AddWithValue("@spidCode", idpAuthnResponse.SpidUserInfo["spidCode"].ToString());
                    cmd.Parameters.AddWithValue("@fiscalNumber", SpidUserInfoHelper.FiscalNumber(idpAuthnResponse.SpidUserInfo));
                    cmd.Parameters.AddWithValue("@ivaCode", SpidUserInfoHelper.FiscalNumber(idpAuthnResponse.SpidUserInfo));
                    cmd.Parameters.AddWithValue("@Data", DateTime.Today.ToString("d"));
                    cmd.Parameters.AddWithValue("@Ora", DateTime.Now.TimeOfDay);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }


                


                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                
                }



                //Redirect
               Dictionary<string, object> postData_URL = new Dictionary<string, object>();
               string stringa_2;
               stringa_2 = Encoding.UTF8.GetString(Convert.FromBase64String(Session["spidAuthnRequest"].ToString()));
               postData_URL.Add("AuthnRequest", stringa_2);

               stringa_2 = Encoding.UTF8.GetString(Convert.FromBase64String(formCollection["SAMLResponse"].ToString()));
               postData_URL.Add("Response", stringa_2);
              
                postData_URL.Add("AuthnReq_ID", spidAuthnRequestId);
                postData_URL.Add("AuthnReq_IssueInstant", idpAuthnResponse.IssueInstant.UtcDateTime);
                postData_URL.Add("spidCode", idpAuthnResponse.SpidUserInfo["spidCode"].ToString());
                postData_URL.Add("FiscalNumber", SpidUserInfoHelper.FiscalNumber(idpAuthnResponse.SpidUserInfo));
                postData_URL.Add("ivaCode", SpidUserInfoHelper.FiscalNumber(idpAuthnResponse.SpidUserInfo));
                postData_URL.Add("Data", DateTime.Today.ToString("d"));
                postData_URL.Add("Ora", DateTime.Now.TimeOfDay);
                postData_URL.Add("Mail", SpidUserInfoHelper.Email(idpAuthnResponse.SpidUserInfo));
                
                return this.RedirectAndPost(ConfigurationManager.AppSettings["URL_RedirectAndPost"], postData_URL);
                //return View("UserData");

            }
  catch (Exception ex)
            {
                log.Error("Error on ACSController [HttpPost]Index method", ex);
                ViewData["Message"] = "Errore nella lettura della risposta ricevuta dal provider.";
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }


        [HttpPost]

        //add by VaneX
        public string XmlSerializeObject(object obj)
        {
            string xmlStr = String.Empty;
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            settings.NewLineChars = String.Empty;
            settings.NewLineHandling = NewLineHandling.None;

            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(xmlWriter, obj);
                    xmlStr = stringWriter.ToString();
                    xmlWriter.Close();
                }
            }
            return xmlStr.ToString();
        }
        // Fine VaneX ///

        public ActionResult Logout(FormCollection formCollection)
        {
            string IdPName;
            string spidLogoutRequestId;

            // Try to get auth requesta data from cookie
            HttpCookie cookie = Request.Cookies[SPID_COOKIE];
            if (cookie != null)
            {
                IdPName = cookie["IdPName"];
                spidLogoutRequestId = cookie["SpidLogoutRequestId"];
                log.Info($"Cookie {SPID_COOKIE} IdPName: {IdPName}, SpidLogoutRequestId: {spidLogoutRequestId}");
            }
            else
            {
                log.Error("Error on ACSController [HttpPost]Index method: Impossibile recuperare l'Id della sessione.");
                ViewData["Message"] = "Impossibile recuperare i dati della sessione (cookie scaduto).";
                return View("Error");
            }

            // Remove the cookie
            cookie.Values["IdPName"] = string.Empty;
            cookie.Values["SpidAuthnRequestId"] = string.Empty;
            cookie.Values["SpidLogoutRequestId"] = string.Empty;
            cookie.Values["SubjectNameId"] = string.Empty;
            cookie.Values["AuthnStatementSessionIndex"] = string.Empty;
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            // End the session
            Session["AppUser"] = null;

            try
            {
                IdpLogoutResponse idpLogoutResponse = SamlHelper.GetLogoutResponse(formCollection["SAMLResponse"].ToString());

                if (!idpLogoutResponse.IsSuccessful)
                {
                    log.Error($"Error on ACSController [HttpPost]Index method: La risposta dell'IdP riporta il seguente StatusCode: {idpLogoutResponse.StatusCodeInnerValue} con StatusMessage: {idpLogoutResponse.StatusMessage} e StatusDetail: {idpLogoutResponse.StatusDetail}.");
                    ViewData["Message"] = "La richiesta di logout è stata rifiutata.";
                    ViewData["ErrorMessage"] = $"StatusCode: {idpLogoutResponse.StatusCodeInnerValue} con StatusMessage: {idpLogoutResponse.StatusMessage} e StatusDetail: {idpLogoutResponse.StatusDetail}.";
                    return View("Error");
                }

                // Verifica la corrispondenza del valore di spidLogoutRequestId ricavato da cookie con quello restituito dalla risposta
                if (!SamlHelper.ValidLogoutResponse(idpLogoutResponse, spidLogoutRequestId))
                {
                    log.Error($"Error on ACSController [HttpPost]Index method: La risposta dell'IdP non è valida (InResponseTo != spidLogoutRequestId).");
                    ViewData["Message"] = "La risposta dell'IdP non è valida perché non corrisponde alla richiesta.";
                    ViewData["ErrorMessage"] = $"RequestId: _{spidLogoutRequestId}, RequestPath: {Request.Url.ToString()}, InResponseTo: {idpLogoutResponse.InResponseTo}.";
                    return View("Error");
                }

                return View("Logout");
            }

            catch (Exception ex)
            {
                log.Error("Error on ACSController [HttpPost]Index method", ex);
                ViewData["Message"] = "Errore nella lettura della risposta ricevuta dal provider.";
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }

    }
}