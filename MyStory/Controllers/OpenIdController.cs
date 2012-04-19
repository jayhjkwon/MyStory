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
using MyStory.Infrastructure.Common;
using MyStory.Models;

namespace MyStory.Controllers
{
    public class OpenIdController : MyStoryController
    {
        private static readonly OpenIdRelyingParty Openid = new OpenIdRelyingParty();

        [ValidateInput(false)]
        public ActionResult Authenticate(string returnUrl)
        {
            var response = Openid.GetResponse();
            if (response == null)
            {
                Identifier id;
                //make sure users openid_identifier is valid.
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    try
                    {
                        var uri = new UriBuilder(Url.RouteUrl("default", null, "http"))
                        {
                            Query = string.Format("returnUrl={0}", Uri.EscapeUriString(returnUrl))
                        };
                        IAuthenticationRequest request = Openid.CreateRequest(Request.Form["openid_identifier"], Realm.AutoDetect, uri.Uri);

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

                        string name=null, email=null, url=null;
                        
                        var claims = response.GetExtension<ClaimsResponse>();
                        if (claims != null)
                        {
                            name = claims.FullName;
                            email = claims.Email;
                        }
                        
                        var fetch = response.GetExtension<FetchResponse>();
                        if (fetch != null)
                        {
                            name = fetch.GetAttributeValue(WellKnownAttributes.Name.FullName) ??
                                string.Format("{0} {1}", fetch.GetAttributeValue(WellKnownAttributes.Name.First), fetch.GetAttributeValue(WellKnownAttributes.Name.Last));
                            email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                            url = fetch.GetAttributeValue(WellKnownAttributes.Contact.Web.Homepage) ?? fetch.GetAttributeValue(WellKnownAttributes.Contact.Web.Blog);
                        }
                        
                        CommenterCookieManager.SetCommenterCookieValue(Response, email);
                        var commenter = DbContext.Commenters.SingleOrDefault(c => c.Email == email);
                        if (commenter == null)
                        {
                            DbContext.Commenters.Add(new Commenter
                            {
                                OpenId = response.ClaimedIdentifier.ToString(),
                                Email = email,
                                Name = name,
                                Url = url
                            });
                        }
                        else
                        {
                            commenter.OpenId = commenter.OpenId ?? response.ClaimedIdentifier.ToString();
                        }
                        DbContext.SaveChanges();

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
