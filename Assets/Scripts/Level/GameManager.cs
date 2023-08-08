using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject Thomas;
    public static GameManager Instance;
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
        Thomas = GameObject.FindWithTag("Thomas");
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
