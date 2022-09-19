using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePresenter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _hightScore;

    void Start()
    {
        _hightScore.text = PlayerPrefs.GetInt("MaxScore").ToString();
    }

}