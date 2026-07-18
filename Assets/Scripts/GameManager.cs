using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Ending,
        PetCare,
        EndingsArchive,
        Settings
    }

    // housekeeping
    public static GameManager Instance { get; private set; }
    public GameState CurrentState;
    public Wolf wolf;
    public TextManager textManager;
    public AudioManager audioManager;

    // game logic stuff
    public List<int> unlockedEndings = new List<int>();
    public int morality = 0;
    public int daysNo = 1;
    public bool hasBranch = false;
    public bool hasCharm = false;


    // panels
    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject endingPanel;
    public GameObject petCarePanel;
    public GameObject settingsPanel;
    public GameObject endingsArchivePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("GameManager Start");
        Instance = this;
//        CurrentState = GameState.MainMenu;
        CurrentState = GameState.Playing;
        UpdateUI(CurrentState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetRun()
    {
        wolf.Reset();
        textManager.Reset();
        morality = 0;
        daysNo = 1;
        hasBranch = false;
        hasCharm = false;
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

    public void GameOver()
    {
        ChangeState(GameState.Ending);
        ResetRun();
    }

    private void UpdateUI(GameState state)
    {
        // disable all panels first
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (gameplayPanel) gameplayPanel.SetActive(false);
        if (endingPanel) endingPanel.SetActive(false);
        if (petCarePanel) petCarePanel.SetActive(false);
        if (endingsArchivePanel) endingsArchivePanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);

        // enable the relevant panel based on the current state
        switch (state)
        {
            case GameState.MainMenu:
                if (mainMenuPanel) mainMenuPanel.SetActive(true);
                break;
            case GameState.Playing:
                if (gameplayPanel) gameplayPanel.SetActive(true);
                break;
            case GameState.Ending:
                if (endingPanel) endingPanel.SetActive(true);
                break;
            case GameState.PetCare:
                if (petCarePanel) petCarePanel.SetActive(true);
                break;
            case GameState.EndingsArchive:
                if (endingsArchivePanel) endingsArchivePanel.SetActive(true);
                break;
            case GameState.Settings:
                if (settingsPanel) settingsPanel.SetActive(true);
                break;
        }
    }

}
