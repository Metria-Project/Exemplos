using System;
using Metria.Hyperbolic._2;
using System.Drawing;
using Metria.Hyperbolic._2.Voronoi;
using System.Collections.Generic;

namespace Teste_de_Grupos
{
    public struct Mobius
    {
        public float a, b, c, d;
    }

    class Program
    {
        public static float porcent(float a, float b, float t)
        {
            return a + ((b - a) * t) / 10000;
        }
        static int contador;
        static PointF _centro;
        static VoronoiCell C;
        static Bitmap Canvas;
        static Graphics _vincent;
        static Pen _bic;
        static Metria.Hyperbolic._2.Point P;
        static Metria.Hyperbolic._2.Line L;
        static float ppu;

        static List<KeyValuePair<char, Mobius>> geradores;
        static Dictionary<char, Mobius> DicGeradores;
        static Dictionary<string, Metria.Hyperbolic._2.Point> Pontos;
        static Queue<KeyValuePair<Metria.Hyperbolic._2.Point, string>> ProxPontos;
        static void Main(string[] args)
        {
            _centro = new PointF(800, 790);
            P = null;
            L = null;
            bool mock = true;
            ppu = 300f;
            Canvas = new Bitmap(1600, 800);
            _vincent = Graphics.FromImage(Canvas);
            _bic = new Pen(Color.Black);
            List<String> Log = new List<string>();
            float[] vet1 = new float[4];
            vet1[0] = 1f;
            vet1[1] = 1f;
            vet1[2] = 0f;
            vet1[3] = 1f;

            float[] vet2 = new float[4];
            vet2[0] = 0f;
            vet2[1] = -1f;
            vet2[2] = 1f;
            vet2[3] = 0f;

            float[] vet3 = new float[4];
            vet3[0] = 3f;
            vet3[1] = 2f;
            vet3[2] = 0f;
            vet3[3] = 1f;

            float[] vet4 = new float[4];
            vet4[0] = 2;
            vet3[1] = 0;
            vet3[2] = 0;
            vet4[3] = 1;
            string path = "C:\\Voronoi3\\";
            System.IO.Directory.CreateDirectory(path + "finalLadrilho");
            System.IO.Directory.CreateDirectory(path + "finalMax");

            for (int n = 9688; n <= 10000; n++)
            {

                geradores = new List<KeyValuePair<char, Mobius>>();
                DicGeradores = new Dictionary<char, Mobius>();
                ProxPontos = new Queue<KeyValuePair<Metria.Hyperbolic._2.Point, string>>();
                Pontos = new Dictionary<string, Metria.Hyperbolic._2.Point>();
                C = new VoronoiCell(new Metria.Hyperbolic._2.Point(0, 1, 2, 1));
                ProxPontos.Enqueue(new KeyValuePair<Metria.Hyperbolic._2.Point, string>(new Metria.Hyperbolic._2.Point(0, 1, 2, 1), ""));
                Add_gerador('a', 'b', porcent(vet1[0],vet3[0],n), porcent(vet1[1], vet3[1], n), porcent(vet1[2], vet3[2], n), porcent(vet1[3], vet3[3], n));
                Add_gerador('c', 'd', porcent(vet2[0], vet4[0], n), porcent(vet2[1], vet4[1], n), porcent(vet2[2], vet4[2], n), porcent(vet2[3], vet4[3], n));
                //Add_gerador('e', 'f', 3, 2, 0, 1);
                //Add_gerador('g', 'h', 43250, 100, 012, 154);
                System.IO.Directory.CreateDirectory(path+"cell"+n);
                Pontos.Add("", new Metria.Hyperbolic._2.Point(0, 1, 2, 1));

                for (int i = 0; i < 7; i++)
                {
                    Incrementa_lista();
                }
                int j = 0;
                contador = 0;

                foreach (var item in Pontos)
                {
                    try
                    {
                        contador++;
                        P = item.Value;
                        if (P != C.Center)
                        {
                            
                            L = new LineSegment(C.Center, P).MediumBissector();
                            /**
                            _vincent.Clear(Color.White);
                            Desenha();
                            DesenhaPonto(P);
                            Canvas.Save(path + "cell" + n + "\\" + j + "_0.bmp");
                            /**/C.CutPoligonS(L);/*/// ref mock);
                            P = null;
                            L = null;
                            _vincent.Clear(Color.White);
                            Desenha();
                            DesenhaPonto(P);
                            Canvas.Save(path + "cell" + n + "\\" + j + "_1.bmp");
                            j++;/**/

                        }
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("Aqui deu ruim em " + n + "/" + j + " " + e.Message);
                    }
                }

                //Console.WriteLine("Foram criados {0} pontos", contador);
                L = null;
                

                Desenha();
                foreach (KeyValuePair<string,Metria.Hyperbolic._2.Point> P in Pontos)
                {
                    DesenhaPonto(P.Value);
                }
                Console.WriteLine("Celula " + n + " criada com "+Pontos.Count+" pontos");
                Canvas.Save(path+"finalMax\\" + n+".bmp");
                DesenhaLadrilho();
                Canvas.Save(path + "finalLadrilho\\" + n + ".bmp");
            }
            Console.WriteLine("Aperte qualquer tecla para continuar");
            Console.ReadKey();
        }

