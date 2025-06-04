using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[Serializable]
public class ResponseData {
	
}

public class BackendCalls : MonoBehaviour {

	public async Task<ResponseData> PostRequest(
		string url, ResponseData bodyData, 
		UnityEvent<string> OnError = null,
		UnityEvent<ResponseData> OnSuccess = null) {

		string jsonBody = JsonUtility.ToJson(bodyData);
		byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

		using UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
		
		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.error;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}

		try {
			string json = request.downloadHandler.text;
			ResponseData response = JsonUtility.FromJson<ResponseData>(json);
			OnSuccess?.Invoke(response);
			return response;
		}

		catch (Exception e) {
			string parseError = "JSON Parse Error: " + e.Message;
			Debug.LogWarning(parseError);
			OnError?.Invoke(parseError);
			return null;
		}
	}
}