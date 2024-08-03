using Xunit;
using Moq;
using System.Collections.Generic;
using DeerskinSimulation.Models;

public class EventResultTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var result = new EventResult();

        // Assert
        Assert.Empty(result.Records);
        Assert.Null(result.OriginatorAction);
        Assert.Null(result.RecipientAction);
        Assert.Null(result.Meta);
        Assert.Equal(EventResultStatus.None, result.Status);
    }

    [Fact]
    public void Constructor_WithSingleRecord_ShouldInitializeCorrectly()
    {
        // Arrange
        var record = new EventRecord("Test message", "blue");

        // Act
        var result = new EventResult(record);

        // Assert
        Assert.Single(result.Records);
        Assert.Equal("Test message", result.Records[0].Message);
        Assert.Equal("blue", result.Records[0].Color);
    }

    [Fact]
    public void Constructor_WithMultipleRecords_ShouldInitializeCorrectly()
    {
        // Arrange
        var records = new List<EventRecord>
        {
            new EventRecord("Message 1", "red"),
            new EventRecord("Message 2", "green")
        };

        // Act
        var result = new EventResult(records);

        // Assert
        Assert.Equal(2, result.Records.Count);
        Assert.Equal("Message 1", result.Records[0].Message);
        Assert.Equal("Message 2", result.Records[1].Message);
    }

    [Fact]
    public void ApplyActions_ShouldInvokeOriginatorAndRecipientActions()
    {
        // Arrange
        var originatorMock = new Mock<ParticipantRole>("Originator", 0, 0);
        var recipientMock = new Mock<ParticipantRole>("Recipient", 0, 0);

        bool originatorActionInvoked = false;
        bool recipientActionInvoked = false;

        EventAction originatorAction = p => originatorActionInvoked = true;
        EventAction recipientAction = p => recipientActionInvoked = true;

        var result = new EventResult(new List<EventRecord>(), originatorAction, recipientAction);

        // Act
        result.ApplyActions(originatorMock.Object, recipientMock.Object);

        // Assert
        Assert.True(originatorActionInvoked);
        Assert.True(recipientActionInvoked);
    }

    [Fact]
    public void HasRecords_ShouldReturnTrue_WhenRecordsExist()
    {
        // Arrange
        var records = new List<EventRecord>
        {
            new EventRecord("Message 1", "red")
        };

        var result = new EventResult(records);

        // Act
        var hasRecords = result.HasRecords();

        // Assert
        Assert.True(hasRecords);
    }

    [Fact]
    public void HasRecords_ShouldReturnFalse_WhenNoRecordsExist()
    {
        // Arrange
        var records = new List<EventRecord>();

        var result = new EventResult(records);

        // Act
        var hasRecords = result.HasRecords();

        // Assert
        Assert.False(hasRecords);
    }

    [Fact]
    public void LastRecord_ShouldReturnLastRecord_WhenRecordsExist()
    {
        // Arrange
        var records = new List<EventRecord>
        {
            new EventRecord("Message 1", "red"),
            new EventRecord("Message 2", "green")
        };

        var result = new EventResult(records);

        // Act
        var lastRecord = result.LastRecord();

        // Assert
        Assert.Equal("Message 2", lastRecord.Message);
    }

    [Fact]
    public void LastRecord_ShouldReturnNull_WhenNoRecordsExist()
    {
        // Arrange
        var records = new List<EventRecord>();

        var result = new EventResult(records);

        // Act
        var lastRecord = result.LastRecord();

        // Assert
        Assert.Null(lastRecord);
    }
}
