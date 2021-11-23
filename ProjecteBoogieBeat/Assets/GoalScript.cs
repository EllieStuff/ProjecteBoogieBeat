using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    [SerializeField] int lapsAmount = 3;
    [SerializeField] float delayOnReinit = 10.0f;
    [SerializeField] TextMeshProUGUI lapsText;
    [SerializeField] TextMeshProUGUI youWinLoseText;

    private void Start()
    {
        lapsText.text = "0/" + lapsAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerCarController playerScript = other.GetComponent<PlayerCarController>();
            if (playerScript.currLap < lapsAmount)
            {
                playerScript.currLap++;
                lapsText.text = playerScript.currLap + "/" + lapsAmount;
            }
            else
            {
                playerScript.ReachedGoal();
                youWinLoseText.gameObject.SetActive(true);
                StartCoroutine(ReinitSceneCoroutine());
            }

        }

    }


    IEnumerator ReinitSceneCoroutine()
    {
        yield return new WaitForSeconds(delayOnReinit);
        SceneManager.LoadScene("JJ Circuit");
    }

}
