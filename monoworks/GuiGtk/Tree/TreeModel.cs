// TreeModel.cs - MonoWorks Project
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

using MonoWorks.Modeling;

namespace MonoWorks.GuiGtk.Tree
{
	
	
	public class TreeModel : Gtk.TreeStore, IEntityListener
	{	

		
		public TreeModel() : base(typeof(String), typeof(String))
		{
		}
		

		protected Drawing drawing;
		//// <value>
		/// The drawing.
		/// </value>
		public Drawing Drawing
		{
			get { return drawing; }
			set
			{
				this.drawing = value;
				drawing.EntityManager.RegisterEntityListener(this);
//				drawing.EntityManager.RegisterSelectionListener(this);
				GenerateItems();
			}
		}
		
		/// <summary>
		/// Regenerates the entire model.
		/// </summary>
		public void GenerateItems()
		{
			Clear();
			
			foreach (Entity entity in drawing.Children)
				AddEntity(entity);
		}
		
		/// <summary>
		/// Adds an entity to the tree.
		/// </summary>
		public void AddEntity(Entity entity)
		{
			AddEntity(entity, null);
		}

		/// <summary>
		/// Adds an entity to the tree with the given parent.
		/// </summary>
		protected void AddEntity(Entity entity, Gtk.TreeIter? parentIter)
		{
//			EntityTreeItem item = new EntityTreeItem(entity);
			Gtk.TreeIter iter;
			if (parentIter == null)
				iter = AppendValues(entity.ClassName.ToLower(), entity.Name);
			else
				iter = AppendValues((Gtk.TreeIter)parentIter, entity.ClassName.ToLower(), entity.Name);
//			items[entity] = item;

//			item.Selected += OnItemSelected;
//			item.Unselected += OnItemDeselected;

			// add the children
			foreach (Entity child in entity.Children)
				AddEntity(child, iter);
		}
		
		public void RemoveEntity(Entity entity)
		{
			throw new System.NotImplementedException();
		}

		
		
	}
}
