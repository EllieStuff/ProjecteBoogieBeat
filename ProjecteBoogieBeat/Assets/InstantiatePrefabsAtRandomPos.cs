using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefabsAtRandomPos : MonoBehaviour
{
    public GameObject prefab;
    public int prefabAmmount = 10;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        for (int i = 0; i < prefabAmmount; i++)
        {
            Vector3 rndPos = new Vector3(Random.Range(-80, 80), 0.0f, Random.Range(-80, 80));
            while(Vector3.Distance(playerPos, rndPos) < 10)
                rndPos = new Vector3(Random.Range(-80, 80), 0.0f, Random.Range(-80, 80));

            Instantiate(prefab, rndPos, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
