using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ScoringLogic
{
    public partial class Form1
    {
        int currentQuestionScore = -99;
        public void addCheckAnswer(int questionNumber, int wagerNumber, int questionsNumber, int playerNumber)
        {
            if(unknowns == null) unknowns = new List<unknownSet>();
            unknowns.Add(new unknownSet(questionNumber, wagerNumber, questionsNumber, playerNumber));
        }


        public List<string> playerList;

        public List<List<int>> playersUsedUniques;

        //Assumes the cleanInputScoresheet method has been ran
        public void populatePlateList()
        {
            playerList = new List<string>();
            for(int i = 0; i < numOfPlayers; i++) 
            {
                playerList.Add(cleanedScoresheetInputSheet.lines[1 + i][0].ToString());
            }

        }

        public struct playerAnswer
        {
            public string answers;
            public int playerId;

            public int Compare(playerAnswer other)
            {
                return this.playerId.CompareTo(other.playerId); 
            }
        }

        public enum questionType
        {
            Wager, 
            TextAnswer,
            Essay,
            JeopardyWager,
            MystCat
        }

        public struct question
        {
            public string questionID;
            public int questionNumber;
            public questionType questionType;
            public List<string> correctAnswers;
            public List<string> acceptableAnswers;
            public List<string> deniedAnswers;

            public List<playerAnswer> playerAnswers;

        }

        public struct questionSet
        {
            public int questionNumber;
            public List<string> questionIds;
            public List<question> wagers;
            public List<question> questions;
        }

        public struct unknownSet
        {
            public int questionNumber;
            public int wagerNumber;
            public int questionsNumber;
            public int playerNumber;
            public unknownSet(int qN, int wN, int qSN, int pN)
            {
                questionNumber = qN;
                wagerNumber = wN;
                questionsNumber = qSN;
                playerNumber = pN;
            }
        }

        public List<unknownSet> unknowns;
        public List<string> extractAcceptableAnswers(string answers)
        {
            List<string> answerList = new List<string>();
            string curAnswer = "";
            int state = 0;
            for(int i = 0; i < answers.Length; i++)
            {
                if (answers[i] == '[' && state == 0)
                {
                    curAnswer = "";
                    state = 1;
                }
                else if (answers[i] == ']' && state == 1 && answers[i-1] != '\\')
                {
                    answerList.Add(curAnswer);
                    state = 0;
                }
                else
                {
                    curAnswer += answers[i];
                }
            }
            return answerList;
        }

        //Specifically written to extract question number from unique wager lines
        public int extractWagerQuestionNumber(string text)
        {
            text = text.Substring(text.IndexOf(" ") + 1, text.Length - text.IndexOf(" ") -1);
            text = text.Substring(text.IndexOf(" ") + 1, text.Length - text.IndexOf(" ") -1);
            text = text.Substring(0,text.IndexOf(":"));
            return Int32.Parse(text);
        }

        //Specifically written to extract question number from free text answer lines
        public int extractTextQuestionNumber(string text)
        {
            text = text.Substring(text.IndexOf(" ") + 1, text.Length - text.IndexOf(" ") - 1);
            text = text.Substring(0,text.IndexOf(":"));
            return Int32.Parse(text);
        }

        //checks if a given question id is in the set, if not it adds a blank
        void checkQuestionSet(int questionNumber)
        {
            bool found = false;
            for(int i = 0; i < questionList.Count; i++) 
            {
                if (questionList[i].questionNumber == questionNumber)
                found = true;
            }
            if (!found)
            {
                questionSet temp = new questionSet();
                temp.questionIds = new List<string>();
                temp.questionNumber = questionNumber;
                temp.questions = new List<question>();
                temp.wagers = new List<question>();
                questionList.Add(temp);
            }
        }

        public int numOfPlayers = 0;
        public List<questionSet> questionList;
       
        //Assumes that the function CleanInputScoresheet has been ran. 
        public void populateQuestionList()
        {
            questionList = new List<questionSet>();
            //Each question has format (Q#, Question Syntax, Question Type, Answers (if applicable), Score value)
            int curLine = 1+numOfPlayers;
            //For each question
            //Extract the needed info from each cell
            while (curLine < cleanedScoresheetInputSheet.lines.Count)
            {
                //Make a blank variable
                question tempQuestion = new question();
                //Get the question id
                tempQuestion.questionID = cleanedScoresheetInputSheet[curLine][0].ToString();
                //Figure out what type of question it is
                string questionText = cleanedScoresheetInputSheet[(curLine + 2)][1].ToString();
                if (questionText.Contains("Fixed Points"))
                {
                    tempQuestion.questionType = questionType.Essay;
                    if (questionText.Contains("8")) tempQuestion.questionNumber = 8;
                    else tempQuestion.questionNumber = 4;

                }

                else if (questionText.Contains("Unique Wager"))
                { //This is a wager question
                    tempQuestion.questionType = questionType.Wager;
                    //Extract question number
                    tempQuestion.questionNumber = extractWagerQuestionNumber(questionText);
                    //Extract acceptable answers

                }

                else if (questionText.Contains("Jeopardy Wager"))
                { //This is a jeopardy wager question
                    tempQuestion.questionType = questionType.JeopardyWager;
                    //-1 question number denotes final question
                    tempQuestion.questionNumber = -1;
                }

                else if (questionText.Contains("Mystery Category"))
                { //This is a mystery question
                    tempQuestion.questionType = questionType.MystCat;
                    //-1 question number denotes final question
                    tempQuestion.questionNumber = -1;
                }

                else 
                { //This is a text answer question, only type left
                    tempQuestion.questionType = questionType.TextAnswer;
                    //Extract correct answers
                    tempQuestion.correctAnswers = extractAcceptableAnswers(cleanedScoresheetInputSheet[curLine + 3][1].ToString());
                    tempQuestion.acceptableAnswers = new List<string>();
                    //Extract question number
                    tempQuestion.questionNumber = extractTextQuestionNumber(cleanedScoresheetInputSheet[curLine + 2][1].ToString());
                }
                tempQuestion.playerAnswers = new List<playerAnswer>();
                //Add to the appropriate question set
                checkQuestionSet(tempQuestion.questionNumber);
                for(int i = 0; i < questionList.Count; i++) 
                {
                    if (questionList[i].questionNumber == tempQuestion.questionNumber)
                    {
                        questionList[i].questionIds.Add(tempQuestion.questionID);
                        if(tempQuestion.questionType == questionType.Wager || tempQuestion.questionType == questionType.JeopardyWager)
                        {
                            questionList[i].wagers.Add(tempQuestion);
                            
                        }
                        else
                        {
                            questionList[i].questions.Add(tempQuestion);
                            
                        }
                    }
                }

                //Advance to the next question
                do { curLine++; } while (curLine < cleanedScoresheetInputSheet.lines.Count && (cleanedScoresheetInputSheet[curLine].cells.Count > 1 && (cleanedScoresheetInputSheet[curLine][1].ToString() != "")));
            }
        }
        //Populates all the questions with given answers
        public void populateQuestionAnswerList() 
        {
            //iterate accross question ids (q1, q2, q3)
            for (int j = 2; j < cleanedScoresheetInputSheet[0].cells.Count; j++)
            {
                string questionId = cleanedScoresheetInputSheet[0][j].ToString();
                //Find the correct question among the question list
                //Iterate through each questionset in the question list
                bool found= false;
                int questionIdListX = 0;//Which questionset in the list
                int questionIdListY = 0;//Wager or question
                int questionIdListZ = 0;//Posisition in list
                for (int k = 0; k < questionList.Count; k++)
                {
                    //Iterate through each wager to check for question id
                    for (int l = 0; l < questionList[k].wagers.Count; l++)
                    {
                        if (questionId == questionList[k].wagers[l].questionID)
                        {
                            questionIdListX = k;
                            questionIdListY = 0;
                            questionIdListZ = l;
                            found = true;
                            break;
                        }
                    }
                    //Iterate through each question to check for question id
                    for (int l = 0; l < questionList[k].questions.Count; l++)
                    {
                        if (questionId == questionList[k].questions[l].questionID)
                        {
                            questionIdListX = k;
                            questionIdListY = 1;
                            questionIdListZ = l;
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }

                //Iterate down each question id (player1, player2, player3)
                for (int i = 1; i < numOfPlayers + 1; i++)
                {
                    //If the current person answer, include their answer (playerid = i-1)
                    string curAnswer = cleanedScoresheetInputSheet[i][j].ToString();
                    if (curAnswer != "")
                    {
                        playerAnswer tempAnswer;
                        tempAnswer.playerId = i - 1;
                        tempAnswer.answers = curAnswer;
                        if(questionIdListY == 0)
                        {
                            questionList[questionIdListX].wagers[questionIdListZ].playerAnswers.Add(tempAnswer);
                        }
                        else
                        {
                            questionList[questionIdListX].questions[questionIdListZ].playerAnswers.Add(tempAnswer);
                        }
                    }
                }
            }
            
        }

        private void removeColumn(csvLibrary.csvSheet sheet, int col)
        {
            for (int i = 0; i < sheet.lines.Count; i++)
            {
                if (sheet.lines[i].cells.Count > col) sheet.lines[i].cells.RemoveAt(col);
            }
        }

        private void removeColumnRange(csvLibrary.csvSheet sheet, int index, int count)
        {
            for(int i = index; i < (index+count); i++) removeColumn(sheet, index); 
        }

        //This is very hacky, and probably won't work beyond the first game
        public void CleanInputScoresheet()
        {
            cleanedScoresheetInputSheet = scoresheetInputSheet;
            //Remove up until the question lines
            cleanedScoresheetInputSheet.lines.RemoveRange(0, 10);
            //Remove spaces between lines and names
            cleanedScoresheetInputSheet.lines.RemoveRange(1, 2);
            //Count names
            numOfPlayers = 0;
            while (cleanedScoresheetInputSheet[numOfPlayers+1][0].ToString() != "") numOfPlayers++;
            //Remove spaces between each set of questions (done by removed all remaining empty lines)
            for (int i = 0; i < cleanedScoresheetInputSheet.lines.Count; i++)
            {
                if (cleanedScoresheetInputSheet[i].isEmpty())
                {
                    cleanedScoresheetInputSheet.lines.RemoveAt(i);
                    i--;
                }
            }
            //remove from end of names to questions
            cleanedScoresheetInputSheet.lines.RemoveRange((1 + numOfPlayers), 2);

            
            //remove misc collum informations
            removeColumnRange(cleanedScoresheetInputSheet, 2, 19);
        }
        
        //Handles the list of question ones
        public void handleUnknowns()
        {
            if (!unknowns.Any())
            {
                outputSheet();
                questionCheckLabel.Text = "No more questions to check!";
                answerCheckLabel.Text = "";
            }
                
            else
            {
                handleUnknown();
            }
        }
        //Handles each instance of question ones
        public void handleUnknown()
        {
            //Get unknown question
            //Get unknown answer
            //Display info
             questionCheckLabel.Text = questionList[unknowns[0].questionNumber].questions[0].questionNumber.ToString();
             answerCheckLabel.Text = questionList[unknowns[0].questionNumber].questions[0].playerAnswers[unknowns[0].playerNumber].answers;
        }

        //After each unknown, check if the others share an answer, if yes push them to the front of the queue and auto approve
        public void recheckUnknowns(string newAnswer, bool approved, int partialScore)
        {
            for(int i = 0; i < unknowns.Count; i++)
            {
                if (questionList[unknowns[i].questionNumber].questions[unknowns[i].questionsNumber].playerAnswers[unknowns[i].playerNumber].ToString().ToLower() == newAnswer.ToLower())
                {
                    unknownSet temp = unknowns[i];
                    unknowns[i] = unknowns[0];
                    unknowns[0] = temp;
                    //Push to front
                    if (approved) approveUnknown(false);
                    else if (partialScore != -99) partialUnknown(false, partialScore);
                    else disaproveUnknown(false);
                    i--;
                }
            }
        }

        public void unknownsFinished()
        {
            questionCheckLabel.Text = "No more questions to check!";
            answerCheckLabel.Text = "";
        }

        public void approveUnknown(bool newWord)
        {
            if (unknowns.Count == 0) return;
            //give points
            int prevScore = 0;
            //if (outputSavesheet[questionList[unknowns[0].questionNumber].questions[unknowns[0].questionsNumber].playerAnswers[unknowns[0].playerNumber+1].playerId + 1][unknowns[0].questionNumber] != null) prevScore = Int32.Parse(outputSavesheet[questionList[unknowns[0].questionNumber].questions[unknowns[0].questionsNumber].playerAnswers[unknowns[0].playerNumber].playerId + 1][unknowns[0].questionNumber+1].ToString());
            outputSavesheet[unknowns[0].playerNumber+1][unknowns[0].questionNumber+1] = new csvLibrary.csvSheetCell((prevScore + unknowns[0].wagerNumber).ToString());
            //Add to list of approved answers 
            //if (outputSavesheet[unknowns[0].questionNumber + 1 + numOfPlayers][1] == null) outputSavesheet[unknowns[0].questionNumber + 1 + numOfPlayers][1] = new csvLibrary.csvSheetCell("");
            string curAnswer = questionList[unknowns[0].questionNumber].questions[unknowns[0].questionsNumber].playerAnswers[unknowns[0].playerNumber].answers;
            if (newWord)
            {
                string existingAnswers = outputSavesheet[unknowns[0].questionNumber + 3 + numOfPlayers][1].ToString();
                existingAnswers += ". " + curAnswer;
                outputSavesheet[unknowns[0].questionNumber + 3 + numOfPlayers][1].set(existingAnswers);
            }
            //remove front of list
            unknowns.RemoveAt(0);
            //Recheck all unknowns
            if(newWord) recheckUnknowns(curAnswer, true, -99);
            //call handle Unknowns
            handleUnknowns();
        }

        public void disaproveUnknown(bool newWord)
        {

            if (unknowns.Count == 0) return;

            //Add to list of disaproved words
            string curAnswer = questionList[unknowns[0].questionNumber].questions[unknowns[0].questionsNumber].playerAnswers[unknowns[0].playerNumber].answers;
            if (newWord)
            {
                string existingAnswers = "";
                if (outputSavesheet[unknowns[0].questionNumber + 3 + numOfPlayers][2] != null) existingAnswers = outputSavesheet[unknowns[0].questionNumber + 3 + numOfPlayers][2].ToString();

                existingAnswers += ". " + curAnswer;
                outputSavesheet[unknowns[0].questionNumber + 3 + numOfPlayers][2] = new csvLibrary.csvSheetCell(existingAnswers);
            }
            //Check if mystery word
            int score = 0;
            if (unknowns[0].questionNumber == 11)//If yes then remove points
            {
                score = -unknowns[0].wagerNumber;
            }//else put 0 (init to 0)
            outputSavesheet[unknowns[0].playerNumber + 1][unknowns[0].questionNumber + 1] = new csvLibrary.csvSheetCell((score).ToString());
            //remove front of list
            unknowns.RemoveAt(0);
            //Recheck all unknowns
            if(newWord) recheckUnknowns(curAnswer, false, -99);
            //call handle Unknowns
            handleUnknowns();

        }

        public void partialUnknown(bool newWord, int score)
        {
            //give points
            if (unknowns.Count == 0) return;
            int prevScore = 0;
            if (outputSavesheet[unknowns[0].playerNumber + 1][unknowns[0].questionNumber + 1] != null && outputSavesheet[unknowns[0].playerNumber + 1][unknowns[0].questionNumber + 1].ToString() != "") prevScore = Int32.Parse(outputSavesheet[unknowns[0].playerNumber + 1][unknowns[0].questionNumber + 1].ToString());
            outputSavesheet[unknowns[0].playerNumber + 1][unknowns[0].questionNumber + 1] = new csvLibrary.csvSheetCell((prevScore + score).ToString());
            //Add to list of approved answers 
            string curAnswer = questionList[unknowns[0].questionNumber].questions[unknowns[0].questionsNumber].playerAnswers[unknowns[0].playerNumber].answers;
            if (newWord)
            {
                string existingAnswers = "";
                if (outputSavesheet[unknowns[0].questionNumber + 1 + numOfPlayers][1] != null) outputSavesheet[unknowns[0].questionNumber + 1 + numOfPlayers][1].ToString();
                existingAnswers += ", " + curAnswer;
                outputSavesheet[unknowns[0].questionNumber + 3 + numOfPlayers][1] = new csvLibrary.csvSheetCell(existingAnswers);
            }
            
            
           
            //remove front of list
            unknowns.RemoveAt(0);
            //Recheck all unknowns
            if (newWord) recheckUnknowns(curAnswer, false, score);
            //call handle Unknowns
            handleUnknowns();
        }

        public void outputSheet()
        {
            System.IO.FileStream outputFileReader = (System.IO.FileStream)SavesheetOutput.OpenFile();
            outputSavesheet.exportSheet(outputFileReader);
            outputFileReader.Close();
        }



        public void generateScoresheet()
        {
            //Generate new sheet
            csvLibrary.csvSheet outputSheet = new csvLibrary.csvSheet();

            //Clean scoresheet
            cleanedScoresheetInputSheet = scoresheetInputSheet;

            CleanInputScoresheet();

            //Generate our question list
            populateQuestionList();

            //Populate our answer list
            populateQuestionAnswerList();

            //Init the player used uniques list
            playersUsedUniques = new List<List<int>>();
            for(int i = 0; i < numOfPlayers; i++)
            {
                playersUsedUniques.Add(new List<int>());
            }
            
            //If there is a previous sheet load it into the new sheet
            
            //Generate the formatting for the new sheet
            //Create top row ( ,Q1, Q2, Q3, Q4, Q5)
            outputSheet.lines = new List<csvLibrary.csvSheetLine>();
            outputSheet.lines.Add(new csvLibrary.csvSheetLine());
            outputSheet[0].cells.Add(new csvLibrary.csvSheetCell(""));
            for (int i = 1; i < questionList.Count+1; i++)
            {
                outputSheet[0].cells.Add(new csvLibrary.csvSheetCell("Q"+i.ToString()));
            }
            outputSheet[0].cells.Add(new csvLibrary.csvSheetCell("Total"));
            //Add each players row
            for(int i = 0;i<numOfPlayers;i++) 
            {
                outputSheet.lines.Add(new csvLibrary.csvSheetLine());
                outputSheet[i+1].cells.Add(cleanedScoresheetInputSheet[i + 1][0]);
                for(int j = 0; j < questionList.Count; j++) 
                {
                    outputSheet[i + 1].cells.Add(new csvLibrary.csvSheetCell(""));
                }
                char lastChar = (char)('B' + questionList.Count-1);
                outputSheet[i + 1].cells.Add(new csvLibrary.csvSheetCell("=SUM(B" + (2+i).ToString() + ":" + lastChar + (2 + i).ToString() + ")"));
            }
            //spacing row for the answers
            outputSheet.lines.Add(new csvLibrary.csvSheetLine());
            outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell(""));
            outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell(""));
            outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell(""));
            //Top of alt answers 
            outputSheet.lines.Add(new csvLibrary.csvSheetLine());
            outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell(""));
            outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell("Alt answers"));
            outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell("Denied answers"));
            //List all questions
            for (int i = 0; i < questionList.Count; i++)
            {
                outputSheet.lines.Add(new csvLibrary.csvSheetLine());
                outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell("Q" + i.ToString()));
                outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell(""));
                outputSheet[outputSheet.lines.Count - 1].cells.Add(new csvLibrary.csvSheetCell(""));
            }
            
            //Load in values from previous scoresheet if they exist
            if(savesheetInputSheet != null) 
            {
                for (int k = 1; k < savesheetInputSheet[0].cells.Count; k++)
                {
                    //Load in previous scores
                    if (savesheetInputSheet[1].cells[k] != null && savesheetInputSheet[1].cells[k].ToString().Length > 0 && !savesheetInputSheet[1].cells[k].ToString().Contains("="))
                    for (int i = 1; i < numOfPlayers+1; i++)
                    {
                            outputSheet[i][k] = savesheetInputSheet[i][k];
                    }
                }
                
                //Load in previous answers
                for (int k = 2+numOfPlayers; k < savesheetInputSheet.lines.Count; k++)
                {
                    if (savesheetInputSheet[k][1] != null) outputSheet[k][1] = savesheetInputSheet[k][1];
                    if (savesheetInputSheet[k][2] != null) outputSheet[k][2] = savesheetInputSheet[k][2];
                }
            }


            // outputSheet[questionList[i].questions[0].playerAnswers[j].playerId + 1][questionList[i].questionNumber]
            //Calculate score for new questions. 
            for (int i = 0; i < questionList.Count; i++) //For each question
            {
                if (outputSheet[1].cells.Count > i+1 && outputSheet[questionList[i].questions[0].playerAnswers[0].playerId + 1][questionList[i].questionNumber].ToString() != "") continue;
                //Get type of question
                if (questionList[i].questions[0].questionType == questionType.TextAnswer)
                {
                    //Sort the player answer lists to be equal
                    questionList[i].questions[0].playerAnswers.Sort(delegate (playerAnswer x, playerAnswer y)
                    {
                        if (x.playerId > y.playerId) return 1;
                        if (x.playerId < y.playerId) return -1;
                        return x.playerId.CompareTo(y.playerId);

                    });
                    questionList[i].wagers[0].playerAnswers.Sort(delegate (playerAnswer x, playerAnswer y)
                    {
                        if (x.playerId > y.playerId) return 1;
                        if (x.playerId < y.playerId) return -1;
                        return x.playerId.CompareTo(y.playerId);

                    });
                    //For each person
                    //We make the assumption that each person answered both the wager and the question
                    for (int j = 0; j < questionList[i].wagers[0].playerAnswers.Count; j++)
                    {
                        //Get wagers
                        int wager = 0;
                        try
                        {
                            wager = Int32.Parse(questionList[i].wagers[0].playerAnswers[j].answers.ToString());
                        }
                        catch 
                        {
                            wager = 0;
                        }
                        
                        //Check wagers for dupes (if needed)
                        if (playersUsedUniques[questionList[i].wagers[0].playerAnswers[j].playerId].Contains(wager))
                        {
                            wager = 1;
                        }
                        else
                        {
                            playersUsedUniques[questionList[i].wagers[0].playerAnswers[j].playerId].Add(wager);
                        }
                        //Check question
                        //Generate answer list
                        List<string> answers = new List<string>();
                        if (questionList[i].questions[0 ].correctAnswers != null)  answers.AddRange(questionList[i].questions[0].correctAnswers);
                        if (questionList[i].questions[0].acceptableAnswers != null) answers.AddRange(questionList[i].questions[0].acceptableAnswers);
                        bool correctAnswer = false;
                        for(int k = 0; k < answers.Count; k++)
                        {
                            if (questionList[i].questions[0].playerAnswers[j].answers.ToLower() == answers[k].ToLower())
                            {
                                correctAnswer = true;
                                //Correct
                            }
                        }
                        if(correctAnswer)
                        {
                            //Cell at location gets wager
                            outputSheet[questionList[i].questions[0].playerAnswers[j].playerId + 1][questionList[i].questionNumber].set( wager.ToString());
                        }
                        else
                        {
                            addCheckAnswer(i, wager, 0, j);
                        }
                    }
                }

                //Get type of question
                else if (questionList[i].questions[0].questionType == questionType.Essay)
                {
                    List<int> playerScores = new List<int>();
                    for (int k = 0; k < numOfPlayers; k++) playerScores.Add(0);
                    for(int l = 0; l < questionList[i].questions.Count; l++)
                    {
                        //Sort the player answer lists to be equal
                        questionList[i].questions[l].playerAnswers.Sort(delegate (playerAnswer x, playerAnswer y)
                        {
                            if (x.playerId > y.playerId) return 1;
                            if (x.playerId < y.playerId) return -1;
                            return x.playerId.CompareTo(y.playerId);

                        });
                        //For each person
                        //We make the assumption that each person answered both the wager and the question
                        
                        for (int j = 0; j < questionList[i].questions[l].playerAnswers.Count; j++)
                        {

                            //Check question

                            //Generate answer list
                            List<string> answers = new List<string>();
                            if (questionList[i].questions[l].correctAnswers != null) answers.AddRange(questionList[i].questions[l].correctAnswers);
                            if (questionList[i].questions[l].acceptableAnswers != null) answers.AddRange(questionList[i].questions[l].acceptableAnswers);
                            bool correctAnswer = false;
                            for (int k = 0; k < answers.Count; k++)
                            {
                                if (questionList[i].questions[l].playerAnswers[j].answers.ToLower() == answers[k].ToLower())
                                {
                                    correctAnswer = true;
                                    //Correct
                                }
                            }
                            if (correctAnswer)
                            {
                                //Cell at location gets wager
                                outputSheet[questionList[i].questions[0].playerAnswers[j].playerId + 1][questionList[i].questionNumber] = new csvLibrary.csvSheetCell(playerScores[j].ToString());
                            }
                            else
                            {
                                addCheckAnswer(i, 2, l, j);
                            }
                        }
                    }
                    
                }

                //Get type of question
                else if (questionList[i].questions[0].questionType == questionType.MystCat)
                {
                    //Sort the player answer lists to be equal
                    questionList[i].questions[0].playerAnswers.Sort(delegate (playerAnswer x, playerAnswer y)
                    {
                        if (x.playerId > y.playerId) return 1;
                        if (x.playerId < y.playerId) return -1;
                        return x.playerId.CompareTo(y.playerId);

                    });
                    questionList[i].wagers[0].playerAnswers.Sort(delegate (playerAnswer x, playerAnswer y)
                    {
                        if (x.playerId > y.playerId) return 1;
                        if (x.playerId < y.playerId) return -1;
                        return x.playerId.CompareTo(y.playerId);

                    });
                    //For each person
                    //We make the assumption that each person answered both the wager and the question
                    for (int j = 0; j < questionList[i].wagers[0].playerAnswers.Count; j++)
                    {
                        //Get wagers
                        int wager = Int32.Parse(questionList[i].wagers[0].playerAnswers[j].answers.ToString());
                        int wager2 = Int32.Parse(questionList[i].wagers[1].playerAnswers[j].answers.ToString());

                        //Check question
                        //Generate answer list
                        List<string> answers = new List<string>();
                        if (questionList[i].questions[0].correctAnswers != null) answers.AddRange(questionList[i].questions[0].correctAnswers);
                        if (questionList[i].questions[0].acceptableAnswers != null) answers.AddRange(questionList[i].questions[0].acceptableAnswers);
                        bool correctAnswer = false;
                        for (int k = 0; k < answers.Count; k++)
                        {
                            if (questionList[i].questions[0].playerAnswers[j].answers.ToLower() == answers[k].ToLower())
                            {
                                correctAnswer = true;
                                //Correct
                            }
                        }
                        if(wager == wager2 && wager < 21 && wager > -1)
                        {
                            if (correctAnswer)
                            {
                                //Cell at location gets wager
                                outputSheet[questionList[i].questions[0].playerAnswers[j].playerId + 1][questionList.Count] = new csvLibrary.csvSheetCell(wager.ToString());
                            }
                            else
                            {
                                addCheckAnswer(i, wager, 0, j);
                            }
                        }
                        else
                        {
                            addCheckAnswer(i, -1, 0, j);
                        }
                       
                    }
                }
            }
            outputSavesheet = outputSheet;
        }

    }
    
}
