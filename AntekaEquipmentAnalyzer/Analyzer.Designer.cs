
namespace AntekaEquipmentAnalyzer
{
    partial class Analyzer
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
            this.button_SelectWindow = new System.Windows.Forms.Button();
            this.textBox_WindowSelected = new System.Windows.Forms.TextBox();
            this.groupBox_WindowSelector = new System.Windows.Forms.GroupBox();
            this.button_Check = new System.Windows.Forms.Button();
            this.textBox_Value = new System.Windows.Forms.TextBox();
            this.progressBar_Percent = new System.Windows.Forms.ProgressBar();
            this.groupBox_Substat = new System.Windows.Forms.GroupBox();
            this.textBox_ReforgeValue = new System.Windows.Forms.TextBox();
            this.label_Arrow = new System.Windows.Forms.Label();
            this.label_Percent = new System.Windows.Forms.Label();
            this.label_ValueMax = new System.Windows.Forms.Label();
            this.label_Rolls = new System.Windows.Forms.Label();
            this.label_RollsLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel_Substats = new System.Windows.Forms.FlowLayoutPanel();
            this.label_GearScore = new System.Windows.Forms.Label();
            this.label_GearScoreLabel = new System.Windows.Forms.Label();
            this.label_RGearScoreLabel = new System.Windows.Forms.Label();
            this.label_GearScoreReforged = new System.Windows.Forms.Label();
            this.label_MaxPotential = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Quality = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Enhancement = new System.Windows.Forms.TextBox();
            this.progressBar_WeightedTotal = new System.Windows.Forms.ProgressBar();
            this.label_WeightedTotal = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox_WindowSelector.SuspendLayout();
            this.groupBox_Substat.SuspendLayout();
            this.flowLayoutPanel_Substats.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_SelectWindow
            // 
            this.button_SelectWindow.Location = new System.Drawing.Point(6, 19);
            this.button_SelectWindow.Name = "button_SelectWindow";
            this.button_SelectWindow.Size = new System.Drawing.Size(75, 20);
            this.button_SelectWindow.TabIndex = 0;
            this.button_SelectWindow.Text = "Select";
            this.button_SelectWindow.UseVisualStyleBackColor = true;
            this.button_SelectWindow.Click += new System.EventHandler(this.button_SelectWindow_Click);
            // 
            // textBox_WindowSelected
            // 
            this.textBox_WindowSelected.Location = new System.Drawing.Point(87, 19);
            this.textBox_WindowSelected.Name = "textBox_WindowSelected";
            this.textBox_WindowSelected.ReadOnly = true;
            this.textBox_WindowSelected.Size = new System.Drawing.Size(196, 20);
            this.textBox_WindowSelected.TabIndex = 1;
            // 
            // groupBox_WindowSelector
            // 
            this.groupBox_WindowSelector.Controls.Add(this.button_SelectWindow);
            this.groupBox_WindowSelector.Controls.Add(this.textBox_WindowSelected);
            this.groupBox_WindowSelector.Location = new System.Drawing.Point(12, 12);
            this.groupBox_WindowSelector.Name = "groupBox_WindowSelector";
            this.groupBox_WindowSelector.Size = new System.Drawing.Size(289, 52);
            this.groupBox_WindowSelector.TabIndex = 2;
            this.groupBox_WindowSelector.TabStop = false;
            this.groupBox_WindowSelector.Text = "Window Selector";
            // 
            // button_Check
            // 
            this.button_Check.Location = new System.Drawing.Point(12, 70);
            this.button_Check.Name = "button_Check";
            this.button_Check.Size = new System.Drawing.Size(289, 52);
            this.button_Check.TabIndex = 3;
            this.button_Check.Text = "Check";
            this.button_Check.UseVisualStyleBackColor = true;
            this.button_Check.Click += new System.EventHandler(this.button_Check_Click);
            // 
            // textBox_Value
            // 
            this.textBox_Value.AcceptsReturn = true;
            this.textBox_Value.Location = new System.Drawing.Point(47, 28);
            this.textBox_Value.Name = "textBox_Value";
            this.textBox_Value.ReadOnly = true;
            this.textBox_Value.Size = new System.Drawing.Size(67, 26);
            this.textBox_Value.TabIndex = 4;
            // 
            // progressBar_Percent
            // 
            this.progressBar_Percent.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar_Percent.ForeColor = System.Drawing.Color.Lime;
            this.progressBar_Percent.Location = new System.Drawing.Point(47, 60);
            this.progressBar_Percent.Name = "progressBar_Percent";
            this.progressBar_Percent.Size = new System.Drawing.Size(186, 10);
            this.progressBar_Percent.TabIndex = 5;
            this.progressBar_Percent.Value = 10;
            // 
            // groupBox_Substat
            // 
            this.groupBox_Substat.Controls.Add(this.textBox_ReforgeValue);
            this.groupBox_Substat.Controls.Add(this.label_Arrow);
            this.groupBox_Substat.Controls.Add(this.label_Percent);
            this.groupBox_Substat.Controls.Add(this.label_ValueMax);
            this.groupBox_Substat.Controls.Add(this.label_Rolls);
            this.groupBox_Substat.Controls.Add(this.label_RollsLabel);
            this.groupBox_Substat.Controls.Add(this.textBox_Value);
            this.groupBox_Substat.Controls.Add(this.progressBar_Percent);
            this.groupBox_Substat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Substat.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Substat.Name = "groupBox_Substat";
            this.groupBox_Substat.Size = new System.Drawing.Size(286, 82);
            this.groupBox_Substat.TabIndex = 6;
            this.groupBox_Substat.TabStop = false;
            this.groupBox_Substat.Text = "Attack %";
            // 
            // textBox_ReforgeValue
            // 
            this.textBox_ReforgeValue.AcceptsReturn = true;
            this.textBox_ReforgeValue.Location = new System.Drawing.Point(195, 28);
            this.textBox_ReforgeValue.Name = "textBox_ReforgeValue";
            this.textBox_ReforgeValue.ReadOnly = true;
            this.textBox_ReforgeValue.Size = new System.Drawing.Size(67, 26);
            this.textBox_ReforgeValue.TabIndex = 11;
            // 
            // label_Arrow
            // 
            this.label_Arrow.AutoSize = true;
            this.label_Arrow.Location = new System.Drawing.Point(162, 31);
            this.label_Arrow.Name = "label_Arrow";
            this.label_Arrow.Size = new System.Drawing.Size(27, 20);
            this.label_Arrow.TabIndex = 10;
            this.label_Arrow.Text = ">>";
            // 
            // label_Percent
            // 
            this.label_Percent.AutoSize = true;
            this.label_Percent.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Percent.Location = new System.Drawing.Point(239, 57);
            this.label_Percent.Name = "label_Percent";
            this.label_Percent.Size = new System.Drawing.Size(28, 13);
            this.label_Percent.TabIndex = 9;
            this.label_Percent.Text = "84%";
            // 
            // label_ValueMax
            // 
            this.label_ValueMax.AutoSize = true;
            this.label_ValueMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ValueMax.Location = new System.Drawing.Point(120, 41);
            this.label_ValueMax.Name = "label_ValueMax";
            this.label_ValueMax.Size = new System.Drawing.Size(25, 13);
            this.label_ValueMax.TabIndex = 8;
            this.label_ValueMax.Text = "/ 50";
            // 
            // label_Rolls
            // 
            this.label_Rolls.AutoSize = true;
            this.label_Rolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Rolls.Location = new System.Drawing.Point(8, 21);
            this.label_Rolls.Name = "label_Rolls";
            this.label_Rolls.Size = new System.Drawing.Size(33, 37);
            this.label_Rolls.TabIndex = 7;
            this.label_Rolls.Text = "1";
            // 
            // label_RollsLabel
            // 
            this.label_RollsLabel.AutoSize = true;
            this.label_RollsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_RollsLabel.Location = new System.Drawing.Point(10, 57);
            this.label_RollsLabel.Name = "label_RollsLabel";
            this.label_RollsLabel.Size = new System.Drawing.Size(29, 13);
            this.label_RollsLabel.TabIndex = 6;
            this.label_RollsLabel.Text = "Rolls";
            // 
            // flowLayoutPanel_Substats
            // 
            this.flowLayoutPanel_Substats.Controls.Add(this.groupBox_Substat);
            this.flowLayoutPanel_Substats.Location = new System.Drawing.Point(12, 128);
            this.flowLayoutPanel_Substats.Name = "flowLayoutPanel_Substats";
            this.flowLayoutPanel_Substats.Size = new System.Drawing.Size(289, 353);
            this.flowLayoutPanel_Substats.TabIndex = 7;
            // 
            // label_GearScore
            // 
            this.label_GearScore.AutoSize = true;
            this.label_GearScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GearScore.Location = new System.Drawing.Point(348, 177);
            this.label_GearScore.Name = "label_GearScore";
            this.label_GearScore.Size = new System.Drawing.Size(104, 73);
            this.label_GearScore.TabIndex = 8;
            this.label_GearScore.Text = "00";
            // 
            // label_GearScoreLabel
            // 
            this.label_GearScoreLabel.AutoSize = true;
            this.label_GearScoreLabel.Location = new System.Drawing.Point(368, 164);
            this.label_GearScoreLabel.Name = "label_GearScoreLabel";
            this.label_GearScoreLabel.Size = new System.Drawing.Size(58, 13);
            this.label_GearScoreLabel.TabIndex = 9;
            this.label_GearScoreLabel.Text = "GearScore";
            // 
            // label_RGearScoreLabel
            // 
            this.label_RGearScoreLabel.AutoSize = true;
            this.label_RGearScoreLabel.Location = new System.Drawing.Point(347, 273);
            this.label_RGearScoreLabel.Name = "label_RGearScoreLabel";
            this.label_RGearScoreLabel.Size = new System.Drawing.Size(105, 13);
            this.label_RGearScoreLabel.TabIndex = 11;
            this.label_RGearScoreLabel.Text = "GearScore Reforged";
            // 
            // label_GearScoreReforged
            // 
            this.label_GearScoreReforged.AutoSize = true;
            this.label_GearScoreReforged.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GearScoreReforged.Location = new System.Drawing.Point(348, 284);
            this.label_GearScoreReforged.Name = "label_GearScoreReforged";
            this.label_GearScoreReforged.Size = new System.Drawing.Size(104, 73);
            this.label_GearScoreReforged.TabIndex = 10;
            this.label_GearScoreReforged.Text = "00";
            // 
            // label_MaxPotential
            // 
            this.label_MaxPotential.AutoSize = true;
            this.label_MaxPotential.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_MaxPotential.Location = new System.Drawing.Point(348, 394);
            this.label_MaxPotential.Name = "label_MaxPotential";
            this.label_MaxPotential.Size = new System.Drawing.Size(104, 73);
            this.label_MaxPotential.TabIndex = 12;
            this.label_MaxPotential.Text = "00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(364, 381);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Max Potential";
            // 
            // textBox_Quality
            // 
            this.textBox_Quality.AcceptsReturn = true;
            this.textBox_Quality.Location = new System.Drawing.Point(313, 87);
            this.textBox_Quality.Name = "textBox_Quality";
            this.textBox_Quality.ReadOnly = true;
            this.textBox_Quality.Size = new System.Drawing.Size(169, 20);
            this.textBox_Quality.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(320, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Gear Quality";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(320, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Gear Enhancement";
            // 
            // textBox_Enhancement
            // 
            this.textBox_Enhancement.AcceptsReturn = true;
            this.textBox_Enhancement.Location = new System.Drawing.Point(313, 129);
            this.textBox_Enhancement.Name = "textBox_Enhancement";
            this.textBox_Enhancement.ReadOnly = true;
            this.textBox_Enhancement.Size = new System.Drawing.Size(169, 20);
            this.textBox_Enhancement.TabIndex = 15;
            // 
            // progressBar_WeightedTotal
            // 
            this.progressBar_WeightedTotal.ForeColor = System.Drawing.Color.Lime;
            this.progressBar_WeightedTotal.Location = new System.Drawing.Point(12, 503);
            this.progressBar_WeightedTotal.Name = "progressBar_WeightedTotal";
            this.progressBar_WeightedTotal.Size = new System.Drawing.Size(424, 17);
            this.progressBar_WeightedTotal.TabIndex = 17;
            // 
            // label_WeightedTotal
            // 
            this.label_WeightedTotal.AutoSize = true;
            this.label_WeightedTotal.Location = new System.Drawing.Point(441, 505);
            this.label_WeightedTotal.Name = "label_WeightedTotal";
            this.label_WeightedTotal.Size = new System.Drawing.Size(0, 13);
            this.label_WeightedTotal.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 487);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Weighted Total Percentage";
            // 
            // Analyzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 536);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label_WeightedTotal);
            this.Controls.Add(this.progressBar_WeightedTotal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_Enhancement);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_Quality);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_MaxPotential);
            this.Controls.Add(this.label_RGearScoreLabel);
            this.Controls.Add(this.label_GearScoreReforged);
            this.Controls.Add(this.label_GearScoreLabel);
            this.Controls.Add(this.label_GearScore);
            this.Controls.Add(this.flowLayoutPanel_Substats);
            this.Controls.Add(this.button_Check);
            this.Controls.Add(this.groupBox_WindowSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Analyzer";
            this.Text = "Anteka\'s Gear Analyzer";
            this.groupBox_WindowSelector.ResumeLayout(false);
            this.groupBox_WindowSelector.PerformLayout();
            this.groupBox_Substat.ResumeLayout(false);
            this.groupBox_Substat.PerformLayout();
            this.flowLayoutPanel_Substats.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_SelectWindow;
        private System.Windows.Forms.TextBox textBox_WindowSelected;
        private System.Windows.Forms.GroupBox groupBox_WindowSelector;
        private System.Windows.Forms.Button button_Check;
        private System.Windows.Forms.TextBox textBox_Value;
        private System.Windows.Forms.ProgressBar progressBar_Percent;
        private System.Windows.Forms.GroupBox groupBox_Substat;
        private System.Windows.Forms.Label label_Percent;
        private System.Windows.Forms.Label label_ValueMax;
        private System.Windows.Forms.Label label_Rolls;
        private System.Windows.Forms.Label label_RollsLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Substats;
        private System.Windows.Forms.Label label_GearScore;
        private System.Windows.Forms.Label label_GearScoreLabel;
        private System.Windows.Forms.TextBox textBox_ReforgeValue;
        private System.Windows.Forms.Label label_Arrow;
        private System.Windows.Forms.Label label_RGearScoreLabel;
        private System.Windows.Forms.Label label_GearScoreReforged;
        private System.Windows.Forms.Label label_MaxPotential;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_Quality;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_Enhancement;
        private System.Windows.Forms.ProgressBar progressBar_WeightedTotal;
        private System.Windows.Forms.Label label_WeightedTotal;
        private System.Windows.Forms.Label label5;
    }
}

