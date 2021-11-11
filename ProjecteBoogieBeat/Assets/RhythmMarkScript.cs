using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMarkScript : MonoBehaviour
{
    // ToDo: Make Pair RhythmMark

    RhythmManager rhythmManager;
    bool interacting = false;


    // Start is called before the first frame update
    void Start()
    {
        rhythmManager = GetComponentInParent<RhythmManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rhythmManager.GetPlayer().AnyInput && !rhythmManager.interactionPressed)
        {
            interacting = true;
        }

    }

    private void LateUpdate()
    {
        if (interacting) interacting = false;

    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("CenterOfRhythm") && interacting)
        {
            rhythmManager.interactionPressed = true;
            // whatever needed
            Destroy(gameObject);
        }

    }





}
