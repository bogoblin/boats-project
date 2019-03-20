using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInfo {
    public int checkpointIndex = 0; // the index of the next checkpoint of the race
    public int currentLap = 0;
    private BoatBehavior boat;
    private CheckpointRace race;

    private float lastPenalty = 0;
    private float totalPenalty = 0;
    private const float PenaltyPerHit = 5; // number of seconds to add when the boat hits a mark
    private float startTime = 0;
    private bool started = false;

    /// <summary> The time elapsed in the race for this boat. This includes penalties. </summary>
    private float timeElapsed {
        get {
            return ((Time.time + totalPenalty) - startTime);
        }
    }

    /// <summary> The UI elements that display the elapsed time for the player's boat </summary>
    RaceTime[] raceTimes;

    public CheckpointInfo(BoatBehavior requiredBoat, CheckpointRace requiredRace) {
        boat = requiredBoat;
        race = requiredRace;
    }
    public void Update() {
        raceTimes = boat.GetComponentsInChildren<RaceTime>();
        if (raceTimes.Length > 0) {
            foreach(RaceTime raceTime in raceTimes) {
                raceTime.SetTime(timeElapsed);
            }
        }
    }
    public bool Hit(int index) {
        if (index == checkpointIndex) {
            if (!started) {
                started = true;
                startTime = Time.time;
            }
            // this means that it is the next checkpoint
            DisplayRaceText(timeElapsed.ToString(), Color.green);
            if (race.lapped) {
                if (checkpointIndex == 0 && currentLap == race.numberOfLaps) {
                    race.Finish(boat);
                    return false;
                }
                checkpointIndex++;
                if (checkpointIndex == race.checkpoints.Length) {
                    currentLap++;
                    checkpointIndex = 0;
                }
            } else {
                checkpointIndex++;
                if (checkpointIndex == race.checkpoints.Length) {
                    race.Finish(boat);
                    return false;
                }
            }
            return true;
        }
        else {
            return false;
        }
    }

    public void MarkHit() {
        if (lastPenalty == 0 || Time.time - lastPenalty > PenaltyPerHit) {
            lastPenalty = Time.time;
            totalPenalty += PenaltyPerHit;
            DisplayRaceText("+"+PenaltyPerHit.ToString()+" second penalty!", Color.red);
        }
    }

    void DisplayRaceText(string text, Color color) {
        RaceText raceText = boat.GetComponentInChildren<RaceText>();
        if (raceText) boat.GetComponentInChildren<RaceText>().SetText(text, color);
    }
}