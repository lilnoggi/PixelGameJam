using System;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    MenuState,
    PauseState,
    PlayState
}

public class GameManager : MonoBehaviour
{

    private GameState currentGameState = GameState.PlayState;
    private GameState previousGameState;

    [Header("Debug")]
    public bool showDebugMessage = false;
    public bool enableDebugKey = false;

    void Update()
    {
        DebugKey();
    }

    private void DebugKey()
    {
        if (enableDebugKey && Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("dedada");
            if (currentGameState == GameState.PlayState)
            {
                ChangeState(GameState.PauseState);
            }
            else if (currentGameState == GameState.PauseState)
            {
                ChangeState(GameState.PlayState);
            }
        }
    }

    public void ChangeState(GameState newGameState)
    {
        if (newGameState == currentGameState)
        {
            return;
        }

        switch (newGameState)
        {
            case GameState.MenuState:
                Time.timeScale = 0f;
                break;
            case GameState.PauseState:
                Time.timeScale = 0f;
                break;
            case GameState.PlayState:
                Time.timeScale = 1f;
                break;
        }

        if (showDebugMessage)
        {
            Debug.Log("Changed Game State: " + previousGameState + " -> " + currentGameState);
        }

        previousGameState = currentGameState;
        currentGameState = newGameState;
    }
}
