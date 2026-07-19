using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing//,
        //PetCare,
        //EndingsArchive,
        //Settings
    }

    // housekeeping
    public static GameManager Instance { get; private set; }
    public GameState CurrentState;
    public Wolf wolf;
    public TextManager textManager;
    public AudioManager audioManager;

    // game logic stuff
    public List<int> unlockedEndings = new List<int>();
    public int food = 0;
    public int morality = 0;
    public int daysNo = 1;
    public bool hasWolf = false;
    public bool hasCharm = false;


    // panels
    public GameObject gameplayPanel;
    //public GameObject petCarePanel;
    //public GameObject settingsPanel;
    //public GameObject endingsArchivePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        CurrentState = GameState.Playing;
        UpdateUI(CurrentState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetRun()
    {
        wolf.Reset();
//        textManager.Reset();
        morality = 0;
        daysNo = 1;
        food = 0;
        hasCharm = false;
        hasWolf = false;
    }

    public void AddEnding(int endingId)
    {
        if (!unlockedEndings.Contains(endingId))
        {
            unlockedEndings.Add(endingId);
        }
    }

    private void ChangeState(GameState newState)
    {
        CurrentState = newState;
        UpdateUI(newState);
    }

    private void UpdateUI(GameState state)
    {
        // disable all panels first
        if (gameplayPanel) gameplayPanel.SetActive(false);
        //if (petCarePanel) petCarePanel.SetActive(false);
        //if (endingsArchivePanel) endingsArchivePanel.SetActive(false);
        //if (settingsPanel) settingsPanel.SetActive(false);

        // enable the relevant panel based on the current state
        switch (state)
        {
            case GameState.Playing:
                if (gameplayPanel) gameplayPanel.SetActive(true);
                break;
            /*
            case GameState.PetCare:
                if (petCarePanel) petCarePanel.SetActive(true);
                break;
            case GameState.EndingsArchive:
                if (endingsArchivePanel) endingsArchivePanel.SetActive(true);
                break;
            case GameState.Settings:
                if (settingsPanel) settingsPanel.SetActive(true);
                break;
                */
        }
    }

}
