using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameObject GM;
    public Vector3 a = new Vector3(0, 0, 0);
    public Vector3 v = new Vector3(0, 0, 0);
    public Vector3 f = new Vector3(0, 0, 0);

    public float a_friction;
    public Vector3 v_norm;
    
    public float m = 5.0f;
    public float r;

    RaycastHit hit;
    float MaxDistance = 10.0f;

   void Start()
    {
        GM = GameObject.Find("GameManager");
        r = transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        ballFriction(ref v, ref a_friction);

        if (Physics.Raycast(transform.position, -transform.forward, out hit, MaxDistance)) // �� ���� ������
        {
            //v += g*Time.deltaTime;
            transform.position += v * Time.deltaTime;
        }

        else // �ǿ��� ��������
        {
            if (transform.position.z < -5.0f)
            {
                FallDown();
            }
            transform.position += (new Vector3(0, 0, -5) + v) * Time.deltaTime;
        }
    }

    public void ballFriction(ref Vector3 v, ref float a_friction)
    {
        a_friction = -2.0f;
        v_norm = v.normalized;
        v += a_friction * v_norm * Time.deltaTime;
    }


    /// <summary>
    /// if Ball position is out board and position.z < -5.0f, destroy ball
    /// </summary>
    public void FallDown()
    {
        GM.GetComponent<PhysicsManager>().FindBalls();
        Destroy(this.gameObject);
    }
}
