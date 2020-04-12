using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RevitFamilyComparer
{
    public class FamilyCharacteristic
    {
        public string Name;
        public object Characteristic;

        public FamilyCharacteristic(string name, object famChar)
        {
            Name = name;
            Characteristic = famChar;
        }
    }
}
