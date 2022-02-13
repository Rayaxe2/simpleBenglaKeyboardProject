using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using simpleBenglaKeyboardProject;
using System.Linq;

namespace simpleBenglaKeyboardProject
{
    //Tutorial: https://www.youtube.com/watch?v=t8oVVPAOcZA
    public partial class Form1 : Form
    {
        //CreateParams isn't explained too well/in laymen's terms on the internet/on microsoft's online documentation.
        //To my understanding, the CreateParams object holds information/parameters that are/is used to build a control (e.g. a button of the form)
        //Such as it's size (height and width), possition and style (as can be seen in the class definiation). You can usually set these
        //properties via [control].property name, but in this case, we want to set something that we can't set with the control object.
        //We're overiding the CreateParams's constructor setting the "ExStyle" property to 0x08000000 (WS_EX_NOACTIVATE) so the window doesn't take
        //focus from the application we want to type to. Wihtout this, when we click on a button on this application, the active window will be this
        //application and the input we're trying to send when the button is clicked will be sent to to application, since it will be in focus.
        //Clicking On the application we want to type to will put this applicaiton out of focus and to use this applicaiton we would need to click on
        //The input we want to type and that will put it back in focus and send the input to itself as it will be put back in focus before
        //The button's action is executed. Therefor we need this for the applicaiton to be created like an out of focus overlay that we
        //Can still interact with while another applicaiton is in focus.
        //https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles
        //https://stackoverflow.com/questions/25051552/create-a-window-using-the-ws-ex-noactivate-flag-but-it-cant-be-dragged-until-i
        //https://docs.microsoft.com/en-gb/windows/win32/winmsg/window-styles?redirectedfrom=MSDN
        //https://docs.microsoft.com/en-gb/windows/win32/winmsg/window-class-styles?redirectedfrom=MSDN
        //https://stackoverflow.com/questions/156046/show-a-form-without-stealing-focus
        //https://youtu.be/t8oVVPAOcZA?t=873

        //MS Docs: https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.createparams?view=windowsdesktop-6.0#:~:text=A%20CreateParams%20that%20contains%20the,to%20the%20control%20is%20created
        //Note: As stated in the MS docs:
        //The CreateParams property should not be overridden and used to adjust the properties of your derived control.
        //Properties such as the CreateParams.Caption, CreateParams.Width, and CreateParams.Height should be set by
        //the corresponding properties in your control such as Control.Text, Control.Width and Control.Height.
        //The CreateParams should only be extended when you are wrapping a standard Windows control class or to set styles not provided by the Windows Forms namespace.

        //Source of explanation: https://www.oreilly.com/library/view/net-windows-forms/0596003382/re354.html
        //This class wraps the set of parameters passed to a Win32 window in its CreateWindow() or CreateWindowEx() function.
        //If you are wrapping a Win32 control with your own managed Control, you can override the Control.

        //Source of explanation: https://www.vbforums.com/showthread.php?831919-how-to-use-CreateParams
        //CreateParams is a virtual property. That means you're able to augment its behavior with inheritance by using the Overrides keyword.
        //That tells VB you'd like your class to have a special implementation that does more than the default implementation.
        //To override it, pick a blank part of your Form's file and type "Overrides" and a space. You'll get a list of the form's virtual members.
        //Type "CreateParams" and it ought to be highlighted. If not, you're not in a form. If so, press Enter.
        //You'll get something like this generated (something like the code bellow).
        //The code doesn't do anything the base doesn't do. In fact, it just asks the base property for its value and returns that.
        //If you want to modify the value, you have to do something with it. The property returns a CreateParams class.
        //You should modify the one the base class returns. 
        protected override CreateParams CreateParams {
            //Note: The protected access modifier give access to anything within the class it's declared int and lets objects
            //of classes that inherited the class it's delcared in access it (objects of the class it declared it can't access it)
            //The attributed can also be accessed within the classes that inherited it
            //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/protected
            get
            {
                CreateParams param = base.CreateParams;
                param.ExStyle |= 0x08000000;
                // x|=y is basically x=x|(y) (x OR y) - so if x is 1001 (binary) and y is 0101 (binary) then x will equal 1101
                //This is a bitwise oprator so evaluates the binary values of the operands so will 1 & 2 would be false
                //(0001 AND 0001 is 0000), 1 && 2 would be true (1 and 2 both evalutate to true - true AND true is true - though, you'd
                //not be able to evaluate ints with && in a langauge like c, it only takes boolean values) 
                //In C# 23 (010111) & 8 (001000) would be 0 (000000). 23 (010111) & 5 (000101) would be 5 (000101).
                //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#compound-assignment
                //https://stackoverflow.com/questions/6942477/what-does-single-pipe-equal-and-single-ampersand-equal-mean
                return param;
            }
        }

