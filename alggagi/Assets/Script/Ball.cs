using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ball : MonoBehaviour
{
    GameObject GM;
    public Vector3 a = new Vector3(0, 0, 0);
    public float a_friction;
    public Vector3 v_norm;
    public Vector3 v = new Vector3(0, 0, 0);
    public Vector3 f = new Vector3(0, 0, 0);
    public float m = 5.0f;
    public float r;

    public bool isCollision = false;

    RaycastHit hit;
    float MaxDistance = 10.0f;

   void Start()
    {
        GM = GameObject.Find("GameMannager");
        r = transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        ballFriction(ref v, ref a_friction);

        if (Physics.Raycast(transform.position, -transform.forward, out hit, MaxDistance)) // 판 위에 있을때
        {
            transform.position += v * Time.deltaTime;
        }

        else // 판에서 떨어질때
        {
            if (transform.position.z < -5.0f)
            {
                FallDown();
                Destroy(gameObject);
            }
            transform.position += (new Vector3(0, 0, -5) + v) * Time.deltaTime;
        }
    }

    public void ballFriction(ref Vector3 v, ref float a_friction)
    {
        a_friction = -2.0f;
        Vector3 v_norm = v.normalized;
        v += a_friction * v_norm * Time.deltaTime;
    }  


    /// <summary>
    /// if Ball position is out board and position.z < -5.0f, destroy ball
    /// </summary>
    public void FallDown()
    {
        Debug.LogError("FallDown");
        GM.GetComponent<PhysicsManager>().FindBalls();
        Destroy(gameObject);
    }
}
