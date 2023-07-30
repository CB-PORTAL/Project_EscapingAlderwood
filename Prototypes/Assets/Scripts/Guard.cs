using DuRound;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;

public class Guard : Mabel
{
    private Transform [] movePoints { get; set; }
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Task GuardMove()
    {
        while (isCollide)
        {
            Task.Yield();
        }
        return Task.CompletedTask;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Thomas"))
        {
            isCollide = true;
            collision.gameObject.SetActive(false);
            //TODO update game have Thomas
        }
    }
}
