using System;
namespace BlazorECommerceApp.Client.Util
{
    public class PublicHttpClient
    {
        public HttpClient Http { get; }

        public PublicHttpClient(HttpClient httpClient)
            => Http = httpClient;

    }
}
