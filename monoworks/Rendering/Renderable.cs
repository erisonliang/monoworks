// IRenderable.cs - MonoWorks Project
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

namespace MonoWorks.Rendering
{
	
	/// <summary>
	/// Base class for renderable objects.
	/// </summary>
	public abstract class Renderable
	{	
		
		/// <summary>
		/// True if the renderable is dirty and needs its geometry recomputed.
		/// </summary>
		protected bool dirty = true;
		
		/// <summary>
		/// Makes the renderable dirty.
		/// </summary>
		public virtual void MakeDirty()
		{
			dirty = true;
		}

		protected Bounds bounds;
		/// <summary>
		/// The bounding box of the renderable.
		/// Should be updated by ComputeGeometry().
		/// </summary>
		public Bounds Bounds
		{
			get {return bounds;}
		}

		
		/// <summary>
		/// Forces the renderable to compute its geometry.
		/// </summary>
		public virtual void ComputeGeometry()
		{
			dirty = false;
		}
		
		/// <summary>
		/// Renders the opaque portion of the renderable.
		/// </summary>
		/// <param name="viewport"> A <see cref="IViewport"/> to render to. </param>
		public virtual void RenderOpaque(IViewport viewport)
		{			
			if (dirty)
				ComputeGeometry();
		}
		
		/// <summary>
		/// Renders the transparent portion of the renderable, 
		/// </summary>
		/// <param name="viewport"> A <see cref="IViewport"/> to render to. </param>
		public virtual void RenderTransparent(IViewport viewport)
		{			
			if (dirty)
				ComputeGeometry();
		}
		
		
		/// <summary>
		/// Renders the overlay portion of the renderable, 
		/// </summary>
		/// <param name="viewport"> A <see cref="IViewport"/> to render to. </param>
		public virtual void RenderOverlay(IViewport viewport)
		{			
			if (dirty)
				ComputeGeometry();
		}
		
		
		
	}
	
}