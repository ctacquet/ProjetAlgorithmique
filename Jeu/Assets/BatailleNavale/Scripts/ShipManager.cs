﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipManager
{
    private GameObject SM;//GO
    private List<Ship> LShip;//Liste de bateaux
    private int totalHP = 0;//Hp totaux de la flotte

    public ShipManager(string nom, Vector3 pos)
    {
        //Creation des bateaux pour le ShipManager (script) en cours
        SM = new GameObject(nom);
        LShip = new List<Ship>();
        Ship Torpilleur0 = new Ship(SM, "Torpilleur 0", 2, new Vector3(2, 1, 1), "txtTp", "ShipLayer");
        LShip.Add(Torpilleur0);
        Ship ContreToprilleur1 = new Ship(SM, "ContreTorpilleur 1", 3, new Vector3(3, 1, 1), "txtCtp", "ShipLayer");
        LShip.Add(ContreToprilleur1);
        Ship SousMarin2 = new Ship(SM, "SousMarin 2", 3, new Vector3(3, 1, 1), "txtSm", "ShipLayer");
        LShip.Add(SousMarin2);
        Ship Croiseur3 = new Ship(SM, "Croiseur 3", 4, new Vector3(4, 1, 1), "txtCs", "ShipLayer");
        LShip.Add(Croiseur3);
        Ship PorteAvion4 = new Ship(SM, "PorteAvion 4", 5, new Vector3(5, 1, 1), "txtPa", "ShipLayer");
        LShip.Add(PorteAvion4);

        for (int i = 0; i < 5; i++) //Set les points de vie et positions initiales de tout les bateaux
        {
            SM.transform.GetChild(i).transform.position = new Vector3(pos.x + 12.5f, pos.y + 1 + i * 2, pos.z + 0);
            totalHP = totalHP + LShip[i].getHP();
            getClassShip(i).updateG();
        }
    }

    public void moveShip(float x, float y, float z) //Déplace les 5 bateaux en fonction des coords x y z en entrée.
    {
        Vector3 V;
        for (int i = 0; i < 5; i++)
        {
            if (SM.transform.GetChild(i).GetComponent<Draggable>().getMag())
            {
                V = SM.transform.GetChild(i).position;
                SM.transform.GetChild(i).position = new Vector3(V.x + x, V.y + y, V.z + z);
            }
        }
    }

    public Ship getClassShip(int i) //Retourne le bateau de la liste à l'indice donné
    {
        return LShip[i];
    }

    public GameObject getShip(int i) //Retourne le GO du bateau en fonction de l'indice donné
    {
        return SM.transform.GetChild(i).gameObject;
    }


    public bool checkContact(int ix) //Vérifie que deux bateaux ne se chevauche l'un l'autre (placement), renvoie True si chevauchage, false sinon
    {
        for (int i = 0; i < 5; i++)
        {
            if (LShip[i].Equals(LShip[ix]) != true)//!
            {
                for (int j = 0; j < LShip[ix].getLength(); j++)
                {
                    for (int k = 0; k < LShip[i].getLength(); k++)
                    {
                        if (Vector3.Distance(LShip[ix].getVecteur().getVal(j), LShip[i].getVecteur().getVal(k)) == 0)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool checkTir(Vector3 V) //Vérifie qu'un tir a touché un bateau adverse, place un sprite touché ou raté sur la grille marquage du tireur et sur la grille de base de la cible, retourne true si touché, false sinon.
    {
        CanvasGenerator CvsGN = GameObject.FindObjectOfType<GameNavale>().getCvsGN();
        for (int i = 0; i < 5; i++)
        {
            for (int k = 0; k < LShip[i].getLength(); k++)
            {
                Debug.Log(V + "  +  " + LShip[i].getVecteur().getVal(k));
                if (Vector3.Distance(V, LShip[i].getVecteur().getVal(k)) == 0)
                {
                    LShip[i].hit();
                    GameObject marquet = new GameObject("Touche" + i + k);
                    marquet.transform.position = V;
                    marquet.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/Flame");
                    marquet.GetComponent<SpriteRenderer>().sortingLayerName = "ShipLayer";
                    return true;
                }
            }
        }
        GameObject marquer = new GameObject("rate" +V.x+V.y);
        marquer.transform.position = V;
        marquer.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Textures/WaterDiffuseMini2");
        marquer.GetComponent<SpriteRenderer>().color = Color.grey; ;
        marquer.GetComponent<SpriteRenderer>().sortingLayerName = "ShipLayer";
        CvsGN.setText(4, "");
        GameObject.Find("TextSlider1").GetComponent<Text>().text = "Raté";
        CvsGN.getPanel(4).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Miss");
        CvsGN.setText(5, "");
        GameObject.Find("TextSlider2").GetComponent<Text>().text = "Raté";
        CvsGN.getPanel(5).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/Miss");
        return false;
    }

    public int getHPtotal()// Retourne le nombre de point de vie total de la flotte
    {
        totalHP = 0;
        for (int i = 0; i < 5; i++)
        {
            totalHP = totalHP + LShip[i].getHP();
        }
        return totalHP;
    }
}

