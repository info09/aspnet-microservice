using HealthChecks.UI.Client;

namespace Product.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("tedu-microservice_swagger");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
                c.DisplayRequestDuration();
            });

            app.UseAuthentication();

            app.UseRouting();
            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
