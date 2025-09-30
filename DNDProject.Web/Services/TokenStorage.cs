using Microsoft.JSInterop;

namespace DNDProject.Web.Services;

public interface ITokenStorage
{
    Task SaveAsync(string token);
    Task<string?> GetAsync();
    Task ClearAsync();
}

public sealed class TokenStorage : ITokenStorage
{
    private readonly IJSRuntime _js;
    private const string Key = "auth_token";

    public TokenStorage(IJSRuntime js) => _js = js;

    public Task SaveAsync(string token) =>
        _js.InvokeVoidAsync("localStorage.setItem", Key, token).AsTask();

    public Task<string?> GetAsync() =>
        _js.InvokeAsync<string?>("localStorage.getItem", Key).AsTask();

    public Task ClearAsync() =>
        _js.InvokeVoidAsync("localStorage.removeItem", Key).AsTask();
}
