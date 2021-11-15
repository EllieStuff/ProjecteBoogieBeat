using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    // ToDo: Controlar RhythmMarksSets desde aqui sense anar-los instanciant
    //  1. Treure tema instancies
    //  2. Fer que el RhythmMarksSet es reinicii i esperi al seu torn (al agafar-ne un, sumar 1 al idx de utilitzats i asegurarse de que estigui en desus)
    //  3. Controlar si el jugador a posat malament el input desde aqui, mirant els RhythmMarksSet en us de l'array

    private bool interactionPressed = false;

    PlayerCarController playerScript;
    [SerializeField] float baseSpeed = 0.6f;
    [SerializeField] float startDelay = 3.0f;
    [SerializeField] float spawnDelay = 0.5f;
    [SerializeField] float destroyDelay = 0.25f;
    [SerializeField] GameObject rhythmMarksSetPrefab;

    private float actualSpeed = 0.0f;
    private bool wrongTimingChecked = false;
    private bool isWrongTiming = true;
    private RhythmMarksSetScript[] marksSetsArray;
    //private BoxCollider centerOfRhythmCollider;

    public float RhythmSpeed { get { return actualSpeed; } }
    public float DestroyDelay { get { return destroyDelay; } }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCarController>();
        marksSetsArray = transform.GetChild(1).GetComponentsInChildren<RhythmMarksSetScript>();
        //centerOfRhythmCollider = GetComponentInChildren<BoxCollider>();

        actualSpeed = baseSpeed;
        StartCoroutine(SpawnMarksSetsCoroutine());

    }


    private void Update()
    {

        if (interactionPressed)
        {
            // Do Stuff here
            playerScript.RefreshInputs();


            interactionPressed = false;
        }

        //if (!isWrongTiming)
        //{
        //    isWrongTiming = true;
        //}

        if (wrongTimingChecked)
        {
            if (isWrongTiming)
                TriggerWrongTiming();

            isWrongTiming = true;
            wrongTimingChecked = false;
        }



        //AfterUpdate();
    }


    //private void AfterUpdate()
    //{
    //    if (interactionPressed)
    //        interactionPressed = false;
    //}


    public void CheckIfWrongTiming(bool _isCollidingCenter)
    {
        wrongTimingChecked = true;
        if (_isCollidingCenter) isWrongTiming = false;
    }
    //public bool IsWrongTiming()
    //{
    //    bool result = isWrongTiming;
    //    isWrongTiming = true;

    //    return result;
    //}
    public void TriggerWrongTiming()
    {
        // Do Stuff Here
        playerScript.TriggerWrongTiming();
    }


    public PlayerCarController GetPlayer()
    {
        return playerScript;
    }

    public bool GetInteractionPressed()
    {
        return interactionPressed;
    }

    public void SetInteractionPressed(bool _state)
    {
        interactionPressed = _state;
    }


    IEnumerator SpawnMarksSetsCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        while (playerScript.IsPlaying)
        {
            Instantiate(rhythmMarksSetPrefab, transform);
            yield return new WaitForSeconds(spawnDelay);
        }

    }


}
