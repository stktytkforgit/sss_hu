using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Imaging;

using GazoYaro;

namespace aaa
{
	public partial class Form000 : Form
	{

		const int INI_W = 512;
		const int INI_H = 512;
		Bitmap Bmp000;
		byte [] Data000;

		int MouseX;
		int MouseY;

		int Xg;
		int Yg;

		double Angle;

		public Form000()
		{

			InitializeComponent();
		
			PixelFormat fmt = PixelFormat.Format8bppIndexed;
			Bmp000 = new Bitmap( INI_W, INI_H, fmt );

			int w = Bmp000.Width;
			int h = Bmp000.Height;
			int numpix = w * h;

			Data000 = new byte[ numpix ];

			//for ( int j = 0; j < h; j++ )
			//{
			//	for ( int i = 0; i < w; i++ )
			//	{
			//		Data000[ i + w * j ] = (byte)( i + j );				
			//	}
			//}

			GazoYaroImageProcessing gyip = new GazoYaroImageProcessing();
			gyip.Fill( Data000, w, h, 0x00 );

			GazoYaroUtil gyu = new GazoYaroUtil();
			gyu.SetPaletteBpp08ToBmp( Bmp000 );
			gyu.SetDataToBmp( Bmp000, Data000, w, h );

		}

		private void Form000_Load(object sender, EventArgs e)
		{

			panel000.Dock = DockStyle.Fill;
			panel000.AutoScroll = true;

			tssLabel000.BackColor = Color.Transparent;
			tssLabel001.BackColor = Color.Transparent;
			tssLabel002.BackColor = Color.Transparent;
			tssLabel003.BackColor = Color.Transparent;

			tssLabel000.Text = "";
			tssLabel001.Text = "";
			tssLabel002.Text = "";
			tssLabel003.Text = "";
	
			int w = Bmp000.Width;
			int h = Bmp000.Height;
			pictureBox000.SetBounds( 0, 0, w, h );

			this.BackColor = Color.DarkGray;

			this.Left = 32;
			this.Top = 32;
			this.Width = 1024;
			this.Height = 768;

		}

		private void Form000_FormClosing(object sender, FormClosingEventArgs e)
		{
			
			if ( Bmp000 != null )
			{
				Bmp000.Dispose();
			}

		}

		private void pictureBox000_Paint(object sender, PaintEventArgs e)
		{

			if ( Bmp000 == null )
			{
				return;
			}

			Graphics grph = e.Graphics;

			int w = Bmp000.Width;
			int h = Bmp000.Height;

			e.Graphics.DrawImage( Bmp000, 0, 0, w, h );
		
			GazoYaroGdi gyg = new GazoYaroGdi();
			gyg.DrawCrossLine( grph, Color.Red, Xg, Yg, w, h );
			
			double r = 500.0;
			double tmp_x = Xg + ( r * Math.Cos( Angle ));
			double tmp_y = Yg - ( r * Math.Sin( Angle ));

			int x = (int)( tmp_x + 0.5 );
			int y = (int)( tmp_y + 0.5 );
			gyg.DrawLine( grph, Color.Lime, Xg, Yg, x, y );

		}

		private void menuApplicationQuit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void pictureBox000_MouseDown(object sender, MouseEventArgs e)
		{

			if (( e.Button & MouseButtons.Left ) == MouseButtons.Left )
			{

				MouseX = e.X;
				MouseY = e.Y;

				int x = MouseX;
				int y = MouseY;
				int wh = 16;

				GazoYaroImageProcessing gyip = new GazoYaroImageProcessing();
				int w = Bmp000.Width;
				int h = Bmp000.Height;
				int xs = x - ( wh >> 1 );
				int ys = y - ( wh >> 1 );
				int xe = xs + wh - 1;
				int ye = ys + wh - 1;
				byte level = 0xff;
				gyip.DrawRect( Data000, w, h, xs, ys, xe, ye, level );

				GazoYaroUtil gyu = new GazoYaroUtil();
				gyu.SetDataToBmp( Bmp000, Data000, w, h );

			}
			else if (( e.Button & MouseButtons.Right ) == MouseButtons.Right )
			{
			
				int w = Bmp000.Width;
				int h = Bmp000.Height;

				GazoYaroImageProcessing gyip = new GazoYaroImageProcessing();
				gyip.Fill( Data000, w, h, 0x00 );
				
				GazoYaroUtil gyu = new GazoYaroUtil();
				gyu.SetDataToBmp( Bmp000, Data000, w, h );

			}
			else
			{
				// NOP.
			}

		}

		private void pictureBox000_MouseMove(object sender, MouseEventArgs e)
		{

			if (( e.Button & MouseButtons.Left ) == MouseButtons.Left )
			{
				MouseX = e.X;
				MouseY = e.Y;
				this.pictureBox000_MouseDown( sender, e );
			}

			pictureBox000.Invalidate( true );

		}

		private void menuDebugExec_Click(object sender, EventArgs e)
		{

			int w = Bmp000.Width;
			int h = Bmp000.Height;

			double xg = 0.0;
			double yg = 0.0;
			GetCoG( ref xg, ref yg, Data000, w, h );

			double M11 = 0.0;
			double M20 = 0.0;
			double M02 = 0.0;
			Get_M11_M20_M02( ref M11, ref M20, ref M02, Data000, w, h, xg, yg );

			double top = 2.0 * M11;
			double btm = M20 - M02;
			Angle = 0.5 * Math.Atan( top / btm );

			Xg = (int)( xg );
			Yg = (int)( yg );

			this.pictureBox000.Invalidate( true );

		}

		bool GetCoG( ref double xg, ref double yg, byte [] data, int w, int h )
		{
		
			uint top_x = 0;
			uint btm_x = 0;

			uint top_y = 0;
			uint btm_y = 0;

			for ( int j = 0; j < h; j++ )
			{
				for ( int i = 0; i < w; i++ )
				{
					
					if ( data[ i + w * j ] != 0x00 )
					{
						top_x += (uint)( i );
						btm_x++;

						top_y += (uint)( j );
						btm_y++;
					}

				}
			}
		
			xg = (double)( top_x )/(double)( btm_x );
			yg = (double)( top_y )/(double)( btm_y );

			return true;
		
		}

		bool Get_M11_M20_M02( ref double M11, ref double M20, ref double M02, byte [] data, int w, int h, double xg, double yg )
		{

			for ( int j = 0; j < h; j++ )
			{
				for ( int i = 0; i < w; i++ )
				{
					
					int x = i;
					int y = j;

					double dx = (double)( x - xg );
					double dy = (double)( y - yg );

					if ( data[ i + w * j ] != 0x00 )
					{
						M11 = dx * dy;
						M20 = dx * dx;
						M02 = dy * dy;
					}

				}
			}
	
			return true;

		}


	}
}
