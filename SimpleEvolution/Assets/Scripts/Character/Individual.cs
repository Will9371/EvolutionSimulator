using UnityEngine;

public class Individual : MonoBehaviour
{
    public DNA dna = new DNA();
    public Stats stats = new Stats();
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();

        RandomizeAllTraits();
    }

    public void RandomizeAllTraits()
    {
        dna.height = Random.Range(DNA.HEIGHT_MIN, DNA.HEIGHT_MAX);
        dna.width = Random.Range(DNA.WIDTH_MIN, DNA.WIDTH_MAX);
        dna.r = Random.Range(DNA.R_MIN, DNA.R_MAX);
        dna.g = Random.Range(DNA.G_MIN, DNA.G_MAX);
        dna.b = Random.Range(DNA.B_MIN, DNA.B_MAX);

        RefreshDisplay();
    }

    public void DriftMutation(float mutationRange)
    {
        dna.height = Bound(dna.height, DNA.HEIGHT_MIN, DNA.HEIGHT_MAX, mutationRange);
        dna.width = Bound(dna.width, DNA.WIDTH_MIN, DNA.WIDTH_MAX, mutationRange);
        dna.r = Bound(dna.r, DNA.R_MIN, DNA.R_MAX, mutationRange);
        dna.g = Bound(dna.g, DNA.G_MIN, DNA.G_MAX, mutationRange);
        dna.b = Bound(dna.b, DNA.B_MIN, DNA.B_MAX, mutationRange);

        RefreshDisplay();
    }

    public void UniformDriftMutation(float mutationRange)
    {
        float mutation = Random.Range(-mutationRange, mutationRange);

        dna.height = Bound(dna.height + mutation, DNA.HEIGHT_MIN, DNA.HEIGHT_MAX);
        dna.width = Bound(dna.width + mutation, DNA.WIDTH_MIN, DNA.WIDTH_MAX);
        dna.r = Bound(dna.r + mutation, DNA.R_MIN, DNA.R_MAX);
        dna.g = Bound(dna.g + mutation, DNA.G_MIN, DNA.G_MAX);
        dna.b = Bound(dna.b + mutation, DNA.B_MIN, DNA.B_MAX);

        RefreshDisplay();
    }

    public void RandomMutation(float mutationChance)
    {
        dna.height = RandomMutateTrait(dna.height, mutationChance, DNA.HEIGHT_MIN, DNA.HEIGHT_MAX);
        dna.width = RandomMutateTrait(dna.width, mutationChance, DNA.WIDTH_MIN, DNA.WIDTH_MAX);
        dna.r = RandomMutateTrait(dna.r, mutationChance, DNA.R_MIN, DNA.R_MAX);
        dna.g = RandomMutateTrait(dna.g, mutationChance, DNA.G_MIN, DNA.G_MAX);
        dna.b = RandomMutateTrait(dna.b, mutationChance, DNA.B_MIN, DNA.B_MAX);

        RefreshDisplay();
    }

    private float RandomMutateTrait(float inheritedValue, float mutationChance, float lowBound, float highBound)
    {
        if (Random.Range(0f, 1f) < mutationChance)
            return Random.Range(lowBound, highBound);
        else
            return inheritedValue;
    }

    private float Bound(float value, float min, float max, float mutationRange)
    {
        return BoundInternal(value + Random.Range(-mutationRange, mutationRange), min, max);
    }

    private float Bound(float value, float min, float max)
    {
        return BoundInternal(value, min, max);
    }

    private float BoundInternal(float value, float min, float max)
    {
        if (value > max)
            return max;
        else if (value < min)
            return min;
        else
            return value;
    }

    public void RefreshDisplay()
    {
        rend.material.color = new Color(dna.r, dna.g, dna.b);
        transform.localScale = new Vector3(dna.width, dna.height, 1f);
    }
}