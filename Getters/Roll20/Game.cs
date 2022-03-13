using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Getters.Roll20;

public record Game
{
    [JsonProperty("id")]
    public string Id { get; set; }
    public string Title { get; set; }
    public string GameSystem { get; set; }
    public DateTimeOffset NextGameDateTime { get; set; }
    public int TotalPlayersNeeded { get; set; }
    public GameType GameType { get; set; }
    public PlayFrequency Frequency { get; set; }
    public AudioVisual AudioVisual { get; set; }
    public string PrimaryLanguage { get; set; }
    public bool NewPlayersWelcome { get; set; }
    public bool MatureContent { get; set; }
    public bool PayToPlay { get; set; }
    public bool PickUpGame { get; set; }
    public string Description { get; set; }
    public DiscussionEntry[] DiscussionEntries { get; set; }

}
public enum PlayFrequency
{
    Unknown,
    Once,
    Weekly,
    BiWeekly,
    Monthly,
}
public enum AudioVisual
{
    Unknown,
    Text,
    Voice,
    VideoAndVoice
}
public enum GameType
{
    RolePlayingGame,
    CardGame,
    BoardGame
}
public record DiscussionEntry
{ 
    public string Title { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}