using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

  private Inventory[] inventories;

  private void Awake() {
    inventories = GetComponentsInChildren<Inventory>(true);
  }
  // Start is called before the first frame update
  void Start() {
    foreach(Inventory inventory in inventories) {
      inventory.DeselectItem();
    }

  }

  // Update is called once per frame
  void Update() {

  }
}
