using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RainerLib
{

    public class Tree : MonoBehaviour
    {
        public float growTime = 10;
        public float miniGrassRadius = 4;
        public float grassRadius = 16;
        public float miniGrassSpwanInterval = 0.3f;
        public float grassSpawnInterval = 1.0f;

        private Timer growTimer;
        private Timer grassTimer;
        private Vector3 treeMaxScl;

        public bool IsEndGrow { get; private set; }
        public bool IsGrowing { get; private set; }
        public float Progress
        {
            get
            {
                return growTimer.Progress;
            }
        }
        public int PlayerNo { get; set; }
        public float Speed { get; set; }
        public Seed Seed { get; private set; }


        private void Awake()
        {
            growTimer = new Timer(growTime);
            grassTimer = new Timer();
            treeMaxScl = transform.localScale;
            transform.localScale = Vector3.zero;
            IsEndGrow = false;
            PlayerNo = -1;
            Speed = 1.0f;
            Seed = transform.parent.GetComponent<Seed>();
        }

        // Use this for initialization
        void Start()
        {
            Seed.enabled = false;
        }

        private void OnEnable()
        {
            if (Ground.IsCreated)
            {
                StartCoroutine(SpawnGrass(miniGrassRadius, miniGrassSpwanInterval));
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (IsEndGrow)
            {
                enabled = false;
                return;
            }

            growTimer.Step(Speed);
            transform.localScale = Vector3.Lerp(Vector3.zero, treeMaxScl, growTimer.Progress);

            if (growTimer.TimesUp())
            {
                //ボーナススコア更新
                if (Ground.IsCreated)
                {
                    StartCoroutine(SpawnGrass(grassRadius, grassSpawnInterval));
                }
                IsEndGrow = true;
                IsGrowing = false;
                enabled = false;
            }

        }

        public void StartGrow()
        {
            if(IsGrowing || PlayerNo < 0)
            {
                return;
            }

            enabled = true;
            Speed = 1.0f;
            IsGrowing = true;
        }

        public void StopGrow()
        {
            PlayerNo = -1;
            IsGrowing = false;
            enabled = false;
        }

        IEnumerator SpawnGrass(float radius, float time)
        {
            var ground = Ground.Instance;
            var uv = Ground.Instance.GetUV(transform.position, 1.0f);
            grassTimer.Reset(time);
            while (!grassTimer.TimesUp())
            {
                grassTimer++;
                ground.GrowGrass(uv, PlayerNo, Mathf.Lerp(0.0f, radius, grassTimer.Progress));
                yield return null;
            }
        }
    }

}