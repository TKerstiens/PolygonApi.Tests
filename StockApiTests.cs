using Xunit.Abstractions;
using Microsoft.Extensions.Logging;

namespace PolygonApi;

public class StocksApiTests
{
    private readonly ITestOutputHelper _output;
    private readonly ILogger<PolygonApi.Rest.RestApiBase> _logger;

    public StocksApiTests(ITestOutputHelper output)
    {
        _output = output;

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddProvider(new XUnitLoggerProvider(_output)).AddFilter("PolygonApi", LogLevel.Information);
        });

        _logger = loggerFactory.CreateLogger<PolygonApi.Rest.Stocks>();
    }

    [Fact]
    public async Task GetAllTickers()
    {
        PolygonApi.Rest.Stocks stocksApi = new PolygonApi.Rest.Stocks("0vYaJNCLUJY5SFlnmCTQxampZMswdgoF", new HttpClient(), _logger);
        List<PolygonApi.Neat.Stocks.Ticker> tickers = await stocksApi.GetAllTickersAsync();

        Assert.True(tickers.Count > 0, "Expected at least one ticker to be returned");

        foreach(PolygonApi.Neat.Stocks.Ticker ticker in tickers)
        {
            if(ticker.Symbol == "NO SYMBOL" || ticker.Name == "NO NAME")
            {
                _logger.LogDebug($"{ticker.Symbol} - {ticker.Name}");
            }
            Assert.NotEqual("NO SYMBOL", ticker.Symbol);
        }
    }
}