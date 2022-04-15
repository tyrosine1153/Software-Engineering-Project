using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomUtility : MonoBehaviour
{
    public static int Probability(float[] probs)
    {
        var total = probs.Sum();
        var randomPoint = Random.value * total;

        for (var i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            randomPoint -= probs[i];
        } 
        return probs.Length - 1;
    }

    public static bool Probability(float prob)
    {
        if (prob >= 100) return true;
        if (prob <= 0) return false;

        return Probability(new[] {prob, 100 - prob}) == 0;
    }
}