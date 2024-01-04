using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScoringLogic
{
    
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void ScoresheetInput_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                
                scoresheetInputReader = new System.IO.StreamReader(ScoresheetInput.FileName);
                //Load into the CSV variable here
                scoresheetInputSheet = new csvLibrary.csvSheet();
                scoresheetInputSheet.readInFile(scoresheetInputReader);
                //Close the reader
                scoresheetInputReader.Close();
            }
            catch (SecurityException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void selectScoresheetButton_Click(object sender, EventArgs e)
        {
            ScoresheetInput.ShowDialog();
            scoresheetLabel.Text = ScoresheetInput.FileName;
        }
        private void SavesheetInput_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                savesheetInputReader = new System.IO.StreamReader(SavesheetInput.FileName);
                //Load into the CSV variable here
                savesheetInputSheet = new csvLibrary.csvSheet();
                savesheetInputSheet.readInFile(savesheetInputReader);
                //Close the reader
                savesheetInputReader.Close();
            }
            catch (SecurityException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void previousSaveLocationButton_Click(object sender, EventArgs e)
        {
            SavesheetInput.ShowDialog();
            previousScoresLocationLabel.Text = SavesheetInput.FileName;
        }
        private void clearSelectionButton_Click(object sender, EventArgs e)
        {
            SavesheetInput.FileName = null;
        }
        private void SavesheetOutput_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void saveLocationButton_Click(object sender, EventArgs e)
        {
            SavesheetOutput.ShowDialog();
            saveLocationLabel.Text = SavesheetOutput.FileName;
        }

        
        private void scoreCalcButton_Click(object sender, EventArgs e)
        {
            //Calculate the new sheet with the scores
            generateScoresheet();
            //Output the sheet
            handleUnknowns();
            //Select output sheet as new sheet
            //Increment outputsheet counter
            Console.WriteLine("Finished");
        }

        private void approveAnswerButton_Click(object sender, EventArgs e)
        {
            //If statement prevents pressing in the future
            approveUnknown(true);
        }

        private void disaproveAnswerButton_Click(object sender, EventArgs e)
        {
            //If statement prevents pressing in the future
           disaproveUnknown(true);
        }

        private void partialCreditButton_Click(object sender, EventArgs e)
        {
            partialUnknown(true, Int32.Parse(partialCreditBox.Text));
        }
    }
}
