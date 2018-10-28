using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RainerLib
{

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                var name = typeof(T).ToString();
                var gameObject = GameObject.Find(name);

                if (gameObject != null)
                    return instance = gameObject.GetComponent<T>();

                return instance = new GameObject(name).AddComponent<T>();
            }
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

    public class StateMachine<StateType> where StateType : struct
    {
        private StateType currentState;
        private Action<StateType> enterStateAction;
        private Action<StateType> updateStateAction;

        public Action<StateType> EnterStateAction
        {
            get
            {
                return enterStateAction;
            }
            set
            {
                enterStateAction = value ?? DoNothing;
            }
        }
        public Action<StateType> UpdateStateAction
        {
            get
            {
                return updateStateAction;
            }
            set
            {
                updateStateAction = value ?? DoNothing;
            }
        }

        public StateType CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                if(!currentState.Equals(value))
                {
                    currentState = value;
                    EnterStateAction(value);
                }                
            }
        }

        public StateMachine(Action<StateType> enterStateAction, Action<StateType> updateStateAction)
        {
            EnterStateAction = enterStateAction;
            UpdateStateAction = updateStateAction;
        }
        public void Start(StateType initState)
        {
            currentState = initState;
            EnterStateAction(initState);
        }
        public void Update()
        {
            UpdateStateAction(currentState);
        }

        private void DoNothing(StateType action) { }
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

            public static bool operator ==(State a, State b)
            {
                return a.Type.Equals(b.Type);
            }
            public static bool operator !=(State a, State b)
            {
                return !a.Type.Equals(b.Type);
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