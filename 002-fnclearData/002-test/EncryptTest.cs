using System;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

[TestFixture]
public class EncryptTest
{
    

    [Test]
    public void Encrypt_ShouldReturnEncryptedString_WhenValidInputIsProvided()
    {
        // Arrange
        string plainText = "Hello, World!";
        string key = "0123456789abcdef"; // 16 bytes key for AES-128
        string iv = "abcdef0123456789"; // 16 bytes IV for AES

        // Act
        string encryptedText = plainText.Encrypt(key, iv);

        // Assert
        Assert.IsNotNull(encryptedText);
        Assert.IsNotEmpty(encryptedText);
    }

    [Test]
    public void Encrypt_ShouldReturnDifferentEncryptedString_ForDifferentPlainTexts()
    {
        // Arrange
        string plainText1 = "Hello, World!";
        string plainText2 = "Goodbye, World!";
        string key = "0123456789abcdef"; // 16 bytes key for AES-128
        string iv = "abcdef0123456789"; // 16 bytes IV for AES

        // Act
        string encryptedText1 = plainText1.Encrypt(key, iv);
        string encryptedText2 = plainText2.Encrypt(key, iv);

        // Assert
        Assert.AreNotEqual(encryptedText1, encryptedText2);
    }

    [Test]
    public void Encrypt_ShouldReturnDifferentEncryptedString_ForDifferentKeys()
    {
        // Arrange
        string plainText = "Hello, World!";
        string key1 = "0123456789abcdef"; // 16 bytes key for AES-128
        string key2 = "fedcba9876543210"; // 16 bytes key for AES
        string iv = "abcdef0123456789"; // 16 bytes IV for AES

        // Act
        string encryptedText1 = plainText.Encrypt(key1, iv);
        string encryptedText2 = plainText.Encrypt(key2, iv);

        // Assert
        Assert.AreNotEqual(encryptedText1, encryptedText2);
    }

    [Test]
    public void Encrypt_ShouldReturnDifferentEncryptedString_ForDifferentIVs()
    {
        // Arrange
        string plainText = "Hello, World!";
        string key = "0123456789abcdef"; // 16 bytes key for AES-128
        string iv1 = "abcdef0123456789"; // 16 bytes IV for AES
        string iv2 = "1234567890abcdef"; // 16 bytes IV for AES

        // Act
        string encryptedText1 = plainText.Encrypt(key, iv1);
        string encryptedText2 = plainText.Encrypt(key, iv2);

        // Assert
        Assert.AreNotEqual(encryptedText1, encryptedText2);
    }
}
