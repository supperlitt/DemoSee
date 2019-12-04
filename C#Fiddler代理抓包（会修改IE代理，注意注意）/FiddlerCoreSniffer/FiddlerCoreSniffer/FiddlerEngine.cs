namespace FiddlerCoreConsoleApplication
{
    using Fiddler;
    using System;
    using System.Collections.Generic;

    public class FiddlerEngine
    {
        public string HostToSniff { get; private set; }

        public FiddlerEngine(string hostToSniff)
        {
            HostToSniff = hostToSniff;
        }

        public void Start()
        {
            // 加载证书，可以对HTTPS进行监控
            if (!CertMaker.rootCertExists())
            {
                bool k = CertMaker.createRootCert();
                k = CertMaker.trustRootCert();
            }

            FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            //FiddlerApplication.Startup(8888, FiddlerCoreStartupFlags.CaptureLocalhostTraffic | FiddlerCoreStartupFlags.RegisterAsSystemProxy);
            FiddlerApplication.Startup(8888, FiddlerCoreStartupFlags.Default);
        }

        void FiddlerApplication_BeforeRequest(Session oSession)
        {
            Console.WriteLine(oSession.RequestMethod + " " + oSession.hostname);
            if (oSession.hostname.Contains("cmpay") && oSession.RequestMethod == "GET")
            {

            }
        }

        void FiddlerApplication_BeforeResponse(Session oSession)
        {
            if (oSession.hostname.Contains("cmpay"))
            {

            }
        }

        public void Stop()
        {
            FiddlerApplication.AfterSessionComplete -= HandleFiddlerSessionComplete;
            if (FiddlerApplication.IsStarted())
            {
                FiddlerApplication.Shutdown();
            }
        }

        private void HandleFiddlerSessionComplete(Session session)
        {
            // Ignore HTTPS connect requests
            if (session.RequestMethod == "CONNECT")
            {
                return;
            }

            if (session.hostname.ToLower() != this.HostToSniff)
            {
                return;
            }

            string url = session.fullUrl.ToLower();

            var extensions = new List<string> { ".ico", ".gif", ".jpg", ".png", ".axd", ".css" };
            foreach (var ext in extensions)
            {
                if (url.Contains(ext))
                {
                    return;
                }
            }

            if (session == null || session.oRequest == null || session.oRequest.headers == null)
            {
                return;
            }

            string headers = session.oRequest.headers.ToString();
            var reqBody = session.GetRequestBodyAsString();

            Console.WriteLine(headers);
            if (!string.IsNullOrEmpty(reqBody))
            {
                Console.WriteLine(string.Join(Environment.NewLine, reqBody.Split(new char[] { '&' })));
            }

            Console.WriteLine(Environment.NewLine);

            // if you wanted to capture the response
            //string respHeaders = session.oResponse.headers.ToString();
            //var respBody = session.GetResponseBodyAsString();
        }
    }
}