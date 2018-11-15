using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RainerLib;

public class RainerManager : Singleton<RainerManager> {

    public static int LayerRainerIdle { get; private set; }
    public static int LayerRainerFollow { get; private set; }

    [SerializeField]
    GameObject rainerPrefab;

    [SerializeField]
    List<Material> materials;

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
    List<Transform> spawnList = new List<Transform>();
    int timer;

    protected override void Awake()
    {
        base.Awake();
        LayerRainerIdle     = LayerMask.NameToLayer("RainerIdle");
        LayerRainerFollow   = LayerMask.NameToLayer("RainerFollow");
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
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var rainer in rainers)
        {            
            if (rainer.gameObject.layer == LayerRainerFollow)
            {
                // 周辺のrainer.gameObjectをboidsに設定
                rainer.FindBoidsNearby(rainers, unity_range);

                rainer.move +=
                    rainer.MoveSeparate(avoid_range) * 1.4f
                    + rainer.MoveAlign()
                    + rainer.MoveConhesion()
                    + rainer.MoveChase(avoid_range);

                rainer.SetSpeed(Vector3.Lerp(rainer.move, rainer.leader.CharacterController.velocity, 0.1f).magnitude);
            }
            else if (rainer.gameObject.layer == LayerRainerIdle)
            {
                
                rainer.move = rainer.point - rainer.transform.position;
            }

            rainer.move = Vector3.ClampMagnitude(rainer.move, max_speed);

            // 移動する
            rainer.CharacterController.SimpleMove(rainer.move);

        }

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

    void SpawnRainer(Vector3 position)
    {
        var rainerObj = Instantiate(rainerPrefab, position, Quaternion.identity, transform);
        var rainer = rainerObj.GetComponent<RainerController>();
        rainer.SetIdle(position);

        rainers.Add(rainer);
    }

    public static Material GetMaterial(int playerNo)
    {
        return Instance.materials[playerNo];
    }
}
