using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using GestãoDeIdeasV2.Dtos;
using Microsoft.Extensions.Logging;

namespace GestãoDeIdeasV2.Services;

public class AdviceService : IAdviceService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AdviceService> _logger;

    public AdviceService(HttpClient httpClient, ILogger<AdviceService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetRandomAdviceAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<AdviceSlipDto>("/advice");
        return response?.Slip?.Advice ?? "No advice available right now.";
    }
}