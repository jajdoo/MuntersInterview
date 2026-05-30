namespace MuntersInterview.Giphy.Accessor;

public class GiphyAccessorOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.giphy.com";
    public int Limit { get; set; } = 25;
}
