using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    public string currentLocation;

    private List<Item> inventory;

    // Use this for initialization
    void Start () {

        SetImage();

        inventory = new List<Item>();

        inventory.Add(new Item(image, "nathan", 5));

    }

    // Update is called once per frame
    void Update () {
        
    }
}