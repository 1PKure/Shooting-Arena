using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button deleteButton;
    void Start()
    {
        saveButton.onClick.AddListener(() => GameManager.Instance.SaveGame());
        loadButton.onClick.AddListener(() => GameManager.Instance.LoadGame());
        deleteButton.onClick.AddListener(() => GameManager.Instance.DeleteSave());
    }

    public void Save()
    {
        Debug.Log("Guardando partida...");
        GameManager.Instance.SaveGame();
    }

    public void Load()
    {
        Debug.Log("Cargando partida...");
        GameManager.Instance.LoadGame();
    }

    public void Delete()
    {
        Debug.Log("Borrando partida...");
        GameManager.Instance.DeleteSave();
    }
}

