using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainerManager : MonoBehaviour {
    //[SerializeField]
    //List<GameObject> players;
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

    List<GameObject> rainers = new List<GameObject>();

    public Transform spawnGroup;
    List<Transform> spawnList = new List<Transform>();
    int timer;

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
            var rainerController = rainer.GetComponent<RainerController>();
            
            if (rainer.layer == LayerMask.NameToLayer("RainerFollow"))
            {
                // 周辺のrainerをboidsに設定
                rainerController.FindBoidsNearby(rainers, unity_range);

                rainerController.move +=
                    rainerController.MoveSeparate(avoid_range) * 1.4f
                    + rainerController.MoveAlign()
                    + rainerController.MoveConhesion()
                    + rainerController.MoveChase(avoid_range);

                if (rainerController.move.magnitude > max_speed)
                {
                    rainerController.move = rainerController.move.normalized * max_speed;
                }

                // 移動する
                rainer.GetComponent<CharacterController>().SimpleMove(rainerController.move);

                rainerController.SetSpeed(Vector3.Lerp(rainerController.move, rainerController.leader.GetComponent<CharacterController>().velocity, 0.1f).magnitude);
            }
            else if (rainer.layer == LayerMask.NameToLayer("RainerIdle"))
            {
                rainerController.move = rainerController.point - rainer.transform.position;

                if (rainerController.move.magnitude > max_speed)
                {
                    rainerController.move = rainerController.move.normalized * max_speed;
                }

                // 移動する
                rainer.GetComponent<CharacterController>().SimpleMove(rainerController.move);
            }
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
        var rainerObj = Instantiate(rainerPrefab, position, Quaternion.identity);

        rainerObj.GetComponent<RainerController>().SetIdle(position);

        rainers.Add(rainerObj);
    }
}
