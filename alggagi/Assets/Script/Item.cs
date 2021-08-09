using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    PhysicsManager PM;

    //BIGGER ITEM
    const int ITEMTOTALNUM = 1;
    const int BALLTOTALNUM = 4;

    public GameObject itemFactory;

    public List<GameObject> Items = new List<GameObject>();
    public List<GameObject> PlayerBalls = new List<GameObject>();

    float r1, r2;
    float getItemSize;
    bool getItems = false;
    bool startTime = false;
    float startTimer;

    int GetItemPlayer_index;

    public bool addPlayersList = true;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(itemFactory, new Vector3(0.0f, 0.0f, 0.8f), Quaternion.identity);

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Item"))
        {
            Items.Add(go);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (addPlayersList)
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                PlayerBalls.Add(go);
            }
            addPlayersList = false;
        }

        for (int i = 0; i < PlayerBalls.Count; i++) // null -> 제거
        {
            if (PlayerBalls[i] == null)
            {
                PlayerBalls.RemoveAt(i);
            }
        }

        for (int i = 0; i < Items.Count; i++) // null -> 제거
        {
            if (Items[i] == null)
            {
                Items.RemoveAt(i);
            }
        }

        getItem();
    }


    void getItem()
    {
        for (int i = 0; i < PlayerBalls.Count; i++)
        {
            r1 = PlayerBalls[i].GetComponent<Ball>().r; // player
            r2 = Items[0].transform.localScale.x / 2;  // item


            if (Vector3.Distance(PlayerBalls[i].transform.position, Items[0].transform.position) <= (r1 + r2))
            {
                GetItemPlayer_index = i;
                getItemSize = 0.5f;

                PlayerBalls[i].GetComponent<Ball>().r = getItemSize / 2;
                PlayerBalls[i].transform.localScale = new Vector3(getItemSize, getItemSize, getItemSize);

                startTime = true;
                getItems = true;

                Items[0].transform.position = new Vector3(0, 0, -5);
                //Items[0].SetActive(false);
                Destroy(Items[0]);
            }

            if (startTime)
            {
                startTimer = Time.time;
                startTime = false;
            }

            if (getItems)
            {
                if (Time.time - startTimer >= 5.0f)
                {
                    PlayerBalls[GetItemPlayer_index].GetComponent<Ball>().r = 0.15f;  // 값
                    PlayerBalls[GetItemPlayer_index].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    getItems = false;
                }
            }
        }
    }

}
