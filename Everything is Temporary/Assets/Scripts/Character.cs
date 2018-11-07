using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Character : MonoBehaviour {

    public Sprite image;

    public string name;

    public string description;

    public string mood;

    private SpriteRenderer sr;

    // Use this for initialization
    void Start () {

        SetImage();
        
    }

    // Update is called once per frame
    void Update () {
        
    }

    protected void SetImage () {

        sr = GetComponent<SpriteRenderer>();

        sr.sprite = image;

    }
}
