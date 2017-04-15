﻿/*
    Copyright (C) 2014-2017 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using dnSpy.Contracts.Debugger;
using dnSpy.Contracts.Debugger.Breakpoints.Code;

namespace dnSpy.Debugger.Breakpoints.Code.CondChecker {
	abstract class DbgCodeBreakpointTraceMessagePrinter {
		public abstract void Print(DbgBoundCodeBreakpoint boundBreakpoint, DbgThread thread, DbgCodeBreakpointTrace trace);
	}

	[Export(typeof(DbgCodeBreakpointTraceMessagePrinter))]
	sealed class DbgCodeBreakpointTraceMessagePrinterImpl : DbgCodeBreakpointTraceMessagePrinter {
		readonly Lazy<ITracepointMessageListener>[] tracepointMessageListeners;

		[ImportingConstructor]
		DbgCodeBreakpointTraceMessagePrinterImpl([ImportMany] IEnumerable<Lazy<ITracepointMessageListener>> tracepointMessageListeners) =>
			this.tracepointMessageListeners = tracepointMessageListeners.ToArray();

		public override void Print(DbgBoundCodeBreakpoint boundBreakpoint, DbgThread thread, DbgCodeBreakpointTrace trace) {
			if (tracepointMessageListeners.Length != 0) {
				var message = CreateTracepointMessage(boundBreakpoint, thread, trace);
				foreach (var lz in tracepointMessageListeners)
					lz.Value.Message(message);
			}
		}

		string CreateTracepointMessage(DbgBoundCodeBreakpoint boundBreakpoint, DbgThread thread, DbgCodeBreakpointTrace trace) {
			//TODO:
			return trace.Message;
		}
	}
}
