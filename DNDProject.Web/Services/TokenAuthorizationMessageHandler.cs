using System.Net.Http;
using System.Net.Http.Headers;

namespace DNDProject.Web.Services;

public sealed class TokenAuthorizationMessageHandler : DelegatingHandler
{
    private readonly ITokenStorage _tokens;

    public TokenAuthorizationMessageHandler(ITokenStorage tokens) => _tokens = tokens;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokens.GetAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
