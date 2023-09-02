using FakeMG.Database;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FakeMG.People {
    public class PeopleListDataLoader : MonoBehaviour {
        [SerializeField] private GameObject contentHolder;
        [SerializeField] private GameObject template;

        private void Awake() {
            RetrievePeopleName();
        }

        private void RetrievePeopleName() {
            DatabaseBehavior.Instance.LoadData("people", snapshot => {
                foreach (DataSnapshot dataSnapshot in snapshot.Children) {
                    GameObject go = Instantiate(template, contentHolder.transform);
                    go.transform.SetParent(contentHolder.transform, false);
                    go.SetActive(true);

                    DatabaseBehavior.Instance.DownloadImage(dataSnapshot.Key,
                        go.transform.GetChild(0).GetComponent<RawImage>());
                    go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = dataSnapshot.Key;
                }
            });
        }
    }
}