using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMarkScript : MonoBehaviour
{
    public bool isMain = false;

    RhythmMarksSetScript marksManager = null;
    bool interacting = false;
    bool collidingCenter = false;


    // Start is called before the first frame update
    void Start()
    {
        marksManager = GetComponentInParent<RhythmMarksSetScript>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();

        GetInputs();

    }


    private void Move()
    {
        Vector3 moveDir = (marksManager.transform.position - transform.position).normalized;
        // Fer que es mogui amb velocitat del RhythmMarksSetScript
        transform.position = transform.position + moveDir * marksManager.Speed * Time.deltaTime;

    }

    private void GetInputs()
    {
        if (isMain)
        {
            if (!interacting && marksManager.GetRMPlayer().RealAnyInput /*&& !marksManager.GetRMInteractionPressed()*/)
            {
                interacting = true;

                //RhythmManager tmpRM = marksManager.GetRhythmManager();
                marksManager.GetRhythmManager().CheckIfWrongTiming(collidingCenter);
                //if (marksManager.GetRhythmManager().IsWrongTiming())
                //{
                //    marksManager.GetRhythmManager().TriggerWrongTiming();
                //}

            }
            else if (interacting)
            {
                interacting = false;
            }

        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (isMain)
        {
            if (other.tag.Equals("CenterOfRhythm"))
            {
                collidingCenter = true;
                marksManager.StartDestroySetTimer();
            }

            Debug.Log("Colliding with " + other.tag);

        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (isMain)
        {
            if (other.tag.Equals("CenterOfRhythm") && interacting)
            {
                Debug.Log("Interacting with " + other.tag);
                marksManager.GetRhythmManager().SetInteractionPressed(true);
                marksManager.DestroySet();
            }

        }

    }


}
