using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzard
{
    class Quizzard
    {
        private Queue<string> questions;
        public string currentQuestion;  //A current variable, affected by Dequeue()

        private Queue<string> correctAnswers;
        public string currentAnswer;    //A current variable, affected by Dequeue()

        private Queue<string> hints;
        public string currentHint;      //A current variable, affected by Dequeue()

        private Queue<List<string>> enumerationAnswers;
        public List<string> currentEnumerationAnswers;  //A current variable, affected by Dequeue()

        private Queue<List<string>> alternateAnswers;
        public List<string> currentAlternateAnswers;     //A current variable, affected by Dequeue()

        private Queue<List<string>> choices;
        public List<string> currentChoices;    //A current variable, affected by Dequeue()


        private string type;
        private int maxItems;
        private int itemsLeft;
        private int score;

        private bool questionOrdered;
        private bool caseSensitive;
        private bool hintEnabled;
        private bool alternateAnswersEnabled;
        private bool enumerationOrdered;
        private string username;

        public Quizzard()
        {
            type = "Quiz";
            questions = new Queue<string>();
            correctAnswers = new Queue<string>();
            hints = new Queue<string>();
            alternateAnswers = new Queue<List<string>>();
            choices = new Queue<List<string>>();
            maxItems = 0;
            itemsLeft = 0;
            score = 0;     
            questionOrdered = false;
            caseSensitive = false;
            hintEnabled = false; ;
            alternateAnswersEnabled = false;
            enumerationOrdered = false;
            username = "John_Doe";
        }

        public Quizzard(string quizID, string user)
        {
            type = XMLFileHandling.GetQuizType(quizID);
            if (type == "Identification")
            {
                Identification quiz = new Identification(quizID);
                if (!quiz.IsInOrder)
                    quiz.Shuffle();
                questions = new Queue<string>(quiz.Questions);
                correctAnswers = new Queue<string>(quiz.CorrectAnswers);
                hints = new Queue<string>(quiz.Hints);
                alternateAnswers = new Queue<List<string>>(quiz.AlternateAnswers);
                maxItems = quiz.ItemCount();
                itemsLeft = quiz.ItemCount();
                score = 0;
                questionOrdered = quiz.IsInOrder;
                caseSensitive = quiz.IsCaseSensitive;
                hintEnabled = quiz.IsHintEnabled;
                alternateAnswersEnabled = quiz.IsAlternateAnswersEnabled;
                username = user;
            }
            else if (type == "Multiple Choice")
            {
                MultipleChoice quiz = new MultipleChoice(quizID);
                if (!quiz.IsInOrder)
                    quiz.Shuffle();
                questions = new Queue<string>(quiz.Questions);
                correctAnswers = new Queue<string>(quiz.CorrectAnswers);
                choices = new Queue<List<string>>(quiz.Choices);
                maxItems = quiz.ItemCount();
                itemsLeft = quiz.ItemCount();
                score = 0;
                questionOrdered = quiz.IsInOrder;
                username = user;
            }
        }

        //public Quizzard(Enumeration quiz, string user){}

        public int Max => maxItems;
        public string Type => type;



        //This function dequeues one item from all the queues and assignes them to the "current" variables.
        public void Dequeue()
        {
            if (type == "Identification")
            {
                currentHint = hints.Dequeue();
                currentAlternateAnswers = alternateAnswers.Dequeue();
                currentAnswer = correctAnswers.Dequeue();
                currentQuestion = questions.Dequeue();
            }
            else if (type == "Multiple Choice")
            {
                currentAnswer = correctAnswers.Dequeue();
                currentChoices = choices.Dequeue();
                currentQuestion = questions.Dequeue();
            }
            else if (type == "Enumeration")
            {
                currentQuestion = questions.Dequeue();
            }
            else
                throw new Exception("Invalid quiz type encountered.");
        }

        //This function takes an answer as an argument and then compares it with the current answer.
        public void CheckAnswer(string answer)
        {
            if (type == "Identification")
            {
                if (answer == currentAnswer)
                    score++;
                else if (alternateAnswersEnabled)
                    foreach (string alternateAnswer in currentAlternateAnswers)
                        if (answer == alternateAnswer)
                        {
                            score++;
                            break;
                        }
            }
            else if (type == "Multiple Choice")
            {
                if (answer == currentAnswer)
                    score++;
            }
            else
                throw new Exception("Invalid quiz type encountered.");
        }

        //This function takes an array of answers as an argument and then compares it with the current enumeration answer.
        public void CheckAnswer(string[] answer)
        {
            if (type == "Enumeration")
            {
            }
            else
                throw new Exception("Invalid quiz type encountered.");
        }

        //This lambda expression returns a statement of the user's score.
        public string Result => username + ", your score is " + score.ToString() + "/" + maxItems.ToString();
    }
}
