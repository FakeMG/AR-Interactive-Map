using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace FakeMG.Utilities {
    public class TextToSpeech : MonoBehaviour {
        private const string URL = "https://api.fpt.ai/hmi/tts/v5";
        private const string API_KEY = APIKeys.FPT_TTS_API_KEY;
        private const string VOICE = "banmai";
        private Coroutine _playAudioCoroutine;
        private Coroutine _getAudioURLCoroutine;

        public void PlayAudio(TextMeshProUGUI textMeshProUGUI) {
            StopAudio();
            _getAudioURLCoroutine = StartCoroutine(GetAudioURL(textMeshProUGUI.text));
        }
    
        private IEnumerator GetAudioURL(string text) {
            string payload = text;

            Dictionary<string, string> headers = new Dictionary<string, string> {
                { "api-key", API_KEY },
                { "speed", "" },
                { "voice", VOICE }
            };

            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

            using UnityWebRequest request = new UnityWebRequest(URL, "POST");
            request.uploadHandler = new UploadHandlerRaw(payloadBytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            foreach (KeyValuePair<string, string> header in headers) {
                request.SetRequestHeader(header.Key, header.Value);
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success) {
                Debug.LogError("TTS request failed. Error: " + request.error);
                yield break;
            }

            string responseText = request.downloadHandler.text;
            TextToSpeechInfo response = JsonUtility.FromJson<TextToSpeechInfo>(responseText);
        
            _playAudioCoroutine = StartCoroutine(PlayAudioFileCoroutine(response.async));
        
            yield return null;
        }

        private IEnumerator PlayAudioFileCoroutine(string url) {
            using UnityWebRequest audioRequest = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);
            yield return audioRequest.SendWebRequest();

            if (audioRequest.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Audio request failed. Error: " + audioRequest.error);
                yield break;
            }

            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(audioRequest);
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.Play();
        
            yield return null;
        }

        public void StopAudio() {
            if (_getAudioURLCoroutine != null)
                StopCoroutine(_getAudioURLCoroutine);
            if (_playAudioCoroutine != null)
                StopCoroutine(_playAudioCoroutine);
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null) {
                audioSource.Stop();
                Destroy(audioSource);
            }
        }
    
    
    }

    internal class TextToSpeechInfo {
        public string async;
        public int error;
        public string message;
    }
}