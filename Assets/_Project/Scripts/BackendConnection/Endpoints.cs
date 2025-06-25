
public class Endpoints {
	private const string GENERAL_ROUTE = "http://127.0.0.1:5000";

	public const string SIGNIN = GENERAL_ROUTE + "/api/auth/register";
	public const string LOGIN = GENERAL_ROUTE + "/api/auth/login";

	public const string TTS = "https://generativelanguage.googleapis.com/v1beta/models/"
		+"gemini-2.5-flash-preview-tts:generateContent?"
		+ "key=AIzaSyAUOl7tx3q4atsrHUuuVDusqxwDIE5KZpM";

	public const string STT = GENERAL_ROUTE + "/api/speech/transcribe";
}