using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using TUIO;

public class TuioDemo : Form, TuioListener
{
	private TuioClient client;
	private Dictionary<long, TuioDemoObject> objectList;
	private Dictionary<long, TuioCursor> cursorList;
	private Dictionary<long, TuioBlob> blobList;
	private object cursorSync = new object();
	private object objectSync = new object();
	private object blobSync = new object();

	public static int width, height;
	private int window_width = 940;
	private int window_height = 680;
	private int window_left = 0;
	private int window_top = 0;
	private int screen_width = Screen.PrimaryScreen.Bounds.Width;
	private int screen_height = Screen.PrimaryScreen.Bounds.Height;

	private bool fullscreen;
	private bool verbose;

	SolidBrush blackBrush = new SolidBrush(Color.Black);
	SolidBrush whiteBrush = new SolidBrush(Color.White);

	SolidBrush grayBrush = new SolidBrush(Color.Gray);
	Pen fingerPen = new Pen(new SolidBrush(Color.Blue), 1);
	Timer mytime = new Timer();
	Bitmap img2 = new Bitmap("basket.bmp");
	Bitmap off;


	public TuioDemo(int port)
	{

		verbose = true;
		fullscreen = false;
		width = window_width;
		height = window_height;

		this.ClientSize = new System.Drawing.Size(width, height);
		this.Name = "TuioDemo";
		this.Text = "TuioDemo";

		this.Closing += new CancelEventHandler(Form_Closing);
		this.KeyDown += new KeyEventHandler(Form_KeyDown);

		this.SetStyle(ControlStyles.AllPaintingInWmPaint |
						ControlStyles.UserPaint |
						ControlStyles.DoubleBuffer, true);

		objectList = new Dictionary<long, TuioDemoObject>(128);
		cursorList = new Dictionary<long, TuioCursor>(128);

		client = new TuioClient(port);
		client.addTuioListener(this);

		client.connect();

		//-----------------------------

		off = new Bitmap(ClientSize.Width, ClientSize.Height);
		mytime.Tick += Mytime_Tick;
		mytime.Start();

	}

	private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
	{

		if (e.KeyData == Keys.F1)
		{
			if (fullscreen == false)
			{

				width = screen_width;
				height = screen_height;

				window_left = this.Left;
				window_top = this.Top;

				this.FormBorderStyle = FormBorderStyle.None;
				this.Left = 0;
				this.Top = 0;
				this.Width = screen_width;
				this.Height = screen_height;

				fullscreen = true;
			}
			else
			{

				width = window_width;
				height = window_height;

				this.FormBorderStyle = FormBorderStyle.Sizable;
				this.Left = window_left;
				this.Top = window_top;
				this.Width = window_width;
				this.Height = window_height;

				fullscreen = false;
			}
		}
		else if (e.KeyData == Keys.Escape)
		{
			this.Close();

		}
		else if (e.KeyData == Keys.V)
		{
			verbose = !verbose;
		}

	}

	private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
	{
		client.removeTuioListener(this);

		client.disconnect();
		System.Environment.Exit(0);
	}

	public void addTuioObject(TuioObject o)
	{
		lock (objectSync)
		{
			objectList.Add(o.SessionID, new TuioDemoObject(o));
		}
		if (verbose) Console.WriteLine("add obj " + o.SymbolID + " (" + o.SessionID + ") " + o.X + " " + o.Y + " " + o.Angle);

	}

	public void updateTuioObject(TuioObject o)
	{
		lock (objectSync)
		{
			objectList[o.SessionID].update(o);
		}
		if (verbose) Console.WriteLine("set obj " + o.SymbolID + " " + o.SessionID + " " + o.X + " " + o.Y + " " + o.Angle + " " + o.MotionSpeed + " " + o.RotationSpeed + " " + o.MotionAccel + " " + o.RotationAccel);
	}

	public void removeTuioObject(TuioObject o)
	{
		lock (objectSync)
		{
			objectList.Remove(o.SessionID);
		}
		if (verbose) Console.WriteLine("del obj " + o.SymbolID + " (" + o.SessionID + ")");
	}

	public void addTuioCursor(TuioCursor c)
	{
		lock (cursorSync)
		{
			cursorList.Add(c.SessionID, c);
		}
		if (verbose) Console.WriteLine("add cur " + c.CursorID + " (" + c.SessionID + ") " + c.X + " " + c.Y);
	}

	public void updateTuioCursor(TuioCursor c)
	{
		if (verbose) Console.WriteLine("set cur " + c.CursorID + " (" + c.SessionID + ") " + c.X + " " + c.Y + " " + c.MotionSpeed + " " + c.MotionAccel);
	}

	public void removeTuioCursor(TuioCursor c)
	{
		lock (cursorSync)
		{
			cursorList.Remove(c.SessionID);
		}
		if (verbose) Console.WriteLine("del cur " + c.CursorID + " (" + c.SessionID + ")");
	}

	public void addTuioBlob(TuioBlob b)
	{
		lock (blobSync)
		{
			blobList.Add(b.SessionID, b);
		}
		if (verbose) Console.WriteLine("add blb " + b.BlobID + " (" + b.SessionID + ") " + b.X + " " + b.Y + " " + b.Angle + " " + b.Width + " " + b.Height + " " + b.Area);
	}

