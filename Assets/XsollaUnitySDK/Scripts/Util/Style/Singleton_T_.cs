using UnityEngine;
using System.Collections;

namespace Xsolla {
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		
		private static object _lock = new object();
		
		public static T Instance
		{
			get
			{
				if (applicationIsQuitting) {
					Logger.Log("[Singleton] Instance '"+ typeof(T) +
					                 "' already destroyed on application quit." +
					                 " Won't create again - returning null.");//Warning
					return null;
				}
				
				lock(_lock)
				{
					if (_instance == null)
					{
						_instance = (T) FindObjectOfType(typeof(T));
						
						if ( FindObjectsOfType(typeof(T)).Length > 1 )
						{
							Logger.Log("[Singleton] Something went really wrong " +
							               " - there should never be more than 1 singleton!" +
							               " Reopening the scene might fix it.");//Error
							return _instance;
						}
						
						if (_instance == null)
						{
							GameObject singleton = new GameObject();
							_instance = singleton.AddComponent<T>();
							singleton.name = "(singleton) "+ typeof(T).ToString();
							
							DontDestroyOnLoad(singleton);
							
							Logger.Log("[Singleton] An instance of " + typeof(T) + 
							          " is needed in the scene, so '" + singleton +
							          "' was created with DontDestroyOnLoad.");
						} else {
							Logger.Log("[Singleton] Using instance already created: " +
							          _instance.gameObject.name);
						}
					}
					
					return _instance;
				}
			}
		}
		
		private static bool applicationIsQuitting = false;

		public void OnDestroy () {
			applicationIsQuitting = true;
		}
	}
}
