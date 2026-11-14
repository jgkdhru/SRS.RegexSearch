namespace RegexSearch.Models
{
    public enum State
    {
        Start,
        Plus,
        CountryCode,
        Space,
        Digits,
        Accept,
        Error
    }
}