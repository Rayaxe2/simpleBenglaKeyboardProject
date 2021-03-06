using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public Form1()
        {
            InitializeComponent();
            this.TopMost = true; //Keeps the window/form on top/in the foreground, even if you click away from it

            //Possitions keyboard at the bottom of the screen
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void shoonnoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("০");
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
            SendKeys.Send("অ");
        }

        private void aaKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("আ");
        }

        private void iKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ই");
        }

        private void iiKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঈ");
        }

        private void uKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("উ");
        }

        private void uuKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঊ");
        }

        private void riKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঋ");
        }

        private void riiKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৠ");
        }

        private void liKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঌ");
        }

        private void liiKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৡ");
        }

        private void eKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("এ");
        }

        private void oiKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঐ");
        }

        private void oKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ও");
        }

        private void ouKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঔ");
        }

        private void koKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ক");
        }

        private void khoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("খ");
        }

        private void goKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("গ");
        }

        private void ghoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঘ");
        }

        private void ngoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঙ");
        }

        private void backSpaceKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{BACKSPACE}"); 
        }

        private void coKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("চ");
        }

        private void choKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ছ");
        }

        private void joKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("জ");
        }

        private void jhoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঝ");
        }

        private void nnooKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঞ");
        }

        private void toKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ট");
        }

        private void thoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঠ");
        }

        private void doKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ড");
        }

        private void dhoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঢ");
        }

        private void noKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ণ");
        }

        private void ttoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ত");
        }

        private void tthoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("থ");
        }

        private void ddoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("দ");
        }

        private void ddhoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ধ");
        }

        private void nnoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ন");
        }

        private void poKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("প");
        }

        private void phoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ফ");
        }

        private void boKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ব");
        }

        private void bhoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ভ");
        }

        private void moKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ম");
        }

        private void yyoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("য");
        }

        private void rroKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("র");
        }

        private void loKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ল");
        }

        private void ssoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("শ");
        }

        private void soKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ষ");
        }

        private void ssooKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("স");
        }

        private void hoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("হ");
        }

        private void yoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("য়");
        }

        private void roKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ড়");
        }

        private void rhoKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঢ়");
        }

        private void raKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৰ");
        }

        private void raaKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৱ");
        }

        private void anusvaraModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ং");
        }

        private void visargaModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঃ");
        }

        private void aaVowelSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("া");
        }

        private void signTwoModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ী");
        }

        private void signOneModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ি");
        }

        private void eSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ে");
        }

        private void aiSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৈ");
        }

        private void oSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ো");
        }

        private void auSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৌ");
        }

        private void auTwoModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৗ");
        }

        private void candravinduSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঁ");
        }

        private void naktaSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("়");
        }

        private void vimraSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("্");
        }

        private void uSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ু");
        }

        private void uuSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ূ");
        }

        private void rSoundModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৃ");
        }

        private void rrSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৄ");
        }

        private void lSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৢ");
        }

        private void llSignModKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৣ");
        }

        private void spaceKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send(" "); 
        }

        private void taKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ৎ");
        }

        private void avagrahaKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("ঽ");
        }

        private void commaKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send(",");
        }

        private void fullStopKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send(".");
        }

        private void questionMarkKey_Click(object sender, EventArgs e)
        {
            SendKeys.Send("?");
        }

        private void closeKey_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}