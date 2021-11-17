using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMarksSetScript : MonoBehaviour
{
    [SerializeField] RhythmMarkScript[] rhythmMarks = new RhythmMarkScript[2];
    [SerializeField] internal Color lerpFinalColor = Color.black;
    [SerializeField] internal float lerpMaxTime = 1.0f;

    RhythmManager rhythmManager;
    float initMarksDistance;
    Color lerpInitFinalColor;

    public float Speed { get { return rhythmManager.RhythmSpeed; } }


    // Start is called before the first frame update
    void Start()
    {
        rhythmManager = transform.GetComponentInParent<Transform>().GetComponentInParent<RhythmManager>();

        initMarksDistance = Vector3.Distance(transform.position, rhythmMarks[0].transform.position);
        lerpInitFinalColor = lerpFinalColor;
        gameObject.SetActive(false);

    }


    public void StartReinitSetTimer()
    {
        StartCoroutine(ReinitSetCoroutine());
        rhythmMarks[0].StartLerpColorCoroutine();
        rhythmMarks[1].StartLerpColorCoroutine();
    }
    public void ReinitSet()
    {
        rhythmMarks[0].transform.position = transform.position + (transform.right * initMarksDistance);
        rhythmMarks[1].transform.position = transform.position + (-transform.right * initMarksDistance);
        rhythmMarks[0].ReinitColor();
        rhythmMarks[1].ReinitColor();

        rhythmMarks[0].isCollidingCenter = false;
        lerpFinalColor = lerpInitFinalColor;
        gameObject.SetActive(false);

    }

    public RhythmMarkScript GetMainMark()
    {
        return rhythmMarks[0];
    }


    IEnumerator ReinitSetCoroutine()
    {
        float waitTime = 0.0f;
        while(waitTime < rhythmManager.DestroyDelay)
        {
            yield return new WaitForEndOfFrame();
            waitTime += Time.deltaTime;

        }

        if(lerpFinalColor != Color.green)
            rhythmManager.TriggerWrongTiming();

        ReinitSet();
    }

}
