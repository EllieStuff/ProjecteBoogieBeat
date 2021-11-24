using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
    [SerializeField] int lapsAmount = 3;
    [SerializeField] float delayOnReinit = 10.0f;
    [SerializeField] string youWinText = "You Win!!";
    [SerializeField] string youLoseText = "You Lose!!";
    [SerializeField] TextMeshProUGUI lapsText;
    [SerializeField] TextMeshProUGUI youWinLoseText;

    bool playerWins = true;
    bool winnerDecided = false;

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
                winnerDecided = true;

                EndGame();
            }

        }
        else if (other.tag.Equals("AICar"))
        {
            AICarController aiCarScript = other.GetComponent<AICarController>();
            if (aiCarScript.currLap < lapsAmount)
            {
                aiCarScript.currLap++;
            }
            else
            {
                aiCarScript.ReachedGoal();
                if (!winnerDecided)
                {
                    winnerDecided = true;
                    playerWins = false;
                }
            }

        }

    }


    public void EndGame()
    {
        youWinLoseText.gameObject.SetActive(true);

        if (playerWins)
            youWinLoseText.text = youWinText;
        else
            youWinLoseText.text = youLoseText;

        StartCoroutine(ReinitSceneCoroutine());

    }

    IEnumerator ReinitSceneCoroutine()
    {
        yield return new WaitForSeconds(delayOnReinit);
        SceneManager.LoadScene("JJ Circuit");
    }

}
