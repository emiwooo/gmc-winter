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
        Inventory
    }

    // housekeeping
    public static GameManager Instance { get; private set; }
    public GameState CurrentState;
    public Wolf wolf;
    public TextManager textManager;
    public AudioManager audioManager;

    // game logic stuff
    public List<int> unlockedEndings = new List<int>();
    public List<string> inventory = new List<string>();


    // panels
    public GameObject mainMenuPanel;
    public GameObject gameplayPanel;
    public GameObject endingPanel;
    public GameObject petCarePanel;
    public GameObject endingsArchivePanel;
    public GameObject inventoryPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        CurrentState = GameState.MainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetRun()
    {
        wolf.Reset();
        textManager.Reset();
        inventory.Clear();
    }

    public void AddEnding(int endingId)
    {
        if (!unlockedEndings.Contains(endingId))
        {
            unlockedEndings.Add(endingId);
        }
    }

    public void AddToInventory(string item)
    {
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
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
        if (inventoryPanel) inventoryPanel.SetActive(false);

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
            case GameState.Inventory:
                if (inventoryPanel) inventoryPanel.SetActive(true);
                break;
        }
    }

}
