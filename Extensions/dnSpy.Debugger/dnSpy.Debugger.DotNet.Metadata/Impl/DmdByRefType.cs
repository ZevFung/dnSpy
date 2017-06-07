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
using System.Runtime.InteropServices;

namespace dnSpy.Debugger.DotNet.Metadata.Impl {
	sealed class DmdByRefType : DmdTypeBase {
		public override DmdTypeSignatureKind TypeSignatureKind => DmdTypeSignatureKind.ByRef;
		public override DmdTypeScope TypeScope => SkipElementTypes().TypeScope;
		public override DmdModule Module => SkipElementTypes().Module;
		public override string Namespace => SkipElementTypes().Namespace;
		public override DmdType BaseType => null;
		public override StructLayoutAttribute StructLayoutAttribute => null;
		public override DmdTypeAttributes Attributes => DmdTypeAttributes.NotPublic | DmdTypeAttributes.AutoLayout | DmdTypeAttributes.Class | DmdTypeAttributes.AnsiClass;
		public override string Name => DmdMemberFormatter.FormatName(this);
		public override DmdType DeclaringType => null;
		public override int MetadataToken => 0x02000000;
		public override bool IsMetadataReference => false;

		readonly DmdTypeBase elementType;

		public DmdByRefType(DmdTypeBase elementType) {
			this.elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
			IsFullyResolved = elementType.IsFullyResolved;
		}

		public override DmdType GetElementType() => elementType;

		protected override DmdType ResolveNoThrowCore() => this;
		public override bool IsFullyResolved { get; }
		public override DmdTypeBase FullResolve() {
			if (IsFullyResolved)
				return this;
			var et = elementType.FullResolve();
			if ((object)et != null)
				return (DmdTypeBase)AppDomain.MakeByRefType(et);
			return null;
		}
	}
}