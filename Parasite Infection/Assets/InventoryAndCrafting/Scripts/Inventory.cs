using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour {
  public List<Item> playerItems = new List<Item>();
  [SerializeField] private UIInventory inventoryUI;
  ItemDatabase itemDatabase;

  public void Save() {
    ES3.Save<List<Item>>("Inventory", playerItems);
  }

  public void Load() {
    // TODO see if the correct inventory is loaded or if it gets appended to something else
    List<Item> itemsToLoad = ES3.Load<List<Item>>("Inventory");
    itemsToLoad.ForEach(item => GiveItem(itemDatabase.GetItemId(item)));
  }

  void Awake() {
    if (FindObjectsOfType<Inventory>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
    itemDatabase = FindObjectOfType<ItemDatabase>();

  }

  private void Start() {
    inventoryUI.gameObject.SetActive(false);
  }

  public void GiveItem(int id) {
    Item itemToAdd = itemDatabase.GetItem(id);
    PerformGiveItem(itemToAdd);
  }

  public void GiveItem(string itemName) {
    Item itemToAdd = itemDatabase.GetItem(itemName);
    PerformGiveItem(itemToAdd);
  }

  private void PerformGiveItem(Item itemToAdd) {
    inventoryUI.AddItemToUI(itemToAdd);
    playerItems.Add(itemToAdd);
  }

  public Item CheckForItem(int id) {
    return playerItems.Find(item => item.id == id);
  }

  public Item CheckForItem(string itemName) {
    return playerItems.Find(item => item.itemName == itemName);
  }

  public void RemoveItem(int id) {
    Item itemToRemove = CheckForItem(id);
    PerformRemoveItem(itemToRemove);
  }

  public void RemoveItem(string itemName) {
    Item itemToRemove = CheckForItem(itemName);
    PerformRemoveItem(itemToRemove);
  }

  private void PerformRemoveItem(Item itemToRemove) {
    if (itemToRemove != null) {
      playerItems.Remove(itemToRemove);
    }
  }

  public void ClearInventory() {
    inventoryUI.ClearSlots();
    playerItems.Clear();
  }
}
