using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public bool interactionPressed = false;
    
    PlayerCarController playerScript;
    [SerializeField] CenterOfRhythmScript centerScript;


    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCarController>();

    }


    private void LateUpdate()
    {
        if (interactionPressed)
            interactionPressed = false;
    }


    public PlayerCarController GetPlayer()
    {
        return playerScript;
    }


}
