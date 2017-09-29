using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary1;

namespace Test_equation_solver
{
    [TestClass]
    public class Test_equation_solver
    {
        [TestMethod]
        public void Parsing_inputToArray_equal()
        {
            string input ="1x^2+4x+3=0";
            var coeffTest = new int[3] { 1, 4, 3 };
            var coeffFromMethod = Equation_solver.Parsing(input);
            for (int i = 0; i < 3; i++) {
                Assert.AreEqual(coeffFromMethod[i], coeffTest[i]);
            }
        }
        [TestMethod]
        public void Check_discriminant()
        {
            var coeff = new int[3] { 1, 4, 3 };
            var discriminantTest = Math.Pow(coeff[1], 2) - 4 * coeff[0] * coeff[2];
            var discriminantFromMethod = Equation_solver.FindDiscriminant(coeff);

            Assert.AreEqual(discriminantTest, discriminantFromMethod);
        }
        [TestMethod]
        public void ValidateDiscriminant_negative_NoRoots()
        {
            var discriminant = -3;
            var output_errorTest = "Данное уравнение не имеет решений в области вещественных чисел";
            var output_errorFromMethod = Equation_solver.ValidateDiscriminant(discriminant);
            Assert.AreEqual(output_errorTest, output_errorFromMethod);

        }

        [TestMethod]
        public void Check_left_root()
        {
            var discriminant = 4;
            var coeff = new int[2] { 1, 4 };
            var rootTest = -3;
            var rootFromMethod = Equation_solver.FindLeftRoot(discriminant, coeff);
            Assert.AreEqual(rootTest, rootFromMethod);
        }
        [TestMethod]
        public void Check_right_root()
        {
            var discriminant = 4;
            var coeff = new int[2] { 1, 4 };
            var rootTest =-1;
            var rootFromMethod = Equation_solver.FindRightRoot(discriminant, coeff);
            Assert.AreEqual(rootTest, rootFromMethod);


        }
        [TestMethod]
        public void Result_RootsToOutput_equal()
        {
            double left_root = -3;
            double right_root = -1;
            string outputTest = String.Format("Первый корень x1={0:f} Второй корень x2={1:f}",left_root,right_root);
            var outputFromMethod =Equation_solver.ShowResult(left_root,right_root);
            Assert.AreEqual(outputTest, outputFromMethod);
        }

    }
}