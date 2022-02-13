using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simpleBenglaKeyboardProject
{
    class junktoComboStruct
    {
        public string comboResult { get; }
        public string[] combo { get; }

        public junktoComboStruct(string[] comboArray, string result) {
            comboResult = result;
            combo = comboArray;
        }
    }
}
