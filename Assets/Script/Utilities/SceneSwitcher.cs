using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FakeMG.Utilities {
    public class SceneSwitcher : MonoBehaviour {
        public void SwitchToScene(string sceneName) {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone) {
                yield return null;
            }

            // Scene has finished loading, you can perform additional actions here
        }
    }
}