        static bool Add_gerador(char sigla,char sigla2, float a, float b, float c,float d)
        {
            Mobius m = new Mobius()
            {
                a = a,
                b = b,
                c = c,
                d = d
            };
            KeyValuePair<char, Mobius> par = new KeyValuePair<char, Mobius>(sigla, m);
            if (a * d - b * c <= 0 || geradores.Contains(par))
            {
                return false;
            }
            geradores.Add(par);
            DicGeradores.Add(sigla, m);
            m.a = d;
            m.b = -b;
            m.c = -c;
            m.d = a;
            par = new KeyValuePair<char, Mobius>(sigla2, m);
            geradores.Add(par);
            DicGeradores.Add(sigla2, m);
            return true;
        }

        static Metria.Hyperbolic._2.Point Aplica_mobius(Metria.Hyperbolic._2.Point P , Mobius m)
        {
            if(P.Y>0)
            {
                float divisor = ((m.c * P.X + m.d) * (m.c * P.X + m.d)) + (m.c * m.c * P.Y * P.Y);
                return new Metria.Hyperbolic._2.Point((m.a * P.X + m.b) * (m.c * P.X + m.d) + (m.a * m.c * P.Y * P.Y), divisor, (P.Y * (m.a * m.d - m.b * m.c)), divisor);
            }
            if (P.Y == 0 )
            {
                if (m.c != 0)
                {
                    if (P.X == -m.d / m.c)
                    {
                        return new Metria.Hyperbolic._2.Point(P.X, -1);
                    }
                    
                    float divisor = ((m.c * P.X + m.d) * (m.c * P.X + m.d)) + (m.c * m.c * P.Y * P.Y);
                    return new Metria.Hyperbolic._2.Point((m.a * P.X + m.b) * (m.c * P.X + m.d) + (m.a * m.c * P.Y * P.Y), divisor, (P.Y * (m.a * m.d - m.b * m.c)), divisor);
                    
                }
                
            }
            if (m.c != 0)
            {
                return new Metria.Hyperbolic._2.Point(m.a / m.c, 0);
            }
            return P;
        }

        static void Incrementa_lista()
        {
            KeyValuePair<Metria.Hyperbolic._2.Point, string> auxP;
            Queue<KeyValuePair<Metria.Hyperbolic._2.Point, string>> auxQ = new Queue<KeyValuePair<Metria.Hyperbolic._2.Point, string>>();
            Metria.Hyperbolic._2.Point novo;
            while (ProxPontos.Count > 0)
            {
                auxP = ProxPontos.Dequeue();
                foreach (KeyValuePair<char, Mobius> gerador in geradores)
                {
                    novo = Aplica_mobius(auxP.Key, gerador.Value);
                    bool existe = false;
                    foreach (var p in Pontos)
                    {
                        //if((p.Value.xup * novo.xdown == p.Value.xdown * novo.xup)&&(p.Value.yup * novo.ydown == p.Value.ydown * novo.yup))
                        if (p.Value == novo)
                        {
                            existe = true;
                        }
                    }
                    //if (Pontos.ContainsValue(novo))
                    if (existe)
                    {
                        //Console.WriteLine("Whoa Cowboy, thas not nice");
                    }
                    else
                    //if(!Pontos.ContainsValue(novo))
                    {
                        //Console.WriteLine("Adding a new point :D");
                        Pontos.Add(auxP.Value + gerador.Key, novo);
                        auxQ.Enqueue(new KeyValuePair<Metria.Hyperbolic._2.Point, string>(novo, auxP.Value + gerador.Key));
                    }
                }
            }
            ProxPontos = auxQ;
        }

