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
    int index;

    Vector3 a_Player, f_Player, v_Player;
    float a_friction;
    float a_friction2 = -0.1f;
    float m_Player;

    bool startTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitTurn());
    }

    // Update is called once per frame
    void Update()
    {
        if(startTrigger)
        {
            AddForce();
        }
    }

    public IEnumerator waitTurn()
    {
        yield return new WaitForSeconds(1.0f);
        Attack();
        yield return new WaitForSeconds(2.0f);
        startTrigger = true;
    }

    private void Attack()
    {
        index = random.Next(0, GM.OpponentBalls.Count);
        //Ball ball = GM.OpponentBalls[index].gameObject.GetComponent<Ball>();
        Debug.Log($"index : {index}");

        Transform target = GameObject.FindGameObjectWithTag("Player").transform;

        GM.OpponentBalls[index].transform.LookAt(target);

        dir = (target.position - GM.OpponentBalls[index].transform.position).normalized;

        power = dir * random.Next(500, 2000);

        Debug.Log($"Power : {power}");

        a_Player = GM.OpponentBalls[index].gameObject.GetComponent<Ball>().a;
        f_Player = GM.OpponentBalls[index].gameObject.GetComponent<Ball>().f;
        m_Player = GM.OpponentBalls[index].gameObject.GetComponent<Ball>().m;
        v_Player = GM.OpponentBalls[index].gameObject.GetComponent<Ball>().v;
        a_friction = GM.OpponentBalls[index].gameObject.GetComponent<Ball>().a_friction;

        v_Player = new Vector3(0, 0, 0);
    }


    private void AddForce()
    {
        f_Player = power;
        a_Player = f_Player / m_Player;

        v_Player += a_Player;

        Debug.Log($"v_player : {v_Player}");

        GM.OpponentBalls[index].gameObject.GetComponent<Ball>().ballFriction(ref v_Player, ref a_friction2);

        GM.OpponentBalls[index].gameObject.transform.position += v_Player * Time.deltaTime;
    }
}
