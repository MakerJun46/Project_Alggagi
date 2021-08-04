using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    //ball 개수 조절
    //const int BALLNUM = 2;
    //const int BALLTOTALNUM = 4;
   
    public GameObject playerFactory;
    public GameObject ballFactory;
    public List<GameObject> Balls = new List<GameObject>();
      
    public bool isCollision;
    public bool isAlive;

    /*float[] arrX = new float[BALLNUM];
    float[] arrY = new float[BALLNUM];
    float numX = -2.2f;
    float numY = -1.0f;*/
    
    
    public Vector3 v1, v2;
    Vector3 c1, c2, n;
    float n1, n2;
    float v1pp, v2pp;
    Vector3 v1p, v2p;
    Vector3 a1, a2;
    Vector3 c11, c22;
    float v1pprime, v2pprime;
    Vector3 v;
    public Vector3 v11, v22;

    float m1, m2;
    float r1, r2;
    float e = 1.0f;

    public static int notAliveBallNum;
    public static int aliveBallNum;

    public bool playerAlive;

    public static int playerNum;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < Balls.Count; i++) // null -> 제거
        {
            if (Balls[i] == null)
            {
                Balls.RemoveAt(i);
            }
        }
        moveBalls();
    }

    void moveBalls()
    {
        for (int i = 0; i < Balls.Count; i++) // 두 공 충돌 물리적 엔진
        {
            for (int j = i + 1; j < Balls.Count; j++)
            {
                r1 = Balls[i].GetComponent<Ball>().r;
                r2 = Balls[j].GetComponent<Ball>().r;

                if (Vector3.Distance(Balls[i].transform.position, Balls[j].transform.position) <= r1 + r2)
                {
                    isCollision = true;

                    m1 = Balls[i].GetComponent<Ball>().m;
                    m2 = Balls[j].GetComponent<Ball>().m;

                    c1 = Balls[i].transform.position;
                    c2 = Balls[j].transform.position;

                    if (Balls[i].gameObject.tag == "Player")
                    {
                        Balls[i].GetComponent<PlayerMove>().isCollision_Player = true;
                        v1 = Balls[i].GetComponent<PlayerMove>().v_Player;
                    }

                    else
                    {
                        Balls[i].GetComponent<Ball>().isCollision = true;
                        v1 = Balls[i].GetComponent<Ball>().v;
                    }

                    Balls[j].GetComponent<Ball>().isCollision = true;
                    v2 = Balls[j].GetComponent<Ball>().v;

                    n = (c2 - c1) / Vector3.Distance(c2, c1);

                    v1pp = Vector3.Dot(v1, n);
                    v1p = v1pp * n;
                    a1 = v1 - v1p;

                    v2pp = Vector3.Dot(v2, n);
                    v2p = v2pp * n;
                    a2 = v2 - v2p;

                    float collisionTime = (r1 + r2 - Vector3.Distance(Balls[i].transform.position, Balls[j].transform.position)) / Mathf.Abs(v1pp - v2pp);

                    Balls[i].transform.position -= collisionTime * v1;
                    Balls[j].transform.position -= collisionTime * v2;

                    v1pprime = (((m1 - e * m2) * v1pp) + ((1 + e) * m2 * v2pp)) / (m1 + m2);
                    v2pprime = (((m2 - e * m1) * v2pp) + ((1 + e) * m1 * v1pp)) / (m1 + m2);

                    v11 = v1pprime*n + a1; // 충돌 후 원 c1의 속도
                    v22 = v2pprime*n + a2; // 충돌 후 원 c2의 속도

                    if (Balls[i].gameObject.tag == "Player")
                        Balls[i].GetComponent<PlayerMove>().v_Player = v11;

                    else
                        Balls[i].GetComponent<Ball>().v = v11;

                    Balls[j].GetComponent<Ball>().v = v22;

                    Balls[i].transform.position += (Time.deltaTime - collisionTime) * v11;
                    Balls[j].transform.position += (Time.deltaTime - collisionTime) * v22;
                }
            }
        }
    }

    public void FindBalls()
    {
        List<GameObject> tmp = new List<GameObject>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            tmp.Add(go);
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Opponent"))
        {
            tmp.Add(go);
        }

        Balls = tmp;
    }
}