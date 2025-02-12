using ARMeilleure.IntermediateRepresentation;
using System;

namespace ARMeilleure.CodeGen.LoongArch64
{
    enum LoongArch64Condition
    {
        Eq = 0,
        Ne = 1,
        GeUn = 2,
        LtUn = 3,
        Mi = 4,
        Pl = 5,
        Vs = 6,
        Vc = 7,
        GtUn = 8,
        LeUn = 9,
        Ge = 10,
        Lt = 11,
        Gt = 12,
        Le = 13,
        Al = 14,
        Nv = 15,
    }

    static class ComparisonLoongArch64Extensions
    {
        public static LoongArch64Condition ToLoongArch64Condition(this Comparison comp)
        {
            return comp switch
            {
#pragma warning disable IDE0055 // Disable formatting
                Comparison.Equal            => LoongArch64Condition.Eq,
                Comparison.NotEqual         => LoongArch64Condition.Ne,
                Comparison.Greater          => LoongArch64Condition.Gt,
                Comparison.LessOrEqual      => LoongArch64Condition.Le,
                Comparison.GreaterUI        => LoongArch64Condition.GtUn,
                Comparison.LessOrEqualUI    => LoongArch64Condition.LeUn,
                Comparison.GreaterOrEqual   => LoongArch64Condition.Ge,
                Comparison.Less             => LoongArch64Condition.Lt,
                Comparison.GreaterOrEqualUI => LoongArch64Condition.GeUn,
                Comparison.LessUI           => LoongArch64Condition.LtUn,
#pragma warning restore IDE0055

                _ => throw new ArgumentException(null, nameof(comp)),
            };
        }
    }
}
