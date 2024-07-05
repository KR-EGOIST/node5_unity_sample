using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spawner : MonoBehaviour
{
    public static Spawner instance;
    private HashSet<string> currentUsers = new HashSet<string>();
    
    void Awake() {
        instance = this;
    }

    public void Spawn(LocationUpdate data) {
        if (!GameManager.instance.isLive) {
            return;
        }
        
        HashSet<string> newUsers = new HashSet<string>();

        foreach(LocationUpdate.UserLocation user in data.users) {
                newUsers.Add(user.id);

                GameObject player = GameManager.instance.pool.Get(user);
                PlayerPrefab playerScript = player.GetComponent<PlayerPrefab>();
                playerScript.UpdatePosition(user.x, user.y);
        }

        foreach (string userId in currentUsers) {
            if (!newUsers.Contains(userId)) {
                GameManager.instance.pool.Remove(userId);
            }
        }
        
        currentUsers = newUsers;
    }
}