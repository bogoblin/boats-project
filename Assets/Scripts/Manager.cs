using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	public GameObject PlayerBoatPrefab;
	public GameObject AiBoatPrefab;

	private static Manager _instance;
	public static Manager Instance {get {return _instance;}}

	private void Awake() {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
		DontDestroyOnLoad(transform.gameObject);
    }

	public void Reset() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void StartSailing() {
		SceneManager.LoadScene("SampleScene");
	}
	public void MainMenu() {
		SceneManager.LoadScene("MainMenu");
	}
}
