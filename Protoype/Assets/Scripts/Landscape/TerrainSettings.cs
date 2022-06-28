
using UnityEngine;

public class TerrainSettings : MonoBehaviour
{
    public int seed1;
    public int seed2;

    private float topProb;
    private float midProb;
    private float bottomProb;


    public static TerrainSettings inst;
    private void Awake() {

        // generate a new pair of seeds in each 
        // run of the game.
        inst = this;
        seed1 = Random.Range(0, 10000);
        seed2 = Random.Range(0, 10000);
        while (seed2 == seed1) {
            seed2 = Random.Range(0, 10000);
        }
    }
}
