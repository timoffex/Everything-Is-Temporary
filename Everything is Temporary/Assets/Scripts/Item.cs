using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : IComparable<Item>, IEquatable<Item> {

    private Sprite image;

    private string name;

    private int quantity;

    private int[] position;

    public Item (Sprite itemImage, string itemName, int itemQuantity) {

        image = itemImage;

        name = itemName;

        quantity = itemQuantity;

    }

    public Item (string itemName, int itemQuantity) {

        name = itemName;

        quantity = itemQuantity;

    }

    // Use this for initialization
    void Start () {

        position = new int[2];
        
    }

    // Update is called once per frame
    void Update () {
        
    }

    public Sprite GetImage () {

        return image;

    }

    public void SetImage (Sprite newImage) {

        image = newImage;

    }

    public string GetName () {

        return name;

    }

    public void SetName (string newName) {

        name = newName;

    }

    public int GetQuantity () {

        return quantity;

    }

    public void SetQuantity (int newQuantity) {

        quantity = newQuantity;

    }

    public int CompareTo(Item other) {

        if (other == null) {

            return 1;

        }

        return string.Compare(name, other.GetName(), true);

    }

    public bool Equals (Item other) {

        if (name == other.GetName()) {

            return true;

        }

        return false;

    }

    public override string ToString () {

        return "Name: " + name + " Quantity: " + quantity;

    }
}
