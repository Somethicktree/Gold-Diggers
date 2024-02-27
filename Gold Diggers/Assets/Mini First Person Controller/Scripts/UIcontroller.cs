using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    public Text ScoreText;

    private void Start()
    {
        ScoreText = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        ScoreText.text = GameManager.Instance.GetScore().ToString("D3");
    }
}
