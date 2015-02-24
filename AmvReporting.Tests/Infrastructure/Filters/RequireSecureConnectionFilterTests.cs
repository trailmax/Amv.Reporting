using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AmvReporting.Infrastructure.Filters;
using NSubstitute;
using Xunit;


namespace AmvReporting.Tests.Infrastructure.Filters
{
    public class RequireSecureConnectionFilterTests
    {
        private readonly HttpRequestBase request;
        private readonly AuthorizationContext filterContext;

        public RequireSecureConnectionFilterTests()
        {
           
            var @params = new NameValueCollection();
            var responseHeaders = new NameValueCollection();

            request = Substitute.For<HttpRequestBase>();
            request.Params.Returns(@params);

            var response = Substitute.For<HttpResponseBase>();
            response.Headers.Returns(responseHeaders);

            var context = Substitute.For<HttpContextBase>();
            context.Request.Returns(request);
            context.Response.Returns(response);

            var controller = Substitute.For<ControllerBase>();

            var actionDescriptor = Substitute.For<ActionDescriptor>();
            var controllerContext = new ControllerContext(context, new RouteData(), controller);
            filterContext = new AuthorizationContext(controllerContext, actionDescriptor);
        }


        [Fact]
        public void OnAuthorisation_NoContext_ThrowsException()
        {
            var sut = new RequireSecureConnectionFilter();
            Assert.Throws<ArgumentNullException>(() => sut.OnAuthorization(null));
        }


        [Fact]
        public void OnAuthorisation_LocalRequest_RequestNotRedirected()
        {
            //Arrange
            request.IsLocal.Returns(true);
            var sut = new RequireSecureConnectionFilter();

            // Act
            sut.OnAuthorization(filterContext);

            // Assert - checking if we are not being redirected
            var redirectResult = filterContext.Result as RedirectResult;
            Assert.Null(redirectResult);
        }


        [Fact]
        public void OnAuthorisation_NonLocalRequest_RedirectedToHttps()
        {
            //Arrange
            request.IsLocal.Returns(false);
            var sut = new RequireSecureConnectionFilter();

            // Act && Assert 
            // here we check if controll is passed down to RequireHttpsAttribute code
            // and we are not testing for Microsoft code.
            Assert.Throws<InvalidOperationException>(() => sut.OnAuthorization(filterContext));
        }


        [Fact]
        public void SecureConnection_Adds_STSHeader()
        {
            //Arrange
            request.IsLocal.Returns(false);
            request.IsSecureConnection.Returns(true);
            request.HttpMethod.Returns("GET");
            var sut = new RequireSecureConnectionFilter();

            // Act && Assert 
            // here we check if controll is passed down to RequireHttpsAttribute code
            // and we are not testing for Microsoft code.
            sut.OnAuthorization(filterContext);

            var stsHeader = filterContext.HttpContext.Response.Headers.Get("Strict-Transport-Security");
            Assert.NotNull(stsHeader);
            Assert.Equal(stsHeader, "max-age=31536000");
        }
    }

}
