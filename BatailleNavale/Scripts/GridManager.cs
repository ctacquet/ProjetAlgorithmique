﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private int rows = 11;
    private int cols = 11;
    int pos = 0;
    private Grille<int> col0; //vecteur qui contiendra la colonne des chiffres (juste visuel) 
    private Grille<int> row0; //vecteur qui contiendra la ligne des lettres (juste visuel)
    private Grille<int> grille; //matrice qui contiendra les valeurs de l'eau/bateau, libre/raté/touché
    public Sprite WaterDiffuseMini; //var texture des cases d'eau
    public Sprite Cadre; //var textures cases chiffres et lettres
    Canvas myCanvas;
    GameObject cvs;

    // Start is called before the first frame update
    void Start()
    {

        // Canvas ///////////////////////////////
        cvs = new GameObject();
        cvs.name = ("Canvas Main");
        cvs.AddComponent<Canvas>();
        RectTransform Rect3;
        myCanvas = cvs.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.WorldSpace;
        myCanvas.transform.position = new Vector3(5f,-5f, 0);
        Rect3 = myCanvas.GetComponent<RectTransform>();
        Rect3.sizeDelta=new Vector2(11, 11);
        cvs.AddComponent<CanvasScaler>();//useless
        cvs.AddComponent<GraphicRaycaster>();//useless
        this.grille = new Grille<int>(rows-1, cols-1);
        this.col0 = new Grille<int>(cols);
        this.row0 = new Grille<int>(rows);
        // Canvas ////////////////////////////////////

        for (int i = 0; i < rows; i++)
        {
            col0.ajoutVect(i, i);
            row0.ajoutVect(i, i+65); //convertit un entier en char
            }
        grille.setVal(0);
        ShowGrid(col0, row0, grille);
    }

    void ShowGrid(Grille<int> col00, Grille<int> row00, Grille<int> grille00)//affiche les différentes matrices/vecteurs
    {
        int i, j = 0;
        for (i = 0; i < rows; i++)
        {
            CreateTileNumber(pos, 0, i, col00.getVal(i)) ;//creer un sprite avec un textechiffre
            CreateTileChar(pos, i, 0, row00.getVal(i));//creer un sprite avec un texte lettre
        }
        for (i = 0; i < rows - 1; i++)
        {
            for (j = 0; j < cols - 1; j++)
                CreateTileWater(this.pos,i+1, j+1, grille.getVal(i, j));//creer les cases d'eau
        }
    }

        void CreateTileNumber(int pos,int i, int j, int v)//voir CreateTileWater
        {
            GameObject t = new GameObject("X:" + i + "Y:" + j);
            t.transform.position = new Vector3(pos+i,pos-j);
            t.AddComponent<SpriteRenderer>().sprite = Cadre;
            GenText(pos, i, j, v);
    }

    void CreateTileChar(int pos, int i, int j, int v)//voir CreateTileWater
    {
        GameObject t = new GameObject("X:" + i + "Y:" + j);
        t.transform.position = new Vector3(pos+i,pos+j);
        t.AddComponent<SpriteRenderer>().sprite = Cadre;
        GenText(pos, i, j, v);
    }

    void CreateTileWater(int pos, int i, int j, int v)
        {
            GameObject t = new GameObject("X:" + i + "Y:" + j);//creer un gameObject sprite avec un nom donné
            t.transform.position = new Vector3(pos+i, pos-j);//place le gameObject dans la scene
            t.AddComponent<SpriteRenderer>().sprite = WaterDiffuseMini;//creer et attache un rendu au sprite et lui attribut la texture de l'eau
            BoxCollider2D b = new BoxCollider2D();//creer un collider rectangulaire 2D
            t.AddComponent<BoxCollider2D>().autoTiling = b;//attache le collider précédent au sprite en le resizant automatiquement au sprite auquel il est attaché
            t.AddComponent<Selection>();//attache au sprite le script selection
            t.transform.parent = this.transform;//range les cases d'eau dans l'objet auquel le script est lié
    }

    void GenText(int pos, int i, int j, int v)
    {
        GameObject myText;
        Text text;
        RectTransform rectTransform;

        // Text
        myText = new GameObject();
        myText.transform.parent = cvs.transform;
        if (v > 10)
        {
            myText.name = System.Convert.ToString(System.Convert.ToChar(v));
        }
        else
        {
            myText.name = System.Convert.ToString(v);
        }

            text = myText.AddComponent<Text>();
        text.color = Color.black;//couleur de texte noire
        text.font = (Font)Resources.GetBuiltinResource<Font>("Arial.ttf");//utilise la police Arial pour afficher le texte
        text.text = myText.name;
        text.fontSize = 5;

        // Text position
        rectTransform = text.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(5f, -5f, 0);
        rectTransform.sizeDelta = new Vector2(11, 11);
   

    }

        // Update is called once per frame
        void Update()
        {
        }
    }