using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public bool isMouseUp = false;
    //public GameObject gameobject;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
        Color colStart = new Color(0.4784314f, 0.3764706f, 0.2196079f, 0.2f);
        Color colFinish = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        lineRenderer.SetColors(colStart,colFinish);
    }

    // Update is called once per frame
    void Update()
    {
        isMouseUp = GetComponent<PlayerMove>().isMouseUp;
       
        if (isMouseUp)
        {
            lineRenderer.enabled = true;
            Vector3 originPos = GetComponent<PlayerMove>().originPos;
            Vector3 mousePos =GetComponent<PlayerMove>().mousePos;
            float d = GetComponent<PlayerMove>().d;

            Vector3 moveDir = originPos - mousePos;

            lineRenderer.SetPosition(0, mousePos);
            lineRenderer.SetPosition(1, transform.position+moveDir.normalized*(d/60));
        }

        else
        {
            lineRenderer.enabled = false;
        }

    }
}