        private List<junktoComboStruct> allJunktoCombos_Results;
        private string currentCombo = "";
        private int maxJuktoSlots = 19;
        private int juktoSugguestionSlotsUsed = 0;
        private List<Button> sugguestionButtonArray = new List<Button>();

        private bool juktoMode = false;

        public Form1()
        {
            InitializeComponent();
            this.TopMost = true; //Keeps the window/form on top/in the foreground, even if you click away from it

            //Possitions keyboard at the bottom of the screen
            this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height); //If task bar is at the bottom, this puts it above the task bar
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.Bounds.Height - this.Height); //This puts it right at the bottom, where the taskbar starts by default

            //Array of Junktos from https://writebangla.com/juktoborno.html
            string[] junktoArray = { "ক্ক = ক+ক ", "ক্ট = ক+ট ", "ক্ট্র = ক+ট+র ", "ক্ত = ক+ত ", "ক্ত্র = ক+ত+র ", "ক্ব = ক+ব ", "ক্ম = ক+ম ", "ক্য = ক+য ", "ক্র = ক+র  ", "ক্ল = ক+ল ", "ক্ষ = ক+ষ ", "ক্ষ্ণ = ক+ষ+ণ ", "ক্ষ্ব = ক+ষ+ব ", "ক্ষ্ম = ক+ষ+ম ", "ক্ষ্ম্য=ক+ষ+ম+য ", "ক্ষ্য = ক+ষ+য ", "ক্স = ক+স ", "খ্য = খ+য ", "খ্র = খ+ র ", "গ্‌ণ = গ+ণ ", "গ্ধ = গ+ধ ", "গ্ধ্য = গ+ধ+য ", "গ্ধ্র = গ+ধ+র ", "গ্ন = গ+ন ", "গ্ন্য = গ+ন+য ", "গ্ব = গ+ব ", "গ্ম = গ+ম ", "গ্য = গ+য ", "গ্র = গ+র ", "গ্র্য = গ+র+য ", "গ্ল = গ+ল ", "ঘ্ন = ঘ+ন ", "ঘ্য = ঘ+য ", "ঘ্র = ঘ+র ", "ঙ্ক = ঙ+ক ", "ঙ্‌ক্ত = ঙ+ক+ত", "ঙ্ক্য = ঙ+ক+য ", "ঙ্ক্ষ = ঙ+ক+ষ ", "ঙ্খ = ঙ+খ ", "ঙ্গ = ঙ+গ ", "ঙ্গ্য = ঙ+গ+য ", "ঙ্ঘ = ঙ+ঘ ", "ঙ্ঘ্য = ঙ+ঘ+য ", "ঙ্ঘ্র = ঙ+ঘ+র ", "ঙ্ম = ঙ+ম ", "চ্চ = চ+চ ", "চ্ছ = চ+ছ ", "চ্ছ্ব = চ+ছ+ব ", "চ্ছ্র = চ+ছ+র ", "চ্ঞ = চ+ঞ ", "চ্ব = চ+ব ", "চ্য = চ+য ", "জ্জ = জ+জ ", "জ্জ্ব = জ+জ+ব ", "জ্ঝ = জ+ঝ ", "জ্ঞ = জ+ঞ ", "জ্ব = জ+ব ", "জ্য = জ+য ", "জ্র = জ+র ", "ঞ্চ = ঞ+চ ", "ঞ্ছ = ঞ+ছ ", "ঞ্জ = ঞ+জ ", "ঞ্ঝ = ঞ+ঝ ", "ট্ট = ট+ট ", "ট্ব = ট+ব ", "ট্ম = ট+ম ", "ট্য = ট+য ", "ট্র = ট+র ", "ড্ড = ড+ড  ", "ড্ব = ড+ব ", "ড্য = ড+য ", "ড্র = ড+র ", "ড়্গ = ড়+গ ", "ঢ্য = ঢ+য ", "ঢ্র = ঢ+র ", "ণ্ট = ণ+ট ", "ণ্ঠ = ণ+ঠ ", "ণ্ঠ্য = ণ+ঠ+য ", "ণ্ড = ণ+ড ", "ণ্ড্য = ণ+ড+য ", "ণ্ড্র = ণ+ড+র ", "ণ্ঢ = ণ+ঢ ", "ণ্ণ = ণ+ণ ", "ণ্ব = ণ+ব ", "ণ্ম = ণ+ম ", "ণ্য = ণ+য ", "ৎক = ত+ক ", "ত্ত = ত+ত ", "ত্ত্ব = ত+ত+ব ", "ত্ত্য = ত+ত+য ", "ত্থ = ত+থ ", "ত্ন = ত+ন ", "ত্ব = ত+ব ", "ত্ম = ত+ম ", "ত্ম্য = ত+ম+য ", "ত্য = ত+য ", "ত্র = ত+র ", "ত্র্য = ত+র+য ", "ৎল = ত+ল ", "ৎস = ত+স ", "থ্ব = থ+ব ", "থ্য = থ+য ", "থ্র = থ+র ", "দ্গ = দ+গ ", "দ্ঘ = দ+ঘ ", "দ্দ = দ+দ ", "দ্দ্ব = দ+দ+ব ", "দ্ধ = দ+ধ ", "দ্ব = দ+ব ", "দ্ভ = দ+ভ ", "দ্ভ্র = দ+ভ+র ", "দ্ম = দ+ম ", "দ্য = দ+য ", "দ্র = দ+র ", "দ্র্য = দ+র+য ", "ধ্ন = ধ+ন ", "ধ্ব = ধ+ব ", "ধ্ম = ধ+ম ", "ধ্য = ধ+য ", "ধ্র = ধ+র ", "ন্ট = ন+ট ", "ন্ট্র = ন+ট+র ", "ন্ঠ = ন+ঠ ", "ন্ড = ন+ড ", "ন্ড্র = ন+ড+র ", "ন্ত = ন+ত ", "ন্ত্ব = ন+ত+ব ", "ন্ত্য = ন+ত+য ", "ন্ত্র = ন+ত+র ", "ন্ত্র্য =ন+ত+র+য ", "ন্থ = ন+থ ", "ন্থ্র = ন+থ+র ", "ন্দ = ন+দ ", "ন্দ্য = ন+দ+য ", "ন্দ্ব = ন+দ+ব ", "ন্দ্র = ন+দ+র ", "ন্ধ = ন+ধ ", "ন্ধ্য = ন+ধ+য ", "ন্ধ্র = ন+ধ+র ", "ন্ন = ন+ন ", "ন্ব = ন+ব ", "ন্ম = ন+ম ", "ন্য = ন+য ", "প্ট = প+ট ", "প্ত = প+ত ", "প্ন = প+ন ", "প্প = প+প ", "প্য = প+য ", "প্র = প+র ", "প্র্য = প+র+য ", "প্ল = প+ল ", "প্স = প+স ", "ফ্র = ফ+র ", "ফ্ল = ফ+ল ", "ব্জ = ব+জ ", "ব্দ = ব+দ ", "ব্ধ = ব+ধ ", "ব্ব = ব+ব ", "ব্য = ব+য ", "ব্র = ব+র ", "ব্ল = ব+ল ", "ভ্ব =ভ+ব ", "ভ্য = ভ+য ", "ভ্র = ভ+র ", "ম্ন = ম+ন ", "ম্প = ম+প ", "ম্প্র = ম+প+র ", "ম্ফ = ম+ফ ", "ম্ব = ম+ব ", "ম্ব্র = ম+ব+র  ", "ম্ভ = ম+ভ ", "ম্ভ্র = ম+ভ+র ", "ম্ম = ম+ম ", "ম্য = ম+য ", "ম্র = ম+র ", "ম্ল = ম+ল ", "য্য = য+য ", "র্ক = র+ক ", "র্ক্য = র+ক+য ", "র্গ্য = র+গ+য ", "র্ঘ্য = র+ঘ+য ", "র্চ্য = র+চ+য ", "র্জ্য = র+জ+য ", "র্ণ্য = র+ণ+য ", "র্ত্য = র+ত+য ", "র্থ্য = র+থ+য ", "র্ব্য = র+ব+য ", "র্ম্য = র+ম+য ", "র্শ্য = র+শ+য ", "র্ষ্য = র+ষ+য ", "র্হ্য = র+হ+য ", "র্খ = র+খ ", "র্গ = র+গ ", "র্গ্র = র+গ+র ", "র্ঘ = র+ঘ ", "র্চ = র+চ ", "র্ছ = র+ছ ", "র্জ = র+জ ", "র্ঝ = র+ঝ ", "র্ট = র+ট ", "র্ড = র+ড ", "র্ণ = র+ণ ", "র্ত = র+ত ", "র্ত্র = র+ত+র ", "র্থ = র+থ ", "র্দ = র+দ ", "র্দ্ব = র+দ+ব ", "র্দ্র = র+দ+র ", "র্ধ = র+ধ ", "র্ধ্ব = র+ধ+ব ", "র্ন = র+ন ", "র্প = র+প ", "র্ফ = র+ফ ", "র্ভ = র+ভ ", "র্ম = র+ম ", "র্য = র+য ", "র্ল = র+ল ", "র্শ = র+শ ", "র্শ্ব = র+ শ+ব ", "র্ষ = র+ষ ", "র্স = র+স ", "র্হ = র+হ ", "র্ঢ্য = র+ঢ+য ", "ল্ক = ল+ক", "ল্ক্য = ল+ক+য ", "ল্গ = ল+গ ", "ল্ট = ল+ট ", "ল্ড = ল+ড ", "ল্প = ল+প ", "ল্‌ফ = ল+ফ ", "ল্ব = ল+ব ", "ল্‌ভ = ল+ভ ", "ল্ম = ল+ম ", "ল্য = ল+য ", "ল্ল = ল+ল ", "শ্চ = শ+চ ", "শ্ছ = শ+ছ ", "শ্ন = শ+ন ", "শ্ব = শ+ব ", "শ্ম = শ+ম ", "শ্য = শ+য ", "শ্র = শ+র ", "শ্ল = শ+ল ", "ষ্ক = ষ+ক ", "ষ্ক্র = ষ+ক+র ", "ষ্ট = ষ+ট ", "ষ্ট্য = ষ+ট+য ", "ষ্ট্র = ষ+ট+র ", "ষ্ঠ = ষ+ঠ ", "ষ্ঠ্য = ষ+ঠ+য ", "ষ্ণ = ষ+ণ ", "ষ্প = ষ+প ", "ষ্প্র = ষ+প+র ", "ষ্ফ = ষ+ফ ", "ষ্ব = ষ+ব ", "ষ্ম = ষ+ম ", "ষ্য = ষ+য ", "স্ক = স+ক  ", "স্ক্র = স+ক্র ", "স্খ = স+খ", "স্ট = স+ট ", "স্ট্র = স+ট্র ", "স্ত = স+ত ", "স্ত্ব = স+ত+ব ", "স্ত্য = স+ত+য ", "স্ত্র = স+ত+র ", "স্থ = স+থ ", "স্থ্য = স+থ+য ", "স্ন = স+ন ", "স্প = স+প ", "স্প্র = স+প +র ", "স্প্‌ল = স+প+ল ", "স্ফ = স+ফ ", "স্ব = স+ব ", "স্ম = স+ম ", "স্য = স+য ", "স্র = স+র ", "স্ল = স+ল ", "হ্ণ = হ+ণ ", "হ্ন = হ+ন ", "হ্ব = হ+ব ", "হ্ম = হ+ম ", "হ্য = হ+য ", "হ্র = হ+র ", "হ্ল = হ+ল ", "হৃ = হ+ৃ  " };
            //Spliting array of junktos into logical self-created structure 
            List<junktoComboStruct> junktoCombos_Results = new List<junktoComboStruct>();

