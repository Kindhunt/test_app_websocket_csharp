namespace WebSocketServer
{
    internal class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseWebSockets(); 

            app.Map("/ws", wsApp =>
            {
                wsApp.Use(async (context, next) =>
                {
                    if (!context.WebSockets.IsWebSocketRequest)
                    {
                        context.Response.StatusCode = 400;
                    }
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var webSocketHandler = new WebSocketHandler();
                    await webSocketHandler.HandleWebSocket(webSocket);
                    await next();
                });
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/swagger", async context =>
                {
                    await context.Response.WriteAsync("Hello, WebSocket Server!");
                });
            });
        }

    }
}
