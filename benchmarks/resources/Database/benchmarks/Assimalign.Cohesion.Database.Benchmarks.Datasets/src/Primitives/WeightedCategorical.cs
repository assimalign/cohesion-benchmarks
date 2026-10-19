using System;
using System.Collections.Generic;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>Immutable Walker alias table: O(n) construction, O(1) sampling and storage per category.</summary>
public sealed class WeightedCategorical
{
    private readonly double[] probability;
    private readonly int[] alias;

    public WeightedCategorical(params double[] weights)
    {
        if (weights.Length == 0) throw new ArgumentException("Weights are empty.", nameof(weights));
        probability = new double[weights.Length];
        alias = new int[weights.Length];
        double sum = 0;
        foreach (double w in weights)
        {
            if (!double.IsFinite(w) || w < 0) throw new ArgumentOutOfRangeException(nameof(weights));
            sum += w;
        }
        if (!(sum > 0) || !double.IsFinite(sum)) throw new ArgumentOutOfRangeException(nameof(weights));
        var small = new Stack<int>();
        var large = new Stack<int>();
        for (int i = 0; i < weights.Length; i++)
        {
            probability[i] = weights[i] * weights.Length / sum;
            (probability[i] < 1 ? small : large).Push(i);
        }
        while (small.Count != 0 && large.Count != 0)
        {
            int a = small.Pop(), b = large.Pop();
            alias[a] = b;
            probability[b] -= 1 - probability[a];
            (probability[b] < 1 ? small : large).Push(b);
        }
        foreach (int i in large) probability[i] = 1;
        foreach (int i in small) probability[i] = 1;
    }

    public int Sample(ref EntityRandom random)
    {
        int column = random.Int(0, probability.Length - 1);
        return random.NextDouble() < probability[column] ? column : alias[column];
    }
}
