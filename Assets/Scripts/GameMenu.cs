using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{

    private bool isPaused = false;

    // on start, hide the children
    void Start() {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        isPaused = false;
    }

    // on update, if the Escape key is pressed, show the object and pause the game
    void Update() {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.PageUp)) {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }
    }

    public void PauseGame() {
        // activate the children
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        // pause the game
        Time.timeScale = 0;
        isPaused = true;
    }
    public void ResumeGame() {
        // hide the children
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        // unpause the game
        Time.timeScale = 1;
        isPaused = false;
    }

    public void RestartGame() {
        // load level Game
        ResumeGame();
        GameManager.Instance.ResetLevel();
    }

    public void MainMenu() {
        // load level StartMenu
        ResumeGame();
        GameManager.Instance.LoadMenu();
    }

    public void QuitGame() {
        // Quit the game
        Application.Quit();
    }
}
