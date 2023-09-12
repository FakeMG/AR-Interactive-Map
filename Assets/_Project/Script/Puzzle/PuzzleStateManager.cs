using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FakeMG.Puzzle {
    public class PuzzleStateManager : MonoBehaviour {
        [SerializeField] private List<GameObject> playingObjects;
        [SerializeField] private List<GameObject> pausedObjects;
        [SerializeField] private List<GameObject> wonObjects;

        [SerializeField] private UnityEvent onPlayingSwitch;
        [SerializeField] private UnityEvent onPausedSwitch;
        [SerializeField] private UnityEvent onWonSwitch;

        public void ChangeState(string value) {
            if (!Enum.TryParse(value, out PuzzleState state)) {
                throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
            switch (state) {
                case PuzzleState.Playing:
                    ToggleAllObjects(false);
                    ToggleObjects(playingObjects, true);
                    onPlayingSwitch?.Invoke();
                    break;
                case PuzzleState.Paused:
                    ToggleAllObjects(false);
                    ToggleObjects(pausedObjects, true);
                    onPausedSwitch?.Invoke();
                    break;
                case PuzzleState.Won:
                    ToggleAllObjects(false);
                    ToggleObjects(wonObjects, true);
                    onWonSwitch?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
        
        private void ToggleObjects(List<GameObject> objects, bool toggle) {
            foreach (var obj in objects) {
                obj.SetActive(toggle);
            }
        }
        
        private void ToggleAllObjects(bool toggle) {
            ToggleObjects(playingObjects, toggle);
            ToggleObjects(pausedObjects, toggle);
            ToggleObjects(wonObjects, toggle);
        }
    }

    public enum PuzzleState {
        Playing,
        Paused,
        Won
    }
}