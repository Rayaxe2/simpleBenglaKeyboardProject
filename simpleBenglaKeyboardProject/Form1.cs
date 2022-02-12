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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendKeys.Send("কঁ");
        }
    }
}
