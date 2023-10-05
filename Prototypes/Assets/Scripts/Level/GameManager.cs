using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuRound;

public class GameManager : MonoBehaviour
{
    private GameObject Thomas;
    public static GameManager Instance;
    public bool isBegin { get; set; } = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        CheckBeginner();
        Thomas = GameObject.FindWithTag("Thomas");
        Time.timeScale = 1;
    }
    private void CheckBeginner()
    {
        PlayerPrefs.DeleteAll();
        var checkBegin = PlayerPrefs.GetInt(GameData.BEGIN_GAME);
        if (checkBegin == 0)
        {
            isBegin = true;
            PlayerPrefs.SetInt(GameData.BEGIN_GAME, 1);
        }
        else
            isBegin = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableThomas()
    {
        Thomas.SetActive(true);
    }
}
