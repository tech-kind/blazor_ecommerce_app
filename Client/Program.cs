using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorECommerceApp.Client;
using BlazorECommerceApp.Client.Util;
using Blazored.LocalStorage;
using BlazorECommerceApp.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("BlazorECommerceApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddHttpClient<PublicHttpClient>("BlazorECommerceApp.AnonymousAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazorECommerceApp.ServerAPI"));

builder.Services.AddServices();

// Azure AD B2C���g���ݒ���s��
builder.Services.AddMsalAuthentication(options =>
{
    // AzureAdB2C�̃L�[��wwwroot�t�H���_�ɂ���appsettings.json�̓��e��ǂݍ���ł���
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://techkind.onmicrosoft.com/6636c585-2c2c-42be-95b1-7a00f99f8365/API.Access");
    // ���_�C���N�g�Ƀ��O�C����ʂ�\�����������ꍇ�́A���L��L���ɂ���B
    // options.ProviderOptions.LoginMode = "redirect";
});

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
