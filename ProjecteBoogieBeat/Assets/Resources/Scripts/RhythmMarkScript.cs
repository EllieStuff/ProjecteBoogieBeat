using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmMarkScript : MonoBehaviour
{
    public bool isMain = false;
    internal bool isCollidingCenter = false;

    RhythmMarksSetScript marksManager = null;
    SpriteRenderer spriteRenderer;
    Color initColor;
    float lerpColorSpeed = 3.0f;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initColor = spriteRenderer.color;
    }

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

    public void StartLerpColorCoroutine()
    {
        StartCoroutine(LerpColorCoroutine());
    }
    public void ReinitColor()
    {
        spriteRenderer.color = initColor;
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


    private IEnumerator LerpColorCoroutine()
    {
        float lerpT = 0.0f;
        while(lerpT < marksManager.lerpMaxTime)
        {
            spriteRenderer.color = Color.Lerp(initColor, marksManager.lerpFinalColor, lerpT);

            yield return new WaitForEndOfFrame();
            lerpT += Time.deltaTime * lerpColorSpeed;
        }

        ReinitColor();

    }


}
