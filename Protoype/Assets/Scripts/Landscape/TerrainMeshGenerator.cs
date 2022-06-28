using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainMeshGenerator : MonoBehaviour {

    private Vector3[] verts;
    private int[] triangles;
    private Mesh terrainMesh;
    public int octaves = 3;
    public float pers = .5f;
    public float lac = 2;
    public float scale = 2;
    public int seed;
    private float multiplier = 30;
    public AnimationCurve multiplierCurve;
    public float manualOffsetX;
    public float manualOffsetZ;
    [Range(1, 500)]public float colourTransitionRate;
    private float maxHeight;
    private float ampl = 1;
    Vector2[] offsets;
    public Material material;
    public int side;
    private int phase;
    private Color[] colours;

    public Color[] phase0Colours;
    public Color[] phase1Colours;
    public Color[] phase2Colours;
    public float[] startHeights;


    public int width = 20;
    public int height = 20;

    private void Awake() {
        phase = Constants.PHASE[0];
        colours = phase0Colours;
    }
    private void Start() {
        terrainMesh = new Mesh();
        gameObject.GetComponent<MeshFilter>().mesh = terrainMesh;

        if (side == 1) {
            seed = TerrainSettings.inst.seed1;
        } else {
            seed = TerrainSettings.inst.seed2;
        }
        RegenrateTerrain();


    }
    
    private void Update() {

        // change the colours of the mountains based on which phase the player is in
        if (phase == Constants.PHASE[0] && ObstaclePool.phase >= Constants.PHASE[1]) {
            phase = Constants.PHASE[1];
            SetColour(phase1Colours);
        } else if (phase == Constants.PHASE[1] && ObstaclePool.phase == Constants.PHASE[2]) {
            phase = Constants.PHASE[2];
            SetColour(phase2Colours);
        }
    }
    
    
    public void RegenrateTerrain() {
        
        GenerateTerrain();
        UpdateTerrain();
    }
    private void GenerateTerrain() {

        // Generate height map with various layers of Perlin noise
        GenerateHeightMap();
        // create the triangles to render the mesh
        GenerateTriangles();


    }

    private void UpdateTerrain() {
        terrainMesh = gameObject.GetComponent<MeshFilter>().mesh;
        terrainMesh.Clear();
        terrainMesh.vertices = verts;
        terrainMesh.triangles = triangles;
        // to respond accordingly to light
        terrainMesh.RecalculateNormals();
    }

    private void GenerateTriangles() {
        int currVert = 0;
        int currTrian = 0;
        triangles = new int[width * height * 6];
        for (int z = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                // get the six vertices for each quad
                triangles[currTrian] = currVert;
                triangles[currTrian + 1] = currVert + width + 1;
                triangles[currTrian + 2] = currVert + 1;
                triangles[currTrian + 3] = currVert + 1;
                triangles[currTrian + 4] = currVert + width + 1;
                triangles[currTrian + 5] = currVert + width + 2;
                currVert++;
                currTrian += 6;

            }
            currVert++;
        }
    }

    private void GenerateHeightMap() {
        GenerateOffsets();
        float[,] map = new float[(width + 1), (height + 1)];
        float perlX, perlZ, freq, perlVal;
        for (int z = 0; z <= height; z++) {
            for (int x = 0; x <= width; x++) {
                // make the end points to the lowest height to prevent 
                // the player from the player seeing through the terrain
                if (x < width - 1 && x > 0) {
                    freq = 1;
                    ampl = 1;
                    perlVal = 0;
                    // calculates the height of a vertex after considering each octave
                    for (int octNum = 0; octNum < octaves; octNum++) {
                        perlX = (x + offsets[octNum].x + manualOffsetX) * freq / scale;
                        perlZ = (z + offsets[octNum].y - manualOffsetZ) * freq / scale;
                        perlVal += Mathf.PerlinNoise(perlX, perlZ) * ampl;

                        // as the octave number increases, the effect of their amplitudes decreases
                        ampl *= pers;
                        // as the number of octaves increases, the effect of frequencies increases
                        // this allows for "smaller features" to be present, such as small bumps, or
                        // dips in the terrain
                        freq *= lac;
                    }
                    map[x, z] = perlVal;
                } else {
                    map[x, z] = 1;
                }

            }

        }
        // Normalize all the heights
        for (int z = 0; z <= height; z++) {
            for (int x = 0; x <= width; x++) {
                map[x, z] /= maxHeight;
            }
        }
        
        // send the data of the terrain to the shader to
        // colour the terrain properly
        sendTerraindata();

        // generate the vertices of the terrain mesh
        GenerateVertices(map);
    }

    private void GenerateVertices(float[,] heightsMap) {
        verts = new Vector3[(width + 1) * (height + 1)];
        int i = 0;
        for (int z = 0; z <= height; z++) {
            for (int x = 0; x <= width; x++) {
                verts[i] = new Vector3(
                // Use the animation curve to create a more realistic terrain
                // Make vertices up to a certain height lower to simulate a plane
                // area and make the mountain slopes steeper
                 x, multiplierCurve.Evaluate(heightsMap[x, z]) * multiplier, z);
                i++;
            }
        }
    }

    private void sendTerraindata() {
        material.SetFloat("minY", 1.15f * multiplier * multiplierCurve.Evaluate(1));
        material.SetFloat("maxY", 1.15f * multiplier * multiplierCurve.Evaluate(0));
        material.SetColorArray("baseColours", colours);
        material.SetFloatArray("baseStartHeights", startHeights);
        material.SetInt("baseColourCount", colours.Length);
        material.SetFloat("colourTransitionRate", colourTransitionRate);


    }

    // Generates a random offset to the perlin noise dictated
    //  by the seed given ensuring a different map each game
    private void GenerateOffsets() {
        
        maxHeight = 0;
        ampl = 1;
        offsets = new Vector2[octaves];
        System.Random random = new System.Random(seed);
        for (int i = 0; i < octaves; i++) {
            float xOffset = random.Next(-1000, 1000);
            float zOffset = random.Next(-1000, 1000);
            offsets[i] = new Vector2(xOffset, zOffset);
            maxHeight += ampl;
            ampl *= pers;
        }
    }
    private void SetColour(Color[] newColours) {
        for (int i = 0; i < colours.Length; i++) {
            colours[i] = newColours[i];
        }
    }

}
    