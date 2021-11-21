using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfRhythmScript : MonoBehaviour
{
    [SerializeField] float echoActiveTime = 1.0f;
    //[SerializeField] float scaleSpeed = 0.01f;
    //[SerializeField] float fadeSpeed = 0.01f;
    [SerializeField] float scaleFactor = 1.5f;

    Transform echoTransform;
    SpriteRenderer echoSpriteRenderer;
    Vector3 initScale, finalScale;
    Color initColor, finalColor;
    int echoId = 0;

    private void Start()
    {
        echoTransform = transform.GetChild(0);
        echoSpriteRenderer = echoTransform.GetComponent<SpriteRenderer>();

        initScale = echoTransform.localScale;
        finalScale = new Vector3(initScale.x * scaleFactor, initScale.y * scaleFactor, initScale.z);
        initColor = echoSpriteRenderer.color;
        finalColor = new Color(initColor.r, initColor.g, initColor.b, 0.0f);

        echoTransform.gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("RhythmMark"))
        {
            echoId++;
            if (echoId > 100) echoId = 0;

            StartCoroutine(EchoCoroutine());

        }

    }

    private IEnumerator EchoCoroutine()
    {
        echoTransform.gameObject.SetActive(true);
        echoTransform.localScale = initScale;
        echoSpriteRenderer.color = initColor;

        float timePast = 0.0f;
        int thisEchoId = echoId;
        while(timePast < echoActiveTime && thisEchoId == echoId)
        {
            //echoTransform.localScale = new Vector3(transform.localScale.x + scaleSpeed, transform.localScale.y + scaleSpeed, initScale.z);

            //if(echoSpriteRenderer.color.a > 0.0f)
            //    echoSpriteRenderer.color = new Color(initColor.r, initColor.g, initColor.b, initColor.a - fadeSpeed);

            float lerpT = timePast;
            echoTransform.localScale = Vector3.Lerp(initScale, finalScale, lerpT);
            echoSpriteRenderer.color = Color.Lerp(initColor, finalColor, lerpT);

            yield return new WaitForEndOfFrame();
            timePast += Time.deltaTime;
        }

        if (thisEchoId == echoId)
        {
            echoTransform.localScale = initScale;
            echoSpriteRenderer.color = initColor;
            echoTransform.gameObject.SetActive(false);
        }

    }

}
