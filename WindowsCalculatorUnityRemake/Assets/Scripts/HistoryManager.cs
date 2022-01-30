using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HistoryManager : MonoBehaviour
{
    [SerializeField] private GameObject historyItemPrefab;
    private RectTransform rt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        UpdateHeight();
    }

    private void Update()
    {
        UpdateHeight();
    }

    private void UpdateHeight()
    {
        if (transform.childCount * 100 <= 290)
        {
            rt.sizeDelta = new Vector2(0, 290);
        }
        else
        {
            rt.sizeDelta = new Vector2(0, transform.childCount * 100);
        }
    }

    public void AddHistoryItem(string equation, string answer)
    {
        GameObject newItem = Instantiate(historyItemPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newItem.transform.SetParent(transform);
        newItem.transform.GetChild(0).GetComponent<TMP_Text>().text = equation;
        newItem.transform.GetChild(1).GetComponent<TMP_Text>().text = answer;
    }

    public void DeleteHistory()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        UpdateHeight();
    }
}
