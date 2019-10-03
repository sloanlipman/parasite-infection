using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSerializer : MonoBehaviour {
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }


  public static void SavePosition(string key, Vector3 data) {
    PlayerPrefs.SetFloat(key + "-PositionX", data.x);
    PlayerPrefs.SetFloat(key + "-PositionY", data.y);
    PlayerPrefs.SetFloat(key + "-PositionZ", data.z);
  }
 
  public static Vector3 LoadPosition(string key) {
    Debug.Log(PlayerPrefs.GetFloat(key + "-PositionX"));
    Debug.Log(PlayerPrefs.GetFloat(key + "-PositionY"));
    return new Vector3() {
      x = PlayerPrefs.GetFloat(key + "-PositionX"),
      y = PlayerPrefs.GetFloat(key + "-PositionY"),
      z = PlayerPrefs.GetFloat(key + "-PositionZ")
    };
  }
}
