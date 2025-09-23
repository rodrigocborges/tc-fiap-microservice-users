using FIAPCloudGames.Domain.ValueObjects;

namespace FIAPCloudGames.Tests;

public class EmailTest
{
    [Fact]
    public void Constructor_WithEmptyEmail_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Email(""));
    }

    [Fact]
    public void Constructor_WithInvalidEmailFormat_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Email("rodrigo@"));
    }

    [Theory]
    [InlineData("rodrigo@email.com")]
    [InlineData("rodrigo.borges@email.br")]
    [InlineData("rodrigo.borges@rb.dev")]
    public void Constructor_WithValidEmail_SetsValueCorrectly(string inputEmail)
    {
        Email email = new Email(inputEmail);
        Assert.Equal(inputEmail, email.Value);
    }
}
