using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩을 보관할 변수
    public GameObject[] prefabs;

    // 풀 담당 하는 리스트들
    List<GameObject> pool;

     // 유저를 관리할 딕셔너리
    Dictionary<string, GameObject> userDictionary = new Dictionary<string, GameObject>();

    void Awake() {
        pool = new List<GameObject>();
    }

    public GameObject Get(LocationUpdate.UserLocation data) {
        // 유저가 이미 존재하면 해당 유저 반환
        if (userDictionary.TryGetValue(data.id, out GameObject existingUser)) {
            return existingUser;
        }
        
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는(비활성화) 게임 오브젝트 접근
        foreach (GameObject item in pool) {
            if (!item.activeSelf) {
                // 발견하면 select에 할당
                select = item;
                select.GetComponent<PlayerPrefab>().Init(data.playerId, data.id);
                select.SetActive(true);
                userDictionary[data.id] = select;
                break;
            }
        }
        // ... 못 찾으면
        if (select == null) {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[0], transform);
            pool.Add(select);
            select.GetComponent<PlayerPrefab>().Init(data.playerId, data.id);
            userDictionary[data.id] = select;
        }

        return select;
    }

    public void Remove(string userId) {
        if (userDictionary.TryGetValue(userId, out GameObject userObject)) {
            Debug.Log($"Removing user: {userId}");
            userObject.SetActive(false);
            userDictionary.Remove(userId);
        } else {
            Debug.Log($"User {userId} not found in dictionary");
        }
    }
}
