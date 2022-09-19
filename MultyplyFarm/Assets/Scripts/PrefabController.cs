using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class PrefabController : MonoBehaviour
{
    private bool _isCorutineActive = false;
    
    private float _midPosition;
    private float _angleY = 0;

    private Color markColor = MarkFinder.compareColor;
    
    private void Update()
    {
        SaveRotation();
        if (!_isCorutineActive) StartCoroutine(CheckDirection());
        
    }

    private void SaveRotation()
    {
        transform.rotation = Quaternion.Euler(0, _angleY, 0);
        transform.Find("Canvas").Find("number").rotation = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator CheckDirection()
    {
        _isCorutineActive = true;
        _midPosition = transform.position.x;
        yield return new WaitForSeconds(0.2f);
        if (transform.position.x - _midPosition > 1.5f)
        {
            _angleY = 180;
        }
        else if (transform.position.x - _midPosition < -1.5f)
        {
            _angleY = 0;
        }
        _isCorutineActive = false;
    }

    public int PrefabNumber
    {
        set
        {
            GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
            name = value.ToString();
        }
    }

    private void OnMouseDown()
    {
        if (MainGameController.pause == false)
        {
            Color currentColor = GetComponent<SpriteRenderer>().color;
            if (currentColor == Color.white & MarkFinder.changeColorsCount < 2)
            {
                GetComponent<SpriteRenderer>().color = markColor;
                MarkFinder.changeColorsCount += 1;
            }
            else if (currentColor == markColor)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                MarkFinder.changeColorsCount -= 1;
            }
        }

    }
}
