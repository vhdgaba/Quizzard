using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzard
{
    class Enumeration : Quiz
    {
        //Private data
        private bool isEnumerationOrdered;
        private List<List<string>> correctAnswersEnum;
        
        //PROPERTIES
        public bool IsEnumerationOrdered { get => isEnumerationOrdered; set => isEnumerationOrdered = value; }
        public List<List<string>> CorrectAnswersEnum { get => correctAnswersEnum; set => correctAnswersEnum = value; }

        //Default constructor
        public Enumeration() : base()
        {
            CorrectAnswersEnum = new List<List<string>>();
            Type = "Enumeration";
        }

        //Main constructor to be used
        public Enumeration(string ID) : base(ID)
        {
            Type = "Enumeration";
            Load(ID);
        }

        public override void Load(string quizID)
        {
            base.Load(quizID);
            XMLFileHandling fh = new XMLFileHandling();
            Questions = fh.getQuestions(quizID, "Enumeration");
            CorrectAnswers = fh.getIdentifcationAnswers(quizID);
            IsInOrder = fh.IdentificationQuizOrder(quizID);
            //IsEnumerationOrdered = ;
        }

        public override void Save()
        {
            base.Save();
            XMLFileHandling fh = new XMLFileHandling();
            fh.AddEnumerationElement(Questions, CorrectAnswersEnum, QuizID, IsInOrder);
        }

        //Adds an identification item with hint
        public void IdentificationAddItem(string question, string[] correctAnswers)
        {
            List<string> correct = new List<string>();
            base.AddItem(question);
            foreach (string answer in correctAnswers)
            {
                correct.Add(answer);
            }
        }

        //Removes an identification item
        public void IdentificationRemoveItem(string question)
        {
            if (Questions.Contains(question))
            {
                int index = Questions.IndexOf(question);
                base.RemoveItem(question);
                CorrectAnswersEnum.RemoveAt(index);
            }
        }

        public void Shuffle()
        {
            Random rand = new Random();
            string temp;
            List<string> tempEnumAns;
            int max = Questions.Count;
            int x, y;
            for (int i = 0; i < max * 5; i++)
            {
                x = rand.Next(max);
                y = rand.Next(max);

                temp = Questions[x];
                Questions[x] = Questions[y];
                Questions[y] = temp;

                tempEnumAns = CorrectAnswersEnum[x];
                CorrectAnswersEnum[x] = CorrectAnswersEnum[y];
                CorrectAnswersEnum[y] = tempEnumAns;
                
            }
        }
    }
}
