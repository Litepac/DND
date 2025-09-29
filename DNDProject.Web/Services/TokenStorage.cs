using Microsoft.JSInterop;

namespace DNDProject.Web.Services;

public class TokenStorage
{
    private readonly IJSRuntime _js;
    private const string Key = "authToken";

    public TokenStorage(IJSRuntime js) => _js = js;

    public ValueTask SaveAsync(string token) =>
        _js.InvokeVoidAsync("localStorage.setItem", Key, token);

    public ValueTask<string?> GetAsync() =>
        _js.InvokeAsync<string?>("localStorage.getItem", Key);

    public ValueTask ClearAsync() =>
        _js.InvokeVoidAsync("localStorage.removeItem", Key);
}
