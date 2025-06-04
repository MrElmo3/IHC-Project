using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Subject abstract class of the Observer pattern
/// </summary>
public abstract class Subject : MonoBehaviour {
	private readonly List<IObserver> _observers = new();

	public void AddObserver(IObserver observer) {
		_observers.Add(observer);
	}

	public void RemoveObserver(IObserver observer) {
		_observers.Remove(observer);
	}

	protected void NotifyObservers() {
		_observers.ForEach((_observer) => {
			_observer.OnNotify();
		});
	}
}

/// <summary>
/// Interface of the observer pattern
/// </summary>
public interface IObserver {

	/// <summary>
	/// Event triggered when the Subject calls the observers
	/// (The parameters may be changed or modified)
	/// </summary>
	public void OnNotify();
}