using System.IO;

namespace ScoringLogic
{
    public partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ScoresheetInput = new System.Windows.Forms.OpenFileDialog();
            this.SavesheetOutput = new System.Windows.Forms.SaveFileDialog();
            this.SavesheetInput = new System.Windows.Forms.OpenFileDialog();
            this.selectScoresheetButton = new System.Windows.Forms.Button();
            this.previousSaveLocationButton = new System.Windows.Forms.Button();
            this.saveLocationButton = new System.Windows.Forms.Button();
            this.scoresheetLabel = new System.Windows.Forms.Label();
            this.previousScoresLocationLabel = new System.Windows.Forms.Label();
            this.saveLocationLabel = new System.Windows.Forms.Label();
            this.scoreCalcButton = new System.Windows.Forms.Button();
            this.clearSelectionButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.questionCheckLabel = new System.Windows.Forms.Label();
            this.answerCheckLabel = new System.Windows.Forms.Label();
            this.approveAnswerButton = new System.Windows.Forms.Button();
            this.disaproveAnswerButton = new System.Windows.Forms.Button();
            this.partialCreditButton = new System.Windows.Forms.Button();
            this.partialCreditBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ScoresheetInput
            // 
            this.ScoresheetInput.FileName = "Scoresheet";
            this.ScoresheetInput.FileOk += new System.ComponentModel.CancelEventHandler(this.ScoresheetInput_FileOk);
            // 
            // SavesheetOutput
            // 
            this.SavesheetOutput.DefaultExt = "csv";
            this.SavesheetOutput.FileName = "SavesheetOutput";
            this.SavesheetOutput.FileOk += new System.ComponentModel.CancelEventHandler(this.SavesheetOutput_FileOk);
            // 
            // SavesheetInput
            // 
            this.SavesheetInput.FileName = "SavesheetInput";
            this.SavesheetInput.FileOk += new System.ComponentModel.CancelEventHandler(this.SavesheetInput_FileOk);
            // 
            // selectScoresheetButton
            // 
            this.selectScoresheetButton.Location = new System.Drawing.Point(34, 37);
            this.selectScoresheetButton.Name = "selectScoresheetButton";
            this.selectScoresheetButton.Size = new System.Drawing.Size(109, 23);
            this.selectScoresheetButton.TabIndex = 0;
            this.selectScoresheetButton.Text = "select scoresheet";
            this.selectScoresheetButton.UseVisualStyleBackColor = true;
            this.selectScoresheetButton.Click += new System.EventHandler(this.selectScoresheetButton_Click);
            // 
            // previousSaveLocationButton
            // 
            this.previousSaveLocationButton.Location = new System.Drawing.Point(34, 111);
            this.previousSaveLocationButton.Name = "previousSaveLocationButton";
            this.previousSaveLocationButton.Size = new System.Drawing.Size(109, 35);
            this.previousSaveLocationButton.TabIndex = 1;
            this.previousSaveLocationButton.Text = "select previous scores";
            this.previousSaveLocationButton.UseVisualStyleBackColor = true;
            this.previousSaveLocationButton.Click += new System.EventHandler(this.previousSaveLocationButton_Click);
            // 
            // saveLocationButton
            // 
            this.saveLocationButton.Location = new System.Drawing.Point(34, 191);
            this.saveLocationButton.Name = "saveLocationButton";
            this.saveLocationButton.Size = new System.Drawing.Size(109, 23);
            this.saveLocationButton.TabIndex = 2;
            this.saveLocationButton.Text = "select save location";
            this.saveLocationButton.UseVisualStyleBackColor = true;
            this.saveLocationButton.Click += new System.EventHandler(this.saveLocationButton_Click);
            // 
            // scoresheetLabel
            // 
            this.scoresheetLabel.AutoSize = true;
            this.scoresheetLabel.Location = new System.Drawing.Point(166, 42);
            this.scoresheetLabel.Name = "scoresheetLabel";
            this.scoresheetLabel.Size = new System.Drawing.Size(65, 13);
            this.scoresheetLabel.TabIndex = 3;
            this.scoresheetLabel.Text = "not selected";
            // 
            // previousScoresLocationLabel
            // 
            this.previousScoresLocationLabel.AutoSize = true;
            this.previousScoresLocationLabel.Location = new System.Drawing.Point(166, 116);
            this.previousScoresLocationLabel.Name = "previousScoresLocationLabel";
            this.previousScoresLocationLabel.Size = new System.Drawing.Size(65, 13);
            this.previousScoresLocationLabel.TabIndex = 4;
            this.previousScoresLocationLabel.Text = "not selected";
            // 
            // saveLocationLabel
            // 
            this.saveLocationLabel.AutoSize = true;
            this.saveLocationLabel.Location = new System.Drawing.Point(166, 196);
            this.saveLocationLabel.Name = "saveLocationLabel";
            this.saveLocationLabel.Size = new System.Drawing.Size(65, 13);
            this.saveLocationLabel.TabIndex = 5;
            this.saveLocationLabel.Text = "not selected";
            // 
            // scoreCalcButton
            // 
            this.scoreCalcButton.Location = new System.Drawing.Point(34, 271);
            this.scoreCalcButton.Name = "scoreCalcButton";
            this.scoreCalcButton.Size = new System.Drawing.Size(109, 23);
            this.scoreCalcButton.TabIndex = 6;
            this.scoreCalcButton.Text = "Calculate scores";
            this.scoreCalcButton.UseVisualStyleBackColor = true;
            this.scoreCalcButton.Click += new System.EventHandler(this.scoreCalcButton_Click);
            // 
            // clearSelectionButton
            // 
            this.clearSelectionButton.Location = new System.Drawing.Point(34, 143);
            this.clearSelectionButton.Name = "clearSelectionButton";
            this.clearSelectionButton.Size = new System.Drawing.Size(109, 21);
            this.clearSelectionButton.TabIndex = 7;
            this.clearSelectionButton.Text = "clear selection";
            this.clearSelectionButton.UseVisualStyleBackColor = true;
            this.clearSelectionButton.Click += new System.EventHandler(this.clearSelectionButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(395, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Current Question:";
            // 
            // questionCheckLabel
            // 
            this.questionCheckLabel.AutoSize = true;
            this.questionCheckLabel.Location = new System.Drawing.Point(395, 82);
            this.questionCheckLabel.Name = "questionCheckLabel";
            this.questionCheckLabel.Size = new System.Drawing.Size(83, 13);
            this.questionCheckLabel.TabIndex = 9;
            this.questionCheckLabel.Text = "\"Question Text\"";
            // 
            // answerCheckLabel
            // 
            this.answerCheckLabel.AutoSize = true;
            this.answerCheckLabel.Location = new System.Drawing.Point(395, 133);
            this.answerCheckLabel.Name = "answerCheckLabel";
            this.answerCheckLabel.Size = new System.Drawing.Size(89, 13);
            this.answerCheckLabel.TabIndex = 10;
            this.answerCheckLabel.Text = "\"Current Answer\"";
            // 
            // approveAnswerButton
            // 
            this.approveAnswerButton.Location = new System.Drawing.Point(398, 322);
            this.approveAnswerButton.Name = "approveAnswerButton";
            this.approveAnswerButton.Size = new System.Drawing.Size(75, 23);
            this.approveAnswerButton.TabIndex = 11;
            this.approveAnswerButton.Text = "approve";
            this.approveAnswerButton.UseVisualStyleBackColor = true;
            this.approveAnswerButton.Click += new System.EventHandler(this.approveAnswerButton_Click);
            // 
            // disaproveAnswerButton
            // 
            this.disaproveAnswerButton.Location = new System.Drawing.Point(398, 380);
            this.disaproveAnswerButton.Name = "disaproveAnswerButton";
            this.disaproveAnswerButton.Size = new System.Drawing.Size(75, 23);
            this.disaproveAnswerButton.TabIndex = 12;
            this.disaproveAnswerButton.Text = "disaprove";
            this.disaproveAnswerButton.UseVisualStyleBackColor = true;
            this.disaproveAnswerButton.Click += new System.EventHandler(this.disaproveAnswerButton_Click);
            // 
            // partialCreditButton
            // 
            this.partialCreditButton.Location = new System.Drawing.Point(398, 351);
            this.partialCreditButton.Name = "partialCreditButton";
            this.partialCreditButton.Size = new System.Drawing.Size(75, 23);
            this.partialCreditButton.TabIndex = 13;
            this.partialCreditButton.Text = "Partial credit";
            this.partialCreditButton.UseVisualStyleBackColor = true;
            this.partialCreditButton.Click += new System.EventHandler(this.partialCreditButton_Click);
            // 
            // partialCreditBox
            // 
            this.partialCreditBox.Location = new System.Drawing.Point(479, 353);
            this.partialCreditBox.Name = "partialCreditBox";
            this.partialCreditBox.Size = new System.Drawing.Size(47, 20);
            this.partialCreditBox.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.partialCreditBox);
            this.Controls.Add(this.partialCreditButton);
            this.Controls.Add(this.disaproveAnswerButton);
            this.Controls.Add(this.approveAnswerButton);
            this.Controls.Add(this.answerCheckLabel);
            this.Controls.Add(this.questionCheckLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clearSelectionButton);
            this.Controls.Add(this.scoreCalcButton);
            this.Controls.Add(this.saveLocationLabel);
            this.Controls.Add(this.previousScoresLocationLabel);
            this.Controls.Add(this.scoresheetLabel);
            this.Controls.Add(this.saveLocationButton);
            this.Controls.Add(this.previousSaveLocationButton);
            this.Controls.Add(this.selectScoresheetButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ScoresheetInput;
        private System.Windows.Forms.SaveFileDialog SavesheetOutput;
        private System.Windows.Forms.OpenFileDialog SavesheetInput;
        private System.Windows.Forms.Button selectScoresheetButton;
        private System.Windows.Forms.Button previousSaveLocationButton;
        private System.Windows.Forms.Button saveLocationButton;
        private System.Windows.Forms.Label scoresheetLabel;
        private System.Windows.Forms.Label previousScoresLocationLabel;
        private System.Windows.Forms.Label saveLocationLabel;
        private System.Windows.Forms.Button scoreCalcButton;
        private System.Windows.Forms.Button clearSelectionButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label questionCheckLabel;
        private System.Windows.Forms.Label answerCheckLabel;
        private System.Windows.Forms.Button approveAnswerButton;
        private System.Windows.Forms.Button disaproveAnswerButton;

        //Added variables
        private StreamReader scoresheetInputReader;
        private csvLibrary.csvSheet scoresheetInputSheet;
        private csvLibrary.csvSheet cleanedScoresheetInputSheet;
        private StreamReader savesheetInputReader;
        private csvLibrary.csvSheet savesheetInputSheet;
        private csvLibrary.csvSheet outputSavesheet;
        //0 -> waiting for check, 1 -> need check, 2 -> approve, 3 -> disapprove 4 -> partial credit
        private int QUCheck = 0;
        private System.Windows.Forms.Button partialCreditButton;
        private System.Windows.Forms.TextBox partialCreditBox;
    }
}

