using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	void Awake() {
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
