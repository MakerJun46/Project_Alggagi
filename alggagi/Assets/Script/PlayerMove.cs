using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Ball
{
    private Vector3 m_Offset;
    private float m_ZCoord;
    public bool isMouseUp = false;
    public bool MouseDragUp = false;
    public float a_friction2 = -0.1f;
    public Vector3 originPos;
    public Vector3 mousePos;
    public Vector3 v_Player, a_Player, f_Player;
    public float m_player,d;
    public Vector3 prevPos;
    public Vector3 curPos;
    public bool isCollision_Player=false;
    bool isMove;
    Vector3 mouseDragPos;

    public static int clickNum = 0;

    private void OnMouseDown()
    {
        m_ZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        m_Offset = gameObject.transform.position - GetMouseWorldPosition();
        isMouseUp = true;
    }

    private void OnMouseDrag()
    {
        //transform.position = GetMouseWorldPosition() + m_Offset;
        //mousePos = this.transform.position;

        mouseDragPos = GetMouseWorldPosition() + m_Offset;
        mousePos = mouseDragPos;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseUp() // ¶¿ ¶§
    {
        MouseDragUp = true;
        isMove = true;
        isMouseUp = false;
        clickNum += 1;
    }
    // Start is called before the first frame update

    void Start()
    {
        originPos = this.transform.position;
        v_Player = GetComponent<Ball>().v;
        m_player = GetComponent<Ball>().m;
    }

    // Update is called once per frame
    void Update()
    {
        //isCollision_Player = GameObject.Find("GameObject").GetComponent<PhysicsManager>().isCollision;

        prevPos = this.transform.position;
        playermove_();
        curPos = this.transform.position;
        if (isMove)
        {
            if (Mathf.Abs(curPos.x - prevPos.x) < 0.001f && Mathf.Abs(curPos.y - prevPos.y) < 0.001f)
            {
                originPos = this.transform.position;
                v_Player = new Vector3(0, 0, 0);
                isMove = false;
            }
        }
    }


    public void ballFriction()
    {
        a_friction = -0.1f;
        Vector3 v_norm = v_Player.normalized;
        v_Player += a_friction * v_norm * Time.deltaTime;
    }


    void playermove_()
    {
        a_Player = GetComponent<Ball>().a;
        f_Player = GetComponent<Ball>().f;

        d = Vector3.Distance(originPos, mousePos) * 100;

        if (MouseDragUp)
        {
            Vector3 moveDir = originPos - mousePos;
            f_Player = moveDir * 100;
            a_Player = f_Player / m_player;
                        
            v_Player += a_Player  * (d / 1000);        
            MouseDragUp = false;
        }

        ballFriction(ref v_Player, ref a_friction2);

        transform.position += v_Player * Time.deltaTime;

    }

}
