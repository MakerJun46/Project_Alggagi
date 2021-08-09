using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public List<GameObject> Balls = new List<GameObject>();
      
    /// <summary>
    ///  ���� �ӵ�
    /// </summary>
    public Vector3 v1, v2;

    /// <summary>
    /// ���� ��ġ
    /// </summary>
    Vector3 c1, c2;

    /// <summary>
    /// c1c2 ��������
    /// </summary>
    Vector3 n;
    
    float v1x_scalar, v2x_scalar;

    /// <summary>
    /// v�� x����
    /// </summary>
    Vector3 v1x, v2x;

    /// <summary>
    /// v�� y����
    /// </summary>
    Vector3 v1y, v2y;

    /// <summary>
    /// �浹 �� �ӵ� ���
    /// </summary>
    float v1_collide_scalar, v2_collide_scalar;

    /// <summary>
    /// �浹 �� �ӵ�
    /// </summary>
    public Vector3 v1_collide, v2_collide;

    /// <summary>
    /// ���� ����
    /// </summary>
    float m1, m2;

    /// <summary>
    /// ���� ������
    /// </summary>
    float r1, r2;

    /// <summary>
    /// ź�� ���
    /// </summary>
    float e = 1.0f;

    float collisionTime=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < Balls.Count; i++) // null -> ����
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
        for (int i = 0; i < Balls.Count; i++) // �� �� �浹 ������ ����
        {
            for (int j = 0; j < Balls.Count; j++)
            {
                if (i != j)
                {                
                    r1 = Balls[i].GetComponent<Ball>().r;
                    r2 = Balls[j].GetComponent<Ball>().r;

                    if (Vector3.Distance(Balls[i].transform.position, Balls[j].transform.position) <= r1 + r2)
                    {

                        m1 = Balls[i].GetComponent<Ball>().m;
                        m2 = Balls[j].GetComponent<Ball>().m;

                        c1 = Balls[i].transform.position;
                        c2 = Balls[j].transform.position;

                        //if (Balls[i].gameObject.tag == "Player")
                        //{
                        //    v1 = Balls[i].GetComponent<PlayerMove>().v_Player;
                        //}

                        //else v1 = Balls[i].GetComponent<Ball>().v;

                        v1 = Balls[i].GetComponent<Ball>().v;
                        v2 = Balls[j].GetComponent<Ball>().v;


                        n = (c2 - c1) / Vector3.Distance(c2, c1);

                        v1x_scalar = Vector3.Dot(v1, n);
                        v1x = v1x_scalar * n;
                        v1y = v1 - v1x;

                        v2x_scalar = Vector3.Dot(v2, n);
                        v2x = v2x_scalar * n;
                        v2y = v2 - v2x;

                        if (Mathf.Abs(v1x_scalar - v2x_scalar) != 0)
                        {
                            collisionTime = (r1 + r2 - Vector3.Distance(Balls[i].transform.position, Balls[j].transform.position)) / Mathf.Abs(v1x_scalar - v2x_scalar);
                            Balls[i].transform.position -= collisionTime * v1;
                            Balls[j].transform.position -= collisionTime * v2;
                        }
                        //float collisionTime = (r1 + r2 - Vector3.Distance(Balls[i].transform.position, Balls[j].transform.position)) / Mathf.Abs(v1x_scalar - v2x_scalar);

                        v1_collide_scalar = (((m1 - e * m2) * v1x_scalar) + ((1 + e) * m2 * v2x_scalar)) / (m1 + m2);
                        v2_collide_scalar = (((m2 - e * m1) * v2x_scalar) + ((1 + e) * m1 * v1x_scalar)) / (m1 + m2);

                        v1_collide = v1_collide_scalar * n + v1y; // �浹 �� �� c1�� �ӵ�
                        v2_collide = v2_collide_scalar * n + v2y; // �浹 �� �� c2�� �ӵ�

                        //if (Balls[i].gameObject.tag == "Player")
                        //    Balls[i].GetComponent<PlayerMove>().v_Player = v1_collide;

                        //else
                        //    Balls[i].GetComponent<Ball>().v = v1_collide;

                        Balls[i].GetComponent<Ball>().v = v1_collide;
                        Balls[j].GetComponent<Ball>().v = v2_collide;


                        if (Mathf.Abs(v1x_scalar - v2x_scalar) != 0)
                        {
                            Balls[i].transform.position += (Time.deltaTime - collisionTime) * v1_collide;
                            Balls[j].transform.position += (Time.deltaTime - collisionTime) * v2_collide;
                        }

                        //Debug.Log("Balls[i] = " + Balls[i].transform.position + "  Balls[j] = " + Balls[j].transform.position);
                    }
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

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Wall"))
        {
            tmp.Add(go);
        }

        Balls = tmp;
    }
}