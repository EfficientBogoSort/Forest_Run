using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProcPlatform : MonoBehaviour
{
    // object to instantiate
    public GameObject ent;
    public GameObject player;
    public GameObject tmp;
    private const int StartDelay = 2;
    private const float DespawnDist = 1.4f;
    private float nextRest = 0;
    private const float RestCoolDown = 40;
    private const int RestDur = 5;
    private const int NumPowerUps = 4;
    private TerrainMeshGenerator side1;
    private TerrainMeshGenerator side2;

    private void Start() {
        player = GameObject.Find(Constants.PLAYER_NAME);
        this.transform.name = Constants.PLAT_NAME;
        side1 = transform.Find("Side1").
            GetComponent<TerrainMeshGenerator>();
        side2 = transform.Find("Side2").
            GetComponent<TerrainMeshGenerator>();

        // spawn objects
        respawnEntities();
            
        

        moveSides();    
        PlatformPool.inst.platCount++;
    }
    // Update is called once per frame
    void Update()
    {
        // recycle this platform by putting it at the end
        if (transform.InverseTransformPoint(player.transform.position).z >= DespawnDist) {
            this.transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + transform.localScale.z * PlatformPool.inst.numPlats);
            
            moveSides();
            PlatformPool.inst.platCount++;
            // respawn the objects to a new position
            respawnEntities();

        }
        if (Time.timeSinceLevelLoad  > nextRest + RestDur) {
            nextRest = Time.timeSinceLevelLoad  + RestCoolDown + RestDur;
        }

    }


    // respawns entities on the road
    private void respawnEntities() {    
        if (Time.timeSinceLevelLoad  > Constants.PHASE_LIMIT[0] / 2
            && Time.timeSinceLevelLoad  > nextRest) {
            // in resting period
            if (PowerUpPool.inst != null) {
                RespawnPowerUps();
            }

        } else {
            // spawn an obstacle by chance
            double chance = Random.Range(0.0f, 1.0f);
            if (ObstaclePool.inst != null && chance > ObstaclePool.inst.minSpawnChance) {
                respawnObs();
            }
            if (PowerUpPool.inst != null) {
                RespawnPowerUps();
            }
            
        }
       

       

    }

    private void respawnObs() {
        // Do not spawn obstacle at the beginning of the game
        // to let the player get ready
        if (PlatformPool.inst.platCount < StartDelay) {
            return;
        }
        // spawn an obstacle
        GameObject obs = ObstaclePool.inst.getObstacle();
        if (obs != null) {
            obs.transform.position = transform.position;
            obs.SetActive(true);
        }
    }

    private void RespawnPowerUps() {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < NumPowerUps; i++) {
            SpawnPowerUp(points);
        }

    }

    private void SpawnPowerUp(List<Vector2> points) {
        float chance = Random.Range(0, 1.0f);
        GameObject tmp = PowerUpPool.inst.getPowerUp();
        if (tmp != null && chance < tmp.GetComponent<PowerUpScript>().spawnChance) {
            if (tmp.tag == "SpeedModifier" && ObstaclePool.phase < Constants.PHASE[2]){
                SpawnPowerUp(points);
                return;
            }
            tmp.GetComponent<PowerUpScript>().SetPos(transform.position, points);
            tmp.SetActive(true);
        }
    }

    // Returns whether if the spawn point specified is a suitable 
    // point to spawn it
    private bool inPowerUpArea(List<Vector2> occupied, Vector2 pos) {
        for (int i = 0; i < occupied.Count; i++) {
            if (Vector2.Distance(pos, occupied[i]) < 0.2667f) {
                return false;
            }
        }
        return !(pos.y * PlatformPool.inst.platform.transform.localScale.z > -6 &&
                    pos.y * PlatformPool.inst.platform.transform.localScale.z < 6);
    }

    // moves the side terrains to a new position and regenerates
    // their features to fit in
    private void moveSides() {
        side1.manualOffsetZ = - PlatformPool.inst.platCount *
                transform.localScale.z;
        side2.manualOffsetZ = - PlatformPool.inst.platCount *
            transform.localScale.z;
        side1.RegenrateTerrain();
        side2.RegenrateTerrain();
    }

}
