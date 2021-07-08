using Italia.Spid.Authentication.IdP;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Italia.Spid.AspNet.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CreateConfigDataListJsonFile();
            InitializeIdentityProviderList().GetAwaiter().GetResult();            
        }

        private async Task InitializeIdentityProviderList()
        {
            // Get the IdPs metadata
            List<IdentityProviderMetaData> idpMetadataList = null;
            string idpMetadataListUrl = ConfigurationManager.AppSettings["IDP_METADATA_LIST_URL"];
            if (!string.IsNullOrWhiteSpace(idpMetadataListUrl))
            {
                idpMetadataList = await IdentityProvidersList.GetIdpMetaDataListAsync(idpMetadataListUrl);
            }

            // Get the IdPs configuration data 
            List<IdentityProviderConfigData> idpConfigDataList = null;
            using (StreamReader sr = new StreamReader(Server.MapPath("~/idpConfigDataList.json")))
            {
                idpConfigDataList = JsonConvert.DeserializeObject<List<IdentityProviderConfigData>>(sr.ReadToEnd());
            }

            // Initialize the IdP list
            IdentityProvidersList.IdentityProvidersListFactory(idpMetadataList, idpConfigDataList);
        }

        private void CreateConfigDataListJsonFile()
        {
            List<IdentityProviderConfigData> idpConfigDataList = new List<IdentityProviderConfigData>
            {
                 new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_SPIDValidatorTest,
                    OrganizationName = "Local",
                    OrganizationDisplayName = "Local",
                    OrganizationUrl =ConstWrapper.IDP_SPIDValidatorTest,
                    SingleSignOnServiceUrl =ConstWrapper.IDP_SPIDValidatorTest+"/sso",
                    SingleLogoutServiceUrl = ConstWrapper.IDP_SPIDValidatorTest+"/slo",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_SpIDValidator,
                    OrganizationName = "Local",
                    OrganizationDisplayName = "Local",
                    OrganizationUrl =ConstWrapper.IDP_SpIDValidator,
                    SingleSignOnServiceUrl =ConstWrapper.IDP_SpIDValidator+"/sso",
                    SingleLogoutServiceUrl = ConstWrapper.IDP_SpIDValidator+"/slo",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                   new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_Namirial,
                    OrganizationName = "NamirialTest",
                    OrganizationDisplayName = "NamirialTest",
                    OrganizationUrl = ConstWrapper.IDP_Namirial,
                    SingleSignOnServiceUrl ="https://idp.test.namirialtsp.com/idp/profile/SAML2/POST/SSO",
                    SingleLogoutServiceUrl = "https://idp.test.namirialtsp.com/idp/profile/SAML2/POST/SLO",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                  new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_Lepida,
                    OrganizationName = "Poste Italiane SpA IDP DI TEST",
                    OrganizationDisplayName = "Poste Italiane SpA IDP DI TEST",
                    OrganizationUrl = ConstWrapper.IDP_Lepida,
                    SingleSignOnServiceUrl ="https://id.lepida.it/idp/profile/SAML2/POST/SSO",
                    SingleLogoutServiceUrl = "https://id.lepida.it/idp/profile/SAML2/POST/SLO",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_PosteItaliane,
                    OrganizationName = "Poste Italiane SpA IDP DI TEST",
                    OrganizationDisplayName = "Poste Italiane SpA IDP DI TEST",
                    OrganizationUrl = ConstWrapper.IDP_PosteItaliane,
                    SingleSignOnServiceUrl ="https://posteid.poste.it//jod-fs/ssoservicepost",
                    SingleLogoutServiceUrl = "https://posteid.poste.it//jod-fs/sloservicepost",
                    SubjectNameIdRemoveText = "SPID-", // We need to remove it from Subject Name ID otherwise subsequent logout will fail
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                   new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_SpIDValidator,
                    OrganizationName = "AgID",
                    OrganizationDisplayName = "SpID Validator",
                    OrganizationUrl= "https://www.agid.gov.it/",
                    SingleSignOnServiceUrl= "https://validator.spid.gov.it/samlsso",
                    SingleLogoutServiceUrl= "https://validator.spid.gov.it/samlsso",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta= 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_Aruba,
                    OrganizationName = "ArubaPEC S.p.A.",
                    OrganizationDisplayName = "ArubaPEC S.p.A.",
                    OrganizationUrl = "https://www.pec.it/",
                    SingleSignOnServiceUrl ="https://loginspid.aruba.it/ServiceLoginWelcome",
                    SingleLogoutServiceUrl = "https://loginspid.aruba.it/ServiceLogoutRequest",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_Intesa,
                    OrganizationName = "IN.TE.S.A. S.p.A.",
                    OrganizationDisplayName = "Intesa S.p.A.",
                    OrganizationUrl = "https://www.intesa.it/",
                    SingleSignOnServiceUrl ="https://spid.intesa.it/Time4UserServices/services/idp/AuthnRequest/",
                    SingleLogoutServiceUrl = "https://spid.intesa.it/Time4UserServices/services/idp/SingleLogout",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_InfoCert,
                    OrganizationName = "InfoCert S.p.A.",
                    OrganizationDisplayName = "InfoCert S.p.A.",
                    OrganizationUrl = "https://www.infocert.it",
                    SingleSignOnServiceUrl ="https://identity.infocert.it/spid/samlsso",
                    SingleLogoutServiceUrl = "https://identity.infocert.it/spid/samlslo",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_Namirial,
                    OrganizationName = "Namirial",
                    OrganizationDisplayName = "Namirial S.p.a. Trust Service Provider",
                    OrganizationUrl = "https://www.namirialtsp.com",
                    SingleSignOnServiceUrl ="https://idp.namirialtsp.com/idp/profile/SAML2/POST/SSO",
                    SingleLogoutServiceUrl = "https://idp.namirialtsp.com/idp/profile/SAML2/POST/SLO",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_PosteItaliane,
                    OrganizationName = "Poste Italiane SpA",
                    OrganizationDisplayName = "Poste Italiane SpA",
                    OrganizationUrl = "https://www.poste.it",
                    SingleSignOnServiceUrl ="https://posteid.poste.it/jod-fs/ssoservicepost",
                    SingleLogoutServiceUrl = "https://posteid.poste.it/jod-fs/sloserviceresponsepost",
                    SubjectNameIdRemoveText = "SPID-", // We need to remove it from Subject Name ID otherwise subsequent logout will fail
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_RegisterIT,
                    OrganizationName = "Register.it S.p.A.",
                    OrganizationDisplayName = "Register.it S.p.A.",
                    OrganizationUrl = "https//www.register.it",
                    SingleSignOnServiceUrl ="https://spid.register.it/login/sso",
                    SingleLogoutServiceUrl = "https://spid.register.it/login/singleLogout",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_Sielte,
                    OrganizationName = "Sielte S.p.A.",
                    OrganizationDisplayName = "Sielte S.p.A.",
                    OrganizationUrl = "http://www.sielte.it",
                    SingleSignOnServiceUrl ="https://identity.sieltecloud.it/simplesaml/saml2/idp/SSO.php",
                    SingleLogoutServiceUrl = "https://identity.sieltecloud.it/simplesaml/saml2/idp/SLS.php",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                    NowDelta = -2 // TODO: Only for Sielte, still valid and required?
                },
                new IdentityProviderConfigData()
                {
                    EntityId = ConstWrapper.IDP_TrustTechnologies,
                    OrganizationName = "TI Trust Technologies srl",
                    OrganizationDisplayName = "Trust Technologies srl",
                    OrganizationUrl = "https://www.trusttechnologies.it",
                    SingleSignOnServiceUrl =ConstWrapper.IDP_TrustTechnologies,
                    SingleLogoutServiceUrl = "https://login.id.tim.it/affwebservices/public/saml2slo",
                    SubjectNameIdRemoveText = string.Empty,
                    DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'",
                    NowDelta = 0
                }
            };

            using (StreamWriter sw = new StreamWriter(Server.MapPath("~/idpConfigDataList.json")))
            {
                sw.Write(JsonConvert.SerializeObject(idpConfigDataList));
                sw.Close();
            }
        }
    }
}
