using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMarksSetScript : MonoBehaviour
{
    [SerializeField] float baseSpeed;
    [SerializeField] RhythmMarkScript[] rhythmMark = new RhythmMarkScript[2];

    RhythmManager rhythmManager;
    float actualSpeed = 0;

    public float Speed { get { return actualSpeed; } }


    // Start is called before the first frame update
    void Start()
    {
        rhythmManager = GetComponentInParent<RhythmManager>();
        actualSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public RhythmManager GetRhythmManager()
    {
        return rhythmManager;
    }

    public PlayerCarController GetRMPlayer()
    {
        return rhythmManager.GetPlayer();
    }

    public bool GetRMInteractionPressed()
    {
        return rhythmManager.GetInteractionPressed();
    }

}
