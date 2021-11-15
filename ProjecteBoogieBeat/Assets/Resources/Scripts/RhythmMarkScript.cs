using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMarkScript : MonoBehaviour
{
    public bool isMain = false;
    internal bool isCollidingCenter = false;

    RhythmMarksSetScript marksManager = null;


    // Start is called before the first frame update
    void Start()
    {
        marksManager = GetComponentInParent<RhythmMarksSetScript>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }


    private void Move()
    {
        Vector3 moveDir = (marksManager.transform.position - transform.position).normalized;
        transform.position = transform.position + moveDir * marksManager.Speed * Time.deltaTime;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (isMain)
        {
            if (other.tag.Equals("CenterOfRhythm"))
            {
                isCollidingCenter = true;
                marksManager.StartReinitSetTimer();
            }

            //Debug.Log("Colliding with " + other.tag);

        }

    }


}
