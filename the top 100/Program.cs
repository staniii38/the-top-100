using System;

using System.Collections.Generic;

using System.IO;

using System.Linq;



namespace IndovinelliGame

{

    class Program

    {

        static string utentiFile = "utenti.txt";

        static string indovinelliFile = "indovinelli.txt";

        static Dictionary<string, int> scoreboard = new Dictionary<string, int>();

        static List<Indovinello> tuttiIndovinelli = new List<Indovinello>();

        static string nomeUtente = "";



        static void Main()

        {

            CaricaUtenti();

            CaricaIndovinelli();



            Console.Write("Inserisci il tuo nome utente: ");

            nomeUtente = Console.ReadLine().Trim();



            if (!scoreboard.ContainsKey(nomeUtente))

                scoreboard[nomeUtente] = 0;



            Menu();

        }



        static void Menu()

        {

            while (true)

            {

                Console.Clear();

                Console.WriteLine($"\nBenvenuto, {nomeUtente}!");

                Console.WriteLine("1 - Nuova partita");

                Console.WriteLine("2 - Scoreboard");

                Console.WriteLine("3 - Elimina dati");

                Console.WriteLine("4 - Esci");

                Console.Write("Scelta: ");



                switch (Console.ReadLine())

                {

                    case "1":

                        NuovaPartita();

                        break;

                    case "2":

                        MostraScoreboard();

                        break;

                    case "3":

                        EliminaDati();

                        break;

                    case "4":

                        SalvaUtenti();

                        return;

                    default:

                        Console.WriteLine("Scelta non valida. Premi un tasto...");

                        Console.ReadKey();

                        break;

                }

            }

        }



        static void CaricaUtenti()

        {

            if (!File.Exists(utentiFile)) return;



            foreach (var line in File.ReadAllLines(utentiFile))

            {

                var parti = line.Split('|');

                if (parti.Length == 2 && int.TryParse(parti[1], out int score))

                    scoreboard[parti[0]] = score;

            }

        }



        static void SalvaUtenti()

        {

            var righe = scoreboard.Select(u => $"{u.Key}|{u.Value}");

            File.WriteAllLines(utentiFile, righe);

        }



        static void CaricaIndovinelli()

        {

            if (!File.Exists(indovinelliFile))

            {

                Console.WriteLine("File indovinelli.txt mancante!");

                Environment.Exit(1);

            }



            foreach (var line in File.ReadAllLines(indovinelliFile))

            {

                var parti = line.Split('|');

                if (parti.Length == 3)

                    tuttiIndovinelli.Add(new Indovinello(parti[0], parti[1].ToLower(), parti[2]));

            }



            if (tuttiIndovinelli.Count < 100)

            {

                Console.WriteLine("Servono almeno 100 indovinelli.");

                Environment.Exit(1);

            }

        }



        static void MostraScoreboard()

        {

            Console.Clear();

            Console.WriteLine("=== SCOREBOARD ===");

            foreach (var u in scoreboard.OrderByDescending(u => u.Value))

                Console.WriteLine($"{u.Key}: {u.Value} punti");

            Console.WriteLine("Premi un tasto per tornare...");

            Console.ReadKey();

        }



        static void EliminaDati()

        {

            if (File.Exists(utentiFile))

                File.Delete(utentiFile);

            scoreboard.Clear();

            Console.WriteLine("Dati eliminati. Premi un tasto.");

            Console.ReadKey();

        }



        static void NuovaPartita()

        {

            Console.Clear();

            int punteggioPartita = 0;

            var rnd = new Random();

            var disponibili = tuttiIndovinelli.OrderBy(x => rnd.Next()).Take(100).ToList();



            foreach (var ind in disponibili)

            {

                int punti = 100;

                int tentativi = 0;

                bool corretto = false;



                while (punti > 0)

                {

                    Console.Clear();

                    Console.WriteLine($"Domanda: {ind.Domanda}");



                    if (tentativi == 2 || punti <= 50)

                        Console.WriteLine($"Indizio: {ind.Indizio}");



                    Console.Write("Risposta: ");

                    string risposta = Console.ReadLine().ToLower().Trim();



                    if (risposta == ind.Risposta)

                    {

                        Console.WriteLine($"✔️ Corretto! Hai guadagnato {punti} punti");

                        punteggioPartita += punti;

                        corretto = true;

                        break;

                    }

                    else

                    {

                        Console.WriteLine("❌ Sbagliato.");

                        tentativi++;

                        punti -= (punti > 50) ? 10 : 5;

                    }

                }



                if (!corretto)

                    Console.WriteLine($"❗ Risposta corretta: {ind.Risposta}");



                Console.WriteLine("Premi un tasto per continuare...");

                Console.ReadKey();

            }



            scoreboard[nomeUtente] += punteggioPartita;

            Console.Clear();

            Console.WriteLine($"🎉 Partita finita! Hai guadagnato {punteggioPartita} punti.");

            Console.WriteLine("Premi un tasto per tornare al menu.");

            Console.ReadKey();

        }

    }



    class Indovinello

    {

        public string Domanda { get; }

        public string Risposta { get; }

        public string Indizio { get; }



        public Indovinello(string domanda, string risposta, string indizio)

        {

            Domanda = domanda;

            Risposta = risposta;

            Indizio = indizio;

        }

    }

}
