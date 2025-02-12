using System;

namespace ARMeilleure.CodeGen.LoongArch64
{
    static class CallingConvention
    {
        private const int RegistersMask = unchecked((int)0xffffffff);

        // Some of those register have specific roles and can't be used as general purpose registers.
        // $r0 - Constant zero
        // $r1 - Return address
        // $r2 - Thread pointer
        // $r3 - 	Stack pointer
        // $r21 - Reserved
        // $r22 - Frame pointer
        private const int ReservedRegsMask = (1 << CodeGenCommon.ReservedRegister) | (1 << 0) | (1 << 1) | (1 << 2) | (1 << 3) | (1 << 21) | (1 << 22);

        public static int GetIntAvailableRegisters()
        {
            return RegistersMask & ~ReservedRegsMask;
        }

        public static int GetVecAvailableRegisters()
        {
            return RegistersMask;
        }

        public static int GetIntCallerSavedRegisters()
        {
            return (GetIntCalleeSavedRegisters() ^ RegistersMask) & ~ReservedRegsMask;
        }

        public static int GetFpCallerSavedRegisters()
        {
            return GetFpCalleeSavedRegisters() ^ RegistersMask;
        }

        public static int GetVecCallerSavedRegisters()
        {
            return GetVecCalleeSavedRegisters() ^ RegistersMask;
        }

        public static int GetIntCalleeSavedRegisters()
        {
            return unchecked((int)0xff800000); // $r23 to $r31
        }

        public static int GetFpCalleeSavedRegisters()
        {
            return unchecked((int)0xff000000); // $f24 to $f31
        }

        public static int GetVecCalleeSavedRegisters()
        {
            return 0;
        }

        public static int GetArgumentsOnRegsCount()
        {
            return 8;
        }

        public static int GetIntArgumentRegister(int index)
        {
            if ((uint)index >= 4 && (uint)index < (uint)GetArgumentsOnRegsCount() + 4)
            {
                return index;
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        public static int GetVecArgumentRegister(int index)
        {
            if ((uint)index < (uint)GetArgumentsOnRegsCount())
            {
                return index;
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        public static int GetIntReturnRegister()
        {
            return 4;
        }

        public static int GetIntReturnRegisterHigh()
        {
            return 5;
        }

        public static int GetVecReturnRegister()
        {
            return 0;
        }
    }
}
