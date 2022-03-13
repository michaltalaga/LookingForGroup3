using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Web;

namespace Getters.Roll20
{
    public class Roll20GamePageParser : IRoll20GamePageParser
    {
        public Game Parse(GameDetailsScrappedPage gameDetailsPage)
        {

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(gameDetailsPage.RawHtml);

            var tableNode = htmlDoc.DocumentNode.SelectSingleNode("//table[contains(@class, 'lfgtable')]");
            var getText = (string selector) => HttpUtility.HtmlDecode(tableNode.SelectSingleNode(selector).InnerText.Trim());
            var getTextFromNextCellByLabel = (string label) => HttpUtility.HtmlDecode(tableNode.SelectSingleNode($"//*[contains(text(), '{label}')]/parent::td/following-sibling::td").InnerText.Trim());
            var getBoolFromNextCellByLabel = (string label) => getTextFromNextCellByLabel(label) == "No" ? false : true;
            //var getEnumFromNextCellByLabel = (string label) => EnumsNET

            //var getAttribute = (string selector, string attribute) => htmlDoc.DocumentNode.SelectSingleNode(selector).Attributes[attribute].Value;
            var game = new Game
            {
                Id = gameDetailsPage.Id,
                Title = getText("//*[@class='campaign_name']"),
                GameSystem = getTextFromNextCellByLabel("Playing"),
                NextGameDateTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(getText("//*[@class='nextgame']/*[@class='localtimeme']"))),
                TotalPlayersNeeded = int.Parse(getTextFromNextCellByLabel("Total Players Needed")),
                GameType = ParseEnum<GameType>(getTextFromNextCellByLabel("Game Type")),
                Frequency = ParseEnum<PlayFrequency>(getTextFromNextCellByLabel("Frequency")),
                AudioVisual = ParseEnum<AudioVisual>(getTextFromNextCellByLabel("Audio / Visual")),
                PrimaryLanguage = getTextFromNextCellByLabel("Primary Language"),
                NewPlayersWelcome = getBoolFromNextCellByLabel("New Players are Welcome"),
                MatureContent = getBoolFromNextCellByLabel("Mature Content(18+)"),
                PayToPlay = getBoolFromNextCellByLabel("Pay to Play"),
                PickUpGame = getBoolFromNextCellByLabel("Pick Up Game")

            };
            return game;
        }
        private T ParseEnum<T>(string text) where T : struct, Enum
        {
            text ??= "";
            text = text.Trim();
            var mappedValue = enumMap[typeof(T)].GetValueOrDefault(text, text);
            try
            {
                return EnumsNET.Enums.Parse<T>(mappedValue);
            }
            catch (Exception)
            {

                throw;
            }
        }
        Dictionary<Type, Dictionary<string, string>> enumMap = new()
        {
            {
                typeof(GameType),
                new(StringComparer.InvariantCultureIgnoreCase)
                {
                    { "Role Playing Game", "RolePlayingGame" },
                    { "Card Game", "CardGame" },
                    { "Board Game", "BoardGame" }
                }
            },
            {
                typeof(PlayFrequency),
                new(StringComparer.InvariantCultureIgnoreCase)
                {
                    { "", "Unknown" },
                    { "Non-Recurring (One-Time) Game", "Once" },
                    { "Played Weekly", "Weekly" },
                    { "Played Every Other Week", "BiWeekly" },
                    { "Played Monthly", "Monthly" }
                }
            },
            {
                typeof(AudioVisual),
                new(StringComparer.InvariantCultureIgnoreCase)
                {
                    { "", "Unknown" },
                    { "Text Only", "Text" },
                    { "Voice Only", "Voice" },
                    { "Video And Voice", "VideoAndVoice" }
                }
            }
        };
    }
}