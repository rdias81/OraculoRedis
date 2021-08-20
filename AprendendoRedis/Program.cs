using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace AprendendoRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            Oraculo();
        }

        private static void Oraculo()
        {
            string conn = "40.65.216.220";
            var redis = ConnectionMultiplexer.Connect(conn);
            var db = redis.GetDatabase();

            var sub = redis.GetSubscriber();
            sub.Subscribe("perguntas").OnMessage(
            m => {
                if (m.Message.ToString().IndexOf(":") > 0)
                {
                    var perguntaId = m.Message.ToString().Substring(1, m.Message.ToString().IndexOf(":") - 1);
                    var resposta = Convert.ToInt32(perguntaId) * 2;
                    var respFinal = $"P{perguntaId} Equipe5 {resposta}";
                    db.HashSet($"P{perguntaId}", "Equipe5", resposta);
                }
            });

            Console.ReadKey();
        }

        private static void Publisher(string canal, string mensagem)
        {
            string conn = "40.65.216.220";
            var redis = ConnectionMultiplexer.Connect(conn);
            
            var sub = redis.GetSubscriber();
            sub.Publish(canal, mensagem);
        }

        private static void Subscriber(string canal)
        {
            string conn = "40.65.216.220";
            var redis = ConnectionMultiplexer.Connect(conn);
            
            var sub = redis.GetSubscriber();
            sub.Subscribe(canal).OnMessage(
            m => {
                Console.WriteLine(m.Message);
            });
            Console.ReadLine();
        }

        private static void PrimeiroExercicio()
        {
            Console.WriteLine("Hello REDIS!");

            string conn = "localhost";
            var redir = ConnectionMultiplexer.Connect(conn);
            var db = redir.GetDatabase();

            db.StringSet("A", 1);
            Console.WriteLine(db.StringGet("A"));

            db.StringIncrement("A", 2);
            Console.WriteLine(db.StringGet("A"));

            db.SetAdd("tech", "SQL");


            db.ListLeftPush("L01", "A");
            db.ListLeftPush("L01", "B");
            Console.WriteLine(db.ListLeftPop("L01"));
            Console.WriteLine(db.ListLeftPop("L01"));

            Console.ReadKey();
        }
    }
}
