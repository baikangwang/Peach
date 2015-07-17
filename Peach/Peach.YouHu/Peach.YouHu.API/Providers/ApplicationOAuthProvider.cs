using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Peah.YouHu.API.Models;

namespace Peah.YouHu.API.Providers
{
    using System.Text.RegularExpressions;

    using Microsoft.Owin;

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string role = this.GetRole(context.Request.Path.Value);

            if (string.IsNullOrEmpty(role))
                return;

            ClaimsIdentity oAuthIdentity;
            ClaimsIdentity cookiesIdentity;
            AuthenticationProperties properties;
            if (Regex.IsMatch(role, "owner"))
            {
                var userManager = context.OwinContext.GetUserManager<OwnerManager>();
                Owner user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                oAuthIdentity = await user.GenerateUserIdentityAsync(
                    userManager,
                    OAuthDefaults.AuthenticationType);
                cookiesIdentity = await user.GenerateUserIdentityAsync(
                    userManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                properties = CreateProperties(user.UserName);
            }
            else
            {
                var userManager = context.OwinContext.GetUserManager<DriverManager>();
                Driver user = await userManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,OAuthDefaults.AuthenticationType);
                cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,CookieAuthenticationDefaults.AuthenticationType);

                properties = CreateProperties(user.UserName);
            }

            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        private string GetRole(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;

            return string.Empty;
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}