using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RainerLib
{
    [DisallowMultipleComponent]
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static Singleton<T> instance;
        public static T Instance
        {
            get
            {
                return instance ? (T)instance : new GameObject().AddComponent<T>();
            }
        }
        public static bool IsCreated
        {
            get
            {
                return instance != null;
            }
        }

        protected virtual void Awake()
        {
            name = typeof(T).ToString();

            if (instance == this)
            {
                name += " (Singleton)";
            }
            else
            {
                name += " (Duplicated Singleton)";
                gameObject.SetActive(false);
            }
        }

        protected Singleton()
        {
            if (instance != null)
                throw new Exception($"シングルトンのインスタンスが複数生成された: {typeof(T).ToString()}");

            instance = this;
        }

    }

    public class Timer
    {
        protected float interval;
        protected float elapsed;

        public float Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value;
                UpdateProgress();
            }
        }
        public float Elapsed
        {
            get
            {
                return elapsed;
            }
            set
            {
                elapsed = value;
                UpdateProgress();
            }
        }
        public float Progress { get; protected set; }

        public Timer(float interval = 1.0f)
        {
            this.interval = interval;
        }

        public static Timer operator ++(Timer timer)
        {
            timer.Step();
            return timer;
        }

        public void Step()
        {
            Elapsed += Time.deltaTime;
        }

        public void Reset()
        {
            Elapsed = 0.0f;
        }

        public void Reset(float interval)
        {
            this.interval = interval;
            Elapsed = 0.0f;
        }

        public bool TimesUp()
        {
            return Progress >= 1.0f;
        }

        virtual protected void UpdateProgress()
        {
            Progress = elapsed / interval;
        }
    }

}

namespace RainerLib.Development
{
    public class StateMachine<T> where T : struct
    {
        public class State
        {
            public T Type { get; private set; }
            public StateMachine<T> Host { get; protected set; }

            protected State(StateMachine<T> host, T type)
            {
                Type = type;
                Host = host;
            }

            public virtual void Enter() { }
            public virtual void Update() { }
            public virtual bool Exit(State nextState) { return true; }
        }

        private State currentState;

        public State CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                if (currentState.Exit(value))
                {
                    currentState = value;
                    currentState.Enter();
                }
            }
        }

        public StateMachine(State initState)
        {
            currentState = initState;
            initState.Enter();
        }

        public void Update()
        {
            currentState.Update();
        }
    }

}