using ARMeilleure.IntermediateRepresentation;
using System;
using System.Diagnostics;

namespace ARMeilleure.CodeGen.LoongArch64
{
    static class CodeGeneratorIntrinsic
    {
        public static void GenerateOperation(CodeGenContext context, Operation operation)
        {
            Intrinsic intrin = operation.Intrinsic;

            IntrinsicInfo info = IntrinsicTable.GetInfo(intrin);

            switch (info.Type)
            {
                default:
                    throw new NotImplementedException(info.Type.ToString());
            }
        }
    }
}
