using System;
using NUnit.Framework;

[TestFixture]
public class SqlExtratorTest
{
    [Test]
    public void ExtractSqlFromResponse_ShouldReturnSql_WhenSqlBlockIsPresent()
    {
        // Arrange
        string response = "Here is some text ```sql SELECT * FROM Users; ``` more text";
        string expectedSql = "SELECT * FROM Users;";

        // Act
        string actualSql = response.ExtractSqlFromResponse();

        // Assert
        Assert.AreEqual(expectedSql, actualSql);
    }

    [Test]
    public void ExtractSqlFromResponse_ShouldReturnEmptyString_WhenNoSqlBlockIsPresent()
    {
        // Arrange
        string response = "Here is some text without any SQL block.";
        
        // Act
        string actualSql = response.ExtractSqlFromResponse();

        // Assert
        Assert.AreEqual(string.Empty, actualSql);
    }

    [Test]
    public void ExtractSqlFromResponse_ShouldReturnEmptyString_WhenSqlBlockIsEmpty()
    {
        // Arrange
        string response = "Here is some text ```sql ``` more text";
        
        // Act
        string actualSql = response.ExtractSqlFromResponse();

        // Assert
        Assert.AreEqual(string.Empty, actualSql);
    }

    [Test]
    public void ExtractSqlFromResponse_ShouldReturnFirstSqlBlock_WhenMultipleSqlBlocksArePresent()
    {
        // Arrange
        string response = "Here is some text ```sql SELECT * FROM Users; ``` more text ```sql SELECT * FROM Orders; ```";
        string expectedSql = "SELECT * FROM Users;";

        // Act
        string actualSql = response.ExtractSqlFromResponse();

        // Assert
        Assert.AreEqual(expectedSql, actualSql);
    }

    [Test]
    public void ExtractSqlFromResponse_ShouldHandleSqlBlockWithNewLines()
    {
        // Arrange
        string response = "Here is some text ```sql\nSELECT *\nFROM Users;``` more text";
        string expectedSql = "SELECT *\nFROM Users;";

        // Act
        string actualSql = response.ExtractSqlFromResponse();

        // Assert
        Assert.AreEqual(expectedSql, actualSql);
    }
}
