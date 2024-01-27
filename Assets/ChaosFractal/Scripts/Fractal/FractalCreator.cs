using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ChaosFractal
{
    public class FractalCreator : MonoBehaviour
    {
        [Serializable]
        public class Events
        {
            public UnityEvent OnFractalCancel;
            public UnityEvent OnStartDotsPlaced;
        }
        public enum State { PlaceStartDots, CreateFractal }
        [SerializeField] private DotsUI dotsUI;
        [SerializeField] private DotsCreator dotsCreator;
        [SerializeField] private Events events;
        private DotsHolder dotsHolder;
        [SerializeField, Range(0, 1)] private float lerpToDotCoef = 0.5f;
        private Dictionary<State, UnityEvent> stateEventsMap;

        public event Action<State> OnStateChange;

        public void Start()
        {
            dotsHolder = new();
            dotsUI.Initialize();
            SubscribeOnStateChange();
            AssignStateEventsMap();
            UpdateState(State.PlaceStartDots);
        }

        public void UpdateState(State state)
        {
            OnStateChange?.Invoke(state);
        }

        private void SubscribeOnStateChange()
        {
            OnStateChange += dotsHolder.ChangeState;
            OnStateChange += dotsUI.ChangeState;
            OnStateChange += InvokeStateEvents;
        }

        private void InvokeStateEvents(State state)
        {
            stateEventsMap[state]?.Invoke();
        }

        public void CreateStartDotInTouch()
        {
            if (Input.touches.Length == 0)
            {
                return;
            }
            var touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            var dot = dotsCreator.CreateStartDot(touchPos);
            dotsHolder.StartDots.Add(dot);
            dotsHolder.AddDot(dot);
        }

        public void ArrangeDots(int number)
        {
            for(int i = 0; i < number; i++)
            {
                var dotPos = LerpFromLastToRandomStartDot();
                var dot = dotsCreator.CreateNewDot(dotPos);
                dotsHolder.AddDot(dot);
            }
        }

        public void CompleteStartPlacement()
        {
            UpdateState(State.CreateFractal);
        }

        public void CancelFractal()
        {
            UpdateState(State.PlaceStartDots);
        }

        private Vector3 LerpFromLastToRandomStartDot()
        {
            return LerpToRandomStartDot(dotsHolder.LastDot.position);
        }

        private Vector3 LerpToRandomStartDot(Vector3 dotPos)
        {
            return Vector3.Lerp(dotsHolder.GetRandomStartDot().position, dotPos, lerpToDotCoef);
        }

        private void AssignStateEventsMap()
        {
            stateEventsMap = new()
            {
                { State.PlaceStartDots, events.OnFractalCancel },
                { State.CreateFractal, events.OnStartDotsPlaced },
            };
        }
    }
}
