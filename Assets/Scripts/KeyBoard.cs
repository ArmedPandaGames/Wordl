using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KeyState
{
    Default,
    Correct,
    WrongPosition,
    Wrong
}
public class KeyBoard : MonoBehaviour
{
    private KeyState[] keyStates = new KeyState[26]; // Array to hold the state of each key (A-Z)
    public Color[] stateColors = new Color[4]; // Colors for each state: Default, Correct, WrongPosition, Wrong
    public Button[] keyButtons = new Button[26]; // Array of UI Buttons representing the keys

    void Start()
    {
        // Initialize all keys to the default state
        for (int i = 0; i < keyStates.Length; i++)
        {
            keyStates[i] = KeyState.Default;
            UpdateKeyColor(i);
        }
    }

    public void SetKeyState(char letter, KeyState state)
    {
        int index = char.ToUpper(letter) - 'A'; // Convert letter to index (0-25)
        if (index >= 0 && index < keyStates.Length)
        {
            // Only update if the new state has higher priority than the current one.
            // Priority: Correct > WrongPosition > Wrong > Default
            KeyState current = keyStates[index];
            if (GetPriority(state) > GetPriority(current))
            {
                keyStates[index] = state;
                UpdateKeyColor(index);
            }
        }
    }

    private int GetPriority(KeyState s)
    {
        switch (s)
        {
            case KeyState.Correct: return 3;
            case KeyState.WrongPosition: return 2;
            case KeyState.Wrong: return 1;
            default: return 0;
        }
    }

    private void UpdateKeyColor(int index)
    {
        if (index >= 0 && index < keyButtons.Length && keyButtons[index] != null)
        {
            Image image = keyButtons[index].GetComponent<Image>();
            if (image != null)
            {
                image.color = stateColors[(int)keyStates[index]];
            }
        }
    }

    public void ResetKeyboard()
    {
        for (int i = 0; i < keyStates.Length; i++)
        {
            keyStates[i] = KeyState.Default;
            UpdateKeyColor(i);
        }
    }
}
