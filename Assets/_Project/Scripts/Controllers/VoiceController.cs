using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class VoiceController : Singleton<VoiceController> {
	[SerializeField] AudioSource audioSource;

	private AudioClip clip;

	public async void Speak(string text) {
		if(string.IsNullOrEmpty(text)) {
			Debug.LogWarning("Text is empty, nothing to speak.");
			return;
		}
		byte[] audioBytes = Convert.FromBase64String(await BackendCalls.GenerateTTS(text));
		audioSource.clip = ToAudioClipFromL16(audioBytes);
		audioSource.Play();
	}

	public void StartMic(){
		clip = Microphone.Start(null, false, 1800, 24000);
	}

	public void StopMic() {
		Microphone.End(null);
	}

	public async Task<string> Hear() {		
		// Convert to WAV format instead of raw L16
		byte[] wavBytes = ToWavFromAudioClip(clip);
		
		string response = await BackendCalls.GenerateSTT(wavBytes,
			(error) => Debug.LogError("STT Error: " + error),
			(response) => Debug.Log("STT Response: " + response)
		);

		return response;
	}

	private static byte[] ToWavFromAudioClip(AudioClip clip) {
		// Get audio data
		float[] samples = new float[clip.samples * clip.channels];
		clip.GetData(samples, 0);
		
		// Convert to 16-bit PCM
		short[] intData = new short[samples.Length];
		const float rescaleFactor = 32767f;
		for (int i = 0; i < samples.Length; i++) {
			intData[i] = (short)(Mathf.Clamp(samples[i], -1f, 1f) * rescaleFactor);
		}
		
		// Create WAV file
		return CreateWavFile(intData, clip.frequency, clip.channels);
	}

	private static byte[] CreateWavFile(short[] audioData, int sampleRate, int channels) {
		int audioDataSize = audioData.Length * 2; // 2 bytes per sample (16-bit)
		int fileSize = 44 + audioDataSize; // WAV header is 44 bytes
		
		byte[] wavFile = new byte[fileSize];
		
		// WAV Header
		int index = 0;
		
		// RIFF chunk descriptor
		WriteString(wavFile, ref index, "RIFF");
		WriteInt32(wavFile, ref index, fileSize - 8);
		WriteString(wavFile, ref index, "WAVE");
		
		// fmt sub-chunk
		WriteString(wavFile, ref index, "fmt ");
		WriteInt32(wavFile, ref index, 16); // Sub-chunk size (16 for PCM)
		WriteInt16(wavFile, ref index, 1);  // Audio format (1 for PCM)
		WriteInt16(wavFile, ref index, (short)channels);
		WriteInt32(wavFile, ref index, sampleRate);
		WriteInt32(wavFile, ref index, sampleRate * channels * 2); // Byte rate
		WriteInt16(wavFile, ref index, (short)(channels * 2)); // Block align
		WriteInt16(wavFile, ref index, 16); // Bits per sample
		
		// data sub-chunk
		WriteString(wavFile, ref index, "data");
		WriteInt32(wavFile, ref index, audioDataSize);
		
		// Audio data
		for (int i = 0; i < audioData.Length; i++) {
			WriteInt16(wavFile, ref index, audioData[i]);
		}
		
		return wavFile;
	}

	// Helper methods for writing to byte array
	private static void WriteString(byte[] buffer, ref int index, string text) {
		byte[] bytes = System.Text.Encoding.ASCII.GetBytes(text);
		Array.Copy(bytes, 0, buffer, index, bytes.Length);
		index += bytes.Length;
	}

	private static void WriteInt32(byte[] buffer, ref int index, int value) {
		byte[] bytes = BitConverter.GetBytes(value);
		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(bytes);
		}
		Array.Copy(bytes, 0, buffer, index, 4);
		index += 4;
	}

	private static void WriteInt16(byte[] buffer, ref int index, short value) {
		byte[] bytes = BitConverter.GetBytes(value);
		if (!BitConverter.IsLittleEndian) {
			Array.Reverse(bytes);
		}
		Array.Copy(bytes, 0, buffer, index, 2);
		index += 2;
	}

	// Alternative: Simple WAV converter using Unity's built-in methods
	private static byte[] ToWavFromAudioClipSimple(AudioClip clip) {
		// Get raw audio data
		float[] samples = new float[clip.samples * clip.channels];
		clip.GetData(samples, 0);
		
		// Convert to bytes (this creates a simple WAV-like format)
		byte[] audioBytes = new byte[samples.Length * 2];
		const float rescaleFactor = 32767f;
		
		for (int i = 0; i < samples.Length; i++) {
			short sample = (short)(Mathf.Clamp(samples[i], -1f, 1f) * rescaleFactor);
			byte[] sampleBytes = BitConverter.GetBytes(sample);
			audioBytes[i * 2] = sampleBytes[0];
			audioBytes[i * 2 + 1] = sampleBytes[1];
		}
		
		return CreateWavFile(ConvertBytesToShorts(audioBytes), clip.frequency, clip.channels);
	}

	private static short[] ConvertBytesToShorts(byte[] audioBytes) {
		short[] shorts = new short[audioBytes.Length / 2];
		for (int i = 0; i < shorts.Length; i++) {
			shorts[i] = BitConverter.ToInt16(audioBytes, i * 2);
		}
		return shorts;
	}

	// Optional: Save WAV file to disk for testing
	private void SaveWavFile(AudioClip clip, string filename) {
		byte[] wavData = ToWavFromAudioClip(clip);
		string path = Application.persistentDataPath + "/" + filename + ".wav";
		System.IO.File.WriteAllBytes(path, wavData);
		Debug.Log("WAV file saved to: " + path);
	}

	private AudioClip ToAudioClipFromL16(
		byte[] rawPcmData, 
		int sampleRate = 24000, 
		int channels = 1, 
		string clipName = "PCMClip") {

		int totalSamples = rawPcmData.Length / 2; // 16-bit = 2 bytes
		float[] audioData = new float[totalSamples];

		for (int i = 0; i < totalSamples; i++) {
			short sample = BitConverter.ToInt16(rawPcmData, i * 2);
			audioData[i] = sample / 32768f;
		}

		AudioClip audioClip = AudioClip.Create(clipName, totalSamples / channels, channels, sampleRate, false);
		audioClip.SetData(audioData, 0);
		return audioClip;
	}

}