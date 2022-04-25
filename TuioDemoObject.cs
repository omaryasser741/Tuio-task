/*
	TUIO C# Demo - part of the reacTIVision project
	http://reactivision.sourceforge.net/

	Copyright (c) 2005-2009 Martin Kaltenbrunner <martin@tuio.org>

	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using TUIO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

public class TuioDemoObject : TuioObject
{
	
	//public static int s1;
 //   public static int s2;
	//public static int a1;
	//public static int a2;
	//public static bool Flag1 = false;
	//public static bool Flag2 = false;

	SolidBrush black = new SolidBrush(Color.Black);
	SolidBrush white = new SolidBrush(Color.White);
	

	//Bitmap img = new Bitmap("1.bmp");
	//Bitmap imgg = new Bitmap("2.bmp");    
	
	


	public TuioDemoObject(long s_id, int f_id, float xpos, float ypos, float angle) : base(s_id, f_id, xpos, ypos, angle) {
		
	}

	public TuioDemoObject(TuioObject o) : base(o) {
		
	}
	
	public void paint(Graphics g) {


		int Xpos = (int)(xpos * TuioDemo.width);
		int Ypos = (int)(ypos * TuioDemo.height);
		int size = TuioDemo.height / 10;
       


        g.TranslateTransform(Xpos, Ypos);
		g.RotateTransform((float)(angle / Math.PI * 180.0f));
		g.TranslateTransform(-1 * Xpos, -1 * Ypos);
		//if (symbol_id == 2)
		//{
		//	g.DrawImage(img, Xpos - size / 2, Ypos - size / 2);
		//	a1 = Xpos - size / 2;
		//	a2 = Ypos - size / 2;
		//	Flag2 = true;
		//}
		//if (symbol_id == 6)
  //      {
		//	g.DrawImage(imgg, Xpos - size / 2, Ypos - size / 2);
  //          s1 = Xpos - size / 2;
  //          s2 = Ypos - size / 2;
		//	Flag1 = true;
  //      }		
        
        g.TranslateTransform(Xpos, Ypos);
		g.RotateTransform(-1 * (float)(angle / Math.PI * 180.0f));
		g.TranslateTransform(-1 * Xpos, -1 * Ypos);

		Font font = new Font("Arial", 10.0f);
		g.DrawString(symbol_id + "", font, white, new PointF(Xpos - 10, Ypos - 10));
		
	}

    private void DrawDubb(object v)
    {
        throw new NotImplementedException();
    }

    private object CreateGraphics()
    {
        throw new NotImplementedException();
    }
}

