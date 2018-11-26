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

        protected virtual void OnDestroy()
        {
            if(instance == this)
            {
                instance = null;
            }
        }

        protected Singleton()
        {
            if (instance != null)
                throw new Exception($"シングルトンのインスタンスが複数生成された: {typeof(T).ToString()}");

            instance = this;
        }

        public void Destroy()
        {
            Destroy(gameObject);
            instance = null;
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

        public void Step()
        {
            Elapsed += Time.deltaTime;
        }

        public void Step(float speedScale)
        {
            Elapsed += Time.deltaTime * speedScale;
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
        public static Timer operator ++(Timer timer)
        {
            timer.Step();
            return timer;
        }

        public static implicit operator float(Timer timer)
        {
            return timer.elapsed;
        }

        public static implicit operator bool(Timer timer)
        {
            return timer.TimesUp();
        }

    }

    public class Pool<T> : MonoBehaviour where T : Component, IPoolObject
    {

        public GameObject perfab;
        public int maxCount;

        private List<T> list;

        protected virtual void Awake()
        {
            list = new List<T>(maxCount);
            for(var i = 0; i < maxCount; i++)
            {
                var t = Instantiate(perfab, transform).GetComponent<T>();
                t.gameObject.SetActive(false);
                list.Add(t);
            }
        }

        public T Get()
        {
            foreach (var obj in list)
            {
                if (!obj.IsUsing)
                {
                    obj.Init();
                    obj.gameObject.SetActive(true);
                    obj.IsUsing = true;
                    return obj;
                }
            }

            return null;
        }

        public T Get(Transform parent)
        {
            var obj = Get();
            if (obj != null)
            {
                obj.transform.parent = parent;
            }
            return obj;
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            var obj = Get();
            if (obj != null)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
            }
            return obj;

        }

        public T Get(Vector3 position, Quaternion rotation, Transform parent)
        {
            var obj = Get();
            if(obj != null)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.transform.parent = parent;
            }
            return obj;
        }


    }

    public interface IPoolObject
    {
        bool IsUsing { get; set; }
        void Init();
        void Uninit();
    }

    public struct Int2
    {
        public int x;
        public int y;

        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Int2(Vector2 v)
        {
            x = (int)v.x;
            y = (int)v.y;
        }

        public static implicit operator Vector2(Int2 int2)
        {
            return new Vector2(int2.x, int2.y);
        }

        public static explicit operator Int2(Vector2 v)
        {
            return new Int2(v);
        }

        public static Int2 operator +(Int2 a, Int2 b)
        {
            return new Int2(a.x + b.x, a.y + b.y);
        }

        public static Int2 operator -(Int2 int2)
        {
            return new Int2(-int2.x, -int2.y);
        }

        public static Int2 operator -(Int2 a, Int2 b)
        {
            return a + (-b);
        }

        public static Int2 operator *(Int2 int2, int i)
        {
            return new Int2(int2.x * i, int2.y * i);
        }

        public static Vector2 operator *(Int2 int2, float f)
        {
            return new Vector2(int2.x * f, int2.y * f);
        }

        public static Int2 operator *(int i, Int2 int2)
        {
            return int2 * i;
        }

        public static Vector2 operator *(float f, Int2 int2)
        {
            return int2 * f;
        }

        public static Int2 operator /(Int2 a, int d)
        {
            return new Int2(a.x / d, a.y / d);
        }

        public static Vector2 operator /(Int2 a, float d)
        {
            return new Vector2(a.x / d, a.y / d);
        }

        public static bool operator ==(Int2 lhs, Int2 rhs)
        {
            return lhs.x == rhs.x && lhs.x == rhs.y;
        }

        public static bool operator !=(Int2 lhs, Int2 rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return (Int2)obj == this;
        }

            // override object.GetHashCode
        public override int GetHashCode()
        {
            return (x << 2) ^ y;
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