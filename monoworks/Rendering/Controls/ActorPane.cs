// ActorPane.cs - MonoWorks Project
//
//  Copyright (C) 2009 Andy Selvig
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 

using System;

using MonoWorks.Base;
using MonoWorks.Rendering;
using MonoWorks.Rendering.Events;

using Tao.OpenGl;

namespace MonoWorks.Rendering.Controls
{
	
	/// <summary>
	/// The ActorPane is an 3D plane that can be placed directly into the scene.
	/// </summary>
	public class ActorPane : Actor, IPane
	{
		
		public ActorPane() : base()
		{
			Position = new Vector();
		}
				
		public ActorPane(Control2D control) : this()
		{
			Control = control;
		}
		
		
		/// <value>
		/// The size of the pane.
		/// </value>
		public Coord Size
		{
			get
			{
				if (Control != null)
					return Control.Size;
				else
					return new Coord();
			}
		}
		
		public double Width
		{
			get {return Size.X;}
		}
		
		public double Height
		{
			get {return Size.Y;}
		}
		
		/// <value>
		/// The position of the upper left corner of the pane.
		/// </value>
		public Vector Position {get; set;}
		
		
		public Control2D Control {get; set;}
		
		
		
#region Interaction
		
		public override void OnButtonPress (MonoWorks.Rendering.Events.MouseButtonEvent evt)
		{
			base.OnButtonPress (evt);
		}

		public override void OnButtonRelease (MonoWorks.Rendering.Events.MouseButtonEvent evt)
		{
			base.OnButtonRelease (evt);
		}

		public override void OnMouseMotion (MonoWorks.Rendering.Events.MouseEvent evt)
		{
			base.OnMouseMotion (evt);
		}

		public override void OnMouseWheel (MonoWorks.Rendering.Events.MouseWheelEvent evt)
		{
			base.OnMouseWheel (evt);
		}

#endregion

		
#region Rendering
		
		/// <summary>
		/// Handle to the OpenGL texture that the control will be rendered to.
		/// </summary>
		private uint texture = 0;
		
		public override void ComputeGeometry()
		{
			base.ComputeGeometry();
			
			if (Control == null)
				return;
						
			if (texture == 0)
			{
				Gl.glGenTextures(1, out texture);
//				Console.WriteLine("generating texture: {0} ({1})", texture, Gl.GL_INVALID_OPERATION);
			}
						
			// copy the control image to the texture
			Gl.glBindTexture( Gl.GL_TEXTURE_RECTANGLE_ARB, texture );
			Control.RenderImage();
			Gl.glTexImage2D(Gl.GL_TEXTURE_RECTANGLE_ARB,
			                0, 
			                Gl.GL_RGBA, 
			                Control.IntWidth, 
			                Control.IntHeight, 
			                0, 
			                Gl.GL_BGRA, 
			                Gl.GL_UNSIGNED_BYTE, 
			                Control.ImageData);
			
		}
		
		public override void RenderTransparent(Viewport viewport)
		{
			if (Control == null)
				return;
			
			base.RenderTransparent(viewport);
			
			if (Control.IsDirty)
				ComputeGeometry();
			
			// determine how big the control should be 
			double scaling = viewport.Camera.ViewportToWorldScaling;
			double width = Width * scaling;
			double height = Height * scaling;
						
			// render the texture
			viewport.Lighting.Disable();
			Gl.glEnable(Gl.GL_TEXTURE_RECTANGLE_ARB);
			Gl.glBindTexture( Gl.GL_TEXTURE_RECTANGLE_ARB, texture );
			Gl.glBegin(Gl.GL_QUADS);
			Gl.glColor3f(1f, 1f, 1f);
			Gl.glTexCoord2d(0.0,Control.Height);
			Position.glVertex();
			Gl.glTexCoord2d(Control.Width, Control.Height);
			Gl.glVertex2d(Position.X + width, Position.Y);
			Gl.glTexCoord2d(Control.Width,0.0);
			Gl.glVertex2d(Position.X + width, Position.Y + height);
			Gl.glTexCoord2d(0.0,0.0);
			Gl.glVertex2d(Position.X, Position.Y + height);
			Gl.glEnd();
			Gl.glDisable(Gl.GL_TEXTURE_RECTANGLE_ARB);
			viewport.Lighting.Enable();
		}

#endregion
		
	}
}
