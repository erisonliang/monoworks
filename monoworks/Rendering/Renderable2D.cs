// Control2D.cs - MonoWorks Project
//
//  Copyright (C) 2008 Andy Selvig
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
using System.Collections.Generic;

using MonoWorks.Base;
using MonoWorks.Rendering;
using MonoWorks.Rendering.Events;
using System.Runtime.InteropServices;



namespace MonoWorks.Rendering
{	
	
	/// <summary>
	/// Handler for simple control events.
	/// </summary>
	public delegate void ControlEventHandler(Renderable2D control);
	
	
	/// <summary>
	/// Base class for all renderable controls.
	/// </summary>
	public abstract class Renderable2D : Renderable
	{
		protected Renderable2D() : base()
		{			
			Origin = new Coord();
			RenderSize = new Coord();
			
			HitStateChanged += OnHitStateChanged;
			
			_isEnabled = true;
		}
		
		
		/// <value>
		/// The control's parent.
		/// </value>
		public Renderable2D ParentControl {
			get { return Parent as Renderable2D; }
			set { Parent = value; }
		}


		
		private IPane _pane;
		/// <value>
		/// The pane this control belongs to.
		/// </value>
		public IPane Pane
		{
			get
			{
				if (_pane != null)
					return _pane;
				return ParentControl != null ? ParentControl.Pane : null;
			}
			set
			{
				_pane = value;
			}
		}
		

		
		public override void MakeDirty ()
		{
			base.MakeDirty();
			
			if (ParentControl != null)
				ParentControl.MakeDirty();
		}

		

		#region Size and Position

		/// <value>
		/// The relative position of the lower left of the control.
		/// </value>
		/// <remarks>The absolute position if a control will be the combination 
		/// of all positions through the control hierarchy.</remarks>
		public Coord Origin { get; set; }


		/// <summary>
		/// The absolute position that the control was last rendered at.
		/// </summary>
		/// <remarks>Use this for hit testing.</remarks>
		protected Coord LastPosition { get; set; }

		/// <value>
		/// The last rendered size of the control.
		/// </value>
		public Coord RenderSize { get; protected set; }

		/// <summary>
		/// The width of the control as it was last rendered.
		/// </summary>
		public double RenderWidth
		{
			get {return RenderSize.X;}
			set {RenderSize.X = value;}
		}

		/// <summary>
		/// The height of the control as it was last rendered.
		/// </summary>
		public double RenderHeight
		{
			get {return RenderSize.Y;}
			set {RenderSize.Y = value;}
		}

		/// <value>
		/// The minimum size that the control needs to render correctly.
		/// </value>
		[MwxProperty]
		public Coord MinSize { get; protected set; }

		/// <summary>
		/// The minimum width of the control.
		/// </summary>
		public double MinWidth
		{
			get {return MinSize.X;}
			protected set {MinSize.X = value;}
		}

		/// <summary>
		/// The minimum height of the control.
		/// </summary>
		public double MinHeight
		{
			get {return MinSize.Y;}
			protected set {MinSize.Y = value;}
		}
		
		/// <value>
		/// If not null, the layout system will attempt to use this size over the MinSize when computing RenderSize.
		/// </value>
		[MwxProperty]
		public Coord UserSize {get; set;}
		
		/// <summary>
		/// Tries to apply the user size to the render size, otherwise uses the min size.
		/// </summary>
		protected void ApplyUserSize()		{
			RenderSize = UserSize != null ? Coord.Max(MinSize, UserSize) : MinSize.Copy();
		}

		private double _padding = 4;
		/// <summary>
		/// The padding to use on the interior of controls.
		/// </summary>
		public double Padding
		{
			get { return _padding; }
			set { _padding = value; }
		}
		
		/// <value>
		/// The integer width of the control.
		/// </value>
		public int IntWidth
		{
			get {return (int)Math.Ceiling(RenderWidth);}
		}
		
		/// <value>
		/// The integer height of the control.
		/// </value>
		public int IntHeight
		{
			get {return (int)Math.Ceiling(RenderHeight);}
		}

		#endregion

		
		#region Rendering

		/// <summary>
		/// Sets the size to MinSize if UserSize is false.
		/// </summary>
		public override void ComputeGeometry()
		{
			base.ComputeGeometry();
			
			if (RenderSize == null)
				RenderSize = new Coord();

			if (MinSize == null)
				MinSize = new Coord();
		}
				
		/// <summary>
		/// Tells the parent pane to redraw its control during the next render cycle, even if it isn't dirty.
		/// </summary>
		/// <remarks>This is useful when you want the control to be redrawn but don't 
		/// need to bother computing geometry.</remarks>
		public void QueuePaneRender()
		{
			if (Pane != null)
				Pane.QueueRender();
		}

		#endregion
				
				
		#region Image Data
		
		/// <summary>
		/// Renders the control to an internal image surface.
		/// </summary>
		public abstract void RenderImage(Scene scene);
		
		protected byte[] _imageData;
		/// <value>
		/// The image data.
		/// </value>
		public byte[] ImageData
		{
			get {return _imageData;}
		}
		
