using UnityEngine;

public class NewGeneration : MonoBehaviour
{
    // Data storage
    Individual[] population;
    Individual[] nextGeneration;

    // Utilities
    CopyDNAUtility copier;

    public NewGeneration(CopyDNAUtility copier)
    {
        this.copier = copier;
    }

    public void AssignOffspring(Individual[] population, float selectionPressure)
    {
        int cutoffRank = (int)(population.Length * selectionPressure);

        foreach (Individual individual in population)
        {
            if (individual.stats.rank <= cutoffRank)
                individual.stats.offspring = (int)(1 / selectionPressure);
            else
                individual.stats.offspring = 0;
        }
    }

    public void CreateNextGeneration(Individual[] population, Individual[] nextGeneration, InheritanceType inheritanceType)
    {
        this.population = population;
        this.nextGeneration = nextGeneration;

        int childIndex = 0;

        foreach (Individual individual in population)
        {
            if (individual.stats.offspring == 0)
                continue;

            for (int j = 0; j < individual.stats.offspring; j++)
            {
                if (childIndex >= population.Length)
                    return;

                CreateChild(childIndex, individual, inheritanceType);
                childIndex++;
            }
        }
    }

    private void CreateChild(int childIndex, Individual firstParent, InheritanceType inheritanceType)
    {
        switch (inheritanceType)
        {
            case InheritanceType.Single: copier.CopyDNA(nextGeneration[childIndex].dna, firstParent.dna); break;
            case InheritanceType.Double: copier.SpliceDNA(nextGeneration[childIndex].dna, firstParent.dna, GetRandomIndividual(population).dna); break;
        }
    }

    private Individual GetRandomIndividual(Individual[] population)
    {
        return population[Random.Range(0, population.Length)];
    }
}
