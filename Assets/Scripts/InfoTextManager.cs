using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum InfoTextState
{
    Welcome,
    WordNotFound,
    NotEnoughLetters,
    LevelUp,
    Clear
}
public class InfoTextManager : MonoBehaviour
{
    private TMP_Text infoText;
    // Start is called before the first frame update
    void Start()
    {
        infoText = GetComponent<TMP_Text>();
        ChooseText(InfoTextState.Welcome);
    }

    public void ChooseText(InfoTextState state)
    {
        switch (state)
        {
            case InfoTextState.Welcome:
                infoText.text = "Good luck guessing!";
                StartCoroutine(ClearInfoTextAfterDelay(2f));
                break;
            case InfoTextState.WordNotFound:
                infoText.text = "Word not found!";
                StartCoroutine(ClearInfoTextAfterDelay(2f));
                break;
            case InfoTextState.NotEnoughLetters:
                infoText.text = "Not enough letters!";
                StartCoroutine(ClearInfoTextAfterDelay(2f));
                break;
            case InfoTextState.LevelUp:
                infoText.text = "Level Up!";
                StartCoroutine(ClearInfoTextAfterDelay(2f));
                break;
            case InfoTextState.Clear:
                infoText.text = "";
                break;
        }
    }

    private IEnumerator ClearInfoTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChooseText(InfoTextState.Clear);
    }


}
