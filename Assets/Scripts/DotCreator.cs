using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCreator : MonoBehaviour
{
    private enum State { CreateMainDots, MakeAFractal }
    private State state = State.CreateMainDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private GameObject dotClearPrefab;
    [SerializeField] private GameObject CreateMainDotsUI;
    [SerializeField] private GameObject MakeAFractalUI;
    private List<Transform> mainDots = new List<Transform>();
    private List<Transform> allDots = new List<Transform>();
    private Transform lastDot = null;

    private void Awake()
    {
        UpdateState(0);
    }

    public void CreateMainDot2D()
    {
        if(Input.touches.Length == 0)
        {
            return;
        }
        var touch = Input.GetTouch(0);
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        CreateMainDotInPos(touchPos);
    }

    public void UpdateState(int state)
    {
        var newState = (State)state;
        switch (newState)
        {
            case State.CreateMainDots:
                ClearAllDots();
                CreateMainDotsUI.SetActive(true);
                MakeAFractalUI.SetActive(false);
                break;
            case State.MakeAFractal:
                foreach(var dot in mainDots)
                {
                    Destroy(dot.GetChild(0).gameObject);
                }
                CreateMainDotsUI.SetActive(false);
                MakeAFractalUI.SetActive(true);
                break;
        }
    }

    private void ClearAllDots()
    {
        foreach(var dot in mainDots)
        {
            Destroy(dot.gameObject);
        }
        mainDots.Clear();
        foreach(var dot in allDots)
        {
            Destroy(dot.gameObject);
        }
        allDots.Clear();
    }

    private void CreateMainDotInPos(Vector3 pos)
    {
        var dot = Instantiate(dotPrefab);
        dot.SetActive(true);
        dot.transform.position = pos;
        mainDots.Add(dot.transform);
    }

    public void CreateNewDots(int amount = 1)
    {
        MakeAFractalUI.SetActive(false);
        for (int i = 0; i < amount; i++)
        {
            CreateNewDot();
        }
        MakeAFractalUI.SetActive(true);
    }

    public void CreateNewDot()
    {
        if(lastDot == null)
        {
            lastDot = GetRandomMainDot();
        }
        var dot = Instantiate(dotClearPrefab);
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
        return GetRandomMainDot().position;
    }

    private Transform GetRandomMainDot()
    {
        if(mainDots.Count == 0)
        {
            return null;
        }
        int rand = Random.Range(0, mainDots.Count);
        return mainDots[rand];
    }
}
