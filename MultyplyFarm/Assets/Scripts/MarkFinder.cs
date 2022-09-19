using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkFinder : MonoBehaviour
{
    [SerializeField] private Transform _spawnField;
    [SerializeField] private PrefabController _prefabController;

    public static int changeColorsCount;
   
    
    public static Color compareColor = new Color32(255, 132, 132, 255);

    public int ScanSpawnField()
    {
        int countMarks = 0;
        foreach (var sr in GetChildColor(_spawnField))
        {
            if (sr.color == compareColor) countMarks++;
        }
        return countMarks;
    }

    public GameObject[] GetMarkedGameObjects()
    {
        List<GameObject> highlightedObjects = new();
        foreach (var sr in GetChildColor(_spawnField))
        {
            if (sr.color == compareColor) highlightedObjects.Add(sr.gameObject);
        }
        return highlightedObjects.ToArray();
    }

    public void ClearMark()
    {
        foreach (var sr in GetChildColor(_spawnField))
        {
            sr.color = Color.white;
        }
        changeColorsCount = 0;
    }

    private SpriteRenderer[] GetChildColor(Transform field)
    {
        SpriteRenderer[] childrens = new SpriteRenderer[field.childCount];
        for (int i = 0; i < field.childCount; i++)
        {
            childrens[i] = field.GetChild(i).GetComponent<SpriteRenderer>();
        }
        return childrens;
    }
}
