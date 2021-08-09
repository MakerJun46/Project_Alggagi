using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public Level level1 = new Level();
    public bool isPlayerTurn;
    private bool isgameOver = false;
    private bool levelWin = false;
    private int level = 1;

    [Header("UI Text")]
    public Text RemainOpponentCountText;
    public Text playerBallCountText;
    public Text ClickCountText;
    public Text GameOverText;
    public Text LevelText;

    [Header("Prefabs")]
    public GameObject Opponent;
    public GameObject Player;

    public GameObject PlayerParent;
    public GameObject OpponentParent;

    public List<GameObject> OpponentBalls = new List<GameObject>();
    public List<GameObject> PlayerBalls = new List<GameObject>();

    PhysicsManager PM;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PM = GetComponent<PhysicsManager>();
        LoadingNewLevel(level);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Opponent"))
        {
            OpponentBalls.Add(go);
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerBalls.Add(go);
        }
        PM.FindBalls();
    }


    void Update()
    {
        if(PlayerBalls.Count == 0 && OpponentBalls.Count > 0)
        {
            isgameOver = true;
        }
        else if(PlayerBalls.Count > 0 && OpponentBalls.Count == 0)
        {
            levelWin = true;
            level++;

            GameObject [] leftBalls = GameObject.FindGameObjectsWithTag("Player");

            for(int i = 0; i < leftBalls.Length; i++)
            {
                Destroy(leftBalls[i]);
            }

            StartCoroutine(newLevel(level));

            /*
            LoadingNewLevel(level);
            findBalls();
            PM.FindBalls();
            */
        }

        if(isgameOver)
        {
            GameOverText.gameObject.SetActive(true);
            return;
        }
        else
        {
            UpdateText();
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
        // if player ball fall down, respawn new player ball ============================================
        if(PlayerBalls.Count == 0)
        {
            level1 = loadData();

            for (int i = 0; i < level1.PlayerLocation.Count; i++)
            {
                Instantiate(Player, new Vector3(level1.PlayerLocation[i].x, level1.PlayerLocation[i].y, 0.8f),
                                Quaternion.identity, PlayerParent.transform);
            }

            PM.FindBalls();
        }
        // if player ball fall down, respawn new player ball ============================================
        */
    }


    void UpdateText()
    {
        playerBallCountText.text = "Player Life : " + PlayerBalls.Count;
        ClickCountText.text = "ClickCount : " + PlayerMove.clickNum.ToString();
        RemainOpponentCountText.text = "Remain Opponent Count : " + OpponentBalls.Count;
        LevelText.text = "Level : " + level.ToString();
    }

    void findBalls()
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

    private void LoadingNewLevel(int level)
    {
        level1 = loadData(level);

        for (int i = 0; i < level1.OpponentLocation.Count; i++)
        {
            Instantiate(Opponent, new Vector3(level1.OpponentLocation[i].x, level1.OpponentLocation[i].y, 0.8f),
                            Quaternion.identity, OpponentParent.transform);
        }

        for (int i = 0; i < level1.PlayerLocation.Count; i++)
        {
            Instantiate(Player, new Vector3(level1.PlayerLocation[i].x, level1.PlayerLocation[i].y, 0.8f),
                            Quaternion.identity, PlayerParent.transform);
        }
    }
    public void saveData(object obj, int levCount)
    {
        string jsonData = JsonUtility.ToJson(obj, true);

        Debug.Log(jsonData);
        File.WriteAllText(Application.dataPath + "/JsonData/jsondata_" + levCount.ToString() +  ".txt", jsonData);
    }

    public Level loadData(int index)
    {
        Debug.Log($"loadNew Data : level : {index}");
        string jsonData = File.ReadAllText(Application.dataPath + "/JsonData/jsondata_" + index.ToString() + ".txt");

        Level data = JsonUtility.FromJson<Level>(jsonData);

        return data;
    }
    // JSON DATA serialize ==============================================================================
}
