using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class BackendCalls {

	public static async Task<UserResponse> SignIn(
		UserData user,
		Action<string> OnError = null,
		Action<UserResponse> OnSuccess = null
	) {
		string jsonBody = JsonUtility.ToJson(user);
		byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

		string url = Endpoints.SIGNIN;

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string json = request.downloadHandler.text;
			UserResponse response = JsonUtility.FromJson<UserResponse>(json);
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

	public static async Task<UserResponse> LogIn(
		UserData user,
		Action<string> OnError = null,
		Action<UserResponse> OnSuccess = null
	) {
		string jsonBody = JsonUtility.ToJson(user);
		byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

		string url = Endpoints.LOGIN;

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string json = request.downloadHandler.text;
			UserResponse response = JsonUtility.FromJson<UserResponse>(json);
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
	
	public static async Task<SessionResponse> SessionCreate(
		SessionData session,
		Action<string> OnError = null,
		Action<SessionResponse> OnSuccess = null
	) {

		string jsonBody = JsonUtility.ToJson(session);
		byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

		string url = Endpoints.SESSION_CREATE;

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string json = request.downloadHandler.text;
			SessionResponse response = JsonUtility.FromJson<SessionResponse>(json);
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

	public static async Task<MessageResponse> SessionConversation(
		int sessionId,
		SessionMessage message,
		Action<string> OnError = null,
		Action<MessageResponse> OnSuccess = null
	) {

		string jsonBody = JsonUtility.ToJson(message);
		byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

		string url = Endpoints.SESSION_CONVERSATION.Replace("id", sessionId.ToString());

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string json = request.downloadHandler.text;
			MessageResponse response = JsonUtility.FromJson<MessageResponse>(json);
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

	public static async Task<MessageResponse> SessionAnswer(
		AnswerData answer,
		Action<string> OnError = null,
		Action<MessageResponse> OnSuccess = null
	){
		string jsonBody = JsonUtility.ToJson(answer);
		byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonBody);

		string url = Endpoints.SESSION_ANSWER;

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
		await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string json = request.downloadHandler.text;
			MessageResponse response = JsonUtility.FromJson<MessageResponse>(json);
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

	public static async Task<QuestionResponse> SessionQuestion(
		int sessionId,
		Action<string> OnError = null,
		Action<QuestionResponse> OnSuccess = null
	) {

		string url = Endpoints.SESSION_QUESTION;
		byte[] jsonToSend = Encoding.UTF8.GetBytes($@"
		{{
		""session_id"": ""{sessionId}""
		}}"
		);

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string json = request.downloadHandler.text;
			QuestionResponse response = JsonUtility.FromJson<QuestionResponse>(json);
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

	public static async Task<SessionResponse> SessionEnd(
		int sessionId,
		Action<string> OnError = null,
		Action<SessionResponse> OnSuccess = null
	) {

		string url = Endpoints.SESSION_END.Replace("id", sessionId.ToString());

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string json = request.downloadHandler.text;
			SessionResponse response = JsonUtility.FromJson<SessionResponse>(json);
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

	public static async Task<string> GenerateTTS(
		string text,
		Action<string> OnError = null,
		Action<string> OnSuccess = null
	) {

		string voice = "";
		switch(AppManager.Instance.GetSimulationCharacter()) {
			case SimulationCharacter.Joe:
				voice = "Sadaltager";
				break;
			case SimulationCharacter.Louise:
				voice = "Sulafat";
				break;
		}

		byte[] jsonToSend = Encoding.UTF8.GetBytes($@"
		{{
			""contents"": [{{
				""parts"":[{{
					""text"": ""{text}""
			}}]
		}}],
		""generationConfig"": {{
			""responseModalities"": [""AUDIO""],
			""speechConfig"": {{
				""voiceConfig"": {{
					""prebuiltVoiceConfig"": {{
						""voiceName"": ""{voice}""
					}}
				}}
			}}
		}},
		""model"": ""gemini-2.5-flash-preview-tts""
		}}");

		string url = Endpoints.TTS;

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.uploadHandler = new UploadHandlerRaw(jsonToSend);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string pattern = @"""data""\s*:\s*""(.*?)""";
			Match match = Regex.Match(request.downloadHandler.text, pattern);

			string response = null;
			if(match.Success) {
				response = match.Groups[1].Value;
			}

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

	public static async Task<string> GenerateSTT(
		byte[] audioData,
		Action<string> OnError = null,
		Action<string> OnSuccess = null
	) {
		
		string url = Endpoints.STT;

		byte[] jsonBody = Encoding.UTF8.GetBytes($@"
		{{
		""base64_audio"": ""{Convert.ToBase64String(audioData)}""
		}}
		");

		using UnityWebRequest request = new(url, UnityWebRequest.kHttpVerbPOST);

		request.uploadHandler = new UploadHandlerRaw(jsonBody);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		var operation = request.SendWebRequest();
		while (!operation.isDone)
			await Task.Yield();

		if (request.result != UnityWebRequest.Result.Success) {
			string errorMsg = request.downloadHandler.text;
			Debug.LogWarning("HTTP Error: " + errorMsg);
			OnError?.Invoke(errorMsg);
			return null;
		}
		try {
			string response = request.downloadHandler.text;
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

[Serializable]
public class UserData {
	public int id;
	public string email;
	public string first_name;
	public string last_name;
	public string preferred_difficulty;
	public int anxiety_level;
	public string password;
}

[Serializable]
public class UserResponse {
	public string message;
	public UserData user;
}

[Serializable]
public class SessionData {
	public int id;
	public int user_id;
	public int scenario_id;
	public string difficulty_level;
	public string interviewer_avatar;
	public string environment;
	public string custom_description;
	public string started_at;
	public string ended_at;
	public string status;
	public SessionMessage[] conversation;
	public SessionMetrics performance_metrics;
}

[Serializable]
public class SessionMessage {
	public string speaker;
	public string message;
	public string timestamp;
	public float response_time;
}

[Serializable]
public class SessionMetrics {
	public int total_responses;
	public int total_words;
	public float average_response_time;
	public int session_duration;
}

[Serializable]
public class SessionResponse {
	public string message;
	public SessionData session;
}

[Serializable]
public class MessageResponse {
	public string message;
	public int conversation_length;
}

[SerializeField]
public class QuestionResponse {
	public string question;
}

[SerializeField]
public class AnswerData {
	public int session_id;
	public string user_response;
}