using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Login.Demo.Controllers;
using Login.Demo.Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            // services.AddScoped<ITest, Test>();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/user/login";

                });
            services.AddControllersWithViews();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder
                .RegisterType<CallLogger>()
                .AsSelf()
                .InstancePerLifetimeScope();

            //ע����ַ��� autofac
            builder
                .RegisterType<Test>()
                .As<ITest>()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CallLogger))
                .InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            //��¼��֤
            app.UseAuthentication();
            //��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public class CallLogger : IInterceptor
    {


        public CallLogger()
        {

        }

        public void Intercept(IInvocation invocation)
        {
            //AOP �������� ���
            //���Ըɺܻ�
            #region AOP�༭
            if (invocation.Arguments[0] is string && invocation.Method.Name == "GetName")
            {
                invocation.Arguments[0] += "�����Ǿ���Aop��";
            } 
            #endregion
            invocation.Proceed();
            //����ִ��֮�����ǻ����Ը������ܶණ��
            //ȫ�ֵ�ͨ�õķ���


        }
    }
}