        static void Incrementa_listaQDTP()
        {
            KeyValuePair<Metria.Hyperbolic._2.Point, string> auxP;
            Queue<KeyValuePair<Metria.Hyperbolic._2.Point, string>> auxQ = new Queue<KeyValuePair<Metria.Hyperbolic._2.Point, string>>();
            Metria.Hyperbolic._2.Point novo;
            while (ProxPontos.Count > 0)
            {
                auxP = ProxPontos.Dequeue();
                foreach (KeyValuePair<char, Mobius> gerador in geradores)
                {
                    novo = Aplica_mobius(auxP.Key, gerador.Value);
                    bool existe = false;
                    foreach (var p in Pontos)
                    {
                        if (p.Value == novo)
                        {
                            existe = true;
                        }
                    }
                    if (!existe)
                    {
                        Pontos.Add(auxP.Value + gerador.Key, novo);
                        auxQ.Enqueue(new KeyValuePair<Metria.Hyperbolic._2.Point, string>(novo, auxP.Value + gerador.Key));
                    }
                }
            }
            ProxPontos = auxQ;
        }


        public static System.Drawing.PointF Transforma(Metria.Hyperbolic._2.Point P)
        {
            System.Drawing.PointF retorno = new PointF()
            {
                X = _centro.X + (P.X * ppu),
                Y = _centro.Y - (P.Y * ppu)
            };
            return retorno;
        }
        public static void DesenhaPonto(Metria.Hyperbolic._2.Point P)
        {
            if (P != null)
            {
                PointF final = Transforma(P);
                _vincent.DrawEllipse(_bic, final.X - 2, final.Y - 2, 4, 4);
                //_vincent.DrawString(P.ToString(), new Font("Arial", 8),new SolidBrush(Color.Black), final.X,final.Y+4);
            }
        }
        public static void DesenhaPonto(Metria.Hyperbolic._2.Point P, Color C)
        {
            if (P != null)
            {
                PointF final = Transforma(P);
                _vincent.DrawEllipse(new Pen(C), final.X - 2, final.Y - 2, 4, 4);
                //_vincent.DrawString(P.ToString(), new Font("Arial", 12),new SolidBrush(Color.Black), final.X, final.Y - 4);
            }
        }
        public static void DesenhaReta(Metria.Hyperbolic._2.Line L)
        {
            if (L != null)
            {
                if (L is LineSegment) DesenhaSegmento(L as LineSegment);
                else if (L is Ray) DesenhaSemiReta(L as Ray);
                else
                {
                    DesenhaPonto(L.A);
                    DesenhaPonto(L.B);
                    if (L.Beta.Y == -1)
                    {
                        PointF referencia = Transforma(L.A);
                        _vincent.DrawLine(_bic, referencia.X, 0, referencia.X, 800);
                    }
                    else
                    {
                        PointF centro = Transforma(L.Center);
                        _vincent.DrawArc(_bic, new RectangleF(centro.X - L.Radius * ppu, centro.Y - L.Radius * ppu, L.Radius * ppu * 2, L.Radius * ppu * 2), 180, 180);
                    }
                }
            }
        }
        public static void DesenhaReta(Metria.Hyperbolic._2.Line L, Color C)
        {
            if (L != null)
            {
                if (L is LineSegment) DesenhaSegmento(L as LineSegment);
                else if (L is Ray) DesenhaSemiReta(L as Ray);
                else
                {
                    DesenhaPonto(L.A);
                    DesenhaPonto(L.B);
                    if (L.Beta.Y == -1)
                    {
                        PointF referencia = Transforma(L.A);
                        _vincent.DrawLine(_bic, referencia.X, 0, referencia.X, 800);
                    }
                    else
                    {
                        PointF centro = Transforma(L.Center);
                        _vincent.DrawArc(new Pen(C), new RectangleF(centro.X - L.Radius * ppu, centro.Y - L.Radius * ppu, L.Radius * ppu * 2, L.Radius * ppu * 2), 180, 180);
                    }
                }
            }
        }
        public static void DesenhaSemiReta(Metria.Hyperbolic._2.Ray R)
        {
            DesenhaPonto(R.A);
            DesenhaPonto(R.B);
            if (R.Beta.Y == -1)
           {
                PointF referencia = Transforma(R.A);
                if (R.B.Y == -1) _vincent.DrawLine(_bic, referencia, new PointF(referencia.X, 0));
                else _vincent.DrawLine(_bic, referencia, new PointF(referencia.X, _centro.Y));
            }
            else
            {
                PointF centro = Transforma(R.Center);
                float AnguloB = (R.B == R.Alfa) ? 180 : 360;
                float aux = R.Center.X - R.A.X;
                float cos = (float)System.Math.Abs(aux / R.Radius);
                float anguloINI = (float)System.Math.Acos(cos) * 57.2958f;
                anguloINI = (R.A.X > R.Center.X) ? 360 - anguloINI : 180 + anguloINI;


                _vincent.DrawArc(_bic, new RectangleF(centro.X - R.Radius * ppu, centro.Y - R.Radius * ppu, R.Radius * ppu * 2, R.Radius * ppu * 2), anguloINI, AnguloB - anguloINI);
            }
        }
        public static void DesenhaSegmento(Metria.Hyperbolic._2.LineSegment L)
        {
            DesenhaPonto(L.A);
            DesenhaPonto(L.B);
            //if (L.Beta.Y == -1)
            if (Math.Abs(L.A.X - L.B.X) < 0.001)
            {
                _vincent.DrawLine(_bic, Transforma(L.A), Transforma(L.B));
            }
            else
            {
                //Angulo incial
                float aux = L.Center.X - L.A.X;
                float cos = (float)System.Math.Abs(aux / L.Radius);
                float anguloINI = (float)System.Math.Acos(cos) * 57.2958f;
                anguloINI = (L.A.X > L.Center.X) ? 360 - anguloINI : 180 + anguloINI;

                //Angulo final
                aux = L.Center.X - L.B.X;
                cos = (float)System.Math.Abs(aux / L.Radius);
                float anguloFIM = (float)System.Math.Acos(cos) * 57.2958f;
                anguloFIM = (L.B.X > L.Center.X) ? 360 - anguloFIM - anguloINI : 180 + anguloFIM - anguloINI;

                //Inicical e varredura
                PointF A = Transforma(L.A), B = Transforma(L.B), centro = Transforma(L.Center);
                //Desenho
                _vincent.DrawArc(_bic, new RectangleF(centro.X - L.Radius * ppu, centro.Y - L.Radius * ppu, L.Radius * ppu * 2, L.Radius * ppu * 2), anguloINI, anguloFIM);
            }
        }
        public static void DesenhaLadrilho()
        {
            _vincent.Clear(Color.White);
            foreach (KeyValuePair<string, Metria.Hyperbolic._2.Point> par in Pontos)
            {
                DesenhaPonto(par.Value, Color.Green);

            }
            if (L != null)
            {
                DesenhaReta(L, Color.Red);
            }
            DesenhaPonto(C.Center, Color.Blue);
            foreach (Metria.Hyperbolic._2.Voronoi.VoronoiLine lado in C.Sides)
            {
                DesenhaReta(lado._line, Color.Red);
                try
                {
                    foreach (var par in Pontos)
                    {
                        Line Laux;
                        if (lado._line is LineSegment) Laux = new LineSegment(lado.A, lado.B);
                        else if (lado._line is Ray) Laux = new Ray(lado.A, lado.B);
                        else Laux = new Line(lado.A, lado.B);
                        if (par.Key != "")
                            AplicaString(Laux, par.Key);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (L != null) DesenhaPonto(L.Center);
        }

        public static void Desenha()
        {
            _vincent.Clear(Color.White);
            foreach (KeyValuePair<string, Metria.Hyperbolic._2.Point> par in Pontos)
            {
                DesenhaPonto(par.Value, Color.Green);

            }
            if (L != null)
            {
                DesenhaReta(L, Color.Red);
            }
            DesenhaPonto(C.Center, Color.Blue);
            foreach (Metria.Hyperbolic._2.Voronoi.VoronoiLine lado in C.Sides)
            {
                DesenhaReta(lado._line, Color.Red);
            }
            if (L != null) DesenhaPonto(L.Center);
        }


        public static void AplicaString(Line L, string s)
        {

            for(int i = 0; i < s.Length; i++)
            {
                L.A = Aplica_mobius(L.A, DicGeradores[s[i]]);
                L.B = Aplica_mobius(L.B, DicGeradores[s[i]]);
            }
            if (L is LineSegment) DesenhaSegmento(new LineSegment(L.A, L.B));
            else if (L is Ray) DesenhaSemiReta(new Ray(L.A, L.B));
            else DesenhaReta(new Line(L.A,L.B)) ;
        }
    }
}
