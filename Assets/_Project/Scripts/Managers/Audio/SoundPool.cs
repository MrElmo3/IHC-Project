using UnityEngine;

class SoundPool : PoolSystem<SoundPool> {
	
	[SerializeField] GameObject soundEmmiterPrefab;

	protected override void Start() {
		objectPrefab = soundEmmiterPrefab;
		base.Start();
	}

	public EmmiterController PoolSoundEmmiter() {
		var emmiterObject = PoolObject(Vector3.zero, Quaternion.identity);
		EmmiterController emmiter = emmiterObject.GetComponent<EmmiterController>();
		return emmiter;
	}

	public void ReturnEmmiter(EmmiterController emmiter) {
		ReturnToQueue(emmiter.gameObject);
	}

}