﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class GridManager
{
    private GameObject tileReference;
    private int ligne, colonne;
    private float espacement = 1.1f;
    private GrilleSudoku grille;

    public GridManager(GrilleSudoku grille)
    {
        tileReference = GameObject.Find("TilePrefab");
        this.grille = grille;
        this.colonne = grille.getCols();
        this.ligne = grille.getRows();
    }

    public void GenerateGrid(float posX, float posY, Transform parent)
    {
        for (int i = 0; i < this.ligne; i++)
        {
            for (int j = 0; j < this.colonne; j++)
            {
                Vector2 pos = new Vector2(posX + (j * espacement - (this.colonne - 1) * espacement / 2), posY + (i * -espacement - (this.ligne - 1) * -espacement / 2));
                GameObject tile = UnityEngine.Object.Instantiate(tileReference, pos, tileReference.transform.rotation, parent);
                tile.name = "Case" + i + "_" + j;
                afficher(i, j, tile);
            }
        }
    }

    public void UpdateGrid()
    {
        for (int i = 0; i < this.ligne; i++)
        {
            for (int j = 0; j < this.colonne; j++)
            {
                GameObject tile = GameObject.Find("Case" + i + "_" + j);
                afficher(i, j, tile);
            }
        }
    }
    
    private void afficher(int i, int j, GameObject tile)
    {
        Color blue = new Color(0, 0, 240, 200);
        Color white = new Color(255, 255, 255, 200);
        tile.GetComponent<SpriteRenderer>().color = Color.gray;
        if (this.grille.getVal(i, j).getChangeable() == false)
        {
            tile.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = this.grille.getVal(i, j).ToString();
            tile.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = white;
        }
        else {
            tile.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().color = blue;
        }
    }
}