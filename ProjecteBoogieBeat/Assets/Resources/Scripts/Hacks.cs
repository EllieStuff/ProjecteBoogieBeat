using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacks : MonoBehaviour
{
    public PlayerCarController player;
    public Vector3[] spawnPos;
    public Vector3[] spawnRot;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnByIdx(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnByIdx(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnByIdx(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnByIdx(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SpawnByIdx(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SpawnByIdx(5);
        }

    }


    private void SpawnByIdx(int _idx)
    {
        player.StopCompletely();
        player.transform.position = spawnPos[_idx];
        player.transform.rotation = Quaternion.Euler(spawnRot[_idx]);
    }

}
