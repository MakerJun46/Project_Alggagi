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

    float a_friction2;

    bool AI_turn = true;
    public bool addLists = true;
    
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
        if (addLists)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                PlayerBalls.Add(go);
            }

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Opponent"))
            {
                OpponentBalls.Add(go);
            }

            addLists = false;
        }

        if (AI_turn)
        {
            AddForceAI();
            AI_turn = false;
        }

        ballFriction(ref v_ai, ref a_friction2);
    }

   void AddForceAI()
   {
        i = Random.Range(0, PlayerBalls.Count);
        j = Random.Range(0, OpponentBalls.Count);
  
        Vector3 moveDir = (PlayerBalls[i].transform.position - OpponentBalls[j].transform.position).normalized;

        f_ai = moveDir* Random.Range(30, 35);
        a_ai = f_ai / m_ai;

        v_ai += a_ai;
        OpponentBalls[j].GetComponent<Ball>().v = v_ai;
    }


}