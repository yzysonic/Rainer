using System;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;

public class RainerController : Rainer
{
    public enum State
    {
        Free,
        Follow,
        MoveToTree,
        GrowTree
    }

    static readonly float max_force = 0.3f;

    RainerManager manager;
    List<RainerController> boids = new List<RainerController>();
    State state;
    Vector3 move;
    Tree targetTree;
    float speed;

    public Rainer Leader { get; private set; }
    public Renderer CoatRenderer { get; private set; }

    public override int PlayerNo
    {
        get
        {
            return Leader?.PlayerNo ?? -1;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        manager = RainerManager.Instance;
        state = State.Free;
        gameObject.layer = RainerManager.LayerRainerIdle;
        CoatRenderer = Model.GetChild(1).GetComponent<Renderer>();
    }

    protected override void Update()
    {

        switch (state)
        {
            case State.Follow:

                // 周辺のrainer.gameObjectをboidsに設定
                boids = manager.GetBoidsNearby(this);

                move +=
                    MoveSeparate(manager.avoid_range) * 1.4f
                    + MoveAlign()
                    + MoveConhesion()
                    + MoveChase(manager.avoid_range);

                move = Vector3.ClampMagnitude(move, manager.max_speed);

                SetSpeed(Vector3.Lerp(move, Leader.CharacterController.velocity, 0.1f).magnitude);

                break;


            case State.MoveToTree:

                move = targetTree.transform.position - transform.position;

                if(move.sqrMagnitude < 4.0f)
                {
                    //StartGrowTree(targetTree);
                    targetTree.enabled = true;
                    state = State.GrowTree;
                    move = Vector3.zero;
                    goto case State.GrowTree;
                }

                move = Vector3.ClampMagnitude(move, manager.max_speed);

                break;


            case State.GrowTree:

                if (!targetTree.IsEndGrow)
                {
                    targetTree.Grow(PlayerNo);
                }
                else
                {
                    targetTree = null;
                    SetFree();
                }

                break;

        }

        // 移動する
        CharacterController.SimpleMove(move);

        base.Update();
    }

    // 距離をとる
    public Vector3 MoveSeparate(float range)
    {
        var steer = Vector3.zero;
        var vec = Vector3.zero;
        int cnt = 0;

        foreach (RainerController boid in boids)
        {
            vec = gameObject.transform.position - boid.transform.position;
            if (vec.magnitude != 0.0f && vec.magnitude < range)
            {
                steer += vec.normalized / vec.magnitude;
                cnt++;
            }
        }

        vec = gameObject.transform.position - Leader.transform.position;
        if (vec.magnitude != 0.0f && vec.magnitude < range)
        {
            steer += vec.normalized / vec.magnitude;
            cnt++;
        }

        if (cnt > 0)
        {
            steer /= (float)cnt;
            steer = steer.normalized * speed - move;
        }

        if (steer.sqrMagnitude > max_force * max_force)
        {
            steer = steer.normalized * max_force;
        }

        return steer;
    }

    // 平均方向へ向かう
    public Vector3 MoveAlign()
    {
        var sum = Vector3.zero;

        foreach (RainerController boid in boids)
        {
            sum += boid.move;
        }

        sum += Leader.CharacterController.velocity;
        
        if (boids.Count != 0)
        {
            sum /= (float)(boids.Count + 1);
            sum = sum.normalized * speed - move;
        }

        if (sum.sqrMagnitude > max_force * max_force)
        {
            sum = sum.normalized * max_force;
        }

        return sum;
    }

    // 平均位置へ向かう
    public Vector3 MoveConhesion()
    {
        var sum = Vector3.zero;
        var vec = Vector3.zero;

        foreach (RainerController boid in boids)
        {
            sum += boid.transform.position;
        }

        sum += Leader.transform.position;

        sum /= (float)(boids.Count + 1);
        vec = (sum - gameObject.transform.position).normalized * speed - move;
        

        if (vec.sqrMagnitude > max_force * max_force)
        {
            vec = vec.normalized * max_force;
        }

        return vec;
    }

    // 所持者を追う
    public Vector3 MoveChase(float range)
    {
        var vec = Leader.transform.position - gameObject.transform.position;

        if (vec.magnitude != 0.0f && vec.magnitude > range)
        {
            vec = vec.normalized * speed * speed - move;
        }
        else
        {
            vec = -vec.normalized * speed * speed - move;
        }

        if (vec.sqrMagnitude > max_force * max_force)
        {
            vec = vec.normalized * max_force;
        }

        return vec;
    }

    // 速度を設定
    public void SetSpeed(float value)
    {
        speed = value;
    }

    // 待機状態にする
    public void SetFree()
    {
        state = State.Free;
        gameObject.layer = RainerManager.LayerRainerIdle;
        CoatRenderer.material = manager.GetDefaultMaterial();
        MinimapIcon.gameObject.SetActive(true);
    }

    public void SetGrowTree(Tree tree)
    {
        state = State.MoveToTree;
        gameObject.layer = RainerManager.LayerRainerIdle;
        targetTree = tree;
    }

    // 追跡状態にする
    public void SetFollow(PlayerController player)
    {
        Leader = player;
        state = State.Follow;
        gameObject.layer = RainerManager.LayerRainerFollow;
        CoatRenderer.material = manager.GetMaterial(PlayerNo);
        MinimapIcon.gameObject.SetActive(false);
    }

}
