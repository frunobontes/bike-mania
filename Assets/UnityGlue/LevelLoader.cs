using System.Collections.Generic;
using UnityEngine;
using BikeMania.Core;

// Unity glue: loads level JSON from Resources/Levels and instantiates simple primitives
public class LevelLoader : MonoBehaviour
{
    public string levelResourceName = "Levels/example"; // path under Resources without extension
    public GameObject playerPrefab;
    public Material groundMaterial;

    private List<GameObject> spawned = new List<GameObject>();

    void Start()
    {
        var txt = Resources.Load<TextAsset>(levelResourceName);
        if (txt == null)
        {
            Debug.LogError("Level resource not found: " + levelResourceName);
            return;
        }
        var level = LevelParser.ParseFromJson(txt.text);

        // spawn track
        foreach (var seg in level.Track)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = new Vector3(seg.X + seg.Width/2f, seg.Y - seg.Height/2f, 0);
            go.transform.localScale = new Vector3(seg.Width, seg.Height, 1);
            if (groundMaterial != null) go.GetComponent<Renderer>().material = groundMaterial;
            spawned.Add(go);
        }

        // spawn player
        if (playerPrefab != null && level.Spawn != null)
        {
            var p = Instantiate(playerPrefab);
            p.transform.position = new Vector3(level.Spawn.X, level.Spawn.Y, 0);
            p.transform.rotation = Quaternion.Euler(0, 0, level.Spawn.Rotation);
            spawned.Add(p);
        }
    }
}
