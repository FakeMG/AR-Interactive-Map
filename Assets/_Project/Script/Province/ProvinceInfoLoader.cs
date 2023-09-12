using System.Text;
using FakeMG.Main.Database;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FakeMG.Province {
    public class ProvinceInfoLoader : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private RawImage image;

        private void Awake() {
            displayName.text = transform.parent.name;
        }

        private void Start() {
            description.text = "";
        }

        public void RetrieveProvinceData(string provinceName) {
            displayName.text = provinceName;
            var imagePath = "provinces icon" + "/" + provinceName + ".jpg"; 
            DatabaseBehavior.Instance.DownloadImage(imagePath, image);
            
            DatabaseBehavior.Instance.LoadData("provinces/" + provinceName, snapshot => {
                description.text = "";

                foreach (DataSnapshot provincesSnapshot in snapshot.Children) {
                    if (description) {
                        StringBuilder sb = new StringBuilder(description.text);
                        if (provincesSnapshot.Key != "Description")
                            sb.AppendLine("- " + provincesSnapshot.Key + ": " + provincesSnapshot.Value);
                        description.text = sb.ToString();
                    } else {
                        Debug.Log("ProvinceInfoText is null");
                        return;
                    }
                }
            });
        }
    }
}