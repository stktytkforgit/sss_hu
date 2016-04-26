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
		double R_Major;
		double R_Minor;

		Moments Mmt = new Moments();

		public Form000()
		{

			InitializeComponent();
		
			PixelFormat fmt = PixelFormat.Format8bppIndexed;
			Bmp000 = new Bitmap( INI_W, INI_H, fmt );

			int w = Bmp000.Width;
			int h = Bmp000.Height;
			int numpix = w * h;

			Data000 = new byte[ numpix ];

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

			Color c = Color.Red;

			double dx;
			double dy;
			int xs;
			int ys;
			int xe;
			int ye;

			double r0 = R_Major;
			double r1 = R_Minor;

			double angle0 = Angle;
			double angle1 = Angle + ( 0.5 * Math.PI );

			// 長径を描画する.
			dx = ( r0 * Math.Cos( angle0 ));
			dy = ( r0 * Math.Sin( angle0 ));
			xs = (int)( Xg + dx );
			ys = (int)( Yg + dy );
			xe = (int)( Xg - dx );
			ye = (int)( Yg - dy );
			gyg.DrawLine( grph, c, xs, ys, xe, ye );

			// 短径を描画する.
			dx = ( r1 * Math.Cos( angle1 ));
			dy = ( r1 * Math.Sin( angle1 ));
			xs = (int)( Xg + dx );
			ys = (int)( Yg + dy );
			xe = (int)( Xg - dx );
			ye = (int)( Yg - dy );
			gyg.DrawLine( grph, c, xs, ys, xe, ye );

			// 重心を描画する.
			int wh = 10;
			xs = Xg - ( wh >> 1 );
			ys = Yg - ( wh >> 1 );
			xe = xs + wh - 1;
			ye = ys + wh - 1;
			gyg.FillRectangle( grph, c, xs, ys, xe, ye );

			double m00 = Mmt.m00;
			double m10 = Mmt.m10;
			double m01 = Mmt.m01;
			double M11 = Mmt.M11;
			double M20 = Mmt.M20;
			double M02 = Mmt.M02;

			String str000 = "";
			String str001 = "";
			String str002 = "";
			String str003 = "";

			str000 = String.Format( "{0} * {1}", w, h );
			str001 = String.Format( "{0}, {1}, {2}, {3}, {4}, {5}", m00, m10, m01, M11, M20, M02 );

			tssLabel000.Text = str000;
			tssLabel001.Text = str001;
			tssLabel002.Text = str002;
			tssLabel003.Text = str003;

		}

		private void menuApplicationQuit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void pictureBox000_MouseDown(object sender, MouseEventArgs e)
		{

			int w = Bmp000.Width;
			int h = Bmp000.Height;

			if (( e.Button & MouseButtons.Left ) == MouseButtons.Left )
			{

				MouseX = e.X;
				MouseY = e.Y;

				int x = MouseX;
				int y = MouseY;
				int wh = 16;

				GazoYaroImageProcessing gyip = new GazoYaroImageProcessing();
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

			Xg = 0;
			Yg = 0;
			Angle = 0.0;
			R_Major = 0.0;
			R_Minor = 0.0;

			GazoYaroMomentUtil gymtu = new GazoYaroMomentUtil();
			
			// 重心をもとめるためのモーメントを取得する.
			double m00 = 0.0;
			double m10 = 0.0;
			double m01 = 0.0;
			gymtu.Get_m00_m10_m01( ref m00, ref m10, ref m01, Data000, w, h );
			
			// zero div error.
			if ( m00 != 0.0 )
			{

				// 重心を取得する.
				double xg = m10 / m00;
				double yg = m01 / m00;

				// 偏角と長半径と短半径をもとめるためのモーメントを取得する.
				double M11 = 0.0;
				double M20 = 0.0;
				double M02 = 0.0;
				gymtu.Get_M11_M20_M02( ref M11, ref M20, ref M02, Data000, w, h, xg, yg );

				// 偏角と長半径と短半径をもとめる.
				Angle = 0.0;
				R_Major = 0.0;
				R_Minor = 0.0;
				gymtu.GetAngleAndRadiusFromMoment( ref Angle, ref R_Major, ref R_Minor, m00, M11, M20, M02 );

				// グローバルな変数にかきこむ.
				Mmt.SetData( m00, m10, m01, M11, M20, M02 );

				// 描画用の重心のため整数にする.
				Xg = (int)( xg );
				Yg = (int)( yg );

			}

			this.pictureBox000.Invalidate( true );

		}

		private void menuFileSave_Click(object sender, EventArgs e)
		{
			
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "BMP|*.bmp|ALL|*.*";
			
			if ( sfd.ShowDialog() == DialogResult.Cancel )
			{
				return;
			}

			String filepath = sfd.FileName;

			ImageFormat fmt = ImageFormat.Bmp;
			Bmp000.Save( filepath, fmt );

		}
		

	}
}
