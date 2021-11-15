using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefabsAtRandomPos : MonoBehaviour
{
    private const int RANGE = 180;

    public GameObject prefab;
    public int prefabAmmount = 10;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        for (int i = 0; i < prefabAmmount; i++)
        {
            Vector3 rndPos = new Vector3(Random.Range(-RANGE, RANGE), 0.0f, Random.Range(-RANGE, RANGE));
            while(Vector3.Distance(playerPos, rndPos) < 10)
                rndPos = new Vector3(Random.Range(-RANGE, RANGE), 0.0f, Random.Range(-RANGE, RANGE));

            Instantiate(prefab, rndPos, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
