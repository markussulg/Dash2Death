using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour {

    [SerializeField] private Button serverButton;

    private void Awake() {
        serverButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Menu");
        });
    }
}
