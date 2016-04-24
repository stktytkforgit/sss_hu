using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GazoYaro
{

	public class GazoYaroMomentUtil
	{

		public bool Get_M00( ref double M00, byte [] data, int w, int h, double xg, double yg )
		{

			// いわゆる面積.

			M00 = 0.0;

			int counter = 0;

			for ( int j = 0; j < h; j++ )
			{
				for ( int i = 0; i < w; i++ )
				{

					if ( data[ i + w * j ] != 0x00 )
					{
						counter++;
					}

				}
			}

			M00 = counter;

			return true;

		}


		public bool Get_M10_M01( ref double M10, ref double M01, byte [] data, int w, int h, double xg, double yg )
		{

			M10 = 0.0;
			M01 = 0.0;

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

						M10 += dx;
						M01 += dy;
					}

				}
			}
	
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

		public bool Get_M21_M12( ref double M21, ref double M12, byte [] data, int w, int h, double xg, double yg )
		{

			M21 = 0.0;
			M12 = 0.0;

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

						M21 += (( dx * dx ) * dy );
						M12 += ( dx * ( dy * dy ));
					}

				}
			}
	
			return true;

		}

		public bool Get_M30_M03( ref double M30, ref double M03, byte [] data, int w, int h, double xg, double yg )
		{

			M30 = 0.0;
			M03 = 0.0;

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

						M30 += ( dx * dx * dx );
						M03 += ( dy * dy * dy );
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

	}
}
