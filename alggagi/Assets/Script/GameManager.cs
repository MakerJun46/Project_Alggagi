using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public Level level1 = new Level();

    [Header("UI Text")]
    public Text RemainOpponentCountText;
    public Text playerBallCountText;
    public Text ClickCountText;

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
        LoadingNewLevel();

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
        UpdateText();
        
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

    }


    void UpdateText()
    {
        playerBallCountText.text = "Player Life : " + PlayerBalls.Count;
        ClickCountText.text = "ClickCount : " + PlayerMove.clickNum.ToString();
        RemainOpponentCountText.text = "Remain Opponent Count : " + OpponentBalls.Count;
    }



    // JSON DATA serialize ==============================================================================
    private void LoadingNewLevel()
    {
        level1 = loadData();

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
    public void saveData(object obj)
    {
        string jsonData = JsonUtility.ToJson(obj, true);

        Debug.Log(jsonData);
        File.WriteAllText(Application.dataPath + "/JsonData/jsondata.txt", jsonData);
    }

    public Level loadData()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/JsonData/jsondata.txt");

        Level data = JsonUtility.FromJson<Level>(jsonData);

        return data;
    }
    // JSON DATA serialize ==============================================================================
}
