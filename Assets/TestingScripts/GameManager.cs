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


    // DebugKey method is used for pausing the game via F1 key (can only be enabled in the Unity Engine)
    private void DebugKey()
    {
        if (enableDebugKey && Input.GetKeyDown(KeyCode.F1))
        {
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
        // checks if the last Game state is the new one     if it is it will return and save us GPU power
        if (newGameState == currentGameState)
        {
            return;
        }


        // adjusts the ingame Time to whatever state is the new one
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


        // Debug Message for state changes (can only be enabled in the Unity Engine)
        if (showDebugMessage)
        {
            Debug.Log("Changed Game State: " + previousGameState + " -> " + currentGameState);
        }


        // adjust the current and previous game states
        previousGameState = currentGameState;
        currentGameState = newGameState;
    }
}
