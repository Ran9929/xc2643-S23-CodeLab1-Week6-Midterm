using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ASCIILevelLoadScript : MonoBehaviour
{
    public static ASCIILevelLoadScript Instance;
    public GameObject player;
    public GameObject wall;
    public GameObject enemy;
    public GameObject treasure;
    public GameObject damageZone;
    
    GameObject currentPlayer;
    GameObject level;
    
    int currentLevel = 0;

    public int CurrentLevel
    {
        get { return currentLevel; }
        set
        {
            currentLevel = value;
            LoadLevel();
        }
    }

    private int score;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            ChangeLevel();
        }
    }

    const string FILE_NAME = "LevelNum.txt";
    const string FILE_DIR = "/Levels/";
    string FILE_PATH;

    public float xOffset;
    public float yOffset;

    public Vector2 playerStartPos;
    
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
        FILE_PATH = Application.dataPath + FILE_DIR + FILE_NAME;

        LoadLevel();
    }

    bool LoadLevel()
    {
        Destroy(level);
        
        level = new GameObject("Level");
        
        string newPath = FILE_PATH.Replace("Num", currentLevel + "");
        
        //load all the lines of the file into an array of strings
        string[] fileLines = File.ReadAllLines(newPath);

        //for loop to go through each line
        for (int yPos = 0; yPos < fileLines.Length; yPos++)
        {
            //get each line out of the array
            string lineText = fileLines[yPos];

            //turn the current line into an array of chars
            char[] lineChars = lineText.ToCharArray();

            //loop through each char
            for (int xPos = 0; xPos < lineChars.Length; xPos++)
            {
                //get the current char
                char c = lineChars[xPos];

                //make a variable for a new GameObject
                GameObject newObj;

                switch (c)
                {
                    case 'p':  //is the char a p?
                        playerStartPos = new Vector2(xOffset + xPos, yOffset - yPos);
                        newObj = Instantiate<GameObject>(player); //make a new player
                        currentPlayer = newObj;
                        break;
                    case 'w': //is the char a w?
                        newObj = Instantiate<GameObject>(wall);
                        break;
                    case 't': //is there a carrot?
                        newObj = Instantiate<GameObject>(treasure); 
                        break;
                    case 'e':
                        newObj = Instantiate<GameObject>(enemy);
                        break;
                    case 'd':
                        newObj = Instantiate<GameObject>(damageZone);
                        break;
                    default: //otherwise
                        newObj = null; //null
                        break;
                }

                //if we made a new GameObject
                if (newObj != null)
                {
                    //position it based on where it was
                    //in the file, using the variables
                    //we used to loop through our arrays
                    newObj.transform.position =
                        new Vector2(
                            xOffset + xPos, 
                            yOffset - yPos);

                    newObj.transform.parent = level.transform;
                }
            }
        }

        return false;
    }

    public void ResetPlayer()
    {
        currentPlayer.transform.position = playerStartPos;
    }

    public void ChangeLevel()
    {
        if (score == (CurrentLevel + 1) * 3 && score < 14)
        {
            CurrentLevel++;
        }
        
    }

    public void ChangeScore(int amount)
    {
        Score = Mathf.Clamp(Score + amount, 0, 16);
        if (score == 15)
        {
            GameManager.Instance.EndGame();
        }
    }

    void Update()
    {
        
    }
}
