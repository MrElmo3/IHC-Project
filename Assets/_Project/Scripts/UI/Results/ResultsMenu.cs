using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsMenu : BaseCanvas {
	[Header("Scores")]
	[SerializeField] TMP_Text m_overallScore;
	[SerializeField] TMP_Text m_comunicationScore;
	[SerializeField] TMP_Text m_confidenceScore;
	[SerializeField] TMP_Text m_techScore;

	[Header("Fortalezas")]
	[SerializeField] TMP_Text m_fortaleza1;
	[SerializeField] TMP_Text m_fortaleza2;
	[SerializeField] TMP_Text m_fortaleza3;

	[Header("Mejora")]
	[SerializeField] TMP_Text m_mejora1;
	[SerializeField] TMP_Text m_mejora2;
	[SerializeField] TMP_Text m_mejora3;

	[Header("Especificas")]
	[SerializeField] TMP_Text m_especificas1;
	[SerializeField] TMP_Text m_especificas2;
	[SerializeField] TMP_Text m_especificas3;

	[Header("Buttons")]
	[SerializeField] Button m_returnButton;

	protected override void Awake() {
		base.Awake();

		m_returnButton.onClick.AddListener(() => {
			AppManager.Instance.ChangeCanvas(Menus.MainMenu);
		});
	}
}