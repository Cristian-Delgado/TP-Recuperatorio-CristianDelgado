using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TP1_Recuperatorio_CristianDelgado
{
    // Realizar un programa que represente una simulación de burbujas ascendiendo en la consola, utilizando el símbolo "o" para cada burbuja.
    // El programa debe cumplir con las siguientes condiciones:
    // Definir una clase Configuracion que almacene los parámetros de la simulación, como la cantidad de filas, columnas,
    // velocidad base del ascenso y cantidad máxima de burbujas permitidas.
    // Definir una clase Burbuja que modele el comportamiento de una burbuja. Cada burbuja debe tener una posición dentro de la consola,
    // una velocidad propia y métodos para mostrarse, borrarse y desplazarse hacia arriba de manera irregular.
    // Usar una lista para administrar todas las burbujas activas durante la simulación.
    // Implementar una lógica que controle el ascenso de las burbujas, evitando que dos burbujas ocupen la misma posición tanto vertical
    // como horizontalmente.
    // Las burbujas deben mover­se de forma más natural: pueden ascender derecho, desviarse levemente hacia la izquierda o derecha, y deben hacerlo con velocidades diferentes entre sí.
    // Cuando una burbuja llegue a la fila superior, deberá eliminarse de la simulación para permitir la aparición de nuevas burbujas.
    // Las burbujas deben aparecer de forma aleatoria, no constante, simulando un comportamiento más realista.
    // El programa debe ejecutarse en un ciclo continuo, generando una animación que simule burbujas ascendiendo dentro de un “acuario” en la consola.

    class Configuracion
    {
        public int Filas;
        public int Columnas;
        public int Velocidad;
        public int MaximoBurbujas;

        public Configuracion()
        {
            this.Filas = 20;
            this.Columnas = 30;
            this.Velocidad = 100;
            this.MaximoBurbujas = 15;
        }

        public Configuracion(int filas, int columnas, int velocidad, int maximo)
        {
            this.Filas = filas;
            this.Columnas = columnas;
            this.Velocidad = velocidad;
            this.MaximoBurbujas = maximo;
        }
    }

    class Burbuja
    {
        public int X;
        public int Y;
        public char Simbolo;
        public int Velocidad;

        public Burbuja(int x, int y, int velocidad)
        {
            this.X = x;
            this.Y = y;
            this.Simbolo = 'o';
            this.Velocidad = velocidad;
        }

        public Burbuja(int x, int y, char simbolo, int velocidad)
        {
            this.X = x;
            this.Y = y;
            this.Simbolo = simbolo;
            this.Velocidad = velocidad;
        }

        public void Dibujar()
        {
            Console.SetCursorPosition(this.X, this.Y);
            Console.Write(this.Simbolo);
        }

        public void Borrar()
        {
            Console.SetCursorPosition(this.X, this.Y);
            Console.Write(" ");
        }

        public void Subir()
        {
            this.Y--;
        }
    }

    class Program
    {
        static Configuracion config;
        static List<Burbuja> burbujasActivas;
        static bool[,] tablero;
        static Random r = new Random();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            config = new Configuracion(20, 30, 100, 15);
            burbujasActivas = new List<Burbuja>();
            tablero = new bool[config.Filas, config.Columnas];
            DateTime ultimo = DateTime.Now;

            while (true)
            {
                TimeSpan tiempo = DateTime.Now - ultimo;
                if (tiempo.TotalMilliseconds >= config.Velocidad)
                {
                    ultimo = DateTime.Now;
                    if (burbujasActivas.Count < config.MaximoBurbujas && r.Next(100) < 40)
                    {
                        int columna = r.Next(config.Columnas);
                        if (!tablero[config.Filas - 1, columna])
                        {
                            int velocidad = r.Next(1, 4);
                            burbujasActivas.Add(new Burbuja(columna, config.Filas - 1, velocidad));
                        }
                    }

                    ActualizarBurbujas();
                    Dibujar();
                }
            }
        }

        static void ActualizarBurbujas()
        {
            for (int i = burbujasActivas.Count - 1; i >= 0; i--)
            {
                Burbuja b = burbujasActivas[i];
                tablero[b.Y, b.X] = false;
                if (b.Y == 0)
                {
                    burbujasActivas.RemoveAt(i);
                }
                else
                {
                    int movimiento = r.Next(-1, 2);
                    if (b.X + movimiento >= 0 && b.X + movimiento < config.Columnas)
                    {
                        b.X = b.X + movimiento;
                    }
                    b.Subir();
                    tablero[b.Y, b.X] = true;
                }
            }
        }

        static void Dibujar()
        {
            Console.Clear();
            for (int fila = 0; fila < config.Filas; fila++)
            {
                for (int col = 0; col < config.Columnas; col++)
                {
                    if (tablero[fila, col])
                    {
                        Console.SetCursorPosition(col, fila);
                        Console.Write("o");
                    }
                }
            }

            foreach (Burbuja b in burbujasActivas)
            {
                b.Dibujar();
            }
        }
    }
}