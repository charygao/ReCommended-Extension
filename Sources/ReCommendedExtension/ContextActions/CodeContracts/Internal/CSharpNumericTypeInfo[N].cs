using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;

namespace ReCommendedExtension.ContextActions.CodeContracts.Internal
{
    internal sealed class CSharpNumericTypeInfo<N> : CSharpNumericTypeInfo where N : struct
    {
        readonly N one;

        [NotNull]
        readonly Func<N, N, bool> isLessOrEquals;

        [NotNull]
        readonly Func<N, bool> isZero;

        readonly Func<N, N> getNext;

        [NotNull]
        readonly Func<N, N> getMultipliedWithTwo;

        [NotNull]
        readonly Func<N, double> convertToDouble;

        internal CSharpNumericTypeInfo(
            bool isSigned,
            N one,
            [NotNull] string literalSuffix,
            string epsilonLiteral,
            [NotNull] Func<N, N, bool> isLessOrEquals,
            [NotNull] Func<N, bool> isZero,
            Func<N, N> getNext,
            [NotNull] Func<N, N> getMultipliedWithTwo,
            [NotNull] Func<N, double> convertToDouble) : base(isSigned, epsilonLiteral, literalSuffix)
        {
            this.one = one;
            this.isLessOrEquals = isLessOrEquals;
            this.isZero = isZero;
            this.getNext = getNext;
            this.getMultipliedWithTwo = getMultipliedWithTwo;
            this.convertToDouble = convertToDouble;
        }

        public override EnumBetweenFirstAndLast.EnumContractInfo TryCreateEnumContractInfoForEnumBetweenFirstAndLast(IList<IField> members)
        {
            Debug.Assert(getNext != null);

            return EnumBetweenFirstAndLast.EnumContractInfo<N>.TryCreate(members, isLessOrEquals, getNext);
        }

        public override EnumFlags.EnumContractInfo TryCreateEnumFlags(IList<IField> members)
            => EnumFlags.EnumContractInfo<N>.TryCreate(members, one, convertToDouble, isZero, isLessOrEquals, getMultipliedWithTwo, LiteralSuffix);
    }
}