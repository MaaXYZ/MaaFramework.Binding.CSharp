using System.Net;
using System.Text.Json;

using MaaCommon.Interop;

namespace MaaCommon.Server
{
    public class HttpService
    {
        HttpListener server = new HttpListener();
        bool exit = false;

        public HttpService(ushort port = 8080, string local = "127.0.0.1")
        {
            server.Prefixes.Add($"http://{local}:{port}/");
        }

        public void listen()
        {
            server.Start();
            if (!server.IsListening)
            {
                return ;
            }

            while (!exit)
            {
                var context = server.GetContext();
                ThreadPool.QueueUserWorkItem(process, context);
            }
            server.Close();
        }

        private void process(object? o)
        {
            HttpListenerContext? context = o as HttpListenerContext;
            if (context == null)
            {
                return;
            }
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            var buf = System.Text.Encoding.UTF8.GetBytes(process(request));
            response.ContentType = "application/json";
            response.OutputStream.Write(buf, 0, buf.Length);
            response.OutputStream.Close();
        }

        private string process(HttpListenerRequest request)
        {
            if (request.HttpMethod != "POST")
            {
                return """{ "success": false, "error": "Only POST is supported"}""";
            }
            string body = "";
            if (request.HasEntityBody)
            {
                var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                body = reader.ReadToEnd();
            }
            switch (request.Url?.AbsolutePath ?? "")
            {
                default:
                    return """{ "success": false, "error": "Path unknown"}""";
            }
        }
    }
}

