using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIPlayer : MonoBehaviour
{
    System.Random random = new System.Random();
    public GameObject GeneticAlgorithm;
    GameObject GAManager;

    public bool Shooting = false;
    public int result;
    //public int index;
    public int TargetIndex;

    public bool TargetFind = false;
    public bool waitDestroy = false;

    public bool coroutineActive = false;

    public int findcount = 0;

    public Vector3[] dir;
    public Vector3 power = new Vector3(0, 0, 0);
    public int[] randomPower;

    float m_ai;
    Vector3 a_ai, v_ai;

    // Start is called before the first frame update
    void Start()
    {
        dir = new Vector3[GameManager.instance.MovableCount];
        randomPower = new int[GameManager.instance.MovableCount];


        GAManager = GameObject.Find("GAManager");
        if (GAManager.GetComponent<GAManager>().TargetDirFind)
        {
            dir = GAManager.GetComponent<GAManager>().TargetDir;
            randomPower = GAManager.GetComponent<GAManager>().power;
            Debug.LogError("randomPower : " + randomPower);
            StartCoroutine(Playing());
        }
        else
        {
            StartCoroutine(FindTarget());
        }
    }
    public void TargetFound()
    {
        // 타겟 방향을 아직 찾지 못했고, levelwin이라면 (방향을 찾은 시점)
        if (!GAManager.GetComponent<GAManager>().TargetDirFind && GameManager.instance.levelWin)
        {
            GAManager.GetComponent<GAManager>().TargetDirFind = true;
            GAManager.GetComponent<GAManager>().TargetDir = dir;
            GAManager.GetComponent<GAManager>().power = randomPower;
            GAManager.GetComponent<GAManager>().TargetIndex = TargetIndex;
            Debug.LogError("TargetFind - aiplayer");
        }
    }
      
    void Update()
    {
        TargetFound();
    }

    public IEnumerator Playing()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.instance.PM.FindBalls();
        yield return new WaitForSeconds(1.0f);

        int index = 0;

        while (GameManager.instance.MovableCount != 0 && GameManager.instance.PlayerBalls.Count != 0)
        {
            AttackTarget(index);
            Debug.LogError("PlayIndex : " + index);
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(1.0f);
                if (GameManager.instance.isBallsAllStop())
                {
                    index++;
                    break;
                }
            }
        }
    }

    public IEnumerator FindTarget()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.instance.PM.FindBalls();
        yield return new WaitForSeconds(1.0f);
        //FindAnswerDir();
        int index = 0;
        
        while (GameManager.instance.MovableCount != 0)
        {
            FindAnswerDir(index);
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(1.0f);
                if (GameManager.instance.isBallsAllStop())
                {
                    index++;
                    break;
                }
            }
        }
        



        /*
        for(int i = 0; i < 5; i ++)
        {
            yield return new WaitForSeconds(1.0f);
            if(GameManager.instance.isBallsAllStop())
            {
                break;
            }
        }

        if(GameManager.instance.MovableCount != 0)
        {
            StartCoroutine(FindTarget());
        }
        */
    }

    private void FindAnswerDir(int index)
    {
        findcount++;
        Debug.Log($"TargetFinding.. : {findcount}");
        GameManager.instance.findBalls();

        /*
        float MinDistance = 10000.0f;
        Vector3 playerPosition = GameManager.instance.PlayerBalls[0].transform.position;

        for (int i = 0; i < GameManager.instance.OpponentBalls.Count; i++)
        {
            Vector3 offset = GameManager.instance.OpponentBalls[i].transform.position - playerPosition;
            float tmpDist = offset.sqrMagnitude;

            if (MinDistance > tmpDist) // 최소 거리인 하나의 오브젝트 찾기
            {
                MinDistance = tmpDist;
                TargetIndex = i;
            }
        }
        */

        TargetIndex = random.Next(GameManager.instance.OpponentBalls.Count + GameManager.instance.Walls.Count);

        float xDirOffset = (float)random.NextDouble() / 6.25f;
        float yDirOffset = (float)random.NextDouble() / 6.25f;

        xDirOffset = random.NextDouble() > 0.5 ? xDirOffset : xDirOffset * -1f;
        yDirOffset = random.NextDouble() > 0.5 ? yDirOffset : xDirOffset * -1f;

        m_ai = GameManager.instance.PlayerBalls[0].GetComponent<Ball>().m;
        v_ai = GameManager.instance.PlayerBalls[0].GetComponent<Ball>().v;

        Transform target;
        if(TargetIndex < GameManager.instance.OpponentBalls.Count)
        {
            target = GameManager.instance.OpponentBalls[TargetIndex].transform;
        }
        else
        {
            TargetIndex -= GameManager.instance.OpponentBalls.Count;
            target = GameManager.instance.Walls[TargetIndex].transform;
        }

        Vector3 targetDirOffset = new Vector3(target.position.x + xDirOffset, target.position.y + yDirOffset, target.position.z);

        dir[index] = (targetDirOffset - GameManager.instance.PlayerBalls[0].transform.position).normalized;

        randomPower[index] = random.Next(10, 200);
        power = dir[index] * randomPower[index];

        a_ai = power / m_ai;
        v_ai += a_ai;

        GameManager.instance.PlayerBalls[0].GetComponent<Ball>().v = v_ai;

        GameManager.instance.MovableCount--;
    }

    private void AttackTarget(int index) // target이 정해져 있는 경우 attack 옵션
    {
        GameManager.instance.findBalls();

        m_ai = GameManager.instance.PlayerBalls[0].GetComponent<Ball>().m;
        v_ai = GameManager.instance.PlayerBalls[0].GetComponent<Ball>().v;

        int randomPowerOffset = random.Next(-5, 5);
        Vector3 randomDirOffset = new Vector3((float)random.NextDouble() * 0.2f * random.Next(-1, 1), (float)random.NextDouble() * 0.2f * random.Next(-1, 1), 0);
        Vector3 tmp = new Vector3(0, 0, 0);

        randomPower[index] += randomPowerOffset;
        dir[index] += randomDirOffset;

        power = dir[index] * randomPower[index];

        a_ai = power / m_ai;
        v_ai += a_ai;

        GameManager.instance.PlayerBalls[0].GetComponent<Ball>().v = v_ai;

        GameManager.instance.MovableCount--;
    }


    /*
    private void AttackRandom()     // target이 random으로 설정
    {
        GameManager.instance.findBalls();



        index = random.Next(GameManager.instance.PlayerBalls.Count);
        int targetIndex = random.Next(GameManager.instance.OpponentBalls.Count);

        m_ai = GameManager.instance.PlayerBalls[index].GetComponent<Ball>().m;
        v_ai = GameManager.instance.PlayerBalls[index].GetComponent<Ball>().v;

        Transform target = GameManager.instance.OpponentBalls[targetIndex].transform;

        dir = (target.position - GameManager.instance.PlayerBalls[index].transform.position).normalized;
        power = dir * random.Next(20, 100);

        a_ai = power / m_ai;
        v_ai += a_ai;

        Debug.Log("Add Velocity");
        GameManager.instance.PlayerBalls[index].GetComponent<Ball>().v = v_ai;  // add velocity
    }
    */
}


