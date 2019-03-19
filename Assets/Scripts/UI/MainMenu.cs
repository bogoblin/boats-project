using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public GameObject[] submenus;

	public void ShowSubmenu(int submenuIndex) {
		submenus[submenuIndex].SetActive(true);
		gameObject.SetActive(false);
	}
}
