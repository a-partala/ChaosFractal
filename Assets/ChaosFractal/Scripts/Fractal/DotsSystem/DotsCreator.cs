using System;
using UnityEngine;

namespace ChaosFractal
{
    [Serializable]
    public class DotsCreator
    {
        [SerializeField] private GameObject startDotPrefab;
        [SerializeField] private GameObject dotPrefab;
        [SerializeField, Range(0, 10)] private float dotScale = 1f;

        public Transform CreateStartDot(Vector3 pos)
        {
            var dot = UnityEngine.Object.Instantiate(startDotPrefab);
            dot.transform.position = pos;
            dot.transform.localScale *= dotScale;
            return dot.transform;
        }

        public Transform CreateNewDot(Vector3 pos)
        {
            var dot = UnityEngine.Object.Instantiate(dotPrefab);
            dot.transform.position = pos;
            dot.transform.localScale *= dotScale;
            return dot.transform;
        }
    }
}
