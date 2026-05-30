namespace MuntersInterview.Giphy.Manager;

public class GiphyManagerOptions
{
    public TimeSpan CacheTtl { get; set; } = TimeSpan.FromHours(1);
}