            string postSpiltComboResult;
            string[] postSplitComboArray;

            foreach (string junktoCombo in junktoArray)
            {
                postSpiltComboResult = junktoCombo.Split("=")[0].Replace(" ", "");
                postSplitComboArray = junktoCombo.Split("=")[1].Replace(" ", "").Split("+");

                junktoCombos_Results.Add(new junktoComboStruct(postSplitComboArray, postSpiltComboResult));
            }

            allJunktoCombos_Results = junktoCombos_Results;
        }

        private void processUserInput(string input)
        {
            int matchedChars = 0;

            //Removes old buttons
            foreach (Button oldButton in sugguestionButtonArray)
            {
                this.juktoSugguestionPanel.Controls.Remove(oldButton);
            }
            juktoSugguestionSlotsUsed = 0;

            //If jukto mode is active, jukto words are searched for and sugguestions are rendered
            if (juktoMode) {
                currentCombo += input;
                List<string> sugguestion = new List<string>();
                //Creates a list of sugguestions to make buttons with
                foreach (junktoComboStruct combo_comboResult in allJunktoCombos_Results)
                {
                    matchedChars = 0;
                    foreach (char benglaChar in currentCombo)
                    {

                        if ((combo_comboResult.combo.Length > matchedChars) && (combo_comboResult.combo[matchedChars] == char.ToString(benglaChar)))
                        {
                            matchedChars += 1;
                        }
                        else
                        {
                            matchedChars = 0;
                            break;
                        }

                        if (matchedChars == currentCombo.Length)
                        {
                            sugguestion.Add(combo_comboResult.comboResult);
                        }
                    }
                }

                int counter = 0;
                int xLocation = -61;
                int xPosIncrement = 61;
                Button juktoSugguestionBtn;

                //Creates buttons that output sugguestions and adds them to the sugguestion panel bar
                foreach (string banglaCharSuggestion in sugguestion)
                {
                    counter += 1;
                    juktoSugguestionBtn = new Button();
                    juktoSugguestionBtn.Font = new Font(juktoSugguestionBtn.Font.Name, 16);
                    juktoSugguestionBtn.Name = "Sugguestion_" + counter;
                    juktoSugguestionBtn.Size = new Size(60, 60);
                    juktoSugguestionBtn.Location = new Point((xLocation + xPosIncrement), 0);
                    xLocation += xPosIncrement;
                    juktoSugguestionBtn.Text = banglaCharSuggestion;
                    juktoSugguestionBtn.BackColor = Color.Crimson;
                    juktoSugguestionBtn.Visible = true;
                    juktoSugguestionBtn.Click += new EventHandler(
                        (s, e) =>
                        {
                            for (int i = 0; i < currentCombo.Length; i++) 
                            {
                                SendKeys.Send("{BACKSPACE}");
                            }
                            SendKeys.Send(banglaCharSuggestion);
                            foreach (Button oldButton in sugguestionButtonArray)
                            {
                                this.juktoSugguestionPanel.Controls.Remove(oldButton);
                            }
                            this.juktoButton.PerformClick();
                        }
                    );
                    sugguestionButtonArray.Add(juktoSugguestionBtn);
                    if (juktoSugguestionSlotsUsed <= maxJuktoSlots) {
                        this.juktoSugguestionPanel.Controls.Add(juktoSugguestionBtn);
                        juktoSugguestionSlotsUsed += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                if (sugguestion.Count == 0) 
                {
                    this.juktoButton.PerformClick();
                }
            }
            SendKeys.Send(input);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void shoonnoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("০");
        }

        private void shoonnoKey_MouseEnter(object sender, EventArgs e)
        {
            shoonnoKey.BackColor = SystemColors.ButtonHighlight;
            shoonnoKey.ForeColor = SystemColors.GradientInactiveCaption;
        }

        private void ekKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("১");
        }

        private void duiKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("২");
        }

        private void tinKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("৩");
        }

        private void chaaKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("৪");
        }

        private void paanchKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("৫");
        }

        private void chchoyKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("৬");
        }

        private void shatKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("৭");
        }

        private void atKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("৮");
        }

        private void noyKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("৯");
        }

        private void takaKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("৳");
        }

        private void zeroKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("0");
        }

        private void oneKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("1");
        }

        private void twoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("2");
        }

        private void threeKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("3");
        }

        private void fourKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("4");
        }

        private void fiveKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("5");
        }

        private void sixKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("6");
        }

        private void sevenKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("7");
        }

        private void eightKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("8");
        }

        private void nineKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("9");
        }

        private void aKey_Click(object sender, EventArgs e)
        {
            processUserInput("অ");
        }

        private void aaKey_Click(object sender, EventArgs e)
        {
            processUserInput("আ");
        }

        private void iKey_Click(object sender, EventArgs e)
        {
            processUserInput("ই");
        }

        private void iiKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঈ");
        }

        private void uKey_Click(object sender, EventArgs e)
        {
            processUserInput("উ");
        }

        private void uuKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঊ");
        }

        private void riKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঋ");
        }

        private void riiKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৠ");
        }

        private void liKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঌ");
        }

        private void liiKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৡ");
        }

        private void eKey_Click(object sender, EventArgs e)
        {
            processUserInput("এ");
        }

        private void oiKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঐ");
        }

        private void oKey_Click(object sender, EventArgs e)
        {
            processUserInput("ও");
        }

        private void ouKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঔ");
        }

        private void koKey_Click(object sender, EventArgs e)
        {
            processUserInput("ক");
        }

        private void khoKey_Click(object sender, EventArgs e)
        {
            processUserInput("খ");
        }

        private void goKey_Click(object sender, EventArgs e)
        {
            processUserInput("গ");
        }

        private void ghoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঘ");
        }

        private void ngoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঙ");
        }

        private void backSpaceKey_Click(object sender, EventArgs e)
        {
            currentCombo = currentCombo.Remove(currentCombo.Length - 1, 1);
            SendKeys.Send("{BACKSPACE}"); 
        }

        private void coKey_Click(object sender, EventArgs e)
        {
            processUserInput("চ");
        }

        private void choKey_Click(object sender, EventArgs e)
        {
            processUserInput("ছ");
        }

        private void joKey_Click(object sender, EventArgs e)
        {
            processUserInput("জ");
        }

        private void jhoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঝ");
        }

        private void nnooKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঞ");
        }

        private void toKey_Click(object sender, EventArgs e)
        {
            processUserInput("ট");
        }

        private void thoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঠ");
        }

        private void doKey_Click(object sender, EventArgs e)
        {
            processUserInput("ড");
        }

        private void dhoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঢ");
        }

        private void noKey_Click(object sender, EventArgs e)
        {
            processUserInput("ণ");
        }

        private void ttoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ত");
        }

        private void tthoKey_Click(object sender, EventArgs e)
        {
            processUserInput("থ");
        }

        private void ddoKey_Click(object sender, EventArgs e)
        {
            processUserInput("দ");
        }

        private void ddhoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ধ");
        }

        private void nnoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ন");
        }

        private void poKey_Click(object sender, EventArgs e)
        {
            processUserInput("প");
        }

        private void phoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ফ");
        }

        private void boKey_Click(object sender, EventArgs e)
        {
            processUserInput("ব");
        }

        private void bhoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ভ");
        }

        private void moKey_Click(object sender, EventArgs e)
        {
            processUserInput("ম");
        }

        private void yyoKey_Click(object sender, EventArgs e)
        {
            processUserInput("য");
        }

        private void rroKey_Click(object sender, EventArgs e)
        {
            processUserInput("র");
        }

        private void loKey_Click(object sender, EventArgs e)
        {
            processUserInput("ল");
        }

        private void ssoKey_Click(object sender, EventArgs e)
        {
            processUserInput("শ");
        }

        private void soKey_Click(object sender, EventArgs e)
        {
            processUserInput("ষ");
        }

        private void ssooKey_Click(object sender, EventArgs e)
        {
            processUserInput("স");
        }

        private void hoKey_Click(object sender, EventArgs e)
        {
            processUserInput("হ");
        }

        private void yoKey_Click(object sender, EventArgs e)
        {
            processUserInput("য়");
        }

        private void roKey_Click(object sender, EventArgs e)
        {
            processUserInput("ড়");
        }

        private void rhoKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঢ়");
        }

        private void raKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৰ");
        }

        private void raaKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৱ");
        }

        private void anusvaraModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ং");
        }

        private void visargaModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঃ");
        }

        private void aaVowelSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("া");
        }

        private void signTwoModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ী");
        }

        private void signOneModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ি");
        }

        private void eSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ে");
        }

        private void aiSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৈ");
        }

        private void oSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ো");
        }

        private void auSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৌ");
        }

        private void auTwoModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৗ");
        }

        private void candravinduSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঁ");
        }

        private void naktaSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("়");
        }

        private void vimraSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("্");
        }

        private void uSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ু");
        }

        private void uuSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ূ");
        }

        private void rSoundModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৃ");
        }

        private void rrSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৄ");
        }

        private void lSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৢ");
        }

        private void llSignModKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৣ");
        }

        private void spaceKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send(" ");
            currentCombo = "";
            juktoMode = false;
            this.juktoButton.BackColor = Color.DarkBlue;
        }

        private void taKey_Click(object sender, EventArgs e)
        {
            processUserInput("ৎ");
        }

        private void avagrahaKey_Click(object sender, EventArgs e)
        {
            processUserInput("ঽ");
        }

        private void commaKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send(",");
            juktoMode = false;
            this.juktoButton.BackColor = Color.DarkBlue;
        }

        private void fullStopKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send(".");
            juktoMode = false;
            this.juktoButton.BackColor = Color.DarkBlue;
        }

        private void questionMarkKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("?");
            juktoMode = false;
            this.juktoButton.BackColor = Color.DarkBlue;
        }

        private void closeKey_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void juktoSugguestionPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void juktoButton_Click(object sender, EventArgs e)
        {
            currentCombo = "";
            if (juktoMode == false) {
                juktoMode = true;
                this.juktoButton.BackColor = Color.DarkCyan;
            }
            else {
                juktoMode = false;
                this.juktoButton.BackColor = Color.DarkBlue;
            }
        }
    }
}