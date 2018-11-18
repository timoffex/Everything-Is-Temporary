using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public Sprite itemImage;

    public string itemName;

    public int itemQuantity;

    public bool createItem;

    public string selectedItem;

    public bool updateItem, removeItem;

    public bool emptyInventory;

    public bool printInventory;

    [Range(1, 10)]
    public int gridSize;

    private int maxInventorySize;

    private List<Item> inventory;

    private Item[,] grid;

    private int[] currentIndex;

    // Use this for initialization
    void Start () {

        Time.fixedDeltaTime = 0.25f;

        inventory = new List<Item>();

        grid = new Item[gridSize, gridSize];

        maxInventorySize = gridSize * gridSize;

        currentIndex = new int[2];

    }

    // Update is called once per frame
    void FixedUpdate () {

        if (createItem) {

            createItem = false;

            Item item = new Item(itemImage, itemName, itemQuantity);

            AddItem(item);

        }

        if (updateItem) {

            updateItem = false;

            Item item = FindItemByName(selectedItem);

            UpdateItem(item, itemImage, itemName, itemQuantity);

        }

        if (removeItem) {

            removeItem = false;

            Item item = FindItemByName(selectedItem);

            DeleteItem(item);

        }

        if (emptyInventory) {

            emptyInventory = false;

            EmptyInventory();

        }

        if (printInventory) {

            printInventory = false;

            PrintInventory();

        }

    }

    public void AddItem (Item item) {

        if (inventory.Contains(item)) {

            Debug.Log(item.GetName() + " already exists. Call UpdateItem to modify " + item.GetName());

            return;

        }

        inventory.Add(item);

        DisplayGrid();

    }

    public void UpdateItem (Item item, Sprite newSprite, string newName, int newQuantity) {

        if (inventory.Contains(item)) {

            item.SetImage(newSprite);

            item.SetName(newName);

            item.SetQuantity(newQuantity);

            DisplayGrid();

        } else {

            Debug.Log("Item not found");

        }

    }

    public void DeleteItem (Item item) {

        if (inventory.Contains(item)) {

            inventory.Remove(item);

            DisplayGrid();

        } else {

            Debug.Log("Item not found");

        }

    }

    public void EmptyInventory () {

        inventory.Clear();

    }

    public Item FindItemByName (string name) {

        Item item = new Item("", -1);

        foreach (Item i in inventory) {

            if (i.GetName() == name) {

                return item = i;

            }

        }

        Debug.Log("Item not found. Returning empty Item");

        return item;

    }

    public void DisplayGrid () {

        inventory.Sort();

    }

    public void PrintInventory () {

        string buffer = "";

        foreach (Item item in inventory) {

            buffer += item + "\n";

        }

        if (buffer == "") {

            Debug.Log("Inventory Empty");

        } else {

            Debug.Log(buffer);

        }

    }
}
