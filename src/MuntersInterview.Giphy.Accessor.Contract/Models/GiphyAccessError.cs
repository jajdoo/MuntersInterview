namespace MuntersInterview.Giphy.Accessor.Contract;

public abstract record GiphyAccessError()
{
	public record FailedToFetch() : GiphyAccessError;
    public record AuthFailed() : GiphyAccessError;
}
