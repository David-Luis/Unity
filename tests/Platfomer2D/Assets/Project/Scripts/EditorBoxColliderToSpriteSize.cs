﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorBoxColliderToSpriteSize : MonoBehaviour {

    void Awake()
    {
        runInEditMode = true;
    }

    void Update()
    {
        var _sprite = GetComponent<SpriteRenderer>();
        var _collider = GetComponent<BoxCollider2D>();

        _collider.offset = new Vector2(0, 0);
        _collider.size = new Vector3(_sprite.bounds.size.x ,
                                     _sprite.bounds.size.y ,
                                     _sprite.bounds.size.z / transform.lossyScale.z);
    }
}
