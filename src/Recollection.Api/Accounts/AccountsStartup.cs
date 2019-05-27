﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollection.Accounts
{
    public class AccountsStartup
    {
        private readonly IConfiguration configuration;

        public AccountsStartup(IConfiguration configuration) => this.configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            services
                .AddTransient<JwtSecurityTokenHandler>()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    JwtOptions configuration = this.configuration.GetSection("Jwt").Get<JwtOptions>();

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.Issuer,
                        ValidAudience = configuration.Issuer,
                        IssuerSigningKey = configuration.GetSecurityKey()
                    };

                    options.SaveToken = true;
                });

            services
                .AddAuthorization(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();
                });
        }

        public void ConfigureAuthentication(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
        }
    }
}