using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public static class Equation_solver
    {
        public static string TypeEquation()
        {
            var user_input = Console.ReadLine();
            return user_input;
        }
        public static int[] Parsing(string user_input)
        {
            string dec = "-?[0-9]+";
            string eq_pattern = String.Format("^({0})x\\^2\\+({0})x\\+({0})=0", dec);
            
            var matches = Regex.Matches(user_input, eq_pattern);
            string[] result = new string[3];
            result[0] = matches[0].Groups[1].Value;
            result[1] = matches[0].Groups[2].Value;
            result[2] = matches[0].Groups[3].Value;
            int[] coeff = Array.ConvertAll(result, int.Parse);
            return (coeff);
        }
       public static double FindDiscriminant(int[] coeff )
        {
            var discriminant = Math.Pow(coeff[1], 2) - 4 * coeff[0] * coeff[2];

            return discriminant;
        }
        public static string  ValidateDiscriminant(int discriminant)
        {
            if (discriminant < 0)
            {
                return ("Данное уравнение не имеет решений в области вещественных чисел");
            }
            
                return default(string);

        }
        public static double FindLeftRoot(int discriminant, int[] coeff)
        {
            var left_root =(-coeff[1] - Math.Sqrt(discriminant)) / (2 * coeff[0]);
            return left_root;
        }
        public static double FindRightRoot( int discriminant,int[] coeff)
        {
            var right_root =(-coeff[1] + Math.Sqrt(discriminant)) / (2 * coeff[0]);
            return right_root;
        }

        public static string ShowResult(double left_root,double right_root)
        {
            string output = String.Format("Первый корень x1={0:f} Второй корень x2={1:f}", left_root, right_root);
            return output;
        }
    }
}
