using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    //============================= //
    //    [Vector3 Extensions]
    //============================= //
    #region [Vector3 Extensions]
    // SetX, SetY, SetZ
    public static Vector3 SetX (this Vector3 vector, float x)
    {
        return new Vector3(x, vector.y, vector.z);
    }
    public static Vector3 SetY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }
    public static Vector3 SetZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }

    // AddX, AddY, AddZ
    public static Vector3 AddX(this Vector3 vector, float x)
    {
        return new Vector3(vector.x + x, vector.y, vector.z);
    }

    public static Vector3 AddY(this Vector3 vector, float y)
    {
        return new Vector3(vector.x, vector.y + y, vector.z);
    }

    public static Vector3 AddZ(this Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, vector.z + z);
    }
    #endregion


    //============================= //
    //   [GameObject Extensions]
    //============================= //
    #region [GameObject Extensions]
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        var component = gameObject.GetComponent<T>();
        if (component == null) gameObject.AddComponent<T>();
        return component;
    }

    public static bool HasComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    {
        return gameObject.GetComponent<T>() != null;
    }
    #endregion


    //============================= //
    //     [Random Extensions]
    //============================= //
    #region [Random Extensions]
    public static T GetRandomItem<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            var value = list[j];
            list[j] = list[i];
            list[i] = value;
        }
    }
    #endregion


    //============================= //
    //   [Transform Extensions]
    //============================= //
    #region [Transform Extensions]
    public static void DestroyChildren(this Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static void ResetTransformation(this Transform transform)
    {
        transform.position = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
    #endregion


    //============================= //
    //   [RigidBody Extensions]
    //============================= //
    #region [RigidBody Extensions]
    public static void ChangeDirection(this Rigidbody rigidbody, Vector3 direction)
    {
        rigidbody.linearVelocity = direction * rigidbody.linearVelocity.magnitude;
    }

    public static void NormalizeVelocity(this Rigidbody rigidbody, float magnitude = 1)
    {
        rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * magnitude;
    }
    #endregion
}
