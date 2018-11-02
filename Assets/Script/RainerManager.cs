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
    
    List<GameObject> rainers = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        SpawnRainer(new Vector3(10,1,10));
        SpawnRainer(new Vector3(0,1,10));
        SpawnRainer(new Vector3(10,1,0));
        SpawnRainer(new Vector3(10,1,-10));
        SpawnRainer(new Vector3(0,1,-10));
        SpawnRainer(new Vector3(-10,1,10));
        SpawnRainer(new Vector3(-10,1,0));
        SpawnRainer(new Vector3(-10,1,-10));
    }

    // Update is called once per frame
    void FixedUpdate()
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
    }

    void SpawnRainer(Vector3 position)
    {
        var rainerObj = Instantiate(rainerPrefab, position, Quaternion.identity);

        rainerObj.GetComponent<RainerController>().SetIdle(position);

        rainers.Add(rainerObj);
    }
}
