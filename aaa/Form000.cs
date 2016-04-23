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

		public Form000()
		{

			InitializeComponent();
		
			PixelFormat fmt = PixelFormat.Format8bppIndexed;
			Bmp000 = new Bitmap( INI_W, INI_H, fmt );

			int w = Bmp000.Width;
			int h = Bmp000.Height;
			int numpix = w * h;

			Data000 = new byte[ numpix ];

			for ( int j = 0; j < h; j++ )
			{
				for ( int i = 0; i < w; i++ )
				{
					Data000[ i + w * j ] = (byte)( i + j );				
				}
			}

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

			int w = Bmp000.Width;
			int h = Bmp000.Height;

			e.Graphics.DrawImage( Bmp000, 0, 0, w, h );
		
		}

		private void menuApplicationQuit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

	}
}
