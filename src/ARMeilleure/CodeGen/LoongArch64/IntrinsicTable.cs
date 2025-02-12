using ARMeilleure.Common;
using ARMeilleure.IntermediateRepresentation;

namespace ARMeilleure.CodeGen.LoongArch64
{
    static class IntrinsicTable
    {
        private static readonly IntrinsicInfo[] _intrinTable;

        static IntrinsicTable()
        {
            _intrinTable = new IntrinsicInfo[EnumUtils.GetCount(typeof(Intrinsic))];

#pragma warning disable IDE0055 // Disable formatting
#pragma warning restore IDE0055
        }

        private static void Add(Intrinsic intrin, IntrinsicInfo info)
        {
            _intrinTable[(int)intrin] = info;
        }

        public static IntrinsicInfo GetInfo(Intrinsic intrin)
        {
            return _intrinTable[(int)intrin];
        }
    }
}
