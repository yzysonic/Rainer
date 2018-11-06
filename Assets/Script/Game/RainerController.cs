﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class RainerController : MonoBehaviour
{
    public Vector3 move;
    List<GameObject> boids = new List<GameObject>();
    public GameObject leader;
    public Vector3 point;
    float speed;
    static readonly float max_force = 0.3f;

    // 周辺のrainerをboidsに設定
    public void FindBoidsNearby(List<GameObject> rainers, float range)
    {
        boids.Clear();
        foreach (GameObject rainer in rainers)
        {
            if (rainer != gameObject
                && rainer.layer == LayerMask.NameToLayer("RainerFollow")
                && rainer.GetComponent<RainerController>().leader == leader
                )
            {
                var vec = rainer.transform.position - gameObject.transform.position;

                if (vec.magnitude < range)
                {
                    boids.Add(rainer);
                }
            }
        }
    }

    // 距離をとる
    public Vector3 MoveSeparate(float range)
    {
        var steer = Vector3.zero;
        var vec = Vector3.zero;
        int cnt = 0;

        foreach (GameObject boid in boids)
        {
            vec = gameObject.transform.position - boid.transform.position;
            if (vec.magnitude != 0.0f && vec.magnitude < range)
            {
                steer += vec.normalized / vec.magnitude;
                cnt++;
            }
        }

        vec = gameObject.transform.position - leader.transform.position;
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

        foreach (GameObject boid in boids)
        {
            sum += boid.GetComponent<RainerController>().move;
        }

        sum += leader.GetComponent<CharacterController>().velocity;
        
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

        foreach (GameObject boid in boids)
        {
            sum += boid.transform.position;
        }

        sum += leader.transform.position;

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
        var vec = leader.transform.position - gameObject.transform.position;

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
    public void SetIdle(Vector3 position)
    {
        gameObject.layer = LayerMask.NameToLayer("RainerIdle");
        point = position;
    }

    // 追跡状態にする
    public void SetFollow(GameObject player)
    {
        gameObject.layer = LayerMask.NameToLayer("RainerFollow");
        leader = player;
    }

}