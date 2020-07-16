using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Training_1.Steps
{
    class MiscFunctions
    {
        public MiscFunctions()
        { 
            //empty constr;
        }

        public string GenerateRandomString(int numberOfLetters)
        {
            var randomString = new StringBuilder();
            char letterToBeAdded;
            Random rand = new Random();

            while (numberOfLetters > 0)
            {
                letterToBeAdded = (char)rand.Next(65, 91);
                randomString.Append(letterToBeAdded);
                numberOfLetters--;
            }

            return randomString.ToString();
        }
    }
}
