using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    GameObject GM;
    GameObject sizeObject;
    GameObject massObject;
    Slider size;
    Slider mass;
    public Vector3 a = new Vector3(0, 0, 0);
    public Vector3 v = new Vector3(0, 0, 0);
    public Vector3 f = new Vector3(0, 0, 0);

    public Vector3 stopCmp = new Vector3(0, 0, 0);

    public float a_friction;
    public Vector3 v_norm;

    public bool isBallStop;
    
    public float m = 5.0f;
    public float r;

    RaycastHit hit;
    float MaxDistance = 10.0f;

   void Start()
    {
        GM = GameObject.Find("GameManager");
        sizeObject = GameObject.Find("size");
        massObject = GameObject.Find("mass");
        size = sizeObject.GetComponent<Slider>();
        mass = massObject.GetComponent<Slider>();
        r = transform.localScale.x / 2;
        isBallStop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(v != new Vector3(0, 0, 0))
        {
            isBallStop = false;
        }
        else
        {
            isBallStop = true;
        }
        ballFriction(ref v, ref a_friction);

        if (Physics.Raycast(transform.position, -transform.forward, out hit, MaxDistance)) // 판 위에 있을때
        {
            //v += g*Time.deltaTime;
            transform.position += v * Time.deltaTime;
        }

        else // 판에서 떨어질때
        {
            if (transform.position.z < -1.0f)
            {
                FallDown();
            }
            transform.position += (new Vector3(0, 0, -5) + v) * Time.deltaTime;
        }


        // 공의 속도가 0.001 이하라면 공이 멈추어 있는 것으로 간주
        /*
        if(v.x < 0.01f && v.y < 0.01f)
        {
            v = new Vector3(0, 0, 0);
        }
        */

        if(Math.Abs(v.x) < 0.01f)
        {
            v = new Vector3(0, v.y, 0);
        }
        if(Math.Abs(v.y) < 0.01f)
        {
            v = new Vector3(v.x, 0, 0);
        }

        if(this.gameObject.tag == "Player")
        {
            gameObject.transform.localScale = new Vector3(size.value, size.value, size.value);
            r = size.value / 2;
            m = mass.value;
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
        Destroy(this.gameObject);
    }

}