	public void updateTuioBlob(TuioBlob b)
	{
		if (verbose) Console.WriteLine("set blb " + b.BlobID + " (" + b.SessionID + ") " + b.X + " " + b.Y + " " + b.Angle + " " + b.Width + " " + b.Height + " " + b.Area + " " + b.MotionSpeed + " " + b.RotationSpeed + " " + b.MotionAccel + " " + b.RotationAccel);
	}

	public void removeTuioBlob(TuioBlob b)
	{
		lock (blobSync)
		{
			blobList.Remove(b.SessionID);
		}
		if (verbose) Console.WriteLine("del blb " + b.BlobID + " (" + b.SessionID + ")");
	}

	public void refresh(TuioTime frameTime)
	{
		Invalidate();
	}



	/********************/

	private void TuioDemo_Load(object sender, EventArgs e)
	{


	}

	public class Actor
	{
		public int X, Y, dix, diy;
		public Bitmap img;
	}
	public List<Actor> lball = new List<Actor>();

	public int counttick = 0;
	private void Mytime_Tick(object sender, EventArgs e)
	{
		if (counttick % 10 == 0)
		{
			//counttick = 0;
			creatActor();
			MoveActor();
		}
		counttick++;
	
		DrowDupp(CreateGraphics());

	}

	public void MoveActor()
	{
		for (int i = 0; i < lball.Count; i++)
		{
			
				lball[i] .Y -= lball[i].diy * 20;
			
		}
	}
	public void creatActor()
	{
		Random rr = new Random();
		Actor pnn = new Actor();
		pnn.X = rr.Next(20, 900);
		pnn.Y = 20;
		pnn.img = new Bitmap("ball2.bmp");
		pnn.img.MakeTransparent(pnn.img.GetPixel(0, 0));
		pnn.dix = 0;
		pnn.diy = -1;
		lball.Add(pnn);
	}

	public void DrowDupp(Graphics g)
	{
		Graphics g2 = Graphics.FromImage(off);
		Drowscene(g2);
		g.DrawImage(off, 0, 0);
	}

	public void Drowscene(Graphics g)
	{
		g.Clear(Color.White);
		for (int i = 0; i < lball.Count; i++)
		{
			g.DrawImage(lball[i].img, lball[i].X, lball[i].Y);

		}
	}


	/******************/




	protected override void OnPaintBackground(PaintEventArgs pevent)
	{
		// Getting the graphics object

		Graphics g = pevent.Graphics;
		g.FillRectangle(whiteBrush, new Rectangle(0, 0, width, height));



		// draw the cursor path
		if (cursorList.Count > 0)
		{
			lock (cursorSync)
			{
				foreach (TuioCursor tcur in cursorList.Values)
				{
					List<TuioPoint> path = tcur.Path;
					TuioPoint current_point = path[0];

					for (int i = 0; i < path.Count; i++)
					{
						TuioPoint next_point = path[i];
						g.DrawLine(fingerPen, current_point.getScreenX(width), current_point.getScreenY(height), next_point.getScreenX(width), next_point.getScreenY(height));
						current_point = next_point;
					}
					g.FillEllipse(grayBrush, current_point.getScreenX(width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
					Font font = new Font("Arial", 10.0f);
					g.DrawString(tcur.CursorID + "", font, blackBrush, new PointF(tcur.getScreenX(width) - 10, tcur.getScreenY(height) - 10));
				}
			}
		}

		// draw the objects
		if (objectList.Count > 0)
		{
			lock (objectSync)
			{
				foreach (TuioDemoObject tobject in objectList.Values)
				{
					//tobject.paint(g);
					if (tobject.SymbolID == 1)
					{
						int Xpos = (int)(tobject.Position.X * TuioDemo.width);
						int Ypos = (int)(tobject.Position.Y * TuioDemo.height);
						int size = TuioDemo.height / 10;

						g.TranslateTransform(Xpos, Ypos);
						g.RotateTransform((float)(tobject.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-1 * Xpos, -1 * Ypos);

						g.DrawImage(img2, Xpos - size / 2, Ypos - size / 2,60,60);

						g.TranslateTransform(Xpos, Ypos);
						g.RotateTransform(-1 * (float)(tobject.Angle / Math.PI * 180.0f));
						g.TranslateTransform(-1 * Xpos, -1 * Ypos);
					}
					//DrowDupp(CreateGraphics());

				}
			}
		}
	}

	private void InitializeComponent()
	{
		this.SuspendLayout();
		// 
		// TuioDemo
		// 
		this.ClientSize = new System.Drawing.Size(282, 253);
		this.Name = "TuioDemo";
		this.Load += new System.EventHandler(this.TuioDemo_Load);
		this.ResumeLayout(false);

	}



	public static void Main(String[] argv)
	{
		int port = 0;
		switch (argv.Length)
		{
			case 1:
				port = int.Parse(argv[0], null);
				if (port == 0) goto default;
				break;
			case 0:
				port = 3333;
				break;
			default:
				Console.WriteLine("usage: java TuioDemo [port]");
				System.Environment.Exit(0);
				break;
		}

		TuioDemo app = new TuioDemo(port);
		Application.Run(app);
	}
}