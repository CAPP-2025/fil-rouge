using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// using textmesh pro
using TMPro;

public class Tutorial : MonoBehaviour
{

    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI dialogSkip;
    
    
    void Start() {
        if (GameManager.Instance.isTutorial) {
            StartCoroutine(displayTutorialText());
        } else {
            gameObject.SetActive(false);
        }
    }
    
    public IEnumerator displayTutorialText() {
    
        tutorialText.text = "Welcome to the tutorial! Press the arrow keys to move around.";
        yield return new WaitForSeconds(0.5f);
        dialogSkip.gameObject.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space)) {
            yield return null;
        }

        dialogSkip.gameObject.SetActive(false);
        tutorialText.text = "Press the space bar to jump. (B)";
        yield return new WaitForSeconds(0.5f);
        dialogSkip.gameObject.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space)) {
            yield return null;
        }

        dialogSkip.gameObject.SetActive(false);
        tutorialText.text = "Press the pageDown key to control your fox. (Y)";
        yield return new WaitForSeconds(0.5f);
        dialogSkip.gameObject.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space)) {
            yield return null;
        }

        dialogSkip.gameObject.SetActive(false);
        tutorialText.text = "To enter a door, press the up arrow key.";
        yield return new WaitForSeconds(0.5f);
        dialogSkip.gameObject.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space)) {
            yield return null;
        }

        dialogSkip.gameObject.SetActive(false);
        tutorialText.text = "Avoid the adults and snakes, and collect the roses. Have fun !";
        yield return new WaitForSeconds(0.5f);
        dialogSkip.gameObject.SetActive(true);
        while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.Space)) {
            yield return null;
        }

        gameObject.SetActive(false);
        GameManager.Instance.isTutorial = false;
    }
}
