using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourseDropdown : Dropdown {

	public static string[] Courses = {"P-Course"};

	void Start () {
		this.ClearOptions();
		this.AddOptions(new List<string>(Courses));
	}

	public string GetSelectedCourse() {
		return Courses[this.value];
	}

	public void TimeTrial() {
		Manager.TimeTrial(GetSelectedCourse());
	}
	public void Race() {
		Manager.Race(GetSelectedCourse());
	}
}
