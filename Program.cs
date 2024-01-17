using System.Runtime.Intrinsics.X86;

namespace MVCCoreApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("/end"))
                {
                    await context.Response.WriteAsync("Middleware chain terminated.");
                }
                else
                {
                    await next();
                }
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("display"))
                {
                    await context.Response.WriteAsync("hello1");
                    await next.Invoke();
                    await context.Response.WriteAsync("hello2");
                }
                else
                {
                    await next.Invoke();
                }
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("hello"))
                {
                    await context.Response.WriteAsync("hellooooo");
                }
                else
                {
                    await next.Invoke();
                }
            });



            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}