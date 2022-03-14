using Getters;
using Getters.Roll20;
using System;
using System.IO;
using Xunit;

namespace GettersTests.Roll20;

public class Roll20ParserTests
{
    [Fact]
    public void WeeklyVoiceOnly()
    {
        var parser = new Roll20GamePageParser();
        var html = File.ReadAllText("./Roll20/SampleFiles/WeeklyVoiceOnly.html");
        var gameDetailsPage = new GameDetailsScrappedPage { Id = "123", RawHtml = html, Source = "file1", Timestamp = DateTimeOffset.UtcNow };
        var game = parser.Parse(gameDetailsPage);
        Assert.Equal(gameDetailsPage.Id, game.Id);
        Assert.Equal("The Ring Hunt", game.Title);
        Assert.Equal("D&D 5E", game.GameSystem);
        Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1648261800), game.NextGameDateTime);
        Assert.Equal(3, game.TotalPlayersNeeded);
        Assert.Equal(GameType.RolePlayingGame, game.GameType);
        Assert.Equal(PlayFrequency.Weekly, game.Frequency);
        Assert.Equal(AudioVisual.Voice, game.AudioVisual);
        Assert.Equal("English", game.PrimaryLanguage);
        Assert.True(game.NewPlayersWelcome);
        Assert.False(game.MatureContent);
        Assert.False(game.PayToPlay);
        Assert.False(game.PickUpGame);
        Assert.Equal("", game.Description);
    }
    [Fact]
    public void NonRecuringVoiceOnly()
    {
        var parser = new Roll20GamePageParser();
        var html = File.ReadAllText("./Roll20/SampleFiles/NonRecuringVoiceOnly.html");
        var gameDetailsPage = new GameDetailsScrappedPage { Id = "123", RawHtml = html, Source = "file1", Timestamp = DateTimeOffset.UtcNow };
        var game = parser.Parse(gameDetailsPage);
        Assert.Equal(gameDetailsPage.Id, game.Id);
        Assert.Equal("**~.⚡️⚔️⚡️ DM WhoLee's OWC \"The Rock: Grimmwolf Chronicles\" ⚡️⚔️⚡️ OWC ⚡️⚔️⚡️ First Session Free.  ⚡️⚔️⚡️ \"Play anytime, anywhere.\" ⚡️⚔️⚡️.~**", game.Title);
        Assert.Equal("D&D 5E", game.GameSystem);
        Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1647119400), game.NextGameDateTime);
        Assert.Equal(700, game.TotalPlayersNeeded);
        Assert.Equal(GameType.RolePlayingGame, game.GameType);
        Assert.Equal(PlayFrequency.Once, game.Frequency);
        Assert.Equal(AudioVisual.Voice, game.AudioVisual);
        Assert.Equal("English", game.PrimaryLanguage);
        Assert.True(game.NewPlayersWelcome);
        Assert.True(game.MatureContent);
        Assert.False(game.PayToPlay);
        Assert.True(game.PickUpGame);
        Assert.Contains("WELCOME TO THE ROCK !", game.Description);
    }
}