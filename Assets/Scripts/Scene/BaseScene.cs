using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    Define.Scene scene_type;
    public Define.Scene SceneType { get; protected set; } = Define.Scene.None;

    protected virtual void Init()
    {
        
    }

    public abstract void Clear();
}