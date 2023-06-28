using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tesseract;

namespace AntekaEquipmentAnalyzer
{
    public partial class Analyzer : Form
    {
        internal HookProc _globalLlMouseHookCallback;
        internal IntPtr _hGlobalLlMouseHook;
        private static IntPtr _targetWindow; 

        public Analyzer()
        {
            InitializeComponent();
            groupBox_Substat.Dispose();
        }

        #region Window Selection

        //Pinvoke shit
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point lpPoint);
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(System.Drawing.Point p);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        // When you click this button, it'll hook the mouse event globally for the next press.
        private void button_SelectWindow_Click(object sender, EventArgs e)
        {
            // Create an instance of HookProc.
            _globalLlMouseHookCallback = SelectWindow;

            _hGlobalLlMouseHook = NativeMethods.SetWindowsHookEx(
                HookType.WH_MOUSE_LL,
                _globalLlMouseHookCallback,
                Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),
                0);
        }
        public int SelectWindow(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                // Get the mouse WM from the wParam parameter
                var wmMouse = (MouseMessage)wParam;
                if (wmMouse == MouseMessage.WM_LBUTTONDOWN)
                {
                    Point p;
                    if (GetCursorPos(out p))
                    {
                        _targetWindow = WindowFromPoint(p);
                        int length = GetWindowTextLength(_targetWindow);
                        StringBuilder sb = new StringBuilder(length + 1);
                        GetWindowText(_targetWindow, sb, sb.Capacity);
                        textBox_WindowSelected.Text = sb.ToString();
                    }

                    if (_hGlobalLlMouseHook != IntPtr.Zero)
                    {
                        // Unhook the low-level mouse hook
                        if (!NativeMethods.UnhookWindowsHookEx(_hGlobalLlMouseHook))
                            throw new Win32Exception("Unable to clear hook;");
                        _hGlobalLlMouseHook = IntPtr.Zero;
                    }
                }
            }

            // Pass the hook information to the next hook procedure in chain
            return NativeMethods.CallNextHookEx(_hGlobalLlMouseHook, nCode, wParam, lParam);
        }
        #endregion

        #region Screenshot Capture

        //Pinvoke shit
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr handle, ref Rectangle rect);
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr handle, out Rectangle rect);

        public static Bitmap CropPercent(Bitmap b, float left, float right, float top, float bottom)
        {
            var height = b.Height * (1f - top - bottom);
            var width = b.Width * (1f - left - right);
            Bitmap nb = new Bitmap((int)width, (int)height);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -b.Width * left, -b.Height * top);
                return nb;
            }
        }

        static Bitmap TrimBitmap(Bitmap source)
        {
            Rectangle srcRect = default(Rectangle);
            BitmapData data = null;
            try
            {
                data = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte[] buffer = new byte[data.Height * data.Stride];
                Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
                int xMin = int.MaxValue;
                int xMax = 0;
                int yMin = int.MaxValue;
                int yMax = 0;
                for (int y = 0; y < data.Height; y++)
                {
                    for (int x = 0; x < data.Width; x++)
                    {
                        int totalVal = 0;
                        for (int i = 0; i < 3; i++)
                            totalVal += buffer[y * data.Stride + 4 * x + i];
                        if (totalVal < 765)
                        {
                            if (x < xMin) xMin = x;
                            if (x > xMax) xMax = x;
                            if (y < yMin) yMin = y;
                            if (y > yMax) yMax = y;
                        }
                    }
                }
                if (xMax < xMin || yMax < yMin)
                {
                    // Image is empty...
                    return null;
                }
                srcRect = Rectangle.FromLTRB(xMin, yMin, xMax, yMax);
            }
            finally
            {
                if (data != null)
                    source.UnlockBits(data);
            }

            Bitmap dest = new Bitmap(srcRect.Width, srcRect.Height);
            Rectangle destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            using (Graphics graphics = Graphics.FromImage(dest))
            {
                graphics.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return dest;
        }

        // We might be able to avoid a lot of trimming/time if we capture only the client area, but
        // In my tests bluestacks was still pulling a blank top so who knows for sure.
        static Bitmap CaptureImage()
        {
            Rectangle rect = new Rectangle();
            GetClientRect(_targetWindow, out rect);

            //rect.Width = rect.Width - rect.X;
            //rect.Height = rect.Height - rect.Y;

            // Create a bitmap to draw the capture into
            using (Bitmap bitmap = new Bitmap(rect.Width, rect.Height))
            {
                // Use PrintWindow to draw the window into our bitmap
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    IntPtr hdc = g.GetHdc();
                    if (!PrintWindow(_targetWindow, hdc, 0x00000002))
                    {
                        int error = Marshal.GetLastWin32Error();
                        var exception = new System.ComponentModel.Win32Exception(error);
                        Debug.WriteLine("ERROR: " + error + ": " + exception.Message);
                        // TODO: Throw the exception?
                    }
                    g.ReleaseHdc(hdc);
                }
                Directory.CreateDirectory("images");
                var trimmed = TrimBitmap(bitmap);
                bitmap.Save("images/raw_screen.png");
                trimmed.Save("images/raw_screen_trimmed.png");
                return trimmed;
            }
        }

        // We're going to check the average brightness of each pixel and set it to black or white based on
        // a cut off threshold.
        public static Bitmap Polarize(Bitmap bmp, float cutoff, bool forceBlack = true, int maxColorDist = 255, bool invert = false)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);
            for (int counter = 0; counter < rgbValues.Length; counter++)
            {
                //scan over rgb
                var brightness = 0f;
                var minBrightness = 255;
                var minColor = 255;
                var maxColor = 0;
                for (int i = 0; i < 3; i++)
                {
                    brightness += rgbValues[counter + i];
                    if (rgbValues[counter + i] < minBrightness)
                        minBrightness = rgbValues[counter + i];
                    if (rgbValues[counter + i] < minColor)
                        minColor = rgbValues[counter + i];
                    if (rgbValues[counter + i] > maxColor)
                        maxColor = rgbValues[counter + i];
                }
                brightness /= (255 * 3);

                for (int i = 0; i < 3; i++)
                {
                    if (maxColor - minColor > maxColorDist)
                        rgbValues[counter + i] = 255;
                    else
                        rgbValues[counter + i] = (byte)(brightness > cutoff ? (forceBlack ? 0 : (invert ? brightness : 1 - brightness) * 255) : 255);
                }
                counter += 3;
            }
            Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        public bool CropImage()
        {
            Bitmap bp = (Bitmap)Bitmap.FromFile("images/raw_screen_trimmed.png");

            // These crops should be able to be set by selection, but fuck it for now.
            // Hard code baby lets go.

            // This is the stats - I'm going to save these seperately in case I need to debug
            var cropped = CropPercent(bp, 0.02f, 0.71f, 0.34f, 0.5f);
            cropped.Save("images/stats.png");
            Polarize(cropped, 0.2f, false, 20, false).Save("images/stats_polarized.png");
            cropped = CropPercent(bp, 0.02f, 0.71f, 0.34f, 0.5f);
            Polarize(cropped, 0.2f, false, 20, true).Save("images/stats_polarized_inverted.png");

            // This is the gear level bubble.
            var gearLevel = CropPercent(bp, 0.074f, 0.9f, 0.11f, 0.85f);
            gearLevel.Save("images/gearlevel.png");
            Polarize(gearLevel, 0.8f, false, 20).Save("images/gearlevel_polarized.png");

            // This is the gear type
            var gearType = CropPercent(bp, 0.105f, 0.82f, 0.1f, 0.82f);
            gearType.Save("images/geartype.png");
            Polarize(gearType, 0.3f).Save("images/geartype_polarized.png");

            var tooSmall = bp.Width < 1200;
            bp.Dispose();
            if (tooSmall)
                return !(MessageBox.Show("The size of the captured image is smaller than the recommended, this may result in errors if you proceed anyway. You can increase the size of your emulator window to get better results. Proceed anyway?", "Resolution Size Issue", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No);
            return true;
        }


        #endregion

        private void button_Check_Click(object sender, EventArgs e)
        {
            if(_targetWindow != IntPtr.Zero)
                CaptureImage(); //Saved image
            if(!File.Exists("images/raw_screen_trimmed.png"))
            {
                MessageBox.Show("No image to process exists. Make sure you're selecting the correct window.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!CropImage())
                    return;
                try
                {
                    AnalyzeGear();
                }
                catch
                {
                    MessageBox.Show("There was an error analyzing gear. You can try resizing the emulator to potentially fix the issue. If you'd like to report the error, please add the image located in 'AntekaEquipmentAnalyzer\\images\\raw_screen_trimmed.png' to an issue on the github page.", "Error", MessageBoxButtons.OK);
                };
            }
        }

        private void AnalyzeGear()
        {
            foreach (var c in flowLayoutPanel_Substats.Controls)
                ((GroupBox)c).Dispose();
            flowLayoutPanel_Substats.Controls.Clear();

            string sGearStats, sGearLevel, sGearType, sGearStatsInverted;
            // First we need to OCR all the images that have been cut to build the item.
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                engine.DefaultPageSegMode = PageSegMode.SingleBlock;
                Bitmap bmp = (Bitmap)Bitmap.FromFile("images/stats_polarized.png");
                using (var img = PixConverter.ToPix(bmp))
                    using (var page = engine.Process(img))
                        sGearStats = page.GetText();

                bmp.Dispose();
                bmp = (Bitmap)Bitmap.FromFile("images/stats_polarized_inverted.png");
                using (var img = PixConverter.ToPix(bmp))
                    using (var page = engine.Process(img))
                        sGearStatsInverted = page.GetText();

                bmp.Dispose();
                bmp = (Bitmap)Bitmap.FromFile("images/gearlevel_polarized.png");
                using (var img = PixConverter.ToPix(bmp))
                    using (var page = engine.Process(img))
                        sGearLevel = page.GetText();

                bmp.Dispose();
                bmp = (Bitmap)Bitmap.FromFile("images/geartype_polarized.png");
                using (var img = PixConverter.ToPix(bmp))
                    using (var page = engine.Process(img))
                        sGearType = page.GetText();
                bmp.Dispose();
            }
            // Build the gear piece
            var gear = new Gear();
            gear.SetGearEnhanceFromString(sGearLevel);
            gear.SetGearTypeFromString(sGearType);

            // Build the substats array
            var sGearStatsToks = Regex.Replace(sGearStats, @"\t|\n|\r", "|").Split('|').Where(x => x != string.Empty).ToArray();
            var sGearStatsInvToks = Regex.Replace(sGearStatsInverted, @"\t|\n|\r", "|").Split('|').Where(x => x != string.Empty).ToArray();
            var sGearStatsCombined = new List<string>();
            for(int i = 0; i < Math.Max(sGearStatsToks.Length, sGearStatsInvToks.Length); i++)
            {
                // If one has digits and the other does not, just add that one
                // If they both have digits, use the longer one for now
                if (!sGearStatsToks[i].Any(char.IsDigit) && sGearStatsInvToks[i].Any(char.IsDigit))
                    sGearStatsCombined.Add(sGearStatsInvToks[i]);
                else if (!sGearStatsInvToks[i].Any(char.IsDigit) && sGearStatsToks[i].Any(char.IsDigit))
                    sGearStatsCombined.Add(sGearStatsToks[i]);
                else
                    sGearStatsCombined.Add(sGearStatsInvToks[i].Length > sGearStatsToks[i].Length ? sGearStatsInvToks[i] : sGearStatsToks[i]);
            }
            gear.AddSubstatsFromString(sGearStatsCombined.ToArray());
            gear.AttemptToAssignRollCounts(); // Try to figure out where things rolled
            gear.CalculateIdealRolls();

            foreach (var sub in gear.subs)
                flowLayoutPanel_Substats.Controls.Add(new SubstatInfo(sub, gear.gearType).groupBox_Substat);

            label_GearScore.Text = $"{gear.gearscore}";
            label_GearScoreReforged.Text = $"{gear.gearscoreReforge}";
            label_MaxPotential.Text = $"{gear.gearscoreReforge + gear.idealIncrease}";
            textBox_Enhancement.Text = $"+{gear.eLevel}";
            textBox_Quality.Text = $"{gear.gearTypeStr}";

            // Calculate weighted percent total
            var maxPossibleGearscore = gear.subs.Sum(x => x.maxPossibleGearScoreValue(gear.gearType));
            var minPossibleGearscore = gear.subs.Sum(x => x.minPossibleGearScoreValue(gear.gearType));
            var weightedTotal = (gear.gearscore - minPossibleGearscore) / (maxPossibleGearscore - minPossibleGearscore) * 100;
            progressBar_WeightedTotal.Value = (int)weightedTotal;
            label_WeightedTotal.Text = $"{(int)weightedTotal}%";
        }

    }

    public class SubstatInfo
    {
        public TextBox textBox_Value, textBox_ReforgeValue;
        public ProgressBar progressBar_Percent;
        public GroupBox groupBox_Substat;
        public Label label_Percent, label_ValueMax, label_Rolls, label_RollsLabel, label_Arrow;
        public SubstatInfo(Substat s, int type)
        {
            //Group Box
            groupBox_Substat = new GroupBox();
            groupBox_Substat.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            groupBox_Substat.Location = new Point(3, 3);
            groupBox_Substat.Name = "groupBox_Substat";
            groupBox_Substat.Size = new Size(286, 82);
            groupBox_Substat.Text = s.name;

            //Progress Bar
            progressBar_Percent = new ProgressBar();
            progressBar_Percent.BackColor = SystemColors.Control;
            progressBar_Percent.ForeColor = Color.Lime;
            progressBar_Percent.Location = new Point(47, 60);
            progressBar_Percent.Size = new Size(186, 10);
            progressBar_Percent.Value = (int)s.percentVal(type);

            //Value Textbox
            textBox_Value = new TextBox();
            textBox_Value.Location = new Point(47, 28);
            textBox_Value.ReadOnly = true;
            textBox_Value.Size = new Size(67, 26);
            textBox_Value.Text = $"{s.value}";

            //Roll Label
            label_RollsLabel = new Label();
            label_RollsLabel.Font = new Font("Microsoft Sans Serif", 7F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            label_RollsLabel.Location = new Point(10, 57);
            label_RollsLabel.Size = new Size(29, 13);
            label_RollsLabel.Text = "Rolls";

            //Roll Value Label
            label_Rolls = new Label();
            label_Rolls.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            label_Rolls.Location = new Point(8, 21);
            label_Rolls.Size = new Size(33, 37);
            label_Rolls.Text = $"{s.rolls - 1}";

            //Max Value Label
            label_ValueMax = new Label();
            label_ValueMax.Font = new Font("Microsoft Sans Serif", 7F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            label_ValueMax.Location = new Point(120, 41);
            label_ValueMax.Name = "label_ValueMax";
            label_ValueMax.Size = new Size(35, 13);
            label_ValueMax.Text = $"/ {s.maxPossibleValue(type)}";

            //Percent Label
            label_Percent = new Label();
            label_Percent.Font = new Font("Microsoft Sans Serif", 7F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            label_Percent.Location = new Point(239, 57);
            label_Percent.Size = new Size(40, 13);
            label_Percent.Text = $"{(int)s.percentVal(type)}%";

            //Arrow Label
            label_Arrow = new Label();
            label_Arrow.Location = new Point(162, 31);
            label_Arrow.Size = new Size(27, 20);
            label_Arrow.Text = ">>";

            //Reforge Value Textbox
            textBox_ReforgeValue = new TextBox();
            textBox_ReforgeValue.Location = new Point(195, 28);
            textBox_ReforgeValue.ReadOnly = true;
            textBox_ReforgeValue.Size = new Size(67, 26);
            textBox_ReforgeValue.Text = $"{s.valueReforged}";

            groupBox_Substat.Controls.Add(label_Percent);
            groupBox_Substat.Controls.Add(label_ValueMax);
            groupBox_Substat.Controls.Add(label_Rolls);
            groupBox_Substat.Controls.Add(label_RollsLabel);
            groupBox_Substat.Controls.Add(label_Arrow);
            groupBox_Substat.Controls.Add(textBox_Value);
            groupBox_Substat.Controls.Add(textBox_ReforgeValue);
            groupBox_Substat.Controls.Add(progressBar_Percent);
        }
    }

    public class Gear
    {
        public int gearType = 1; // 0 is heroic, 1 is epic
        public string gearTypeStr => gearType == 0 ? "Heroic" : "Epic";
        public int eLevel = 0;
        public int rolls => (eLevel / 3) - ((gearType == 0 && eLevel > 11) ? 1 : 0);
        public int maxRolls => gearType == 0 ? 4 : 5;
        public List<Substat> subs = new List<Substat>();


        public int[] idealRolls = new[] { 0, 0, 0, 0 };
        public int idealIncrease = 0;

        public void CalculateIdealRolls()
        {
            while(idealRolls.Sum() + rolls < maxRolls)
            {
                var maxIncrease = 0;
                var index = 0;
                for(int i = 0; i < subs.Count; i++)
                {
                    var sub = subs[i];
                    int increase = (int)((sub.reforgeValues[sub.rolls + idealRolls[i]] - sub.reforgeValues[sub.rolls + idealRolls[i] - 1] + sub.maxRoll[gearType]) * sub.scoreMulti);
                    if (increase > maxIncrease)
                    {
                        maxIncrease = increase;
                        index = i;
                    }
                }
                idealRolls[index]++;
                idealIncrease += maxIncrease;
            }
            idealIncrease += (gearType == 0 && eLevel < 12) ? 8 : 0; // If its a heroic piece below 12, just add 8. It's prety likely this is the best outcome of a new sub.
        }

        public void AttemptToAssignRollCounts()
        {
            var rollsToDistribute = rolls;
            foreach(var s in subs)
            {
                s.rolls = s.minPotentialRolls(gearType);
                rollsToDistribute -= s.rolls - 1; // This removes the 1 initial roll gear alwways has
            }
            while(rollsToDistribute > 0)
            {
                var likelyTarget = subs.OrderByDescending(x => x.maxPotentialRolls(gearType) - x.rolls).First();
                likelyTarget.rolls++;
                rollsToDistribute--;
            }
        }
        public void SetGearEnhanceFromString(string s)
        {
            int level = 0;
            int.TryParse(s.Trim('+', '\n'), out level);
            eLevel = level;
        }
        public void SetGearTypeFromString(string s)
        {
            var toks = s.Split(' ');
            switch(toks[0])
            {
                case "Heroic":
                    gearType = 0;
                    break;
                default:
                    gearType = 1;
                    break;
            }
        }
        // This shit should probaly be in another file, but fuck it
        // Levenshtein Dist calc from https://www.dotnetperls.com/levenshtein
        public int LevenshteinDist(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0)
                return m;
            if (m == 0)
                return n;
            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            // Begin looping.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }

        private readonly string[] substatNames = { "Attack", "Defense", "Health", "Effectiveness", "Effect Resistance", "Critical Hit Damage", "Critical Hit Chance", "Speed" };
        public void AddSubstatsFromString(string[] toks)
        {
            for(var i = 0; i < toks.Count(); i++)
            {
                var innerToks = toks[i].Split(' ');
                int value = 0;
                int index = 0;
                while (!int.TryParse(innerToks[index].Trim('+', '%'), out value))
                {
                    index++;
                    if (index > innerToks.Length)
                        break;
                }
                if(index > 0)
                {
                    var flatValue = !innerToks[index].Contains('%');
                    var targetSubName = string.Join(" ", innerToks.Take(index));
                    var minDist = int.MaxValue;
                    var subName = string.Empty;
                    foreach(var substatName in substatNames)
                    {
                        var dist = LevenshteinDist(targetSubName, substatName);
                        if(dist < minDist)
                        {
                            minDist = dist;
                            subName = substatName;
                        }
                    }

                    switch (subName)
                    {
                        case "Attack":
                            if (flatValue)
                                subs.Add(new Sub_FlatAttack(value));
                            else
                                subs.Add(new Sub_AttackPercent(value));
                            break;
                        case "Defense":
                            if (flatValue)
                                subs.Add(new Sub_FlatDefense(value));
                            else
                                subs.Add(new Sub_DefensePercent(value));
                            break;
                        case "Health":
                            if (flatValue)
                                subs.Add(new Sub_FlatHealth(value));
                            else
                                subs.Add(new Sub_HealthPercent(value));
                            break;
                        case "Effectiveness":
                            subs.Add(new Sub_Effectiveness(value));
                            break;
                        case "Effect Resistance":
                            subs.Add(new Sub_EffectResistance(value));
                            break;
                        case "Speed":
                            subs.Add(new Sub_Speed(value));
                            break;
                        case "Critical Hit Damage":
                            subs.Add(new Sub_CritDamage(value));
                            break;
                        case "Critical Hit Chance":
                            subs.Add(new Sub_CritChance(value));
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Debug.WriteLine("Error: Failed to parse a substat value");
                }
            }
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var sub in subs)
                sb.AppendLine(sub.ToString());
            sb.AppendLine($"GEARSCORE: {subs.Sum(x => x.gearScoreValue)}");
            return sb.ToString();
        }

        public int gearscore => (int)subs.Sum(x => x.gearScoreValue);
        public int gearscoreReforge => (int)subs.Sum(x => x.gearScoreValReforge);
    }

    #region Substats
    public abstract class Substat 
    {
        public Substat(int val) { value = val; }
        public int value;
        public int valueReforged => value + reforgeValues[rolls - 1];
        public int rolls = 1; // How many rolls have gone into this stat - its going to be a guess.
        public virtual string name => "Substat";
        public virtual float scoreMulti => 1;
        public virtual int[] maxRoll => new[] { 8, 8 };
        public virtual int[] minRoll => new[] { 4, 4 };
        public virtual int[] reforgeValues => new[] { 1, 3, 4, 5, 7, 8 };

        public float maxPotentialRolls(int type) => (float)value / minRoll[type];
        public int minPotentialRolls(int type) => (int)Math.Ceiling(((double)value / maxRoll[type]));
        public int maxPossibleValue(int type) => rolls * maxRoll[type];
        public int minPossibleValue(int type) => rolls * minRoll[type];
        public float gearScoreValue => value * scoreMulti;
        public float maxPossibleGearScoreValue(int type) => maxPossibleValue(type) * scoreMulti;
        public float minPossibleGearScoreValue(int type) => minPossibleValue(type) * scoreMulti;
        public float gearScoreValReforge => valueReforged * scoreMulti;
        public float percentVal(int type) => (value - minPossibleValue(type)) / (float)(maxPossibleValue(type) - minPossibleValue(type)) * 100f;
        public override string ToString() => $"{name} : {value}";
    }
    /* Fribbles GearScore Calc:
     * Score = Attack %
     * + Defense %
     * + Hp %
     * + Effectiveness
     * + Effect Resistance
     * + Speed * (8/4)
     * + Crit Damage * (8/7)
     * + Crit Chance * (8/5)
     * + Flat Attack * 3.46 / 39
     * + Flat Defense * 4.99 / 31
     * + Flat Hp * 3.09 / 174
     */
    public class Sub_AttackPercent : Substat
    {
        public override string name => "Attack %";
        public Sub_AttackPercent(int val) : base(val) { }
    }
    public class Sub_DefensePercent : Substat
    {
        public override string name => "Defense %";
        public Sub_DefensePercent(int val) : base(val) { }
    }
    public class Sub_HealthPercent : Substat
    {
        public override string name => "Health %";
        public Sub_HealthPercent(int val) : base(val) { }
    }
    public class Sub_Effectiveness : Substat
    {
        public override string name => "Effectiveness";
        public Sub_Effectiveness(int val) : base(val) { }
    }
    public class Sub_EffectResistance : Substat
    {
        public override string name => "Effect Res";
        public Sub_EffectResistance(int val) : base(val) { }
    }
    public class Sub_Speed : Substat
    {
        public override string name => "Speed";
        public Sub_Speed(int val) : base(val) { }
        public override float scoreMulti => 2;
        public override int[] maxRoll => new[] { 4, 4 }; // Fuck 5 speed rolls
        public override int[] minRoll => new[] { 1, 1 }; 
        public override int[] reforgeValues => new[] { 0, 1, 2, 3, 4, 4 };
    }
    public class Sub_CritDamage : Substat
    {
        public override string name => "Crit Damage";
        public Sub_CritDamage(int val) : base(val) { }
        public override float scoreMulti => 8f / 7f;
        public override int[] maxRoll => new[] { 7, 7 };
        public override int[] reforgeValues => new[] { 1, 2, 3, 4, 6, 7 };
    }
    public class Sub_CritChance : Substat
    {
        public override string name => "Crit Chance";
        public Sub_CritChance(int val) : base(val) { }
        public override float scoreMulti => 8f / 5f;
        public override int[] maxRoll => new[] { 5, 5 };
        public override int[] minRoll => new[] { 3, 3 };
        public override int[] reforgeValues => new[] { 1, 2, 3, 4, 5, 6 };
    }
    public class Sub_FlatAttack : Substat
    {
        public override string name => "Attack";
        public Sub_FlatAttack(int val) : base(val) { }
        public override float scoreMulti => 3.46f / 39f;
        public override int[] maxRoll => new[] { 44, 46 };
        public override int[] minRoll => new[] { 31, 33 };
        public override int[] reforgeValues => new[] { 11, 22, 33, 44, 55, 66 };
    }
    public class Sub_FlatDefense : Substat
    {
        public override string name => "Defense";
        public Sub_FlatDefense(int val) : base(val) { }
        public override float scoreMulti => 4.99f / 31f;
        public override int[] maxRoll => new[] { 33, 35 };
        public override int[] minRoll => new[] { 26, 28 };
        public override int[] reforgeValues => new[] { 9, 18, 27, 36, 45, 54 };
    }
    public class Sub_FlatHealth : Substat
    {
        public override string name => "Health";
        public Sub_FlatHealth(int val) : base(val) { }
        public override float scoreMulti => 3.09f / 174f;
        public override int[] maxRoll => new[] { 192, 202 };
        public override int[] minRoll => new[] { 149, 157 };
        public override int[] reforgeValues => new[] { 56, 112, 168, 224, 280, 336 };
    }

    #endregion
}
