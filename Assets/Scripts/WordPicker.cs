using UnityEngine;
using TMPro;

[System.Serializable]
public class WordList
{
    public string[] words;
}

public class WordPicker : MonoBehaviour
{
    //public TMP_Text wordDisplayText;

    public WordList wordList;
    public string solution;
    void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Words/words");
        wordList = JsonUtility.FromJson<WordList>(jsonFile.text);

        PickRandomWord();
    }

    public void PickRandomWord()
    {
        int index = Random.Range(0, wordList.words.Length);
        //wordDisplayText.text = wordList.words[index];
        solution = wordList.words[index];
        solution = solution.ToLower().Trim();
    }
}