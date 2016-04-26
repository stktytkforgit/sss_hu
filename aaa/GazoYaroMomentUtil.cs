using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GazoYaro
{

	public class Moments
	{
		public double m00;
		public double m10;
		public double m01;
		public double M11;
		public double M20;
		public double M02;

		public Moments()
		{
			this.InitZero();
		}

		public bool InitZero()
		{
			m00 = 0.0;
			m10 = 0.0;
			m01 = 0.0;
			M11 = 0.0;
			M20 = 0.0;
			M02 = 0.0;
			return true;
		}

		public bool SetData( double m00, double m10, double m01, double M11, double M20, double M02 )
		{
			
			this.m00 = m00;
			this.m10 = m10;
			this.m01 = m01;
			this.M11 = M11;
			this.M20 = M20;
			this.M02 = M02;

			return true;

		}

	}

	public class GazoYaroMomentUtil
	{

		public bool Get_m00_m10_m01( ref double m00, ref double m10, ref double m01, byte [] data, int width, int height )
		{

			int w = width;
			int h = height;

			int counter = 0;
			double sum_m10 = 0.0;
			double sum_m01 = 0.0;

			for ( int j = 0; j < h; j++ )
			{
				for ( int i = 0; i < w; i++ )
				{

					if ( data[ i + w * j ] != 0x00 )
					{
						counter++;

						int x = i;
						int y = j;

						sum_m10 += x;
						sum_m01 += y;
					}

				}
			}
			
			m00 = (double)( counter );
			m10 = sum_m10;
			m01 = sum_m01;

			return true;
		}

		public bool Get_M11_M20_M02( ref double M11, ref double M20, ref double M02, byte [] data, int w, int h, double xg, double yg )
		{

			M11 = 0.0;
			M20 = 0.0;
			M02 = 0.0;

			for ( int j = 0; j < h; j++ )
			{
				for ( int i = 0; i < w; i++ )
				{

					if ( data[ i + w * j ] != 0x00 )
					{
						int x = i;
						int y = j;

						double dx = (double)( x - xg );
						double dy = (double)( y - yg );

						M11 += ( dx * dy );
						M20 += ( dx * dx );
						M02 += ( dy * dy );
					}

				}
			}
	
			return true;

		}

		public bool Get_Myu(
			ref double [] myu,
			double M00,
			double M10,
			double M01,
			double M11,
			double M20,
			double M02,
			double M21,
			double M12,
			double M30,
			double M03,
			double xg,
			double yg
			)
		{
		
			double x = xg;
			double y = yg;

			double myu00 = M00; 
			double myu01 = 0.0;
			double myu10 = 0.0; 
			double myu11 = M11 - x * M01; // = M11 - y *  M10;
			double myu20 = M20 - x * M10;
			double myu02 = M02 - y * M01;
			double myu21 = M21 - ( 2.0 * x * M11 ) - ( y * M20 ) + ( 2.0 * ( x * x ) * M01 );
			double myu12 = M12 - ( 2.0 * y * M11 ) - ( x * M02 ) + ( 2.0 * ( y * y ) * M10 );
			double myu30 = M30 - ( 3.0 * x * M20 ) + ( 2.0 * ( x * x ) * M10 );
			double myu03 = M03 - ( 3.0 * y * M02 ) + ( 2.0 * ( y * y ) * M01 );
		
			myu = new double[10];
			myu[0] = myu00;
			myu[1] = myu01;
			myu[2] = myu10;
			myu[3] = myu11;
			myu[4] = myu20;
			myu[5] = myu02;
			myu[6] = myu21;
			myu[7] = myu12;
			myu[8] = myu30;
			myu[9] = myu03;

			return true;

		}

		public bool Get_MyuEx(
			ref double myu00,
			ref double myu01,
			ref double myu10,
			ref double myu11,
			ref double myu20,
			ref double myu02,
			ref double myu21,
			ref double myu12,
			ref double myu30,
			ref double myu03,
			double M00,
			double M10,
			double M01,
			double M11,
			double M20,
			double M02,
			double M21,
			double M12,
			double M30,
			double M03,
			double xg,
			double yg
			)
		{
		
			double x = xg;
			double y = yg;

			double [] arr = new double[10];
			Get_Myu( ref arr, M00, M10, M01, M11, M20, M02, M21, M12, M30, M03, xg, yg );

			myu00 = arr[0];
			myu01 = arr[1];
			myu10 = arr[2];
			myu11 = arr[3];
			myu20 = arr[4];
			myu02 = arr[5];
			myu21 = arr[6];
			myu12 = arr[7];
			myu30 = arr[8];
			myu03 = arr[9];

			return true;

		}

		public bool GetAngleAndRadiusFromMoment(
			ref double angle,
			ref double r_major,
			ref double r_minor,
			double numpix_or_m00,
			double M11,
			double M20,
			double M02
			)
		{

			angle = 0.0;
			r_major = 0.0;
			r_minor = 0.0;
		
			double pi = Math.PI; // 180deg.

			double top = 2.0 * M11;
			double btm = M20 - M02;

			if ( btm == 0.0 )
			{
				// zero div.
				return false;
			}
			else
			{

				double radian = 0.5 * Math.Atan( top / btm );
	
				if ( M20 > M02 )
				{
					angle = radian;
				}
				else
				{
					double half_pi = 0.5 * pi; //  90deg.
					angle = radian + half_pi;
				}

			}		

			double numpix = numpix_or_m00;

			if ( numpix == 0.0 )
			{
				return false;
			}
			else
			{

				double root_two = Math.Sqrt( 2.0 );
	
				double uxy = M11 / (double)( numpix );
				double uxx = M20 / (double)( numpix );
				double uyy = M02 / (double)( numpix );

				double common = Math.Sqrt( (( uxx - uyy ) * ( uxx - uyy )) + ( 4.0 * uxy * uxy ) );

				r_major = root_two * Math.Sqrt( uxx + uyy + common );
				r_minor = root_two * Math.Sqrt( uxx + uyy - common );

				return true;

			}


		}


	}
}
