using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Range
{

    /// <summary>
    /// Structure that holds a value restrited by an upper and a lower bound.
    /// </summary>
    public class Rangeable
    {

        #region Attributes

        /// <summary>
        /// The value holded.
        /// </summary>
        private double _value;

        /// <summary>
        /// The upper bound, inclusive.
        /// </summary>
        private double _max;

        /// <summary>
        /// The lower bound, inclusive.
        /// </summary>
        private double _min;

        /// <summary>
        /// The step for generating values.
        /// </summary>
        private double _step;

        #endregion

        #region Constructors

        /// <summary>
        /// Structure that holds a value restrited by an upper and a lower bound.
        /// </summary>
        /// <param name="value">The value holded.</param>
        /// <param name="min">The lower bound, inclusive.</param>
        /// <param name="max">The upper bound, inclusive.</param>
        /// <param name="step">The step for generating values.</param>
        public Rangeable(double value, double min, double max, double step)
        {
            // throw exception if min > max
            if (min > max)
                throw new ArgumentException("min given is greather than max.");

            // Throw exception if val > max or val < min
            if (value > max)
                throw new ArgumentException("value given is invalid (greather than max).");

            if (value < min)
                throw new ArgumentException("value given is invalid (smaller than min).");

            _value = value;
            _min = min;
            _max = max;
            _step = step;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the lower bound, inclusive.
        /// </summary>
        public double Min
        {
            get
            {
                return _min;
            }
        }

        /// <summary>
        /// Sets the upper bound, inclusive.
        /// </summary>
        public double Max
        {
            get
            {
                return _max;
            }
        }

        /// <summary>
        /// Gets or Sets the value holded.
        /// </summary>
        /// <param name="value"></param>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                // Throw exception if val > max or val < min
                if (value > _max)
                    throw new ArgumentException("value given is invalid (greather than max).");

                if (value < _min)
                    throw new ArgumentException("value given is invalid (smaller than min).");

                _value = value;
            }

        }

        public double Step
        {
            get
            {
                return _step;
            }
        }

        #endregion

        #region Parse

        /// <summary>
        /// Creates an instance of Rangeable that holds a byte.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Rangeable ForByte(Byte b)
        {
            return new Rangeable((double)b, (double)byte.MinValue, (double)byte.MaxValue, 1);
        }

        /// <summary>
        /// Creates an instance of Rangeable that holds normalized double [0.0 .. 1.0].
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Rangeable ForNormalizedDouble(double d)
        {
            return new Rangeable((double)d, 0.0, 1.0, (1.0 / 255.0));
        }

        /// <summary>
        /// Creates an instance of Rangeable that holds a double.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Rangeable ForDouble(double d)
        {
            return new Rangeable(d, double.MinValue, double.MaxValue, 0.0001);
        }

        /// <summary>
        /// Creates an instance of Rangeable that holds a int.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Rangeable ForInt(int i)
        {
            return new Rangeable(i, int.MinValue, int.MaxValue, 1);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Explicit cast operator from Rangeable to double.
        /// </summary>
        /// <param name="rng"></param>
        /// <returns></returns>
        public static explicit operator double(Rangeable rng)
        {
            return rng._value;
        }

        #endregion

        #region object

        /// <summary>
        /// Converts the value holded by this instance to its string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        #endregion
    }
}
