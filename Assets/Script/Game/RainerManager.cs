using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class RainerManager : Singleton<RainerManager> {

    public static int LayerRainerIdle { get; private set; }
    public static int LayerRainerFollow { get; private set; }

    [SerializeField]
    GameObject rainerPrefab;

    [Range(5.0f, 20.0f)]
    public float max_speed = 10.0f;
    [Range(10.0f, 200.0f)]
    public float unity_range = 200.0f;
    [Range(1.0f, 10.0f)]
    public float avoid_range = 3.0f;

    [Range(0, 1000)]
    public int pop_interval = 100;

    List<RainerController> rainers = new List<RainerController>();

    public Transform spawnGroup;
    List<Material> materials = new List<Material>();
    List<Transform> spawnList = new List<Transform>();
    int timer;

    protected override void Awake()
    {
        base.Awake();
        LayerRainerIdle     = LayerMask.NameToLayer("RainerIdle");
        LayerRainerFollow   = LayerMask.NameToLayer("RainerFollow");

        materials.Add(Resources.Load<Material>("Materials/Rainer_coat"));

        foreach (var color in GameSetting.PlayerColors)
        {
            var material = new Material(materials[0])
            {
                color = Color.Lerp(Color.white * 0.7f, color, 0.4f)
            };
            materials.Add(material);
        }
    }

    // Use this for initialization
    void Start()
    {
        timer = 0;
        for (int i = 0; i < spawnGroup.childCount; i++)
        {
            spawnList.Add(spawnGroup.GetChild(i));
            spawnList[i].gameObject.SetActive(false);
        }
        foreach (var rainer in rainers)
        {
            rainer.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        if (timer > pop_interval)
        {
            timer = 0;

            if (spawnList.Count > 0)
            {
                int index = Random.Range(0, spawnList.Count - 1);

                SpawnRainer(spawnList[index].position);

                spawnList.Remove(spawnList[index]);
            }
        }
    }

    public RainerController SpawnRainer(Vector3 position)
    {
        var rainerObj = Instantiate(rainerPrefab, position, Quaternion.identity, transform);
        var rainer = rainerObj.GetComponent<RainerController>();
        rainer.CreateCloud();
        rainer.SetFree();
        rainers.Add(rainer);
        return rainer;
    }

    public List<RainerController> GetBoidsNearby(RainerController rainer)
    {
        var boids = new List<RainerController>();

        foreach (RainerController other in rainers)
        {
            if (other.gameObject != rainer.gameObject
                && other.gameObject.layer == LayerRainerFollow
                && other.Leader == rainer.Leader
                )
            {
                var vec = other.transform.position - rainer.gameObject.transform.position;

                if (vec.magnitude < unity_range)
                {
                    boids.Add(other);
                }
            }
        }

        return boids;
    }

    /// <summary>
    /// プレイヤーの色に応じてコートのマテリアルを返す
    /// </summary>
    /// <param name="playerNo"></param>
    /// <returns></returns>
    public Material GetMaterial(int playerNo)
    {
        return materials[playerNo+1];
    }

    public Material GetDefaultMaterial()
    {
        return materials[0];
    }
}
