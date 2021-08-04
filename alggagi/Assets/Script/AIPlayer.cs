using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIPlayer : MonoBehaviour
{
    System.Random random = new System.Random();
    public GameManager GM;

    Vector3 dir;
    Vector3 power;

    float m_ai;
    Vector3 a_ai, v_ai;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitTurn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator waitTurn()
    {
        yield return new WaitForSeconds(1.0f);
        Attack();
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(waitTurn());
    }

    private void Attack()
    {
        int index = random.Next(GM.OpponentBalls.Count);
        int targetIndex = random.Next(GM.PlayerBalls.Count);

        m_ai = GM.OpponentBalls[index].GetComponent<Ball>().m;
        v_ai = GM.OpponentBalls[index].GetComponent<Ball>().v;

        Transform target = GM.PlayerBalls[targetIndex].transform;

        dir = (target.position - GM.OpponentBalls[index].transform.position).normalized;
        power = dir * random.Next(20, 100);

        a_ai = power / m_ai;
        v_ai += a_ai;

        GM.OpponentBalls[index].GetComponent<Ball>().v = v_ai;  // add velocity
    }
}
