public class CopyDNAUtility
{
    public void CopyPopulation(Individual[] to, Individual[] from)
    {
        for (int i = 0; i < from.Length; i++)
            CopyDNA(to[i].dna, from[i].dna);
    }

    public void CopyDNA(DNA to, DNA from)
    {
        to.height = from.height;
        to.width = from.width;
        to.r = from.r;
        to.g = from.g;
        to.b = from.b;
    }

    public void SpliceDNA(DNA to, DNA from1, DNA from2)
    {
        to.height = RandomizeTrait(from1.height, from2.height);
        to.width = RandomizeTrait(from1.width, from2.width);
        to.r = RandomizeTrait(from1.r, from2.r);
        to.g = RandomizeTrait(from1.g, from2.g);
        to.b = RandomizeTrait(from1.b, from2.b);
    }

    public float RandomizeTrait(float trait1, float trait2)
    {
        if (UnityEngine.Random.Range(0, 2) < 1)
            return trait1;
        else
            return trait2;
    }
}
