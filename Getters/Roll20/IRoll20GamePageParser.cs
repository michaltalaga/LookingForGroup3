namespace Getters.Roll20
{
    public interface IRoll20GamePageParser
    {
        Game Parse(GameDetailsScrappedPage gameDetailsPage);
    }
}