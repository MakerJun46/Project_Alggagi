using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public Level level1 = new Level();
    public bool isPlayerTurn;
    public bool isgameOver = false;
    public  bool levelWin = false;
    private int level = 1;

    [Header("UI Text")]
    public Text RemainOpponentCountText;
    public Text playerBallCountText;
    public Text ClickCountText;
    public Text GameOverText;
    public Text LevelText;
    public Text RestartText;

    public int MovableCount;
    public int MovableCounttmp;

    [Header("Prefabs")]
    public GameObject Opponent;
    public GameObject Player;
    public GameObject Wall;
    public GameObject AIPlayer;

    public GameObject PlayerParent;
    public GameObject OpponentParent;
    public GameObject WallParent;

    public List<GameObject> OpponentBalls = new List<GameObject>();
    public List<GameObject> PlayerBalls = new List<GameObject>();
    public List<GameObject> Walls = new List<GameObject>();

    public PhysicsManager PM;
    public GameObject GAManager;

    public int findCount = 0;
    public int WinningCount;

    public string[] args;

    public string winOrLose;
    public string resultPath;

    [Header("play Level")]
    public int levelWinRate;

    Slider size;
    Slider mass;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        winOrLose = "Lose";
        WinningCount = 0;
        args = Environment.GetCommandLineArgs();
        MovableCounttmp = MovableCount;
        PM = GetComponent<PhysicsManager>();
        GAManager = GameObject.Find("GAManager");

        size = GameObject.Find("size").GetComponent<Slider>();
        mass = GameObject.Find("mass").GetComponent<Slider>();

        if (args[1] != "-projectpath")
        {
            Debug.LogError($"Excuted path : {args[1]}");


            if (args[3] == "True")
            {
                Debug.LogError("승률 테스트용 게임 시작");
                result playresult = getResult(int.Parse(args[4]));
                Debug.LogError($"가져온 result index : {args[4]}");
                GAManager.GetComponent<GAManager>().TargetDirFind = true;
                GAManager.GetComponent<GAManager>().TargetDir = playresult.TargetDir;
                GAManager.GetComponent<GAManager>().power = playresult.power;
                resultPath = "D:/SourceTree/Project_Alggagi/alggagi/Assets/WinRateTestResult/";
            }
            else
            {
                resultPath = "D:/SourceTree/Project_Alggagi/alggagi/Assets/PlayResult/";
            }
            StartCoroutine(newLevel2(args[1].ToString()));
        }
        else
        {
            StartCoroutine(newLevel(levelWinRate));
        }

    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            gameReset();
            StartCoroutine(newLevel(levelWinRate));
        }

        for (int i = 0; i < OpponentBalls.Count; i++) // null -> 제거
        {
            if (OpponentBalls[i] == null)
            {
                OpponentBalls.RemoveAt(i);
            }
        }

        for (int i = 0; i < PlayerBalls.Count; i++) // null -> 제거
        {
            if (PlayerBalls[i] == null)
            {
                PlayerBalls.RemoveAt(i);
            }
        }

        /*
        if(!OpponentBalls.Any() && !PlayerBalls.Any())
        {
            saveResult(resultPath);
            Application.Quit();
        }
        */
        if(isBallsAllStop())
        {
            if (MovableCount >= 0 && OpponentBalls.Count <= WinningCount)
            {
                Debug.Log("WIN");
                winOrLose = "Win";
                levelWin = true;
                GameOverText.gameObject.SetActive(true);
                GameOverText.text = "WIN!!";
                saveResult(resultPath);
                this.enabled = false;
            }
            else if (PlayerBalls.Count == 0 || MovableCount == 0 && OpponentBalls.Count > WinningCount)
            {
                Debug.Log("LOSE");
                winOrLose = "Lose";
                saveResult(resultPath);
                isgameOver = true;
            }
        }

        if(isgameOver && isBallsAllStop())
        {
            gameOver();
        }
        else
        {
            UpdateText();
        }
    }

    public bool isBallsAllStop()
    {
        Vector3 cmp = new Vector3(0.0f, 0.0f, 0.0f);
        bool isStop = true;

        for(int i = 0; i < OpponentBalls.Count; i++)
        {
            if(OpponentBalls[i].GetComponent<Ball>().v != cmp)
            {
                isStop = false;
                break;
            }
        }

        for(int i = 0; i < PlayerBalls.Count; i++)
        {
            if(PlayerBalls[i].GetComponent<Ball>().v != cmp)
            {
                isStop = false;
                break;
            }
        }

        return isStop;
    }

    public void gameOver()
    {
        Debug.Log("GameOver");
        GameOverText.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(true);
        UpdateText();
        //this.enabled = false;
    }

    public void gameReset()
    {
        isgameOver = false;
        levelWin = false;
        this.enabled = true;
        GameOverText.gameObject.SetActive(false);
        RestartText.gameObject.SetActive(false);

        size.value = 0.3f;
        mass.value = 5;

        GameObject[] leftBalls = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] leftOpponent = GameObject.FindGameObjectsWithTag("Opponent");
        GameObject[] leftWalls = GameObject.FindGameObjectsWithTag("Wall");

        for (int i = 0; i < leftBalls.Length; i++)
        {
            Destroy(leftBalls[i]);
        }
        for(int i = 0; i < leftOpponent.Length; i++)
        {
            Destroy(leftOpponent[i]);
        }
        for (int i = 0; i < leftWalls.Length; i++)
        {
            Destroy(leftWalls[i]);
        }

        OpponentBalls.Clear();
        PlayerBalls.Clear();
        Walls.Clear();

        GameObject go = GameObject.Find("AIPlayer(Clone)");

        Destroy(go);
    }


    void UpdateText()
    {
        playerBallCountText.text = "Player Life : " + PlayerBalls.Count;
        ClickCountText.text = "MovableCount : " + MovableCount.ToString();
        RemainOpponentCountText.text = "Remain Opponent Count : " + OpponentBalls.Count;
        LevelText.text = "Level : " + level.ToString();

    }

    public void findBalls()
    {
        OpponentBalls.Clear();
        PlayerBalls.Clear();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Opponent"))
        {
            OpponentBalls.Add(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerBalls.Add(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Walls.Add(go);
        }
    }



    // JSON DATA serialize ==============================================================================
    public IEnumerator newLevel(int level)
    {
        LoadingNewLevel(level);
        yield return new WaitForEndOfFrame();
        findBalls();
        yield return new WaitForEndOfFrame();
        PM.FindBalls();
    }

    public void LoadingNewLevel(int level)
    {
        level1 = loadData(level);
        for (int i = 0; i < level1.OpponentLocation.Count; i++)
        {
            GameObject go = Instantiate(Opponent, new Vector3(level1.OpponentLocation[i].x, level1.OpponentLocation[i].y, 0.8f),
                            Quaternion.identity, OpponentParent.transform);
            OpponentBalls.Add(go);
        }

        for (int i = 0; i < level1.PlayerLocation.Count; i++)
        {
            GameObject go = Instantiate(Player, new Vector3(level1.PlayerLocation[i].x, level1.PlayerLocation[i].y, 0.8f),
                            Quaternion.identity, PlayerParent.transform);
            PlayerBalls.Add(go);
        }

        for (int i = 0; i < level1.WallLocation.Count; i++)
        {
            GameObject go = Instantiate(Wall, new Vector3(level1.WallLocation[i].x, level1.WallLocation[i].y, 0.8f),
                            Quaternion.identity, WallParent.transform);
            Walls.Add(go);
        }
        MovableCount = MovableCounttmp;
    }


    public void saveData(object obj, int levCount)
    {
        string jsonData = JsonUtility.ToJson(obj, true);

        File.WriteAllText(Application.dataPath + "/JsonData/jsondata_" + levCount.ToString() +  ".txt", jsonData);
    }

    public Level loadData(int index)
    {
        string jsonData = File.ReadAllText("D:/SourceTree/Project_Alggagi/alggagi/Assets/GeneticAlgorithmResult/" + index.ToString() + "_winRateResult.txt");

        Level data = JsonUtility.FromJson<Level>(jsonData);

        return data;
    }
    // JSON DATA serialize ==============================================================================

    public IEnumerator newLevel2(string mapPath)
    {
        LoadingNewLevelString(mapPath);
        yield return new WaitForEndOfFrame();
        findBalls();
        yield return new WaitForEndOfFrame();
        PM.FindBalls();
        yield return new WaitForEndOfFrame();
        Instantiate(AIPlayer, AIPlayer.transform.position, Quaternion.identity);
    }

    public void LoadingNewLevelString(string mapPath)
    {
        level1 = loadData2(mapPath);
        for (int i = 0; i < level1.OpponentLocation.Count; i++)
        {
            GameObject go = Instantiate(Opponent, new Vector3(level1.OpponentLocation[i].x, level1.OpponentLocation[i].y, 0.8f),
                            Quaternion.identity, OpponentParent.transform);
            OpponentBalls.Add(go);
        }

        for (int i = 0; i < level1.PlayerLocation.Count; i++)
        {
            GameObject go = Instantiate(Player, new Vector3(level1.PlayerLocation[i].x, level1.PlayerLocation[i].y, 0.8f),
                            Quaternion.identity, PlayerParent.transform);
            PlayerBalls.Add(go);
        }

        for (int i = 0; i < level1.WallLocation.Count; i++)
        {
            GameObject go = Instantiate(Wall, new Vector3(level1.WallLocation[i].x, level1.WallLocation[i].y, 0.8f),
                            Quaternion.identity, WallParent.transform);
            Walls.Add(go);
        }
        MovableCount = MovableCounttmp;
    }
    public Level loadData2(string mapPath)
    {
        string jsonData = File.ReadAllText("D:/SourceTree/Project_Alggagi/alggagi/Assets/JsonData/jsondata_" + mapPath + ".txt");

        Level data = JsonUtility.FromJson<Level>(jsonData);

        return data;
    }

    public void saveResult(string resultpath)
    {
        result result = new result();

        result.winOrLose = winOrLose;
        result.TargetDir = GameObject.Find("AIPlayer(Clone)").GetComponent<AIPlayer>().dir;
        result.power = GameObject.Find("AIPlayer(Clone)").GetComponent<AIPlayer>().randomPower;
        result.remainBallCount = OpponentBalls.Count;

        string jsonData = JsonUtility.ToJson(result, true);

        Debug.Log("saveResult");
        File.WriteAllText(resultpath + args[2] + "_result.txt", jsonData);

        Application.Quit();
    }


    public result getResult(int index)
    {
        string jsonData = File.ReadAllText("D:/SourceTree/Project_Alggagi/alggagi/Assets/PlayResult/" + index.ToString() + "_result.txt");
        result playResult = UnityEngine.JsonUtility.FromJson<result>(jsonData);

        return playResult;
    }

    IEnumerator autoEscape()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1.0f);
        }
        saveResult(resultPath);
    }
}
