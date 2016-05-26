using UnityEngine;
using System.Collections;


/// <summary>
/// Extendable Singleton class for MonoBehaviors that need to persist through levels.
/// </summary>
public class UnitySingletonPersistent<T> : MonoBehaviour
	where T : Component
{
	public static string objName;
	private static T instance;
	private static bool applicationIsQuitting = false;


	public static T Instance
	{
		get
		{
			if (applicationIsQuitting)
			{
				Debug.Log("Application is quitting cannot access Singleton '" + typeof(T) + "'.");
				return null;
			}

			if (instance == null)
			{
				T[] objs = FindObjectsOfType<T>();

				if (objs.Length > 0)
				{
					instance = objs[0];
				}

				if (objs.Length > 1)
				{
					Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
				}

				if (instance == null)
				{
					GameObject obj = new GameObject();
					obj.hideFlags = HideFlags.DontSave;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}


	public virtual void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		if (instance == null)
		{
			instance = this as T;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	

	public void OnApplicationQuit()
	{
		applicationIsQuitting = true;
	}
}
