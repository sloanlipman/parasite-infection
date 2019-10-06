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
    // try {
      List<Item> itemsToLoad = ES3.Load<List<Item>>("Inventory", this.playerItems);
      itemsToLoad.ForEach(item => GiveItem(itemDatabase.GetItemId(item)));

    // } catch {

    // }
  }

  void Awake() {
    if (FindObjectsOfType<Inventory>().Length > 1) {
      Destroy(this.gameObject);
    }
    DontDestroyOnLoad(this.gameObject);
    itemDatabase = FindObjectOfType<ItemDatabase>();

  }

  private void Start() {
    Load();

    if (playerItems.Count == 0) {
      GiveItem(6);
      GiveItem(9);
      GiveItem(10);
    }
    Save();
    inventoryUI.gameObject.SetActive(false);
  }

  private void Update() {
    // if (!Helper.isBattleCurrentScene() && Input.GetKeyDown(KeyCode.I)) {
    //   inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
    // }
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
}
