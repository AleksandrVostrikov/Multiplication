using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Loose : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _score;
    void Awake()
    {
        _score.text = PlayerPrefs.GetInt("MidScore").ToString();
    }

    private void Update()
    {
        Exit();
    }
    private void Exit()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);
    }
   
}
