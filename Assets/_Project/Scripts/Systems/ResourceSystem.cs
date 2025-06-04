using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// One repository for all scriptable objects. Create your query methods here to keep your business logic clean.
/// I make this a MonoBehaviour as sometimes I add some debug/development references in the editor.
/// If you don't feel free to make this a standard class
/// </summary>
public class ResourceSystem : StaticInstance<ResourceSystem> {

	// public List<SO_Tower> Towers { get; private set; }
	// public List<SO_Enemy> Enemies { get; private set; }
	// private Dictionary<TowerType, SO_Tower> _TowersDict;
	// private Dictionary<EnemyType, SO_Enemy> _EnemiesDict;

	// protected override void Awake() {
	// 	base.Awake();
	// 	AssembleResources();
	// }

	// private void AssembleResources() {
	// 	Towers = Resources.LoadAll<SO_Tower>("Towers").ToList();
	// 	Enemies = Resources.LoadAll<SO_Enemy>("Enemies").ToList();

	// 	_TowersDict = Towers.ToDictionary(r => r.type, r => r);
	// 	_EnemiesDict = Enemies.ToDictionary(r => r.type, r => r);
	// }

	// public SO_Tower GetTower(TowerType type) {
	// 	if (_TowersDict.TryGetValue(type, out var tower)) {
	// 		return tower;
	// 	}
	// 	Debug.LogError($"Tower with type {type} not found in ResourceSystem.");
	// 	return null;
	// }
	// public SO_Enemy GetEnemy(EnemyType type) {
	// 	if (_EnemiesDict.TryGetValue(type, out var enemy)) {
	// 		return enemy;
	// 	}
	// 	Debug.LogError($"Enemy with type {type} not found in ResourceSystem.");
	// 	return null;
	// }
	// public SO_Enemy GetRandomEnemy() {
	// 	if (Enemies.Count == 0) {
	// 		Debug.LogError("No enemies available in ResourceSystem.");
	// 		return null;
	// 	}
	// 	return Enemies[Random.Range(0, Enemies.Count)];
	// }
}   