using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;

namespace MyStory.Controllers
{
    public class OpenIdController : Controller
    {
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.RouteUrl("default");

            var response = openid.GetResponse();
            if (response == null)
            {
                Identifier id;
                //make sure your users openid_identifier is valid.
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    try
                    {
                        IAuthenticationRequest request = openid.CreateRequest(Request.Form["openid_identifier"], Realm.AutoDetect, new Uri(returnUrl));

                        // google openid
                        var fetch = new FetchRequest();
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.FullName);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Web.Blog);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Web.Homepage);
                        request.AddExtension(fetch);

                        // other openid
                        var claims = new ClaimsRequest();
                        claims.FullName = DemandLevel.Require;
                        claims.Email = DemandLevel.Require;
                        request.AddExtension(claims);

                        //request openid_identifier
                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        // TODO handle error message in view
                        TempData["statusMessage"] = ex.Message;
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    // TODO handle error message in view
                    TempData["statusMessage"] = "Invalid identifier";
                    return Redirect(returnUrl);
                }
            }
            else
            {
                //check the response status
                switch (response.Status)
                {
                    //success status
                    case AuthenticationStatus.Authenticated:

                        string name, email, url = null;
                        var fetch = response.GetExtension<FetchResponse>();
                        if (fetch != null)
                        {
                            name = fetch.GetAttributeValue(WellKnownAttributes.Name.FullName) ??
                                string.Format("{0} {1}", fetch.GetAttributeValue(WellKnownAttributes.Name.First), fetch.GetAttributeValue(WellKnownAttributes.Name.Last));
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                            url = fetch.GetAttributeValue(WellKnownAttributes.Contact.Web.Homepage) ?? fetch.GetAttributeValue(WellKnownAttributes.Contact.Web.Blog);
                        }

                        var claims = response.GetExtension<ClaimsResponse>();
                        if (claims != null)
                        {
                            name = claims.FullName;
                            email = claims.Email;
                        }
                        
                        //FormsAuthentication.SetAuthCookie(response.ClaimedIdentifier.ToString(), false);
                        Response.Cookies.Add(new HttpCookie("commenter", response.ClaimedIdentifier.ToString()));

                        //TODO: response.ClaimedIdentifier, to login or create new account 

                        return Redirect(returnUrl);

                    case AuthenticationStatus.Canceled:
                        // TODO handle error message in view
                        TempData["statusMessage"] = "Canceled at provider";
                        return Redirect(returnUrl);

                    case AuthenticationStatus.Failed:
                        // TODO handle error message in view
                        TempData["statusMessage"] = response.Exception.Message;
                        return Redirect(returnUrl);
                }
            }
            return new EmptyResult();
        }

    }
}
