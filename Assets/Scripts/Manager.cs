using System.Collections;
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

	public static List<string> listOfRecordings;

	public static int numberOfAi = 0;

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
		Time.timeScale = 1;

		listOfRecordings = new List<string>(PlayerPrefs.GetString("recordings", "").Split(','));
		listOfRecordings.Remove("");
		Debug.Log(PlayerPrefs.GetString("recordings", ""));
    }

	public static void Reset() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public static void StartSailing() {
		SceneManager.LoadScene("SampleScene");
	}
	public static void MainMenu() {
		Time.timeScale = 1;
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

	public static void ShowReplay(int replayIndex) {
		string replayName = listOfRecordings[replayIndex];
		string courseName = PlayerPrefs.GetString(replayName, "").Split(',')[0];
		SceneManager.LoadScene(courseName);
		GameObject replayBoat = InstantiatePlayerBoat(Vector3.zero, Quaternion.identity);
		Playback playback = replayBoat.AddComponent(typeof(Playback)) as Playback;
		playback.LoadReplay(replayName);
	}

	public static void TimeTrial(string course) {
		Time.timeScale = 1;
		numberOfAi = 0;
		SceneManager.LoadScene(course);
	}

	public static void Race(string course) {
		Time.timeScale = 1;
		numberOfAi = 3;
		SceneManager.LoadScene(course);
	}

	public static void FreeSail() {
		Time.timeScale = 1;
		numberOfAi = 0;
		SceneManager.LoadScene("Free Sail");
	}
}
