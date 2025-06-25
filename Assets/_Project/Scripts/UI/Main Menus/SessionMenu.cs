using UnityEngine;
using UnityEngine.UI;

public class SessionMenu : BaseCanvas {
	[SerializeField] private Button m_startMic;
	[SerializeField] private Button m_stopMic;
	[SerializeField] private Button m_endSession;

	protected override void Awake() {
		_type = Menus.SessionMenu;
		base.Awake();
		m_startMic.onClick.AddListener(() => {
			VoiceController.Instance.StartMic();
		});

		m_stopMic.onClick.AddListener(async () => {
			VoiceController.Instance.StopMic();
			string message = await VoiceController.Instance.Hear();
			AnswerData answer =  new AnswerData {
				user_response = message,
				session_id = AppManager.Instance.GetSessionData().id,
			};
			await BackendCalls.SessionAnswer(answer);

			QuestionResponse response = await BackendCalls.SessionQuestion(AppManager.Instance.GetSessionData().id);
			VoiceController.Instance.Speak(response.question);
		});

		m_endSession.onClick.AddListener(async () => {
			await BackendCalls.SessionEnd(AppManager.Instance.GetSessionData().id);
			AppManager.Instance.ChangeCanvas(Menus.ResultsMenu);
			
		});
	}
}