﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreerJeu : MonoBehaviour
{
    private int ligne = 3, colonne = 9, nbgrilles = 4;
    private Cartons[] grilles;
    private Cartons[] grillesSelection;
    private GridManager[] grid;
    private GridManager gridTirage;

    private Image fillImg;

    private List<int> tirage;
    private List<int> tire;

    private float timer, tempsPioche = 5.0f;
    private int modeJeu = 0;

    private bool fini = false;
    
    //definie un objet physique tile
    GameObject tile;
    GameObject scroll;
    GameObject ObjectMenuGagne;

    //fonction lu au lancement du jeu
    void Awake()
    {
        this.tile = GameObject.Find(0 + ":Case" + 0 + "_" + 0);
        this.scroll = GameObject.Find("Scrollbar");
        this.ObjectMenuGagne = GameObject.Find("ObjectToHide");

        if (!this.tile)
        {
            this.fillImg = GameObject.Find("WaitBar").GetComponent<Image>();
            getNbGrille();
            getTemps();
            initBingo();
            creerBingo();
            tirer();
        }
    }

    //fonction lu à chaque image par seconde
    private void Update()
    {
        if(!fini)
        {
            if (this.tirage.Count > 0)
            {
                //timer de tempsPioche secondes
                timer += Time.deltaTime;
                this.fillImg.fillAmount = timer / tempsPioche;
                if (timer > tempsPioche)
                {
                    tirer();
                    timer = timer - tempsPioche;
                }
            }
            //verifie si on selectionne une case
            select();
            //appel le fonction pour verifier si on gagne
            if (Input.GetKeyDown("g"))
            {
                fini = gagne(this.modeJeu);
            }
        }
        else
        {
            //this.Gagne.SetActive(true);
            afficherBINGO(this.ObjectMenuGagne);
        }
    }

    //recupere le nombre de grille
    private void getNbGrille()
    {
        GameObject value = GameObject.Find("ValueScroll");
        string text = value.transform.GetComponent<TextMeshProUGUI>().text;
        int nb = text[0] - 48;
        this.nbgrilles = nb;
    }

    //recupere le temps d'attente
    private void getTemps()
    {
        GameObject value = GameObject.Find("VariableAttente");
        string text = value.transform.GetComponent<TextMeshProUGUI>().text;
        int nb = text[0] - 48;
        if (nb == 1) nb = 10;
        this.tempsPioche = (float)nb;
    }

    //si le joueur a gagne le petit menu pour rejouer ou revenir au menu affiche
    private void afficherBINGO(GameObject parent)
    {
        Transform[] go = parent.GetComponentsInChildren<RectTransform>(true);
        go[3].gameObject.SetActive(true);
    }

    //initialise toutes les grilles ainsi que les listes
    private void initBingo()
    {

        this.grilles = new Cartons[this.nbgrilles];
        this.grillesSelection = new Cartons[this.nbgrilles];
        this.grid = new GridManager[this.nbgrilles];

        this.tirage = new List<int>();
        this.tire = new List<int>();
    }

    //initialise la case ainsi que la liste des nombres à tirer
    private void initTirage()
    {
        Transform parent;
        for (int i = 0; i < 90; i++) this.tirage.Add(i + 1);
        this.gridTirage = new GridManager();
        parent = GameObject.Find("GridTirage").transform;
        this.gridTirage.GenerateVal(parent.position.x, parent.position.y, parent);
    }

    //fonctione appeler pour creer le jeu de Bingo
    private void creerBingo()
    {
        Transform parent;
        do
        {
            for (int i = 0; i < this.nbgrilles; i++)
            {
                this.grilles[i] = new Cartons(this.ligne, this.colonne);
                this.grilles[i].initGrille();
                this.grillesSelection[i] = new Cartons(this.ligne, this.colonne);
                this.grillesSelection[i].copie(this.grilles[i]);

                this.grid[i] = new GridManager(this.grilles[i], i);
            }
        }
        while (!valCorrect(this.grilles) && (this.nbgrilles < 4));

        ajoutVal();

        for (int i = 0; i < nbgrilles; i++)
        {
            parent = GameObject.Find("GridManager " + i).transform;
            this.grid[i].GenerateGrid(parent.position.x, parent.position.y, parent);
        }

        initTirage();
    }

    //verifie si le nombre de case vide est correct
    public bool valCorrect(Cartons[] b)
    {
        int cpt;
        int nbCaseFullCol = b.Length * 3 - 10;
        if (nbCaseFullCol < 0) nbCaseFullCol = 0;
        for (int i = 0; i < this.colonne; i++)
        {
            cpt = 0;
            for (int j = 0; j < b.Length; j++)
            {
                for (int k = 0; k < this.ligne; k++)
                {
                    if (b[j].getVal(k, i) == -1)
                    {
                        cpt++;
                    }
                }
            }
            if ((cpt < nbCaseFullCol) || (cpt > nbCaseFullCol + 4)) return false;
        }

        return true;
    }

    //ajoute les valeurs dans les grilles
    private void ajoutVal()
    {
        int[] vals = new int[this.ligne * this.nbgrilles];

        for (int i = 0; i < this.colonne; i++)
        {
            copieValCol(vals, i);
            genRand(vals, 9 + i * 10, i * 10);
            trieVal(vals);
            setValCol(vals, i);
        }
    }

    //copie les valeurs de la colonne col dans vals
    private void copieValCol(int[] vals, int col)
    {
        int i = 0;
        while (i < vals.Length)
        {
            for (int j = 0; j < this.grilles.Length; j++)
            {
                for (int k = 0; k < this.ligne; k++)
                {
                    vals[i] = this.grilles[j].getVal(k, col);
                    i++;
                }
            }
        }
    }

    //verifie si la valeur x est dans le tableau t
    private bool estdans(int x, int[] t)
    {
        foreach (int i in t)
        {
            if (i == -1) continue;
            if (x == i) return true;
        }
        return false;
    }

    //rempli la teableau t de valeur random entre min et max
    private void genRand(int[] t, int max, int min)
    {
        int ind;
        for (int i = 0; i < t.Length; i++)
        {
            //ne rempli que si la case peut l'etre (-1 correspond à une case vide)
            if (t[i] != -1)
            {
                do
                {
                    ind = Random.Range(min, max);
                } while (estdans(ind, t));
                t[i] = ind;
            }
        }
    }

    //rempli la colonne col des grilles avec les valeurs dans vals
    private void setValCol(int[] vals, int col)
    {
        int i = 0;
        while (i < vals.Length)
        {
            for (int j = 0; j < this.grilles.Length; j++)
            {
                for (int k = 0; k < this.ligne; k++)
                {
                    this.grilles[j].setVal(k, col, vals[i]);
                    i++;
                }
            }
        }
    }

    //tri les valeurs d'un tableau
    private void trieVal(int[] val)
    {
        int min, temp;
        for (int i = 0; i < val.Length - 1; i++)
        {
            min = i;
            for (int j = i + 1; j < val.Length; j++)
            {
                //trie uniquement si la case n'est pas vide
                if (val[j] != -1)
                {
                    if (val[j] < val[min])
                    {
                        temp = val[j];
                        val[j] = val[min];
                        val[min] = temp;
                    }
                }
            }
        }
    }

    //tire un nombre random dans la liste des nombres a tirer
    private void tirer()
    {
        if(this.tirage.Count != 0)
        {
            int ind = Random.Range(0, this.tirage.Count);
            this.gridTirage.UpdateVal(this.tirage[ind]);
            this.tire.Add(this.tirage[ind]);
            this.tirage.RemoveAt(ind);
        }
    }

    //fonctionne qui verifie si un joueur à gagné en fonction du mode de jeu
    private bool gagne(int mode)
    {
        switch (mode)
        {
            case 0:
                return verifLigne();
            case 1:
                return verifCarton();
            default:
                return false;
        }
    }

    //mode de jeu 1, verifie si une ligne est correct
    private bool verifLigne()
    {
        int cpt = 0;
        int val;
        for (int i = 0; i < nbgrilles; i++)
        {
            for (int j = 0; j < this.ligne; j++)
            {
                for (int k = 0; k < this.colonne; k++)
                {
                    val = this.grillesSelection[i].getVal(j, k);
                    if (val != -1)
                    {
                        if (this.tire.Contains(val))
                        {
                            Debug.Log(val);
                            cpt++;
                        }
                        else
                        {
                            cpt = 0;
                            break;
                        }
                    }
                }
                if (cpt != 0) return true;
            }
        }
        return false;
    }

    //mode de jeu 2, verifie si un carton est correct
    private bool verifCarton()
    {
        int cpt = 0;
        int val;
        for (int i = 0; i < nbgrilles; i++)
        {
            for (int j = 0; j < this.ligne; j++)
            {
                for (int k = 0; k < this.colonne; k++)
                {
                    val = grillesSelection[i].getVal(j, k);
                    if (val != -1)
                    {
                        if (tire.Contains(val)) cpt++;
                        else
                        {
                            cpt = 0;
                            break;
                        }
                    }
                }
                if (cpt == 0) break;
            }
            if (cpt != 0) return true;
        }

        return false;
    }

    /*verifie si un joueur selectionne une case pour la changer de couleur
    et mettre la valeur contenu dans la case dans la grille de selection*/
    private void select()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject tile = GameObject.Find(hit.transform.gameObject.name);
                
                getind(tile);
            }
        }
    }

    //verifie sur quelle case le joueur a cliqué
    private void getind(GameObject sprite)
    {
        string name = sprite.name;
        string ind = Regex.Replace(name, "[^0-9]", "");

        int numGrille = ind[0] - 48;
        int n = ind[1] - 48;
        int m = ind[2] - 48;

        if(grillesSelection[numGrille].getVal(n, m) != -1)
        {
            if (isSelect(numGrille, n, m))
            {
                grillesSelection[numGrille].setVal(n, m, 0);
                changeColor(sprite, Color.white);
            }
            else
            {
                grillesSelection[numGrille].setVal(n, m, grilles[numGrille].getVal(n, m));
                changeColor(sprite, Color.Lerp(Color.black, Color.grey, 0.6f));
            }
        }
        
        //debug pour verifier si la fonction gagne() fonctionne
        this.tire.Add(grilles[numGrille].getVal(n, m));
    }

    //change la couleur de la case selectionnée
    private void changeColor(GameObject sprite, Color couleur)
    {
        sprite.GetComponent<SpriteRenderer>().color = couleur;
    }

    //verifie si une case etait deja selectionnée
    private bool isSelect(int b, int n, int m)
    {
        int val = grillesSelection[b].getVal(n, m);
        if (val != 0)
        {
            return true;
        }
        return false;
    }

    //permet de visualiser un tableau dans la console de debug
    private void afficher(int[] val)
    {
        foreach (int i in val) Debug.Log("valeur: " + i);
    }

}