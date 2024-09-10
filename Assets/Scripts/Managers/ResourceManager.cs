using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public GameObject Instantiate(string path, Vector3 worldPos, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }
        
        GameObject go = Object.Instantiate(original, worldPos, Quaternion.identity);
        go.name = original.name;
        return go;
    }

    public GameObject Instantiate(string path, Vector3 worldPos, Quaternion rotation, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go = Object.Instantiate(original, worldPos, Quaternion.identity, parent);
        go.name = original.name;
        return go;
    }

    /// <summary>
    /// 기본 경로 : "Resources/Animators/"
    /// </summary>
    public RuntimeAnimatorController GetAnimator(string path)
    {
        RuntimeAnimatorController animator = Load<RuntimeAnimatorController>($"Animators/{path}");

        return animator;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
