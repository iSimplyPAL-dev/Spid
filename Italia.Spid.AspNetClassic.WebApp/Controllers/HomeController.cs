





using Italia.Spid.AspNet.WebApp.Models;
using Italia.Spid.Authentication;
using Italia.Spid.Authentication.IdP;
using Italia.Spid.Authentication.Saml;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.IO;
using System.Text;

namespace Italia.Spid.AspNet.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public class IDP_Name
        {
            public int Position { get; set; }
            public string IDPName { get; set; }
                       public string URL_Image { get; set; }
            public string Title { get; set; }
            //public List<string> Name = new List<string>();
        }

        //IDP_Name IDPLIST = new IDP_Name();

            


        private ILog log = LogManager.GetLogger(typeof(HomeController));
        private readonly string SPID_COOKIE = ConfigurationManager.AppSettings["SPID_COOKIE"];

        public ActionResult Index()
       
        {
            //05/03/20
            //https://www.tutorialsteacher.com/mvc/viewbag-in-asp.net-mvc
            //new IDP_Name(){ Position=10, IDPName="https://validator.spid.gov.it/", URL_Image = "~/Content/images/spid-agid.png", Title="SPID_Validator"  },
            IList<IDP_Name> IDPList = new List<IDP_Name>() {
                    new IDP_Name(){ Position=1, IDPName="https://loginspid.aruba.it", URL_Image = "~/Content/images/spid-idp-aruba.png" , Title="Aruba" },
                    new IDP_Name(){ Position=2, IDPName="https://spid.intesa.it", URL_Image = "~/Content/images/spid-idp-intesa.png" , Title="Intesa" },
                    new IDP_Name(){ Position=3, IDPName="https://identity.infocert.it", URL_Image = "~/Content/images/spid-idp-infocertid.png" , Title="Infocert" },
                    new IDP_Name(){ Position=4, IDPName="https://id.lepida.it/idm/app/", URL_Image = "~/Content/images/spid-idp-lepida.png", Title="Lepida" },
                    new IDP_Name(){ Position=5, IDPName="https://idp.namirialtsp.com/idp", URL_Image = "~/Content/images/spid-idp-namirial.png", Title="Namirial" },
                    new IDP_Name(){ Position=6, IDPName="https://posteid.poste.it/", URL_Image = "~/Content/images/spid-idp-posteid.png", Title="Poste Italiane" },
                    new IDP_Name(){ Position=7, IDPName="https://spid.register.it", URL_Image = "~/Content/images/spid-idp-register.png", Title="Register"  },
                    new IDP_Name(){ Position=8, IDPName="https://login.id.tim.it/affwebservices/public/saml2sso", URL_Image = "~/Content/images/spid-idp-titrust.png", Title="TI_Trust_Technologies"  },
                    new IDP_Name(){ Position=9, IDPName="https://identity.sieltecloud.it", URL_Image = "~/Content/images/spid-idp-sielteid.png", Title="Sielte"  },
                    //new IDP_Name(){ Position=10, IDPName="https://validator.spid.gov.it", URL_Image = "~/Content/images/spid-agid.png", Title="SpID Validator"  },
                    //new IDP_Name(){ Position=11, IDPName="https://idptest.spid.gov.it", URL_Image = "~/Content/images/spid-agid.png", Title="TEST"  },
               };

            //metodo di ordinamento

            List<IDP_Name> IDPListRandom = new List<IDP_Name>()
            {
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                //new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
                //new IDP_Name(){ Position=0, IDPName="", URL_Image = "" , Title="Vuoto" },
            };

            int numeroCasuale=0;

            int indiceMax = 9;
            int min = 0;
            int max = 9;


            Random random = new Random();
            int[] numeri_usati = new int[9];

            //il find con lo 0 da errore
            
            for (int i = 0; i < indiceMax; i++)
            {
                if (i==0)
                     {
                    numeroCasuale = random.Next(min,max);
                     }
                else
                  {
                    do {                            
                          numeroCasuale = random.Next(min, max);
                        if (numeroCasuale == 0)
                        {
                            numeroCasuale = 666;
                        }
                        }
                      while ((Array.Find(numeri_usati, item => item == numeroCasuale)>0));
                   }

                numeri_usati[i] = numeroCasuale;

                if (numeroCasuale == 666)
                {
                    numeroCasuale = 0;
                }
 
                    IDPListRandom[i] = IDPList[numeroCasuale];
          

            }


            List<int> numeri_mancanti = new List<int>();
            //popolato da 0 a 7 i primi 8 valori
            for (int i =0; i < 9; i++)
            {
                if (i==0)
                {
                    i = 666;
                }

               if (Array.Find(numeri_usati, item => item == i)==0)
                          {
                    numeri_mancanti.Add(i);
                }

                if (i == 666)
                {
                    i = 0;
                }


            }

            var j = 9;        
            for (int i = 0; i <= numeri_mancanti.Count-1; i++)
            {
                IDPListRandom[j] = IDPList[numeri_mancanti[i]];
                j = j + 1;
            }
                       
                ViewBag.IDPLIst = IDPListRandom;


            if (Session["AppUser"] != null)
            {
                ViewBag.Name = ((AppUser)Session["AppUser"]).Name;
                ViewBag.Surname = ((AppUser)Session["AppUser"]).Surname;
                ViewBag.Logged = true;
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult SpidRequest(string idpName)
        {

            //Test connessione
            //       < add key = "CONN_server" value = "192.168.1.21" />
            // < add key = "CONN_uid" value = "root" />
            //< add key = "CONN_pass" value = "Andreani990" />
            //    < add key = "CONN_db" value = "sp_spid_log" />

            //myConnectionString = "server=192.168.1.21;uid=root;" +
            // "pwd=Andreani990;database=sp_spid_log";
            //string myConnectionString = "server=" + ConfigurationManager.AppSettings["CONN_server"] +
            //    "; uid=" + ConfigurationManager.AppSettings["CONN_uid"] + ";" + "pwd=" + ConfigurationManager.AppSettings["CONN_pass"] +
            //    "; database=" + ConfigurationManager.AppSettings["CONN_db"] ;
            //MySql.Data.MySqlClient.MySqlConnection conn;
            //conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);
            //conn.Open();



            try
            {
                // Create the SPID request id
                string spidAuthnRequestId = Guid.NewGuid().ToString();

                // Select the Identity Provider
                IdentityProvider idp = IdentityProvidersList.GetIdpFromIdPName(idpName);

                // Retrieve the signing certificate
                //var certificate = X509Helper.GetCertificateFromStore(
                //                        StoreLocation.LocalMachine, StoreName.My,
                //                        X509FindType.FindBySubjectName,
                //    ConfigurationManager.AppSettings["SPID_CERTIFICATE_NAME"],
                //    validOnly: false);

                //var certificate = X509Helper.GetCertificateFromStore(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName , "AT", validOnly: false);


                //var certificate = X509Helper.GetCertificateFromStore(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, "Andreani Tributi S.r.l.", validOnly: false);


                //Di Giulio suggerisce installare cert su atorità attendibili
             
               // var certificate = X509Helper.GetCertificateFromStore(StoreLocation.LocalMachine, StoreName.CertificateAuthority, X509FindType.FindBySubjectName, "Andreani Tributi S.r.l.", validOnly: false);

                 //certificate = X509Helper.GetCertificateFromStore(StoreLocation.LocalMachine, StoreName.TrustedPublisher, X509FindType.FindBySubjectName, "Andreani Tributi S.r.l.", validOnly: false);
      
               // var certificate = X509Helper.GetCertificateFromStore(StoreLocation.LocalMachine, StoreName.Root, X509FindType.FindBySubjectName, "Andreani Tributi S.r.l.", validOnly: false);
                
               //var certificate = X509Helper.GetCertificateFromStore(StoreLocation.LocalMachine, StoreName.Root, X509FindType.FindBySubjectName, ConfigurationManager.AppSettings["SPID_CERTIFICATE_NAME"], validOnly: false);
                    var certificate = X509Helper.GetCertificateFromStore(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, ConfigurationManager.AppSettings["SPID_CERTIFICATE_NAME"], validOnly: false);

                // Create the signed SAML request
                var spidAuthnRequest = SamlHelper.BuildAuthnPostRequest(
                    uuid: spidAuthnRequestId,
                    destination: idp.EntityID,
                    consumerServiceURL: ConfigurationManager.AppSettings["SPID_DOMAIN_VALUE"],
                    securityLevel: 1,
                    certificate: certificate,
                    identityProvider: idp,
                    enviroment: ConfigurationManager.AppSettings["ENVIROMENT"] == "dev" ? 1 : 0);

                ViewData["data"] = spidAuthnRequest;
                ViewData["action"] = idp.SingleSignOnServiceUrl;

                // Save the IdP label and SPID request id as a cookie
                HttpCookie cookie = Request.Cookies.Get(SPID_COOKIE) ?? new HttpCookie(SPID_COOKIE);
                cookie.Values["IdPName"] = idpName;
                cookie.Values["SpidAuthnRequestId"] = spidAuthnRequestId;
                cookie.Expires = DateTime.Now.AddMinutes(20);
                Response.Cookies.Add(cookie);

                Session["spidAuthnRequest"] = spidAuthnRequest;
                var stringa_2 = Encoding.UTF8.GetString(Convert.FromBase64String(Session["spidAuthnRequest"].ToString()));
                
                // Send the request to the Identity Provider
                return View("PostData");
            }
            catch (Exception ex)
            {
                log.Error("Error on HomeController SpidRequest", ex);
                ViewData["Message"] = "Errore nella preparazione della richiesta di autenticazione da inviare al provider.";
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }

        public ActionResult LogoutRequest()
        {
            string idpName;
            string subjectNameId;
            string authnStatementSessionIndex;

            // Try to get Authentication data from cookie
            HttpCookie cookie = Request.Cookies[SPID_COOKIE];

            if (cookie == null)
            {
                // End the session
                Session["AppUser"] = null;

                log.Error("Error on HomeController LogoutRequest method: Impossibile recuperare i dati della sessione (cookie scaduto)");
                ViewData["Message"] = "Impossibile recuperare i dati della sessione (cookie scaduto).";
                return View("Error");
            }

            idpName = cookie["IdPName"];
            subjectNameId = cookie["SubjectNameId"];
            authnStatementSessionIndex = cookie["AuthnStatementSessionIndex"];

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

            if (string.IsNullOrWhiteSpace(idpName) ||
                string.IsNullOrWhiteSpace(subjectNameId) ||
                string.IsNullOrWhiteSpace(authnStatementSessionIndex))
            {
                log.Error("Error on HomeController LogoutRequest method: Impossibile recuperare i dati della sessione (il cookie non contiene tutti i dati necessari)");
                ViewData["Message"] = "Impossibile recuperare i dati della sessione (il cookie non contiene tutti i dati necessari).";
                return View("Error");
            }

            try
            {
                // Create the SPID request id and save it as a cookie
                string logoutRequestId = Guid.NewGuid().ToString();

                // Select the Identity Provider
                IdentityProvider idp = IdentityProvidersList.GetIdpFromIdPName(idpName);

                // Retrieve the signing certificate
               // var certificate = X509Helper.GetCertificateFromStore(
                //    StoreLocation.LocalMachine, StoreName.Root ,
                //    X509FindType.FindBySubjectName,
                 //   ConfigurationManager.AppSettings["SPID_CERTIFICATE_NAME"],
                  //  validOnly: false);

                var certificate = X509Helper.GetCertificateFromStore(
                                  StoreLocation.LocalMachine, StoreName.Root, 
                                  X509FindType.FindBySubjectName, 
                                  ConfigurationManager.AppSettings["SPID_CERTIFICATE_NAME"], validOnly: false);



                // Create the signed SAML logout request
                var spidLogoutRequest = SamlHelper.BuildLogoutPostRequest(
                    uuid: logoutRequestId,
                    consumerServiceURL: ConfigurationManager.AppSettings["SPID_DOMAIN_VALUE"],
                    certificate: certificate,
                    identityProvider: idp,
                    subjectNameId: subjectNameId,
                    authnStatementSessionIndex: authnStatementSessionIndex);

                ViewData["data"] = spidLogoutRequest;
                ViewData["action"] = idp.SingleLogoutServiceUrl;

                // Save the IdP label and SPID request id as a cookie
                cookie = new HttpCookie(SPID_COOKIE);
                cookie.Values["IdPName"] = idpName;
                cookie.Values["SpidLogoutRequestId"] = logoutRequestId;
                cookie.Expires = DateTime.Now.AddMinutes(20);
                Response.Cookies.Add(cookie);

                // Send the request to the Identity Provider
                return View("PostData");
            }
            catch (Exception ex)
            {
                log.Error("Error on HomeController SpidRequest", ex);
                ViewData["Message"] = "Errore nella preparazione della richiesta di logout da inviare al provider.";
                ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }

    }
}