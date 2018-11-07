using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : IComparable<Item> {

    private Sprite image;

    private string name;

    private int quantity;

    private SpriteRenderer sr;

    public Item (Sprite itemImage, string itemName, int itemQuantity) {

        image = itemImage;

        name = itemName;

        quantity = itemQuantity;

    }

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        
    }

    public void ChangeQuantity (int newQuantity) {

        quantity = newQuantity;

    }

    public int CompareTo(Item other) {

        if (other == null) {

            return 1;

        }

        return quantity - other.quantity;

    }
}
