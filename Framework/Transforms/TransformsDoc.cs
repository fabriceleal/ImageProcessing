// Transforms


/// <summary>
/// Holds the implementations of the transforms provided and used by the Framework.
/// </summary>
/// <remarks>
/// A transform is an operation that takes a function in the spatial / temporatl domain into the frequency domain.
/// An example of a transform is the FourierTransform, but there a number of transforms that are not implemented in 
/// the Framework: the cosine transform, the sine transform, the Hadamard transform, the Haar transform, the Slant 
/// transform, the KL transform, the Karhunen-Loeve transform, the sinusoidal transforms, the SVD transform may some examples
/// (Fundamentals of Digital Image Processing).
/// </remarks>
/// <example>
/// Get Fourier Transform of an Image and Reverse-Transform it.
/// <code>
/// Image img = Image.FromFile("test.bmp");
/// 
/// FourierTransform ft = new FourierTransform();
/// 
/// // Compute transform
/// ComplexImage trImg = FourierTransform.ApplyTransformBase(Facilities.ToRGBGreyScale(img));
/// 
/// // ... Compute in frequency domain ...
/// 
/// // Get back to spatial domain
/// byte[,] invTrImg = FourierTransform.ApplyReverseTransform(trImg);
/// 
/// </code>
/// 
/// </example>
namespace Framework.Transforms
{ }