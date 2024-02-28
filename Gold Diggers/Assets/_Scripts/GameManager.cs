using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("current score count")]
    [SerializeField]
    int Score;

    private static GameManager instance;

    public static GameManager Instance
    {
        get { return instance; }
    }

    public int GetScore()
    {
        return Score;
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void changeScore(int deltaScore)
    {
        Debug.Log("Scored");
        Score += deltaScore;
    }
}
