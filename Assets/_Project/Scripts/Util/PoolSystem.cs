using System.Collections.Generic;
using UnityEngine;

public interface IPoolSystem {
	public GameObject PoolObject(Vector3 position, Quaternion rotation);
	public void ReturnToQueue(GameObject obj);
}

/// <summary>
/// General class for managing different PoolSystems 
/// (I should make a "PoolSystemManager" for accessing the different PoolSystems created here)
/// </summary>
/// <typeparam name="T">Class of the specific PoolSystem</typeparam>
public abstract class PoolSystem<T> : StaticInstance<T>, 
	IPoolSystem where T : MonoBehaviour {
	[SerializeField] protected int numberOfInstances;

	protected GameObject objectPrefab;
	public readonly List<GameObject> activeObjects = new();
	private readonly Queue<GameObject> _poolQueue = new();

	protected virtual void Start() {
		SetUpInstances();
	}

	private void SetUpInstances() {
		for(int i = 0; i < numberOfInstances; i++) {
			GameObject instance = Instantiate(objectPrefab);
			_poolQueue.Enqueue(instance);
			instance.SetActive(false);
		}
	}

	public GameObject PoolObject(Vector3 position, Quaternion rotation) {
		if(_poolQueue.Count <= 0){ 
			GameObject newObject = Instantiate(objectPrefab, position, rotation);
			activeObjects.Add(newObject);
			return newObject;
		}

		GameObject obj = _poolQueue.Dequeue();
		activeObjects.Add(obj);

		obj.transform.SetPositionAndRotation(position, rotation);
		obj.SetActive(true);

		return obj;
	}

	public void ReturnToQueue(GameObject obj) {
		obj.SetActive(false);
		activeObjects.Remove(obj);
		_poolQueue.Enqueue(obj);
	}
}
