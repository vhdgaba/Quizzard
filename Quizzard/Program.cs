using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzard
{
    class Program
    {
        static void Main(string[] args)
        {
            string username;
            string result = "";

            Console.Write("Enter a username: ");
            username = Console.ReadLine();
            Console.Write("Enter quiz name to load: ");
            Quizzard test = new Quizzard(Console.ReadLine(), username);

            if (test.Type == "Identification")
            {
                string answer;
                for (int i = 0; i < test.Max; i++)
                {
                    test.Dequeue();
                    Console.WriteLine(test.currentQuestion);        //test.Question() dequeues values from all the queues, stores the values in their current variables and returns the question.
                    Console.Write("Enter y to enable hint: ");  
                    if (Console.ReadLine().ToLower() == "y")
                        Console.WriteLine(test.currentHint);   //test.currentHint, as well as other current variables, is automatically updated when test.Question() is called
                    Console.Write("Answer: ");

                    answer = Console.ReadLine();
                    test.CheckAnswer(answer);
                }
                result = test.Result;
            }



            else if (test.Type == "Multiple Choice")
            {
                string answer;
                for (int i = 0; i < test.Max; i++)
                {
                    test.Dequeue();
                    Console.WriteLine(test.currentQuestion);         //test.Question() dequeues values from all the queues, stores the values in their current variables and returns the question.

                    foreach (string choice in test.currentChoices) //test.currentChoices, as well as other current variables, is automatically updated when test.Question() is called
                        Console.WriteLine(choice);

                    Console.Write("Answer: ");
                    answer = Console.ReadLine();
                    test.CheckAnswer(answer);
                }
                result = test.Result;
            }
            else
                Console.WriteLine(test.Type);

            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
