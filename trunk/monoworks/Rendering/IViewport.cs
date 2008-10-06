// IViewport.cs - MonoWorks Project
//
//    Copyright Andy Selvig 2008
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published 
//    by the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public 
//    License along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace MonoWorks.Rendering
{
	/// <summary>
	/// How the viewport is being used.
	/// </summary>
	public enum ViewportUsage {CAD, Plotting};
	
	/// <summary>
	/// The viewing mode of the viewport.
	/// </summary>
	public enum ViewMode {TwoD, ThreeD};
	
	/// <summary>
	/// The IViewport interface defines an interface for MonoWorks viewports. 
	/// This lets the model interact with viewports in a GUI-independant manner.
	/// </summary>
	public interface IViewport
	{
		/// <summary>
		/// Returns the viewport width.
		/// </summary>
		int WidthGL
		{
			get;
		}
		
		/// <summary>
		/// Returns the viewport height.
		/// </summary>
		int HeightGL
		{
			get;
		}
		
		/// <value>
		/// Access the viewport camera.
		/// </value>
		Camera Camera
		{
			get;
		}
		
		/// <summary>
		/// Initializes the rendering.
		/// </summary>
		void InitializeGL();
		
		/// <summary>
		/// Performs the rendering for one frame.
		/// </summary>
		void PaintGL();

		/// <value>
		/// Access the viewport's render manager.
		/// </value>
		RenderManager RenderManager
		{
			get;
		}
		
		/// <value>
		/// The interaction state.
		/// </value>
		InteractionStateBase InteractionState
		{
			get;
		}
		
		/// <value>
		/// The lighting effects for the viewport.
		/// </value>
		Lighting Lighting
		{
			get;
		}

		/// <summary>
		/// Adds a renderable to the rendering list.
		/// </summary>
		/// <param name="renderable"> A <see cref="Renderable"/>. </param>
		void AddRenderable(Renderable renderable);

		/// <summary>
		/// Removes a renderable from the rendering list.
		/// </summary>
		/// <param name="renderable"> A <see cref="Renderable"/>. </param>
		void RemoveRenderable(Renderable renderable);

		/// <value>
		/// The bounds of all renderables.
		/// </value>
		Bounds Bounds {get;}
		
		/// <summary>
		/// Lets the renderables know that the viewport has been resized.
		/// </summary>
		void OnResized();
		
		/// <summary>
		/// Lets the renderables know that the view direction has been changed.
		/// </summary>
		void OnDirectionChanged();
	}
}