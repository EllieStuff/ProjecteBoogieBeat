using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayerAtPos : MonoBehaviour
{
    [SerializeField] Transform spawn;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerCarController playerScript = other.GetComponent<PlayerCarController>();
            playerScript.StopCompletely();

            Debug.Log("RespawningPlayer");

            playerScript.transform.position = spawn.position;
            playerScript.transform.rotation = spawn.rotation;
        }
    }

}
