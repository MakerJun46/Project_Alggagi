using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Excute();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Excute()
    {
        UnityEngine.Debug.Log("¿©±â");
        string path = "D:/SourceTree/Project_Alggagi/buildtest/0823_v1/alggagi_0713.exe";
        string mapPath = "1";

        Process.Start(path, mapPath);

        Process process = new Process();

        //process.StartInfo.FileName = path;
        //process.StartInfo.Arguments = mapPath;

        //process.Start();

    }
}
