using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using Moq;
using System.Web.Routing;
using System.Security.Principal;

namespace MyStory.Tests.FunctionalTests
{
    public static class MvcMockHelpers
    {
        public static bool _IsAuthenticated { get; set; }
        public static bool _IsAjaxRequest { get; set; }

        public static HttpContextBase FakeHttpContext()
		{
            var context = new Mock<HttpContextBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();

            request.Setup(req => req.ApplicationPath).Returns("~/");
            request.Setup(req => req.AppRelativeCurrentExecutionFilePath).Returns("~/");
            request.Setup(req => req.PathInfo).Returns(string.Empty);
            request.Setup(req => req.Form).Returns(new NameValueCollection());
            request.Setup(req => req.QueryString).Returns(new NameValueCollection());
            request.Setup(req => req.Files).Returns(files.Object);
            request.Setup(req => req.IsAuthenticated).Returns(_IsAuthenticated);
            if(_IsAjaxRequest)
                request.Setup(req => req["X-Requested-With"]).Returns("XMLHttpRequest");

            response.Setup(res => res.ApplyAppPathModifier(It.IsAny<string>())).
                Returns((string virtualPath) => virtualPath);
            
            user.Setup(usr => usr.Identity).Returns(identity.Object);
            user.Setup(usr => usr.Identity.Name).Returns("a@a.com");
            
            identity.Setup(ident => ident.IsAuthenticated).Returns(true);

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.User).Returns(user.Object);

            return context.Object;
		}

		public static HttpContextBase FakeHttpContext(string url)
		{
			HttpContextBase context = FakeHttpContext();
			context.Request.SetupRequestUrl(url);
			return context;
		} 

		public static void SetFakeControllerContext(this Controller controller,
                                                    bool isAuthenticated = true,
                                                    bool isAjaxRequest = false)
		{
            _IsAuthenticated = isAuthenticated;
            _IsAjaxRequest = isAjaxRequest;

			var httpContext = FakeHttpContext();
			
            ControllerContext context = new ControllerContext(
                new RequestContext(httpContext, new RouteData()),
                controller);

			controller.ControllerContext = context;
		}

		static string GetUrlFileName(string url)
		{
			if (url.Contains("?"))
				return url.Substring(0, url.IndexOf("?"));
			else
				return url;
		}

		static NameValueCollection GetQueryStringParameters(string url)
		{
			if (url.Contains("?"))
			{
				NameValueCollection parameters = new NameValueCollection();

				string[] parts = url.Split("?".ToCharArray());
				string[] keys = parts[1].Split("&".ToCharArray());

				foreach (string key in keys)
				{
					string[] part = key.Split("=".ToCharArray());
					parameters.Add(part[0], part[1]);
				}

				return parameters;
			}
			else
			{
				return null;
			}
		}

		public static void SetHttpMethodResult(this HttpRequestBase request, string httpMethod)
		{
			Mock.Get(request)
				.Setup(req => req.HttpMethod)
				.Returns(httpMethod);
		}

		public static void SetupRequestUrl(this HttpRequestBase request, string url)
		{
			if (url == null)
				throw new ArgumentNullException("url");

			if (!url.StartsWith("~/"))
				throw new ArgumentException("Sorry, we expect a virtual url starting with \"~/\".");

			var mock = Mock.Get(request);

            mock.Setup(req => req.QueryString)
				.Returns(GetQueryStringParameters(url));
            mock.Setup(req => req.AppRelativeCurrentExecutionFilePath)
				.Returns(GetUrlFileName(url));
            mock.Setup(req => req.PathInfo)
				.Returns(string.Empty);
		}
	}
}
