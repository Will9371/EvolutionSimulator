using UnityEngine;

public enum InheritanceType { Double, Single }
public enum MutationType { Drift, UniformDrift, RandomMutation, Randomize }
public enum NaturalSelectionType { SingleTarget, Aimless }
public enum SelectionPressure { Moderate, Heavy, None }

public class Population : MonoBehaviour
{
    // Delegated systems and utilities
    CopyDNAUtility copier = new CopyDNAUtility();
    FitnessEvaluator evaluator;
    NewGeneration nextGen;

    // Evolution parameters
    [SerializeField] InheritanceType inheritanceType;
    [SerializeField] MutationType mutationType;
    [SerializeField] NaturalSelectionType naturalSelectionType;

    [SerializeField] float driftAmount;
    [SerializeField] float mutationChance;
    [SerializeField] float secondsPerGeneration;
    [SerializeField] SelectionPressure selectionPressure;
    [SerializeField] Individual target;

    // Simulation controls
    [SerializeField] bool run;
    [SerializeField] bool stop;
    [SerializeField] bool step;
    [SerializeField] bool changeTarget;
    [SerializeField] bool randomizePopulation;
    bool running;

    // Population data
    Individual[] population;
    Individual[] nextGeneration;
    float maxFitness;

    private void Start ()
    {
        // Get references to all of the individuals in the simulation
        population = GetComponentsInChildren<Individual>();
        nextGeneration = GetComponentsInChildren<Individual>();

        // Initialize utility classes
        evaluator = new FitnessEvaluator(target);
        evaluator.ComputeMaxFitness();

        nextGen = new NewGeneration(copier);
	}

    private void Update()
    {
        GetUserInput();
    }

    private void GetUserInput()
    {
        if (step)
        {
            CancelInvoke("Tick");
            Tick();

            running = false;
            run = false;
            stop = true;
            step = false;
        }
        if (run && !running)
        {
            InvokeRepeating("Tick", secondsPerGeneration, secondsPerGeneration);
            running = true;
            stop = false;
        }
        else if (stop && running)
        {
            CancelInvoke("Tick");
            running = false;
            run = false;
        }
        else if (!run && running)
            run = true;
        else if (!stop && !running)
            stop = true;
        
        if (changeTarget)
        {
            changeTarget = false;
            target.RandomizeAllTraits();
        }
        if (randomizePopulation)
        {
            randomizePopulation = false;

            foreach (Individual individual in population)
                individual.RandomizeAllTraits();
        }
    }

    private void Tick()
    {
        foreach (Individual individual in population)
        {
            EvolveIndividual(individual);
            evaluator.ComputeFitness(individual);
        }

        if (naturalSelectionType != NaturalSelectionType.Aimless)
        {
            evaluator.RankByFitness(population);
            ApplyNaturalSelection(GetSelectionPressure());
        }
    }

    private void EvolveIndividual(Individual individual)
    {
        switch (mutationType)
        {
            case MutationType.Drift: individual.DriftMutation(driftAmount); break;
            case MutationType.UniformDrift: individual.UniformDriftMutation(driftAmount); break;
            case MutationType.Randomize: individual.RandomizeAllTraits(); break;
            case MutationType.RandomMutation: individual.RandomMutation(mutationChance); break;
        }
    }

    private void ApplyNaturalSelection(float selectionPressure)
    {
        nextGen.AssignOffspring(population, selectionPressure);
        nextGen.CreateNextGeneration(population, nextGeneration, inheritanceType);
        copier.CopyPopulation(population, nextGeneration);
    }

    private float GetSelectionPressure()
    {
        switch (selectionPressure)
        {
            case SelectionPressure.None: return 1f;
            case SelectionPressure.Moderate: return 0.5f;
            case SelectionPressure.Heavy: return 0.25f;
            default: return 0.5f;
        }
    }
}
