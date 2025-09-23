using FIAPCloudGames.Domain.ValueObjects;

namespace FIAPCloudGames.Tests;

public class PasswordTest
{
    [Theory]
    [InlineData("")]
    [InlineData("1234567")]             // Menor que 8 caracteres
    [InlineData("abcdefgh")]            // Sem números e símbolos
    [InlineData("abcd1234")]            // Sem símbolos
    [InlineData("abcd@#$%")]            // Sem números
    public void Constructor_WithInvalidPassword_ThrowsArgumentException(string inputPassword)
    {
        Assert.Throws<ArgumentException>(() => new Password(inputPassword));
    }

    [Theory]
    [InlineData("Password1!")]
    [InlineData("Abc123$%")]
    [InlineData("Test@1234")]
    public void Constructor_WithValidPassword_HashesCorrectly(string inputPassword)
    {
        Password password = new Password(inputPassword);

        Assert.False(string.IsNullOrWhiteSpace(password.Hash));
        Assert.NotEqual(inputPassword, password.Hash); // Verifica se foi de fato aplicado o hash
        Assert.True(BCrypt.Net.BCrypt.Verify(inputPassword, password.Hash)); // Valida o hash
    }
}
