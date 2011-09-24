using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Core.Filters.Frequency
{
    /// <summary>
    /// Implementation of a complex number.
    /// </summary>
    public class Complex
    {
        #region Attributes

        /// <summary>
        /// The real and imaginary parts of the complex number.
        /// </summary>
        public double real, imag;

        #endregion

        #region Constructors

        /// <summary>
        /// New complex number
        /// </summary>
        /// <param name="iReal">the real part</param>
        /// <param name="iImag">the imaginary part</param>
        public Complex(double iReal, double iImag)
        {
            real = iReal;
            imag = iImag;
        }

        #endregion

        #region Operations

        /// <summary>
        /// Power spectrum / Spectral density of a complex number, as used by Gonzalez and Woods (4.2-12)
        /// </summary>
        /// <returns>real * real + imag * imag</returns>
        public double PowerSpectrum()
        {
            return real * real + imag * imag;
        }

        /// <summary>
        /// Magnitude / Spectrum of a complex number, as used by Gonzalez and Woods (4.2-10)
        /// </summary>
        /// <returns>Math.Sqrt(real * real + imag * imag)</returns>
        public double Magnitude()
        {
            return Math.Sqrt(real * real + imag * imag);
        }

        /// <summary>
        /// Phase angle / Phase spectrum of a complex number, as used by Gonzalez and Woods (4.2-11)
        /// </summary>
        /// <returns>Math.Atan(imag / real)</returns>
        public double Phase()
        {
            return Math.Atan(imag / real);
        }

        /// <summary>
        /// Conjugate of complex number
        /// </summary>
        /// <returns>new Complex(real, -1 * imag)</returns>
        public Complex Conjugate()
        {
            return new Complex(real, -1 * imag);
        }

        /// <summary>
        /// Clones this complex number.
        /// </summary>
        /// <returns></returns>
        public Complex Clone()
        {
            return new Complex(this.real, this.imag);
        }

        #region Arithmetic Operations
        // Aritmetic Operations Complex <-> Complex and Complex <-> double

        #region multiplication

        public static Complex operator *(double op1, Complex op2)
        {
            return new Complex(op1, 0) * op2;
        }

        public static Complex operator *(Complex op1, double op2)
        {
            return op2 * op1;
        }

        public static Complex operator *(Complex op1, Complex op2)
        {
            if (null == op1 || null == op2)
                return null;

            // Algebra Linear com Aplicacoes, 8 Ed. & Wikipedia
            // (a+bi)(c+di) = (ac-bd) + (ad+bc)i
            return new Complex(
                    op1.real * op2.real - op1.imag * op2.imag,
                    op1.real * op2.imag + op1.imag * op2.real);
        }

        #endregion

        #region division

        public static Complex operator /(double op1, Complex op2)
        {
            return new Complex(op1, 0) / op2;
        }

        public static Complex operator /(Complex op1, double op2)
        {
            return new Complex(op2, 0) / op1;
        }

        public static Complex operator /(Complex op1, Complex op2)
        {
            if (null == op1 || null == op2)
                return null;

            double denom = op2.real * op2.real + op2.imag * op2.imag;
            if (denom == 0.0)
            {
                return new Complex(0.0, 0.0);
            }

            // Wikipedia
            return new Complex(
                    (op1.real * op2.real + op1.imag * op2.imag) / denom,
                    (op1.real * op2.imag - op1.imag * op2.real) / denom);
        }

        #endregion

        #region addition

        public static Complex operator +(double op1, Complex op2)
        {
            return new Complex(op1, 0) + op2;
        }

        public static Complex operator +(Complex op1, double op2)
        {
            return new Complex(op2, 0) + op1;
        }

        public static Complex operator +(Complex op1, Complex op2)
        {
            if (null == op1 || null == op2)
                return null;

            return new Complex(op1.real + op2.real, op1.imag + op2.imag);
        }

        #endregion

        #region subtraction

        public static Complex operator -(double op1, Complex op2)
        {
            return new Complex(op1, 0) - op2;
        }

        public static Complex operator -(Complex op1, double op2)
        {
            return new Complex(op2, 0) - op1;
        }

        public static Complex operator -(Complex op1, Complex op2)
        {
            if (null == op1 || null == op2)
                return null;

            return new Complex(op1.real - op2.real, op1.imag - op2.imag);
        }

        #endregion

        #endregion

        #endregion

    }
}
