using System.Collections;
using System;
using System.Collections.Generic;
using IronPython.Hosting;
using UnityEngine;
using Microsoft.Scripting.Hosting;

//The game manager. This is the gateway between C# and python.
public class GM : MonoBehaviour
{
    private ScriptEngine engine;
    public dynamic Master;
    public string Test; 
    // Start is called before the first frame update
    void Awake()
    {
        engine = Python.CreateEngine();
        ICollection<string> searchPaths = engine.GetSearchPaths();
        searchPaths.Add(Application.dataPath);
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        engine.SetSearchPaths(searchPaths);
        dynamic lm = engine.ExecuteFile(Application.dataPath + @"\Python/Master.py");
        Master = lm.Master("Assets/Python/");
        Master.GM = this;
        print(Master.GetTest());

        //Load into main menu
        //for now we load straight into the game
        OnLoadLevel();
    }
    void OnLoadLevel()
    {
        Transform cam = Camera.main.transform;
        GameObject character = Instantiate(Resources.Load<GameObject>("Prefabs/Character"));
        character.transform.position = new Vector3(cam.position.x, cam.position.y, 0);
        PlayerMovement mvmt = character.AddComponent<PlayerMovement>();
        mvmt.Camera = cam;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
