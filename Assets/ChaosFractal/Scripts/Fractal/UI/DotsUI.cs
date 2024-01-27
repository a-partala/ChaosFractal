using System;
using System.Collections.Generic;
using UnityEngine;
using static ChaosFractal.FractalCreator;

namespace ChaosFractal
{
    [Serializable]
    public class DotsUI
    {
        [SerializeField] private GameObject PlaceStartDotsUI;
        [SerializeField] private GameObject CreateFractalUI;

        private Dictionary<State, GameObject> canvasMap;

        public void Initialize()
        {
            AssignMap();
        }

        public void ChangeState(State state)
        {
            foreach (var key in canvasMap.Keys)
            {
                bool isStateMatch = key == state;
                canvasMap[key].SetActive(isStateMatch);
            }
        }

        private void AssignMap()
        {
            canvasMap = new()
            {
                { State.PlaceStartDots, PlaceStartDotsUI },
                { State.CreateFractal, CreateFractalUI },
            };
        }
    }
}
