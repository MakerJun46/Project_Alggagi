using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public GameObject playerFactory;
    public GameObject ballFactory;
    public List<GameObject> Balls = new List<GameObject>();

    public bool isCollision;
    public bool isAlive;

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


    float elasticModulus = 1.0f; // 탄성 계수
    float thisRadius, collisionRadius;
    float thisMass, collisionMass;
    float collisionTime;
    Vector3 thisVelocity, collisionVelocity;
    Vector3 thisCenter, collisionCenter;
    Vector3 thisVelocityPrime, collisionVelocityPrime;
    Vector3 ReturnVelocity;
    

    // Start is called before the first frame update
    void Start()
    {
        thisRadius = GetComponent<Ball>().r;
        thisMass = GetComponent<Ball>().m;
        thisCenter = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionRadius = collision.gameObject.GetComponent<Ball>().r;
        collisionMass = collision.gameObject.GetComponent<Ball>().m;
        collisionCenter = collision.gameObject.GetComponent<Ball>().transform.position;

        if(this.gameObject.tag == "Player")
        {
            //gameObject.GetComponent<PlayerMove>().isClick_Player = true;
            thisVelocity = gameObject.GetComponent<PlayerMove>().v_Player;
        }
        else
        {
            //gameObject.GetComponent<Ball>().isCollision = true;
            thisVelocity = gameObject.GetComponent<Ball>().v;
        }

        //collision.gameObject.GetComponent<Ball>().isCollision = true;
        collisionVelocity = collision.gameObject.GetComponent<Ball>().v;

        n = (collisionCenter - thisCenter) / Vector3.Distance(collisionCenter, thisCenter);

        v1pp = Vector3.Dot(thisVelocity, n);
        v1p = v1pp * n;
        a1 = v1 - v1p;

        v2pp = Vector3.Dot(collisionVelocity, n);
        v2p = v2pp * n;
        a2 = v2 - v2p;

        v1pprime = (((thisMass - elasticModulus * collisionMass) * v1pp) + ((1 + elasticModulus) * collisionMass * v2pp)) / (thisMass + collisionMass);
        v2pprime = (((collisionMass - elasticModulus * thisMass) * v2pp) + ((1 + elasticModulus) * thisMass * v1pp)) / (thisMass + collisionMass);

        v11 = v1pprime * n + a1; // 충돌 후 원 c1의 속도
        v22 = v2pprime * n + a2; // 충돌 후 원 c2의 속도

        if (this.gameObject.tag == "Player")
            this.gameObject.GetComponent<PlayerMove>().v_Player = v11;

        else
        {
            this.gameObject.GetComponent<Ball>().v = v11;
        }

        collision.gameObject.GetComponent<Ball>().v = v22;

        this.gameObject.transform.position += (Time.deltaTime - collisionTime) * v11;
        collision.gameObject.transform.position += (Time.deltaTime - collisionTime) * v22;

    }

    private void OnCollisionStay(Collision collision)
    {
        collisionTime = (thisRadius + collisionRadius - Vector3.Distance(this.gameObject.transform.position, collision.gameObject.transform.position)) / Mathf.Abs(v1pp - v2pp);

        this.gameObject.transform.position -= collisionTime * v1;
        collision.gameObject.transform.position -= collisionTime * v2;
    }

    private void OnCollisionExit(Collision collision)
    {
    }


    /*
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
                        Balls[i].GetComponent<PlayerMove>().isClick_Player = true;
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

                    v1x_scalar = Vector3.Dot(v1, n);
                    v1x = v1x_scalar * n;
                    v1y = v1 - v1x;

                    v2pp = Vector3.Dot(v2, n);
                    v2x = v2pp * n;
                    v2y = v2 - v2x;

                    float collisionTime = (r1 + r2 - Vector3.Distance(Balls[i].transform.position, Balls[j].transform.position)) / Mathf.Abs(v1x_scalar - v2pp);

                    Balls[i].transform.position -= collisionTime * v1;
                    Balls[j].transform.position -= collisionTime * v2;

                    v1_collide_scalar = (((m1 - e * m2) * v1x_scalar) + ((1 + e) * m2 * v2pp)) / (m1 + m2);
                    v2_collide_scalar = (((m2 - e * m1) * v2pp) + ((1 + e) * m1 * v1x_scalar)) / (m1 + m2);

                    v1_collide = v1_collide_scalar * n + v1y; // 충돌 후 원 c1의 속도
                    v2_collide = v2_collide_scalar * n + v2y; // 충돌 후 원 c2의 속도

                    if (Balls[i].gameObject.tag == "Player")
                        Balls[i].GetComponent<PlayerMove>().v_Player = v1_collide;

                    else
                        Balls[i].GetComponent<Ball>().v = v1_collide;

                    Balls[j].GetComponent<Ball>().v = v2_collide;

                    Balls[i].transform.position += (Time.deltaTime - collisionTime) * v1_collide;
                    Balls[j].transform.position += (Time.deltaTime - collisionTime) * v2_collide;
                }
            }
        }

    }
    */

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