		#endregion
		
		
		#region Hit Testing

		/// <summary>
		/// Performs the hit test on the rectangle defined by position and size.
		/// </summary>
		public virtual bool HitTest(Coord pos)
		{
			if (RenderSize == null || LastPosition == null || RenderSize == null)
				return false;
			return pos >= LastPosition && pos <= (LastPosition + RenderSize);
		}

		#endregion
		
				
		#region Interaction
		
		private bool _isHoverable;
		/// <value>
		/// Whether the control responds to mouse motion over it by going into the hovering state.
		/// </value>
		[MwxProperty]
		public bool IsHoverable
		{
			get {return _isHoverable;}
			set {_isHoverable = value;}
		}

		private bool _isEnabled;
		/// <value>
		/// Whether the control responds to user interaction.
		/// </value>
		/// <remarks>Disabled controls should be rendered differently.</remarks>
		[MwxProperty]
		public bool IsEnabled {
			get { return _isEnabled; }
			set {
				_isEnabled = value;
				MakeDirty();
			}
		}
		
		public override void OnButtonPress(MouseButtonEvent evt)
		{
			QueuePaneRender();
		}
		
		public override void OnButtonRelease(MouseButtonEvent evt)
		{
			QueuePaneRender();
		}
		
		public override void OnMouseMotion(MouseEvent evt)
		{
			if (!IsEnabled)
				return;
			
			if (_isHoverable && !evt.IsHandled)
			{
				if (HitTest(evt.Pos)) // it's hovering now
				{
					if (!IsHovering) // it wasn't hovering already
						OnEnter(evt);
					IsHovering = true;
					evt.Handle(this);
					QueuePaneRender();
				}
				else if (IsHovering) // it's not hovering now
				{
					OnLeave(evt);
					IsHovering = false;
					QueuePaneRender();
				}
			}
			else
				IsHovering = false;
			//QueuePaneRender(); // this makes it refresh all the time, but it doesn't seem right
			
		}
		
		/// <summary>
		/// Handles a mouse wheel event.
		/// </summary>
		public override void OnMouseWheel(MouseWheelEvent evt)
		{
			
		}

		/// <summary>
		/// This will get called whenever the mouse enters the region of the control.
		/// </summary>
		protected virtual void OnEnter(MouseEvent evt)
		{
			MakeDirty();
		}

		/// <summary>
		/// This will get called whenever the mouse leaves the region of the control.
		/// </summary>
		protected virtual void OnLeave(MouseEvent evt)
		{
			MakeDirty();
		}

		/// <summary>
		/// Handles a keyboard event.
		/// </summary>
		public override void OnKeyPress(KeyEvent evt)
		{
		
		}
		
		/// <summary>
		/// Raises the FocusEnter and FocusLeave events based on the hit state changing.
		/// </summary>
		private void OnHitStateChanged(object sender, HitStateChangedEvent evt)
		{
			var pane = Pane;
			if (!evt.OldValue.IsFocused() && IsFocused) // became focused
			{
				if (pane != null)
					pane.InFocus = this;
				OnFocusEnter();
			}
			else if (evt.OldValue.IsFocused() && !IsFocused) // lost focus
			{
				if (pane != null)
					pane.InFocus = null;
				OnFocusLeave();
			}
		}


		#endregion
		
		
		#region Focus
		
		/// <summary>
		/// Gets called when the control enters focus.
		/// </summary>
		protected virtual void OnFocusEnter()
		{
		}
		
		/// <summary>
		/// Gets called when the control leaves focus.
		/// </summary>
		protected virtual void OnFocusLeave()
		{
		}
		
		/// <summary>
		/// Gets the next control in the focus chain.
		/// </summary>
		public virtual Renderable2D GetNextFocus()
		{
			if (ParentControl == null)
				return this;
			else
				return ParentControl.GetNextFocus(this);
		}
		
		/// <summary>
		/// Gets the next control in the focus chain after the given child.
		/// </summary>
		public virtual Renderable2D GetNextFocus(Renderable2D child)
		{
			throw new Exception("This method has no meaning on controls which aren't containers.");
		}
		
		/// <summary>
		/// Gets the first control in this control's focus chain.
		/// </summary>
		public virtual Renderable2D GetFirstFocus()
		{
			return this;
		}
		
		/// <summary>
		/// Gets the last control in this control's focus chain.
		/// </summary>
		public virtual Renderable2D GetLastFocus()
		{
			return this;
		}
		
		/// <summary>
		/// Gets the previous control in the focus chain.
		/// </summary>
		public virtual Renderable2D GetPreviousFocus()
		{
			if (ParentControl == null)
				return this;
			else
				return ParentControl.GetPreviousFocus(this);
		}
		
		/// <summary>
		/// Gets the previous control in the focus chain from the given child.
		/// </summary>
		public virtual Renderable2D GetPreviousFocus(Renderable2D child)
		{
			throw new Exception("This method has no meaning on controls which aren't containers.");
		}
		
		#endregion
		
	}
}
