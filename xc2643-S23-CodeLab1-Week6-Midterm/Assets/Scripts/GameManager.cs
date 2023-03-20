using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshPro displayText;
    public float timer = 0;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    private bool inGame = true;

    private int score = 0;

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    private int health = 3;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public List<int> fastestRecords = new List<int>();

    private string FILE_PATH;
    private const string FILE_DIR = "/Data/";
    private const string FILE_NAME = "fastestRecord.txt";
    private void Awake()
    {
        if (!Instance)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    void Start()
    {
        timer = 0;
        FILE_PATH = Application.dataPath + FILE_DIR + FILE_NAME;
    }
    
    void Update()
    {
        if (inGame)
        {
            timer += Time.deltaTime;
            displayText.text =
                "Timer: " + (int)timer + "\n" +
                /*"Score: " + score + "\n" +*/
                "Health: " + health;
        }
    
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (health == 0 && inGame)
        {
            SceneManager.LoadScene("EndScreen");
            Debug.Log("Game OVA!");
            inGame = false;
            ShowFastestRecordes();
        }  
    }

    void ShowFastestRecordes()
    {
        if (fastestRecords.Count == 0) 
        {
            string fileContents = File.ReadAllText(FILE_PATH);
            
            string[] fileSplit = fileContents.Split('\n');
            
            for (int i = 1; i < fileSplit.Length - 1; i++)
            {
                fastestRecords.Add(Int32.Parse(fileSplit[i]));
            }
        }
        
        string fastestRecordStr = "Fast Record:\n";
            
        for (int i = 0; i < fastestRecords.Count; i++)
        {
            fastestRecordStr += fastestRecords[i] + "\n";
        }

        displayText.text = 
            "YOU LOSE\n" +
            fastestRecordStr;
    }
    void UpdateFastestRecords()
    {
        
        if (fastestRecords.Count == 0) 
        {
            string fileContents = File.ReadAllText(FILE_PATH);
            
            string[] fileSplit = fileContents.Split('\n');
            
            for (int i = 1; i < fileSplit.Length - 1; i++)
            {
                fastestRecords.Add(Int32.Parse(fileSplit[i]));
            }
        }

        int intTimer = (int)timer;
        for (int i = 0; i < fastestRecords.Count; i++)
        {
            if (fastestRecords[i] > timer)
            {
                fastestRecords.Insert(i, intTimer);
                break;
            }
        }
        
        fastestRecords.RemoveRange(5, fastestRecords.Count - 5);

        string fastestRecordStr = "Fast Record:\n";
            
        for (int i = 0; i < fastestRecords.Count; i++)
        {
            fastestRecordStr += fastestRecords[i] + "\n";
        }

        displayText.text = fastestRecordStr;
        File.WriteAllText(FILE_PATH, fastestRecordStr);
    }
    
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        health = Mathf.Clamp(health + amount, 0, 3);
    }

    public void EndGame()
    {
        SceneManager.LoadScene("EndScreen");
        Debug.Log("Game OVA!");
        inGame = false;
        UpdateFastestRecords();
    }
    
}
