using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMarksSetScript : MonoBehaviour
{
    [SerializeField] RhythmMarkScript[] rhythmMarks = new RhythmMarkScript[2];

    RhythmManager rhythmManager;

    public float Speed { get { return rhythmManager.RhythmSpeed; } }


    // Start is called before the first frame update
    void Start()
    {
        rhythmManager = GetComponentInParent<RhythmManager>();
    }


    public void StartDestroySetTimer()
    {
        StartCoroutine(DestroySetCoroutine());
    }
    public void DestroySet()
    {
        Destroy(gameObject);
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


    IEnumerator DestroySetCoroutine()
    {
        float waitTime = 0.0f;
        while(waitTime < rhythmManager.DestroyDelay)
        {
            yield return new WaitForEndOfFrame();
            waitTime += Time.deltaTime;

        }

        rhythmManager.TriggerWrongTiming();
        Destroy(gameObject);
    }

}
