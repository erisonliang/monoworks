// ToolBar.cs - MonoWorks Project
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
using System.Collections;
using System.Collections.Generic;

using Cairo;

using MonoWorks.Base;
using MonoWorks.Rendering;

namespace MonoWorks.Rendering.Controls
{
	
	/// <summary>
	/// A stack of buttons.
	/// </summary>
	public class ToolBar : Stack, IEnumerable<Button>
	{
		
		public ToolBar() : base()
		{
			StyleClassName = "toolbar";
			ToolStyle = "tool";
		}
		
		
		public override void Add(Control2D child)
		{
			base.Add(child);
			
			child.StyleClassName = toolStyle;
			
			if (child is Button)
				(child as Button).ButtonStyle = buttonStyle;
		}

		
		private string toolStyle = "tool";
		/// <value>
		/// The style class to use for the child controls.
		/// </value>
		public string ToolStyle
		{
			get {return toolStyle;}
			set
			{
				toolStyle = value;
				foreach (Control2D child in Children)
					child.StyleClassName = value;
			}
		}


		protected override void Render(Context cr)
		{	
			base.Render(cr);
						
		}
		
		protected ButtonStyle buttonStyle = ButtonStyle.ImageOverLabel;
		/// <value>
		/// Set the button style of all buttons in the toolbar.
		/// </value>
		public ButtonStyle ButtonStyle
		{
			set
			{
				buttonStyle = value;
				foreach (Control2D child in Children)
				{
					if (child is Button)
						(child as Button).ButtonStyle = buttonStyle;
				}
			}
			get {return buttonStyle;}
		}

		
#region Button Enumeration
		
		
		public IEnumerator<Button> GetEnumerator()
		{
			foreach(Control2D child in children)
			{
				if (child is Button)
					yield return (child as Button);
        	}
		}

		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
				
		/// <summary>
		/// Get a child button by it label.
		/// </summary>
		/// <returns> The button, or null if there isn't one present. </returns>
		public Button GetButton(string label)
		{
			foreach(Control2D child in children)
			{
				if (child is Button && (child as Button).LabelString == label)
					return (child as Button);
        	}
			return null;
		}
		
#endregion


#region Mouse Interaction


		public override void OnMouseMotion(MonoWorks.Rendering.Events.MouseEvent evt)
		{
			base.OnMouseMotion(evt);

			// catch hover even if the buttons didn't
			if (HitTest(evt.Pos))
				evt.Handle();
		}


#endregion


	}
}
