using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseCanvas<T> : StaticInstance<T> where T: MonoBehaviour {

	public readonly System.Enum _type;

	private CanvasGroup _canvasGroup;

	private bool _startShow = false;
	private bool _startHide = false;

	protected UnityAction OnStartShow;
	protected UnityAction OnStartHide;

	protected UnityAction OnCompleteShow;
	protected UnityAction OnCompleteHide;

	protected override void Awake(){
		base.Awake();
		_canvasGroup = GetComponent<CanvasGroup>();
		OnStartHide += StartHide;
		OnStartShow += StartShow;

		ForceHide();
	}

	public virtual void Show(){
		OnStartShow?.Invoke();
		_canvasGroup.DOFade(1.0f, 0.6f)
		.OnComplete(() => { 
			ForceShow();
		});
	}

	public virtual void Hide(){
		OnStartHide?.Invoke();
		_canvasGroup.DOFade(0.0f, 0.6f)
		.OnComplete( () => {
			ForceHide();
		});
	}

	protected virtual void ForceShow(){
		if(!_startShow)
			OnStartShow?.Invoke();
		_canvasGroup.alpha = 1.0f;
		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;
		_startShow = false;
		OnCompleteShow?.Invoke();

	}

	protected virtual void ForceHide(){
		if(!_startHide)
			OnStartHide?.Invoke();
		_canvasGroup.alpha = 0.0f;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
		_startHide = false;
		OnCompleteHide?.Invoke();
	}

	private void StartHide() { _startHide = true;}
	private void StartShow() { _startShow = true;}
	public System.Enum GetCanvasType() { return _type; }
}
