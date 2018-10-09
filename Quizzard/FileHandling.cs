using System;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quizzard
{
    class XMLFileHandling
    {
        public XMLFileHandling() { }

        //This function loads the XML file
        //If the XML file is existing, append
        //If the XML doesn't exist, create a new file

        public void LoadXML()//filename sample: Quizzard.xml
        {
            if (!File.Exists(@"Quizzard.xml"))
            {
                XElement xElem;
                XDocument xDoc;

                xDoc = new XDocument();
                xElem = new XElement("Quizzard");
                //Add Room Element Name Quizzard
                xDoc.Add(xElem);
                xDoc.Save(@"Quizzard.xml");
            }
        }

        public void SaveToXML(XmlDocument xDocument)
        {
            //Save to XML File
            XmlTextWriter xTextWriter = new XmlTextWriter(Environment.CurrentDirectory + @"\Quizzard.xml", null);
            xTextWriter.Formatting = Formatting.Indented;

            xDocument.WriteContentTo(xTextWriter);
            xTextWriter.Close();
            Console.WriteLine("Successfully added xml data.");

            Console.ReadKey();
        }

        public void AddEnumerationElement(List<string> questions, List<List<string>> answers, string quizID, bool order)
        {
            LoadXML();

            XmlDocument xDoc = new XmlDocument();
            xDoc.RemoveAll();
            xDoc.Load(@"Quizzard.xml");

            //Append Child Quiz
            XmlElement xElementQuiz = xDoc.CreateElement("Quiz");
            xElementQuiz.SetAttribute("id", quizID);
            xElementQuiz.SetAttribute("type", "Enumeration");
            xElementQuiz.SetAttribute("order", order.ToString());

            int count = 0; // Counter to match the element number of the question
            foreach (string question in questions)
            {
                //Append Child Item
                XmlElement xElementItem = xDoc.CreateElement("Item");
                xElementQuiz.AppendChild(xElementItem);

                //Append Child Question
                XmlElement xElementQuestion = xDoc.CreateElement("Question");
                xElementQuestion.InnerXml = question;
                xElementItem.AppendChild(xElementQuestion);

                //Appending multiple answers
                foreach (string answer in answers[count])
                {
                    XmlElement xElementAnswers;
                    xElementAnswers = xDoc.CreateElement("Answer");
                    xElementAnswers.InnerXml = answer;
                    xElementItem.AppendChild(xElementAnswers);
                }
                //Append to XML
                xDoc.DocumentElement.AppendChild(xElementQuiz);
                count++;
            }

            //Save to XML File
            SaveToXML(xDoc);
        }

        public void AddIdentificationElement(List<string> questions, List<string> answers, List<string> hint,
                                             List<List<string>> altAnswers, string quizID, bool order, bool enAlt, bool enHint)
        {
            LoadXML();//Load XML
            XmlDocument xDoc = new XmlDocument();
            xDoc.RemoveAll();
            xDoc.Load(@"Quizzard.xml");

            XmlElement xElementQuiz = xDoc.CreateElement("Quiz");
            xElementQuiz.SetAttribute("id", quizID);
            xElementQuiz.SetAttribute("type", "Identification");
            xElementQuiz.SetAttribute("order", order.ToString());
            xElementQuiz.SetAttribute("enAlt", enAlt.ToString());
            xElementQuiz.SetAttribute("enHint", enHint.ToString());

            for (int counter = 0; counter < questions.Count; counter++)
            {
                //Append Child Item
                XmlElement xElementItem = xDoc.CreateElement("Item");
                xElementQuiz.AppendChild(xElementItem);

                //Append Child Question
                XmlElement xElementQuestion = xDoc.CreateElement("Question");
                xElementQuestion.InnerXml = questions[counter];
                xElementItem.AppendChild(xElementQuestion);

                //Append multiple answers
                XmlElement xElementAnswers;
                xElementAnswers = xDoc.CreateElement("Answer");
                xElementAnswers.InnerXml = answers[counter];
                xElementItem.AppendChild(xElementAnswers);

                //Append hint
                XmlElement xElementHint;
                xElementHint = xDoc.CreateElement("Hint");
                xElementHint.InnerXml = hint[counter];
                xElementItem.AppendChild(xElementHint);

                //Append Alternate Answers
                foreach (string answer in altAnswers[counter])
                {
                    XmlElement xElementAltAnswers;
                    xElementAltAnswers = xDoc.CreateElement("Alternate-Answer");
                    xElementAltAnswers.InnerXml = answer;
                    xElementItem.AppendChild(xElementAltAnswers);
                }
            }
            //Append to XML
            xDoc.DocumentElement.AppendChild(xElementQuiz);

            //Save to XML
            SaveToXML(xDoc);
        }

        public void AddMultipleChoiceElement(List<string> questions, List<string> answers, List<List<string>> choices,
                                        string quizID, bool order)
        {
            //Load XML
            LoadXML();
            XmlDocument xDoc = new XmlDocument();
            xDoc.RemoveAll();
            xDoc.Load(@"Quizzard.xml");

            //Append Child Quiz
            XmlElement xElementQuiz = xDoc.CreateElement("Quiz");
            xElementQuiz.SetAttribute("id", quizID);
            xElementQuiz.SetAttribute("type", "Multiple Choice");
            xElementQuiz.SetAttribute("order", order.ToString());

            int count = 0; // Counter to match the element number of the question
            foreach (string question in questions)
            {
                //Append Child Item
                XmlElement xElementItem = xDoc.CreateElement("Item");
                xElementQuiz.AppendChild(xElementItem);

                //Append Child Question
                XmlElement xElementQuestion = xDoc.CreateElement("Question");
                xElementQuestion.InnerXml = question;
                xElementItem.AppendChild(xElementQuestion);

                //Appending multiple answers

                XmlElement xElementAnswer = xDoc.CreateElement("Answer");
                xElementAnswer.InnerXml = answers[count];
                xElementItem.AppendChild(xElementAnswer);

                XmlElement xElementChoice;
                foreach (string choice in choices[count])
                {
                    xElementChoice = xDoc.CreateElement("Choice");
                    xElementChoice.InnerXml = choice;
                    xElementItem.AppendChild(xElementChoice);
                }
                count++;
            }
            //Append to XML
            xDoc.DocumentElement.AppendChild(xElementQuiz);

            //Save to XML File
            SaveToXML(xDoc);

        }

        //Gets the type of the quiz
        public static string GetQuizType(string quizID)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");
            string type = "";
            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  select el;

            foreach (XElement el in address.Nodes().Ancestors())
            {
                if ((string)el.Attribute("type") == "Identification")
                    type = "Identification";
                else if ((string)el.Attribute("type") == "Multiple Choice")
                    type = "Multiple Choice";
                else
                    type = "Enumeration";
                break;
            }
            return type;
        }

        //Gets the state of Enumeration if the quiz is in order or not
        public bool EnumerationQuizOrder(string quizID)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");
            bool state = false;
            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Enumeration"
                  select el;

            foreach (XElement el in address.Nodes().Ancestors())
            {
                if ((bool)el.Attribute("order") == true)
                    state = true;
                else
                    state = false;
                break;
            }
            return state;

        }

        //Gets the state of Identification if the quiz is in order or not
        public bool IdentificationQuizOrder(string quizID)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");
            bool state = false;
            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Identification"
                  select el;

            foreach (XElement el in address.Nodes().Ancestors())
            {
                if ((bool)el.Attribute("order") == true)
                    state = true;
                else
                    state = false;
                break;
            }
            return state;
        }

        //Gets the state of Multiple Choice if the quiz is in order or not
        public bool MultipleChoiceQuizOrder(string quizID)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");
            bool state = false;
            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Multiple Choice"
                  select el;

            foreach (XElement el in address.Nodes().Ancestors())
            {
                if ((bool)el.Attribute("order") == true)
                    state = true;
                else
                    state = false;
                break;
            }
            return state;

        }

        //Gets the state of Alternete Answer if it is enabled or not
        public bool IdentificationAlternateAnswerState(string quizID)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");
            bool state = false;
            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  select el;

            foreach (XElement el in address.Nodes().Ancestors())
            {
                if ((bool)el.Attribute("enAlt") == true)
                    state = true;
                else
                    state = false;
                break;
            }
            return state;

        }

        //Gets the state of Hint if it is enabled or not
        public bool IdentificationHintState(string quizID)
        {
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");
            bool state = false;
            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  select el;

            foreach (XElement el in address.Nodes().Ancestors())
            {
                if ((bool)el.Attribute("enHint") == true)
                    state = true;
                else
                    state = false;
                break;
            }
            return state;

        }

        //Returns a list of answers for multiple choice
        public List<string> getMultipleChoiceAnswers(string quizID)
        {
            List<string> answers = new List<string>();

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");

            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Multiple Choice"
                  select el;

            //Get the Element of Quiz
            foreach (XElement el in address.Descendants("Item").Elements())
                if (el.Name == "Answer")
                    answers.Add(el.Value);

            return answers;
        }

        //Returns a list of lists of choices for multiple choice
        public List<List<string>> getMultipleChoiceChoices(string quizID)
        {
            List<List<string>> inAnswers = new List<List<string>>();
            List<string> outAnswers = new List<string>();


            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");

            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Multiple Choice"
                  select el;

            //Get the Element of Quiz
            foreach (XElement el in address.Descendants("Item"))
            {
                //Get the element of Item
                foreach (XElement val in el.Elements())
                    if (val.Name == "Choice")
                        outAnswers.Add(val.Value);

                inAnswers.Add(outAnswers);
                outAnswers = new List<string>();
            }

            return inAnswers;
        }

        //Returns a list of lists of answers for enumeration
        public List<List<string>> getEnumerationAnswers(string quizID)
        {
            List<List<string>> inAnswers = new List<List<string>>();
            List<string> outAnswers = new List<string>();


            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");

            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Enumeration"
                  select el;

            //Get the Element of Quiz
            foreach (XElement el in address.Descendants("Item"))
            {
                //Get the element of Item
                foreach (XElement val in el.Elements())
                    if (val.Name == "Answer")
                        outAnswers.Add(val.Value);

                inAnswers.Add(outAnswers);
                outAnswers = new List<string>();
            }

            return inAnswers;
        }

        //Returns a list of lists of alternate answers for identification
        public List<List<string>> getIdentificationAlternateAnswers(string quizID)
        {
            List<List<string>> inAnswers = new List<List<string>>();
            List<string> outAnswers = new List<string>();


            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");

            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Identification"
                  select el;

            //Get the Element of Quiz
            foreach (XElement el in address.Descendants("Item"))
            {
                //Get the element of Item
                foreach (XElement val in el.Elements())
                    if (val.Name == "Alternate-Answer")
                        outAnswers.Add(val.Value);

                inAnswers.Add(outAnswers);
                outAnswers = new List<string>();
            }

            return inAnswers;
        }

        //Returns the list of answers for Identification having provided the quizID
        public List<string> getIdentifcationAnswers(string quizID)
        {
            List<string> answers = new List<string>();

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");

            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Identification"
                  select el;
            
            //Get the Element Item
            foreach (XElement el in address.Descendants("Item").Elements())
                if (el.Name == "Answer")
                   answers.Add(el.Value);        

            return answers;
        }

        //Returns the list of hints for Identification having provided the quizID
        public List<string> getIdentifcationHints(string quizID)
        {
            List<string> answers = new List<string>();

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");

            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == "Identification"
                  select el;

            //Get the Element Item
            foreach (XElement el in address.Descendants("Item").Elements())
                if (el.Name == "Hint")
                    answers.Add(el.Value);

            return answers;
        }

        // Return the questions with quizID and quizType 
        //QuizType: Multiple Choice, Identification, Enumeration
        public List<string> getQuestions(string quizID, string quizType)
        {
            List<string> questions = new List<string>();

            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");

            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  where (string)el.Attribute("type") == quizType
                  select el;

            foreach (XElement el in address.Descendants("Item").Elements())
                if (el.Name == "Question")
                    questions.Add(el.Value);


            return questions;
        }

        public bool QuizIDExists(string quizID)
        {
            bool state = false;
            XDocument xDoc = new XDocument();
            xDoc = XDocument.Load(@"Quizzard.xml");
            XElement root = XElement.Load(@"Quizzard.xml");

            IEnumerable<XElement> address =
                  from el in root.Elements("Quiz")
                  where (string)el.Attribute("id") == quizID
                  select el;

            if (address.Count() != 0)
                state = true;
            return state;
        }
       /*
        static void Main()
        {
            XMLFileHandling fh = new XMLFileHandling();
            ////Unit test for Enumeration
            //  List < List < string >> inAnswer = new List<List<string>>();
            //  List < string > outAnswer = new List<string>();
            //  List<string> question = new List<string>();
            //  question.Add("Sino ka?");
            //  question.Add("Ikaw ba?");

            //  outAnswer.Add("Dayne");
            //  outAnswer.Add("Dayne");
            //  inAnswer.Add(outAnswer);
            //  inAnswer.Add(outAnswer);
            //  fh.AddEnumerationElement(question, inAnswer, "214151", true);

           // Unit test for identification

           //List < string > hint = new List<string>();
           //List < string > question = new List<string>();
           // List<string> answer = new List<string>();
           // List < List < string >> altAnswer = new List<List<string>>();
           // List<string> alAnswer = new List<string>();

           // question.Add("Ano name mo?");
           // answer.Add("Dayne");
           // hint.Add("Ewan");
           // alAnswer.Add("Baby");
           // altAnswer.Add(alAnswer);
           // altAnswer.Add(alAnswer);

           // question.Add("Sila ba");
           // answer.Add("Hindi");
           // hint.Add("Nosi");
           // alAnswer.Add("Baby");
           // altAnswer.Add(alAnswer);
           // altAnswer.Add(alAnswer);
           // fh.AddIdentificationElement(question, answer, hint, altAnswer, "214151", true, false);

            //Unit Test for multiple choice
            //List<List<string>> Inchoice = new List<List<string>>();
            //List<string> outchoice = new List<string>();
            //List<string> answers = new List<string>();
            //List<string> questions = new List<string>();
            //questions.Add("Sino ka?");


            //outchoice.Add("Octopus");
            //outchoice.Add("Squid");
            //outchoice.Add("Shrimp");

            //answers.Add("Octopus");
            //Inchoice.Add(outchoice);

            //fh.AddMultipleChoiceElement(questions, answers, Inchoice, "214151", true);

            //fh.getIdentificationQuestion("214151");
            //fh.getQuestions("214151", "Enumeration");
            //fh.QuizIDExists("214151");
        }*/

    }
}




     