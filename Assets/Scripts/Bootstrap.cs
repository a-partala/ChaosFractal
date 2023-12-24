using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private DotCreator dotCreator;

    private void Start()
    {
        if(dotCreator != null)
        {
            dotCreator.Initialize();
        }
    }
}
