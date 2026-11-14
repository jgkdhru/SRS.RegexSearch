using System.Collections.Generic;
using System.Text.RegularExpressions;
using RegexSearch.Models;

namespace RegexSearch.Services
{
    public class SearchService
    {
        private const string Pattern = @"\+\d{1,3}\s\d(?:[\s-]?\d){9}";

        public List<MatchResult> FindWithRegex(string text)
        {
            var results = new List<MatchResult>();
            var matches = Regex.Matches(text, Pattern);
            int num = 1;
            foreach (Match m in matches)
            {
                results.Add(new MatchResult
                {
                    Number = num++,
                    Value = m.Value,
                    Position = m.Index,
                    Length = m.Length
                });
            }
            return results;
        }

        public List<MatchResult> FindWithAutomaton(string text)
        {
            var results = new List<MatchResult>();
            int i = 0;
            int num = 1;

            while (i < text.Length)
            {
                if (text[i] == '+')
                {
                    int start = i;
                    int countryDigits = 0;
                    int phoneDigits = 0;
                    bool isValid = false;

                    i++;

                    while (i < text.Length && char.IsDigit(text[i]) && countryDigits < 3)
                    {
                        countryDigits++;
                        i++;
                    }

                    if (countryDigits > 0 && i < text.Length && text[i] == ' ')
                    {
                        i++;

                        while (i < text.Length && phoneDigits < 10)
                        {
                            char c = text[i];
                            if (char.IsDigit(c))
                            {
                                phoneDigits++;
                                i++;
                            }
                            else if ((c == '-' || c == ' ') && phoneDigits > 0 && phoneDigits < 10)
                            {
                                i++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (phoneDigits == 10)
                        {
                            isValid = true;
                        }
                    }

                    if (isValid)
                    {
                        string matchValue = text.Substring(start, i - start).TrimEnd();
                        int actualEnd = start + matchValue.Length;
                        results.Add(new MatchResult
                        {
                            Number = num++,
                            Value = matchValue,
                            Position = start,
                            Length = matchValue.Length
                        });
                        i = actualEnd;
                    }
                    else
                    {
                        i = start + 1;
                    }
                }
                else
                {
                    i++;
                }
            }
            return results;
        }
    }
}