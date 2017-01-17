﻿using UnityEngine;
using System.Collections;

public class Path : BaseNode {

    public MeshRenderer model;
    public Material passableMaterial;

    public override void SpreadActive() {
        if (!canPass) {
            ChangeModel();
            base.SpreadActive();
        }
    }

    void ChangeModel() {
        model.material = passableMaterial;
    }
}