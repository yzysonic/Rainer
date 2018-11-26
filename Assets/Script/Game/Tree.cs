using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RainerLib
{

    public class Tree : MonoBehaviour
    {

        public int normalScore = 2;
        public int bonusScore = 50;
        public float scoreTime = 0.5f;
        public float growTime = 10;

        private Timer scoreTimer;
        private Timer growTimer;
        private Vector3 treeMaxScl;

        public bool IsEndGrow { get; private set; }
        public bool IsGrowing { get; private set; }
        public Transform Model { get; private set; }
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
            scoreTimer = new Timer(scoreTime);
            growTimer = new Timer(growTime);
            Model = transform.Find("Model");
            treeMaxScl = Model.localScale;
            Model.localScale = Vector3.zero;
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

        // Update is called once per frame
        void Update()
        {
            if (IsEndGrow)
            {
                enabled = false;
                return;
            }

            growTimer.Step(Speed);
            scoreTimer.Step(Speed);
            Model.localScale = Vector3.Lerp(Vector3.zero, treeMaxScl, growTimer.Progress);

            if (scoreTimer.TimesUp())
            {
                scoreTimer.Reset();
                ScoreManager.Instance.AddScore(PlayerNo, normalScore);
                //スコア更新

            }

            if (growTimer.TimesUp())
            {
                //ボーナススコア更新
                ScoreManager.Instance.AddScore(PlayerNo, bonusScore);
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
    }

}