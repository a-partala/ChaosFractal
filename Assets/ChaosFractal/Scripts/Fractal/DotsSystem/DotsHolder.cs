using System;
using System.Collections.Generic;
using UnityEngine;
using static ChaosFractal.FractalCreator;

namespace ChaosFractal
{
    public class DotsHolder
    {
        public List<Transform> StartDots { get; private set; }
        private List<Transform> AllDots;
        private Transform lastDot;
        public Transform LastDot
        { 
            get
            {
                if (lastDot == null)
                {
                    lastDot = GetRandomStartDot();
                }
                return lastDot;
            }

            private set
            {
                lastDot = value;
            }
        }
        private Dictionary<State, Action> statesMap;

        public DotsHolder()
        {
            AssignMap();
            AssignLists();
        }

        public void AddDot(Transform dot)
        {
            AllDots.Add(dot);
            LastDot = dot;
        }

        public void ChangeState(State state)
        {
            statesMap[state]?.Invoke();
        }

        public void DestroyDots()
        {
            StartDots.Clear();
            foreach (var dot in AllDots)
            {
                UnityEngine.Object.Destroy(dot.gameObject);
            }
            AllDots.Clear();
        }

        private void HideBorders()
        {
            foreach (var dot in StartDots)
            {
                UnityEngine.Object.Destroy(dot.GetChild(0).gameObject);
            }
        }

        public Transform GetRandomStartDot()
        {
            if (StartDots.Count == 0)
            {
                return null;
            }
            int rand = UnityEngine.Random.Range(0, StartDots.Count);
            return StartDots[rand];
        }

        #region Assigners

        private void AssignMap()
        {
            statesMap = new()
            {
                { State.PlaceStartDots, DestroyDots },
                { State.CreateFractal, HideBorders },
            };
        }

        private void AssignLists()
        {
            StartDots = new();
            AllDots = new();
        }

        #endregion
    }
}