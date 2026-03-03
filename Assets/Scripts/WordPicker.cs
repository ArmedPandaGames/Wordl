using UnityEngine;
using TMPro;

[System.Serializable]
public class WordList
{
    public string[] words;
}
[System.Serializable]
public class SolutionList
{
    public string[] words;
}
public class WordPicker : MonoBehaviour
{
    public WordList wordList;
    public SolutionList solutionList;
    public string solution;
    void Awake()
    {
        TextAsset textFile = Resources.Load<TextAsset>("Words/words_all");
        wordList.words = textFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        TextAsset solutionFile = Resources.Load<TextAsset>("Words/solutions");
        solutionList.words = solutionFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        PickRandomWord();
    }

    public void PickRandomWord()
    {
        int index = Random.Range(0, solutionList.words.Length);
        solution = solutionList.words[index];
        solution = solution.ToLower().Trim();
    }
}