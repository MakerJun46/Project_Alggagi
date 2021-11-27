using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class TestShakespeare : MonoBehaviour
{
    [Header("Genetic Algorithm")]
    [SerializeField] int populationSize;
    [SerializeField] float mutationRate;
    [SerializeField] int elitism;

    [Header("UI")]
    [SerializeField] Text TargetWinRate;
    [SerializeField] Text Generation;
    [SerializeField] Text BestGeneFitness;

    [Header("GameTest")]
    [SerializeField] int DirTestPlayCount;
    [SerializeField] int WinRateTestplayCount;
    [SerializeField] int WinRateGoal;

    [Header("Level Info")]
    [SerializeField] int BallCount;
    [SerializeField] int WallCount;

    [Header("Other Info")]
    public GameObject Board;

    [Header("Auto Player")]
    public GameObject AIPlayer;

    private GeneticAlgorithm<Location> ga;
    private GameObject GAManager;
    private System.Random random;

    public bool GamePlaying;

    private int dnaSize;
    public int playResult;


    int count = 0;

    void Start()
    {
        playResult = 0;
        GAManager = GameObject.Find("GAManager");
        random = new System.Random();
        ga = new GeneticAlgorithm<Location>(populationSize, BallCount + WallCount, random, GetRandomLocation, FitnessFunction, elitism, mutationRate);
        new WaitForSeconds(2.0f);
        TargetWinRate.text = "Target WinRate : " + WinRateGoal.ToString();
    }

    void Update()
    {
        if (ga.GenerationEnd)
        {
            ga.NewGeneration();
            Debug.Log($"playCount : {count}");
            Debug.Log($"Best Fitness : {ga.BestFitness}");

            Generation.text = "Generation : " + ga.Generation.ToString();
            BestGeneFitness.text = "BestGene Fitness : " + ga.BestFitness.ToString();

            if (ga.BestFitness >= 1 &&  ga.BestFitness <= 2)
            {
                CreateGAResult(ga.BestGenes);
                Debug.Log("BestFitness Founded");
                this.enabled = false;
            }
            count++;
        }
    }

    private Location GetRandomLocation()
    {

        int width = (int)((Board.transform.localScale.x / 2 - 1) / 0.3f);
        int height = (int)((Board.transform.localScale.y / 2 - 1) / 0.3f);

        float x = (float)random.Next(-width, width) * 0.3f;
        float y = (float)random.Next(-height, height) * 0.3f;

        Location loc = new Location(x, y);

        return loc;
    }

    private float FitnessFunction(int index)
    {
        confirmDnaOverlap(index);
        float score = 0;
        int WinCount = 0;

        bool FindAnswer = false;
        int resultIndex = 0;

        DateTime[] excuteTime = new DateTime[DirTestPlayCount];
        string resultPath = "D:/SourceTree/Project_Alggagi/alggagi/Assets/PlayResult/";

        DNA<Location> dna = ga.Population[index];

        CreateNewLevel(dna, 1);

        for (int i = 0; i < DirTestPlayCount; i++)      // Find Answer
        {
            runGame(1, i);
            excuteTime[i] = DateTime.Now;
        }

        int testIndex = 0;

        DateTime MaxWaitTime = DateTime.Now.AddSeconds(20);

        while (true)		// Confirm Results
        {
            DateTime resultCreateTime = File.GetCreationTime(resultPath + testIndex.ToString() + "_result.txt");

            if (resultCreateTime > excuteTime[testIndex])
            {
                testIndex++;

                if (testIndex == DirTestPlayCount)
                {
                    break;
                }
            }
            Thread.Sleep(1000);

            if (DateTime.Now > MaxWaitTime)
            {
                break;
            }
        }

        for (int i = 0; i < DirTestPlayCount; i++)
        {
            result playResult = getResult(i);

            if (playResult.winOrLose == "Win")
            {
                //WinCount++; // 0913 방향 테스트 자체를 승률 테스트로 보는 경우
                FindAnswer = true;
                resultIndex = i;
            }
        }
        
        MaxWaitTime = DateTime.Now.AddSeconds(15);

        if (FindAnswer)
        {
            for (int i = 0; i < WinRateTestplayCount; i++)
            {
                runGame(1, i, true, resultIndex);
                excuteTime[i] = DateTime.Now;
            }

            while (true)        // Confirm Results
            {
                DateTime resultCreateTime = File.GetCreationTime(resultPath + testIndex.ToString() + "_result.txt");

                if (resultCreateTime > excuteTime[testIndex])
                {
                    testIndex++;

                    if (testIndex == WinRateTestplayCount)
                    {
                        break;
                    }
                }
                Thread.Sleep(1000);

                if (DateTime.Now > MaxWaitTime)
                {
                    break;
                }
            }

            for (int i = 0; i < WinRateTestplayCount; i++)
            {
                result playresult = getWinRateResult(i);
                if (playresult.winOrLose == "Win")
                {
                    WinCount++;
                }
            }
        }
        else
        {
            return 0;	// 정답 way가 없는 경우 fitness : 0
        }
        
        Debug.Log($"{ga.Generation}세대 {index}번째 검사 결과 승리 수 : {WinCount}");

        int WinRateResult = WinCount * 10;

        score = (float)Math.Pow(Math.E, -(WinRateGoal - WinRateResult));

        return score;
    }

    private void CreateNewLevel(DNA<Location> dna, int index)
    {
        List<Location> tmp = new List<Location>();
        foreach (Location loc in dna.Genes)
        {
            tmp.Add(loc);
        }

        Level lev = new Level(tmp, WallCount);

        saveData(lev, index);
    }
    public void runGame(int levelNum, int resultNum, bool isWinRateTest = false, int resultIndex = 999)
    {
        string path = "D:/SourceTree/Project_Alggagi/buildtest/0823_v1/alggagi_0713.exe";
        string args = levelNum.ToString() + " " + resultNum.ToString() + " " + isWinRateTest.ToString() + " " + resultIndex.ToString();

        Process.Start(path, args);
    }

    public result getResult(int index)
    {
        string jsonData = File.ReadAllText("D:/SourceTree/Project_Alggagi/alggagi/Assets/PlayResult/" + index.ToString() + "_result.txt");
        result playResult = UnityEngine.JsonUtility.FromJson<result>(jsonData);

        return playResult;
    }
    public result getWinRateResult(int index)
    {
        string jsonData = File.ReadAllText("D:/SourceTree/Project_Alggagi/alggagi/Assets/WinRateTestResult/" + index.ToString() + "_result.txt");
        result playResult = UnityEngine.JsonUtility.FromJson<result>(jsonData);

        return playResult;
    }

    public void CreateGAResult(Location[] locations)
    {
        List<Location> tmp = new List<Location>();

        foreach (Location loc in locations)
        {
            tmp.Add(loc);
        }

        Level lev = new Level(tmp, WallCount);

        string jsonData = JsonUtility.ToJson(lev, true);
        File.WriteAllText(Application.dataPath + "/GeneticAlgorithmResult/" + WinRateGoal.ToString() + "_winRateResult.txt", jsonData);
    }

    public void confirmDnaOverlap(int index)
    {
        DNA<Location> tmp = ga.Population[index];

        for (int i = 0; i < tmp.Genes.Length; i++)
        {
        confirm:
            Location[] cmp = Array.FindAll<Location>(tmp.Genes, x => x == tmp.Genes[i]);
            if (cmp.Length > 1)  // 중복 값이 2개 이상 있다.
            {
                // 중복 값이 있으면 해당 유전자의 위치(index)를 찾아서 radomLocation으로 대체
                ga.Population[index].Genes[Array.FindIndex<Location>
                (ga.Population[index].Genes, x => x == ga.Population[index].Genes[i])] = GetRandomLocation();
                goto confirm;
            }
        }
    }
    public void saveData(object obj, int levCount)
    {
        string jsonData = JsonUtility.ToJson(obj, true);

        File.WriteAllText(Application.dataPath + "/JsonData/jsondata_" + levCount.ToString() + ".txt", jsonData);
    }
}