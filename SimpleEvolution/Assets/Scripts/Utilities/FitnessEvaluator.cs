using System;
using UnityEngine;

public class FitnessEvaluator
{
    float maxFitness;
    Individual target;

    public FitnessEvaluator(Individual target)
    {
        this.target = target;
    }

    public void ComputeMaxFitness()
    {
        maxFitness = 0;

        maxFitness += DNA.HEIGHT_MAX - DNA.HEIGHT_MIN;
        maxFitness += DNA.WIDTH_MAX - DNA.WIDTH_MIN;
        maxFitness += DNA.R_MAX - DNA.R_MIN;
        maxFitness += DNA.G_MAX - DNA.G_MIN;
        maxFitness += DNA.B_MAX - DNA.B_MIN;
        //Debug.Log("Maximum fitness: " + maxFitness);
    }

    public void ComputeFitness(Individual individual)
    {
        float totalError = 0;

        totalError += Difference(individual.dna.height, target.dna.height);
        totalError += Difference(individual.dna.width, target.dna.width);
        totalError += Difference(individual.dna.r, target.dna.r);
        totalError += Difference(individual.dna.g, target.dna.g);
        totalError += Difference(individual.dna.b, target.dna.b);

        individual.stats.totalFitness = maxFitness - totalError;
        //Debug.Log(individual.stats.totalFitness);
    }

    private float Difference(float from, float to)
    {
        return Mathf.Abs(Mathf.Abs(from) - Mathf.Abs(to));
    }

    public void RankByFitness(Individual[] population)
    {
        // Sort the individuals of the population by their total fitness value (most fit at beginning of array)
        Array.Sort(population, delegate (Individual x, Individual y) { return y.stats.totalFitness.CompareTo(x.stats.totalFitness); });

        // Assign a rank to each individual (1 = most fit)
        for (int i = 1; i <= population.Length; i++)
            population[i - 1].stats.rank = i;
    }
}
