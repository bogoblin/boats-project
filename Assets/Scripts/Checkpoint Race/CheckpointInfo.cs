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

    public CheckpointInfo(BoatBehavior requiredBoat, CheckpointRace requiredRace) {
        boat = requiredBoat;
        race = requiredRace;
    }
    public bool Hit(int index) {
        if (index == checkpointIndex) {
            if (!started) {
                started = true;
                startTime = Time.time;
            }
            // this means that it is the next checkpoint
            DisplayRaceText(((Time.time + totalPenalty) - startTime).ToString());
            if (checkpointIndex == 0 && currentLap == race.numberOfLaps) {
                race.Finish(boat);
                DisplayRaceText("Finished!");
                return true;
            }
            checkpointIndex++;
            if (checkpointIndex == race.checkpoints.Length) {
                currentLap++;
                checkpointIndex = 0;
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
            DisplayRaceText("+5 second penalty!");
        }
    }

    void DisplayRaceText(string text) {
        Debug.Log(text);
    }
}