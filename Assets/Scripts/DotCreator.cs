using System.Collections.Generic;
using UnityEngine;

public class DotCreator : MonoBehaviour
{
    private enum State { CreateStartDots, MakeAFractal }
    private State state = State.CreateStartDots;

    [Header("Prefabs")]
    [SerializeField] private GameObject borderedDotPrefab;
    [SerializeField] private GameObject dotPrefab;

    [Header("UI")]
    [SerializeField] private GameObject CreateStartDotsUI;
    [SerializeField] private GameObject MakeAFractalUI;

    private List<Transform> startDots = new List<Transform>();
    private List<Transform> allDots = new List<Transform>();
    private Transform lastDot = null;

    private void Start()
    {
        UpdateState(State.CreateStartDots);
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

    private void UpdateState(State state)
    {
        var newState = state;
        switch (newState)
        {
            case State.CreateStartDots:
                ClearAllDots();
                CreateStartDotsUI.SetActive(true);
                MakeAFractalUI.SetActive(false);
                break;
            case State.MakeAFractal:
                foreach (var dot in startDots)
                {
                    var child = dot.GetChild(0);
                    if (child == null)
                    {
                        continue;
                    }
                    Destroy(child.gameObject);
                }
                CreateStartDotsUI.SetActive(false);
                MakeAFractalUI.SetActive(true);
                break;
        }
        this.state = newState;
    }

    [SerializeField]
    private void UpdateState(int state)
    {
        var newState = (State)state;
        switch (newState)
        {
            case State.CreateStartDots:
                ClearAllDots();
                CreateStartDotsUI.SetActive(true);
                MakeAFractalUI.SetActive(false);
                break;
            case State.MakeAFractal:
                foreach(var dot in startDots)
                {
                    var child = dot.GetChild(0);
                    if(child == null)
                    {
                        continue;
                    }
                    Destroy(child.gameObject);
                }
                CreateStartDotsUI.SetActive(false);
                MakeAFractalUI.SetActive(true);
                break;
        }
        this.state = (State)newState;
    }

    private void ClearAllDots()
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
            lastDot = GetRandomMainDot();
            if(lastDot == null)
            {
                Debug.Log("There is no last dot");
                return;
            }
        }
        var dot = Instantiate(dotPrefab);
        dot.SetActive(true);
        dot.transform.position = GetHalfPathFromLastToRandomMainDot();
        allDots.Add(dot.transform);
        lastDot = dot.transform;
    }

    private Vector3 GetHalfPathFromLastToRandomMainDot()
    {
        return GetHalfPathToRandomMainDot(lastDot.position);
    }

    private Vector3 GetHalfPathToRandomMainDot(Vector3 dotPos)
    {
        return (GetRandomMainDotPos() + dotPos) / 2f;
    }

    private Vector3 GetRandomMainDotPos()
    {
        Transform mainDot = GetRandomMainDot();
        if(mainDot == null)
        {
            Debug.LogError("There is no start dots");
            return Vector3.zero;
        }
        return GetRandomMainDot().position;
    }

    private Transform GetRandomMainDot()
    {
        if(startDots.Count == 0)
        {
            return null;
        }
        int rand = Random.Range(0, startDots.Count);
        return startDots[rand];
    }
}
