using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    public GameObject exitMessage;
    void Start()
    {
        exitMessage.SetActive(true);
        Invoke(nameof(HideExitMessage), 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Gameplay");
        }
    }

    void HideExitMessage()
    {
        exitMessage.SetActive(false);
    }
}

