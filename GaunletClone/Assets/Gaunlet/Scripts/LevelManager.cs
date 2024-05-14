using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int level;
    public bool lastLevel;

    //Instances (For Singleton)
    public static LevelManager Instance = null;

    void Start()
    {
        UIManager ui = FindAnyObjectByType<UIManager>();
        ui.setLevel(level);
    }

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void switchScene()
    {
        if (lastLevel)
        {
            SceneManager.LoadScene(0);
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
