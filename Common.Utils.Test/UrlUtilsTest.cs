namespace Common.Utils.Test;
using Common.Utils;

public class UrlUtilsTest
{
    [Fact]
    public void ToURLParamsDictionary()
    {
        var url = "https://www.google.com/search?q=hello+world&hl=en&gl=us&tbm=nws&source=lnt&tbs=qdr:d&sa=X&ved=2ahUKEwjBn8iK5ZL9AhUJxYUKHd4tCqQQ0QF6BAgBEAU&biw=1920&bih=937&dpr=1";
        var result = url.ToURLParamsDictionary();
        Assert.Equal("hello+world", result["q"]);
    }
}
