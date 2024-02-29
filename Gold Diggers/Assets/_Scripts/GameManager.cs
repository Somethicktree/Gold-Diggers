using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Tooltip("current score count")]
    [SerializeField]
    int Score;

    private static GameManager instance;
    public List<Player_Movement> players;

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

    public bool CheckAllPlayersReadyToLeave()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].readyToLeave)
            {
                return false;
            }
        }

        return true;
    }

    public void Leave()
    {
        if (CheckAllPlayersReadyToLeave())
        {
            Debug.Log("LEAVE!!!");
        }
    }

    public void GameEnded()
    {

    }
}
