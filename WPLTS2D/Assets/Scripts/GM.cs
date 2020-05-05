using System.Collections;
using System.Collections.Generic;
using IronPython.Hosting;
using UnityEngine;
using Microsoft.Scripting.Hosting;

//The game manager. This is the gateway between C# and python.
public class GM : MonoBehaviour
{
    private ScriptEngine engine;
    public dynamic LoreMaster;
    // Start is called before the first frame update
    void Start()
    {
        engine = Python.CreateEngine();
        ICollection<string> searchPaths = engine.GetSearchPaths();
        searchPaths.Add(Application.dataPath);
        searchPaths.Add(Application.dataPath + @"\Plugins\Lib\");
        engine.SetSearchPaths(searchPaths);
        dynamic lm = engine.ExecuteFile(Application.dataPath + @"\Python/LoreMaster.py");
        LoreMaster = lm.LoreMaster("Assets/Python/");
        print(LoreMaster.GetTest());

        //Load into main menu


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
