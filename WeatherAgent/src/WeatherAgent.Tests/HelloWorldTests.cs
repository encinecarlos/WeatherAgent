using Xunit;

public class HelloWorldTests
{
    [Fact]
    public void TestHelloWorld()
    {
        Assert.Equal("Hello, World!", HelloWorld.GetMessage());
    }
}