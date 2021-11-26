using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private TextMeshProUGUI _text;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        _text.text = _player.GetComponent<Move>().ToString() +
                     _player.GetComponent<Jump>().ToString();
    }
}
