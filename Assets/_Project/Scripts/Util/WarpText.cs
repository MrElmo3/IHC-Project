using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// This script is for bending the text of TextMeshPro
/// </summary>
public class WarpText : MonoBehaviour {

	private TMP_Text _textComponent;

	[SerializeField] AnimationCurve VertexCurve = new AnimationCurve(
		new Keyframe(0, 0),
		new Keyframe(0.5f, 1.0f),
		new Keyframe(1, 0f)
	);
	[SerializeField] float CurveScale = 1.0f;

	void Awake() {
		_textComponent = GetComponent<TMP_Text>();
	}

	void Start() {
		StartCoroutine(Warp());
	}
	
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve) {
		AnimationCurve newCurve = new AnimationCurve();
		newCurve.keys = curve.keys;
		return newCurve;
	}

	IEnumerator Warp() {

		VertexCurve.preWrapMode = WrapMode.Clamp;
		VertexCurve.postWrapMode = WrapMode.Clamp;

		//Mesh mesh = m_TextComponent.textInfo.meshInfo[0].mesh;

		Vector3[] vertices;
		Matrix4x4 matrix;

		_textComponent.havePropertiesChanged = true; // Need to force the TextMeshPro Object to be updated.
		CurveScale *= 10;
		float old_CurveScale = CurveScale;
		AnimationCurve old_curve = CopyAnimationCurve(VertexCurve);

		while (true) {
			if (!_textComponent.havePropertiesChanged &&
				old_CurveScale == CurveScale &&
				old_curve.keys[1].value == VertexCurve.keys[1].value) {

				yield return null;
				continue;
			}

			old_CurveScale = CurveScale;
			old_curve = CopyAnimationCurve(VertexCurve);

			// Generate the mesh and populate the textInfo with data we can use and manipulate.
			_textComponent.ForceMeshUpdate(); 

			TMP_TextInfo textInfo = _textComponent.textInfo;
			int characterCount = textInfo.characterCount;

			if (characterCount == 0) continue;

			//vertices = textInfo.meshInfo[0].vertices;
			//int lastVertexIndex = textInfo.characterInfo[characterCount - 1].vertexIndex;

			float boundsMinX = _textComponent.bounds.min.x; 
			float boundsMaxX = _textComponent.bounds.max.x;

			for (int i = 0; i < characterCount; i++) {

				if (!textInfo.characterInfo[i].isVisible) continue;

				int vertexIndex = textInfo.characterInfo[i].vertexIndex;

				// Get the index of the mesh used by this character.
				int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

				vertices = textInfo.meshInfo[materialIndex].vertices;

				// Compute the baseline mid point for each character
				Vector3 offsetToMidBaseline = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, textInfo.characterInfo[i].baseLine);
				//float offsetY = VertexCurve.Evaluate((float)i / characterCount + loopCount / 50f); // Random.Range(-0.25f, 0.25f);

				// Apply offset to adjust our pivot point.
				vertices[vertexIndex + 0] += -offsetToMidBaseline;
				vertices[vertexIndex + 1] += -offsetToMidBaseline;
				vertices[vertexIndex + 2] += -offsetToMidBaseline;
				vertices[vertexIndex + 3] += -offsetToMidBaseline;

				// Compute the angle of rotation for each character based on the animation curve
				float x0 = (offsetToMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX); // Character's position relative to the bounds of the mesh.
				float x1 = x0 + 0.0001f;
				float y0 = VertexCurve.Evaluate(x0) * CurveScale;
				float y1 = VertexCurve.Evaluate(x1) * CurveScale;

				Vector3 horizontal = new Vector3(1, 0, 0);
				//Vector3 normal = new Vector3(-(y1 - y0), (x1 * (boundsMaxX - boundsMinX) + boundsMinX) - offsetToMidBaseline.x, 0);
				Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetToMidBaseline.x, y0);

				float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
				Vector3 cross = Vector3.Cross(horizontal, tangent);
				float angle = cross.z > 0 ? dot : 360 - dot;

				matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

				vertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 0]);
				vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);

				vertices[vertexIndex + 0] += offsetToMidBaseline;
				vertices[vertexIndex + 1] += offsetToMidBaseline;
				vertices[vertexIndex + 2] += offsetToMidBaseline;
				vertices[vertexIndex + 3] += offsetToMidBaseline;
			}

			// Upload the mesh with the revised information
			_textComponent.UpdateVertexData();

			yield return new WaitForSeconds(0.025f);
		}
	}
}