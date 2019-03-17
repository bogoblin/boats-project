﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	public GameObject PlayerBoatPrefab;
	public GameObject AiBoatPrefab;

	private static GameObject _playerBoatPrefab;
	private static GameObject _aiBoatPrefab;

	private static Manager _instance;
	public static Manager Instance {get {return _instance;}}
	private static GameObject currentPlayerBoat;

	private void Awake() {
		_playerBoatPrefab = PlayerBoatPrefab;
		_aiBoatPrefab = AiBoatPrefab;
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        } else {
            _instance = this;
        }
		DontDestroyOnLoad(transform.gameObject);
		
    }

	public static void Reset() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public static void StartSailing() {
		SceneManager.LoadScene("SampleScene");
	}
	public static void MainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	// We make sure here that there is only ever one player boat
	public static GameObject InstantiatePlayerBoat(Vector3 position, Quaternion rotation) {
		Destroy(currentPlayerBoat);
		currentPlayerBoat = Instantiate(_playerBoatPrefab, position, rotation);
		return currentPlayerBoat;
	}
	public static GameObject GetPlayerBoat() {
		return currentPlayerBoat;
	}

	public static GameObject InstantiateAiBoat(Vector3 position, Quaternion rotation) {
		return Instantiate(_aiBoatPrefab, position, rotation);
	}
}
