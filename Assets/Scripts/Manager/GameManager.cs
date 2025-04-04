using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public bool isGameOver;

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameClear()
    {

    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("GameOver!!!!");
    }
}
