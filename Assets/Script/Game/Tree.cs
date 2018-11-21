using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RainerLib
{

    public class Tree : MonoBehaviour
    {

        public int growScore = 2;
        public int bonusScore = 50;
        public float scoreTime = 0.5f;
        public float growTime = 10;

        private Timer scoreTimer;
        private Timer bonusTimer;
        private Vector3 treeMaxScl;

        public bool IsEndGrow { get; private set; }
        public bool IsGrowing { get; private set; }
        public Transform Model { get; private set; }


        private void Awake()
        {
            scoreTimer = new Timer(scoreTime);
            bonusTimer = new Timer(growTime);
            Model = transform.Find("Model");
            treeMaxScl = Model.localScale;
            Model.localScale = Vector3.zero;
            IsEndGrow = false;

            var ground = Ground.Instance;
            ground.PaintGrass.Paint(ground.GetUV(transform.position, 1.0f));
        }

        // Use this for initialization
        void Start()
        {
            transform.Find("SeedIcon")?.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            IsGrowing = false;
        }

        private void OnDisable()
        {
            IsGrowing = false;
        }

        public void Grow(int playerNo)
        {
            if (IsEndGrow || !enabled)
            {
                return;
            }

            IsGrowing = true;

            bonusTimer.Step();
            scoreTimer.Step();
            Model.localScale = Vector3.Lerp(Vector3.zero, treeMaxScl, bonusTimer.Progress);

            if (scoreTimer.TimesUp())
            {
                scoreTimer.Reset();
                //ScoreManager.Instance.AddScore(playerNo, growScore);
                //スコア更新

            }

            if (bonusTimer.TimesUp())
            {
                //ボーナススコア更新
                //ScoreManager.Instance.AddScore(playerNo, bonusScore);
                IsEndGrow = true;
            }

        }
    }

}