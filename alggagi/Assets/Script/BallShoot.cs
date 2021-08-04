using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShoot : Ball
{
    public List<GameObject> OpponentBalls = new List<GameObject>();
    public List<GameObject> PlayerBalls = new List<GameObject>();


    float m_ai;
    Vector3 f_ai;
    Vector3 a_ai;
    public Vector3 v_ai;

    float a_friction2, a_friction;

    bool test = true;
    bool aaa = true;
    public bool bbb = false;

    int i, j;
    // Start is called before the first frame update
    void Start()
    {
        m_ai = GetComponent<Ball>().m;
        v_ai = GetComponent<Ball>().v;
    }

    // Update is called once per frame
    void Update()
    {
        if (aaa)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                PlayerBalls.Add(go);
            }

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Opponent"))
            {
                OpponentBalls.Add(go);
            }

            aaa = false;
        }


        if (test)
        {
            i = Random.Range(0, 2);
            j = Random.Range(0, 6);
            Debug.Log("start test");
            Vector3 moveDir = (PlayerBalls[0].transform.position - OpponentBalls[0].transform.position).normalized;
            f_ai = moveDir * Random.Range(25, 30);
            a_ai = f_ai / m_ai;

            v_ai += a_ai;
            OpponentBalls[0].GetComponent<Ball>().v = v_ai;
            test = false;

        }

        ballFriction(ref v_ai, ref a_friction2);
        //Debug.Log("v_ai : " + v_ai.x + " , " + v_ai.y + " , " + v_ai.z);
        //OpponentBalls[0].transform.position += v_ai * Time.deltaTime;
        //OpponentBalls[0].GetComponent<Ball>().v = v_ai;
    }

    //public void shoot(Vector3 power)
    //{

    //    Vector3 moveDir = (PlayerBalls[0].transform.position- OpponentBalls[0].transform.position).normalized;
    //    power = moveDir * Random.Range(500, 2000);
    //    a_ai = power / m_ai;

    //    v_ai += a_ai * (1/10);

    //    ballFriction(ref v_ai, ref a_friction2);

    //    transform.position += v_ai * Time.deltaTime;
    //}


}