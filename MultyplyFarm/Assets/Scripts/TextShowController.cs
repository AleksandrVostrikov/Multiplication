using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class TextShowController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _multiplyResult;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _wrongAnswers;

    [SerializeField] private Transform _spawnField;

    public delegate void ChangeDifficult();
    public event ChangeDifficult ChangeLooseTime;
    public event ChangeDifficult MistakesIsFool;

    public void SetMultiplyResult()
    {
        _multiplyResult.text = FindMultiplyResultText();
    }
    public int MyltiplyResult
    {
        get { return Convert.ToInt32(_multiplyResult.text); }
    }
    public int Score
    {
        get { return Convert.ToInt32(_score.text); }
        set 
        { 
            _score.text = value.ToString();
            if (value % 100 == 0) ChangeLooseTime?.Invoke();
        }
    }
    public int WrongAnswers
    {
        get { return Convert.ToInt32(_wrongAnswers.text); }
        set
        {
            _wrongAnswers.text = value.ToString();
            if (value == 3) MistakesIsFool?.Invoke();
        }
    }
    private string FindMultiplyResultText()
    {
        List<int> numbers = new();
        for (int i = 0; i < _spawnField.childCount; i++)
        {
            numbers.Add(Convert.ToInt32(_spawnField.GetChild(i).Find("Canvas").Find("number").GetComponent<TextMeshProUGUI>().text));
        }
        int a, b, temp;
        temp = Random.Range(0, numbers.Count - 1);
        a = numbers[temp];
        numbers.RemoveAt(temp);
        b = numbers[Random.Range(0, numbers.Count - 1)];
        return (a * b).ToString();
    }
}
