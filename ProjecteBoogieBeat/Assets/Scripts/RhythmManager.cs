using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    private bool interactionPressed = false;

    PlayerCarController playerScript;
    [SerializeField] CenterOfRhythmScript centerScript;
    [SerializeField] GameObject rhythmMarkPrefab;


    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCarController>();

    }


    private void Update()
    {

        if (interactionPressed)
        {
            // Do Stuff here



            interactionPressed = false;
        }





        //AfterUpdate();
    }


    //private void AfterUpdate()
    //{
    //    if (interactionPressed)
    //        interactionPressed = false;
    //}


    public PlayerCarController GetPlayer()
    {
        return playerScript;
    }

    public bool GetInteractionPressed()
    {
        return interactionPressed;
    }

    public void SetInteractionPressed(bool _state)
    {
        interactionPressed = _state;
    }

}
