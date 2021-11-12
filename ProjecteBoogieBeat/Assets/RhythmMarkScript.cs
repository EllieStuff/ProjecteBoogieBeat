using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMarkScript : MonoBehaviour
{
    public bool isMain = false;

    RhythmMarksSetScript marksManager = null;
    bool interacting = false;


    // Start is called before the first frame update
    void Start()
    {
        marksManager = GetComponentInParent<RhythmMarksSetScript>();

    }

    // Update is called once per frame
    void Update()
    {


        GetInputs();

    }


    private void Move()
    {
        Vector3 moveDir = (marksManager.transform.position - transform.position).normalized;
        // Fer que es mogui amb velocitat del RhythmMarksSetScript
        //transform.position = 

    }

    private void GetInputs()
    {
        if (isMain)
        {
            if (!interacting && marksManager.GetRMPlayer().AnyInput && !marksManager.GetRMInteractionPressed())
            {
                interacting = true;
            }
            else if (interacting)
            {
                interacting = false;
            }

        }

    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("CenterOfRhythm") && interacting)
        {
            marksManager.GetRhythmManager().SetInteractionPressed(true);
            // whatever needed
            Destroy(gameObject);
        }

    }





}
