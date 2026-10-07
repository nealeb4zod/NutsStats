using System.Net.Http.Json;
using NutsStats.Application;

namespace NutsStats.Web.Services;

public interface IVenueApiClient
{
    string? LastError { get; }
    Task<List<VenueSummaryDto>> GetVenuesAsync();
    Task<VenueDetailDto?> GetVenueByIdAsync(int id);
}

public sealed class VenueApiClient(HttpClient httpClient) : IVenueApiClient
{
    public string? LastError { get; private set; }

    public async Task<List<VenueSummaryDto>> GetVenuesAsync()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<VenueSummaryDto>>("api/venues");
            LastError = null;
            return response ?? new();
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
            return new();
        }
    }

    public async Task<VenueDetailDto?> GetVenueByIdAsync(int id)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<VenueDetailDto>($"api/venues/{id}");
            LastError = null;
            return response;
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
            return null;
        }
    }
}
