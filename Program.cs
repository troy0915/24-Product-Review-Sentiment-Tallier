using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Review
{
    public string Text { get; }
    public List<string> Tokens { get; }

    public Review(string text)
    {
        Text = text;
        Tokens = Tokenize(text);
    }

    private List<string> Tokenize(string text)
    {
        text = text.ToLower();
        text = Regex.Replace(text, @"[^\w\s]", ""); 
        return text.Split(' ',(char) StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    public Sentiment GetSentiment(HashSet<string> positive, HashSet<string> negative)
    {
        int posCount = Tokens.Count(t => positive.Contains(t));
        int negCount = Tokens.Count(t => negative.Contains(t));

        if (posCount > negCount) return Sentiment.Positive;
        if (negCount > posCount) return Sentiment.Negative;
        return Sentiment.Neutral;
    }
}

public enum Sentiment
{
    Positive,
    Negative,
    Neutral
}


namespace _24__Product_Review_Sentiment_Tallier
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var positiveWords = new HashSet<string> { "good", "great", "excellent", "amazing", "love", "happy" };
            var negativeWords = new HashSet<string> { "bad", "terrible", "awful", "hate", "poor", "sad" };

            var reviews = new List<string>
        {
            "I love this product, it's absolutely amazing!",
            "This is bad and I hate it.",
            "It was okay, nothing special.",
            "Great quality but poor customer service."
        };

            List<Review> reviewObjects = reviews.Select(r => new Review(r)).ToList();

            int posTotal = 0, negTotal = 0, neuTotal = 0;

            Console.WriteLine("Per Review Sentiment:");
            foreach (var review in reviewObjects)
            {
                var sentiment = review.GetSentiment(positiveWords, negativeWords);
                Console.WriteLine($"\"{review.Text}\" → {sentiment}");

                switch (sentiment)
                {
                    case Sentiment.Positive: posTotal++; break;
                    case Sentiment.Negative: negTotal++; break;
                    case Sentiment.Neutral: neuTotal++; break;
                }
            }


            int total = posTotal + negTotal + neuTotal;
            Console.WriteLine("\nOverall Sentiment Ratio:");
            Console.WriteLine($"Positive: {posTotal} ({(posTotal * 100.0 / total):F1}%)");
            Console.WriteLine($"Negative: {negTotal} ({(negTotal * 100.0 / total):F1}%)");
            Console.WriteLine($"Neutral: {neuTotal} ({(neuTotal * 100.0 / total):F1}%)");
        }
    }
}




