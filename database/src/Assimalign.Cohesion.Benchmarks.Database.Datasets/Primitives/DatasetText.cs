using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Assimalign.Cohesion.Benchmarks.Database.Datasets;

/// <summary>Original word inventories and deterministic synthetic sentences; no scraped prose.</summary>
public static class DatasetText
{
    public static readonly string[] Words = Read("words");
    public static readonly string[] FirstNames = Read("first-names");
    public static readonly string[] LastNames = Read("last-names");
    public static readonly string[] Brands = Read("brands");
    public static readonly string[] ProductNouns = Read("product-nouns");
    public static readonly string[] Adjectives = Read("adjectives");
    public static readonly string[] Topics = Read("topics");
    public static readonly City[] Cities = ReadCities();

    private static string[] Read(string name)
    {
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Assimalign.Cohesion.Benchmarks.Database.Datasets.Resources.{name}.txt")!;
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return reader.ReadToEnd().Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    private static City[] ReadCities()
    {
        string[] lines = Read("cities");
        var cities = new City[lines.Length];
        for (int i = 0; i < lines.Length; i++) { string[] fields = lines[i].Split('|'); cities[i] = new City(fields[0], fields[1]); }
        return cities;
    }

    public static string Pick(string[] words, ref EntityRandom random) => words[random.Int(0, words.Length - 1)];

    public static string Paragraph(ref EntityRandom random, int minimum, int maximum, double median, double sigma = 0.8)
    {
        int length = random.LogNormalLength(minimum, maximum, median, sigma);
        var text = new StringBuilder(length);
        int sentenceWords = 0;
        while (text.Length < length - 2)
        {
            string word = Pick(Words, ref random);
            if (text.Length + word.Length + 2 > length) break;
            if (sentenceWords == 0) { text.Append(char.ToUpperInvariant(word[0])); text.Append(word.AsSpan(1)); }
            else text.Append(word);
            sentenceWords++;
            if (sentenceWords >= 7 && random.Chance(0.15))
            {
                text.Append('.');
                text.Append(random.Chance(0.15) ? '\n' : ' ');
                sentenceWords = 0;
            }
            else text.Append(' ');
        }
        // Complete the final word without padding an entity to its configured maximum.
        if (text.Length > 0 && char.IsWhiteSpace(text[^1])) text.Length--;
        text.Append('.');
        while (text.Length < minimum) text.Append(" Good.");
        return text.ToString();
    }
}
