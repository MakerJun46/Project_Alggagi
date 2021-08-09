using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject WallFactory;
    public List<GameObject> Walls = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(WallFactory, new Vector3(3.6f, -3.6f, 0.8f), Quaternion.identity);
        Instantiate(WallFactory, new Vector3(-3.6f, -3.6f, 0.8f), Quaternion.identity);
        Instantiate(WallFactory, new Vector3(3.6f, 3.6f, 0.8f), Quaternion.identity);
        Instantiate(WallFactory, new Vector3(-3.6f, 3.6f, 0.8f), Quaternion.identity);


        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Walls.Add(go);
        }

        for (int i = 0; i < Walls.Count; i++)
        {
            Walls[i].GetComponent<Ball>().m = 1000;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
