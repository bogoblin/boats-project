using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointInfo {
    public int checkpointIndex = 0; // the index of the next checkpoint of the race
    public int currentLap = 0;
    private BoatBehavior boat;
    private CheckpointRace race;

    public CheckpointInfo(BoatBehavior requiredBoat, CheckpointRace requiredRace) {
        boat = requiredBoat;
        race = requiredRace;
    }
    public bool Hit(int index) {
        if (index == checkpointIndex) {
            if (checkpointIndex == 0 && currentLap == race.numberOfLaps) {
                race.Finish(boat);
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
}