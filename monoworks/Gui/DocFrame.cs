// DocFrame.cs - MonoWorks Project
//
// Copyright (C) 2008 Andy Selvig
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;

using Qyoto;

using MonoWorks.Model;

namespace MonoWorks.Gui
{
	
	/// <summary>
	/// The document frame is used to hold a single document.
	/// </summary>
	public class DocFrame : QSplitter
	{
		protected QVBoxLayout vbox;
		protected ViewportToolbar viewportToolbar;
		protected TreeModel treeModel;
			
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DocFrame(QWidget parent) : base(Qt.Orientation.Horizontal, parent)
		{
			// initialize resources
			ResourceManager.Initialize();
									
			// create the default document
			document = new TestDocument();
						
			// create the tree model and view
			treeModel = new TreeModel(document);
			treeView = new TreeView(this);
			treeView.SetModel(treeModel);
			
			// create the attribute frame
			attributeFrame = new Attributes.Frame(this);
			attributeFrame.SetVisible(false);
			Connect(treeView, SIGNAL("EntityAttributes()"), this, SLOT("OnEntityAttributes()"));
			
			// create the viewport frame
			QFrame frame = new QFrame(this);
			frame.FrameShape = Shape.Box;
			frame.FrameShadow = Shadow.Sunken;
			vbox = new QVBoxLayout(frame);
			vbox.Margin = 0;
			vbox.Spacing = 0;
			frame.SetLayout(vbox);		
						
			// add the viewport and toolbar
			viewport = new Viewport(document);
			viewportToolbar = new ViewportToolbar(frame, viewport);
			vbox.AddWidget(viewportToolbar);
			vbox.AddWidget(viewport);
			
			
			// connect the tree view with the viewport
			Connect(treeView, SIGNAL("PaintViewport()"), viewport, SLOT("Paint()"));
			Connect(viewport, SIGNAL("SelectionChanged()"), treeView, SLOT("OnExternalSelectionChanged()"));
						
		}
				
		
		/// <summary>
		/// Constructs the frame without a parent.
		/// </summary>
		public DocFrame() : this(null)
		{
		}
		
		protected Document document;
		/// <value>
		/// The document associated with this frame.
		/// </value>
		public Document Document
		{
			get {return document;}
			set
			{
				document = value;
				viewport.Document = document;
				treeModel.Document = document;
			}
		}

		protected Viewport viewport;
		/// <value>
		/// The viewport.
		/// </value>
		public Viewport Viewport
		{
			get {return viewport;}
		}
		
		
		protected TreeView treeView;
		/// <value>
		/// The tree view widget.
		/// </value>
		public TreeView Treeview
		{
			get {return treeView;}
			set {treeView = value;}
		}
		
		
#region Entity Attributes
		
		/// <summary>
		/// The entity attributes frame.
		/// </summary>
		protected Attributes.Frame attributeFrame;
		
		/// <summary>
		/// Shows the attributes frame for the current entity.
		/// </summary>
		[Q_SLOT]
		public void OnEntityAttributes()
		{
			Entity entity = document.LastSelected;
			if (entity != null)
			{
//				attributeFrame = new AttributeFrame();
//				this.InsertWidget(1, attributeFrame);
				attributeFrame.Entity = entity;
				attributeFrame.SetVisible(true);
			}
		}
		
		/// <summary>
		/// Cancels the entity action editing.
		/// </summary>
		[Q_SLOT]
		public void EntityAttributesCancel()
		{
			Console.WriteLine("entity attributes cancel");
			attributeFrame.SetVisible(false);
		}
		
		/// <summary>
		/// Applies the entity action editing.
		/// </summary>
		[Q_SLOT]
		public void EntityAttributesApply()
		{
			Console.WriteLine("entity attributes apply");
			attributeFrame.SetVisible(false);
		}
		
#endregion
		
	}
}