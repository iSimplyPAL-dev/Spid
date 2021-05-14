using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Italia.Spid.AspNet.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
    /// <summary>
    /// Classe per la gestione delle variabili da config
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class ConstWrapper
    {
        public static string IDP_SPIDValidatorTest
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_SPIDValidatorTest"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_SPIDValidatorTest"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_Namirial
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_Namirial"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_Namirial"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_Lepida
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_Lepida"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_Lepida"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_PosteItaliane
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_PosteItaliane"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_PosteItaliane"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_SpIDValidator
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_SPIDValidator"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_SPIDValidator"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_Aruba
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_Aruba"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_Aruba"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_Intesa
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_Intesa"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_Intesa"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_InfoCert
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_InfoCert"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_InfoCert"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_RegisterIT
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_RegisterIT"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_RegisterIT"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_Sielte
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_Sielte"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_Sielte"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
        public static string IDP_TrustTechnologies
        {
            get
            {
                try
                {
                    if (ConfigurationManager.AppSettings["IDP_TrustTechnologies"] != null)
                    {
                        return ConfigurationManager.AppSettings["IDP_TrustTechnologies"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception Err)
                {
                    throw Err;
                }
            }
        }
    }
}
