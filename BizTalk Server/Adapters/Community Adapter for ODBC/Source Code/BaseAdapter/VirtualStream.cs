//---------------------------------------------------------------------
// File: VirtualStream.cs
// 
// Summary: Implementation of an adapter framework sample adapter. 
// This class constitutes one of the BaseAdapter classes, which, are
// a set of generic re-usable set of classes to help adapter writers.
//
// Sample: Base Adapter Class Library v1.0.1
//
//---------------------------------------------------------------------
// This file is part of the Microsoft BizTalk Server 2004 SDK
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft BizTalk
// Server 2004 release and/or on-line documentation. See these other
// materials for detailed information regarding Microsoft code samples.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
// PURPOSE.
//---------------------------------------------------------------------

using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;


namespace Microsoft.Samples.BizTalk.Adapters.BaseAdapter
{
	/// <summary>
	/// Summary description for VirtualStream.
	/// </summary>
	public class VirtualStream : Stream, IDisposable
	{
		private Stream					stream;
		private bool					isDisposed		= false;
		private bool					isInMemory		= true;
		private int						threshholdSize;
		private const int				THRESHOLD_MAX	= 1*1024*1024;	// 1MB max size
		private const int				defaultSize		= 10240;		// 10K default size

		public VirtualStream() : this(defaultSize)
		{
		}

		public VirtualStream(int buffer)
		{
			this.stream = new MemoryStream(buffer);
			this.threshholdSize = THRESHOLD_MAX;
		}

		// Stream Properties...
		#region Stream Properties 

		override public bool CanRead
		{
			get {return this.stream.CanRead;}
		}

		override public bool CanWrite
		{
			get {return this.stream.CanWrite;}
		}

		override public bool CanSeek
		{
			get {return this.stream.CanSeek;}
		}

		override public long Length
		{
			get {return this.stream.Length;}
		}

		override public long Position
		{
			get {return this.stream.Position;}
			set {this.stream.Seek(value, SeekOrigin.Begin);}
		}

		#endregion // Stream Properties 

		// Stream Methods...
		#region Stream Methods 

		override public void Close()
		{
			if(!this.isDisposed)
			{
				GC.SuppressFinalize(this);
				Cleanup();
			}
		}

		override public void Flush()
		{
			ThrowIfDisposed();
			this.stream.Flush();
		}

		override public int Read(byte[] buffer, int offset, int count)
		{
			ThrowIfDisposed();
			
			return this.stream.Read(buffer, offset, count);
		}
	
		override public long Seek(long offset, SeekOrigin origin)
		{
			ThrowIfDisposed();

			return this.stream.Seek(offset, origin);
		}

		override public void SetLength(long length)
		{
			ThrowIfDisposed();

			if ( (length > this.threshholdSize) && this.isInMemory )
			{
				lock (this)
				{
					// The stream is currently in memory, the new write will tip 
					// it over the limit. Switching to persisted stream...
					if ( (length > this.threshholdSize) && this.isInMemory )
					{
						// Create the persisted stream...
						Stream persistedStream = CreatePersistentStream();

						// Copy the contents of the memory stream into the persisted
						CopyStreamContent((MemoryStream)this.stream, persistedStream);

						// Switch over the streams...
						this.stream = persistedStream;
						this.isInMemory = false;
					}

					this.stream.SetLength(length);
				}
			}
			else
				this.stream.SetLength(length);
		}

		override public void Write(byte[] buffer, int offset, int count)
		{			
			ThrowIfDisposed();

			if ( ((count + this.stream.Position) > this.threshholdSize) && this.isInMemory )
			{
				lock (this)
				{
					// The stream is currently in memory, the new write will tip 
					// it over the limit. Switching to persisted stream...
					if ( ((count + this.stream.Position) > this.threshholdSize) && this.isInMemory )
					{
						// Create the persisted stream...
						Stream persistedStream = CreatePersistentStream();

						// Copy the contents of the memory stream into the persisted
						CopyStreamContent((MemoryStream)this.stream, persistedStream);

						// Switch over the streams...
						this.stream = persistedStream;
						this.isInMemory = false;
					}

					this.stream.Write (buffer, offset, count);
				}
			}
			else
				this.stream.Write (buffer, offset, count);
		}

		#endregion // Stream Methods

		#region IDisposable Interface

		public void Dispose()
		{
			Close();
		}

		#endregion // IDisposable Interface

		private void ThrowIfDisposed()
		{
			if(this.isDisposed || (this.stream == null))
				throw new ObjectDisposedException("VirtualStream");
		}

		private void Cleanup()
		{
			if(!this.isDisposed)
			{
				this.isDisposed = true;
				if(null != this.stream)
				{
					this.stream.Close();
					this.stream = null;
				}
			}
		}

		private void CopyStreamContent(MemoryStream source, Stream dest)
		{
			long currentPosition = source.Position;

			// Optimization
			if (source.Length < int.MaxValue)
			{
				dest.Write(source.GetBuffer(), 0, (int) source.Length);
			}
			else
			{
				source.Position = 0;
				byte[] tempBuffer = new Byte[this.threshholdSize];
				int read = 0;
			
				while ((read = source.Read(tempBuffer, 0, this.threshholdSize)) != 0)
					dest.Write(tempBuffer, 0, read);
			}

			dest.Position = currentPosition;
		}

		public static Stream CreatePersistentStream()
		{
			StringBuilder name = new StringBuilder(256);
			IntPtr	handle;
			if(0 == NativeCalls.GetTempFileName(Path.GetTempPath(), "VSTMP", 0, name))
				throw new ApplicationException("GetTempFileName Failed.");
			handle = NativeCalls.CreateFile(name.ToString(), (UInt32)FileAccess.ReadWrite, 0, IntPtr.Zero, (UInt32)FileMode.Create, 0x04000100, IntPtr.Zero);

			if(IntPtr.Zero == handle)
				throw new ApplicationException("CreateFile Failed.");
			return new FileStream(handle, FileAccess.ReadWrite);
		}
	}
}
