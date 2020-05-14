using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

//The game manager. This is the gateway between C# and python.
public class GM : MonoBehaviour
{
    public Config config = new Config();
    // Start is called before the first frame update
    void Awake()
    {
        //SaveConfig(new Config());//uncomment this line to reset the config
        //config = GetConfig();
        //Load into main menu
        //for now we load straight into the game
        OnLoadLevel();

    }
    public static Config GetConfig()
    {
        string path = Application.streamingAssetsPath + "/config.xml";
        XmlSerializer x = new XmlSerializer(typeof(Config));
        FileStream fstream = new FileStream(path, FileMode.Open);
        Config conf = x.Deserialize(fstream) as Config;
        fstream.Close();
        return conf;
    }
    public static void SaveConfig(Config conf)
    {
        XmlSerializer xmlser = new XmlSerializer(typeof(Config));
        string savePath = Application.streamingAssetsPath + "/config.xml";
        FileStream stream = new FileStream(savePath, FileMode.Create);
        Config d = conf;
        xmlser.Serialize(stream, d);
        stream.Close();
    }
    void OnLoadLevel()
    {
        Transform cam = Camera.main.transform;
        GameObject character = Instantiate(Resources.Load<GameObject>("Prefabs/Character"));
        character.transform.position = new Vector3(cam.position.x, cam.position.y, 0);
        PlayerMovement mvmt = character.AddComponent<PlayerMovement>();
        mvmt.Camera = cam;
        character.GetComponent<CharacterModelData>().IsPlayer = true;
        if(GameObject.Find("PlayerSpawn"))
        {
            character.transform.position = GameObject.Find("PlayerSpawn").transform.position;
            Destroy(GameObject.Find("PlayerSpawn"));
        }
        if(GameObject.Find("Enemies"))
        {
            foreach(Transform t in GameObject.Find("Enemies").GetComponentsInChildren<Transform>())
            {
                CreateAI(t.transform.position);
                Destroy(t.gameObject);
            }
        }
    }
    void CreateAI(Vector3 position)
    {
        GameObject character = Instantiate(Resources.Load<GameObject>("Prefabs/Character"));
        character.transform.position = position;
        var nav = character.AddComponent<UnityEngine.AI.NavMeshAgent>();
        nav.speed = 1.7f;
        nav.angularSpeed = 10000f;
        nav.height = 1.7f;
        nav.radius = .3f;
        AI ai = character.AddComponent<AI>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            config = GetConfig();
        }
    }
}
