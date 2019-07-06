using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tablero : MonoBehaviour
{
    // Se crean dos variables de tipo entero
    int ancho = 10;
    int alto = 10;

    public GameObject pieza;// se crea un objeto para asignar la forma
    private GameObject[,] esf;

    //Se crean cuatro variables de color 
    public Color ColorBase;
    public Color ColorJugador1;
    public Color ColorJugador2;
    public Color ColorPared;

    //Se crea un booleano para cambiar de jugador.
    private bool turnoJugador1;

    //Se crea un booleano para desactivar el Juego.
    private bool FinJuego = true; 

    public void Start()
    {
        esf = new GameObject[ancho, alto];//se inicia la matriz

        for (int i = 0; i < ancho; i++)// Se crea un bucle para crear la matriz en eje x
        {
            for (int j = 0; j < alto; j++)// Se crea un bucle para crear la matriz en eje x
            {
                GameObject esfera = GameObject.Instantiate(pieza) as GameObject;// al objeto esfera se le asigna la forma
                Vector3 position = new Vector3(i, j, 0);// se crea un vector don se le asigna las variable de los ciclos.
                esfera.transform.position = position; // al objeto se le asigna la posicion
                esfera.GetComponent<Renderer>().material.color = ColorBase; // se pinta las esferas con el color de la base
                esf[i, j] = esfera;//se le asigna el objeto a la matriz
            }
        }
    }

    public void Update()
    {
        if (FinJuego == true)//Se crea una condicion que revisa cuando el juego finaliza.
        {
            Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//se crea un vector que indique la posicion del mouse
            SeleccionarFicha(mPosition);//se llama a la funcion seleccionar ficha.
        }
    }

    public void SeleccionarFicha(Vector3 position)//se crea un funcion para la seleccion de ficha
    {
        //Se crea dos variables de tipo entero que resive la posicion del mouse y las convierte en la posicion de las esfera
        int i = (int)(position.x + 0.5f);
        int j = (int)(position.y + 0.5f);

        if (Input.GetButtonDown("Fire1"))//se crea un condicional que revise cuando se da clic
        {
            if (i >= 0 && j >= 0 && i < ancho && j < alto)//Se crea un condicional para revisar que las variables no salgan de la matriz.
            {
                GameObject esfera = esf[i, j];//Al dar clic se manda las coordenadas de la esfera para poder pintar.
                if (esfera.GetComponent<Renderer>().material.color == ColorBase)//se crea un condicional para revisar si la esfera es igual al color de la base.
                {
                    Color colorAUsar = Color.clear;//reinicia el color de la variable
                    if (turnoJugador1)
                        colorAUsar = ColorJugador2;//Se le asigna el color del jugador 2.
                    else
                        colorAUsar = ColorJugador1;//Se le asigna el color del jugador 3.

                    esfera.GetComponent<Renderer>().material.color = colorAUsar; //Pinta la esferea dependiendo el color.
                    turnoJugador1 = !turnoJugador1;//Cambia el valor del booleano.
                    RevisionHorizontal(i, j, colorAUsar);//Se llama la funcion que Revisa las lineas horizontales
                    RevisionVertical(i, j, colorAUsar);//Se llama la funcion que Revisa las lineas verticales
                    RevisionDiagonalArriba(i, j, colorAUsar);//Se llama la funcion que Revisa las Diagonales hacia arriba
                    RevisionDiagonalAbajo(i, j, colorAUsar);//Se llama la funcion que Revisa las Diagonales hacia abajo
                    Pared(i, j, colorAUsar);//Se llama a la funcion pared
                }
            }
        }
    }

    public void RevisionHorizontal(int x, int y, Color colorAVerificar) //Se crea una funcion para Revisar las esferas en el eje X.
    {
        int contador = 0;//Se crea una variable de tipo entero.

        for (int i = x-3; i <= x+3; i++)//se crea un bucle que revisara la posicion de x, restara y sumara 3.
        {
            if (i < 0 || i >= ancho)//se coloca un condicional que revise si es menor a 0 o mayor a 9;
            continue;// si la condicion es verdadera esto se salta el codigo.

            GameObject esfera = esf[i, y];// se le asigna i para a la matriz de la esfera para revisar el color.

            if (esfera.GetComponent<Renderer>().material.color == colorAVerificar)//se coloca un condicional que revise si el color de la esfera es igual al anterior.
            {
                contador++;// el contador aumenta una unidad
                if (contador == 4)//Se crea un condicional que revise si el contador llega a 4
                {
                    Debug.Log("Ganastes");//se imprime en consola 
                    FinJuego = false;//el booleano se pone en falso para que se acabe el juego
                }
            }
            else
                contador = 0;//El contador vuelve a cero.
        }
    }

    public void RevisionVertical(int x, int y, Color colorAVerificar) //Se crea una funcion para Revisar las esferas en el eje Y.
    {
        int contador = 0; //Se crea una variable de tipo entero.
        for (int j = y - 3; j <= y + 3; j++) //se crea un bucle que revisara la posicion de y, restara y sumara 3.
        {
            if (j < 0 || j >= alto)// se coloca un condicional que revise si es menor a 0 o mayor a 9;
            continue;// si la condicion es verdadera esto se salta el codigo.

            GameObject esfera = esf[x, j];// se le asigna  j para a la matriz de la esfera para revisar el color.

            if (esfera.GetComponent<Renderer>().material.color == colorAVerificar)//se coloca un condicional que revise si el color de la esfera es igual al anterior.
            {
                contador++;// el contador aumenta una unidad
                if (contador == 4)//Se crea un condicional que revise si el contador llega a 4
                {
                    Debug.Log("Ganastes");//se imprime en consola 
                    FinJuego = false; //el booleano se pone en falso para que se acabe el juego
                }
            }
            else
                contador = 0;//El contador vuelve a cero.
        }
    }

    public void RevisionDiagonalArriba(int x, int y, Color colorAVerificar)  //Se crea una funcion para verificar las esferas en Diagonal hacia arriba.
    {
        int contador = 0;//Se crea una variable de tipo entero.
        for (int i = x - 4; i <= x + 2;) //se crea un bucle que revisara la posicion de x, restara 4 y este ira hasta 2.
        {
            for (int j = y - 4; j <= y + 2;) //se crea un bucle que revisara la posicion de y, restara 4 y este ira hasta 2.
            {
                i++;//aumenta j una unidad
                j++;//aumenta i una unidad

                if (j < 0 || j >= alto || i < 0 || i >= ancho)// se coloca un condicional que revise si es menor a 0 o mayor a 9;
                    continue; // si la condicion es verdadera esto se salta el codigo.

                GameObject esfera = esf[i, j];//Se le asignan las variables a la matriz

                if (esfera.GetComponent<Renderer>().material.color == colorAVerificar)//se coloca un condicional que revise si el color de la esfera es igual al anterior.
                {
                    contador++;// el contador aumenta una unidad
                    if (contador == 4)//Se crea un condicional que revise si el contador llega a 4
                    {
                        Debug.Log("Ganaste");//se imprime en consola 
                        FinJuego = false; //el booleano se pone en falso para que se acabe el juego
                    }
                }
                else
                    contador = 0;//El contador vuelve a cero.
            }
            
        }
    }

    public void RevisionDiagonalAbajo(int x, int y, Color colorAVerificar) //Se crea una funcion para verificar las esferas en Diagonal hacia abajo.
    {
        int contador = 0;//Se crea una variable de tipo entero.
        for (int i = x - 4; i <= x + 2;)//se crea un bucle que revisara la posicion de x, restara 4 y este ira hasta 2.
        {
            for (int j = y + 4; j >= y - 2;)//se crea un bucle que revisara la posicion de j, suma 4 y este ira hasta -2.
            {
                i++;//aumenta i una unidad
                j--;//Disminuye j una unidad

                if (j < 0 || j >= alto || i < 0 || i >= ancho) // se coloca un condicional que revise si es menor a 0 o mayor a 9;
                    continue;// si la condicion es verdadera esto se salta el codigo.

                GameObject esfera = esf[i, j];//Se le asignan las variables a la matriz

                if (esfera.GetComponent<Renderer>().material.color == colorAVerificar)//se coloca un condicional que revise si el color de la esfera es igual al anterior.
                {
                    contador++;// el contador aumenta una unidad

                    if (contador == 4)//Se crea un condicional que revise si el contador llega a 4
                    {
                        Debug.Log("Ganaste");//se imprime en consola 
                        FinJuego = false; //el booleano se pone en falso para que se acabe el juego
                    }
                }
                else
                   contador = 0;// el contador vuelve a cero
            }

        }
    }

    public void Pared(int x, int y, Color colorAVerificar)//Se crea una funcion para generar paredes aleatorias
    {
        //Se crea dos variables de tipo entero con numeros random con rango de 0 a 10
        int i = Random.Range(0, 10);
        int j = Random.Range(0, 10);

        GameObject esfera = esf[i, j];// se le asigna las variables a la matriz;

        if (esfera.GetComponent<Renderer>().material.color == ColorBase)//Se crea una condicion para revisar si el color es igual a la base
        {
            esfera.GetComponent<Renderer>().material.color = ColorPared;//pinta la esfera 
        }
    }


}