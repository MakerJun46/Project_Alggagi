using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gettingItem : MonoBehaviour
{
    const int ITEMTOTALNUM = 1;
    public GameObject itemFactory;
    public GameObject[] items = new GameObject[ITEMTOTALNUM];

    float r1, r2;
    float getItemSize = 0.0f;
    bool getItems = false;
    // Start is called before the first frame update
    void Start()
    {
        makeItem();
    }

    // Update is called once per frame
    void Update()
    {
       // getItem();  
    }
    void makeItem()
    {
        items[0] = (GameObject)Instantiate(itemFactory, new Vector3(0.0f, -3.0f, -0.1f), Quaternion.identity);
    }
    //void getItem()
    //{
    //    Gameobject Balls  = GetComponent<PhysicsManager>().Balls;
    //    r1 = Balls[0].GetComponent<Ball>().r;
    //    r2 = items[0].transform.localScale.x / 2;

    //    Debug.Log("r= " + Balls[0].GetComponent<Ball>().r + "  size = " + getItemSize + " real ball size =  " + Balls[0].transform.localScale);

    //    if (Vector3.Distance(Balls[0].transform.position, items[0].transform.position) <= (r1 + r2))
    //    {
    //        getItems = true;
    //        Debug.Log("getItems = " + getItems);
    //        if (getItems)
    //        {
    //            getItemSize = 2.0f;

    //            Balls[0].GetComponent<Ball>().r *= getItemSize;
    //            Balls[0].transform.localScale = new Vector3(r1 * getItemSize, r1 * getItemSize, r1 * getItemSize);

    //            Debug.Log("r= " + Balls[0].GetComponent<Ball>().r + "  size = " + getItemSize + " real ball size =  " + Balls[0].transform.localScale);

    //            items[0].transform.position = new Vector3(0, 0, 5);
    //            items[0].SetActive(false);
    //            getItems = false;

    //        }

    //    }

    //}
}
