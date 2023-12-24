using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DotCreator : MonoBehaviour
{
    public enum State { CreateStartDots, MakeAFractal }
    [Serializable]
    public class StateSetup
    {
        public State State;
        public UnityEvent Event;
    }
    private State state = State.CreateStartDots;
    [SerializeField] private float lerpToDotCoef = 0.5f;

    [SerializeField] private StateSetup[] stateSetups = new StateSetup[2];
    private Dictionary<State, StateSetup> stateSetupsMap = new Dictionary<State, StateSetup>();

    [Header("Prefabs")]
    [SerializeField] private GameObject borderedDotPrefab;
    [SerializeField] private GameObject dotPrefab;

    private List<Transform> startDots = new List<Transform>();
    private List<Transform> allDots = new List<Transform>();
    private Transform lastDot = null;
    private bool isInitialized = false;

    [HideInInspector]
    public void Initialize()
    {
        if(isInitialized)
        {
            return;
        }
        isInitialized = true;
        AssignSetupDictionary();
        UpdateState(State.CreateStartDots);
    }

    private void AssignSetupDictionary()
    {
        if(stateSetups == null || stateSetups.Length == 0)
        {
            Debug.LogError($"{nameof(stateSetups)} is not assigned");
            return;
        }

        stateSetupsMap.Clear();
        foreach(var item in stateSetups)
        {
            if(stateSetupsMap.ContainsKey(item.State))
            {
                continue;
            }
            stateSetupsMap.Add(item.State, item);
        }

        if(stateSetupsMap.Count < Enum.GetValues(typeof(State)).Length)
        {
            Debug.LogError($"{nameof(stateSetups)} is less than {typeof(State)} enum");
        }
    }

    public void CreateStartDotInTouch()
    {
        if(Input.touches.Length == 0)
        {
            return;
        }
        var touch = Input.GetTouch(0);
        Vector2 worldTouchPos = Camera.main.ScreenToWorldPoint(touch.position);
        CreateStartDotInPos(worldTouchPos);
    }

    public void UpdateState(int state)
    {
        UpdateState((State)state);
    }

    public void UpdateState(State state)
    {
        this.state = state;
        stateSetupsMap[state].Event?.Invoke();
    }

    public void ClearDotsChildren()
    {
        foreach (var dot in startDots)
        {
            var child = dot.GetChild(0);
            if (child == null)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
    }

    public void ClearAllDots()
    {
        foreach (var dot in startDots)
        {
            Destroy(dot.gameObject);
        }
        startDots.Clear();
        foreach(var dot in allDots)
        {
            Destroy(dot.gameObject);
        }
        allDots.Clear();
    }

    private void CreateStartDotInPos(Vector3 pos)
    {
        var dot = Instantiate(borderedDotPrefab);
        dot.SetActive(true);
        dot.transform.position = pos;
        startDots.Add(dot.transform);
    }

    public void CreateNewDots(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            CreateNewDot();
        }
    }

    public void CreateNewDot()
    {
        if(lastDot == null)
        {
            lastDot = GetRandomStartDot();
            if(lastDot == null)
            {
                Debug.Log("There is no last dot");
                return;
            }
        }
        var dot = Instantiate(dotPrefab);
        dot.SetActive(true);
        dot.transform.position = LerpToRandomStartDot(lastDot.position);
        allDots.Add(dot.transform);
        lastDot = dot.transform;
    }

    private Vector3 LerpToRandomStartDot(Vector3 dotPos)
    {
        var newPos = Vector3.Lerp(dotPos, GetRandomStartDotPos(), lerpToDotCoef);
        return newPos;
    }

    private Vector3 GetRandomStartDotPos()
    {
        Transform mainDot = GetRandomStartDot();
        if(mainDot == null)
        {
            Debug.LogError("There is no start dots");
            return Vector3.zero;
        }
        return mainDot.position;
    }

    private Transform GetRandomStartDot()
    {
        if(startDots.Count == 0)
        {
            return null;
        }
        int rand = UnityEngine.Random.Range(0, startDots.Count);
        return startDots[rand];
    }
}
