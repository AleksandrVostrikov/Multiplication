using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class MainGameController : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private Transform _spawnObject;
    [SerializeField] private Transform _spawnField;
    [SerializeField] private GameObject _pauseImage;
    
    [SerializeField] private MarkFinder _markFinder;
    [SerializeField] private PrefabController _prefabController;
    [SerializeField] private TextShowController _textShowController;
    [SerializeField] private Slider _slider;

    private int _currentcount = 0;
    private float _timeLeft = 10f;
    private float _maxLooseTime = 10;
    private bool _waitSpawnObjects = true;
    private bool _waitNewMultiplyNumber = false;
    private bool _firstStart = false;
    private bool _looseTimer = false;

    public static bool pause = false;



    private void Start()
    {
        _textShowController.ChangeLooseTime += ReduceLooseTime;
        _textShowController.MistakesIsFool += Restart;
    }

    
    private void Update()
    {
        CheckPause();
        ExitGame();

        if (_looseTimer) Restart();

        if (_currentcount == _count) { }
        else if (_waitSpawnObjects) StartCoroutine(SpawnObjects());

        if (_waitNewMultiplyNumber) StartCoroutine(FindMultiplyResult());

        if (_spawnField.childCount < _count) _currentcount = _spawnField.childCount;

        if (MarkFinder.changeColorsCount == 2 & _currentcount == _count) CheckResults();
    }

    private void ExitGame()
    {
        WriteMaxScore();
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0); ;
    }

    private void WriteMaxScore()
    {
        int _maxScore = PlayerPrefs.GetInt("MaxScore");
        if (_textShowController.Score > _maxScore) PlayerPrefs.SetInt("MaxScore", _textShowController.Score);
        PlayerPrefs.SetInt("MidScore", _textShowController.Score);
    }

    private void Restart()
    {
        WriteMaxScore();
        SceneManager.LoadScene(2);
        MarkFinder.changeColorsCount = 0;
    }

    private void ReduceLooseTime()
    {
        if (_maxLooseTime > 4) _maxLooseTime -= 2;
        _slider.maxValue = _maxLooseTime;
    }
    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Space) & pause == false)
        {
            pause = true;
            Time.timeScale = 0f;
            _pauseImage.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Space) & pause == true)
        {
            pause = false;
            Time.timeScale = 1f;
            _pauseImage.SetActive(false);
        }
    }

    private void CheckResults()
    {
        GameObject[] highlightedObjects = _markFinder.GetMarkedGameObjects();
        int[] multipliers = new int[2];
        multipliers[0] = Convert.ToInt32(highlightedObjects[0].name.Remove(1));
        multipliers[1] = Convert.ToInt32(highlightedObjects[1].name.Remove(1));
        if (_textShowController.MyltiplyResult == multipliers[0] * multipliers[1])
        {
            _waitNewMultiplyNumber = false;
            StopAllCoroutines();
            _timeLeft = _maxLooseTime;
            StartCoroutine(Destroer());
        }
        else
        {
            _markFinder.ClearMark();
            _textShowController.WrongAnswers += 1;
        }
    }

    private IEnumerator Destroer()
    {
        GameObject[] highlightedObjects = _markFinder.GetMarkedGameObjects();
        _markFinder.ClearMark();
        foreach (var go in highlightedObjects)
        {
            go.GetComponentInChildren<ParticleSystem>().Play();
            go.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            go.GetComponentInChildren<Canvas>().enabled = false;
        }
        yield return new WaitForSeconds(1);
        Destroy(highlightedObjects[0]);
        Destroy(highlightedObjects[1]);
        _waitNewMultiplyNumber = true;
        _textShowController.Score += 10;
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            _slider.value = _timeLeft;
            if (_slider.value == 0) _looseTimer = true;
            yield return null;
        }
    }

    private IEnumerator SpawnObjects()
    {
        _waitSpawnObjects = false;
        _prefabController.PrefabNumber = Random.Range(1, 10);
        Instantiate(_spawnObject, CalculatePosition(), Quaternion.identity, _spawnField);
        _currentcount++;

        yield return new WaitForSeconds(0.3f);

        if (_currentcount == _count && !_firstStart)
        {
            _waitNewMultiplyNumber = true;
            _firstStart = true;
            StartCoroutine(Timer());
        }
        _waitSpawnObjects = true;
    }

    private Vector2 CalculatePosition()
    {
        return new Vector2(Random.Range(1, 45) * Random.Range(-1, 2), 4 + Random.Range(1, 20));
    }

    private IEnumerator FindMultiplyResult()
    {
        _waitNewMultiplyNumber = false;
        _textShowController.SetMultiplyResult();
        yield return new WaitForSeconds(10f);
        _markFinder.ClearMark();
        _waitNewMultiplyNumber = true;
    }
}
