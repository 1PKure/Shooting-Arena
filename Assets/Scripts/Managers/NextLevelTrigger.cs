using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "GameplayScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance != null && GameManager.Instance.IsTutorialCompleted())
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

