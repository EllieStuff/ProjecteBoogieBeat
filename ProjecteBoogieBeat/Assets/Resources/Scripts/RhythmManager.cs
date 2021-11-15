using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    const float DEFAULT_SPEED_MULTIPLIER = 1.0f;

    [SerializeField] float baseSpeed = 0.6f;
    [SerializeField] float speedMultiplier = DEFAULT_SPEED_MULTIPLIER;
    [SerializeField] float startDelay = 3.0f;
    [SerializeField] float spawnDelay = 0.5f;
    [SerializeField] float destroyDelay = 0.25f;

    private PlayerCarController playerScript;
    private RhythmMarksSetScript[] marksSetsArray;
    private bool interactionPressed = false;
    private float actualSpeed = 0.0f;
    private int currMarksSetIdx = 0;

    public float RhythmSpeed { get { return actualSpeed; } }
    public float DestroyDelay { get { return destroyDelay; } }


    private void Awake()
    {
        marksSetsArray = transform.GetChild(1).GetComponentsInChildren<RhythmMarksSetScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCarController>();

        actualSpeed = baseSpeed;
        StartCoroutine(SpawnMarksSetsCoroutine());

    }


    private void Update()
    {
        CheckMarksInputs();

        RefreshActualSpeed();

    }

    private void CheckMarksInputs()
    {
        if (!interactionPressed && playerScript.RealAnyInput)
        {
            interactionPressed = true;

            // Find Colliding Mark Idx
            int collidingMarksSetIdx = -1;
            for (int i = 0; i < marksSetsArray.Length && collidingMarksSetIdx < 0; i++)
            {
                if (marksSetsArray[i].gameObject.activeSelf)
                {
                    if (marksSetsArray[i].GetMainMark().isCollidingCenter)
                        collidingMarksSetIdx = i;

                }

            }

            // Once searched, penalize if not found
            if(collidingMarksSetIdx < 0)
            {
                Debug.Log("Wrong Timing");
                TriggerWrongTiming();
            }
            // Else, Reinit the MarksSet
            else
            {
                Debug.Log("Good Timing");
                marksSetsArray[collidingMarksSetIdx].ReinitSet();
            }

            // Refresh the Inputs
            playerScript.RefreshInputs();

        }
        else if(interactionPressed && !playerScript.RealAnyInput)
        {
            interactionPressed = false;
        }

    }

    private void RefreshActualSpeed()
    {
        actualSpeed = baseSpeed * speedMultiplier;
    }


    public void TriggerWrongTiming()
    {
        // Do Stuff Here
        playerScript.TriggerWrongTiming();
    }


    private void ActivateRhythmMarksSet()
    {
        bool marksSetAvailable = true;
        int idxCopy = currMarksSetIdx;
        do {
            currMarksSetIdx++;
            if (currMarksSetIdx >= marksSetsArray.Length) currMarksSetIdx = 0;
            if (currMarksSetIdx == idxCopy) marksSetAvailable = false;
        } 
        while (marksSetsArray[currMarksSetIdx].gameObject.activeSelf && marksSetAvailable);

        if (marksSetAvailable)
        {
            marksSetsArray[currMarksSetIdx].gameObject.SetActive(true);
            // ToDo: Activate effect if needed
        }


    }


    IEnumerator SpawnMarksSetsCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        while (playerScript.IsPlaying)
        {
            ActivateRhythmMarksSet();

            yield return new WaitForSeconds(spawnDelay);
        }

    }


}
