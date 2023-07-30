using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ExcelDataReader;
using Firebase;
using Firebase.Database;
using UnityEngine;

namespace FakeMG.Database {
    public class Database : MonoBehaviour {
        private FirebaseApp _app;
        private DatabaseReference _databaseReference;
        private bool _firebaseInitialized = false;

        // private void Awake() {
        //     FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        //         var dependencyStatus = task.Result;
        //         if (dependencyStatus == DependencyStatus.Available) {
        //             _app = FirebaseApp.DefaultInstance;
        //
        //             Debug.Log("Firebase initialized successfully!");
        //             _firebaseInitialized = true;
        //         } else {
        //             Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
        //         }
        //     });
        // }

        private void RemoveAccentMark(string input) {
            input = "Việt Nam là quê hương tôi";
            char[] charArray = input.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            string output = new string(charArray);
            Debug.Log(output);
        }

        private void WriteDataFromExcel() {
            FileStream stream = File.Open("C://Users//Lam//Desktop//file.xlsx", FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            if (excelReader == null) {
                Debug.Log("Invalid file");
                return;
            }

            Debug.Log("Valid file");

            excelReader.Read();
            List<string> columns = new List<string>();
            for (int i = 0; i < excelReader.FieldCount; i++) {
                string columnName = excelReader.GetString(i);
                if (!string.IsNullOrEmpty(columnName)) {
                    columns.Add(columnName);
                }
            }

            while (excelReader.Read()) {
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                string provinceName = "";

                for (int i = 0; i < columns.Count; i++) {
                    if (i == 1) {
                        provinceName = excelReader.GetString(i);
                        continue;
                    }

                    object cellValue = excelReader.GetValue(i);
                    if (cellValue != null) {
                        rowData.Add(columns[i], cellValue);
                    }
                }

                _databaseReference.Child("provinces").Child(provinceName).SetValueAsync(rowData);
            }

            excelReader.Close();
            stream.Close();
        }
    }
}